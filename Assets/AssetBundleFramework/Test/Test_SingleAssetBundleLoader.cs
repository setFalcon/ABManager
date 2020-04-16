using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetBundleFramework.Test {
    /// <summary>
    /// SingleAssetBundleLoader的功能测试类
    /// SingleAssetBundleLoader只适合没有依赖关系的AssetBundle进行直接加载
    /// </summary>
    public class Test_SingleAssetBundleLoader : MonoBehaviour {
        // 此部分展示了如何使用SingleAssetBundleLoader加载一个普通无依赖关系的资源

        #region 简单无依赖关系资源加载

        private SingleAssetBundleLoader _assetBundleLoader; // SingleAssetBundleLoader引用
        private string _abName = "test_scene/prefabs.ab"; // 需要加载的Ab包
        private string _simpleAssetName = "Cylinder.prefab"; // 需要加载的资源名称

        private void Start() {
            _assetBundleLoader = new SingleAssetBundleLoader(_abName, LoadComplete); // 创建实例
            StartCoroutine(_assetBundleLoader.LoadABWWW()); // 加载Ab包
        }

        private void LoadComplete(string abName) { // 完成Ab包加载的回调方法
            Object cloneObject = _assetBundleLoader.LoadAsset(_simpleAssetName, false); // 加载资源并实例化
            Instantiate(cloneObject);
        }

        private void OnDestroy() {
            _assetBundleLoader.DisposeAll(); // 释放无用资源
        }

        #endregion

        //并不推荐使用此方法加载有依赖关系资源,手动管理资源的关系要书写的代码过于复杂

        #region 复杂有依赖关系资源加载

//        private SingleAssetBundleLoader _assetBundleLoader; // SingleAssetBundleLoader引用
//        private string _abName = "test_scene/prefabs.ab"; // 需要加载的Ab包
//        private string _dependAbName1 = "test_scene/textures.ab"; // 与prefabs.ab相关的Ab包
//        private string _dependAbName2 = "test_scene/materials.ab";
//        private string _assetName = "Cube.prefab"; // 需要加载的资源名称
//        private void Start() {
//            SingleAssetBundleLoader dependLoader1 = new SingleAssetBundleLoader(_dependAbName1, LoadDependComplete1);
//            StartCoroutine(dependLoader1.LoadABWWW()); // 先加载依赖关系1
//        }
//        private void LoadDependComplete1(string abName) { //完成依赖关系1加载的回调方法
//            Debug.Log("依赖包1加载完毕");
//            SingleAssetBundleLoader dependLoader2 = new SingleAssetBundleLoader(_dependAbName2, LoadDependComplete2);
//            StartCoroutine(dependLoader2.LoadABWWW()); // 再加载依赖关系2
//        }
//        private void LoadDependComplete2(string abName) { //完成依赖关系2加载的回调方法
//            Debug.Log("依赖包2加载完毕");
//            _assetBundleLoader = new SingleAssetBundleLoader(_abName, LoadComplete);
//            StartCoroutine(_assetBundleLoader.LoadABWWW()); // 最后加载需要加载的AssetBundle包
//        }
//        private void LoadComplete(string abName) { //完成最终包加载的回调方法
//            Instantiate(_assetBundleLoader.LoadAsset(_assetName, false)); // 获取最终的资源并实例化
//        }
//        private void OnDestroy() {
//            _assetBundleLoader.DisposeAll(); // 清理无用镜像资源和内存资源
//        }

        #endregion
    }
}