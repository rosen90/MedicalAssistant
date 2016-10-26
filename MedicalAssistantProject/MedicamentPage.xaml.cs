using MedicalAssistantProject.Common;
using MedicalAssistantProject.Objects;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MedicalAssistantProject
{

    public sealed partial class MedicamentPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        DB_Connector db = new DB_Connector();

        /* MedicamentPage Constructor */
        public MedicamentPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            LoadMedicamentData();
          
        }

        /* Navigation to About Page */
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AboutPage));
        }

        /* Open Rate windows */
        private async void RateApp_Click(object sender, RoutedEventArgs e)
        {
            var appId = Windows.ApplicationModel.Store.CurrentApp.AppId;
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + appId));
        }

        /* Load Medicament Information Data from Database */
        private void LoadMedicamentData()
        {
            MedName.Header = MainPage.getMedicamentName().med_name;
            Description.Text = db.getMedicamentInfo(MainPage.getMedicamentName().med_name).FirstOrDefault().med_descr;
            Before_use.Text = db.getMedicamentInfo(MainPage.getMedicamentName().med_name).FirstOrDefault().med_before_use;
            How_to_use.Text = db.getMedicamentInfo(MainPage.getMedicamentName().med_name).FirstOrDefault().med_how_to_use;
            Side_effects.Text = db.getMedicamentInfo(MainPage.getMedicamentName().med_name).FirstOrDefault().med_side_effect;
            How_to_storage.Text = db.getMedicamentInfo(MainPage.getMedicamentName().med_name).FirstOrDefault().med_how_to_storage;
            Additional_info.Text = db.getMedicamentInfo(MainPage.getMedicamentName().med_name).FirstOrDefault().med_additional_info;
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
