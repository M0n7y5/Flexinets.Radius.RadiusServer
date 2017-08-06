﻿using log4net;
using System;
using System.Text;

namespace Flexinets.Radius
{
    public static class Utils
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Utils));


        /// <summary>
        /// Convert a string of hex encoded bytes to a byte array
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static Byte[] StringToByteArray(String hex)
        {
            var NumberChars = hex.Length;
            var bytes = new Byte[NumberChars / 2];
            for (var i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }


        /// <summary>
        /// Convert a byte array to a string of hex encoded bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static String ToHexString(this Byte[] bytes)
        {
            var hex = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                hex.Append($"{b:x2}");
            }
            return hex.ToString();
        }


        /// <summary>
        /// Get the mccmnc as a string from a 3GPP-User-Location-Info vendor attribute.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static (LocationType locationType, String mccmnc) GetMccMncFrom3GPPLocationInfo(Byte[] bytes)
        {
            String mccmnc = null;
            var type = (LocationType)bytes[0];  // hmm...
            if (type == LocationType.CGI || type == LocationType.ECGI || type == LocationType.RAI || type == LocationType.SAI || type == LocationType.TAI || type == LocationType.TAIAndECGI)
            {
                var mccDigit1 = (bytes[1] & 15).ToString();
                var mccDigit2 = ((bytes[1] & 240) >> 4).ToString();
                var mccDigit3 = (bytes[2] & 15).ToString();

                var mncDigit1 = (bytes[3] & 15).ToString();
                var mncDigit2 = ((bytes[3] >> 4)).ToString();
                var mncDigit3 = (bytes[2] >> 4).ToString();

                mccmnc = mccDigit1 + mccDigit2 + mccDigit3 + mncDigit1 + mncDigit2;
                if (mncDigit3 != "15")
                {
                    mccmnc = mccmnc + mncDigit3;
                }
            }
            else
            {
                _log.Error($"Unable to parse mccmnc from location attribute {bytes.ToHexString()}");
            }

            return (type, mccmnc);
        }


        /// <summary>
        /// 3GPP location types
        /// </summary>
        public enum LocationType
        {
            CGI = 0,
            SAI = 1,
            RAI = 2,
            TAI = 128,
            ECGI = 129,
            TAIAndECGI = 130
        }
    }
}