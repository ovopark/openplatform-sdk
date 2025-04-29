<?php
/**
 * 万店掌 SDK demo
 * author:YmJ omyweb@163.com
 * date:2018-01-09
 * version:1.0
 */
require_once 'ovopark.php';
/**
 * 浏览器友好的变量输出
 * @param mixed $var 变量
 * @param boolean $echo 是否输出 默认为True 如果为false 则返回输出字符串
 * @param string $label 标签 默认为空
 * @param boolean $strict 是否严谨 默认为true
 * @return mixed|null|string|string[]
 */
function dump($var, $echo=true, $label=null, $strict=true) {
    $label = ($label === null) ? '' : rtrim($label) . ' ';
    if (!$strict) {
        if (ini_get('html_errors')) {
            $output = print_r($var, true);
            $output = '<pre>' . $label . htmlspecialchars($output, ENT_QUOTES) . '</pre>';
        } else {
            $output = $label . print_r($var, true);
        }
    } else {
        ob_start();
        var_dump($var);
        $output = ob_get_clean();
        if (!extension_loaded('xdebug')) {
            $output = preg_replace('/\]\=\>\n(\s+)/m', '] => ', $output);
            $output = '<pre>' . $label . htmlspecialchars($output, ENT_QUOTES) . '</pre>';
        }
    }
    if ($echo) {
        echo($output);
        return null;
    }else{
        return $output;
    }
}

$open = new Ovopark();
echo'<h2>添加分组</h2><hr>';
//$data = $open->addGroup('demo');
//dump($data);

echo'<h2>查询分组</h2><hr>';
$data = $open->queryGroup();//查询分组
dump($data);

echo'<h2> 查询人脸设备</h2><hr>';
$data = $open->queryDevice();
dump($data);

echo'<h2> 绑定人脸设备</h2><hr>';
$data = $open->bindingDevice(59,9467);
dump($data);

//echo'<h2>添加用户</h2><hr>';
//
//$param   = array();
//$param[] = array(
//    "orgid"=>$open->orgid,//分组id(即groupid)
//    "departno"=>$open->departno,//分组id(即groupid)
//    "userid"=>'2',//开发者提供的会员id(第三方系统自行生成)，唯一
//    "thirdpicurl"=>"http://yun.sosucn.com/1234.jpg",//开发者提供的公网可以访问的图片url地址
//    "username"=>"baby",//用户姓名
//    "memberType"=>"1",//用户类别(0：新顾客；1：会员；2：店员)
//    "mobilephone"=>"13880008000",//手机号码
//    "gender"=>'1',//性别,主要做校准使用(0:男,1：女)
//    "checkrepeat"=>1 //验证手机号是否重复, 需要验证为1，不需要验证为0)
//);
//
//$data = $open->addUser($param);//添加用户
//
//dump($data);
//
//echo'<h2>更新用户</h2><hr>';
//$param = array(
//    "userid"=>'1',//开发者提供的会员id(第三方系统自行生成)，唯一
//    "thirdpicurl"=>"http://yun.sosucn.com/1234.jpg",//开发者提供的公网可以访问的图片url地址
//    "username"=>"李四",//用户姓名
//    "memberType"=>"2",//用户类别(0：新顾客；1：会员；2：店员)
//    "mobilephone"=>"13880008000",//手机号码
//    "gender"=>'1',//性别,主要做校准使用(0:男,1：女)
//    "checkrepeat"=>0//验证手机号是否重复, 需要验证为1，不需要验证为0)
//);
//$data = $open->updateUser($param);//添加用户
//dump($data);



