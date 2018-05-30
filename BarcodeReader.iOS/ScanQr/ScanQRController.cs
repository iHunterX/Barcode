using BarcodeReader.Abstractions.Models;
using BarcodeReader.ScanQr.Services;
using System;
using System.Threading.Tasks;

using ZXing.Mobile;
using ScanQrOverlayView = BarcodeReader.ScanQr.Views.ScanQrOverlayView;

namespace BarcodeReader.iOS.ScanQr.ScanQr
{
    internal static class ScanQrController
    {
        public static void ScanContinuously(
            string message,
            QRCodeScanned onQrCodeScannedFunction,
            TaskCompletionSource<bool> taskCompletionSource,
            string closeButtonTitle,
            bool tryInverted)
        {
            var scanner = new QrCodeScanner();
            var overlayView = ScanQrOverlayView.Create(scanner);
            scanner.CustomOverlay = overlayView;
            overlayView.SetCancelButtonText(closeButtonTitle);
            overlayView.SetDescriptionText(message);
            var position = 0;

            var isScanningContinouslyExecuting = false;
            scanner.ScanContinuously(new MobileBarcodeScanningOptions()
            {
                DelayBetweenContinuousScans = 3000,
                TryInverted = tryInverted
            }, async result =>
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

        public static async Task<string> ScanQrCode(string message, string closeButtonTitle, bool tryInverted)
        {
            var scanner = new QrCodeScanner();
            var overlayView = ScanQrOverlayView.Create(scanner);
            overlayView.SetCancelButtonText(closeButtonTitle);
            overlayView.SetDescriptionText(message);

            scanner.CustomOverlay = overlayView;

            var options = MobileBarcodeScanningOptions.Default;
            options.TryInverted = tryInverted;

            var result = await scanner.Scan(options);

            return result?.Text;
        }
    }
}