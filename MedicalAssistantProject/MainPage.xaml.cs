//#define DEVICE

using MedicalAssistantProject.Common;
using MedicalAssistantProject.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace MedicalAssistantProject
{
    public sealed partial class MainPage : Page
    {
        private DB_Connector db = new DB_Connector();
        private bool headSelected = false, torsoSelected = false, armsSelected = false, legsSelected = false;
        private static List<Models.Illness> illnesses = new List<Models.Illness>();

        private static string doctorName;

        private static Models.Doctor doctor;
        private static Models.Illness illness;
        private static Models.Medicament medicament;
        private static Models.HealthFacility hospital;

        private static float navLatitude;
        private static float navLongitute;

        private static bool waitForDestination;


        private static double drivingDistance;

        public object IsolatedStorageSettings { get; private set; }

        public struct Position
        {
            public double Latitude;
            public double Longitude;
        }

        private static bool mainPageSource;
        private static int pageNavSource;

        RelayCommand checkedGoBackCommand;

        private NavigationHelper navigationHelper;

        /* MainPage Constructor */
        public MainPage()
        {

            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.NavigationCacheMode = NavigationCacheMode.Required;

            //popUp();
            DoctorsSearch();
            IllnessSearch();
            MedicamentSearch();

            pageNavSource = 0;

            checkedGoBackCommand = new RelayCommand(
                                    () => this.CheckGoBack(),
                                    () => this.CanCheckGoBack()
                                );

            navigationHelper.GoBackCommand = checkedGoBackCommand;

        }


        //##################################### Common ###########################################//

        /* Check if BackButton is pressed */
        private bool CanCheckGoBack()
        {
            return true;
        }

        /* Show quit promt message */
        private async void CheckGoBack()
        {
            Debug.WriteLine("CheckGoBack");
            MessageDialog dlg = new MessageDialog("Сигурни ли сте, че искате да излезете ?", "Внимание");
            dlg.Commands.Add(new UICommand("Да", new UICommandInvokedHandler(CommandHandler1)));
            dlg.Commands.Add(new UICommand("Не", new UICommandInvokedHandler(CommandHandler1)));

            await dlg.ShowAsync();
        }

        /* Handle event by choose option */
        private void CommandHandler1(IUICommand command)
        {
            var label = command.Label;
            switch (label)
            {
                case "Да":
                {
                    Application.Current.Exit();
                    break;
                }
                case "Не":
                {
                    break;
                }
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {


            // Create Database if not exist
            bool dbExists = await db.CheckDbAsync();

            if (!dbExists)
            {
                await db.CopyDatabase();
            }

            mapLocations();

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


        //##################################### Pivot View 5 Close to you ##########################################//

        private async void mapLocations()
        {


            List<Models.HealthFacility> healthFacilitties = db.getAllHospitals();

            var locator = new Geolocator();
            locator.DesiredAccuracyInMeters = 5;

#if (DEVICE)

            /* REAL DEVICE */
            var devicePosition = await locator.GetGeopositionAsync();

            await MainMap.TrySetViewAsync(devicePosition.Coordinate.Point, 12D);

#else
            ///*TEMPORARY FOR DEBUGGING*/
            /// DUMMY DEVICE
            var debugPosition = new BasicGeoposition();
            debugPosition.Latitude = 42.642791;
            debugPosition.Longitude = 23.338603;
            var debugPoint = new Geopoint(debugPosition);
            ////#####################################################
#endif

            Position Start, End;

#if (DEVICE) 

            /* REAL DEVICE COORDS */
            Start.Latitude = devicePosition.Coordinate.Point.Position.Latitude;
            Start.Longitude = devicePosition.Coordinate.Point.Position.Longitude;

#else

            /* DUMMY DEVICE COORDS */
            Start.Latitude = 42.642791;
            Start.Longitude = 23.338603;
#endif

            for (int i = 0; i < healthFacilitties.Count; i++)
            {

                End.Latitude = healthFacilitties[i].latitude;
                End.Longitude = healthFacilitties[i].longitude;

                healthFacilitties[i].distance = Distance(Start, End);
                healthFacilitties[i].hospital_name = healthFacilitties[i].hospital_name + " - " +
                healthFacilitties[i].distance.ToString("0.0") + " km";

            }

            healthFacilitties = healthFacilitties.OrderBy(x => x.distance).ToList();
            Hospitals.ItemsSource = healthFacilitties;


#if (DEVICE)
/* REAL DEVICE */
//await MainMap.TrySetViewAsync(devicePosition.Coordinate.Point, 12D);

#else
            /* DUMMY DEVICE */
            await MainMap.TrySetViewAsync(debugPoint, 12D);
#endif

            var pin = new MapIcon()
            {

#if (DEVICE)
                /* REAL DEVICE */
                Location = devicePosition.Coordinate.Point,
#else
                /* DUMMY DEVICE */
                Location = debugPoint,
#endif

                Title = "I am here",
                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Pictures/pin.png")),
                NormalizedAnchorPoint = new Point() { X = 0.32, Y = 0.78 },

            };


            MainMap.MapElements.Add(pin);

            for (int i = 0; i < db.getAllHospitals().Count; i++)
            {

                var currHospitalPos = new BasicGeoposition();
                currHospitalPos.Latitude = db.getAllHospitals()[i].latitude;
                currHospitalPos.Longitude = db.getAllHospitals()[i].longitude;
                var hospitalLocationPoint = new Geopoint(currHospitalPos);


                var hostpital = new MapIcon()
                {
                    Location = hospitalLocationPoint,
                    Title = db.getAllHospitals()[i].hospital_name,
                    Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Pictures/hospital_pin.png")),
                    NormalizedAnchorPoint = new Point() { X = 0.32, Y = 0.78 },
                   
                };

                MainMap.MapElements.Add(hostpital);
            }

        }

        /* Calculate Distance in straight line between your position and desired one */
        public double Distance(Position startPos, Position endPos)
        {
            double km = 6371;
            double dLat = this.toRadian(endPos.Latitude - startPos.Latitude);
            double dLon = this.toRadian(endPos.Longitude - startPos.Longitude);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(this.toRadian(startPos.Latitude)) * Math.Cos(this.toRadian(endPos.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double distance = km * c;

            return distance;
        }

        /* Convert to Radians */
        private double toRadian(double value)
        {
            return (Math.PI / 180) * value;
        }


        /* Select and focus current hospital */
        private async void Hospitals_ItemClick(object sender, ItemClickEventArgs e)
        {
            hospital = (Models.HealthFacility)e.ClickedItem;

            var hospitalPosition = new BasicGeoposition();
            hospitalPosition.Latitude = hospital.latitude;
            hospitalPosition.Longitude = hospital.longitude;
            var hospitalPoint = new Geopoint(hospitalPosition);

            await MainMap.TrySetViewAsync(hospitalPoint, 15D);

            navLatitude = hospital.latitude;
            navLongitute = hospital.longitude;

            waitForDestination = true;

        }

        /* Send location to outsource Navigation app */
        private async void Navigate_Click(object sender, RoutedEventArgs e)
        {
            if (hospital != null )
            {
                if (!waitForDestination)
                {
                    MessageDialog msgbox = new MessageDialog("Моля изчакайте...");
                    msgbox.ShowAsync();
                    waitForDestination = false;
                }
                else
                {

#if (DEVICE)
                    /* REAL DEVICE */
                    string uriToLaunch = @"ms-drive-to:?destination.latitude=" + navLatitude + "&destination.longitude=" + navLongitute + "&destination.name=Sofia, BG";

#else
                    /* DUMMY DEVICE */
                    string uriToLaunch = @"Maps:?destination.latitude=42.684550" + "&destination.longitude=23.303640&destination.name=Sofia, SF";

#endif
                    var uri = new Uri(uriToLaunch);

                    await Windows.System.Launcher.LaunchUriAsync(uri);
                }

            }
            else
            {
                MessageDialog msgbox = new MessageDialog("Не сте избрали дестинация!");
                msgbox.ShowAsync();
            }

        }

        //public async void DrivingDistance(Position startPos, Position endPos)
        //{

        //    /* Requare Map Token from dev center ----> https://msdn.microsoft.com/en-us/library/windows/apps/xaml/dn631250.aspx
        //                                               https://msdn.microsoft.com/en-us/library/windows/apps/xaml/dn741528.aspx */
        //    MapService.ServiceToken = "AuQrJsUO_lfjBItbttjlm73Tnnmmu0FiJi7AJoT3md3IE-Ia_vs3Argzs_WbP67N";

        //    // Start Point - Device Location
        //    BasicGeoposition startLocation = new BasicGeoposition();
        //    startLocation.Latitude = 42.642791;
        //    startLocation.Longitude = 23.338603;
        //    Geopoint startPoint = new Geopoint(startLocation);


        //    // End Hospital Location.
        //    BasicGeoposition endLocation = new BasicGeoposition();
        //    endLocation.Latitude = endPos.Latitude;
        //    endLocation.Longitude = endPos.Longitude;
        //    Geopoint endPoint = new Geopoint(endLocation);


        //    MapRouteFinderResult routeResult =
        //        await MapRouteFinder.GetDrivingRouteAsync(
        //        startPoint,
        //        endPoint,
        //        MapRouteOptimization.Time,
        //        MapRouteRestrictions.None);

        //    if (routeResult.Status == MapRouteFinderStatus.Success)
        //    {
        //        drivingDistance = (routeResult.Route.LengthInMeters / 1000);
        //    }
        //}




        //##################################### Pivot View 4 Medicament ###########################################//

        /* Send query and get information from Database */
        public void MedicamentSearch()
        {
            MedList.ItemsSource = db.getAllMeds();
        }

        /* Send query to database if text in search box is changed */
        private void SearchBoxMed_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Models.Medicament> meds = db.getAllMeds();
            meds.RemoveAll(x => !x.med_name.ToLower().Contains(SearchBoxMed.Text.ToLower()));
            MedList.ItemsSource = meds;
        }

        /* Store Clicked Item Data and navigate to CurrentIllness Page */
        private void MedList_ItemClick(object sender, ItemClickEventArgs e)
        {
            medicament = (Models.Medicament)e.ClickedItem;

            this.Frame.Navigate(typeof(MedicamentPage));
        }

        /* Get Stored Medicament Item Data */
        public static Models.Medicament getMedicamentName()
        {
            return medicament;
        }

        //##################################### Pivot View 3 Illnesses ############################################//

        /* Send query and get information from Database */
        public void IllnessSearch()
        {
            IllnessList.ItemsSource = db.getAll_Illnesses();
        }

        /* Send query to database if text in search box is changed */
        private void SearchBoxIll_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Models.Illness> illnesses = db.getAll_Illnesses();
            illnesses.RemoveAll(x => !x.illness_name.ToLower().Contains(SearchBoxIll.Text.ToLower()));
            IllnessList.ItemsSource = illnesses;
        }

        /* Store Clicked Item Data and navigate to CurrentIllness Page */
        private void IllnessList_ItemClick(object sender, ItemClickEventArgs e)
        {
            illness = (Models.Illness)e.ClickedItem;
            mainPageSource = true;
            this.Frame.Navigate(typeof(CurrentIllness));
        }

        /* Get Stored Illness Item Data */
        public static Models.Illness getIllnessName()
        {
            return illness;
        }

        //##################################### Pivot View 2 Doctors #########################################//

        /* Send query and get information from Database */
        public void DoctorsSearch()
        {
            DoctorList.ItemsSource = db.getAllDoctors();
        }

        /* Send query to database if text in search box is changed */
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Models.Doctor> doctors = db.getAllDoctors();
            doctors.RemoveAll(x => !x.doctor_name.ToLower().Contains(SearchBox.Text.ToLower()));
            DoctorList.ItemsSource = doctors;
        }

        /* Store Clicked Item Data and navigate to DoctorPage */
        private void DoctorList_ItemClick(object sender, ItemClickEventArgs e)
        {
            doctor = (Models.Doctor)e.ClickedItem;
            doctorName = doctor.doctor_prof_name;

            mainPageSource = true;
            pageNavSource = 0;


            this.Frame.Navigate(typeof(DoctorPage));
        }

        /* Set source destination page */
        public static void setPageNavSource(int source)
        {
            pageNavSource = source;
        }

        /* Get source destination page */
        public static int getPageNavSource()
        {
            return pageNavSource;
        }


        /* Set source destination page for illness */
        public static void setDestinationSource(bool source)
        {
            mainPageSource = source;
        }

        /* Get source destination page for illness */
        public static bool getDestinationSource()
        {
            return mainPageSource;
        }

        /* Get Stored Doctor Item Data */
        public static Models.Doctor getDoctorName()
        {
            return doctor;
        }


        //################################# Pivot View 1 Symptoms #######################################//

        /* Show popUp message at start up */
        public async void popUp()
        {
            MessageDialog msgbox = new MessageDialog("Това приложение не ви дава правото да прилагате лечение по своя инициатива.", "Внимание!");
            msgbox.ShowAsync();
        }

        /* Store The List Of Symptoms And Navigate to SearchPage */
        private async void SearchBtn_Click(object sender, RoutedEventArgs e)
        {

            clickSnd.Play();

            if (SymptChoices.Items.Count < 1)
            {

                MessageDialog msgbox = new MessageDialog("Не сте избрали симптоми.");
                msgbox.ShowAsync();

            }
            else
            {

                List<string> list = new List<string>();

                string SympName;

                for (int i = 0; i < SymptChoices.Items.Count; i++)
                {
                   var selectedNameObject = SymptChoices.Items[i] as Models.Symptom;

                    SympName = selectedNameObject.symptom_name;

                    list.Add(SympName);

                }
               
                if (illnesses.Count > 0)
                {
                    illnesses.Clear();
                }

                illnesses = db.getIllness(list);

                MainPage.setDestinationSource(false);
                this.Frame.Navigate(typeof(IllnessesSearchPage));
            }
        }

        /*Get stored List of Illness */
        public static List<Models.Illness> getIllnesses()
        {
            return illnesses;
        }

        /* Add an unique items in combobox */
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SymptomsComboBox.Items.Count > 0)
            {
                if (SymptomsComboBox.SelectedItem != null &&
                    !SymptChoices.Items.Contains(SymptomsComboBox.SelectedItem))
                {
                    SymptChoices.Items.Add(SymptomsComboBox.SelectedItem);
                }
            }
        }

        /* Delete selected item from list */
        private void SymptChoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                SymptChoices.Items.Remove((sender as ListView).SelectedItem);
            }
        }

        /* Delete all items from list with double tap */
        private void listView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            SymptChoices.Items.Clear();
        }


        /* Select Head area */
        private void HeadClick_Click(object sender, RoutedEventArgs e)
        {
            headSelected = !headSelected;
            Head_img.Visibility = headSelected ? Visibility.Collapsed : Visibility.Visible;
            UpdateComboBoxData();
        }

        /* Select Torso area */
        private void TorsoClicked_Click(object sender, RoutedEventArgs e)
        {
            torsoSelected = !torsoSelected;
            Torso_img.Visibility = torsoSelected ? Visibility.Collapsed : Visibility.Visible;
            UpdateComboBoxData();
        }

        private void PivotLayout_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Contains(NearYouTab))
            {
                StatusBar.Visibility = Visibility.Collapsed;
                
            }
            else
            {
                StatusBar.Visibility = Visibility.Visible;
            }
        }


        /* Select Arms area */
        private void ArmsClicked_Click(object sender, RoutedEventArgs e)
        {
            armsSelected = !armsSelected;
            Arms_img.Visibility = armsSelected ? Visibility.Collapsed : Visibility.Visible;
            UpdateComboBoxData();
        }

        /* Select Legs area */
        private void LegsClicked_Click(object sender, RoutedEventArgs e)
        {
            legsSelected = !legsSelected;
            Legs_img.Visibility = legsSelected ? Visibility.Collapsed : Visibility.Visible;
            UpdateComboBoxData();
        }


        /* Determine of body area and get possible symptoms */
        private void UpdateComboBoxData()
        {
            
            var symptoms = new List<Models.Symptom>();
            if (headSelected)
            {
                symptoms = symptoms.Union(db.getSymptoms("глава")).ToList();
               
            }
            if (torsoSelected)
            {
                symptoms = symptoms.Union(db.getSymptoms("торс")).ToList();

            }
            if (armsSelected)
            {
                symptoms = symptoms.Union(db.getSymptoms("ръце")).ToList();
            }
            if (legsSelected)
            {
                symptoms = symptoms.Union(db.getSymptoms("крака")).ToList();

            }

            symptoms = symptoms.GroupBy(x => x.symptom_name).Select(g => g.First()).ToList();
            SymptomsComboBox.DataContext = symptoms;
        }

    }
}
