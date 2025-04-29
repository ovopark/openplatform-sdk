package com.ovopark.sdk.openplatform.utils;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

public class OvoParkUtils {
	/**
	 * 获取编码字符集
	 * @param request
	 * @param response
	 * @return String
	 */
	public static String getCharacterEncoding(HttpServletRequest request,
			HttpServletResponse response) {
		if(null == request || null == response) {
			return "UTF-8";
		}
		String enc = request.getCharacterEncoding();
		if(null == enc || "".equals(enc)) {
			enc = response.getCharacterEncoding();
		}
		if(null == enc || "".equals(enc)) {
			enc = "UTF-8";
		}
		return enc;
	}
}
