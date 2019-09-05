'use strict';

Object.defineProperty(exports, "__esModule", {
	value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _metal = require('metal');

var _metalAjax = require('metal-ajax');

var _metalAjax2 = _interopRequireDefault(_metalAjax);

var _metalStructs = require('metal-structs');

var _metalPromise = require('metal-promise');

var _metalPromise2 = _interopRequireDefault(_metalPromise);

var _errors = require('../errors/errors');

var _errors2 = _interopRequireDefault(_errors);

var _utils = require('../utils/utils');

var _utils2 = _interopRequireDefault(_utils);

var _globals = require('../globals/globals');

var _globals2 = _interopRequireDefault(_globals);

var _Screen2 = require('./Screen');

var _Screen3 = _interopRequireDefault(_Screen2);

var _metalUri = require('metal-uri');

var _metalUri2 = _interopRequireDefault(_metalUri);

var _metalUseragent = require('metal-useragent');

var _metalUseragent2 = _interopRequireDefault(_metalUseragent);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var RequestScreen = function (_Screen) {
	_inherits(RequestScreen, _Screen);

	/**
  * Request screen abstract class to perform io operations on descendant
  * screens.
  * @constructor
  * @extends {Screen}
  */
	function RequestScreen() {
		_classCallCheck(this, RequestScreen);

		/**
   * @inheritDoc
   * @default true
   */
		var _this = _possibleConstructorReturn(this, (RequestScreen.__proto__ || Object.getPrototypeOf(RequestScreen)).call(this));

		_this.cacheable = true;

		/**
   * Holds default http headers to set on request.
   * @type {?Object=}
   * @default {
   *   'X-PJAX': 'true',
   *   'X-Requested-With': 'XMLHttpRequest'
   * }
   * @protected
   */
		_this.httpHeaders = {
			'X-PJAX': 'true',
			'X-Requested-With': 'XMLHttpRequest'
		};

		/**
   * Holds default http method to perform the request.
   * @type {!string}
   * @default RequestScreen.GET
   * @protected
   */
		_this.httpMethod = RequestScreen.GET;

		/**
   * Holds the XHR object responsible for the request.
   * @type {XMLHttpRequest}
   * @default null
   * @protected
   */
		_this.request = null;

		/**
   * Holds the request timeout in milliseconds.
   * @type {!number}
   * @default 30000
   * @protected
   */
		_this.timeout = 30000;
		return _this;
	}

	/**
  * Asserts that response status code is valid.
  * @param {number} status
  * @protected
  */


	_createClass(RequestScreen, [{
		key: 'assertValidResponseStatusCode',
		value: function assertValidResponseStatusCode(status) {
			if (!this.isValidResponseStatusCode(status)) {
				var error = new Error(_errors2.default.INVALID_STATUS);
				error.invalidStatus = true;
				error.statusCode = status;
				throw error;
			}
		}

		/**
   * @inheritDoc
   */

	}, {
		key: 'beforeUpdateHistoryPath',
		value: function beforeUpdateHistoryPath(path) {
			var redirectPath = this.getRequestPath();
			if (redirectPath && redirectPath !== path) {
				return redirectPath;
			}
			return path;
		}

		/**
   * @inheritDoc
   */

	}, {
		key: 'beforeUpdateHistoryState',
		value: function beforeUpdateHistoryState(state) {
			// If state is ours and navigate to post-without-redirect-get set
			// history state to null, that way Senna will reload the page on
			// popstate since it cannot predict post data.
			if (state.senna && state.form && state.redirectPath === state.path) {
				return null;
			}
			return state;
		}

		/**
   * Formats load path before invoking ajax call.
   * @param {string} path
   * @return {string} Formatted path;
   * @protected
   */

	}, {
		key: 'formatLoadPath',
		value: function formatLoadPath(path) {
			var uri = new _metalUri2.default(path);

			uri.setHostname(_globals2.default.window.location.hostname);
			uri.setProtocol(_globals2.default.window.location.protocol);

			if (_globals2.default.window.location.port) {
				uri.setPort(_globals2.default.window.location.port);
			}

			if (_metalUseragent2.default.isIeOrEdge && this.httpMethod === RequestScreen.GET) {
				return uri.makeUnique().toString();
			}

			return uri.toString();
		}

		/**
   * Gets the http headers.
   * @return {?Object=}
   */

	}, {
		key: 'getHttpHeaders',
		value: function getHttpHeaders() {
			return this.httpHeaders;
		}

		/**
   * Gets the http method.
   * @return {!string}
   */

	}, {
		key: 'getHttpMethod',
		value: function getHttpMethod() {
			return this.httpMethod;
		}

		/**
   * Gets request path.
   * @return {string=}
   */

	}, {
		key: 'getRequestPath',
		value: function getRequestPath() {
			var request = this.getRequest();
			if (request) {
				var requestPath = request.requestPath;
				var responseUrl = this.maybeExtractResponseUrlFromRequest(request);
				if (responseUrl) {
					requestPath = responseUrl;
				}
				if (_metalUseragent2.default.isIeOrEdge && this.httpMethod === RequestScreen.GET) {
					requestPath = new _metalUri2.default(requestPath).removeUnique().toString();
				}
				return _utils2.default.getUrlPath(requestPath);
			}
			return null;
		}

		/**
   * Gets the request object.
   * @return {?Object}
   */

	}, {
		key: 'getRequest',
		value: function getRequest() {
			return this.request;
		}

		/**
   * Gets the request timeout.
   * @return {!number}
   */

	}, {
		key: 'getTimeout',
		value: function getTimeout() {
			return this.timeout;
		}

		/**
   * Checks if response succeeded. Any status code 2xx or 3xx is considered
   * valid.
   * @param {number} statusCode
   */

	}, {
		key: 'isValidResponseStatusCode',
		value: function isValidResponseStatusCode(statusCode) {
			return statusCode >= 200 && statusCode <= 399;
		}

		/**
   * Returns the form data
   * This method can be extended in order to have a custom implementation of the form params
   * @param {!Element} formElement
   * @param {!Element} submittedButtonElement
   * @return {!FormData}
   */

	}, {
		key: 'getFormData',
		value: function getFormData(formElement, submittedButtonElement) {
			var formData = new FormData(formElement);
			this.maybeAppendSubmitButtonValue_(formData, submittedButtonElement);
			return formData;
		}

		/**
   * @inheritDoc
   */

	}, {
		key: 'load',
		value: function load(path) {
			var _this2 = this;

			var cache = this.getCache();
			if ((0, _metal.isDefAndNotNull)(cache)) {
				return _metalPromise2.default.resolve(cache);
			}
			var body = null;
			var httpMethod = this.httpMethod;
			var headers = new _metalStructs.MultiMap();
			Object.keys(this.httpHeaders).forEach(function (header) {
				return headers.add(header, _this2.httpHeaders[header]);
			});
			if (_globals2.default.capturedFormElement) {
				this.addSafariXHRPolyfill();
				body = this.getFormData(_globals2.default.capturedFormElement, _globals2.default.capturedFormButtonElement);
				httpMethod = RequestScreen.POST;
				if (_metalUseragent2.default.isIeOrEdge) {
					headers.add('If-None-Match', '"0"');
				}
			}
			var requestPath = this.formatLoadPath(path);
			return _metalAjax2.default.request(requestPath, httpMethod, body, headers, null, this.timeout).then(function (xhr) {
				_this2.removeSafariXHRPolyfill();
				_this2.setRequest(xhr);
				_this2.assertValidResponseStatusCode(xhr.status);
				if (httpMethod === RequestScreen.GET && _this2.isCacheable()) {
					_this2.addCache(xhr.responseText);
				}
				xhr.requestPath = requestPath;
				return xhr.responseText;
			}).catch(function (reason) {
				_this2.removeSafariXHRPolyfill();
				switch (reason.message) {
					case _errors2.default.REQUEST_TIMEOUT:
						reason.timeout = true;
						break;
					case _errors2.default.REQUEST_ERROR:
						reason.requestError = true;
						break;
					case _errors2.default.REQUEST_PREMATURE_TERMINATION:
						reason.requestError = true;
						reason.requestPrematureTermination = true;
						break;
				}
				throw reason;
			});
		}

		/**
   * Adds aditional data to the body of the request in case a submit button
   * is captured during form submission.
   * @param {!FormData} body The FormData containing the request body.
    * @param {!Element} submittedButtonElement
    * @protected
   */

	}, {
		key: 'maybeAppendSubmitButtonValue_',
		value: function maybeAppendSubmitButtonValue_(formData, submittedButtonElement) {
			if (submittedButtonElement && submittedButtonElement.name) {
				formData.append(submittedButtonElement.name, submittedButtonElement.value);
			}
		}

		/**
   * The following method tries to extract the response url value by checking
   * the custom response header 'X-Request-URL' if proper value is not present
   * in XMLHttpRequest. The value of responseURL will be the final URL
   * obtained after any redirects. Internet Explorer, Edge and Safari <= 7
   * does not yet support the feature. For more information see:
   * https://developer.mozilla.org/en-US/docs/Web/API/XMLHttpRequest/responseURL
   * https://xhr.spec.whatwg.org/#the-responseurl-attribute
   * @param {XMLHttpRequest} request
   * @return {?string} Response url best match.
   */

	}, {
		key: 'maybeExtractResponseUrlFromRequest',
		value: function maybeExtractResponseUrlFromRequest(request) {
			var responseUrl = request.responseURL;
			if (responseUrl) {
				return responseUrl;
			}
			return request.getResponseHeader(RequestScreen.X_REQUEST_URL_HEADER);
		}

		/**
   * This function set attribute data-safari-temp-disabled to 
   * true and set disable attribute of an input type="file" tag
   * is used as a polyfill for iOS 11.3 Safari / macOS Safari 11.1 
   * empty <input type="file"> XHR bug.
   * https://github.com/rails/rails/issues/32440
   * https://bugs.webkit.org/show_bug.cgi?id=184490
   */

	}, {
		key: 'addSafariXHRPolyfill',
		value: function addSafariXHRPolyfill() {
			if (_globals2.default.capturedFormElement && _metalUseragent2.default.isSafari) {
				var inputs = _globals2.default.capturedFormElement.querySelectorAll('input[type="file"]:not([disabled])');
				for (var index = 0; index < inputs.length; index++) {
					var input = inputs[index];
					if (input.files.length > 0) {
						return;
					}
					input.setAttribute('data-safari-temp-disabled', 'true');
					input.setAttribute('disabled', '');
				}
			}
		}

		/**
   * This function remove attribute data-safari-temp-disabled and disable attribute
   * of an input type="file" tag is used as a polyfill for iOS 11.3 Safari / macOS Safari 11.1
   * empty <input type="file"> XHR bug.
   * https://github.com/rails/rails/issues/32440
   * https://bugs.webkit.org/show_bug.cgi?id=184490
   */

	}, {
		key: 'removeSafariXHRPolyfill',
		value: function removeSafariXHRPolyfill() {
			if (_globals2.default.capturedFormElement && _metalUseragent2.default.isSafari) {
				var inputs = _globals2.default.capturedFormElement.querySelectorAll('input[type="file"][data-safari-temp-disabled]');
				for (var index = 0; index < inputs.length; index++) {
					var input = inputs[index];
					input.removeAttribute('data-safari-temp-disabled');
					input.removeAttribute('disabled');
				}
			}
		}

		/**
   * Sets the http headers.
   * @param {?Object=} httpHeaders
   */

	}, {
		key: 'setHttpHeaders',
		value: function setHttpHeaders(httpHeaders) {
			this.httpHeaders = httpHeaders;
		}

		/**
   * Sets the http method.
   * @param {!string} httpMethod
   */

	}, {
		key: 'setHttpMethod',
		value: function setHttpMethod(httpMethod) {
			this.httpMethod = httpMethod.toLowerCase();
		}

		/**
   * Sets the request object.
   * @param {?Object} request
   */

	}, {
		key: 'setRequest',
		value: function setRequest(request) {
			this.request = request;
		}

		/**
   * Sets the request timeout in milliseconds.
   * @param {!number} timeout
   */

	}, {
		key: 'setTimeout',
		value: function setTimeout(timeout) {
			this.timeout = timeout;
		}
	}]);

	return RequestScreen;
}(_Screen3.default);

/**
 * Holds value for method get.
 * @type {string}
 * @default 'get'
 * @static
 */


RequestScreen.GET = 'get';

/**
 * Holds value for method post.
 * @type {string}
 * @default 'post'
 * @static
 */
RequestScreen.POST = 'post';

/**
 * Fallback http header to retrieve response request url.
 * @type {string}
 * @default 'X-Request-URL'
 * @static
 */
RequestScreen.X_REQUEST_URL_HEADER = 'X-Request-URL';

exports.default = RequestScreen;