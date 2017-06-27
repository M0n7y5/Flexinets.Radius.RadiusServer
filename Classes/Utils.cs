﻿using System;
using System.Text;

namespace Flexinets.Radius
{
    public static class Utils
    {
        public static Byte[] StringToByteArray(String hex)
        {
            Int32 NumberChars = hex.Length;
            Byte[] bytes = new Byte[NumberChars / 2];
            for (var i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        public static String ByteArrayToString(Byte[] bytes)
        {
            var hex = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }


        /// <summary>
        /// Get the mccmnc as a string from a 3GPP-User-Location-Info vendor attribute. SAI and CGI only for now
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static String GetMccMncFrom3GPPLocationInfo(Byte[] bytes)
        {
            String mccmnc = null;
            var type = (LocationType)bytes[0];  // hmm...
            if (type == LocationType.CGI || type == LocationType.ECGI || type == LocationType.RAI || type == LocationType.SAI || type == LocationType.TAI)
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
            return mccmnc;
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
