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
using System.Security.Cryptography;
using System.IO;

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
            this.BackgroundColor = Settings.BackgroundColor;
		}

        private void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            var emailMessenger = Plugin.Messaging.CrossMessaging.Current.EmailMessenger;
            if (emailMessenger.CanSendEmail)
            {
                //string msg = "Email: " + editEmail.Text + Environment.NewLine + "Počet bodů: " + Visited;
                string msg = "Počet bodů: " + Encrypt(Visited.ToString());
                emailMessenger.SendEmail("holan@tropicliberec.cz", "Tropic - Soutěž", msg);
            }
            else
                DisplayAlert("Chyba", "Email není možné odeslat", "Ok");
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
    }
}