using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectPage : ContentPage
	{
        List<Event> events = new List<Event>();

		public SelectPage ()
		{
			InitializeComponent ();
            DownloadFileFTP();
		}

        private void DownloadFileFTP()
        {
            string serverPath = "ftp://speedtest.tele2.net/"; //TODO: Change Server name

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(serverPath); 

            request.KeepAlive = true;
            request.UsePassive = true;
            request.UseBinary = true;

            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential("demo", "password"); //TODO: Change login and pass

            // Read the file from the server & write to destination                
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse()) // Error here
            using (Stream responseStream = response.GetResponseStream())
                ProcessFile(responseStream);
        }

        private async void ProcessFile(Stream content)
        {
            XmlDocument doc = new XmlDocument();
            using (StreamReader reader = new StreamReader(content))
                doc.LoadXml(await reader.ReadToEndAsync());

            foreach (XmlNode node in doc.GetElementsByTagName("Event"))
            {
                Event event1 = new Event(node);
                if (event1.From < DateTime.Today && event1.To > DateTime.Today)
                    if (Settings.FinishedEvents.Split(';').ToList().Contains(event1.Id.ToString()))
                        events.Add(event1);


            }


        }
    }
}