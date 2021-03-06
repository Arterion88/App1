﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageScan : ContentPage
    {
        ZXingScannerView zxing;
        ZXingDefaultOverlay overlay;

        public PageScan(MainPage mainPage) : base()
        {
            zxing = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                AutomationId = "zxingScannerView",
            };
            zxing.OnScanResult += (result) =>
            {
                zxing.Unfocus();
                zxing.IsScanning = false;
                zxing.IsAnalyzing = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    mainPage.CheckCode(result.Text);
                });
            };

            overlay = new ZXingDefaultOverlay
            {
                TopText = "Přiložte váš telefon ke QR kódu",
                BottomText = "Skenování se provede automaticky",
                ShowFlashButton = zxing.HasTorch,
                AutomationId = "zxingDefaultOverlay",
            };

            overlay.FlashButtonClicked += (sender, e) => {
                zxing.IsTorchOn = !zxing.IsTorchOn;
            };
            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            grid.Children.Add(zxing);
            grid.Children.Add(overlay);

            // The root page of your application
            Content = grid;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            zxing.IsScanning = true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            zxing.IsScanning = false;
        }
    }
}