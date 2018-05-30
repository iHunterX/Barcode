using BarcodeReader.Abstractions.Models;
using System.Threading.Tasks;

namespace BarcodeReader.Abstractions
{
    public interface IBarcodeReaderService
    {
        Task<string> ScanQRCode(string message, string closeButtonTitle = "Close", bool tryInverted = false);

        Task<bool> ScanQRCodeContinously(string message, QRCodeScanned onQrCodeScannedFunction, string closeButtonTitle = "Close", bool tryInverted = false);
    }
}
