using MedicalAssistantProject.Common;
using MedicalAssistantProject.Objects;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MedicalAssistantProject
{
    public sealed partial class DocByCategory : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private DB_Connector db = new DB_Connector();

        private static string doctorName;
        private static Models.Doctor doctor;

        private static string categoryTitle;

        RelayCommand checkedGoBackCommand;


        /* Page Constructor */
        public DocByCategory()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            SetCategoryTitle();
            DoctorsSearch();

            checkedGoBackCommand = new RelayCommand(
                        () => this.CheckGoBack(),
                        () => this.CanCheckGoBack()
                    );

            navigationHelper.GoBackCommand = checkedGoBackCommand;

        }

        /* Check if BackButton is pressed */
        private bool CanCheckGoBack()
        {
            return true;
        }

        /* Show quit promt message */
        private async void CheckGoBack()
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void SetCategoryTitle()
        {
            switch (DoctorPage.GetDocType())
            {
                case "Дерматология": categoryTitle = "Дерматолози";
                    break;
                case "Анестезиология": categoryTitle = "Анестезиолози";
                    break;
                case "Патоанатомия": categoryTitle = "Патоанатоми";
                    break;
                case "Имунология": categoryTitle = "Имунолози";
                    break;
                case "Токсикология": categoryTitle = "Токсиколози";
                    break;
                case "Патология": categoryTitle = "Патолози";
                    break;
                case "Фармакология": categoryTitle = "Фармаколози";
                    break;
                case "Хирургия": categoryTitle = "Хирурзи";
                    break;
                case "Микробиология": categoryTitle = "Микробиолози";
                    break;
                case "Неврология": categoryTitle = "Невролози";
                    break;
                case "Епидемология": categoryTitle = "Епидемолози";
                    break;
                case "Кардиология": categoryTitle = "Кардиолози";
                    break;
                case "Ендокринология": categoryTitle = "Ендокринолози";
                    break;
                case "Психиатрия": categoryTitle = "Психиатри";
                    break;
                case "Детски болести": categoryTitle = "Детски болести";
                    break;
                case "Инфекциозни болести": categoryTitle = "Инф. болести";
                    break;
                case "Гинекология": categoryTitle = "Гинеколози";
                    break;
                case "Диетология": categoryTitle = "Диетолози";
                    break;
                case "Белодробни болести": categoryTitle = "Пулмолози";
                    break;
                case "Гастроентерология": categoryTitle = "Гастроентеролози";
                    break;
                case "Вътрешни болести": categoryTitle = "Вътрешни болести";
                    break;
                case "Оториноларинголия":categoryTitle = "Оториноларингози";
                    break;
                case "Офтамология": categoryTitle = "Офтамолози";
                    break;
                case "Урология": categoryTitle = "Уролози";
                    break;
                case "Хематология": categoryTitle = "Хематолози";
                    break;
                case "Онкология": categoryTitle = "Онколози";
                    break;
                case "Ренгенология": categoryTitle = "Ренгенолози";
                    break;
                case "Патофизиология": categoryTitle = "Патофизиолози";
                    break;
                case "Физиотерапия": categoryTitle = "Физиотераписти";
                    break;
                case "Неврохирургия": categoryTitle = "Неврохирурзи";
                    break;
                case "Нефрология": categoryTitle = "Нефролози";
                    break;
                case "Очни болести": categoryTitle = "Офталмолози";
                    break;
                case "Обща медицина": categoryTitle = "Общопрактикуващи";
                    break;
                case "Ревматология": categoryTitle = "Ренгенолози";
                    break;
                case "Уши нос гърло": categoryTitle = "Уши нос гърло";
                    break;
                default: categoryTitle = "Грешка!!";
                    break;
            }

            Category.Text = categoryTitle;
        }

        /* Send query and get information from Database */
        public void DoctorsSearch()
        {
            DoctorsByCategory.ItemsSource = db.getAllCategoryDoctors(DoctorPage.GetDocType());
        }

        /* Send query to database if text in search box is changed */
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Models.Doctor> doctors = db.getAllCategoryDoctors(DoctorPage.GetDocType());
            doctors.RemoveAll(x => !x.doctor_name.ToLower().Contains(SearchBox.Text.ToLower()));
            DoctorsByCategory.ItemsSource = doctors;
        }

        /* Navigate to Doctor Page */
        private void DoctorsByCategory_ItemClick(object sender, ItemClickEventArgs e)
        {
            doctor = (Models.Doctor)e.ClickedItem;
            doctorName = doctor.doctor_prof_name;

            MainPage.setPageNavSource(2);

            this.Frame.Navigate(typeof(DoctorPage));
        }

        /* Get Stored Doctor Item Data */
        public static Models.Doctor getDoctorName()
        {
            return doctor;
        }

        /* Navigate to About Page */
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AboutPage));
        }

        /* Rate application */
        private async void RateApp_Click(object sender, RoutedEventArgs e)
        {
            var appId = Windows.ApplicationModel.Store.CurrentApp.AppId;
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + appId));
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);       
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

    }
}
