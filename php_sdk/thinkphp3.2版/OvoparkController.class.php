<?php
namespace Api\Controller;
use Common\Controller\Base;
class OvoparkController extends Base {
    public function index(){
        $open = new \Common\ORG\Ovopark();
        echo'<h2>添加分组</h2><hr>';
//      $data = $open->addGroup('demo');
//      dump($data);

        echo'<h2>查询分组</h2><hr>';
        $data = $open->queryGroup();//查询分组
        dump($data);

        echo'<h2> 查询人脸设备</h2><hr>';
        $data = $open->queryDevice();
        dump($data);

        echo'<h2> 绑定人脸设备</h2><hr>';
        $data = $open->bindingDevice(0,0);
        dump($data);

//        echo'<h2>添加用户</h2><hr>';
//
//        $param   = array();
//        $param[] = array(
//            "orgid"=>$open->orgid,//分组id(即groupid)
//            "departno"=>$open->departno,//分组id(即groupid)
//            "userid"=>'2',//开发者提供的会员id(第三方系统自行生成)，唯一
//            "thirdpicurl"=>"http://yun.sosucn.com/1234.jpg",//开发者提供的公网可以访问的图片url地址
//            "username"=>"baby",//用户姓名
//            "memberType"=>"1",//用户类别(0：新顾客；1：会员；2：店员)
//            "mobilephone"=>"13880008000",//手机号码
//            "gender"=>'1',//性别,主要做校准使用(0:男,1：女)
//            "checkrepeat"=>1 //验证手机号是否重复, 需要验证为1，不需要验证为0)
//        );
//
//        $data = $open->addUser($param);//添加用户
//
//        dump($data);
//
//        echo'<h2>更新用户</h2><hr>';
//        $param = array(
//            "userid"=>'1',//开发者提供的会员id(第三方系统自行生成)，唯一
//            "thirdpicurl"=>"http://yun.sosucn.com/1234.jpg",//开发者提供的公网可以访问的图片url地址
//            "username"=>"李四",//用户姓名
//            "memberType"=>"2",//用户类别(0：新顾客；1：会员；2：店员)
//            "mobilephone"=>"13880008000",//手机号码
//            "gender"=>'1',//性别,主要做校准使用(0:男,1：女)
//            "checkrepeat"=>0//验证手机号是否重复, 需要验证为1，不需要验证为0)
//        );
//        $data = $open->updateUser($param);//添加用户
//        dump($data);
    }

    /**
     * 接口回调
     */
    public function notify(){
        $_data = I('post.');
        if($_data){
            //做业务逻辑处理
            F('User_'.time(),$_data);
        }
        $data = array('status'=>1,'msg'=>'success');
        echo(json_encode($data));
    }

}
