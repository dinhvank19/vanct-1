using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace POS.Shared
{
    public static class StringHelper
    {
        #region Cutter

        /// <summary>
        /// Clears the viet key.
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

            return spaceChar.Equals(" ") ? str : str.Replace(" ", spaceChar);
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
        /// SQLs the injection.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string SqlInjection(this string str)
        {
            return string.IsNullOrEmpty(str)
                ? string.Empty
                : str.Replace("'", string.Empty).Replace("%", string.Empty).Replace("\"", string.Empty);
        }

        #endregion

        #region Parser

        public static string ToMoney(this double money)
        {
            return money == 0 ? string.Empty : string.Format("{0:0,0}", money);
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
        ///     Automatics the int32.
        /// </summary>
        /// <param name="s">The arguments.</param>
        /// <returns></returns>
        public static int ToInt32(this string s)
        {
            return int.Parse(s);
        }

        /// <summary>
        ///     Automatics the double.
        /// </summary>
        /// <param name="s">The arguments.</param>
        /// <returns></returns>
        public static long ToInt64(this string s)
        {
            return long.Parse(s);
        }

        /// <summary>
        ///     Automatics the double.
        /// </summary>
        /// <param name="s">The arguments.</param>
        /// <returns></returns>
        public static double ToDouble(this string s)
        {
            return double.Parse(s);
        }

        /// <summary>
        ///     Unixes the time stamp to date time.
        /// </summary>
        /// <param name="unixTimeStamp">The unix time stamp.</param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(this Int64 unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Dates the time to unix timestamp.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Int64 DateTimeToUnixTimestamp(this DateTime value)
        {
            var span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (Int64) span.TotalSeconds;
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
            if (String.IsNullOrEmpty(data)) return String.Empty;
            var encoder = new UTF8Encoding();
            var utf8Decode = encoder.GetDecoder();

            var todecodeByte = Convert.FromBase64String(data);
            var charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
            var decodedChar = new char[charCount];
            utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);
            return new String(decodedChar);
        }

        /// <summary>
        ///     Encode64s the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string Encode64(this string data)
        {
            if (String.IsNullOrEmpty(data)) return String.Empty;
            var encDataByte = Encoding.UTF8.GetBytes(data);
            var encodedData = Convert.ToBase64String(encDataByte);
            return encodedData;
        }

        #endregion

        #region Encryption, Decryption RSA

        public static string BaseEncrypt(this string text, string key)
        {
            return text.Encrypt(true, key);
        }

        public static string BaseDecrypt(this string text, string key)
        {
            return text.Decrypt(true, key);
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

        #region Validation

        /// <summary>
        /// Determines whether this instance is json.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static bool IsJson(this string text)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<JObject>(text);
                return result != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Check valid an email. If valid return true.
        /// </summary>
        /// <param name="address">The email address</param>
        /// <returns>
        ///     <c>true</c> if the specified my email is email; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmail(this string address)
        {
            return !String.IsNullOrEmpty(address) &&
                   Regex.IsMatch(address,
                       @"^[_A-Za-z0-9-]+(\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+([A-Za-z0-9-]+)*(\.[A-Za-z]{2,4})*(\.[A-Za-z]{2,4})$");
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
            return !String.IsNullOrEmpty(addresses) && addresses.Split(',').All(address => IsEmail(address.Trim()));
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
                return Int32.TryParse(o.ToString(), out a);
            }
            catch (Exception)
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
            catch (Exception)
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
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Times the escalation to string.
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

        #endregion
    }
}