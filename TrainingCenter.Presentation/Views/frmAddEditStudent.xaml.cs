using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using TrainingCenter.Service.Implementations;

namespace TrainingCenter.Presentation.Views
{
    public partial class frmAddEditStudent : Window
    {
        private StudentService _service;
        public bool IsSaved { get; private set; } = false;

        public frmAddEditStudent()
        {
            InitializeComponent();
        }

        public frmAddEditStudent(int studentId)
        {
            InitializeComponent();
            _service = StudentService.Find(studentId);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StatusCombo.SelectedIndex = 0;

            if (_service != null)
            {
                FormTitle.Text = "Edit Student";
                FormSubtitle.Text = "Update the student details below";
                SaveBtn.Content = "Update Student";

                TxtFirstName.Text = _service.FirstName;
                TxtLastName.Text = _service.LastName;
                TxtEmail.Text = _service.Email;
                TxtPhone.Text = _service.PhoneNumber ?? "";
                DobPicker.SelectedDate = _service.DateOfBirth.ToDateTime(TimeOnly.MinValue);

                foreach (System.Windows.Controls.ComboBoxItem item in StatusCombo.Items)
                {
                    if (item.Tag.ToString() == _service.Status)
                    {
                        StatusCombo.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e) => Close();
        private void CancelBtn_Click(object sender, RoutedEventArgs e) => Close();

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            HideMessages();

            var errors = Validate();
            if (errors.Any())
            {
                ShowError(errors);
                return;
            }

            if (_service == null)
                _service = new StudentService();

            _service.FirstName = TxtFirstName.Text.Trim();
            _service.LastName = TxtLastName.Text.Trim();
            _service.Email = TxtEmail.Text.Trim();
            _service.PhoneNumber = string.IsNullOrWhiteSpace(TxtPhone.Text)
                                   ? null : TxtPhone.Text.Trim();
            _service.DateOfBirth = DateOnly.FromDateTime(DobPicker.SelectedDate!.Value);
            _service.Status = (StatusCombo.SelectedItem as System.Windows.Controls.ComboBoxItem)
                                   ?.Tag.ToString() ?? "Active";

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
            int excludeId = _service?.StudentId ?? 0;

            if (string.IsNullOrWhiteSpace(TxtFirstName.Text))
                errors.Add("• First name is required.");

            if (string.IsNullOrWhiteSpace(TxtLastName.Text))
                errors.Add("• Last name is required.");

            if (string.IsNullOrWhiteSpace(TxtEmail.Text))
                errors.Add("• Email is required.");
            else if (StudentService.IsExistByEmail(TxtEmail.Text.Trim(), excludeId))
                errors.Add("• This email is already used by another student.");

            if (!string.IsNullOrWhiteSpace(TxtPhone.Text) &&
                StudentService.IsExistByPhone(TxtPhone.Text.Trim(), excludeId))
                errors.Add("• This phone number is already used by another student.");

            if (DobPicker.SelectedDate == null)
                errors.Add("• Date of birth is required.");

            return errors;
        }

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
            {
                Interval = TimeSpan.FromSeconds(2)
            };
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