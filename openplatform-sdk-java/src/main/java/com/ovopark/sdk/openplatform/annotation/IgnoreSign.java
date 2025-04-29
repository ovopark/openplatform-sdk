package com.ovopark.sdk.openplatform.annotation;

import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;
/**
 * 
    * @ClassName: IgnoreSign
    * @Description: TODO(实体类字段配置此注解后，签名 时，忽略该列值)
    * @author Remiel_Mercy xuefei_fly@126.com
    * @date  2017年12月4日 下午2:08:40 
    *
 */
@Retention(RetentionPolicy.RUNTIME)
@Target({java.lang.annotation.ElementType.FIELD})
public @interface IgnoreSign {

}
