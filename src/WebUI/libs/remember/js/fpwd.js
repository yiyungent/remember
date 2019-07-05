$(function () {
	confirmAccount();
	// start 腾讯验证
	// 点击验证
	window.tCaptcha = new TencentCaptcha(
		'2041300407',
		function (res) {
			if (res.ret === 0) {
				// 腾讯云验证通过
				console.log("腾讯第一次验证通过");
				// 第一次验证通过
				// 验证第一次通过后，保存票据和账号，跳到下一步
				if ($("#js-userName").length != 0) {
					$(".data-step .userName").val($("#js-userName").val().trim());
				}
				resetPassword();
				// 准备 服务端 第二次验证
				$(".data-step .ticket").val(res.ticket);
				$(".data-step .randStr").val(res.randstr);
				console.log("第一次验证 结束");
			}
		}
	);
	// end 腾讯验证
	$(".step-list a:eq(0)").on("click", function (e) {
		confirmAccount();
		e.preventDefault();
	});
	$(".step-list a:eq(1)").on("click", function (e) {
		console.log($(this));
		if (!$(this).hasClass("step-pass") && !$(this).hasClass("active")) return;
		resetPassword();
		e.preventDefault();
	});
});

function confirmAccount() {
	var htmlStr = '<div class="form-group">\
						<div class="el-input">\
							<input id="js-userName" class="el-input-inner" type="text" placeholder="请输入绑定的手机号/邮箱">\
						</div>\
						<div class="form-message text-error"></div>\
				   </div>\
				   <div class="form-group">\
						<button class="el-button" type="button"><span>确认</span></button>\
				   </div>';
	$(".step-container").html(htmlStr);

	var isFirst = true;
	$(".el-button").on("click", function () {
		isFirst = false;
		// 是否输入为空
		if ($(".el-input-inner").val().trim() == "") {
			$(".el-input").addClass("input-error");
			$(".form-group .text-error").text("请输入手机号/邮箱");
			return;
		}
		// 弹出滑动验证框
		tCaptcha.show();
	});

	// 效验是否输入
	$(".el-input-inner").on("change", function () {
		if (isFirst) return;
		// 是否输入为空
		if ($(".el-input-inner").val().trim() == "") {
			$(".el-input").addClass("input-error");
			$(".form-group .text-error").text("请输入手机号/邮箱");
		} else {
			$(".el-input").removeClass("input-error");
			$(".form-group .text-error").text("");
		}
	});
}

