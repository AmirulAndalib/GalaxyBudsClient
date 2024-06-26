﻿using InTheHand.Net;

namespace GalaxyBudsClient.Bluetooth.Windows
{
    internal static class MacUtils
    {
        internal static BluetoothAddress? ToAddress(string mac)
        {
            var isValid = BluetoothAddress.TryParse(mac, out var addr);

            if (!isValid)
            {
                return null;
            }

            return addr;
        }
    }
}
