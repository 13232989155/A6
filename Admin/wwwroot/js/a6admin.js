﻿
//右上角屯出错误通知
function ShowPNotify(text) {
    new PNotify({
        title: '错误!',
        text: text,
        type: 'error',
        delay: 1000,
        styling: 'bootstrap3'
    });
}



