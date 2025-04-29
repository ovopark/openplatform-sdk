import hashlib


# 仅签名
def createSign():
    asecret = 'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx'
    sign_dict = {
        '_aid': 'S107',
        '_akey': 'S107-0000xxxx',
        '_requestMode': 'post',
        '_sm': 'md5',
        # '_timestamp': time.strftime('%Y%m%d%H%M%S', time.localtime(time.time())),
        '_timestamp': 20191128141620,
        '_version': 'v1',
        '_mt': 'open.gateway.getBusinessOrg'
    }
    params = {
        'orgid': 11111,
        'starttime': '--------',
        'endtime': '-----'
    }
    # 拼接
    sign_dict = {**sign_dict, **params}
    # 对键进行排序
    d = dict(sorted(sign_dict.items(), key=lambda x: x[0]))
    # 连接参数名与参数值,并在首尾加上secret
    sb = ''
    sb += asecret
    for key in d:
        sb += key
        sb += str(d[key])
    sb += asecret
    # md5加密
    sign = hashlib.md5(sb.encode(encoding='UTF-8')).hexdigest()
    # 转大写
    sign = str(sign).upper()
    print(sign)


if __name__ == '__main__':
    createSign()

