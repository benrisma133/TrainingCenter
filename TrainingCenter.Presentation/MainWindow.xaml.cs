using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TrainingCenter.Presentation.Views;

namespace TrainingCenter.Presentation
{
    public partial class MainWindow : Window
    {
        private Button _activeButton;
        private bool _isSidebarOpen = true;
        private bool _userClosedSidebar = false;
        private CancellationTokenSource _sidebarCts;

        private readonly Geometry _hamburgerIcon = Geometry.Parse("M4 6H20 M4 12H20 M4 18H20");
        private readonly Geometry _closeIcon = Geometry.Parse("M11.7071 4.29289C12.0976 4.68342 12.0976 5.31658 11.7071 5.70711L6.41421 11H20C20.5523 11 21 11.4477 21 12C21 12.5523 20.5523 13 20 13H6.41421L11.7071 18.2929C12.0976 18.6834 12.0976 19.3166 11.7071 19.7071C11.3166 20.0976 10.6834 20.0976 10.2929 19.7071L3.29289 12.7071C3.10536 12.5196 3 12.2652 3 12C3 11.7348 3.10536 11.4804 3.29289 11.2929L10.2929 4.29289C10.6834 3.90237 11.3166 3.90237 11.7071 4.29289Z");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetActiveMenu(BtnDashboard);
            PageTitle.Text = "Dashboard";
        }

        // ── Active menu ───────────────────────────────────────────────────────────
        private void SetActiveMenu(Button btn)
        {
            if (_activeButton != null)
                _activeButton.ClearValue(TagProperty);
            _activeButton = btn;
            _activeButton.Tag = "Active";
        }

        // ── Hamburger toggle ──────────────────────────────────────────────────────
        private async void HamburgerBtn_Click(object sender, RoutedEventArgs e)
        {
            await ToggleSidebar();
            _userClosedSidebar = !_isSidebarOpen;
        }

        private async Task ToggleSidebar(bool forceCollapse = false)
        {
            _sidebarCts?.Cancel();
            _sidebarCts = new CancellationTokenSource();
            var token = _sidebarCts.Token;

            bool targetState = forceCollapse ? false : !_isSidebarOpen;
            const int timing = 220;
            double fromWidth = Sidebar.ActualWidth;
            double toWidth = targetState ? 260 : 64;

            _isSidebarOpen = targetState;

            // Update hamburger icon only — never move it
            HamburgerPath.Data = targetState ? _closeIcon : _hamburgerIcon;

            // Hide TEXT only when collapsing — icons stay visible always
            if (!targetState)
                SetSidebarTextsVisibility(false);

            var ease = new CubicEase { EasingMode = EasingMode.EaseInOut };
            var duration = TimeSpan.FromMilliseconds(timing);

            Sidebar.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = fromWidth,
                To = toWidth,
                Duration = duration,
                EasingFunction = ease
            });

            MainContent.BeginAnimation(MarginProperty, new ThicknessAnimation
            {
                From = MainContent.Margin,
                To = new Thickness(toWidth, 0, 0, 0),
                Duration = duration,
                EasingFunction = ease
            });

            if (targetState)
            {
                try
                {
                    await Task.Delay((int)(timing * 0.6), token);
                    SetSidebarTextsVisibility(true);
                }
                catch (TaskCanceledException) { }
            }
        }

        private void SetSidebarTextsVisibility(bool visible)
        {
            var vis = visible ? Visibility.Visible : Visibility.Collapsed;

            LogoTextStack.Visibility = vis;
            LogoBox.Visibility = vis;
            ProfileTextStack.Visibility = vis;

            foreach (var btn in new[] { BtnDashboard, BtnStudents, BtnCourses,
                                BtnInstructors, BtnEnrollments, BtnSettings })
            {
                if (btn.Content is Grid g)
                {
                    // Show/hide text only
                    foreach (UIElement child in g.Children)
                    {
                        if (child is TextBlock tb)
                            tb.Visibility = vis;
                    }

                    // Center icon when collapsed, left-align when expanded
                    if (g.ColumnDefinitions.Count > 0)
                    {
                        g.ColumnDefinitions[0].Width = visible
                            ? GridLength.Auto
                            : new GridLength(1, GridUnitType.Star);

                        if (g.ColumnDefinitions.Count > 1)
                            g.ColumnDefinitions[1].Width = visible
                                ? new GridLength(1, GridUnitType.Star)
                                : new GridLength(0);
                    }
                }

                // Center button content when collapsed
                btn.HorizontalContentAlignment = visible
                    ? HorizontalAlignment.Left
                    : HorizontalAlignment.Center;
            }
        }

        private System.Collections.Generic.IEnumerable<TextBlock> FindTextBlocks(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is TextBlock tb) yield return tb;
                foreach (var nested in FindTextBlocks(child))
                    yield return nested;
            }
        }

        // ── Window resize ─────────────────────────────────────────────────────────
        private async void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth < 1000)
            {
                if (_isSidebarOpen)
                    await ToggleSidebar(forceCollapse: true);
            }
            else
            {
                if (!_isSidebarOpen && !_userClosedSidebar)
                    await ToggleSidebar(forceCollapse: false);
            }
        }

        // ── Navigation ────────────────────────────────────────────────────────────
        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenu(BtnDashboard);
            PageTitle.Text = "Dashboard";
            PageContent.Content = null;
        }

        private void BtnStudents_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenu(BtnStudents);
            PageTitle.Text = "Students";
            PageContent.Content = new StudentsPage();
        }

        private void BtnCourses_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenu(BtnCourses);
            PageTitle.Text = "Courses";
            PageContent.Content = null;
        }

        private void BtnInstructors_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenu(BtnInstructors);
            PageTitle.Text = "Instructors";
            PageContent.Content = null;
        }

        private void BtnEnrollments_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenu(BtnEnrollments);
            PageTitle.Text = "Enrollments";
            PageContent.Content = null;
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenu(BtnSettings);
            PageTitle.Text = "Settings";
            PageContent.Content = null;
        }
    }
}