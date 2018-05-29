using System;
using System.Collections.Generic;
using System.Text;

namespace BarcodeReader.Abstractions.Models
{
    public class ScanQrUpdate
    {
        public string UpdateMessage { get; set; }
        public bool WillCancel { get; set; }
    }
}
