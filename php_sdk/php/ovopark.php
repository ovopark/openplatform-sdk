<?php
/**
 * 万店掌 SDK
 * author:YmJ omyweb@163.com
 * date:2018-01-09
 * version:1.0
 */
class Ovopark{
    //开放平台系统编号
    private $_aid      = 'S107';

    //万店掌开放平台分配给第三方的开发者key
    private $_akey     = '';

    private $_asid     = '';

    //签名算法 md5,sha1
    private $_sm       = 'md5';

    //版本号
    private $_version  = 'v1';

    //api 地址
    private $api_url   = 'http://openapi.ovopark.com/m.api';

    //企业id
    public $orgid     = 0;//设置默认的$orgid值

    //分组id(即groupid)
    public $departno  = 0; //设置默认的groupid值

    //请求方式post,get
    private $_requestMode = 'post';

    private $options = array();

    public function __construct($options){
        $this->_aid  = isset($options['_aid'])?$options['_aid']:$this->_aid;
        $this->_akey = isset($options['_akey'])?$options['_akey']:$this->_akey;
        $this->_asid = isset($options['_asid'])?$options['_asid']:$this->_asid;
        $this->_sm   = isset($options['_sm'])?$options['_sm']:'md5';
        $this->orgid = isset($options['orgid'])?$options['orgid']:$this->orgid;
        //公共参数
        $this->options =array(
            '_aid'=>$this->_aid,
            '_akey'=>$this->_akey,
            '_sm'=>$this->_sm,
            '_requestMode'=>$this->_requestMode,
            '_version'=>$this->_version,
            'orgid'=>$this->orgid,
            '_format'=>'json',
            '_timestamp'=>date('YmdHis',time())
        );
    }

    /**
     * 签名
     * @param array $parameter 签名所需参数
     * @return string
     */
    private function autograph($parameter=array()){
        if(!$parameter){
            return false;
        }
        //数组升序排序
        ksort($parameter);
        $parameter_str = '';
        //把数组转为连续连接的字符串
        foreach($parameter as $name=>$val){
            $parameter_str .= $name.$val;
        }
        //签名前后需要加上 _asid;
        $_autograph = $this->_asid.$parameter_str.$this->_asid;
        $_autograph = md5($_autograph);
        $_autograph = strtoupper($_autograph);//转为大写
        return $_autograph;
    }

    /**
     * 添加用户
     * 开发者注册会员至万店掌人脸平台
     * @param array $user 用户数据
     * @return bool|mixed|string 返回请求结果
     */
    public function addUser($user = array()){
        if(!$user){
            return array('result'=>'必填字段不能为空！','stat'=>array('code'=>'100002'));
        }

        //公共参数
        $this->options['_mt'] = 'open.face.addUser';

        //业务参数
        $_post['DataUser'] = json_encode($user);

        //合并提交参数
        $param = array_merge($this->options, $_post);

        //获取签名
        $param['_sig']  = $this->autograph($param);

        //发起post请求
        $data = $this->http_post($this->api_url,$param);

        //把 json 转为数组
        $data = json_decode($data,true);
        return $data;
    }

    /**
     * 查询分组
     * 开发者查询人脸分组信息
     */
    public function queryGroup(){
        //公共参数
        $this->options['_mt'] = 'open.face.queryGroup';

        //业务参数
        $_post = array(
            'orgid'=> $this->orgid
        );
        //合并参数
        $param = array_merge($this->options, $_post);

        //获取签名
        $param['_sig']  = $this->autograph($param);

        //发起post请求
        $data = $this->http_post($this->api_url,$param);

        //把 json 转为数组
        $data = json_decode($data,true);
        return $data;
    }

    /**
     * 查询人脸设备
     * 开发者查询人脸设备列表信息
     */
    public function queryDevice(){
        //公共参数
        $this->options['_mt'] = 'open.face.queryDevice';

        //业务参数
        $_post = array(
            'orgid'=> $this->orgid
        );
        //合并参数
        $param = array_merge($this->options, $_post);

        //获取签名
        $param['_sig']  = $this->autograph($param);

        //发起post请求
        $data = $this->http_post($this->api_url,$param);

        //把 json 转为数组
        $data = json_decode($data,true);
        return $data;
    }


