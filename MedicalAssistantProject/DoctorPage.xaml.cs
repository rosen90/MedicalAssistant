//#define DEVICE

using MedicalAssistantProject.Common;
using MedicalAssistantProject.Objects;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.ApplicationModel.Email;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Storage.Streams;
using Windows.Foundation;


// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MedicalAssistantProject
{
    public sealed partial class DoctorPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        DB_Connector db = new DB_Connector();

        private static string postfix = ".jpg";
        private static string PathBase = "ms-appx:///Pictures/Portraits/";
        private static string DefaultPath = PathBase + "default" + postfix;
        private static string docType;
        
        /* DoctorPage Constructor */
        public DoctorPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            string path = PathBase + db.getDoctorID(GetDoctor().doctor_name)[0].doctor_id.ToString() + postfix;
            SetImage(path);
            LoadDoctorData();
        }

        /* Initialization */
        private void Init()
        {
         
        }

        /* Navigate to About Page */
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AboutPage));
        }

        /* Open Rate window */
        private async void RateApp_Click(object sender, RoutedEventArgs e)
        {
            var appId = Windows.ApplicationModel.Store.CurrentApp.AppId;
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + appId));
        }

        /* Check For Navigation Source */
        private Models.Doctor GetDoctor()
        {
            Models.Doctor doctor = new Models.Doctor();

            if (CurrentIllness.getDoctorName() != null || DocByCategory.getDoctorName() != null)
            {
                switch (MainPage.getPageNavSource())
                {
                    case 0:
                        doctor = MainPage.getDoctorName();
                        break;
                    case 1:
                        doctor = CurrentIllness.getDoctorName();
                        break;
                    case 2:
                        doctor = DocByCategory.getDoctorName();
                        break;
                }
            }
            else
            {
                doctor = MainPage.getDoctorName();
            }

            return doctor;
        }

        /* Set Doctor Image */
        private async void SetImage(string path)
        {
            Uri uri = null;

            try
            {
                uri = new Uri(path, UriKind.Absolute);
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
                if(file == null)
                {
                    throw new Exception();
                }
            }
            catch(Exception)
            {
                uri = new Uri(DefaultPath, UriKind.Absolute);
            }
            finally
            {
                DocPortrait.Source = new BitmapImage(uri);
            }
        }

        #region NavigationHelper registration

        /* Adjust Doctor Location on the Map */
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);

            var locator = new Geolocator();
            locator.DesiredAccuracyInMeters = 50;

            var position = await locator.GetGeopositionAsync();

            var docPosition = new BasicGeoposition();
            docPosition.Latitude = db.getDoctorInfo(GetDoctor().doctor_name)[0].latitude;
            docPosition.Longitude = db.getDoctorInfo(GetDoctor().doctor_name)[0].longitude;

            var myPoint = new Geopoint(docPosition);

            if (await DocLocation.TrySetViewAsync(myPoint, 10D))
            {

            }

            DocLocation.ZoomLevel = 16;

            var gl = new Geolocator() { DesiredAccuracy = PositionAccuracy.High };
            var location = await gl.GetGeopositionAsync(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(5));

            var pin = new MapIcon()
            {
                Location = myPoint,
                Title = db.getDoctorInfo(GetDoctor().doctor_name)[0].hospital_name,
                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Pictures/pin.png")),
                NormalizedAnchorPoint = new Point() { X = 0.32, Y = 0.78 },
            };
            DocLocation.MapElements.Add(pin);
        }

        /* Deal Doctor phone number (make a Call) */
        private void button_Click(object sender, RoutedEventArgs e)
        {
            string phoneNumber = db.getDoctorInfo(GetDoctor().doctor_name)[0].doctor_telephone;
            Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(phoneNumber, GetDoctor().doctor_name);
        }

        /* Send Email to Doctor */
        private async void SendEmail_Click(object sender, RoutedEventArgs e)
        {

            if (db.getDoctorInfo(GetDoctor().doctor_name)[0].doctor_email != "няма")
            {
                // Define Recipient
                EmailRecipient sendTo = new EmailRecipient()
                {
                    Name = GetDoctor().doctor_prof_name,
                    Address = db.getDoctorInfo(GetDoctor().doctor_name)[0].doctor_email
                };

                // Create email object
                EmailMessage mail = new EmailMessage();
                mail.Subject = "";
                mail.Body = "";

                mail.To.Add(sendTo);

                await EmailManager.ShowComposeNewEmailAsync(mail);
            }
    
        }

        /* Load Doctor Information Data From Database */
        private void LoadDoctorData()
        {

            string emailWidth = "";

            DoctorName.Header = GetDoctor().doctor_prof_name;
            CallPhone.Content = db.getDoctorInfo(GetDoctor().doctor_name)[0].doctor_telephone;


            if (db.getDoctorInfo(GetDoctor().doctor_name)[0].doctor_email.Length > 23)
            {
                for (int i = 0; i < 20; i++)
                {
                    if (i < 17)
                    {
                        char ch = db.getDoctorInfo(GetDoctor().doctor_name)[0].doctor_email[i];
                        emailWidth += ch;
                    }
                    else
                    {
                        emailWidth += ".";
                    }
                }
            }
            else
            {
                emailWidth = db.getDoctorInfo(GetDoctor().doctor_name)[0].doctor_email;
            }

            EmailDynamicText.Text = emailWidth;

            ShowAdress.Text = db.getDoctorInfo(GetDoctor().doctor_name)[0].hospital_name + " - " +
                db.getDoctorInfo(GetDoctor().doctor_name)[0].address_str;
            DoctorType.Text = db.getDoctorInfo(GetDoctor().doctor_name)[0].type_name;

            docType = DoctorType.Text;

            DoctorBio.Text = db.getDoctorInfo(GetDoctor().doctor_name)[0].doctor_bio;
        }

        public static string GetDocType()
        {
            return docType;
        }

        private void ShowCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DocByCategory));
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

        private async void NavButton_Click(object sender, RoutedEventArgs e)
        {
           
            var docPosition = new BasicGeoposition();
            docPosition.Latitude = db.getDoctorInfo(GetDoctor().doctor_name)[0].latitude;
            docPosition.Longitude = db.getDoctorInfo(GetDoctor().doctor_name)[0].longitude;

#if (DEVICE)

            /* REAL DEVICE */
            string uriToLaunch = @"ms-drive-to:?destination.latitude=" + docPosition.Latitude + "&destination.longitude=" + docPosition.Longitude + "&destination.name=Sofia, BG";

#else
            /* DUMMY DEVICE */
            string uriToLaunch = @"Maps:?destination.latitude=42.684550" + "&destination.longitude=23.303640&destination.name=Sofia, SF";

#endif
            var uri = new Uri(uriToLaunch);

            await Windows.System.Launcher.LaunchUriAsync(uri);

        }

    }
}
