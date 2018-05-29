using System;
using Android.App;
using Android.Widget;

namespace BarcodeReader.ScanQr.Views
{
    internal class ScanQrOverlayView : FrameLayout
    {
        private TextView _descriptionTextView;
        private Button _closeButton;

        public ScanQrOverlayView(Activity activity) : base(activity)
        {
            Init();
        }

        private void Init()
        {
            Inflate(Context, Resource.Layout.scan_qr_overlay_layout, this);
            _descriptionTextView = FindViewById<TextView>(Resource.Id.description_text);
            _closeButton = FindViewById<Button>(Resource.Id.close_button);
            _closeButton.Click += CloseButtonOnClick;
        }

        private void CloseButtonOnClick(object sender, EventArgs eventArgs)
        {
            ViewHelper.CurrentActivity.Finish();
        }

        public void SetCloseButtonText(string closeButtonText)
        {
            ViewHelper.CurrentActivity.RunOnUiThread(() => _closeButton.Text = closeButtonText);
        }

        public void SetDescriptionText(string descriptionText)
        {
            ViewHelper.CurrentActivity.RunOnUiThread(() => _descriptionTextView.Text = descriptionText);
        }
    }
}