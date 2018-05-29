using System.Threading.Tasks;
using Android.App;
using BarcodeReader.ScanQr.Services;
using BarcodeReader.ScanQr.Views;
using ZXing.Mobile;

namespace BarcodeReader
{
    internal class ScanQrController
    {
        public static async Task<string> ScanQRCode(string message, Activity activity, bool tryInverted = false)
        {
            BarcodeScanner.Initialize(activity.Application);

            var scanner = new BarcodeScanner();
            var overlayView = new ScanQrOverlayView(activity);
            scanner.CustomOverlay = overlayView;
            overlayView.SetDescriptionText(message);
            overlayView.SetCloseButtonText("Close");

            var options = MobileBarcodeScanningOptions.Default;
            options.TryInverted = tryInverted;
            var result = await scanner.Scan(options);

            return result?.Text;
        }
    }
}