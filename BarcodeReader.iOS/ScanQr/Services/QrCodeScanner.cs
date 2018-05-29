using BarcodeReader.ScanQr.Controllers;
using System;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using ZXing;
using ZXing.Mobile;

namespace BarcodeReader.ScanQr.Services
{
    internal class QrCodeScanner
    {
        private IQrCodeScanController _qrCodeScanController;
        private readonly ManualResetEvent _scanResultResetEvent = new ManualResetEvent(false);
        private Action<Result> _continousScanHandler;


        public Task<Result> Scan(MobileBarcodeScanningOptions options)
        {
            //options.TryInverted = true;
            return Task.Factory.StartNew(() =>
            {

                try
                {
                    _scanResultResetEvent.Reset();

                    Result result = null;
                    UIDevice.CurrentDevice.InvokeOnMainThread(() =>
                    {


                        _qrCodeScanController = new QrCodeScanController(options, this);
                        _qrCodeScanController.OnScannedResult += barcodeResult =>
                        {

                            ((UIViewController)_qrCodeScanController).InvokeOnMainThread(() =>
                            {

                                _qrCodeScanController.Cancel();

                                try
                                {
                                    ((UIViewController)_qrCodeScanController).DismissViewController(true, () =>
                                    {
                                        result = barcodeResult;
                                        _scanResultResetEvent.Set();
                                    });
                                }
                                catch (ObjectDisposedException)
                                {
                                    result = barcodeResult;
                                    _scanResultResetEvent.Set();
                                }
                            });
                        };
                        ViewHelper.CurrentController.PresentViewController((UIViewController)_qrCodeScanController, true, null);
                    });

                    _scanResultResetEvent.WaitOne();
                    ((UIViewController)_qrCodeScanController).Dispose();

                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            });

        }

        public void Cancel()
        {
            ((UIViewController)_qrCodeScanController)?.InvokeOnMainThread(() =>
            {
                _qrCodeScanController.Cancel();
                ((UIViewController)_qrCodeScanController).DismissViewController(false, null);
            });

            if (_continousScanHandler != null)
            {
                _continousScanHandler.Invoke(null);
                _continousScanHandler = null;
            }

            _scanResultResetEvent.Set();
        }

        public UIView CustomOverlay { get; set; }
    }
}