package com.ovopark.sdk.openplatform;


import java.util.Date;

import com.ovopark.sdk.openplatform.config.OpenplatformConst;
import com.ovopark.sdk.openplatform.kit.DateKit;



public class GwInitRequestHandler extends RequestHandler{
	public GwInitRequestHandler() {
		super();
		
	}
	public void init() {
		this.setParameter("_aid", OpenplatformConst.AID);
		this.setParameter("_sm",OpenplatformConst.Sm.MD5.getValue());
		this.setParameter("_requestMode", OpenplatformConst.RequestMode.POST.getValue());
		this.setParameter("_version", OpenplatformConst.VERSION);
		this.setParameter("_timestamp", DateKit.DateTime2Str(new Date(), DateKit.getSampleTimeFormat()));
		//签名
		this.setParameter("_sig", "");
		this.setParameter("_format", "json");
	}
	public void createSign() {
		super.createSign(false);
		this.setParameter("_sig", getParameter("_sig").toUpperCase());
	}
}
