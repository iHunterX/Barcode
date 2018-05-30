using System;
using System.Threading.Tasks;
using BarcodeReader.ScanQr.Services;
using CoreGraphics;
using UIKit;
using ZXing.Mobile;
using CoreFoundation;

namespace BarcodeReader.ScanQr.Controllers
{
    internal class QrCodeScanController : UIViewController, IQrCodeScanController
    {
        ZXingScannerView _scannerView;

        public event Action<ZXing.Result> OnScannedResult;

        private MobileBarcodeScanningOptions ScanningOptions { get; }
        public QrCodeScanner Scanner { get; set; }
        public bool ContinuousScanning { get; set; }

        UIActivityIndicatorView _loadingView;
        UIView _loadingBg;

        public UIView CustomLoadingView { get; set; }

        public QrCodeScanController(MobileBarcodeScanningOptions options, QrCodeScanner scanner)
        {
            ScanningOptions = options;
            Scanner = scanner;
        }

        public override void LoadView()
        {
            base.LoadView();

            var appFrame = UIScreen.MainScreen.ApplicationFrame;
            View.Frame = new CGRect(0, 0, appFrame.Width, appFrame.Height);
            View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
        }

        public UIViewController AsViewController()
        {
            return this;
        }

        public void Cancel()
        {
            InvokeOnMainThread(_scannerView.StopScanning);
        }

        UIStatusBarStyle _originalStatusBarStyle = UIStatusBarStyle.Default;

        public override void ViewDidLoad()
        {
            _loadingBg = new UIView(View.Frame) { BackgroundColor = UIColor.Black, AutoresizingMask = UIViewAutoresizing.FlexibleDimensions };
            _loadingView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge)
            {
                AutoresizingMask = UIViewAutoresizing.FlexibleMargins
            };
            _loadingView.Frame = new CGRect((View.Frame.Width - _loadingView.Frame.Width) / 2,
                (View.Frame.Height - _loadingView.Frame.Height) / 2,
                _loadingView.Frame.Width,
                _loadingView.Frame.Height);

            _loadingBg.AddSubview(_loadingView);
            View.AddSubview(_loadingBg);
            _loadingView.StartAnimating();

            _scannerView = new ZXingScannerView(new CGRect(0, 0, View.Frame.Width, View.Frame.Height))
            {
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight,
                UseCustomOverlayView = true,
                CustomOverlayView = Scanner.CustomOverlay
            };
            _scannerView.OnCancelButtonPressed += delegate {
                Scanner.Cancel();
            };

            View.InsertSubviewBelow(_scannerView, _loadingView);

            View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
        }

        public override void ViewDidAppear(bool animated)
        {
            _scannerView.OnScannerSetupComplete += HandleOnScannerSetupComplete;

            _originalStatusBarStyle = UIApplication.SharedApplication.StatusBarStyle;

            if (UIDevice.CurrentDevice.CheckSystemVersion(7, 0))
            {
                UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.Default;
                SetNeedsStatusBarAppearanceUpdate();
            }
            else
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.BlackTranslucent, false);
            Task.Factory.StartNew(() =>
            {
                BeginInvokeOnMainThread(() => _scannerView.StartScanning(result => {
                    Console.WriteLine(result);
                    if (!ContinuousScanning)
                    {
                        _scannerView.StopScanning();
                    }

                    var evt = OnScannedResult;
                    evt?.Invoke(result);
                }, ScanningOptions));
            });
        }

        public override void ViewDidDisappear(bool animated)
        {
            _scannerView?.StopScanning();
            if (_scannerView != null)
                _scannerView.OnScannerSetupComplete -= HandleOnScannerSetupComplete;
        }

        public override void ViewWillDisappear(bool animated)
        {
            UIApplication.SharedApplication.SetStatusBarStyle(_originalStatusBarStyle, false);
        }

        public override bool PrefersStatusBarHidden()
        {
            return true;
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            _scannerView?.DidRotate(InterfaceOrientation);
        }
        public override bool ShouldAutorotate()
        {
            return false;
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.All;
        }


        void HandleOnScannerSetupComplete()
        {
            BeginInvokeOnMainThread(() =>
            {
                if (_loadingView != null && _loadingBg != null && _loadingView.IsAnimating)
                {
                    _loadingView.StopAnimating();

                    UIView.BeginAnimations("zoomout");

                    UIView.SetAnimationDuration(2.0f);
                    UIView.SetAnimationCurve(UIViewAnimationCurve.EaseOut);

                    _loadingBg.Transform = CGAffineTransform.MakeScale(2.0f, 2.0f);
                    _loadingBg.Alpha = 0.0f;

                    UIView.CommitAnimations();

                    _loadingBg.RemoveFromSuperview();
                }
            });
        }
    }
}