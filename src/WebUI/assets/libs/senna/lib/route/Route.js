'use strict';

Object.defineProperty(exports, "__esModule", {
	value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _metal = require('metal');

var _metalPathParser = require('metal-path-parser');

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var Route = function () {

	/**
  * Route class.
  * @param {!string|RegExp|Function} path
  * @param {!Function} handler
  * @constructor
  */
	function Route(path, handler) {
		_classCallCheck(this, Route);

		if (!(0, _metal.isDefAndNotNull)(path)) {
			throw new Error('Route path not specified.');
		}
		if (!(0, _metal.isFunction)(handler)) {
			throw new Error('Route handler is not a function.');
		}

		/**
   * Defines the handler which will execute once a URL in the application
   * matches the path.
   * @type {!Function}
   * @protected
   */
		this.handler = handler;

		/**
   * Defines the path which will trigger the route handler.
   * @type {!string|RegExp|Function}
   * @protected
   */
		this.path = path;
	}

	/**
 * Builds parsed data (regex and tokens) for this route.
 * @return {!Object}
 * @protected
 */


	_createClass(Route, [{
		key: 'buildParsedData_',
		value: function buildParsedData_() {
			if (!this.parsedData_) {
				var tokens = (0, _metalPathParser.parse)(this.path);
				var regex = (0, _metalPathParser.toRegex)(tokens);
				this.parsedData_ = {
					regex: regex,
					tokens: tokens
				};
			}
			return this.parsedData_;
		}

		/**
   * Extracts param data from the given path, according to this route.
   * @param {string} path The url path to extract params from.
   * @return {Object} The extracted data, if the path matches this route, or
   *     null otherwise.
   */

	}, {
		key: 'extractParams',
		value: function extractParams(path) {
			if ((0, _metal.isString)(this.path)) {
				return (0, _metalPathParser.extractData)(this.buildParsedData_().tokens, path);
			}
			return {};
		}

		/**
   * Gets the route handler.
   * @return {!Function}
   */

	}, {
		key: 'getHandler',
		value: function getHandler() {
			return this.handler;
		}

		/**
   * Gets the route path.
   * @return {!string|RegExp|Function}
   */

	}, {
		key: 'getPath',
		value: function getPath() {
			return this.path;
		}

		/**
  	 * Matches if the router can handle the tested path.
  	 * @param {!string} value Path to test (may contain the querystring part).
   * @return {boolean} Returns true if matches any route.
   */

	}, {
		key: 'matchesPath',
		value: function matchesPath(value) {
			var path = this.path;

			if ((0, _metal.isFunction)(path)) {
				return path(value);
			}
			if ((0, _metal.isString)(path)) {
				path = this.buildParsedData_().regex;
			}
			if (path instanceof RegExp) {
				return value.search(path) > -1;
			}

			return false;
		}
	}]);

	return Route;
}();

exports.default = Route;