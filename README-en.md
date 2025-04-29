# openplatform-sdk
Opretail open platform Sdk
>The public request parameter _aid is the open platform system number, and the fixed value is S107 to better ensure data security.
The Opretail open platform provides the gateway invocation address of HTTPS for developers to use, and the original HTTP invocation method will also be retained

>https://openapi.opretail.com/m.api

>http://openapi.opretail.com/m.api

If not specified in the API documentation, request body formatting: content-type defaults to application/x-www-form-urlencoded.
Format the request body by urlencode, _akey and _asid are the key and signature secret keys of the developer. Developer aptitude is available after review, please keep it safe. If you have forgotten, you can inquire at https://docs.open.ovopark.com/account/aptitude.
## sdk update instructions

### openplatform-sdk-java

#### 1.2.5
	OvoParkHttpClient add the configuration of different environments
	Rebuild the open sdk version 1.2.5
#### 1.2.6
	open sdk add json processing
#### 1.2.7
	httpclient get requests add request header processing
	Optimized POJO class signature mode
#### 1.2.8
	Upgrade fastjson to 1.2.60
	Upgrade sdk to 1.2.8, add PUT, DELETE request processing 

##  Introduction of the sdk
### openplatform-sdk-java
    com.ovopark.sdk.openplatform.utils.SignUtils  Signature tool class
    com.ovopark.sdk.openplatform.GwInitRequestHandler  Request header style encapsulation class
    com.ovopark.sdk.openplatform.core.OvoParkHttpClient Opretail open platform http client
### python-sdk
    signature.py  only has signature  process
    open_util.py  Timed task + signature + request + log

##  Implementation of signature algorithm
#### Signature mechanism 

###### (1)  All request parameters are sorted in ascending order by parameter name.
###### (2)The parameter names and parameter values are concatenated as required to form a string of characters ：<paramName1><paramValue1><paramName2><paramValue2>…
###### (3) Add the application keys to the header and tail of the above request parameter string, respectively: <secret><Request parameter string><secret>
###### (4) A SHA1/MD5 operation is performed on this string to produce a binary array
###### (5) Converts the binary array to a hexadecimal string that is the signature of the request parameters
###### (6) This signature value is sent to the service open platform with the _sig system-level parameter along with the other request parameters.

#### Signature call example

Call API: open. demo. queryOpenAccount, use the system default MD5 encryption because the grammar of each language is inconsistent, the following instance manifest the logic. For illustrative purposes, assume that the _akey and _asid values are both test, and that the interface application parameter of orgid is 1.
###### (1) Input parameter is
 {_aid=S107, orgid=1, _akey=test, _requestMode=post, _mt=open.demo.queryOpenAccount, _sm=md5, _version=v1,_timestamp=20171221132407}
###### (2)In ascending order by parameter name
{_aid=S107, _akey=test, _mt=open.demo.queryOpenAccount, _requestMode=post, _sm=md5, _timestamp=20171221132407, _version=v1, orgid=1}
###### (3) Connection string
Connect the parameter name with the parameter value and add secret at the beginning and end, as follows:  test_aidS107_akeytest_mtopen.demo.queryOpenAccount_requestModepost_smmd5_timestamp20171221132407_versionv1orgid1test
###### (4) generate the signature
32-bit uppercase MD5 values -> 5489704B80A47394025527D6D51FB582
## The call sample
### The signature sample
#### eg1:
```java
	Map<String, Object> params = new HashMap<String, Object>();
	params.put("_aid","xxx" );
	params.put("_akey","xxx" );
	params.put("_mt","xxx" );
	params.put("_sm","md5" );
	params.put("_requestMode","post" );
	params.put("_version","v1" );
	params.put("_timestamp","20171228140800" );
	params.put("thirdpicurl","http://www.ovopark.com/static/img/contact_company_member_00.923069c.jpg" );
	params.put("wdzpicurl","http://www.ovopark.com/static/img/contact_company_member_00.923069c.jpg" );
	params.put("userid","0000000003" );
	params.put("gender","false" );
	params.put("username","zhaoliu" );
	params.put("memberType","2" );
	params.put("mobilephone","18662250114" );
	params.put("age","20" );
	params.put("glass","Normal" );
	params.put("groupid","9413" );
	params.put("orgid","1" );
	String signValue = SignUtils.sign(params, "123","md5");
	System.out.println(SignUtils.getDebugInfo());
```
#### eg2:
```java
	public void test(){
	queryGroupPoJo pojo=new queryGroupPoJo();
	pojo.set_akey("xxx");
	pojo.set_mt("open.face.queryGroup");
	pojo.setOrgid("1");
	String _sig=SignUtils.signPoJo(pojo, _asid);
	System.out.println("_sig:"+_sig);
	//Get debug information
	String debuginfo = SignUtils.getDebugInfo();
	System.out.println("debuginfo:" + debuginfo);
	}
	class queryGroupPoJo extends GetWayHeader implements Serializable{
	private static final long serialVersionUID = 1L;
	private String orgid;
	@IgnoreSign
	private String aaa;
	public String getOrgid() {
		return orgid;
	}
	public void setOrgid(String orgid) {
		this.orgid = orgid;
	}
	public String getAaa() {
		return aaa;
	}
	public void setAaa(String aaa) {
		this.aaa = aaa;
	}
}
```
### Full call demo
```java
GwInitRequestHandler reqHandler=new  GwInitRequestHandler(); 
	reqHandler.init();
	reqHandler.setApplicationKey(_akey);
	reqHandler.setApplicationSecret(_asid);
	reqHandler.setMethod("open.passengerflow.getPassengers");
	reqHandler.setGateUrl(apigwUrl);
	reqHandler.setParameter("orgid", "");
	reqHandler.setParameter("deviceMac", "");
	reqHandler.setParameter("depId", "");
	reqHandler.setParameter("starttime", "2018-07-27 00:00:00");
	reqHandler.setParameter("endtime", "2018-07-28 23:59:59");
	reqHandler.setParameter("pageNumber", "1");
	reqHandler.setParameter("pageSize", "20");
	OvoParkHttpClient httpClient=new OvoParkHttpClient();
	//Get the URL of the request with parameters
	String requestUrl = reqHandler.getRequestURL();
	System.out.println(requestUrl);
	//Get debug information
	String debuginfo = reqHandler.getDebugInfo();
	System.out.println("debuginfo:" + debuginfo);
	//Set request content
	httpClient.setReqContent(requestUrl);
	if(httpClient.call()){
		String resContent = httpClient.getResContent();
		System.out.println("responseContent:" + resContent);
	}
```

