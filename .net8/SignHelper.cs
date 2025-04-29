using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ovopark.net.Controllers
{
    /// <summary>
    /// Example of ovopark signature method request1
    /// </summary>
    public class SignHelper
    {
        public static string _asid = "nudiwanduinudnua";
        public static string apigwUrl = "http://openapi.ovopark.com/aaaaaaa.api";
        public static string _aid = "a111";
        public static string _akey = "a111-00000213213";
        public static string _format = "json";
        public static string _requestMode = "post";
        public static string _sm = "md5";
        public static string _version = "v1";
        public static string userName = "root";
        public static string password = "123";

        /// <summary>
        /// Login interface
        /// </summary>
        public static GetWayResponse login()
        {
            RequestHandler reqHandler = new RequestHandler();
            init(reqHandler);
            reqHandler.appId = _aid;
            reqHandler.applicationKey = _akey;
            reqHandler.applicationSecret = _asid;
            reqHandler.method = "open.shopweb.security.mobileLogin";
            reqHandler.gateUrl = apigwUrl;
            reqHandler.parameters.Add("userName", userName);
            reqHandler.parameters.Add("password", password);
            string requestUrl = getRequestURL(reqHandler);
            string res = HttpPost(requestUrl, null);
            GetWayResponse result = JsonConvert.DeserializeObject<GetWayResponse>(res);
            return result;
        }

        /// <summary>
        /// Unified response entity
        /// </summary>
        public class GetWayResponse
        {
            /** Response status object **/
            public ResponseStat stat;
            /** API service layer call result description **/
            public string result;
            /** Return result string (usually in JSON format, custom format by subsystem) **/
            public mobileLogin data;
        }

        /// <summary>
        /// Login interface response entity
        /// </summary>
        public class mobileLogin
        {
            public long enterpriseId;
            public string token;
        }

        /// <summary>
        /// Unified response entity
        /// </summary>
        public class ResponseStat
        {
            /** Current system time **/
            public long systime;
            /** Return code **/
            public int code;
            /** Return code name **/
            public string codename;
            /** Call identifier **/
            public string cid;
        }

        /// <summary>
        /// getDepsHavingFlowDevice
        /// example:
        ///         GetWayResponse tokenResult = WdzHelper.getSign();      
        ///         string res = WdzHelper.getDepsHavingFlowDevice(tokenResult.data.token);
        /// </summary>
        public static string getDepsHavingFlowDevice(string token)
        {
            RequestHandler reqHandler = new RequestHandler();
            init(reqHandler);
            reqHandler.appId = _aid;
            reqHandler.applicationKey = _akey;
            reqHandler.applicationSecret = _asid;
            reqHandler.method = "open.shopweb.departments.getDepsHavingFlowDevice";
            reqHandler.gateUrl = apigwUrl;
            string requestUrl = getRequestURL(reqHandler);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("authenticator", token);
            string res = HttpPost(requestUrl, headers);
            return res;
        }

        /// <summary>
        /// Initiates an HTTP request (using POST method)
        /// </summary>
        private static string HttpPost(string url, Dictionary<string, string> headers)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 10000;
            request.AllowAutoRedirect = false;
            if (headers != null && headers.Count > 0)
            {
                foreach (KeyValuePair<string, string> kvp in headers)
                {
                    request.Headers.Add(kvp.Key, kvp.Value);
                }
            }
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

        /// <summary>
        /// Constructs the request URL
        /// </summary>
        private static string getRequestURL(RequestHandler request)
        {
            if (request.ignoreSignature)
            {
                ignoreSign(request);
            }
            else
            {
                createSign(request);
            }
            StringBuilder sb = new StringBuilder();
            Dictionary<string, object> parameters = request.parameters;
            List<string> keys = parameters.Keys.ToList();
            keys.Sort();
            foreach (string key in keys)
            {
                string value = Convert.ToString(parameters[key]);
                if (request.jsonUnescape)
                {
                    value = value.Replace(@"\\", @"\");
                    value = HttpUtility.UrlEncode(value, Encoding.UTF8);
                }
                sb.Append(key).Append("=").Append(HttpUtility.UrlEncode(value, Encoding.UTF8)).Append("&");
            }
            string reqPars = sb.ToString().Substring(0, sb.Length - 1);
            return request.gateUrl + "?" + reqPars;
        }

        /// <summary>
        /// Generates signature information
        /// </summary>
        private static void createSign(RequestHandler request)
        {
            StringBuilder sb = new StringBuilder();
            Dictionary<string, object> parameters = request.parameters;
            string signatureMethod = request.signatureMethod;
            if (!string.IsNullOrEmpty(signatureMethod))
            {
                setDictionary(parameters, "_sm", signatureMethod);
            }
            else
            {
                signatureMethod = parameters.GetValueOrDefault("_sm", "").ToString();
            }
            // Symmetric encryption: add developer secret, prepend secret to signature string
            if (signatureMethod != "rsa")
            {
                sb.Append(request.applicationSecret);
                setDictionary(parameters, "_akey", request.applicationKey);
            }

            setDictionary(parameters, "_aid", request.appId);
            setDictionary(parameters, "_mt", request.method);

            string version = request.version;
            if (!string.IsNullOrEmpty(version))
            {
                setDictionary(parameters, "_version", version);
            }
            string requestMode = request.requestMode;
            if (!string.IsNullOrEmpty(requestMode))
            {
                setDictionary(parameters, "_requestMode", requestMode);
            }

            List<string> keys = parameters.Keys.ToList();
            keys.Sort();
            foreach (string key in keys)
            {
                string value = Convert.ToString(parameters[key]);
                if (request.jsonUnescape && key == "data")
                {
                    value = value.Replace(@"\\", @"\");
                    value = HttpUtility.UrlEncode(value, Encoding.UTF8);
                }
                if (key != "_sig")
                {
                    sb.Append(key).Append(value);
                }
            }

            string sign = "";
            // Check if using RSA signature
            if (signatureMethod == "rsa")
            {
                sign = getRSASign(sb.ToString(), request.rsaPrivateKey);
            }
            else
            {
                sb.Append(request.applicationSecret);
                sign = getSign(signatureMethod, sb.ToString());
            }
            setDictionary(parameters, "_sig", sign);
            request.debugInfo = sb.ToString() + " => sign:" + sign;
        }

        /// <summary>
        /// Encrypts string using sha1, sha256, or MD5
        /// </summary>
        private static string getSign(string signMethod, string parmsValue)
        {
            try
            {
                string sign = "";
                signMethod = signMethod.ToLower();
                if (signMethod == "sha1")
                {
                    var algorithm = SHA1.Create();
                    sign = bytesToHex(algorithm.ComputeHash(Encoding.UTF8.GetBytes(parmsValue)));
                }
                else if (signMethod == "sha256")
                {
                    var algorithm = SHA256.Create();
                    sign = bytesToHex(algorithm.ComputeHash(Encoding.UTF8.GetBytes(parmsValue)));
                }
                else
                {
                    var algorithm = MD5.Create();
                    sign = bytesToHex(algorithm.ComputeHash(Encoding.UTF8.GetBytes(parmsValue)));
                }
                return sign;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"apigetway sign error: {ex.Message}");
            }
        }

        /// <summary>
        /// Converts bytes to hexadecimal
        /// </summary>
        private static string bytesToHex(byte[] bytes)
        {
            var hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:X2}", b);
            }
            return hex.ToString();
        }

        /// <summary>
        /// Encrypts string using RSA
        /// </summary>
        private static string getRSASign(string content, string privateKey)
        {
            try
            {
                byte[] privateKeyBytes = Convert.FromBase64String(privateKey);

                using (RSA rsa = RSA.Create())
                {
                    rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
                    Encoding encoding = Encoding.UTF8;
                    byte[] dataBytes = encoding.GetBytes(content);

                    byte[] signatureBytes = rsa.SignData(
                        dataBytes,
                        HashAlgorithmName.SHA1,
                        RSASignaturePadding.Pkcs1
                    );

                    return Convert.ToBase64String(signatureBytes);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Dictionary helper method
        /// </summary>
        private static void setDictionary(Dictionary<string, object> source, string name, object newData)
        {
            if (source.ContainsKey(name))
            {
                source[name] = newData;
            }
            else
            {
                source.Add(name, newData);
            }
        }

        /// <summary>
        /// Adds parameters without signature (ignoreSignature = false)
        /// </summary>
        private static void ignoreSign(RequestHandler request)
        {
            request.parameters.Add("_mt", request.method);
            request.parameters.Add("_aid", request.appId);
        }

        /// <summary>
        /// Initializes RequestHandler
        /// </summary>
        private static void init(RequestHandler request)
        {
            request.parameters.Add("_sm", _sm);
            request.parameters.Add("_requestMode", _requestMode);
            request.parameters.Add("_version", _version);
            request.parameters.Add("_timestamp", DateTime.Now.ToString("yyyyMMddHHmmss"));
            request.parameters.Add("_sig", "");
            request.parameters.Add("_nonce", Guid.NewGuid().ToString());
            request.parameters.Add("_format", _format);
        }

        /// <summary>
        /// Request entity
        /// </summary>
        public class RequestHandler
        {
            /** Request parameters */
            public Dictionary<string, object> parameters = new Dictionary<string, object>();
            /** Gateway URL */
            public string gateUrl = "";
            /** Method name */
            public string method = "";
            /** Application ID */
            public string appId = "";
            /** Application key */
            public string applicationKey = "";
            /** Application secret */
            public string applicationSecret = "";
            /** Debug information */
            public string debugInfo = "";
            /** Whether to remove JSON escaping */
            public bool jsonUnescape = false;
            /** Version number */
            public string version;
            /** Request method (post/get/put/delete) */
            public string requestMode;
            /** Signature method */
            public string signatureMethod;
            /** RSA private key */
            public string rsaPrivateKey;
            /** Whether to ignore signature */
            public bool ignoreSignature = false;
        }
    }
}
