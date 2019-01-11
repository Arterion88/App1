using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageTitle : ContentPage
	{
		public PageTitle ()
		{
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent ();
            this.BackgroundColor = Settings.BackgroundColor;
        }

        private void BtnNext_Clicked(object sender, EventArgs e) => Navigation.PushAsync(new MainPage());
    }
}