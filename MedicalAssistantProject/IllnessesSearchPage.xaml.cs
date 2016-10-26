using MedicalAssistantProject.Common;
using MedicalAssistantProject.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MedicalAssistantProject
{

    public sealed partial class IllnessesSearchPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private static string illnessName;

        private static bool loadUp = true;
        private static short loadTime = 150;

        DispatcherTimer timer = new DispatcherTimer();

        Random rnd = new Random();
        private int timerValue;

        DB_Connector db = new DB_Connector();
            
        /* IllnessesSearchPage Constructor */
        public IllnessesSearchPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);

            LoadIllnessData();

            if (loadUp && loadTime == 50)
            {
                timer.Interval = TimeSpan.FromMilliseconds(loadTime);
                timer.Start();
                timer.Tick += Timer_Tick;
                loadUp = false;
            }
            else
            {
                HideProgressBar();
                loadTime = 50;
            }
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

        /* Set Load flags */
        public static void SetLoadUpTrue()
        {
            loadUp = true;
            loadTime = 0;                        
        }

        /* Hide The Progress Bar and Show Result List */
        private void HideProgressBar()
        {
            ProgressBar.Visibility = Visibility.Collapsed;
            StaticSearchText.Visibility = Visibility.Collapsed;
            resultsList.Visibility = Visibility.Visible;
        }

        /* Timer Tick Period */
        private void Timer_Tick(object sender, object e)
        {

            timerValue = rnd.Next(3, 17);

            ProgressBar.Value += timerValue;

            if (ProgressBar.Value >= 100)
            {
                timer.Stop();
                HideProgressBar();
            }
        }

        /* Load Result List with Data From Main Page List */
        private void LoadIllnessData()
        {
            resultsList.DataContext = MainPage.getIllnesses();

            resultsList.Visibility = Visibility.Collapsed;
        }
    
        /* Get Illness Name */
        public static string getIllnessName()
        {
            return illnessName;
        }

        /* Store Illness name And Navigate to CurrentIllness Page */
        private void resultsList_ItemClick(object sender, ItemClickEventArgs e)
        {

            Models.Illness tempIll = (Models.Illness)e.ClickedItem;
            illnessName = tempIll.illness_name;

            this.Frame.Navigate(typeof(CurrentIllness));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }
    }
}
