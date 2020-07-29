# openplatform-sdk
万店掌开放平台Sdk
>公共请求参数_aid为开放平台系统编号，固定值为S107为更好地保证数据安全，万店掌开放平台提供了https的网关调用地址供开发者使用，且原始http的调用方式也会保留

>https://openapi.ovopark.com/m.api

>http://openapi.ovopark.com/m.api

>如果api文档里没有特殊注明，请求体格式化：Content-Type默认为application/x-www-form-urlencoded，通过urlencode格式化请求体。_akey和_asid为开发者的key和签名秘钥，开发者认证审核完后可获取，请妥善保管请勿泄露，如有遗忘，可以在https://docs.open.ovopark.com/account/aptitude查询


## 网关更新说明
>2020-07-08 上线中台网关,兼容老网关所有api请求，中台网关地址如下

>https://cloudapi.ovopark.com/m.api

>http://cloudapi.ovopark.com/m.api

## sdk更新说明

### openplatform-sdk-java

#### 1.2.5
	OvoParkHttpClient增加不同环境的配置
	重构open sdk 1.2.5版本
#### 1.2.6
	open sdk 增加json处理
#### 1.2.7
	httpclient get 请求增加请求头处理
	优化POJO类签名方式
#### 1.2.8
	升级fastjson至1.2.60
	升级sdk至1.2.8 增加PUT,DELETE请求处理

## sdk简介
### openplatform-sdk-java
    com.ovopark.sdk.openplatform.utils.SignUtils 签名工具类
    com.ovopark.sdk.openplatform.GwInitRequestHandler 请求报文头报文体封装类
    com.ovopark.sdk.openplatform.core.OvoParkHttpClient 万店掌开放平台http客户端
### python-sdk
    signature.py  仅签名过程
    open_util.py  定时任务+签名+请求+日志

## 签名算法实现
#### 签名机制

###### (1) 所有请求参数按参数名升序排序。
###### (2) 按 请 求 参 数 名 及 参 数 值 相 互 连 接 组 成 一 个 字 符 串：<paramName1><paramValue1><paramName2><paramValue2>…
###### (3) 将应用密钥分别添加到以上请求参数串的头部和尾部：<secret><请求参数字符串><secret>
###### (4) 对该字符串进行 SHA1/MD5 运算，得到一个二进制数组；
###### (5) 将该二进制数组转换为十六进制的字符串，该字符串即是这些请求参数 对应的签名；
###### (6) 该签名值使用 _sig系统级参数一起和其它请求参数一起发送给服务开放平台。

#### 签名调用示例

调用API：open.demo.queryOpenAccount ，使用系统默认MD5加密，因为各语言语法不一致，以下实例只体现逻辑。为便于说明，假设 _akey、_asid值均为test，接口应用参数orgid为1。
###### (1) 输入参数为：
 {_aid=S107, orgid=1, _akey=test, _requestMode=post, _mt=open.demo.queryOpenAccount, _sm=md5, _version=v1,_timestamp=20171221132407}
###### (2)按照参数名称升序排列：
{_aid=S107, _akey=test, _mt=open.demo.queryOpenAccount, _requestMode=post, _sm=md5, _timestamp=20171221132407, _version=v1, orgid=1}
###### (3) 连接字符串：
连接参数名与参数值,并在首尾加上secret，如下： test_aidS107_akeytest_mtopen.demo.queryOpenAccount_requestModepost_smmd5_timestamp20171221132407_versionv1orgid1test
###### (4) 生成签名：
32位大写MD5值-> 5489704B80A47394025527D6D51FB582

## 调用示例
### 签名示例
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
	params.put("username","赵六" );
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
	//获取debug信息
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
### 完整调用demo
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
	//获取请求带参数的url
	String requestUrl = reqHandler.getRequestURL();
	System.out.println(requestUrl);
	//获取debug信息
	String debuginfo = reqHandler.getDebugInfo();
	System.out.println("debuginfo:" + debuginfo);
	//设置请求内容
	httpClient.setReqContent(requestUrl);
	if(httpClient.call()){
		String resContent = httpClient.getResContent();
		System.out.println("responseContent:" + resContent);
	}
```
