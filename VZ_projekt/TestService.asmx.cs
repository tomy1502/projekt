using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.ClientServices.Providers;
using System.Web.Management;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Security.Cryptography;

namespace VZ_projekt
{
    /// <summary>
    /// Summary description for TestService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TestService : System.Web.Services.WebService
    {
        public AuthHeader Credentials = new AuthHeader();

        string pravilniUserName = "tomas";
        string pravilniGeslo = "tomas";

        [SoapHeader("Credentials")]

        [WebMethod]
        public string Login(string username, string password)
        {
            Credentials.UserName = username;
            Credentials.Password = Credentials.HashPassword(password);

            string tmpGeslo = Credentials.HashPassword(pravilniGeslo);


            if (Credentials.UserName != pravilniUserName ||
            Credentials.Password != tmpGeslo)
            {
                return "Neuspešna prijava";
            }

            else

                return "Uspešna prijava";
        }

        public class AuthHeader : SoapHeader
        {
            public string UserName { get; set; }
            public string Password { get; set; }


            public string HashPassword(string password)
            {
                string geslo = EncryptPassword(password);
                string nazajGeslo = DecryptPassword(geslo,privateKey1);

                return nazajGeslo;

            }

            public static RSAParameters privateKey1;
            public static string EncryptPassword(string password)
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    RSAParameters publicKey = rsa.ExportParameters(false);
                    RSAParameters privateKey = rsa.ExportParameters(true);
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                    byte[] encryptedBytes = rsa.Encrypt(passwordBytes, true);
                    string encryptedPassword = Convert.ToBase64String(encryptedBytes);

                    privateKey1 = privateKey;

                    return encryptedPassword;
                }
            }

            public static string DecryptPassword(string encryptedPassword, RSAParameters privateKey)
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {

                    rsa.ImportParameters(privateKey);
                    byte[] encryptedBytes = Convert.FromBase64String(encryptedPassword);
                    byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, true);
                    string decryptedPassword = Encoding.UTF8.GetString(decryptedBytes);

                    return decryptedPassword;
                }
            }




        }

    }
}
