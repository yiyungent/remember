'use strict';

Object.defineProperty(exports, "__esModule", {
	value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _metalDom = require('metal-dom');

var _globals = require('../globals/globals');

var _globals2 = _interopRequireDefault(_globals);

var _metalUri = require('metal-uri');

var _metalUri2 = _interopRequireDefault(_metalUri);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

/**
 * A collection of static utility functions.
 * @const
 */
var utils = function () {
	function utils() {
		_classCallCheck(this, utils);
	}

	_createClass(utils, null, [{
		key: 'copyNodeAttributes',


		/**
   * Copies attributes form source node to target node.
   * @return {void}
   * @static
   */
		value: function copyNodeAttributes(source, target) {
			Array.prototype.slice.call(source.attributes).forEach(function (attribute) {
				return target.setAttribute(attribute.name, attribute.value);
			});
		}

		/**
   * Gets the current browser path including hashbang.
   * @return {!string}
   * @static
   */

	}, {
		key: 'getCurrentBrowserPath',
		value: function getCurrentBrowserPath() {
			return this.getCurrentBrowserPathWithoutHash() + _globals2.default.window.location.hash;
		}

		/**
   * Gets the current browser path excluding hashbang.
   * @return {!string}
   * @static
   */

	}, {
		key: 'getCurrentBrowserPathWithoutHash',
		value: function getCurrentBrowserPathWithoutHash() {
			return _globals2.default.window.location.pathname + _globals2.default.window.location.search;
		}

		/**
   * Gets the given node offset coordinates.
   * @return {!object}
   * @static
   */

	}, {
		key: 'getNodeOffset',
		value: function getNodeOffset(node) {
			var offsetLeft = 0,
			    offsetTop = 0;

			do {
				offsetLeft += node.offsetLeft;
				offsetTop += node.offsetTop;
				node = node.offsetParent;
			} while (node);
			return {
				offsetLeft: offsetLeft,
				offsetTop: offsetTop
			};
		}

		/**
   * Extracts the path part of an url.
   * @return {!string}
   * @static
   */

	}, {
		key: 'getUrlPath',
		value: function getUrlPath(url) {
			var uri = new _metalUri2.default(url);
			return uri.getPathname() + uri.getSearch() + uri.getHash();
		}

		/**
   * Extracts the path part of an url without hashbang.
   * @return {!string}
   * @static
   */

	}, {
		key: 'getUrlPathWithoutHash',
		value: function getUrlPathWithoutHash(url) {
			var uri = new _metalUri2.default(url);
			return uri.getPathname() + uri.getSearch();
		}

		/**
   * Extracts the path part of an url without hashbang and query search.
   * @return {!string}
   * @static
   */

	}, {
		key: 'getUrlPathWithoutHashAndSearch',
		value: function getUrlPathWithoutHashAndSearch(url) {
			var uri = new _metalUri2.default(url);
			return uri.getPathname();
		}

		/**
   * Checks if url is in the same browser current url excluding the hashbang.
   * @param  {!string} url
   * @return {boolean}
   * @static
   */

	}, {
		key: 'isCurrentBrowserPath',
		value: function isCurrentBrowserPath(url) {
			if (url) {
				var currentBrowserPath = this.getCurrentBrowserPathWithoutHash();
				// the getUrlPath will create a Uri and will normalize the path and
				// remove the trailling '/' for properly comparing paths.
				return utils.getUrlPathWithoutHash(url) === this.getUrlPath(currentBrowserPath);
			}
			return false;
		}

		/**
   * Returns true if HTML5 History api is supported.
   * @return {boolean}
   * @static
   */

	}, {
		key: 'isHtml5HistorySupported',
		value: function isHtml5HistorySupported() {
			return !!(_globals2.default.window.history && _globals2.default.window.history.pushState);
		}

		/**
   * Checks if a given url is a valid http(s) uri and returns the formed Uri
   * or false if the parsing failed
   * @return {Uri|boolean}
   * @static
   */

	}, {
		key: 'isWebUri',
		value: function isWebUri(url) {
			try {
				return new _metalUri2.default(url);
			} catch (err) {
				return false;
			}
		}

		/**
   * Removes all attributes form node.
   * @return {void}
   * @static
   */

	}, {
		key: 'clearNodeAttributes',
		value: function clearNodeAttributes(node) {
			Array.prototype.slice.call(node.attributes).forEach(function (attribute) {
				return node.removeAttribute(attribute.name);
			});
		}

		/**
   * Remove elements from the document.
   * @param {!Array<Element>} elements
   */

	}, {
		key: 'removeElementsFromDocument',
		value: function removeElementsFromDocument(elements) {
			elements.forEach(function (element) {
				return (0, _metalDom.exitDocument)(element);
			});
		}

		/**
  * Removes trailing slash in path.
  * @param {!string}
  * @return {string}
  */

	}, {
		key: 'removePathTrailingSlash',
		value: function removePathTrailingSlash(path) {
			var length = path ? path.length : 0;
			if (length > 1 && path[length - 1] === '/') {
				path = path.substr(0, length - 1);
			}
			return path;
		}

		/**
   * Adds a random suffix to the href attribute of the element.
   * @param {!element} element
   * @return {element}
   */

	}, {
		key: 'setElementWithRandomHref',
		value: function setElementWithRandomHref(element) {
			element.href = element.href + '?q=' + Math.random();
			return element;
		}

		/**
   * Overrides document referrer
   * @param {string} referrer
   * @static
   */

	}, {
		key: 'setReferrer',
		value: function setReferrer(referrer) {
			Object.defineProperty(_globals2.default.document, 'referrer', {
				configurable: true,
				get: function get() {
					return referrer;
				}
			});
		}
	}]);

	return utils;
}();

exports.default = utils;