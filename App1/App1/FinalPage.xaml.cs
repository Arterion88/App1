using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Web;

namespace App1
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FinalPage : ContentPage
	{
        int Visited;

        public FinalPage (int visited, int total)
		{
            Visited = visited;
            NavigationPage.SetHasNavigationBar(this, false);
            
            InitializeComponent ();
            lbl1.Text = "Gratulujeme"+Environment.NewLine + Environment.NewLine
                        + "Vaše skóre: "+visited+"/"+total + Environment.NewLine + Environment.NewLine
                        + "Pro zařazení do soutěže prosím vyplňte vaší e-mailovou adresu na následující řádek a odešlete.";
		}

        private void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            SendEmail("Test");
            //var emailMessenger = Plugin.Messaging.CrossMessaging.Current.EmailMessenger;
            //if (emailMessenger.CanSendEmail)
            //{
            //    //string msg = "Email: " + editEmail.Text + Environment.NewLine + "Počet bodů: " + Visited;
            //    string msg = "Počet bodů: " + Visited;
            //    emailMessenger.SendEmail("holan@tropicliberec.cz", "Tropic - Soutěž", msg);
            //}
            //else
            //    DisplayAlert("Chyba", "Email není možné odeslat", "Ok");
        }

        private void SendEmail(string msg)
        {
            try
            {
               
            }
            catch (Exception) { }
        }

        private void BtnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

    }
}