using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TrainingCenter.Repository.Entities;
using TrainingCenter.Service.Implementations;

namespace TrainingCenter.Presentation.Views
{
    public partial class CourseDetailView : UserControl
    {
        private Course _course;
        public event EventHandler OnBack;
        public event EventHandler<int> OnEdit;

        public CourseDetailView()
        {
            InitializeComponent();
        }

        public void LoadCourse(int courseId)
        {
            var all = CourseService.GetAll();
            _course = all.Find(c => c.CourseId == courseId);
            if (_course == null) return;

            IconText.Text = _course.Level switch
            {
                "Beginner" => "🌱",
                "Intermediate" => "📘",
                "Advanced" => "🚀",
                _ => "📚"
            };

            TitleText.Text = _course.Title;
            CodeText.Text = $"Code: {_course.Code}";
            InstructorText.Text = $"Instructor: {_course.Instructor?.FirstName} {_course.Instructor?.LastName}";
            PriceText.Text = $"${_course.Price:F2}";
            DurationText.Text = $"{_course.DurationHours} hours";
            LevelText.Text = _course.Level;
            PublishedText.Text = _course.PublishedAt.HasValue
                                   ? _course.PublishedAt.Value.ToString("MMM dd, yyyy")
                                   : "Not published";
            DescriptionText.Text = string.IsNullOrEmpty(_course.Description)
                                   ? "No description available." : _course.Description;

            SetStatus(_course.Status);
            LoadEnrollments(courseId);
        }

        private void LoadEnrollments(int courseId)
        {
            var enrollments = EnrollmentService.GetByCourseId(courseId);
            EnrollmentCountText.Text = enrollments.Count.ToString();
            EnrollmentsPanel.Children.Clear();

            if (enrollments.Count == 0)
            {
                EnrollmentsPanel.Children.Add(new TextBlock
                {
                    Text = "No students enrolled yet",
                    FontSize = 14,
                    Foreground = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#64748B")),
                    Margin = new Thickness(12)
                });
                return;
            }

            foreach (var enrollment in enrollments)
            {
                var grid = new Grid { Margin = new Thickness(12, 8, 12, 8) };
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });

                var studentName = new TextBlock
                {
                    Text = $"{enrollment.Student?.FirstName} {enrollment.Student?.LastName}",
                    FontSize = 14,
                    FontWeight = FontWeights.Medium,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F1F5F9")),
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(studentName, 0);

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
                    Background = new SolidColorBrush(GetStatusBgColor(enrollment.Status)),
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(10, 4, 10, 4),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center,
                    Child = new TextBlock
                    {
                        Text = enrollment.Status,
                        FontSize = 12,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(GetStatusColor(enrollment.Status))
                    }
                };
                Grid.SetColumn(statusBadge, 2);

                grid.Children.Add(studentName);
                grid.Children.Add(progressStack);
                grid.Children.Add(statusBadge);

                var row = new Border
                {
                    Child = grid,
                    CornerRadius = new CornerRadius(8),
                    Background = new SolidColorBrush(Colors.Transparent)
                };
                row.MouseEnter += (s, e) => row.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#252538"));
                row.MouseLeave += (s, e) => row.Background = new SolidColorBrush(Colors.Transparent);
                EnrollmentsPanel.Children.Add(row);
            }
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
                case "draft":
                    StatusBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F2937"));
                    StatusText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#94A3B8"));
                    break;
                case "archived":
                    StatusBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78350F"));
                    StatusText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F59E0B"));
                    break;
                case "closed":
                    StatusBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F1D1D"));
                    StatusText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF4444"));
                    break;
                default:
                    StatusBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F2937"));
                    StatusText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#94A3B8"));
                    break;
            }
        }

        private Color GetStatusColor(string status) => status?.ToLower() switch
        {
            "completed" => (Color)ColorConverter.ConvertFromString("#10B981"),
            "active" => (Color)ColorConverter.ConvertFromString("#3B82F6"),
            "cancelled" => (Color)ColorConverter.ConvertFromString("#EF4444"),
            _ => (Color)ColorConverter.ConvertFromString("#94A3B8")
        };

        private Color GetStatusBgColor(string status) => status?.ToLower() switch
        {
            "completed" => (Color)ColorConverter.ConvertFromString("#064E3B"),
            "active" => (Color)ColorConverter.ConvertFromString("#1E3A5F"),
            "cancelled" => (Color)ColorConverter.ConvertFromString("#7F1D1D"),
            _ => (Color)ColorConverter.ConvertFromString("#1F2937")
        };

        private void BackBtn_Click(object sender, RoutedEventArgs e)
            => OnBack?.Invoke(this, EventArgs.Empty);

        private void EditBtn_Click(object sender, RoutedEventArgs e)
            => OnEdit?.Invoke(this, _course.CourseId);
    }
}