using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Hulk.Shared
{
    public static class StringUtil
    {
        #region Money

        public static string ToMoneyString(this string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            if (!text.IsDouble()) return string.Empty;
            return
                string.Format("{0:0,0} đ", text.Replace(".", string.Empty).Replace(",", string.Empty).ToInt64())
                    .Replace(",", ".");
        }

        #endregion

        #region Encode/Decode

        #region MD5

        /// <summary>
        ///     MD5s the encode.
        /// </summary>
        /// <param name="text">The input.</param>
        /// <returns></returns>
        public static string ToMd5(this string text)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(text));
            var sBuilder = new StringBuilder();
            foreach (var t in data)
                sBuilder.Append(t.ToString("x2"));
            return sBuilder.ToString();
        }

        /// <summary>
        ///     To the MD5.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static string ToMd5(this int number)
        {
            var text = number.ToString(CultureInfo.InvariantCulture);
            return text.ToMd5();
        }

        /// <summary>
        ///     To the MD5.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static string ToMd5(this double number)
        {
            var text = number.ToString(CultureInfo.InvariantCulture);
            return text.ToMd5();
        }

        #endregion

        #region Base64

        /// <summary>
        ///     Decode64s the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string Decode64(this string data)
        {
            if (string.IsNullOrEmpty(data)) return string.Empty;
            var encoder = new UTF8Encoding();
            var utf8Decode = encoder.GetDecoder();

            var todecodeByte = Convert.FromBase64String(data);
            var charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
            var decodedChar = new char[charCount];
            utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);
            return new string(decodedChar);
        }

        /// <summary>
        ///     Encode64s the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string Encode64(this string data)
        {
            if (string.IsNullOrEmpty(data)) return string.Empty;
            var encDataByte = Encoding.UTF8.GetBytes(data);
            var encodedData = Convert.ToBase64String(encDataByte);
            return encodedData;
        }

        #endregion

        #region Encryption, Decryption RSA

        public static string EncodeKey = "31BF3856AD364E35";

        public static string BaseEncrypt(this string text)
        {
            return text.Encrypt(true, EncodeKey);
        }

        public static string BaseDecrypt(this string text)
        {
            return text.Decrypt(true, EncodeKey);
        }

        /// <summary>
        ///     Encrypts the specified to encrypt.
        /// </summary>
        /// <param name="text">To encrypt.</param>
        /// <param name="useHasing">if set to <c>true</c> [use hasing].</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string Encrypt(this string text, bool useHasing, string key)
        {
            byte[] keyArray;
            var toEncryptArray = Encoding.UTF8.GetBytes(text);
            if (useHasing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else keyArray = Encoding.UTF8.GetBytes(key);

            var tDes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            var cTransform = tDes.CreateEncryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tDes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        ///     Decrypts the specified cypher string.
        /// </summary>
        /// <param name="cypherString">The cypher string.</param>
        /// <param name="useHasing">if set to <c>true</c> [use hasing].</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string Decrypt(this string cypherString, bool useHasing, string key)
        {
            byte[] keyArray;
            var toDecryptArray = Convert.FromBase64String(cypherString);
            if (useHasing)
            {
                var hashmd = new MD5CryptoServiceProvider();
                keyArray = hashmd.ComputeHash(Encoding.UTF8.GetBytes(key));
                hashmd.Clear();
            }
            else
            {
                keyArray = Encoding.UTF8.GetBytes(key);
            }
            var tDes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var cTransform = tDes.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
            tDes.Clear();
            return Encoding.UTF8.GetString(resultArray, 0, resultArray.Length);
        }

        #endregion

        #endregion

        #region Random

        /// <summary>
        ///     Randoms the password.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string RandomPassword(int length)
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var chars = new char[length];
            var rd = new Random();

            for (var i = 0; i < length; i++)
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];

            return new string(chars);
        }

        /// <summary>
        ///     Randoms the string.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var chars = new char[length];
            var rd = new Random();

            for (var i = 0; i < length; i++)
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];

            return new string(chars);
        }

        /// <summary>
        ///     Randoms the number.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string RandomNumber(int length)
        {
            const string allowedChars = "0123456789";
            var chars = new char[length];
            var rd = new Random();

            for (var i = 0; i < length; i++)
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];

            return new string(chars);
        }

        /// <summary>
        ///     Automatics the full time string. Format yyyyMMddHHmmssfff
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static string ToFullTimeString(this DateTime date)
        {
            return date.ToString("yyyyMMddHHmmssfff");
        }

        #endregion

        #region Split, Substring, ReplaceFirst the string, Limited

        public static string CleanHtmlTag(this string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            text = Regex.Replace(text, @"</?span( [^>]*|/)?>", string.Empty);
            text = Regex.Replace(text, @"</?a( [^>]*|/)?>", string.Empty);
            text = Regex.Replace(text, @"</?div( [^>]*|/)?>", string.Empty);
            return text;
        }

        public static string CleanHtmlTableTag(this string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            text = Regex.Replace(text, @"</?table( [^>]*|/)?>", string.Empty);
            text = Regex.Replace(text, @"</?tbody( [^>]*|/)?>", string.Empty);
            return text;
        }

        /// <summary>
        ///     Limiteds the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public static string Limited(this string s, int max)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            s = s.Trim();
            return s.Length <= max ? s : string.Format("{0} ...", s.Substring(0, max));
        }

        /// <summary>
        ///     Splits the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public static IList<string> Split(this string s, string start, string end)
        {
            var list = new List<string>();

            if (string.IsNullOrEmpty(s)) return list;

            while (s.IndexOf(start, StringComparison.Ordinal) >= 0 &&
                   s.IndexOf(end, StringComparison.Ordinal) > s.IndexOf(start, StringComparison.Ordinal))
            {
                list.Add(GetNextParameter(s, out s, start, end));
            }

            return list;
        }

        private static string GetNextParameter(string input, out string output, string start, string end)
        {
            var startIndex = input.IndexOf(start, StringComparison.Ordinal);
            var endIndex = input.IndexOf(end, StringComparison.Ordinal);
            var returnStr = input.Substring(startIndex, endIndex - startIndex + end.Length);
            output = ReplaceFirst(input, returnStr, "");
            return returnStr;
        }

        /// <summary>
        ///     Splits the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="find">The find.</param>
        /// <returns></returns>
        public static IList<string> Split(this string s, string find)
        {
            return Regex.Split(s, find).ToList();
        }

        /// <summary>
        ///     Cuts the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="startChar">The start char.</param>
        /// <param name="endChar">The end char.</param>
        /// <returns></returns>
        public static string SubString(this string s, string startChar, string endChar)
        {
            try
            {
                var startIndex = s.IndexOf(startChar, StringComparison.Ordinal) + startChar.Length;
                var endIndex = s.IndexOf(endChar, StringComparison.Ordinal);
                return s.Substring(startIndex, endIndex - startIndex);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///     Replaces the first.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="search">The search.</param>
        /// <param name="replace">The replace.</param>
        /// <returns></returns>
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            var pos = text.IndexOf(search, StringComparison.Ordinal);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        #endregion

        #region Validate Email, Emails, Int32, Int64, Double, Float, Date

        /// <summary>
        ///     Check valid an email. If valid return true.
        /// </summary>
        /// <param name="address">The email address</param>
        /// <returns>
        ///     <c>true</c> if the specified my email is email; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmail(this string address)
        {
            return !string.IsNullOrEmpty(address) &&
                   Regex.IsMatch(address,
                       @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
        }

        /// <summary>
        ///     Check valid many emails address. If valid return true.
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        /// <returns>
        ///     <c>true</c> if the specified addresses is emails; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmails(this string addresses)
        {
            return !string.IsNullOrEmpty(addresses) && addresses.Split(',').All(address => IsEmail(address.Trim()));
        }

        /// <summary>
        ///     Determines whether the specified o is int32.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns>
        ///     <c>true</c> if the specified o is int32; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInt32(this object o)
        {
            try
            {
                int a;
                return int.TryParse(o.ToString(), out a);
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Determines whether the specified automatic is int64.
        /// </summary>
        /// <param name="o">The automatic.</param>
        /// <returns></returns>
        public static bool IsInt64(this object o)
        {
            try
            {
                long a;
                return long.TryParse(o.ToString(), out a);
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Determines whether the specified o is double.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns>
        ///     <c>true</c> if the specified o is double; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDouble(this object o)
        {
            try
            {
                double a;
                return double.TryParse(o.ToString(), out a);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDecimal(this object o)
        {
            try
            {
                decimal a;
                return decimal.TryParse(o.ToString(), out a);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Determines whether the specified o is float.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns>
        ///     <c>true</c> if the specified o is float; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFloat(this object o)
        {
            try
            {
                float a;
                return float.TryParse(o.ToString(), out a);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Determines whether [is date time] [the specified o].
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static bool IsDateTime(this string o)
        {
            if (string.IsNullOrEmpty(o)) return false;

            try
            {
                if (string.IsNullOrEmpty(o))
                    return false;

                var dt = System.DateTime.ParseExact(o, Date, null);
                return dt.CompareTo(System.DateTime.MinValue) > 0;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Determines whether [is date time] [the specified s].
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="pattern">The pattern.</param>
        /// <returns>
        ///     <c>true</c> if [is date time] [the specified s]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDateTime(this string s, string pattern)
        {
            if (string.IsNullOrEmpty(s)) return false;

            try
            {
                var d = System.DateTime.ParseExact(s, pattern, null);
                return d.ToString(pattern).Equals(s);
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Convert/cast to Boolean, Date, DateTime

        public static string DateTime = "dd-MM-yyyy HH:mm";
        public static string DateTimeFull = "dd-MM-yyyy HH:mm:ss:fff";
        public static string Date = "dd-MM-yyyy";

        /// <summary>
        ///     Unixes the time stamp to date time.
        /// </summary>
        /// <param name="unixTimeStamp">The unix time stamp.</param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(this long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        ///     Dates the time to unix timestamp.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static long ToUnixTimestamp(this DateTime value)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            var span = value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();

            //return the total seconds (which is a UNIX timestamp)
            return (long) span.TotalSeconds;
        }

        /// <summary>
        ///     Times the escalation to string.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public static string TimeEscalationToString(this DateTime time)
        {
            var diff = System.DateTime.Now - time;
            //return string.Format("{0} phút", diff.TotalMinutes);
            if (diff.Hours == 0)
            {
                return string.Format("{0}'", diff.Minutes);
            }
            return string.Format("{0}h {1}'", diff.Hours, diff.Minutes);
        }

        /// <summary>
        ///     Automatics the boolean.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static bool ToBoolean(this string text)
        {
            return bool.Parse(text.ToLower());
        }

        /// <summary>
        ///     Automatics the date.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static DateTime ToDate(this string text)
        {
            return text.IsDateTime(Date)
                ? System.DateTime.ParseExact(text, Date, null)
                : System.DateTime.ParseExact("01-01-2000", Date, null);
        }

        /// <summary>
        ///     Automatics the date.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static DateTime ToDate(this string text, string format)
        {
            return text.IsDateTime(format)
                ? System.DateTime.ParseExact(text, format, null)
                : System.DateTime.ParseExact("01-01-2000", Date, null);
        }

        /// <summary>
        ///     Automatics the int32.
        /// </summary>
        /// <param name="s">The arguments.</param>
        /// <returns></returns>
        public static int ToInt32(this string s)
        {
            return int.Parse(s);
        }

        public static decimal ToDecimal(this object s)
        {
            return decimal.Parse(s.ToString());
        }

        /// <summary>
        ///     Automatics the int64.
        /// </summary>
        /// <param name="s">The arguments.</param>
        /// <returns></returns>
        public static long ToInt64(this string s)
        {
            return long.Parse(s);
        }

        /// <summary>
        ///     Clears the viet key.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string ClearVietKey(this string str)
        {
            return str.ClearVietKey(string.Empty);
        }

        public static string ClearVietKey(this string str, string spaceChar)
        {
            var arr = new string[14, 18];
            const string chuoi = "aAeEoOuUiIdDyY";
            const string thga = "áàạảãâấầậẩẫăắằặẳẵ";
            const string hoaA = "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ";
            const string thge = "éèẹẻẽêếềệểễeeeeee";
            const string hoaE = "ÉÈẸẺẼÊẾỀỆỂỄEEEEEE";
            const string thgo = "óòọỏõôốồộổỗơớờợởỡ";
            const string hoaO = "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ";
            const string thgu = "úùụủũưứừựửữuuuuuu";
            const string hoaU = "ÚÙỤỦŨƯỨỪỰỬỮUUUUUU";
            const string thgi = "íìịỉĩiiiiiiiiiiii";
            const string hoaI = "ÍÌỊỈĨIIIIIIIIIIII";
            const string thgd = "đdddddddddddddddd";
            const string hoaD = "ĐDDDDDDDDDDDDDDDD";
            const string thgy = "ýỳỵỷỹyyyyyyyyyyyy";
            const string hoaY = "ÝỲỴỶỸYYYYYYYYYYYY";

            for (byte i = 0; i < 14; i++)
                arr[i, 0] = chuoi.Substring(i, 1);

            for (byte j = 1; j < 18; j++)
                for (byte i = 1; i < 18; i++)
                {
                    arr[0, i] = thga.Substring(i - 1, 1);
                    arr[1, i] = hoaA.Substring(i - 1, 1);
                    arr[2, i] = thge.Substring(i - 1, 1);
                    arr[3, i] = hoaE.Substring(i - 1, 1);
                    arr[4, i] = thgo.Substring(i - 1, 1);
                    arr[5, i] = hoaO.Substring(i - 1, 1);
                    arr[6, i] = thgu.Substring(i - 1, 1);
                    arr[7, i] = hoaU.Substring(i - 1, 1);
                    arr[8, i] = thgi.Substring(i - 1, 1);
                    arr[9, i] = hoaI.Substring(i - 1, 1);
                    arr[10, i] = thgd.Substring(i - 1, 1);
                    arr[11, i] = hoaD.Substring(i - 1, 1);
                    arr[12, i] = thgy.Substring(i - 1, 1);
                    arr[13, i] = hoaY.Substring(i - 1, 1);
                }
            for (byte j = 0; j < 14; j++)
                for (byte i = 1; i < 18; i++)
                    str = str.Replace(arr[j, i], arr[j, 0]);

            var x = new[]
            {
                ";", ")", "(", "=", "+", ",", ".", "{", "}", ":", "/", "\"", "@", "#", "$", "%", "*", "&", " ", "---", "--"
            };
            spaceChar = string.IsNullOrEmpty(spaceChar) ? "-" : spaceChar;
            str = x.Aggregate(str, (current, s) => current.Replace(s, spaceChar));
            if (str.EndsWith("-")) str = str.Substring(0, str.Length - 1);
            return str.ToLower();
        }

        #endregion
    }
}