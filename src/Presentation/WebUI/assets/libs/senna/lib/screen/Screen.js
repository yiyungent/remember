'use strict';

Object.defineProperty(exports, "__esModule", {
	value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _get = function get(object, property, receiver) { if (object === null) object = Function.prototype; var desc = Object.getOwnPropertyDescriptor(object, property); if (desc === undefined) { var parent = Object.getPrototypeOf(object); if (parent === null) { return undefined; } else { return get(parent, property, receiver); } } else if ("value" in desc) { return desc.value; } else { var getter = desc.get; if (getter === undefined) { return undefined; } return getter.call(receiver); } };

var _metal = require('metal');

var _metalDom = require('metal-dom');

var _Cacheable2 = require('../cacheable/Cacheable');

var _Cacheable3 = _interopRequireDefault(_Cacheable2);

var _metalPromise = require('metal-promise');

var _metalPromise2 = _interopRequireDefault(_metalPromise);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Screen = function (_Cacheable) {
	_inherits(Screen, _Cacheable);

	/**
  * Screen class is a special type of route handler that provides helper
  * utilities that adds lifecycle and methods to provide content to each
  * registered surface.
  * @constructor
  * @extends {Cacheable}
  */
	function Screen() {
		_classCallCheck(this, Screen);

		/**
   * Holds the screen id.
   * @type {string}
   * @protected
   */
		var _this = _possibleConstructorReturn(this, (Screen.__proto__ || Object.getPrototypeOf(Screen)).call(this));

		_this.id = _this.makeId_((0, _metal.getUid)());

		/**
   * Holds the screen meta tags. Relevant when the meta tags
   * should be updated when screen is rendered.
   */
		_this.metas = null;

		/**
   * Holds the screen title. Relevant when the page title should be
   * upadated when screen is rendered.
   * @type {?string=}
   * @default null
   * @protected
   */
		_this.title = null;
		return _this;
	}

	/**
  * Fires when the screen is active. Allows a screen to perform any setup
  * that requires its DOM to be visible. Lifecycle.
  */


	_createClass(Screen, [{
		key: 'activate',
		value: function activate() {}

		/**
   * Gives the Screen a chance to cancel the navigation and stop itself from
   * activating. Can be used, for example, to prevent navigation if a user
   * is not authenticated. Lifecycle.
   * @return {boolean=|?CancellablePromise=} If returns or resolves to true,
   *     the current screen is locked and the next nagivation interrupted.
   */

	}, {
		key: 'beforeActivate',
		value: function beforeActivate() {}

		/**
   * Gives the Screen a chance to cancel the navigation and stop itself from
   * being deactivated. Can be used, for example, if the screen has unsaved
   * state. Lifecycle. Clean-up should not be preformed here, since the
   * navigation may still be cancelled. Do clean-up in deactivate.
   * @return {boolean=|?CancellablePromise=} If returns or resolves to true,
   *     the current screen is locked and the next nagivation interrupted.
   */

	}, {
		key: 'beforeDeactivate',
		value: function beforeDeactivate() {}

		/**
   * Gives the Screen a chance format the path before history update.
   * @path {!string} path Navigation path.
   * @return {!string} Navigation path to use on history.
   */

	}, {
		key: 'beforeUpdateHistoryPath',
		value: function beforeUpdateHistoryPath(path) {
			return path;
		}

		/**
   * Gives the Screen a chance format the state before history update.
   * @path {!object} state History state.
   * @return {!object} History state to use on history.
   */

	}, {
		key: 'beforeUpdateHistoryState',
		value: function beforeUpdateHistoryState(state) {
			return state;
		}

		/**
   * Allows a screen to do any cleanup necessary after it has been
   * deactivated, for example cancelling outstanding requests or stopping
   * timers. Lifecycle.
   */

	}, {
		key: 'deactivate',
		value: function deactivate() {}

		/**
   * Dispose a screen, either after it is deactivated (in the case of a
   * non-cacheable view) or when the App is itself disposed for whatever
   * reason. Lifecycle.
   */

	}, {
		key: 'disposeInternal',
		value: function disposeInternal() {
			_get(Screen.prototype.__proto__ || Object.getPrototypeOf(Screen.prototype), 'disposeInternal', this).call(this);
		}

		/**
   * Allows a screen to evaluate scripts before the element is made visible.
   * Lifecycle.
   * @param {!object} surfaces Map of surfaces to flip keyed by surface id.
   * @return {?CancellablePromise=} This can return a promise, which will
   *     pause the navigation until it is resolved.
   */

	}, {
		key: 'evaluateScripts',
		value: function evaluateScripts(surfaces) {
			Object.keys(surfaces).forEach(function (sId) {
				if (surfaces[sId].activeChild) {
					_metalDom.globalEval.runScriptsInElement(surfaces[sId].activeChild);
				}
			});
			return _metalPromise2.default.resolve();
		}

		/**
   * Allows a screen to evaluate styles before the element is made visible.
   * Lifecycle.
   * @param {!object} surfaces Map of surfaces to flip keyed by surface id.
   * @return {?CancellablePromise=} This can return a promise, which will
   *     pause the navigation until it is resolved.
   */

	}, {
		key: 'evaluateStyles',
		value: function evaluateStyles() {
			return _metalPromise2.default.resolve();
		}

		/**
   * Allows a screen to perform any setup immediately before the element is
   * made visible. Lifecycle.
   * @param {!object} surfaces Map of surfaces to flip keyed by surface id.
   * @return {?CancellablePromise=} This can return a promise, which will pause the
   *     navigation until it is resolved.
   */

	}, {
		key: 'flip',
		value: function flip(surfaces) {
			var _this2 = this;

			var transitions = [];

			Object.keys(surfaces).forEach(function (sId) {
				var surface = surfaces[sId];
				var deferred = surface.show(_this2.id);
				transitions.push(deferred);
			});

			return _metalPromise2.default.all(transitions);
		}

		/**
   * Gets the screen id.
   * @return {string}
   */

	}, {
		key: 'getId',
		value: function getId() {
			return this.id;
		}

		/**
   * Gets the screen meta tags.
   * @return {NodeList|Node}
   */

	}, {
		key: 'getMetas',
		value: function getMetas() {
			return this.metas;
		}

		/**
   * Returns the content for the given surface, or null if the surface isn't
   * used by this screen. This will be called when a screen is initially
   * constructed or, if a screen is non-cacheable, when navigated.
   * @param {!string} surfaceId The id of the surface DOM element.
   * @param {!Object} params Params extracted from the current path.
   * @return {?string|Element=} This can return a string or node representing
   *     the content of the surface. If returns falsy values surface default
   *     content is restored.
   */

	}, {
		key: 'getSurfaceContent',
		value: function getSurfaceContent() {}

		/**
   * Gets the screen title.
   * @return {?string=}
   */

	}, {
		key: 'getTitle',
		value: function getTitle() {
			return this.title;
		}

		/**
   * Returns all contents for the surfaces. This will pass the loaded content
   * to <code>Screen.load</code> with all information you
   * need to fulfill the surfaces. Lifecycle.
   * @param {!string=} path The requested path.
   * @return {!CancellablePromise} This can return a string representing the
   *     contents of the surfaces or a promise, which will pause the navigation
   *     until it is resolved. This is useful for loading async content.
   */

	}, {
		key: 'load',
		value: function load() {
			return _metalPromise2.default.resolve();
		}

		/**
   * Makes the id for the screen.
   * @param {!string} id The screen id the content belongs too.
   * @return {string}
   * @private
   */

	}, {
		key: 'makeId_',
		value: function makeId_(id) {
			return 'screen_' + id;
		}

		/**
   * Sets the screen id.
   * @param {!string} id
   */

	}, {
		key: 'setId',
		value: function setId(id) {
			this.id = id;
		}

		/**
   * Sets the screen meta tags.
   * @param {NodeList|Node} metas
   */

	}, {
		key: 'setMetas',
		value: function setMetas(metas) {
			this.metas = metas;
		}

		/**
   * Sets the screen title.
   * @param {?string=} title
   */

	}, {
		key: 'setTitle',
		value: function setTitle(title) {
			this.title = title;
		}

		/**
   * @return {string}
   */

	}, {
		key: 'toString',
		value: function toString() {
			return this.id;
		}
	}]);

	return Screen;
}(_Cacheable3.default);

/**
 * @param {*} object
 * @return {boolean} Whether a given instance implements
 * <code>Screen</code>.
 */


Screen.isImplementedBy = function (object) {
	return object instanceof Screen;
};

exports.default = Screen;