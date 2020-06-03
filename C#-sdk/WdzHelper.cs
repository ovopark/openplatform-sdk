using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace DAL.Helper
{
    /// <summary>
    /// 万店掌人脸识别接口
    /// </summary>
    public class WdzHelper
    {
        public static string _akey = ConfigurationManager.AppSettings["_akey"];
        public static string _asid = ConfigurationManager.AppSettings["_asid"];
        public static int orgid = Convert.ToInt32(ConfigurationManager.AppSettings["orgid"]);
        static string aurl = ConfigurationManager.AppSettings["aurl"]; // "http://openapi.ovopark.com/m.api";

        /// <summary>
        /// 发起一个HTTP请求（以POST方式）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string HttpPost(string url, string param)
        {
            string wholeUrl = url + param;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(wholeUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 10000;
            request.AllowAutoRedirect = false;
            HttpWebResponse response = null;
            string responseStr = null;

            request.Method = "POST";
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                Console.WriteLine(response.StatusDescription);
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream, Encoding.UTF8);
                responseStr = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();

            }
            catch (Exception e)
            {

            }
            return responseStr;
        }

        #region 查询分组
        public static GroupResult queryGroup()
        {
            string random = GetTimestamp();
            var objdata = new
            {
                _aid = "S107",
                _akey = _akey,
                _mt = "open.face.queryGroup",
                _requestMode = "post",
                _sm = "md5",
                _timestamp = random,
                _version = "v1",
                orgid = orgid
            };

            string jsonstr = JsonConvert.SerializeObject(objdata);
            string param = JosnToStr(jsonstr);
            string sign = SignatureMD5(Md5Str(jsonstr, _asid));
            param += "&_sig=" + sign;

            string rs = HttpPost(aurl, param);
            GroupResult model = null;
            if (!string.IsNullOrEmpty(rs))
            {
                model = JsonConvert.DeserializeObject<GroupResult>(rs);
            }
            return model;
        }
        #endregion

        #region 添加分组
        public static string addGroup(string groupname)
        {
            string groupid = "0";
            string random = GetTimestamp();
            var objdata = new
            {
                _aid = "S107",
                _akey = _akey,
                _mt = "open.face.addGroup",
                _requestMode = "post",
                _sm = "md5",
                _timestamp = random,
                _version = "v1",
                groupname = groupname,
                orgid = orgid
            };

            string jsonstr = JsonConvert.SerializeObject(objdata);
            string param = JosnToStr(jsonstr);
            string sign = SignatureMD5(Md5Str(jsonstr, _asid));
            param += "&_sig=" + sign;
            string rs = HttpPost(aurl, param);
            new LogHelper().WriteFaceJiekouLog("添加分组" + rs);
            dynamic result = JsonConvert.DeserializeObject(rs);
            if (result.result == "成功")
            {
                groupid = result.data.id.ToString();
            }
            return groupid;

        }
        #endregion

        #region 参数构造连接
        public static string JosnToStr(string datas)
        {
            string rs = "";
            JObject jo = JObject.Parse(datas);
            var t = jo.Properties();//.Select(item => item.Value.ToString()).ToArray();
            int i = 0;
            foreach (var item in t)
            {
                i++;
                string name = item.Name;
                string values = item.Value.ToString();
                if (i == 1)
                {
                    rs += "?" + name + "=" + values;
                }
                else
                {
                    rs += "&" + name + "=" + values;
                }

            }
            return rs;
        }
        #endregion

        #region  时间戳
        public static long GetRandom()
        {
            Random random = new Random();
            return random.Next(999999) % 900000 + 100000;
        }

        public static string GetTimestamp()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }
        #endregion

        #region 签名验签
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="KeyName">KeyName用‘&’</param>
        /// <param name="KeyValue">KeyValue用‘*’</param>
        /// <returns></returns>
        public static string SignatureMD5(string key)
        {
            string MadeMD5 = FormsAuthentication.HashPasswordForStoringInConfigFile(key, "MD5").ToUpper();
            return MadeMD5;
        }
        public static string Md5Str(string datas, string sid)
        {
            string rs = "";
            JObject jo = JObject.Parse(datas);
            var t = jo.Properties();//.Select(item => item.Value.ToString()).ToArray();
            foreach (var item in t)
            {
                string name = item.Name;
                string values = item.Value.ToString();
                rs += name + values;

            }
            if (rs != "")
            {
                rs = sid + rs + sid;
            }
            return rs;
        }

        public static string GetSearchSignMD5(Dictionary<string, string> dc)
        {
            string sign = SignatureMD5(dicMd5str(dc, _asid));
            return sign;
        }

        public static string dicMd5str(Dictionary<string, string> dc, string sid)
        {
            string rs = "";
            foreach (var item in dc)
            {
                var name = item.Key;
                var values = item.Value;
                rs += name + values;
            }
            if (rs != "")
            {
                rs = sid + rs + sid;
            }
            return rs;
        }
        #endregion


    }


    #region 分组业务参数
    public class Group
    {
        public string id { get; set; }
        public string groupname { get; set; }

    }
    public class GroupResult
    {
        /// <summary>
        /// 	接口返回提示信息
        /// </summary>
        public string result { get; set; }
        public List<Group> data { get; set; }

    }
    #endregion

}
