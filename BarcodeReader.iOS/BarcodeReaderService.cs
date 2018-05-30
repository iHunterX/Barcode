using BarcodeReader.Abstractions;
using System.Threading.Tasks;
using BarcodeReader.iOS.ScanQr.ScanQr;
using BarcodeReader.Abstractions.Models;

namespace BarcodeReader
{
    public class BarcodeReaderService : IBarcodeReaderService
    {

        public async Task<string> ScanQRCode(string message, string closeButtonTitle = "Close", bool tryInverted = false)
        {
            return await ScanQrController.ScanQrCode(message, closeButtonTitle, tryInverted);
        }

        public Task<bool> ScanQRCodeContinously(string message, QRCodeScanned onQrCodeScannedFunction, string closeButtonTitle = "Close", bool tryInverted = false)
        {
            var taskCompleteSource = new TaskCompletionSource<bool>();
            ScanQrController.ScanContinuously(message, onQrCodeScannedFunction, taskCompleteSource, closeButtonTitle, tryInverted);

            return taskCompleteSource.Task;
        }
    }
}
