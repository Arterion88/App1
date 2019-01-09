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

            stands.Add(new Stand("Hobby", "hobby"));
            stands.Add(new Stand("Fox", "fox", 5));
            stands.Add(new Stand("Mivardi", "mivardi", 2));
            stands.Add(new Stand("Dam", "dam",9));
            stands.Add(new Stand("Egerfish", "egerfish",2));
            stands.Add(new Stand("Saenger", "saenger",7));
            stands.Add(new Stand("Nikl", "nikl",5));
            stands.Add(new Stand("Mikado", "mikado",3));
            stands.Add(new Stand("Mikbaits", "mikbaits",6));
            stands.Add(new Stand("Mivardi", "mivardi",8));
            stands.Add(new Stand("Moss", "moss",4));
            stands.Add(new Stand("Normark", "normark",10));
            stands.Add(new Stand("Stormkloth", "Stormkloth",6));
            stands.Add(new Stand("Slovimex", "slovimex",2));
            stands.Add(new Stand("Svendsen", "svendsen",3));


            string[] array = Settings.Stands.Split(';');
                for (int i = 0; i < array.Length; i++)
                    if (int.TryParse(array[i], out int result))
                        stands[result].Visited = true;

            this.BackgroundColor = Settings.BackgroundColor;
            Show();
        }

        private void Show()
        {
            btnQR.IsVisible = Settings.Permission;

            lblTitle2.Text = "Celkově bodů: " + ClearedPoints + " / " + GetTotalPoints();
            
            parent.Children.Clear();
            foreach (Stand stand in stands)
                parent.Children.Add(stand.GetStackLayout());               
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
