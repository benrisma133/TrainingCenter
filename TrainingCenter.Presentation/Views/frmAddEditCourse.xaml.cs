using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using TrainingCenter.Service.Implementations;

namespace TrainingCenter.Presentation.Views
{
    public partial class frmAddEditCourse : Window
    {
        private CourseService _service;
        public bool IsSaved { get; private set; } = false;

        public frmAddEditCourse()
        {
            InitializeComponent();
        }

        public frmAddEditCourse(int courseId)
        {
            InitializeComponent();
            _service = CourseService.Find(courseId);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LevelCombo.SelectedIndex = 0;
            StatusCombo.SelectedIndex = 0;
            LoadInstructors();

            if (_service != null)
            {
                FormTitle.Text = "Edit Course";
                FormSubtitle.Text = "Update the course details below";
                SaveBtn.Content = "Update Course";

                TxtTitle.Text = _service.Title;
                TxtCode.Text = _service.Code;
                TxtDescription.Text = _service.Description ?? "";
                TxtPrice.Text = _service.Price.ToString();
                TxtDuration.Text = _service.DurationHours.ToString();

                if (_service.PublishedAt.HasValue)
                    PublishedPicker.SelectedDate = _service.PublishedAt.Value;

                SetCombo(LevelCombo, _service.Level);
                SetCombo(StatusCombo, _service.Status);
                SetInstructorCombo(_service.InstructorId);
            }
        }

        private void LoadInstructors()
        {
            var instructors = InstructorService.GetAll();
            InstructorCombo.Items.Clear();
            foreach (var i in instructors)
            {
                InstructorCombo.Items.Add(new ComboBoxItem
                {
                    Content = $"{i.FirstName} {i.LastName}",
                    Tag = i.InstructorId
                });
            }
            if (InstructorCombo.Items.Count > 0)
                InstructorCombo.SelectedIndex = 0;
        }

        private void SetCombo(ComboBox combo, string value)
        {
            foreach (ComboBoxItem item in combo.Items)
                if (item.Tag.ToString() == value)
                { combo.SelectedItem = item; break; }
        }

        private void SetInstructorCombo(int instructorId)
        {
            foreach (ComboBoxItem item in InstructorCombo.Items)
                if ((int)item.Tag == instructorId)
                { InstructorCombo.SelectedItem = item; break; }
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e) => Close();
        private void CancelBtn_Click(object sender, RoutedEventArgs e) => Close();

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            HideMessages();
            var errors = Validate();
            if (errors.Any()) { ShowError(errors); return; }

            if (_service == null)
                _service = new CourseService();

            _service.Title = TxtTitle.Text.Trim();
            _service.Code = TxtCode.Text.Trim().ToUpper();
            _service.Description = string.IsNullOrWhiteSpace(TxtDescription.Text)
                                     ? null : TxtDescription.Text.Trim();
            _service.Price = decimal.Parse(TxtPrice.Text.Trim());
            _service.DurationHours = int.Parse(TxtDuration.Text.Trim());
            _service.Level = GetComboTag(LevelCombo);
            _service.Status = GetComboTag(StatusCombo);
            _service.InstructorId = (int)(InstructorCombo.SelectedItem as ComboBoxItem)!.Tag;
            _service.PublishedAt = PublishedPicker.SelectedDate;

            bool success = _service.Save();

            if (success)
            {
                IsSaved = true;
                ShowSuccess();
            }
            else
            {
                ShowError(new List<string> { "• Failed to save. Please try again." });
            }
        }

        private List<string> Validate()
        {
            var errors = new List<string>();
            int excludeId = _service?.CourseId ?? 0;

            if (string.IsNullOrWhiteSpace(TxtTitle.Text))
                errors.Add("• Course title is required.");

            if (string.IsNullOrWhiteSpace(TxtCode.Text))
                errors.Add("• Course code is required.");
            else if (CourseService.IsExistByCode(TxtCode.Text.Trim(), excludeId))
                errors.Add("• This course code is already used.");

            if (string.IsNullOrWhiteSpace(TxtPrice.Text) ||
                !decimal.TryParse(TxtPrice.Text, out decimal price) || price < 0)
                errors.Add("• Please enter a valid price.");

            if (string.IsNullOrWhiteSpace(TxtDuration.Text) ||
                !int.TryParse(TxtDuration.Text, out int dur) || dur <= 0)
                errors.Add("• Please enter a valid duration in hours.");

            if (InstructorCombo.SelectedItem == null)
                errors.Add("• Please select an instructor.");

            return errors;
        }

        private string GetComboTag(ComboBox combo)
            => (combo.SelectedItem as ComboBoxItem)?.Tag.ToString() ?? "";

        private void ShowError(List<string> errors)
        {
            ErrorText.Text = string.Join("\n", errors);
            ErrorBox.Visibility = Visibility.Visible;
            ErrorBox.BeginAnimation(OpacityProperty,
                new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(250)));
        }

        private void ShowSuccess()
        {
            SuccessBox.Visibility = Visibility.Visible;
            SuccessBox.BeginAnimation(OpacityProperty,
                new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(250)));
            var timer = new System.Windows.Threading.DispatcherTimer
            { Interval = TimeSpan.FromSeconds(2) };
            timer.Tick += (s, e) => { timer.Stop(); Close(); };
            timer.Start();
        }

        private void HideMessages()
        {
            ErrorBox.Visibility = Visibility.Collapsed;
            SuccessBox.Visibility = Visibility.Collapsed;
        }
    }
}