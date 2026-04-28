using System;
using System.Windows;
using TrainingCenter.Service.Implementations;

namespace TrainingCenter.Presentation.Views
{
    public partial class frmAddEditStudent : Window
    {
        private StudentService _service;
        public bool IsSaved { get; private set; } = false;

        // Add mode
        public frmAddEditStudent()
        {
            InitializeComponent();
        }

        // Edit mode
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
                // Edit mode - fill fields
                FormTitle.Text = "Edit Student";
                FormSubtitle.Text = "Update the student details below";
                SaveBtn.Content = "Update Student";

                TxtFirstName.Text = _service.FirstName;
                TxtLastName.Text = _service.LastName;
                TxtEmail.Text = _service.Email;
                TxtPhone.Text = _service.PhoneNumber ?? "";
                DobPicker.SelectedDate = _service.DateOfBirth.ToDateTime(TimeOnly.MinValue);

                // Set status combo
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

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(TxtFirstName.Text))
            { MessageBox.Show("First name is required.", "Validation"); return; }

            if (string.IsNullOrWhiteSpace(TxtLastName.Text))
            { MessageBox.Show("Last name is required.", "Validation"); return; }

            if (string.IsNullOrWhiteSpace(TxtEmail.Text))
            { MessageBox.Show("Email is required.", "Validation"); return; }

            if (DobPicker.SelectedDate == null)
            { MessageBox.Show("Date of birth is required.", "Validation"); return; }

            var selectedStatus = (StatusCombo.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Tag.ToString() ?? "Active";

            if (_service == null)
            {
                // Add mode - create new service instance
                _service = new StudentService();
            }

            _service.FirstName = TxtFirstName.Text.Trim();
            _service.LastName = TxtLastName.Text.Trim();
            _service.Email = TxtEmail.Text.Trim();
            _service.PhoneNumber = string.IsNullOrWhiteSpace(TxtPhone.Text) ? null : TxtPhone.Text.Trim();
            _service.DateOfBirth = DateOnly.FromDateTime(DobPicker.SelectedDate.Value);
            _service.Status = selectedStatus;

            bool success = _service.Save();

            if (success)
            {
                IsSaved = true;
                Close();
            }
            else
            {
                MessageBox.Show("Failed to save student. Please try again.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
            => Close();
    }
}