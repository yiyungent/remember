/// <reference path="../libs/ua-parser-js/dist/ua-parser.min.js" />

var logUrl = "http://localhost:4483/home/log";


// 访问时间: 13位毫秒时间戳
var accessTime = new Date().getTime();
window.onbeforeunload = function () {
	writeLog();
}
// 发送访问日志
function writeLog() {
	var jumpTime = new Date().getTime();
	// userAgent 信息
	var uaParser = new UAParser();
	var userAgent = uaParser.getResult();

	var ip = returnCitySN["cip"];
	var city = returnCitySN["cname"];

	var accessUrl = window.location.href;
	var refererUrl = "";
	if (document.referrer.length > 0) {
		refererUrl = document.referrer;
	}

	if (userAgent.ua.indexOf("msie") > -1) { //IE
		$.ajax({
			url: logUrl,
			type: "Post",
			crossDomain: true,
			async: false,
			data: {
				accessTime: accessTime,
				jumpTime: jumpTime,
				userAgent: JSON.stringify(userAgent),
				ip: ip,
				city: city,
				accessUrl: accessUrl,
				refererUrl: refererUrl
			},
			dataType: "jsonp",
			beforeSend: function (request) {
				var token = "";
				if (!!localStorage.getItem("token")) {
					token = localStorage.getItem("token");
				} else if (!!getCookie("token")) {
					token = getCookie("token");
				}
				if (!!token) {
					request.setRequestHeader("Authorization", "Bearer " + token);
				}
			},
			success: function (data) {
				console.log(data.message);
			}
		});
	} else { // FireFox Chrome
		$.ajax({
			url: logUrl,
			type: "Post",
			async: false,
			data: {
				accessTime: accessTime,
				jumpTime: jumpTime,
				userAgent: JSON.stringify(userAgent),
				ip: ip,
				city: city,
				accessUrl: accessUrl,
				refererUrl: refererUrl
			},
			beforeSend: function (request) {
				var token = "";
				if (!!localStorage.getItem("token")) {
					token = localStorage.getItem("token");
				} else if (!!getCookie("token")) {
					token = getCookie("token");
				}
				if (!!token) {
					request.setRequestHeader("Authorization", "Bearer " + token);
				}
			},
			success: function (data) {
				console.log(data.message);
			}
		});
	}

}


/**
 * 读取cookies 
 * @param {string} name cookie名
 */
function getCookie(name) {
	var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");

	if (arr = document.cookie.match(reg))

		return unescape(arr[2]);
	else
		return null;
} 