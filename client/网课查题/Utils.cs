using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace 网课查题
{
    class Utils
    {
        private static string cookie;
        public static string findAnswer(string question)
        {
            string ramdom = rndNum();
            string encryptedNum = System.Net.WebUtility.UrlEncode(AesEncrypt(ramdom));
            string encryptedQuestion = System.Net.WebUtility.UrlEncode((AesEncrypt(question)));
            string res = Post("http://chati.lxl66.cn/Api.php",
                "question="+encryptedQuestion+
                "&code="+ramdom
                +"&sign="+encryptedNum);
            return res;

        }
        public static void init()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://chati.lxl66.cn/");
            req.Method = "GET";
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

          //  new StreamReader(resp.GetResponseStream()).ReadToEnd();

            cookie = resp.GetResponseHeader("Set-Cookie");


        }
        public static string get(string url)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
        private static string Post(string url, string content)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            byte[] data = Encoding.UTF8.GetBytes(content);
            req.ContentLength = data.Length;
            req. Headers.Add("Cookie", cookie);
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
        private static string key = "woitianxiatancom";
        private static string AesEncrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string rndNum()
        {
            Random random = new Random();
           
            string res = "";
            for(int i=0;i<16;i++)
            {
                res += random.Next(0, 10);
            }
            return res;
        }
    }
}
