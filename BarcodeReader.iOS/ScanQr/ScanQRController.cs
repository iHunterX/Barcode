using BarcodeReader.ScanQr.Services;
using System;
using System.Threading.Tasks;

using ZXing.Mobile;
using ScanQrOverlayView = BarcodeReader.ScanQr.Views.ScanQrOverlayView;

namespace BarcodeReader.iOS.ScanQr.ScanQr
{
    internal static class ScanQrController
    {

        public static async Task<string> ScanQrCode(string message, bool tryInverted)
        {
            ZXing.Result result = null;
            var scanner = new QrCodeScanner();
            var overlayView = ScanQrOverlayView.Create(scanner);
            overlayView.SetCancelButtonText("Close");
            overlayView.SetDescriptionText(message);

            scanner.CustomOverlay = overlayView;

            var options = MobileBarcodeScanningOptions.Default;

            result = await scanner.Scan(options);
            

            return result?.Text;
        }
    }
}