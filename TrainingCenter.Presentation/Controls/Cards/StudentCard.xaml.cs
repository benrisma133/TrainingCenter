using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TrainingCenter.Repository.Entities;

namespace TrainingCenter.Presentation.Controls.Cards
{
    public partial class StudentCard : UserControl
    {
        public event EventHandler<int> OnEdit;
        public event EventHandler<int> OnDelete;

        private int _studentId;

        public StudentCard()
        {
            InitializeComponent();
        }

        public void LoadStudent(Student student)
        {
            _studentId = student.StudentId;

            // Avatar - first letter of first name
            AvatarText.Text = student.FirstName.Substring(0, 1).ToUpper();

            // Name & Email
            FullNameText.Text = $"{student.FirstName} {student.LastName}";
            EmailText.Text = student.Email;

            // Phone
            PhoneText.Text = string.IsNullOrEmpty(student.PhoneNumber)
                ? "N/A" : student.PhoneNumber;

            // Age
            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - student.DateOfBirth.Year;
            if (student.DateOfBirth > today.AddYears(-age)) age--;
            AgeText.Text = $"{age} years";

            // Status badge
            SetStatus(student.Status);
        }

        private void SetStatus(string status)
        {
            StatusText.Text = status;

            switch (status?.ToLower())
            {
                case "active":
                    StatusBadge.Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#064E3B"));
                    StatusText.Foreground = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#10B981"));
                    break;
                case "suspended":
                    StatusBadge.Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#78350F"));
                    StatusText.Foreground = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#F59E0B"));
                    break;
                case "graduated":
                    StatusBadge.Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#1E3A5F"));
                    StatusText.Foreground = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#3B82F6"));
                    break;
                default:
                    StatusBadge.Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#1F2937"));
                    StatusText.Foreground = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#94A3B8"));
                    break;
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
            => OnEdit?.Invoke(this, _studentId);

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
            => OnDelete?.Invoke(this, _studentId);
    }
}