    /**
     * 绑定人脸设备
     * 开发者绑定人脸设备和分组的关系
     * @param int $deviceId  设备ID
     * @param int $groupid  分组ID
     * @return array|mixed|string
     */
    public function bindingDevice($deviceId=0,$groupid=0){
        //公共参数
        $this->options['_mt'] = 'open.face.bindingDevice';

        if(!$deviceId){
            return array('result'=>'缺少deviceId','stat'=>array('code'=>'100002'));
        }
        //业务参数
        $_post = array(
            'orgid'=> $this->orgid,
            'groupid'=> $this->departno,
            'deviceId'=>$deviceId
        );
        if($groupid){
            $_post['groupid'] = $groupid;
        }
        //合并参数
        $param = array_merge($this->options, $_post);

        //获取签名
        $param['_sig']  = $this->autograph($param);

        //发起post请求
        $data = $this->http_post($this->api_url,$param);

        //把 json 转为数组
        $data = json_decode($data,true);
        return $data;
    }

    /**
     * 更新用户
     * 开发者更新会员至万店掌人脸平台
     * @param array $user 用户数据
     * @return array 返回请求结果
     */
    public function updateUser($user = array()){
        //公共参数
        $this->options['_mt'] = 'open.face.updateUser';

        //业务参数
        if( !$user['orgid'] ){
            $user['orgid'] = $this->orgid;
        }

        if( !$user['departno'] ){
            $user['departno'] = $this->departno;
        }
        //是否验证手机号
        if( !$user['checkrepeat'] ){
            $user['checkrepeat'] = 0;
        }

        //合并参数
        $param = array_merge($this->options, $user);

        //获取签名
        $param['_sig']  = $this->autograph($param);

        //发起post请求
        $data = $this->http_post($this->api_url,$param);
        //把 json 转为数组
        $data = json_decode($data,true);
        return $data;
    }


    /**
     * 添加分组
     * 开发者添加人脸分组信息
     * @param string $group_name 分组名称
     * @return array|mixed|string
     */
    public function addGroup($group_name=''){
        if(!$group_name){
            return array('result'=>'必填字段不能为空！','stat'=>array('code'=>'100002'));
        }
        //公共参数
        $this->options['_mt'] = 'open.face.addGroup';

        //业务参数
        $_post = array(
             'groupname' => $group_name,//分组名称
             'orgid'=> $this->orgid
        );
        //合并参数
        $param = array_merge($this->options, $_post);

        //获取签名
        $param['_sig']  = $this->autograph($param);

        //发起post请求
        $data = $this->http_post($this->api_url,$param);

        //把 json 转为数组
        $data = json_decode($data,true);
        return $data;
    }

    /**
     * POST 请求
     * @param string $url
     * @param array $param
     * @param boolean $post_file 是否文件上传
     * @return string content
     */
    public function http_post($url,$param,$post_file=false){
        $oCurl = curl_init();
        if(stripos($url,"https://")!==FALSE){
            curl_setopt($oCurl, CURLOPT_SSL_VERIFYPEER, FALSE);
            curl_setopt($oCurl, CURLOPT_SSL_VERIFYHOST, false);
            curl_setopt($oCurl, CURLOPT_SSLVERSION, 1); //CURL_SSLVERSION_TLSv1
        }
        if (is_string($param) || $post_file) {
            $strPOST = $param;
        } else {
            $aPOST = array();
            foreach($param as $key=>$val){
                $aPOST[] = $key."=".urlencode($val);
            }
            $strPOST =  join("&", $aPOST);
        }
        curl_setopt($oCurl, CURLOPT_URL, $url);
        curl_setopt($oCurl, CURLOPT_RETURNTRANSFER, 1 );
        curl_setopt($oCurl, CURLOPT_POST,true);
        curl_setopt($oCurl, CURLOPT_POSTFIELDS,$strPOST);
        $sContent = curl_exec($oCurl);
        $aStatus = curl_getinfo($oCurl);
        curl_close($oCurl);
        if(intval($aStatus["http_code"])==200){
            return $sContent;
        }else{
            return false;
        }
    }

}