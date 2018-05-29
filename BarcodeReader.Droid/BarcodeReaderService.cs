using BarcodeReader.Abstractions;
using System.Threading.Tasks;

namespace BarcodeReader
{
    public class BarcodeReaderService : IBarcodeReaderService
    {
        public async Task<string> ScanQRCode(string message, bool tryInverted = false)
        {
            return await ScanQrController.ScanQRCode(message, ViewHelper.CurrentActivity, tryInverted);
        }
    }
}
