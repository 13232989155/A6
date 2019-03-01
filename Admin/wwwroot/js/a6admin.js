
//右上角弹出错误通知
function ShowPNotify(text) {
    new PNotify({
        title: '错误!',
        text: text,
        type: 'error',
        delay: 1000,
        styling: 'bootstrap3'
    });
}

//成功通知
function ShowSuccessPNotify(text) {
    new PNotify({
        title: '成功',
        text: text,
        type: 'success',
        delay: 1000,
        styling: 'bootstrap3'
    });
}

//生成guid
function GUID() {
    this.date = new Date();

    /* 判断是否初始化过，如果初始化过以下代码，则以下代码将不再执行，实际中只执行一次 */
    if (typeof this.newGUID != 'function') {

        /* 生成GUID码 */
        GUID.prototype.newGUID = function () {
            this.date = new Date();
            var guidStr = '';
            sexadecimalDate = this.hexadecimal(this.getGUIDDate(), 16);
            sexadecimalTime = this.hexadecimal(this.getGUIDTime(), 16);
            for (var i = 0; i < 9; i++) {
                guidStr += Math.floor(Math.random() * 16).toString(16);
            }
            guidStr += sexadecimalDate;
            guidStr += sexadecimalTime;
            while (guidStr.length < 32) {
                guidStr += Math.floor(Math.random() * 16).toString(16);
            }
            return this.formatGUID(guidStr);
        }

        /*
        * 功能：获取当前日期的GUID格式，即8位数的日期：19700101
        */
        GUID.prototype.getGUIDDate = function () {
            return this.date.getFullYear() + this.addZero(this.date.getMonth() + 1)
                + this.addZero(this.date.getDay());
        }

        /*
        * 功能：获取当前时间的GUID格式，即8位数的时间，包括毫秒，毫秒为2位数：12300933
        */
        GUID.prototype.getGUIDTime = function () {
            return this.addZero(this.date.getHours()) + this.addZero(this.date.getMinutes())
                + this.addZero(this.date.getSeconds()) + this.addZero(parseInt(this.date.getMilliseconds() / 10));
        }

        /*
        * 功能: 为一位数的正整数前面添加0，如果是可以转成非NaN数字的字符串也可以实现
        */
        GUID.prototype.addZero = function (num) {
            if (Number(num).toString() != 'NaN' && num >= 0 && num < 10) {
                return '0' + Math.floor(num);
            } else {
                return num.toString();
            }
        }

        /*
        * 功能：将y进制的数值，转换为x进制的数值
        */
        GUID.prototype.hexadecimal = function (num, x, y) {
            if (y != undefined) {
                return parseInt(num.toString(), y).toString(x);
            } else {
                return parseInt(num.toString()).toString(x);
            }
        }

        /*
        * 功能：格式化32位的字符串为GUID模式的字符串
        */
        GUID.prototype.formatGUID = function (guidStr) {
            var str1 = guidStr.slice(0, 8) + '-',
                str2 = guidStr.slice(8, 12) + '-',
                str3 = guidStr.slice(12, 16) + '-',
                str4 = guidStr.slice(16, 20) + '-',
                str5 = guidStr.slice(20);
            return str1 + str2 + str3 + str4 + str5;
        }
    }
}

//获取七牛资源地址
function GetQiNiuDomain(bucket) {
    var url
    switch (bucket) {
        case "video":
            url = "https://video.geekann.com/";
            break;
        default:
            url = "https://image.geekann.com/";
    }
    return url;
}

//上传文件
function UploadFile(formData) {
    var defer = $.Deferred();
    $("body").mLoading("show");//显示loading组件
    $.ajax({
        type: "post",
        url: "/Home/UploadFile",
        data: formData,
        dataType: "json",
        contentType: false,
        async: true,
        processData: false,
        success: function (result) {
            if (result.code == "200") {
                defer.resolve(result.data)
            }
            else {
                ShowPNotify(result.msg)
            }
        },
        error: function () {
            ShowPNotify("请求错误")
        },
        complete: function () {
            $("body").mLoading("hide");//显示loading组件
        }
    });
    return defer;
}

//获取图片大小
function GetImageSize(file) {
    var AllImgExt = ".jpg|.jpeg|.gif|.bmp|.png|";
    if (AllImgExt.indexOf(GetFileType(file) + "|") == -1) {
        ErrMsg = "该文件类型不允许上传。请上传 " + AllImgExt + " 类型的文件，当前文件类型为" + extName;
        alert(ErrMsg);
        return false;
    }
    // 创建对象
    var img = new Image();
    // 改变图片的src
    img.src = e.target.result;

    var obj = {
        width: "",
        height: ""
    }

    obj.width = img.width;
    obj.height = img.height;

    return obj;
}

//获取七牛token
function GetQiNiuToken(bucket) {
    var defer = $.Deferred();
    $.ajax({
        type: "post",
        url: "/Home/GetQiNiuToken",
        dataType: "json",
        data: {
            bucket: bucket
        },
        success: function (result) {
            if (result.code == "200") {
                defer.resolve(result.data)
                token = result.data
            }
            else {
                ShowPNotify(result.msg)
            }
        },
        error: function () {
            ShowPNotify("请求错误")
        },
    });
    return defer;
}

//文件二进制地址转blob对象
function DataURItoBlob(dataURI) {
    var byteString = atob(dataURI.split(',')[1]);
    var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];
    var ab = new ArrayBuffer(byteString.length);
    var ia = new Uint8Array(ab);
    for (var i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
    }
    return new Blob([ab], { type: mimeString });
}

//文件file转blob对象
function FileToBlob(file) {
    var reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function (e) {
        return dataURItoBlob(e.target.result);
    }
}


//mime转换
function MimeToSuffix(mime) {
    var suffix
    switch (mime) {
        case "video/mp4":
            suffix = ".mp4";
            break;
        case "image/jpeg":
            suffix = ".jpeg";
            break;
        case "image/gif":
            suffix = ".gif";
            break;
        case "image/png":
            suffix = ".png";
            break;
        case "audio/mpeg":
            suffix = ".mp3";
            break;
        case "image/bmp":
            suffix = ".bmp";
            break;
        case "image/x-png":
            suffix = ".png";
            break;
        default:
            suffix = "";
    }
    return suffix;
}
