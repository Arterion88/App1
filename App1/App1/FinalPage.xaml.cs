using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using MailKit.Net.Imap;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;
using System.ComponentModel;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;


namespace App1
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FinalPage : ContentPage
	{
        int Visited;

        protected override void OnAppearing()
        {
            if (Settings.FinishedEvents.Split(';').ToList().Contains(Settings.CurrentEvent.Id.ToString()))
                Navigation.PushAsync(new SelectPage());
            base.OnAppearing();

        }

        public FinalPage (int visited, int total)
		{
            Visited = visited;
            NavigationPage.SetHasNavigationBar(this, false);
            
            InitializeComponent ();
            lbl1.Text = "Gratulujeme"+Environment.NewLine + Environment.NewLine
                        + "Vaše skóre: "+visited+"/"+total + Environment.NewLine + Environment.NewLine
                        + "Pro zařazení do soutěže prosím vyplňte informace na následující řádek a odešlete.";
            this.BackgroundColor = Settings.BackgroundColor;
            
		}

        private void Edit_Focused(object sender, FocusEventArgs e) => ((Entry)sender).BackgroundColor = Color.Default;

        bool IsValidEmail(string email)
        {
            try
            {
                System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            //if (FormValidation())
            //    return;
            string msg = "Jméno: " + editName.Text + Environment.NewLine +
                             "Příjmení: " + editName2.Text + Environment.NewLine + 
                             "Email: "+editMail.Text+Environment.NewLine+
                             "Telefon: "+editPhone.Text+Environment.NewLine+
                             "Počet bodů: "+Visited;
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("tropicliberec.h@gmail.com"));
            message.To.Add(new MailboxAddress("holan@tropicliberec.cz"));
            message.Subject = "Tropic - Soutěž";

            message.Body = new TextPart("plain")
            {
                Text = msg
            };

            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587);
                    client.SslProtocols = System.Security.Authentication.SslProtocols.Default;

                    client.Authenticate("tropicliberec.h@gmail.com", "tropic213021");

                    client.Send(message);
                    client.Disconnect(true);
                }
                //DisplayAlert("", "", "Ok");

                Settings.FinishedEvents += Settings.CurrentEvent.Id.ToString() + ";";
                Navigation.PushAsync(new SelectPage());


            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "Ok");
                return;
            }

        }

        private bool FormValidation()
        {
            editMail.BackgroundColor = editMail2.BackgroundColor = editName.BackgroundColor = editName2.BackgroundColor = Color.Default;

            List<View> arrEntry = gridFrm.Children.Where(x => x.GetType() == typeof(Entry)).ToList();
            arrEntry.Remove(editPhone);
            bool missing = false;
            foreach (View view in arrEntry)
            {
                Entry entry = (Entry)view;

                if (!string.IsNullOrWhiteSpace(entry.Text))
                    continue;
                entry.BackgroundColor = Color.Red;
                missing = true;
            }
            if (missing)
            {
                DisplayAlert("Chybějící údaje","Vyplňte prosím chybějící údaje!","Ok");
                return true;
            }

            if(editMail.Text != editMail2.Text)
            {
                DisplayAlert("Neschodné emaily", "Email se neschoduje s emailem zadaným pro potvrzení. Překontrolujte si prosím vložené emaily", "Ok");
                return true;
            }

            return false;
        }

        private void SendEmail(string msg)
        {
            

        }

        private void BtnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e) => btnSubmit.IsEnabled = ((Xamarin.Forms.Switch)sender).IsToggled;

        #region Encryption
        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";

        public static string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }
        #endregion
    }
}