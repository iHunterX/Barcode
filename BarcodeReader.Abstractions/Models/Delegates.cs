using System.Threading.Tasks;

namespace BarcodeReader.Abstractions.Models
{
    public delegate Task<ScanQrUpdate> QRCodeScanned(int id, string qrCodeContent);
}
