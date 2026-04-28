using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TrainingCenter.Presentation.Controls.Cards;
using TrainingCenter.Repository.Entities;
using TrainingCenter.Service.Implementations;

namespace TrainingCenter.Presentation.Views
{
    public partial class StudentsPage : UserControl
    {
        private List<Student> _allStudents = new();

        public StudentsPage()
        {
            InitializeComponent();
        }

        private async void StudentsPage_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => { }); // ensure UI is ready
            LoadStudents();
        }

        private async void LoadStudents()
        {
            LoadingOverlay.Visibility = Visibility.Visible;

            await Task.Delay(50); // let UI render the overlay

            _allStudents = await Task.Run(() => StudentService.GetAll());
            RenderCards(_allStudents);

            LoadingOverlay.Visibility = Visibility.Collapsed;
        }

        private void RenderCards(List<Student> students)
        {
            CardsPanel.Children.Clear();

            if (students.Count == 0)
            {
                EmptyState.Visibility = Visibility.Visible;
                CardsPanel.Visibility = Visibility.Collapsed;
                return;
            }

            EmptyState.Visibility = Visibility.Collapsed;
            CardsPanel.Visibility = Visibility.Visible;

            foreach (var student in students)
            {
                var card = new StudentCard();
                card.LoadStudent(student);
                card.Margin = new Thickness(8);
                card.Width = double.NaN;
                card.HorizontalAlignment = HorizontalAlignment.Stretch;
                card.OnEdit += Card_OnEdit;
                card.OnDelete += Card_OnDelete;
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

        private void StudentsPage_SizeChanged(object sender, SizeChangedEventArgs e)
            => UpdateColumns();
        

        private void UpdateCardWidths()
        {
            int count = CardsPanel.Children.Count;
            if (count == 0) return;

            double avail = CardsPanel.ActualWidth - 16;
            if (avail <= 0) return;

            double cardWidth = count == 1 ? avail
                             : avail > 900 ? (avail / 3) - 16
                             : avail > 500 ? (avail / 2) - 16
                             : avail - 16;

            foreach (UIElement child in CardsPanel.Children)
            {
                if (child is StudentCard card)
                {
                    card.Width = cardWidth;
                    card.Margin = new Thickness(8, 8, 8, 8);
                }
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = SearchBox.Text.Trim().ToLower();

            SearchPlaceholder.Visibility = string.IsNullOrEmpty(SearchBox.Text)
                ? Visibility.Visible : Visibility.Collapsed;

            var filtered = string.IsNullOrEmpty(query)
                ? _allStudents
                : _allStudents.Where(s =>
                    s.FirstName.ToLower().Contains(query) ||
                    s.LastName.ToLower().Contains(query) ||
                    s.Email.ToLower().Contains(query)).ToList();

            RenderCards(filtered);
        }

        private void AddStudentBtn_Click(object sender, RoutedEventArgs e)
        {
            // Will open frmAddEditStudent in next step
            MessageBox.Show("Add Student form coming soon!", "TrainingCenter");
        }

        private void Card_OnEdit(object sender, int studentId)
        {
            // Will open frmAddEditStudent in Update mode
            MessageBox.Show($"Edit student {studentId} coming soon!", "TrainingCenter");
        }

        private void Card_OnDelete(object sender, int studentId)
        {
            var result = MessageBox.Show(
                "Are you sure you want to delete this student?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                StudentService.Delete(studentId);
                LoadStudents();
            }
        }
    }
}