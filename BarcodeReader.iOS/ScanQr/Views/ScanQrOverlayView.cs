using System;
using BarcodeReader.ScanQr.Services;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace BarcodeReader.ScanQr.Views
{
    internal partial class ScanQrOverlayView : UIView
    {
        private static readonly NSString Key = new NSString(nameof(ScanQrOverlayView));

        public static ScanQrOverlayView Create(QrCodeScanner scanner)
        {

            var viewArrays = NSBundle.MainBundle.LoadNib(Key, null, null);
            var scanQrOverlayView = Runtime.GetNSObject<ScanQrOverlayView>(viewArrays.ValueAt(0));

            scanQrOverlayView.SetScanner(scanner);

            return scanQrOverlayView;
        }
        
        private QrCodeScanner _scanner;

        public ScanQrOverlayView (IntPtr handle) : base (handle)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            DescriptionLabel.Text = string.Empty;
            CreatShadowOnWhiteBackground(DescriptionLabel.Layer);
            CreatShadowOnWhiteBackground(CancelButton.TitleLabel.Layer);

            CenterSquareView.Layer.CornerRadius = 6;
            CenterSquareView.Layer.BorderColor = UIColor.White.CGColor;
            CenterSquareView.Layer.BorderWidth = 1;
        }

        private void CreatShadowOnWhiteBackground(CoreAnimation.CALayer layer)
        {
            layer.ShadowColor = UIColor.DarkGray.CGColor;
            layer.ShadowOffset = new CoreGraphics.CGSize(1, 2);
            layer.ShadowOpacity = 1;
            layer.ShadowRadius = 1;
        }

        public void SetScanner(QrCodeScanner scanner)
        {
            _scanner = scanner;
        }

        public void SetCancelButtonText(string cancelButtonText)
        {
            BeginInvokeOnMainThread(() => CancelButton.SetTitle(cancelButtonText, UIControlState.Normal));
        }

        public void SetDescriptionText(string descriptionText)
        {
            BeginInvokeOnMainThread(() => DescriptionLabel.Text = descriptionText);
        }

        partial void OnCancelButton(UIButton sender)
        {
            _scanner.Cancel();
        }
    }
}