using System.ComponentModel;
using System;
using System.Security.Cryptography;
using System.Text;

namespace GroceryStoreEF6.Common
{
    public static class Utilities
    {
        private static IConfigurationRoot Configuration { get; }
        static Utilities()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // requires Microsoft.Extensions.Configuration.Json
                .AddJsonFile("appsettings.json") // requires Microsoft.Extensions.Configuration.Json
                .AddEnvironmentVariables(); // requires Microsoft.Extensions.Configuration.EnvironmentVariables
            Configuration = builder.Build();
        }

        //public static string GetString(string keyName)
        //{
        //    return Configuration[keyName] ?? throw new InvalidOperationException();
        //}

        public static IConfigurationSection GetSection(string keyName)
        {
            return Configuration.GetSection(keyName);
        }

        //public static bool GetBoolean(string keyName)
        //{
        //    return GetString(keyName) == "true";
        //}

        //public static int GetInt(string keyName)
        //{
        //    return int.Parse(GetString(keyName));
        //}

        /// <summary>
        /// Convert a string to an array using the default separator ',' 
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        //public static string[] GetStringArray(string keyName)
        //{
        //    return GetStringArray(keyName, ',');
        //}

        /// <summary>
        /// Convert a string to an array using the separator
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        //public static string[] GetStringArray(string keyName, char separator)
        //{
        //    var stringConfig = GetString(keyName);
        //    return stringConfig.Split(separator);
        //}

        //public static string Encrypt(string text)
        //{
        //    string key = "123!@#gotothemoon@2023";
        //    using (var md5 = new MD5CryptoServiceProvider())
        //    {
        //        using (var tdes = new TripleDESCryptoServiceProvider())
        //        {
        //            tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
        //            tdes.Mode = CipherMode.ECB;
        //            tdes.Padding = PaddingMode.PKCS7;

        //            using (var transform = tdes.CreateEncryptor())
        //            {
        //                byte[] textBytes = UTF8Encoding.UTF8.GetBytes(text);
        //                byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
        //                return Convert.ToBase64String(bytes, 0, bytes.Length);
        //            }
        //        }
        //    }
        //}

        //public static string Decrypt(string cipher)
        //{
        //    string key = "123!@#";
        //    using (var md5 = new MD5CryptoServiceProvider())
        //    {
        //        using (var tdes = new TripleDESCryptoServiceProvider())
        //        {
        //            tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
        //            tdes.Mode = CipherMode.ECB;
        //            tdes.Padding = PaddingMode.PKCS7;

        //            using (var transform = tdes.CreateDecryptor())
        //            {
        //                byte[] cipherBytes = Convert.FromBase64String(cipher);
        //                byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        //                return UTF8Encoding.UTF8.GetString(bytes);
        //            }
        //        }
        //    }   
        //}

        /// <summary>
        /// Format code primary key
        /// Example: HD0001, KH00001
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="length"></param>
        /// <param name="currentcode"></param>
        /// <returns></returns>
        //public static string FormatCode(string prefix, int length, string currentcode)
        //{
        //    string newCode = "";
        //    currentcode = currentcode.Replace(prefix, "");
        //    int plus = int.Parse(currentcode) + 1;
        //    newCode = prefix.PadRight((length - plus.ToString().Length), '0') + plus.ToString();
        //    return newCode;
        //}

        //public static DateTime GetDateTimeSystem()
        //{
        //    return DateTime.Now;
        //}

        //public static string RandomString(int length)
        //{
        //    const string chars = "0123456789";
        //    return new string(Enumerable.Repeat(chars, length)
        //        .Select(s => s[random.Next(s.Length)]).ToArray());
        //}

        //public static string ToDescription(this Enum value)
        //{
        //    DescriptionAttribute[] da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
        //    return da.Length > 0 ? da[0].Description : value.ToString();
        //}

        //public static string ConvertDayOfWeek(string strDate)
        //{
        //    string title = "";
        //    switch (strDate)
        //    {
        //        case "Tuesday":
        //            title = "Thứ ba";
        //            break;
        //        case "Wednesday":
        //            title = "Thứ tư";
        //            break;
        //        case "Thursday":
        //            title = "Thứ năm";
        //            break;
        //        case "Friday":
        //            title = "Thứ sáu";
        //            break;
        //        case "Saturday":
        //            title = "Thứ bảy";
        //            break;
        //        case "Sunday":
        //            title = "Chủ nhật";
        //            break;
        //        default:
        //            title = "Thứ hai";
        //            break;
        //    }
        //    return title;
        //}
    }
}
