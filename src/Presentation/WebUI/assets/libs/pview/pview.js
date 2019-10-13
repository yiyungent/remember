function PView(options) {

	this.init = function () {
		var that = this;
		$('[pview-btn], [pview-btn-group] [pview-btn], [pview-btn-group] a').on('click', function (event) {
			console.log('init start');
			// 阻止默认事件
			event = event || window.event;
			if (event.preventDefault) {
				event.preventDefault();
			} else {
				event.returnValue = false;
			}

			var goUrl = '';
			if (this.hasAttribute('pview-url')) {
				goUrl = this.getAttribute('pview-url');
			} else if (this.tagName.toLowerCase() == 'a') {
				goUrl = this.getAttribute('href');
			} else {
				goUrl = window.location.href;
			}
			console.log('goUrl', goUrl);

			var pview = '';
			if (this.hasAttribute('pview-targets')) {
				pview = this.getAttribute('pview-targets');
			} else if (this.parentNode.hasAttribute('pview-targets')) {
				pview = this.parentNode.getAttribute('pview-targets');
			} else if (this.parentNode.parentNode.hasAttribute('pview-targets')) {
				pview = this.parentNode.parentNode.getAttribute('pview-targets');
			}

			that.go(pview, goUrl, 'get', {});
			console.log('init success');
		});

		window.addEventListener('popstate', function (event) {
			console.log('location: ' + document.location);
			console.log(event.state);
		});
	},

	/**
		* 前往此页面
		* @param {String} pview 指定要更新的区块 eg: top-nav,main-content
		* @param {String} url	eg: www.baidu.com
		* @param {String} type eg: get or post
		* @param {String | Object} data 要发送的数据
		*/
	this.go = function (pview, url, type, data) {
		var that = this;
		$.ajax({
			url: url,
			type: type,
			headers: { "pview": pview },
			data: data,
			dataType: 'html',
			timeout: 2000,
			success: function (data) {
				// 改页面 url
				if (!!(window.history && history.pushState)) {
					history.pushState(null, '', url);
				}

				// html字符串转dom对象
				var dataObj = document.createElement("code");
				dataObj.innerHTML = data;

				// 需要更新的区块
				var updatePviews = [];
				var updatePviewNames = [];
				if (pview != null && pview.trim() != '') {
					updatePviewNames = pview.split(',');
				}
				for (var i = 0; i < updatePviewNames.length; i++) {
					// updatePviews[i] = $('[pview="' + updatePviewNames[i] + '"]')[0]; // 注意：每个是一个 js对象
					updatePviews[i] = document.querySelector('[pview="' + updatePviewNames[i] + '"]');
				}
				// 查找当前页面的（点击要求更新的）每一个 区块, 并将返回的 html 从中筛选出 相应区块，将对应的 旧的 区块替换
				console.log('---------updatePviews-----------');
				console.log(updatePviews);
				var pviewItem = null;
				var pviewItemName = '', pviewItemHtml = '';
				for (var i = 0; i < updatePviews.length; i++) {
					// 当前 pview块的 名
					// pviewItemName = $(updatePviews[i]).attr('pview');
					pviewItemName = updatePviews[i].getAttribute('pview');
					// 当前 pview块的 新返回的 区块
					pviewItem = dataObj.querySelector('[pview="' + pviewItemName + '"]');
					if (pviewItem != null) {
						// 返回的 html 中存在此区块，才更新此要求的区块
						pviewItemHtml = pviewItem.innerHTML;
						console.log('---------updatePviewItem-----------');
						console.log(updatePviews[i]);
						updatePviews[i].innerHTML = pviewItemHtml;
						console.log('----update success-----');
					}
				}
				// 重新绑定 btn, 防止 btn 位于更新的区块，而引起的未绑定点击事件
				that.init();
			}
		});
	}


}
