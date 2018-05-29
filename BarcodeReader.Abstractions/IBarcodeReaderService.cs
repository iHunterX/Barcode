using System.Threading.Tasks;

namespace BarcodeReader.Abstractions
{
    public interface IBarcodeReaderService
    {
        Task<string> ScanQRCode(string message, bool tryInverted = false);
    }
}
