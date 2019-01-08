using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        List<Stand> stands = new List<Stand>();
        int index = 0;
        int ClearedPoints { get
            {
                int value = 0;
                foreach (Stand item in stands)
                {
                    if (item.Visited)
                        value += item.Points;
                }
                return value;
            } }

        private int GetTotalPoints()
        {

            int value = 0;
            foreach (Stand item in stands)
                value += item.Points;
            return value;
        }

        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            stands.Add(new Stand("Stánek Hobby", "A1 66", "123456"));
            stands.Add(new Stand("Stánek Fox", "C3 12", "654321", 5));
            stands.Add(new Stand("Stánek Mivardi", "B4 35", "mivardi", 2));

                string[] array = Settings.Stands.Split(';');
                for (int i = 0; i < array.Length; i++)
                    if (int.TryParse(array[i], out int result))
                        stands[result].Visited = true;

            Show();
        }

        private void Show()
        {
            btnQR.IsVisible = (index == 1) && Settings.Permission;
            gridPass.IsVisible = index == 1;
            lblTitle.IsVisible = index == 0;
            btnQR.IsVisible = index == 1;
            parent.IsVisible = index == 1;
            btnSubmit.IsVisible = index == 1;

            switch (index)
            {
                case 0:
                    lblTitle2.Text = "Nikdo vám nedá více..";
                    btnCode.Text = "Další";
                    break;
                case 1:
                    lblTitle2.Text = "Celkově bodů: " + ClearedPoints+" / "+ GetTotalPoints();
                    editPass.Text = "";
                    btnCode.Text = "Vložit kód";
                    parent.Children.Clear();  
                    foreach (Stand stand in stands)
                    {
                        parent.Children.Add(stand.GetStackLayout());
                    }       
                    break;
            }
        }

        public void CheckCode(string result)
        {
            editPass.Text = "";
            foreach (Stand stand in stands)
            {
                if (stand.Pass == result)
                {
                    if (stand.Visited == true)
                    {
                        DisplayAlert("Success", "Tento stánek jste již navštívily", "Ok");
                        return;
                    }
                    stand.Visited = true;
                    DisplayAlert("Success", "Stánek úspěšně navštíven", "Ok");
                    Settings.Stands += stands.IndexOf(stand)+";";
                    if (!stands.Exists(x=>!x.Visited))
                    {
                        Submit();
                    }
                    Show();
                    return;
                }
            }
            if (editPass.Text != "")
            {
                DisplayAlert("Chyba", "Špatný kód", "Ok");
            }
            else
                DisplayAlert("Vložte kód", "Napište prosím kód do řádku nad tímto tlačítkem", "Ok");
        }

        private void BtnCode_Clicked(object sender, EventArgs e)
        {
            switch (index)
            {
                default:
                    break;
                case 0:
                    index=1;
                    break;
                case 1:
                    CheckCode(editPass.Text);
                    break;
            }
            Show();
        }

        private async void BtnQR_Clicked(object sender, EventArgs e)
        {

            if(!await TestPermission())
                return;

            await Navigation.PushAsync(new PageScan(this));

            Show();
        }

        private async Task<bool> TestPermission()
        {
            try
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                var status = PermissionStatus.Unknown;
                //Best practice to always check that the key exists
                if (results.ContainsKey(Permission.Camera))
                    status = results[Permission.Camera];
                if (status != PermissionStatus.Granted)
                {
                    Settings.Permission = false;
                    Show();
                    return false;
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Test", ex.Message, "Ok");
                return false;
            }
            return true;
        }

        private void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            Submit();
        }

        private void Submit()
        {
            FinalPage finalPage = new FinalPage(ClearedPoints, GetTotalPoints());
            finalPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.Black);
            Navigation.PushAsync(finalPage);
        }
    }

}
