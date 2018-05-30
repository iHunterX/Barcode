using System.Threading.Tasks;
using Android.App;
using BarcodeReader.Abstractions.Models;
using BarcodeReader.ScanQr.Services;
using BarcodeReader.ScanQr.Views;
using ZXing.Mobile;

namespace BarcodeReader
{
    internal class ScanQrController
    {
        public static void ScanContinuously(
            Activity activity,
            string message,
            string closeButtonTitle,
            QRCodeScanned onQrCodeScannedFunction,
            TaskCompletionSource<bool> taskCompletionSource,
            bool tryInverted)
        {
            BarcodeScanner.Initialize(activity.Application);

            var scanner = new BarcodeScanner();
            var overlayView = new ScanQrOverlayView(activity);
            scanner.CustomOverlay = overlayView;
            overlayView.SetDescriptionText(message);
            overlayView.SetCloseButtonText(closeButtonTitle);
            var position = 0;

            bool isScanningContinouslyExecuting = false;
            scanner.ScanContinuously(new MobileBarcodeScanningOptions()
            {
                DelayBetweenContinuousScans = 3000,
                TryInverted = tryInverted
            },
            async result =>
            {
                if (isScanningContinouslyExecuting) return;
                isScanningContinouslyExecuting = true;

                var scanQrUpdate = await onQrCodeScannedFunction.Invoke(position, result.Text);

                if (scanQrUpdate == null) return;

                if (scanQrUpdate.WillCancel)
                {
                    taskCompletionSource.SetResult(true);
                    taskCompletionSource = null;
                    scanner.Cancel();
                    return;
                }

                overlayView.SetDescriptionText(scanQrUpdate.UpdateMessage);
                position++;

                isScanningContinouslyExecuting = false;
            });
        }

        public static async Task<string> ScanQRCode(string message, string closeButtonTitle, Activity activity, bool tryInverted = false)
        {
            BarcodeScanner.Initialize(activity.Application);

            var scanner = new BarcodeScanner();
            var overlayView = new ScanQrOverlayView(activity);
            scanner.CustomOverlay = overlayView;
            overlayView.SetDescriptionText(message);
            overlayView.SetCloseButtonText(closeButtonTitle);

            var options = MobileBarcodeScanningOptions.Default;
            options.TryInverted = tryInverted;
            var result = await scanner.Scan(options);

            return result?.Text;
        }
    }
}