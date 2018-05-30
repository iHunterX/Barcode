using BarcodeReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestBarcode
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();            
		}

        private async void QRScan_OnClicked(object sender, EventArgs e)
        {
            var qrcode = await CrossBarcodeReaderService.Current.ScanQRCode("Align the QR Code at the screen center.");
            
            await DisplayAlert("Result", qrcode, "OK");
        }
        
    }
}
