using MedicalAssistantProject.Common;
using MedicalAssistantProject.Objects;
using System;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MedicalAssistantProject
{
    public sealed partial class CurrentIllness : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private static string doctorName;
        private static Models.Doctor doctor;

        DB_Connector db = new DB_Connector();

        /* CurrentIllness Constructor */
        public CurrentIllness()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            LoadUpData();

        }

        /* Navigate to About page */
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AboutPage));
        }

        private async void RateApp_Click(object sender, RoutedEventArgs e)
        {
            var appId = Windows.ApplicationModel.Store.CurrentApp.AppId;
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + appId));
        }

        /* Check For Navigation Source */
        public static Models.Illness GetIllness()
        {
            Models.Illness illness = new Models.Illness();
            if (IllnessesSearchPage.getIllnessName() != null && !MainPage.getDestinationSource())
            {
                illness.illness_name = IllnessesSearchPage.getIllnessName();
            }
            else
            {
                illness = MainPage.getIllnessName();
            }

            return illness;
        }

        /* Load Illness Data From Database */
        private void LoadUpData()
        {

            IllnessName.Text = GetIllness().illness_name;
            IllnessesSearchPage.SetLoadUpTrue();

            IllDescription.Text = db.getIllDescription(GetIllness().illness_name)[0].illness_descr;
            Uri currentIll_link = new Uri(db.getIllDescription(GetIllness().illness_name)[0].illness_link, UriKind.Absolute);
            ReadMore.NavigateUri = currentIll_link;

            Doctors.ItemsSource = db.getDoctors(GetIllness().illness_name);


            if (ResizeIllName(GetIllness().illness_name) > 14)
            {
                Thickness margin = SympButton.Margin;
                margin.Top = 89;
                SympButton.Margin = margin;
            }

            if (ResizeIllName(GetIllness().illness_name) > 30)
            {
                Thickness margin = SympButton.Margin;
                margin.Top = 114;
                SympButton.Margin = margin;
            }
        }

        /* Change properties of illness title if its too long */
        private int ResizeIllName(string name)
        {
            if (name.Length > 14)
            {
                IllnessName.FontSize = 26;
                IllnessName.FontWeight = FontWeights.Bold;
                IllnessName.TextWrapping = TextWrapping.Wrap;
                IllnessName.LineHeight = 25;
            }


           return name.Length;
        }

        /* Store Doctor Item Data and Navigate to DoctorPage */
        private void Doctors_ItemClick(object sender, ItemClickEventArgs e)
        {
            doctor = (Models.Doctor)e.ClickedItem;
            doctorName = doctor.doctor_prof_name;

            MainPage.setPageNavSource(1);

            this.Frame.Navigate(typeof(DoctorPage));
        }

        /* Get Doctor Item */
        public static Models.Doctor getDoctorName()
        {
            return doctor;
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

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CurrentIllnesSympt));
        }
    }
}
