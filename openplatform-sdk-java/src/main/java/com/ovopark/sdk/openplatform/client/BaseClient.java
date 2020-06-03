package com.ovopark.sdk.openplatform.client;

import com.ovopark.sdk.openplatform.GwInitRequestHandler;
import com.ovopark.sdk.openplatform.config.OpenplatformConst;

public abstract class BaseClient {
	/**app key */
	private String applicationKey;
	/** 秘钥 */
	private String applicationSecret;
	
    protected BaseClient(String applicationKey, String applicationSecret) {
        this.applicationKey = applicationKey;
        this.applicationSecret = applicationSecret;
    }
    
    protected void setCommonParameters(GwInitRequestHandler reqHandler) {
    	reqHandler.setGateUrl(OpenplatformConst.apigwUrl);
    	reqHandler.setApplicationKey(this.applicationKey);
    	reqHandler.setApplicationSecret(this.applicationSecret);
    }
    
}
