mergeInto(LibraryManager.library, {
	Init: function (adUnitIdStr) {
		console.log("enter Init");
		window.isShowAdZone = true;

		if (window.rewardedAd != null) return;
		
		window.rewardedAd = qg.createRewardedVideoAd({
			adUnitId: Pointer_stringify(adUnitIdStr),
		});

		window.rewardedAd.onError(function (err) {
			console.log("激励视频广告加载失败", JSON.stringify(err));
		});

		window.rewardedAd.onLoad(function (res) {
			console.log("激励视频广告加载完成-onload触发", JSON.stringify(err));
		});
		
		window.rewardedAd.load();
		
		window.onresize=function(){
			console.log("resize the screen");
			var sysInfo = qg.getSystemInfoSync(); 
			window.resizeTo(sysInfo.screenWidth, sysInfo.screenHeight);
		}
	},
	ZDTXWebRequest:function(packageId,channel){
	  console.log("菜");
	  packageId=Pointer_stringify(packageId);
	  channel=Pointer_stringify(channel);
	  console.log(packageId);
	  console.log(channel);
	  var url="http://datacenter.zywxgames.com:15855/api/index/params?pkm="+packageId+"&canshu="+channel+"&url=new&yys=yd";
	  var request=new XMLHttpRequest();
	  request.open("get", url,true);
	  request.setRequestHeader("Content-Type", "application/json");
	  request.send();
	  request.onreadystatechange=function(){
		if (request.readyState == 4 && request.status >= 200&&request.status<400) {
			console.log(request.responseText);
			unityInstance.SendMessage("AdCanvas","ZDTXJSReceiver",request.responseText);
		}
	  }
	},
	WanBanWebRequest:function(productId,channel){
		console.log("菜");
		productId = Pointer_stringify(productId);
		channel = Pointer_stringify(channel);
		var URL = "http://mf777.top/api/mf0202";
		var request = new XMLHttpRequest();
		request.open('POST', URL, true);
		request.setRequestHeader("Content-Type", "application/json");
		var data={};
		data['product_id']= productId;
		data['channel_id'] = channel;
		data['belonging_province'] = '';
		request.send(JSON.stringify(data));
		request.onreadystatechange = function () {
			if (request.readyState == 4 && request.status>=200&& request.status < 400) {
				console.log(request.responseText);
				unityInstance.SendMessage("AdCanvas","WanBanJSReceiver",request.responseText);
		  }
		}
	},
	LuZongInit: function (baoXiangPosIdStr) {
		//判断宝箱是否开启
		if (qg.createNewNativeAd) {
			var native_Ad = qg.createNewNativeAd({
				posId: Pointer_stringify(baoXiangPosIdStr),
			});

			native_Ad.onLoad(function (res) {
				console.log("原生自渲染2.0广告加载完成-onload触发", JSON.stringify(res));
				if (native_Ad) native_Ad.offLoad();
				unityInstance.SendMessage("AdCanvas", "LuZongBoxSwitchReceiver", "On");
			});

			//监听原生自渲染2.0广告加载失败事件
			native_Ad.onError(function (errMsg) {
				console.log("原生自渲染2.0广告加载失败 onError ---------> errMsg: " + JSON.stringify(errMsg));
				var err_Msg = errMsg;
				var err_Code = err_Msg["errCode"];

				console.log("err_Code : ", err_Code);
				unityInstance.SendMessage("AdCanvas", "LuZongBoxSwitchReceiver",  ""+err_Code );
				if (native_Ad) native_Ad.offError();
			});
		}
    },
	AreaShield:function(){
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
					//console.log("获取地区数据:" + data.city);
					console.log("获取地区数据:" +  JSON.stringify(data));
					var arr = [ "厦门市", "北京市","上海市", "重庆市","西安市","南京市","杭州市", "武汉市", "长沙市", "成都市", "广州市","深圳市","东莞市","苏州市","福州市"];
					if (arr.indexOf(data.province) >=0 || arr.indexOf(data.city) >=0) {
          
						console.log("是屏蔽地区");
						unityInstance.SendMessage("AdCanvas", "AreaReceiver", "NoAd");
            
					} else {
          
						console.log("不是屏蔽地区");
						//console.log("获取地区数据:" + data);
						unityInstance.SendMessage("AdCanvas", "AreaReceiver", "HasAd");
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
		console.log("初始化地区结束");
	},
	ShowBannerAd: function (bannerAdUnitIdStr, showDateStr) {
		console.log("enter ShowBannerAd");
		if (showDateStr != null && showDateStr != "") {
		  currentDate = new Date();
		  showDate = new Date(Pointer_stringify(showDateStr));

		  if (showDate - currentDate > 0){
			  return;
		  }
		}

		var bannerAdUnitId = Pointer_stringify(bannerAdUnitIdStr);
		if (window.bannerAd != null) {
			window.bannerAd.hide();
		}

		window.bannerAd = qg.createBannerAd({
			adUnitId: bannerAdUnitId,
			style: {
					top: 0,
					left: (qg.getSystemInfoSync().screenWidth - 1080) * 0.5
					}
		});

		window.bannerAd.onError(function (err) {
			console.log(err.errCode + "--" + err.errMsg);
		});

		window.bannerAd
			.show()
			.then(function () {
				console.log("banner广告展示成功");
			})
			.catch(function (err) {
				console.log("banner广告展示失败", err.toString());
			});
	},

	ShowCustomAd: function (adUnitIdStr, showDateStr, orientation) {
		console.log("enter ShowCustomAd");
		if (showDateStr != null && showDateStr != "") {
			currentDate = new Date();
			showDate = new Date(Pointer_stringify(showDateStr));

			// 还没到屏蔽结束时间直接返回
			if (showDate - currentDate > 0)return;
		}

		// 横竖屏
		var sysInfo = qg.getSystemInfoSync();
		var _top;
		var _left;
		if (orientation == 0) {
			_top = 200;
			_left = (sysInfo.screenWidth - 1080) / 2;
		} else {
			_top = sysInfo.screenHeight * 0.5;
			_left = sysInfo.screenWidth * 0.1;
		}

		if (qg.createCustomAd) {
			window.customAd = qg.createCustomAd({
				adUnitId: Pointer_stringify(adUnitIdStr),
				style: { top: _top },
			});

			window.customAd.onLoad(function () {
				console.log("customAd广告加载成功");
			});

			window.customAd.onError(function (err) {
				console.log("模板原生加载失败", err.toString());
			});

			window.customAd
				.show()
				.then(function () {
					console.log("模板原生展示成功");
				})
				.catch(function (err) {
				console.log("模板原生展示失败", err.toString());
				});
		}
	},

  ShowCustomAdZone: function (adUnitIdStr, showDateStr, orientation) {
    console.log("window.isShowAdZone: " + window.isShowAdZone);
    if (window.isShowAdZone) {
      console.log("展示原生--限制");
      if (showDateStr != null && showDateStr != "") {
        currentDate = new Date();
        showDate = new Date(Pointer_stringify(showDateStr));

        // 还没到屏蔽结束时间直接返回
        if (showDate - currentDate > 0) {
          return;
        }
      }

      // 横竖屏
      var sysInfo = qg.getSystemInfoSync();
      var _top;
      var _left;
      if (orientation == 0) {
        _top = 200;
        _left = (sysInfo.screenWidth - 1080) / 2;
      } else {
        _top = sysInfo.screenHeight * 0.55;
        _left = sysInfo.screenWidth * 0.1;
      }

      if (qg.createCustomAd) {
        window.customAdZone = qg.createCustomAd({
          adUnitId: Pointer_stringify(adUnitIdStr),
          style: { left: _left, top: _top },
        });

        window.customAdZone.onLoad(function () {
          console.log("customAd广告加载成功");
        });

        window.customAdZone.onError(function (err) {
          console.log("模板原生加载失败", err.toString());
        });

        window.customAdZone
          .show()
          .then(function () {
            console.log("模板原生展示成功");
          })
          .catch(function (err) {
            console.log("模板原生展示失败", err.toString());
          });
      }
    }
  },

	HideCustomAd: function () {
		console.log("enter HideCustomAd");
		if (window.customAd != null) {
			window.customAd.hide();
		}
	},
  ShowNative: function (posIdStr, orientation) {
    if (qg.createNewNativeAd) {
      var sysInfo = qg.getSystemInfoSync(); //获取系统信息
      //   console.log("on getSystemInfoSync: success =" + JSON.stringify(sysInfo));

      var _top = sysInfo.screenHeight * 0.3;
      var _left = (sysInfo.screenWidth - 780) * 0.5;

      if (orientation == 0) {
        _top = 100;
        _left = (sysInfo.screenWidth - 780) * 0.5;
      }

      window.native_Ad = qg.createNewNativeAd({
        posId: Pointer_stringify(posIdStr),
      });

      window.native_Ad.onLoad(function (res) {
          console.log("原生自渲染2.0广告加载完成-onload触发", JSON.stringify(res));

        if (res && res.adList) {
          var arr_FormType = res.adList[0].formType;
          var _formType = arr_FormType[0];

          if (arr_FormType.length > 0) {
            if (arr_FormType.indexOf(2) >= 0) {
              _formType = 2;
            }
          }

          //  console.log(`原生自渲染广告2.0   left:${_left}   top:${_top}`);

          var data = {
            adId: res.adList[0].adId,
            formType: _formType,
            style: {
              //可设置广告容易的相对位置，可选：top、bottom、left、right、center，如：top|center
              //（注：设置left时，gravity的left和right不生效且center表示垂直居中；设置top时gravity的top和bottom不生效且center表示水平居中）
              // gravity: "center",
              top: _top,
              left: _left,
            },
          };

          window.native_Ad
            .show(data)
            .then(function () {
                console.log("原生自渲染2.0广告展示完成");
            })
            .catch(function (err) {
              // console.warn(`Native_Ad show error:${JSON.stringify(err)}`);
            });
        }

        if (window.native_Ad) {
          //取消监听原生自渲染2.0广告加载结束事件
          window.native_Ad.offLoad();
          //console.log("原生自渲染2.0广告 移除广告加载事件");
        }
      });

      //监听原生自渲染2.0广告加载失败事件
      window.native_Ad.onError(function (errMsg) {
        console.log("原生自渲染2.0广告加载失败 onError ---------> errMsg: " + JSON.stringify(errMsg));
        var err_Msg = errMsg;
        var err_Code = err_Msg["errCode"];

          console.log("err_Code : ", err_Code);

        if (window.native_Ad) window.native_Ad.offError();
      });
    }
  },

  HideNative: function () {
    if (window.native_Ad) {
      //console.log("Hide_NativeCustom_Ad ----------> 隐藏原生自渲染2.0广告");
      window.native_Ad.hide(); //（某些原生自渲染2.0广告无法隐藏）
      window.native_Ad.destroy();
      window.native_Ad = null;
    }
  },

  ShowNativeIcon: function (posIdStr, orientation) {
    if (qg.createNewNativeAd) {
      var sysInfo = qg.getSystemInfoSync(); //获取系统信息
      //   console.log("on getSystemInfoSync: success =" + JSON.stringify(sysInfo));

      var _top = sysInfo.screenHeight * 0.3;
      var _left = (sysInfo.screenWidth - 180) * 0.5 + 650;//+650 从中间往右边移659

      window.native_Icon = qg.createNewNativeAd({
        posId: Pointer_stringify(posIdStr),
      });

      window.native_Icon.onLoad(function (res) {
          console.log("原生自渲染2.0广告 Icon 加载完成-onload触发", JSON.stringify(res));

        if (res && res.adList) {
          var arr_FormType = res.adList[0].formType;
          var _formType = arr_FormType[0];

            //console.log(`原生自渲染广告2.0  Icon   left:${_left}   top:${_top}`);

          var data = {
            adId: res.adList[0].adId,
            formType: _formType,
            style: {
              //可设置广告容易的相对位置，可选：top、bottom、left、right、center，如：top|center
              //（注：设置left时，gravity的left和right不生效且center表示垂直居中；设置top时gravity的top和bottom不生效且center表示水平居中）
              // gravity: "center",
              top: _top,
              left: _left,
            },
          };

          window.native_Icon
            .show(data)
            .then(function () {
                console.log("原生自渲染2.0广告 Icon 展示完成");
            })
            .catch(function (err) {
              //console.warn(`Native_Icon show error:${JSON.stringify(err)}`);
            });
        }

        if (window.native_Icon) {
          //取消监听原生自渲染2.0广告加载结束事件
          window.native_Icon.offLoad();
          //console.log("原生自渲染2.0广告 移除广告加载事件");
        }
      });

      //监听原生自渲染2.0广告加载失败事件
      window.native_Icon.onError(function (errMsg) {
        console.log("原生自渲染2.0广告 Icon 加载失败 onError ---------> errMsg: " + JSON.stringify(errMsg));
        var err_Msg = errMsg;
        var err_Code = err_Msg["errCode"];

          console.log("err_Code : ", err_Code);

        if (window.native_Icon) window.native_Icon.offError();
      });
    }
  },

  HideNativeIcon: function () {
    if (window.native_Icon) {
      //console.log("Hide_NativeCustom_Ad ----------> 隐藏原生自渲染2.0广告");
      window.native_Icon.hide(); //（某些原生自渲染2.0广告无法隐藏）
      window.native_Icon.destroy();
      window.native_Icon = null;
    }
  },
    
	ShowRewardAd: function (callBackObjectName, callBackMethodName, callBackParam) {
		console.log("enter ShowRewardAd");
		var objectName = Pointer_stringify(callBackObjectName);
		var methodName = Pointer_stringify(callBackMethodName);
		var param = Pointer_stringify(callBackParam);

		window.rewardedAd
			.show()
			.then(function () {
				console.log("激励视频广告展示完成");
			})
			.catch(function (err) {
				console.log("激励视频广告展示失败", JSON.stringify(err));
				qg.showToast({
				//提示内容
				title: "暂无广告,稍后再试",
				duration: 2000,
			});
		});

		window.rewardedAd
			.onClose(function (res) {
				console.log("视频广告关闭回调");
			if (res && res.isEnded) {
				console.log("正常播放结束，可以下发游戏奖励");
				unityInstance.SendMessage(objectName, methodName, param);
			} else {
				console.log("播放中途退出，不下发游戏奖励");
			}
			window.rewardedAd.offClose();
			window.rewardedAd.load();
			unityInstance.SendMessage("AdCanvas", "RewardCallback");
		});
	},

	CreateShortcutButton: function () {
		console.log("enter CreateShortcutButton");
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

	ExitApplication: function () {
		console.log("enter app exit");
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
});
