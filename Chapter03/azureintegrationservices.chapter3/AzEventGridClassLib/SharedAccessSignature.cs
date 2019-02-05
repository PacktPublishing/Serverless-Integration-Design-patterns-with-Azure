using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AzEventGridClassLib
{
  public  class SharedAccessSignature
    {
        public static  string GenerateSharedAccessSignature(string resource, DateTime expirationtime, string key)
        {
            const char Resource = 'r';
            const char Expiration = 'e';
            const char Signature = 's';

            string encodedResource = HttpUtility.UrlEncode(resource);
            var culture = CultureInfo.CreateSpecificCulture("en-US");

            var encodedExpirationUtc = HttpUtility.UrlEncode(expirationtime.ToString(culture));
            string unsignedSas = $"{Resource}={encodedResource}&{Expiration}={encodedExpirationUtc}";
            using (var hmac = new HMACSHA256(Convert.FromBase64String(key)))
            {
                string signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(unsignedSas)));
                string encodedSignature = HttpUtility.UrlEncode(signature);
                string signedSas =  $"{unsignedSas}&{Signature}={encodedSignature}";

                return signedSas;
            }
        }
    }
}