function resetPassword() {
	$(".step-list a:eq(0)").removeClass("active").addClass("step-pass");
	$(".step-list a:eq(1)").addClass("active");
	var htmlStr = '<div id="js-pwd" class="form-group">\
						<span class="form-group-title">新密码：</span>\
	                    <div class="el-input">\
							<input type="password" class="el-input-inner" placeholder="新密码：6～16位字符，区分大小写">\
						</div>\
	                    <div class="form-message text-error"></div>\
				   </div>\
				   <div id="js-repwd" class="form-group">\
						<span class="form-group-title">确认密码：</span>\
						<div class="el-input">\
							<input type="password" class="el-input-inner" placeholder="请输入确认密码">\
						</div>\
						<div class="form-message text-error"></div>\
				   </div>\
				   <div class="form-group">\
						<span class="form-group-title" style="top: 0;">邮箱：</span>\
						<div class="clearfix">\
							<p class="mail-text fl">{0}</p>\
							<a href="#" class="fl">修改</a>\
						</div>\
						<div class="form-message text-error"></div>\
				   </div>\
				   <div id="js-verify" class="form-group">\
						<span class="form-group-title">验证：</span>\
						<div class="clearfix">\
							<div class="el-input verify-code fl">\
								<input type="text" class="el-input-inner" placeholder="请输入短信/邮件验证码">\
							</div>\
							<button type="button" class="verify-btn fl"><span>获取验证码</span></button>\
						</div>\
						<div class="form-message text-error"></div>\
				   </div>\
				   <div class="form-group">\
						<button id="js-btn-confirm" type="button" class="btn-confirm"><span>确认修改</span></button>\
				   </div>';
	htmlStr = htmlStr.format($(".data-step .userName").val());
	$(".step-container").html(htmlStr);

	// 点击获取验证码
	$(".verify-btn").on("click", function () {
		var ticket = $(".data-step .ticket").val();
		var randStr = $(".data-step .randStr").val();
		var userName = $(".data-step .userName").val();
		var verifyDiv = $("#js-verify");
		// 发送滑动验证票据到服务器
		$.ajax({
			url: "/account/login/verifycode",
			type: "POST",
			data: { "ticket": ticket, "randStr": randStr, "action": "rpwd|" + userName },
			dataType: "json",
			success: function (data) {
				if (data.code == -1) {
					// 验证不通过--返回提示重新滑动验证
					//verifyDiv.find(".text-error").html("验证不通过或已过期，请重新验证");
					tCaptcha.show();
				} else if (data.code == 1) {
					// 验证通过--发送验证码到邮箱，返回发送提示
					//verifyDiv.find(".text-error").html('验证码短信/邮件已发出，5分钟内有效，请注意<a target="_blank" href="//mail.126.com" style="font-size: 14px;">查收</a>');
					verifyDiv.find(".text-error").html(data.message);
				}
			}
		});

		// 发送验证码按钮
		settime($(this));
	});

	var isFirst = true;
	// 点击确认修改
	$("#js-btn-confirm").on("click", function () {
		if (!checkResetPwdInput()) {
			isFirst = false;
			return;
		}
		var userName = $(".data-step .userName").val();
		var password = $("#js-pwd input").val();
		var vCode = $("#js-verify input").val();
		// 发送用户名(邮箱)，新密码，验证码到服务器
		$.ajax({
			url: "/account/login/resetpwd",
			type: "POST",
			data: { "userName": userName, "password": password, "vCode": vCode },
			dataType: "json",
			success: function (data) {
				if (data.code == -1) {
					// 失败
					$("#js-verify").find(".text-error").text(data.message);
				} else if (data.code == 1) {
					// 成功
					resetSuccess();
				}
			}
		});
	});

	$(".el-input-inner").on("change", function () {
		if (isFirst) return;
		checkResetPwdInput();
	});

	$(".form-group .mail-text").next("a").on("click", function (e) {
		confirmAccount();
		e.preventDefault();
	});
}

function resetSuccess() {
	$(".step-list").find("a:eq(1)").removeClass("active").addClass("step-pass").end().find("a:eq(2)").addClass("active");
	setTimeout(function () {
		window.location.href = "/account/login";
	}, 5000);
	var sec = 5;
	var htmlStr = '<div>密码已经重置成功，还剩{0}s跳转至登录</div>';
	setInterval(function () {
		$(".step-container").html(htmlStr.format(sec));
		sec--;
	}, 1000);
}

var count = 30;
/**
 * 30s后获取验证码
 */
function settime(obj) {
	if (count == 0) {
		obj.removeAttr("disabled");
		obj.text("发送验证码");
		obj.removeClass("is-disabled");
		count = 30;
		return;
	} else {
		obj.attr("disabled", "disabled");
		obj.text(count + "s后重发");
		obj.addClass("is-disabled");
		count--;
	}
	setTimeout(function () {
		settime(obj)
	}, 1000)
}

function checkResetPwdInput() {
	var isPass = true;
	var pwdDiv = $("#js-pwd");
	var repwdDiv = $("#js-repwd");
	var userName = $(".data-step .userName").val();
	if (pwdDiv.find("input").val() == "") {
		pwdDiv.find(".el-input").addClass("input-error");
		pwdDiv.find(".text-error").text("请输入密码");
		isPass = false;
	} else {
		pwdDiv.find(".el-input").removeClass("input-error");
		pwdDiv.find(".text-error").text("");
	}
	if (repwdDiv.find("input").val() == "") {
		repwdDiv.find(".el-input").addClass("input-error");
		repwdDiv.find(".text-error").text("请输入确认密码");
		isPass = false;
	} else {
		repwdDiv.find(".el-input").removeClass("input-error");
		repwdDiv.find(".text-error").text("");
	}
	if (repwdDiv.find("input").val() != pwdDiv.find("input").val()) {
		repwdDiv.find(".el-input").addClass("input-error");
		repwdDiv.find(".text-error").text("两次输入的密码不一致");
		isPass = false;
	} else {
		repwdDiv.find(".el-input").removeClass("input-error");
		repwdDiv.find(".text-error").text("");
	}
	return isPass;
}


String.prototype.format = function () {
	var args = arguments;
	return this.replace(/\{(\d+)\}/g, function (s, i) {
		return args[i];
	});
};