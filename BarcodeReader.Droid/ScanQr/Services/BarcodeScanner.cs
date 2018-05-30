using System;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using ZXing.Mobile;
using ZXing;

namespace BarcodeReader.ScanQr.Services
{
    internal class BarcodeScanner
    {
        static ActivityLifecycleContextListener _lifecycleListener;
        private Action<Result> _continouslyScanHandler;

        public static void Initialize(Android.App.Application app)
        {
            var version = Build.VERSION.SdkInt;

            if (version >= BuildVersionCodes.IceCreamSandwich)
            {
                _lifecycleListener = new ActivityLifecycleContextListener();
                app.RegisterActivityLifecycleCallbacks(_lifecycleListener);
            }
        }

        public static void Uninitialize(Android.App.Application app)
        {
            var version = Build.VERSION.SdkInt;

            if (version >= BuildVersionCodes.IceCreamSandwich)
                app.UnregisterActivityLifecycleCallbacks(_lifecycleListener);
        }

        public Android.Views.View CustomOverlay { get; set; }

        Context GetContext(Context context)
        {
            if (context != null)
                return context;

            var version = Build.VERSION.SdkInt;

            if (version >= BuildVersionCodes.IceCreamSandwich)
                return _lifecycleListener.Context;
            else
                return Android.App.Application.Context;
        }

        public void ScanContinuously(MobileBarcodeScanningOptions options, Action<Result> scanHandler)
        {
            ScanContinuously(null, options, scanHandler);
        }

        public void ScanContinuously(Context context, MobileBarcodeScanningOptions options, Action<Result> scanHandler)
        {
            _continouslyScanHandler = scanHandler;
            var suitableContext = GetContext(context);
            var scanIntent = new Intent(suitableContext, typeof(ZxingActivity));

            scanIntent.AddFlags(ActivityFlags.NewTask);

            ZxingActivity.UseCustomOverlayView = true;
            ZxingActivity.CustomOverlayView = CustomOverlay;
            ZxingActivity.ScanningOptions = options;
            ZxingActivity.ScanContinuously = true;

            ZxingActivity.CanceledHandler = () =>
            {
                _continouslyScanHandler?.Invoke(null);
            };

            ZxingActivity.ScanCompletedHandler = result =>
            {
                scanHandler?.Invoke(result);
            };

            suitableContext.StartActivity(scanIntent);
        }

        public Task<Result> Scan(MobileBarcodeScanningOptions options)
        {
            return Scan(null, options);
        }
        public Task<Result> Scan(Context context, MobileBarcodeScanningOptions options)
        {
            var ctx = GetContext(context);

            var task = Task.Factory.StartNew(() => {

                var waitScanResetEvent = new System.Threading.ManualResetEvent(false);

                var scanIntent = new Intent(ctx, typeof(ZxingActivity));

                scanIntent.AddFlags(ActivityFlags.NewTask);

                ZxingActivity.UseCustomOverlayView = true;
                ZxingActivity.CustomOverlayView = CustomOverlay;
                ZxingActivity.ScanningOptions = options;
                ZxingActivity.ScanContinuously = false;

                Result scanResult = null;

                ZxingActivity.CanceledHandler = () =>
                {
                    waitScanResetEvent.Set();
                };

                ZxingActivity.ScanCompletedHandler = result =>
                {
                    scanResult = result;
                    waitScanResetEvent.Set();
                };

                ctx.StartActivity(scanIntent);

                waitScanResetEvent.WaitOne();

                return scanResult;
            });

            return task;
        }

        public void Cancel()
        {
            ZxingActivity.RequestCancel();
        }
    }
}