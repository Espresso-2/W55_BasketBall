window['qg'].setIsUnityGame(true);
const xgame = qg;
require("ral.js");
const _onWindowResize = xgame.onWindowResize;
ral.getSystemInfoSync = function () {
    let systemInfo = xgame.getSystemInfoSync();
    systemInfo.windowHeight = systemInfo.windowHeight * systemInfo.pixelRatio;
    systemInfo.windowWidth = systemInfo.windowWidth * systemInfo.pixelRatio;
    systemInfo.screenHeight = systemInfo.windowHeight * systemInfo.pixelRatio;
    systemInfo.screenWidth = systemInfo.windowWidth * systemInfo.pixelRatio;
    systemInfo.pixelRatio = 1;
    return systemInfo;
};

require("unityAdapter.min.js");

function _createProperty(value) {
    let _value = value;

    function _get() {
        return _value;
    }

    function _set(v) {
        _value = v;
    }

    return {
        "get": _get,
        "set": _set
    };
}

function _makePropertyWritable(objBase, objScopeName, propName, initValue) {
    let newProp, initObj;
    if (objBase && objScopeName in objBase && propName in objBase[objScopeName]) {
        if(typeof initValue === "undefined") {
            initValue = objBase[objScopeName][propName];
        }
        newProp = _createProperty(initValue);
        try {
            Object.defineProperty(objBase[objScopeName], propName, newProp);
        } catch (e) {
            initObj = {};
            initObj[propName] = newProp;
            try {
                objBase[objScopeName] = Object.create(objBase[objScopeName], initObj);
            } catch (e) {
                // Workaround, but necessary to overwrite native host objects
            }
        }
    }
}

_makePropertyWritable(
    window,
    "navigator",
    "userAgent",
    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.193 Safari/537.36"
);
_makePropertyWritable(
    window,
    "navigator",
    "appVersion",
    "5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.193 Safari/537.36"
);
_makePropertyWritable(
    window,
    "navigator",
    "appName",
    "Netscape"
);

let _oldCreateElement = document.createElement;
let _webglCanvas = null;
document.createElement = function (name) {
    name = name.toLowerCase();
    if (name === "canvas") {
        let node = _oldCreateElement(name);
        if (!_webglCanvas) {
            _webglCanvas = node;
        } else {
			let oldGetContext = node.getContext.bind(node);

			node.getContext = function (name, opts) {
                if (name === "webgl" || name === "experimental-webgl" || name === "webgl2" || name === "experimental-webgl2") {
                  return _webglCanvas.getContext(name, opts);
                } else {
                    return oldGetContext(name, opts);
                }
            };
        }
        return node;
    }
    return _oldCreateElement(name);
};

const ENVIRONMENT_IS_PTHREAD = true;

document.URL = "http://localhost/";

let _div = document.createElement("div");
document.body.appendChild(_div);

let _unityContainer = document.createElement("div");
_unityContainer.id = "unityContainer";
_div.appendChild(_unityContainer);

let _onScreenCanvas = document.createElement("canvas");
_unityContainer.appendChild(_onScreenCanvas);
let _gl = _onScreenCanvas.getContext("webgl2");
if (!_gl) {
    _gl = _onScreenCanvas.getContext("experimental-webgl2");
    if (!_gl) {
        _gl = _onScreenCanvas.getContext("webgl");
        if (!_gl) {
            _gl = _onScreenCanvas.getContext("experimental-webgl");
        }
    }
}

require("ral_2020.js");
    if (window["qg"]) {
        window["qg"].setWasmTaskCompile(true);
        window.CloseAudioLogError = true;
    }
_onScreenCanvas.width = window.innerWidth;
    _onScreenCanvas.height = window.innerHeight;
let buildUrl = "Build/";
    let loaderUrl = buildUrl + "ha.loader.js";
    let config = {
        dataUrl: buildUrl + "ha.data",
        frameworkUrl: buildUrl + "ha.framework.js",
        codeUrl: buildUrl + "ha.wasm",
        streamingAssetsUrl: "StreamingAssets",
        devicePixelRatio: 1
    };
qg.setLoadingProgress({ progress: 0 });
var unityInstance = null;
    let loaderScript = document.createElement("script");
    loaderScript.src = loaderUrl;
    loaderScript.onload = () => {
      delete loaderScript.onload;
      createUnityInstance(_onScreenCanvas, config, (progress) => {
          console.log("progress", 100 * progress + "%");
          qg.setLoadingProgress({ progress: (progress * 100) | 0 });
          if (progress == 1) {
              if (window["qg"]) {
                  window["qg"].setWasmTaskCompile(false);
              }
              qg.loadingComplete({ complete: function(res) {} });
              var audio = qg.createInnerAudioContext();
              audio.src = "audio/BGM.mp3";
              audio.loop = true;
              audio.volume = 0.5;
              audio.play();
          }
      }).then((unity_instance) => {
        unityInstance = unity_instance;
      }).catch((message) => {
        console.error(message);
      });
    };
    document.body.appendChild(loaderScript);