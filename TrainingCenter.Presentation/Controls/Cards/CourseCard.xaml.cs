using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TrainingCenter.Repository.Entities;

namespace TrainingCenter.Presentation.Controls.Cards
{
    public partial class CourseCard : UserControl
    {
        public event EventHandler<int> OnEdit;
        public event EventHandler<int> OnDelete;
        public event EventHandler<int> OnView;

        private int _courseId;

        public CourseCard()
        {
            InitializeComponent();
        }

        public void LoadCourse(Course course)
        {
            _courseId = course.CourseId;

            // Icon based on level
            IconText.Text = course.Level switch
            {
                "Beginner" => "🌱",
                "Intermediate" => "📘",
                "Advanced" => "🚀",
                _ => "📚"
            };

            TitleText.Text = course.Title;
            CodeText.Text = course.Code;
            PriceText.Text = $"${course.Price:F2}";
            DurationText.Text = $"{course.DurationHours}h";
            LevelText.Text = course.Level;

            SetStatus(course.Status);
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
                case "draft":
                    StatusBadge.Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#1F2937"));
                    StatusText.Foreground = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#94A3B8"));
                    break;
                case "archived":
                    StatusBadge.Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#78350F"));
                    StatusText.Foreground = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#F59E0B"));
                    break;
                case "closed":
                    StatusBadge.Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#7F1D1D"));
                    StatusText.Foreground = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#EF4444"));
                    break;
                default:
                    StatusBadge.Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#1F2937"));
                    StatusText.Foreground = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#94A3B8"));
                    break;
            }
        }

        private void CardBorder_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.OriginalSource is System.Windows.Shapes.Path) return;
            OnView?.Invoke(this, _courseId);
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
            => OnEdit?.Invoke(this, _courseId);

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
            => OnDelete?.Invoke(this, _courseId);
    }
}