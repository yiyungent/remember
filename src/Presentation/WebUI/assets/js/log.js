/// <reference path="../libs/ua-parser-js/dist/ua-parser.min.js" />

var logUrl = "http://localhost:4483/home/log";


// 访问时间: 13位毫秒时间戳
var accessTime = new Date().getTime();
var clickCount = 0;
window.onload = function () {
	recordClick();
	initIdCode();
}
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
				refererUrl: refererUrl,
				clickCount: clickCount,
				visitorInfo: JSON.stringify({
					screen: {
					    width: window.screen.width,
					    height: window.screen.height
					}	
				}),
				idCode: getIdCode()
			},
			dataType: "jsonp",
			beforeSend: function (request) {
				var token = getStorage("token");
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
				refererUrl: refererUrl,
				clickCount: clickCount,
				visitorInfo: JSON.stringify({
					screen: {
					    width: window.screen.width,
					    height: window.screen.height
					}
				}),
				idCode: getIdCode()
			},
			beforeSend: function (request) {
				var token = getStorage("token");
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

function recordClick() {
	document.onclick = function () {
		clickCount++;
	}
}

/**
 * 初始化访客识别码 
 * 确保打开页面后有访客识别码，确保在发日志前存有访客识别码
 * 只有当发现没有存储访客识别码时，才重新生成，并存储
 */
function initIdCode() {
	var val = getStorage("IdCode");
	if (!val) {
		// 没有存储 访客识别码，重新生成，并保存
		if (window.requestIdleCallback) {
		requestIdleCallback(function () {
			val = getFinger();

			// 保存 访客识别码
			setStorage("IdCode", val);
		})
		} else {
			setTimeout(function () {
				val = getFinger();

				// 保存 访客识别码
				setStorage("IdCode", val);
			}, 500);
		}
	}
}

function getFinger() {
	var val = "";
	let options = {
		excludes: {
			userAgent: true,
			audio: true,
			enumerateDevices: true,
			fonts: true,
			fontsFlash: true,
			webgl: true,
			canvas: true
		}
	};
	Fingerprint2.get(options, function (components) {
		// 参数
        const values = components.map(function (component) {
            return component.value
        });
        // 指纹
        const murmur = Fingerprint2.x64hash128(values.join(''), 31);

		//console.log(components) // an array of components: {key: ..., value: ...}

		val = murmur;
	});

	return val;
}

function getIdCode() {
	return getStorage("IdCode");
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

//写cookies
function setCookie(name,value,days){
    var d= new Date();
    d.setTime(d.getTime()+(days*24*60*60*1000));
    var expires = d.toGMTString();
    document.cookie = name+"="+value+";expires="+expires;
}

function getStorage(key) {
	var val = "";
	if (!window.localStorage) {
		// 不支持 localStorage，则使用 cookie
		val = getCookie(key);
	} else {
		val = window.localStorage.getItem(key);
	}

	return val;
}

function setStorage(key, val) {
	if (!window.localStorage) {
		// 不支持 localStorage，则使用 cookie
		setCookie(key,val, 365);
	} else {
		window.localStorage.setItem(key, val);
	}
}