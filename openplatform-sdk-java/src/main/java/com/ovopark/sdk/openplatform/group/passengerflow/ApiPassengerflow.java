package com.ovopark.sdk.openplatform.group.passengerflow;

import com.ovopark.sdk.openplatform.GwInitRequestHandler;
import com.ovopark.sdk.openplatform.client.BaseClient;
import com.ovopark.sdk.openplatform.core.OvoParkHttpClient;
/**
 * @ClassName:  ApiPassengerflow   
 * @Description:TODO(客流API)   
 * @author: Remiel_Mercy 
 * @date:   2020年6月3日 上午10:04:02   
 *     
 * @Copyright: 2020 www.ovopark.com Inc. All rights reserved.
 */
public class ApiPassengerflow extends BaseClient{

	protected ApiPassengerflow(String applicationKey, String applicationSecret) {
		super(applicationKey, applicationSecret);
	}

	
	public String getAllHotspotShops(Integer orgid) {
		return getAllHotspotShops(orgid,null,null,null,null,null);
	}
	
	public String getAllHotspotShops(Integer orgid,Integer depId,String shopId,Integer ovoparkEnterpriseId,Integer pageNumber,Integer pageSize) {
		GwInitRequestHandler reqHandler=new GwInitRequestHandler(); 
		reqHandler.init();
		reqHandler.setMethod("open.passengerflow.getAllHotspotShops");
		setCommonParameters(reqHandler);
		
		//处理业务参数-略
		
		
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
			return resContent;
		}
		return null;
	}
}
