using BarcodeReader.Abstractions;
using System.Threading.Tasks;
using BarcodeReader.iOS.ScanQr.ScanQr;

namespace BarcodeReader
{
    public class BarcodeReaderService : IBarcodeReaderService
    {
        public async Task<string> ScanQRCode(
            string message,
            bool tryInverted = false)
        {
            return await ScanQrController.ScanQrCode(message, tryInverted);
        }
    }
}
