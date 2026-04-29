using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TrainingCenter.Presentation.Controls.Cards;
using TrainingCenter.Repository.Entities;
using TrainingCenter.Service.Implementations;

namespace TrainingCenter.Presentation.Views
{
    public partial class CoursesPage : UserControl
    {
        private List<Course> _allCourses = new();

        public CoursesPage()
        {
            InitializeComponent();
        }

        private void CoursesPage_Loaded(object sender, RoutedEventArgs e)
            => LoadCourses();

        private async void LoadCourses()
        {
            LoadingOverlay.Visibility = Visibility.Visible;
            _allCourses = await Task.Run(() => CourseService.GetAll());
            RenderCards(_allCourses);
            LoadingOverlay.Visibility = Visibility.Collapsed;
        }

        private void RenderCards(List<Course> courses)
        {
            CardsPanel.Children.Clear();

            if (courses.Count == 0)
            {
                EmptyState.Visibility = Visibility.Visible;
                CardsPanel.Visibility = Visibility.Collapsed;
                return;
            }

            EmptyState.Visibility = Visibility.Collapsed;
            CardsPanel.Visibility = Visibility.Visible;

            foreach (var course in courses)
            {
                var card = new CourseCard();
                card.LoadCourse(course);
                card.Margin = new Thickness(8);
                card.Width = double.NaN;
                card.HorizontalAlignment = HorizontalAlignment.Stretch;
                card.OnEdit += (s, id) => Card_OnEdit(id);
                card.OnDelete += (s, id) => Card_OnDelete(id);
                card.OnView += (s, id) => ShowDetail(id);
                CardsPanel.Children.Add(card);
            }

            UpdateColumns();
        }

        private void UpdateColumns()
        {
            double width = ActualWidth;
            CardsPanel.Columns = width > 1100 ? 4
                               : width > 750 ? 3
                               : width > 450 ? 2
                               : 1;
        }

        private void CoursesPage_SizeChanged(object sender, SizeChangedEventArgs e)
            => UpdateColumns();

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchPlaceholder.Visibility = string.IsNullOrEmpty(SearchBox.Text)
                ? Visibility.Visible : Visibility.Collapsed;

            var query = SearchBox.Text.Trim().ToLower();
            var filtered = string.IsNullOrEmpty(query)
                ? _allCourses
                : _allCourses.Where(c =>
                    c.Title.ToLower().Contains(query) ||
                    c.Code.ToLower().Contains(query) ||
                    c.Level.ToLower().Contains(query)).ToList();

            RenderCards(filtered);
        }

        private void AddCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            //var frm = new frmAddEditCourse();
            //frm.Owner = Window.GetWindow(this);
            //frm.ShowDialog();
            //if (frm.IsSaved) LoadCourses();
        }

        private void Card_OnEdit(int courseId)
        {
            //var frm = new frmAddEditCourse(courseId);
            //frm.Owner = Window.GetWindow(this);
            //frm.ShowDialog();
            //if (frm.IsSaved) LoadCourses();
        }

        private void Card_OnDelete(int courseId)
        {
            var result = MessageBox.Show(
                "Are you sure you want to delete this course?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                CourseService.Delete(courseId);
                LoadCourses();
            }
        }

        private void ShowDetail(int courseId)
        {
            DetailView.OnBack -= DetailView_OnBack;
            DetailView.OnEdit -= DetailView_OnEdit;
            DetailView.OnBack += DetailView_OnBack;
            DetailView.OnEdit += DetailView_OnEdit;
            DetailView.LoadCourse(courseId);
            ListView.Visibility = Visibility.Collapsed;
            DetailView.Visibility = Visibility.Visible;
        }

        private void ShowList()
        {
            ListView.Visibility = Visibility.Visible;
            DetailView.Visibility = Visibility.Collapsed;
        }

        private void DetailView_OnBack(object sender, EventArgs e)
            => ShowList();

        private void DetailView_OnEdit(object sender, int courseId)
        {
            //var frm = new frmAddEditCourse(courseId);
            //frm.Owner = Window.GetWindow(this);
            //frm.ShowDialog();
            //if (frm.IsSaved)
            //{
            //    LoadCourses();
            //    ShowDetail(courseId);
            //}
        }
    }
}