using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TrainingCenter.Repository.Entities;
using TrainingCenter.Service.Implementations;

namespace TrainingCenter.Presentation.Views
{
    public partial class StudentDetailView : UserControl
    {
        private Student _student;
        public event EventHandler OnBack;
        public event EventHandler<int> OnEdit;

        public StudentDetailView()
        {
            InitializeComponent();
        }

        public void LoadStudent(int studentId)
        {
            var all = StudentService.GetAll();
            _student = all.Find(s => s.StudentId == studentId);
            if (_student == null) return;

            AvatarText.Text = _student.FirstName.Substring(0, 1).ToUpper();
            FullNameText.Text = $"{_student.FirstName} {_student.LastName}";
            EmailText.Text = _student.Email;
            PhoneText.Text = string.IsNullOrEmpty(_student.PhoneNumber)
                                ? "No phone" : _student.PhoneNumber;

            SetStatus(_student.Status);

            DobText.Text = _student.DateOfBirth.ToString("MMM dd, yyyy");
            RegisteredText.Text = _student.RegisteredAt.ToString("MMM dd, yyyy");

            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - _student.DateOfBirth.Year;
            if (_student.DateOfBirth > today.AddYears(-age)) age--;
            AgeText.Text = $"{age} years";

            var profile = _student.StudentProfile;
            CityText.Text = profile?.City ?? "—";
            CountryText.Text = profile?.Country ?? "—";
            LinkedInText.Text = profile?.LinkedInUrl ?? "—";
            BioText.Text = profile?.Bio ?? "—";

            LoadEnrollments(studentId);
        }

        private void LoadEnrollments(int studentId)
        {
            var enrollments = EnrollmentService.GetByStudentId(studentId);
            EnrollmentCountText.Text = enrollments.Count.ToString();
            EnrollmentsPanel.Children.Clear();

            if (enrollments.Count == 0)
            {
                EnrollmentsPanel.Children.Add(new TextBlock
                {
                    Text = "No enrollments yet",
                    FontSize = 14,
                    Foreground = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#64748B")),
                    Margin = new Thickness(12)
                });
                return;
            }

            foreach (var enrollment in enrollments)
                EnrollmentsPanel.Children.Add(CreateEnrollmentRow(enrollment));
        }

        private Border CreateEnrollmentRow(Enrollment enrollment)
        {
            var grid = new Grid { Margin = new Thickness(12, 8, 12, 8) };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });

            var courseName = new TextBlock
            {
                Text = enrollment.Course?.Title ?? "Unknown",
                FontSize = 14,
                FontWeight = FontWeights.Medium,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F1F5F9")),
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(courseName, 0);

            var progressStack = new StackPanel { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center };
            var progressBar = new ProgressBar
            {
                Value = (double)enrollment.ProgressPercent,
                Maximum = 100,
                Height = 6,
                Width = 90,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D44")),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B82F6")),
                BorderThickness = new Thickness(0)
            };
            var progressText = new TextBlock
            {
                Text = $"{enrollment.ProgressPercent}%",
                FontSize = 11,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#94A3B8")),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 3, 0, 0)
            };
            progressStack.Children.Add(progressBar);
            progressStack.Children.Add(progressText);
            Grid.SetColumn(progressStack, 1);

            var statusBadge = new Border
            {
                Background = new SolidColorBrush(GetEnrollmentStatusBgColor(enrollment.Status)),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(10, 4, 10, 4),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Child = new TextBlock
                {
                    Text = enrollment.Status,
                    FontSize = 12,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new SolidColorBrush(GetEnrollmentStatusColor(enrollment.Status))
                }
            };
            Grid.SetColumn(statusBadge, 2);

            grid.Children.Add(courseName);
            grid.Children.Add(progressStack);
            grid.Children.Add(statusBadge);

            var row = new Border { Child = grid, CornerRadius = new CornerRadius(8), Background = new SolidColorBrush(Colors.Transparent) };
            row.MouseEnter += (s, e) => row.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#252538"));
            row.MouseLeave += (s, e) => row.Background = new SolidColorBrush(Colors.Transparent);
            return row;
        }

        private void SetStatus(string status)
        {
            StatusText.Text = status;
            switch (status?.ToLower())
            {
                case "active":
                    StatusBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#064E3B"));
                    StatusText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10B981"));
                    break;
                case "suspended":
                    StatusBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78350F"));
                    StatusText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F59E0B"));
                    break;
                case "graduated":
                    StatusBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3A5F"));
                    StatusText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B82F6"));
                    break;
                default:
                    StatusBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F2937"));
                    StatusText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#94A3B8"));
                    break;
            }
        }

        private Color GetEnrollmentStatusColor(string status) => status?.ToLower() switch
        {
            "completed" => (Color)ColorConverter.ConvertFromString("#10B981"),
            "active" => (Color)ColorConverter.ConvertFromString("#3B82F6"),
            "cancelled" => (Color)ColorConverter.ConvertFromString("#EF4444"),
            _ => (Color)ColorConverter.ConvertFromString("#94A3B8")
        };

        private Color GetEnrollmentStatusBgColor(string status) => status?.ToLower() switch
        {
            "completed" => (Color)ColorConverter.ConvertFromString("#064E3B"),
            "active" => (Color)ColorConverter.ConvertFromString("#1E3A5F"),
            "cancelled" => (Color)ColorConverter.ConvertFromString("#7F1D1D"),
            _ => (Color)ColorConverter.ConvertFromString("#1F2937")
        };

        private void BackBtn_Click(object sender, RoutedEventArgs e)
            => OnBack?.Invoke(this, EventArgs.Empty);

        private void EditBtn_Click(object sender, RoutedEventArgs e)
            => OnEdit?.Invoke(this, _student.StudentId);
    }
}