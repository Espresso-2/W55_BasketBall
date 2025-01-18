var QgGameBridge = {
  $CONSTANT: {
    ACTION_CALL_BACK_CLASS_NAME_DEFAULT: "QGMiniGameManager",
    ACTION_CALL_BACK_METHORD_NAME_DEFAULT: "DefaultResponseCallback",
    ACTION_CALL_BACK_METHORD_NAME_AD_ERROR: "AdOnErrorCallBack",
    ACTION_CALL_BACK_METHORD_NAME_AD_LOAD: "AdOnLoadCallBack",
    //ACTION_CALL_BACK_METHORD_NAME_AD_SHOW: 'AdOnShowCallBack',
    ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE: "AdOnCloseCallBack",
    ACTION_CALL_BACK_METHORD_NAME_AD_HIDE: "AdOnHideCallBack",
    ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE_REWARDED: "RewardedVideoAdOnCloseCallBack",
    ACTION_CALL_BACK_METHORD_NAME_AD_LOAD_NATIVE: "NativeAdOnLoadCallBack",
  },

  $mAdMap: {},

  $mFileData: {},

  $mRecordManager: null,
  // Storage
  QGStorageSetItem: function (keyName, keyValue) {
    var keyNameStr = Pointer_stringify(keyName);
    var keyValueStr = Pointer_stringify(keyValue);
    localStorage.setItem(keyNameStr, keyValueStr);
    console.log("QGStorageSetItem success");
  },
  QGStorageGetItem: function (keyName) {
    var keyNameStr = Pointer_stringify(keyName);
    var val = localStorage.getItem(keyNameStr);
    console.log("QGStorageGetItem111: " + val);
    if (val) {
      console.log("QGStorageGetItem: " + val);
    } else {
      console.log("QGStorageGetItem No data");
    }
  },
  QGStorageRemoveItem: function (keyName) {
    var keyNameStr = Pointer_stringify(keyName);
    localStorage.removeItem(keyNameStr);
    console.log("QGStorageRemoveItem: " + keyNameStr);
  },

  QGLogin: function (success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    var successID = Pointer_stringify(success);
    var failID = Pointer_stringify(fail);

    qg.login({
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          data: res.data,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "LoginResponseCallback", json);
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
          errMsg: res.errMsg,
          errCode: res.errCode,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "LoginResponseCallback", json);
      },
    });
  },

  QGCreateShortcutButton: function () {
    qg.hasShortcutInstalled({
      success: function (status) {
        if (status) {
          //console.log("已创建");
          qg.showToast({
            title: "桌面图标已创建~",
            duration: 2000,
          });
        } else {
          qg.installShortcut({
            success: function () {
              qg.showToast({
                title: "桌面图标创建成功~",
                duration: 2000,
              });
            },
            fail: function () {
              //console.log("创建桌面图标失败！");
              qg.showToast({
                title: "创建桌面图标失败！",
                duration: 2000,
              });
            },
            complete: function () {
              //console.log("create_ShortcutButton ------------> complete");
            },
          });
        }
      },
      fail: function () {
        console.log("获取桌面图标是否创建失败~");
      },
    });
  },

  QGHasShortcutInstalled: function (success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    var successID = Pointer_stringify(success);
    var failID = Pointer_stringify(fail);

    qg.hasShortcutInstalled({
      success: function (res) {
        // If the icon does not exist, create an icon
        if (res == false) {
          qg.installShortcut({
            success: function () {
              // Perform user created icon rewards
              console.log("qg.installShortcut create icon success");
            },
            fail: function (err) {
              console.log("qg.installShortcut create icon fail");
            },
            complete: function () {
              console.log("qg.installShortcut create icon  called");
            },
          });
        } else {
          console.log("desktop icon has been created");
        }
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
          errMsg: res.errMsg,
          errCode: res.errCode,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "ShortcutResponseCallback", json);
      },
    });
  },

  QGInstallShortcut: function (success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var successID = Pointer_stringify(success);
    var failID = Pointer_stringify(fail);

    qg.installShortcut({
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          data: res,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
          errMsg: res,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
      },
    });
  },
  // bannerAd
  QGCreateBannerAd: function (adId, adUnitId, style) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    // var posIdStr = Pointer_stringify(posId);
    var adUnitIdStr = Pointer_stringify(adUnitId);
    var styleStr = Pointer_stringify(style);
    var adIdStr = Pointer_stringify(adId);

    var bannerAd;
    bannerAd = qg.createBannerAd({
      adUnitId: adUnitIdStr,
      style: {
        width: 820,
      },
      adIntervals: 30,
    });
    if (bannerAd) {
      mAdMap.set(adIdStr, bannerAd);
      bannerAd.onLoad(function () {
        var json = JSON.stringify({
          callbackId: adIdStr,
        });
        console.log("banner load success");
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_LOAD, json);
      });
      bannerAd.onError(function (err) {
        var json = JSON.stringify({
          callbackId: adId,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_ERROR, json);
      });
      bannerAd.show();
    }
  },
  // RewardedVideoAd
  QGCreateRewardedVideoAd: function (adId, adUnitId) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    // var posIdStr = Pointer_stringify(posId);
    var adUnitIdStr = Pointer_stringify(adUnitId);
    var adIdStr = Pointer_stringify(adId);

    var rewardedVideoAd = qg.createRewardedVideoAd({
      adUnitId: adUnitIdStr,
    });
    if (rewardedVideoAd) {
      mAdMap.set(adIdStr, rewardedVideoAd);
      rewardedVideoAd.load();
      rewardedVideoAd.onLoad(function () {
        console.log("rewardedVideoAd onload success");
        rewardedVideoAd.show();
        var json = JSON.stringify({
          callbackId: adIdStr,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_LOAD, json);
      });

      rewardedVideoAd.onClose(function (rec) {
        var json = JSON.stringify({
          callbackId: adIdStr,
          isEnded: rec.isEnded,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE_REWARDED, json);
      });
      rewardedVideoAd.onError(function (err) {
        console.error(" rewardedVideoAd.onError = " + JSON.stringify(err));
        var json = JSON.stringify({
          callbackId: adIdStr,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_ERROR, json);
      });
    }
  },
  // InterstitialAd
  QGCreateInterstitialAd: function (adId, adUnitId) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    // var posIdStr = Pointer_stringify(posId);
    var adUnitIdStr = Pointer_stringify(adUnitId);
    var adIdStr = Pointer_stringify(adId);

    var interstitialAd = qg.createInterstitialAd({
      adUnitId: adUnitIdStr,
    });
    if (interstitialAd) {
      mAdMap.set(adIdStr, interstitialAd);
      interstitialAd.onLoad(function () {
        console.log("Interstitial onload success");
        interstitialAd.show();
        var json = JSON.stringify({
          callbackId: adIdStr,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_LOAD, json);
      });
      interstitialAd.onClose(function () {
        var json = JSON.stringify({
          callbackId: adIdStr,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE, json);
      });
      interstitialAd.onError(function (err) {
        var json = JSON.stringify({
          callbackId: adIdStr,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_ERROR, json);
      });
    }
  },
  // CustomAd
  QGCreateCustomAd: function (adId, adUnitId, style) {
    console.log("QGCreateCustomAd ============================================== 0");
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    console.log("QGCreateCustomAd ============================================== 1");

    // var posIdStr = Pointer_stringify(posId);
    var adUnitIdStr = Pointer_stringify(adUnitId);
    var adIdStr = Pointer_stringify(adId);
    var styleStr = Pointer_stringify(style);

    var customAd = qg.createCustomAd({
      adUnitId: adUnitIdStr,
      style: {
        top: 100,
      },
    });
    if (customAd) {
      mAdMap.set(adIdStr, customAd);
      customAd.onLoad(function (rec) {
        console.log("QGCreateCustomAd ============================================== onLoad");

        var json = JSON.stringify({
          callbackId: adIdStr,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_LOAD, json);
      });
      customAd.onShow();
      // customAd.onClose(function () {
      //   var json = JSON.stringify({
      //     callbackId: adIdStr,
      //   });
      //   unityInstance.SendMessage(
      //     CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
      //     CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE,
      //     json
      //   );
      // });
      customAd.onHide(function () {
        var json = JSON.stringify({
          callbackId: adIdStr,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_HIDE, json);
      });
      customAd.onError(function (err) {
        console.log("QGCreateCustomAd ============================================== onError : " + err.errMsg);
        var json = JSON.stringify({
          callbackId: adIdStr,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_ERROR, json);
      });
    }
  },
  // GameBannerAd
  QGCreateGameBannerAd: function (adId, adUnitId) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    // var posIdStr = Pointer_stringify(posId);
    var adUnitIdStr = Pointer_stringify(adUnitId);
    var adIdStr = Pointer_stringify(adId);

    var gameBannerAd = qg.createGameBannerAd({
      adUnitId: adUnitIdStr,
    });
    if (gameBannerAd) {
      mAdMap.set(adIdStr, gameBannerAd);
      gameBannerAd
        .show()
        .then(function () {
          console.log("show success");
        })
        .catch(function (error) {
          console.log("show fail with:" + error.errCode + "," + error.errMsg);
        });
      gameBannerAd.onLoad(function () {
        var json = JSON.stringify({
          callbackId: adIdStr,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_LOAD, json);
      });
      // gameBannerAd.onClose(function () {
      //   var json = JSON.stringify({
      //     callbackId: adIdStr,
      //   });
      //   unityInstance.SendMessage(
      //    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
      //    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE,
      //    json
      //  );
      // });
      gameBannerAd.onError(function (err) {
        var json = JSON.stringify({
          callbackId: adIdStr,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_ERROR, json);
      });
    }
  },
  // GamePortalAd
  QGCreateGamePortalAd: function (adId, adUnitId) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    // var posIdStr = Pointer_stringify(posId);
    var adUnitIdStr = Pointer_stringify(adUnitId);
    var adIdStr = Pointer_stringify(adId);
    // var imageStr = Pointer_stringify(image);

    var gamePortalAd = qg.createGamePortalAd({
      adUnitId: adUnitIdStr,
    });
    if (gamePortalAd) {
      mAdMap.set(adIdStr, gamePortalAd);
      gamePortalAd
        .load()
        .then(function () {
          console.log("load success");
        })
        .catch(function (error) {
          console.log("load fail with:" + error.errCode + "," + error.errMsg);
        });
      gamePortalAd.onLoad(function () {
        console.log("gamePortalAd onload success");
        gamePortalAd
          .show()
          .then(function () {
            console.log("gamePortalAd show success");
          })
          .catch(function (error) {
            console.log("gamePortalAd show fail with:" + error.errCode + "," + error.errMsg);
          });
        var json = JSON.stringify({
          callbackId: adIdStr,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_LOAD, json);
      });
      gamePortalAd.onClose(function () {
        var json = JSON.stringify({
          callbackId: adIdStr,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE, json);
      });
      gamePortalAd.onError(function (err) {
        var json = JSON.stringify({
          callbackId: adIdStr,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_ERROR, json);
      });
    }
  },
  // GameDrawerAd
  QGCreateGameDrawerAd: function (adId, adUnitId, style) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    // var posIdStr = Pointer_stringify(posId);
    var adUnitIdStr = Pointer_stringify(adUnitId);
    var adIdStr = Pointer_stringify(adId);
    var styleStr = Pointer_stringify(style);

    var GameDrawerAd;
    if (styleStr) {
      GameDrawerAd = qg.createGameDrawerAd({
        adUnitId: adUnitIdStr,
        style: JSON.parse(styleStr),
      });
    } else {
      GameDrawerAd = qg.createGameDrawerAd({
        adUnitId: adUnitIdStr,
      });
    }
    if (GameDrawerAd) {
      mAdMap.set(adIdStr, GameDrawerAd);
      GameDrawerAd.show()
        .then(function () {
          console.log("GameDrawerAd show success");
        })
        .catch(function (error) {
          console.log("GameDrawerAd show fail with:" + error.errCode + "," + error.errMsg);
        });
      GameDrawerAd.onShow(function () {
        console.log("GameDrawerAd onShow success");
      });
      GameDrawerAd.onError(function (err) {
        var json = JSON.stringify({
          callbackId: adIdStr,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_ERROR, json);
      });
    }
  },

  QGShowAd: function (adId, success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    console.log("QGShowAd ============================= 1");

    var successID = Pointer_stringify(success);
    var failID = Pointer_stringify(fail);
    var adIdStr = Pointer_stringify(adId);

    var ad = mAdMap.get(adIdStr);

    if (ad) {
      ad.show()
        .then(function () {
          console.log("QGShowAd ============================= success");
          var json = JSON.stringify({
            callbackId: successID,
          });
          unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
        })
        .catch(function (err) {
          var errMsgStr = !err ? "" : err.data ? err.data.errMsg : err.errMsg;
          console.log("QGShowAd ============================= error : " + errMsgStr);
          var errCodeValue = !err ? "" : err.data ? err.data.errCode : err.errCode;
          var json = JSON.stringify({
            callbackId: failID,
            errMsg: errMsgStr,
            errCode: errCodeValue,
          });
          unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
        });
    } else {
      var json = JSON.stringify({
        callbackId: failID,
        errMsg: "ad is undefined",
        errCode: 404,
      });
      unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
    }
  },

  QGHideAd: function (adId, success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    var successID = Pointer_stringify(success);
    var failID = Pointer_stringify(fail);
    var adIdStr = Pointer_stringify(adId);

    var ad = mAdMap.get(adIdStr);

    if (ad) {
      var s = ad.hide();
      if (s) {
        s.then(function () {
          var json = JSON.stringify({
            callbackId: successID,
          });
          unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
        }).catch(function (err) {
          var errMsgStr = !err ? "" : err.data ? err.data.errMsg : err.errMsg;
          var errCodeValue = !err ? "" : err.data ? err.data.errCode : err.errCode;
          var json = JSON.stringify({
            callbackId: failID,
            errMsg: errMsgStr,
            errCode: errCodeValue,
          });
          unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
        });
      }
    } else {
      var json = JSON.stringify({
        callbackId: failID,
        errMsg: "ad is undefined",
        errCode: 404,
      });
      unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
    }
  },

  QGLoadAd: function (adId, success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    var successID = Pointer_stringify(success);
    var failID = Pointer_stringify(fail);
    var adIdStr = Pointer_stringify(adId);

    var ad = mAdMap.get(adIdStr);

    if (ad) {
      var s = ad.load();
      if (s) {
        s.then(function () {
          var json = JSON.stringify({
            callbackId: successID,
          });
          unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
        }).catch(function (err) {
          var errMsgStr = !err ? "" : err.data ? err.data.errMsg : err.errMsg;
          var errCodeValue = !err ? "" : err.data ? err.data.errCode : err.errCode;
          var json = JSON.stringify({
            callbackId: failID,
            errMsg: errMsgStr,
            errCode: errCodeValue,
          });
          unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
        });
      }
    } else {
      var json = JSON.stringify({
        callbackId: failID,
        errMsg: "ad is undefined",
        errCode: 404,
      });
      unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
    }
  },

  QGDestroyAd: function (adId) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    var adIdStr = Pointer_stringify(adId);

    var ad = mAdMap.get(adIdStr);

    if (ad) {
      ad.destroy();
      mAdMap.delete(adIdStr);
    }
  },

  QGIsShow: function (adId) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    var adIdStr = Pointer_stringify(adId);

    var ad = mAdMap.get(adIdStr);

    if (ad) {
      return ad.isShow();
    }
  },

  QGStorageSetIntSync: function (key, value) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var keyStr = Pointer_stringify(key);
    var valueStr = value + "";

    // qg.setStorageSync({
    // 	key: keyStr,
    // 	value: valueStr,
    // });
    localStorage.setItem(keyStr, valueStr);
  },

  QGStorageGetIntSync: function (key, defaultValue) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var keyStr = Pointer_stringify(key);
    var defaultValueStr = defaultValue + "";

    // var result = qg.getStorageSync({
    // 	key: keyStr,
    // 	default: defaultValueStr,
    // });
    var result = localStorage.getItem(keyStr);
    if (result == null) {
      result = defaultValueStr;
    }
    return parseInt(result);
  },

  QGStorageSetStringSync: function (key, value) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var keyStr = Pointer_stringify(key);
    var valueStr = Pointer_stringify(value);

    // qg.setStorageSync({
    // 	key: keyStr,
    // 	value: valueStr,
    // });
    localStorage.setItem(keyStr, valueStr);
  },

  QGStorageGetStringSync: function (key, defaultValue) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var keyStr = Pointer_stringify(key);
    var defaultValueStr = Pointer_stringify(defaultValue);

    var result = localStorage.getItem(keyStr);
    if (result == null) {
      result = defaultValueStr;
    }

    var bufferSize = lengthBytesUTF8(result) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(result, buffer, bufferSize);

    return buffer;
  },

  QGStorageSetFloatSync: function (key, value) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var keyStr = Pointer_stringify(key);
    var valueStr = value + "";

    // qg.setStorageSync({
    // 	key: keyStr,
    // 	value: valueStr,
    // });
    localStorage.setItem(keyStr, valueStr);
  },

  QGStorageGetFloatSync: function (key, defaultValue) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var keyStr = Pointer_stringify(key);
    var defaultValueStr = defaultValue + "";

    // var result = qg.getStorageSync({
    // 	key: keyStr,
    // 	default: defaultValueStr,
    // });
    var result = localStorage.getItem(keyStr);
    if (result == null) {
      result = defaultValueStr;
    }
    return reparseFloatsult;
  },

  QGStorageDeleteAllSync: function () {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    // qg.clearStorageSync();
    localStorage.clear();
  },

  QGStorageDeleteKeySync: function (key) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var keyStr = Pointer_stringify(key);
    // qg.deleteStorageSync({
    // 	key: keyStr,
    // });
    localStorage.removeItem(keyStr);
  },

  QGStorageHasKeySync: function (key) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var keyStr = Pointer_stringify(key);

    // var result = qg.getStorageSync({
    // 	key: keyStr,
    // });
    var result = localStorage.getItem(keyStr);

    return result === null ? false : true;
  },
  // pay
  QGPay: function (param, success, fail, complete) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    var paramStr = Pointer_stringify(param);
    var successID = Pointer_stringify(success);
    var failID = Pointer_stringify(fail);
    var completeID = Pointer_stringify(complete);

    qg.pay({
      appId: paramStr.appId,
      token: paramStr.token,
      timestamp: paramStr.timestamp,
      orderNo: paramStr.orderNo,
      paySign: paramStr.paySign,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          data: res.data,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "PayResponseCallback", json);
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          data: err.data,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "PayResponseCallback", json);
      },
      complete: function () {
        var json = JSON.stringify({
          callbackId: completeID,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "PayResponseCallback", json);
      },
    });
  },

  QGAccessFile: function (path) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    var pathStr = Pointer_stringify(path);
    var fs = qg.getFileSystemManager();
    var result = fs.access({
      path: pathStr,
      success: function (res) {
        console.log("QGAccessFile  have");
      },
      fail: function (err) {
        console.log("QGAccessFile  nohave");
      },
    });

    var bufferSize = lengthBytesUTF8(result) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(result, buffer, bufferSize);

    return buffer;
  },

  QGReadFile: function (uri, encoding, position, length, success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    var uriStr = Pointer_stringify(uri);
    var encodingStr = Pointer_stringify(encoding);
    var successID = Pointer_stringify(success);
    var failID = Pointer_stringify(fail);

    qg.readFile({
      uri: uriStr,
      encoding: encodingStr,
      position: position,
      length: length,
      success: function (data) {
        if (encodingStr == "binary") {
          mFileData[successID] = data.text;
        }
        var json = JSON.stringify({
          callbackId: successID,
          textStr: data.text,
          encoding: encodingStr,
          byteLength: data.text.byteLength,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "ReadFileResponseCallback", json);
      },
      fail: function (data, code) {
        var json = JSON.stringify({
          callbackId: failID,
          errCode: code,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "ReadFileResponseCallback", json);
      },
    });
  },

  QGGetFileBuffer: function (buffer, callBackId) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    var callBackIdStr = Pointer_stringify(callBackId);
    HEAPU8.set(new Uint8Array(mFileData[callBackIdStr]), buffer);
    delete mFileData[callBackIdStr];
  },

  QGWriteFile: function (filePath, data, encoding, success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var fs = qg.getFileSystemManager();
    var qgDir = qg.env.USER_DATA_PATH;
    var localFilePath = qgDir + "/my/file.txt";
    // var localDir = `${qgDir}/my`;
    var filePathStr = Pointer_stringify(filePath);
    var dataStrFinal = Pointer_stringify(data);
    // var appendStr = Pointer_stringify(append);
    var encodingStr = Pointer_stringify(encoding);
    var successID = Pointer_stringify(success);
    var failID = Pointer_stringify(fail);

    fs.writeFile({
      filePath: localFilePath,
      encoding: encodingStr,
      data: dataStrFinal,
      success: function (filePath) {
        var json = JSON.stringify({
          callbackId: successID,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "WriteFileResponseCallback", json);
      },
      fail: function (data, code) {
        var json = JSON.stringify({
          callbackId: failID,
          errCode: code,
        });
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "WriteFileResponseCallback", json);
      },
    });
  },
  QGQuitGame: function () {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    qg.exitApplication({
      success: function () {
        console.log("exitApplication success");
      },
      fail: function () {
        console.log("exitApplication fail");
      },
      complete: function () {
        console.log("exitApplication complete");
      },
    });
  },

  QGShowToast: function (content) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var contentStr = Pointer_stringify(content);
    qg.showToast({
      title: contentStr,
      icon: "none",
      duration: 2000,
    });
  },
  JsLog: function (content, logType) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var contentStr = Pointer_stringify(content);
    var logTypeStr = Pointer_stringify(logType);
    switch (logTypeStr) {
      case "0":
        console.log(contentStr);
        break;
      case "1":
        console.warn(contentStr);
        break;
      case "2":
        console.error(contentStr);
        break;
      default:
        console.log(contentStr);
        break;
    }
  },
  GetWebCurrentTime: function () {
    var dt = new Date().getTime();
    var currentTime = dt.toString();
    var bufferSize = lengthBytesUTF8(currentTime) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(currentTime, buffer, bufferSize);
    return buffer;
  },
  QGCreateCustomAdZone: function (adId1, adUnitId1, style1) {
    var isAreaLimit = false;
    var xhr = new XMLHttpRequest();
    xhr.open("get", "https://yxapi.tomatojoy.cn/getIp", true);
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.send();
    xhr.onreadystatechange = function () {
      console.log("请求状态" + xhr.readyState);
      if (xhr.readyState == 4) {
        if (xhr.status >= 200 && xhr.status < 400) {
          var json = JSON.parse(xhr.responseText);
          var data = json.data;

          var arr = ["北京", "深圳", "东莞", "上海", "南京", "杭州", "西安", "重庆", "武汉", "合肥", "厦门", "广东", "江苏", "成都市"];
          if (arr.indexOf(data.province) >= 0 || arr.indexOf(data.city) >= 0) {
            console.log("是屏蔽地区");
            isAreaLimit = true;
          } else {
            console.log("不是屏蔽地区");
            console.log("获取地区数据:" + data);
            isAreaLimit = false;
          }
        } else {
          //xhr = new XMLHttpRequest();
          //xhr.open("get", "https://yxapi.tomatojoy.cn/getIp", true);
          //xhr.setRequestHeader("Content-Type", "application/json");
          //xhr.send();
          console.log("请求失败.非200-400状态");
        }
      } else {
        console.log("请求失败  readyState!=4 ");
      }
    };

    var showCustomAdFunc = function (adId, adUnitId, style) {
      if (typeof qg == "undefined") {
        console.log("qg.minigame.jslib  qg is undefined");
        return;
      }

      if (!(mAdMap instanceof Map)) {
        mAdMap = new Map();
      }

      // var posIdStr = Pointer_stringify(posId);
      var adUnitIdStr = Pointer_stringify(adUnitId);
      var adIdStr = Pointer_stringify(adId);
      var styleStr = Pointer_stringify(style);
      console.log("area custom ad:", adUnitIdStr, adIdStr, styleStr);
      var customAd = qg.createCustomAd({
        adUnitId: adUnitIdStr,
        style: {
          top: 300,
        },
      });
      if (customAd) {
        mAdMap.set(adIdStr, customAd);
        customAd.onLoad(function (rec) {
          var json = JSON.stringify({
            callbackId: adIdStr,
          });
          unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_LOAD, json);
        });
        customAd.onShow();
        // customAd.onClose(function () {
        //   var json = JSON.stringify({
        //     callbackId: adIdStr,
        //   });
        //   unityInstance.SendMessage(
        //     CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        //     CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE,
        //     json
        //   );
        // });
        customAd.onHide(function () {
          var json = JSON.stringify({
            callbackId: adIdStr,
          });
          unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_HIDE, json);
        });
        customAd.onError(function (err) {
          var json = JSON.stringify({
            callbackId: adIdStr,
            errMsg: err.errMsg,
            errCode: err.errCode,
          });
          unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_ERROR, json);
        });
      }
    };

    if (!isAreaLimit) {
      console.log("limit area is false,show time");
      showCustomAdFunc(adId1, adUnitId1, style1);
    } else {
      console.log("limit area is true");
    }
  },
  QGRecord: function (duration1, sampleRate1, numberOfChannels1, encodeBitRate1) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var d = duration1 * 1000;
    if (mRecordManager == null) {
      mRecordManager = qg.getRecorderManager();
    }
    const options = {
      duration: d,
      sampleRate: sampleRate1,
      numberOfChannels: numberOfChannels1,
      encodeBitRate: encodeBitRate1,
      format: "aac",
      frameSize: 50,
    };
    var stopFun = function (data) {
      if (data == null) {
        console.warn("the record data is null");
        return;
      }
      console.log("data:", JSON.stringify(data));
      var recordUrl = data.tempFilePath;
      var innerAudioContext = qg.createInnerAudioContext();
      innerAudioContext.loop = false;
      innerAudioContext.volume = 1;
      innerAudioContext.autoplay = false;
      innerAudioContext.src = recordUrl;
      innerAudioContext.play();
    };
    mRecordManager.start(options);
    mRecordManager.onStop(stopFun);
  },
  QGPlayRecord: function () {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    if (mRecordManager == null) {
      return;
    }
    console.log("录音结束--->");
    mRecordManager.stop();
  },
};

autoAddDeps(QgGameBridge, "$mAdMap");
autoAddDeps(QgGameBridge, "$CONSTANT");
autoAddDeps(QgGameBridge, "$mFileData");
autoAddDeps(QgGameBridge, "$mRecordManager");

mergeInto(LibraryManager.library, QgGameBridge);
