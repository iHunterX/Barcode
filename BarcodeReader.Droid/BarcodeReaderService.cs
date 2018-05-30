﻿using BarcodeReader.Abstractions;
using BarcodeReader.Abstractions.Models;
using System.Threading.Tasks;

namespace BarcodeReader
{
    public class BarcodeReaderService : IBarcodeReaderService
    {
        public async Task<string> ScanQRCode(string message, string closeButtonTitle = "Close", bool tryInverted = false)
        {
            return await ScanQrController.ScanQRCode(message,closeButtonTitle,ViewHelper.CurrentActivity, tryInverted);
        }

        public Task<bool> ScanQRCodeContinously(string message, QRCodeScanned onQrCodeScannedFunction, string closeButtonTitle = "Close", bool tryInverted = false)
        {
            var taskCompleteSource = new TaskCompletionSource<bool>();
            ScanQrController.ScanContinuously(ViewHelper.CurrentActivity, message, closeButtonTitle, onQrCodeScannedFunction, taskCompleteSource, tryInverted);

            return taskCompleteSource.Task;
        }
    }
}
