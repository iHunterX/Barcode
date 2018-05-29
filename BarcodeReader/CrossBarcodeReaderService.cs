using BarcodeReader.Abstractions;
using System;

namespace BarcodeReader
{
    public class CrossBarcodeReaderService
    {
        private static readonly Lazy<IBarcodeReaderService> mediaService = new Lazy<IBarcodeReaderService>(CreateBarcodeReaderService, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use 
        /// </summary>
        public static IBarcodeReaderService Current
        {
            get
            {
                var ret = mediaService.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static IBarcodeReaderService CreateBarcodeReaderService()
        {
#if NETSTANDARD2_0
            return null;
#else
            return new BarcodeReaderService();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly. You should reference the Amaris.Mobile.Services.Media NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
