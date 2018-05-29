using BarcodeReader.ScanQr.Services;
using System;
using UIKit;

namespace BarcodeReader.ScanQr.Controllers
{
    internal interface IQrCodeScanController
    {
        void Cancel();
        //bool ContinuousScanning { get; set; }

        event Action<ZXing.Result> OnScannedResult;
        QrCodeScanner Scanner { get; set; }
        UIViewController AsViewController();
    }
}