using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetBundleFramework.Test {
    /// <summary>
    /// SingleAssetBundleLoader的功能测试类
    /// </summary>
    public class Test_SingleAssetBundleLoader : MonoBehaviour {
        #region 简单无依赖关系资源加载

//        // 引用类
//        private SingleAssetBundleLoader _assetBundleLoader;
//
//        // Ab包名称
//        private string _abName = "test_scene/prefabs.ab";
//
//        // Ab包需要加载的资源名称
//        private string _simpleAssetName = "Cylinder.prefab";
//        private void Start() {
//            _assetBundleLoader = new SingleAssetBundleLoader(_abName, LoadComplete);
//            // 加载ab包
//            StartCoroutine(_assetBundleLoader.LoadAssetBundleWWW());
//        }
//
//        /// <summary>
//        /// 加载完成的注册方法
//        /// </summary>
//        private void LoadComplete(string abName) {
//            Debug.Log("加载完成");
//            // 加载简单没有引用的资源
//            Object cloneObject = _assetBundleLoader.LoadAsset(_simpleAssetName, false);
//            // 克隆此对象
//            Instantiate(cloneObject);
//        }

        #endregion

        #region 复杂有依赖关系资源加载

        // 引用类
        private SingleAssetBundleLoader _assetBundleLoader;

        // Ab包名称
        private string _abName = "test_scene/prefabs.ab";
        private string _dependAbName1 = "test_scene/textures.ab";
        private string _dependAbName2 = "test_scene/materials.ab";

        // Ab包需要加载的资源名称
        private string _assetName = "Cube.prefab";


        private void Start() {
            SingleAssetBundleLoader dependLoader1 = new SingleAssetBundleLoader(_dependAbName1,LoadDependComplete1);
            StartCoroutine(dependLoader1.LoadAssetBundleWWW());
        }
        //加载完成的回调方法
        private void LoadDependComplete1(string abName) {
            Debug.Log("依赖包1加载完毕");
            SingleAssetBundleLoader dependLoader2 = new SingleAssetBundleLoader(_dependAbName2,LoadDependComplete2);
            StartCoroutine(dependLoader2.LoadAssetBundleWWW());

        }

        private void LoadDependComplete2(string abName) {
            Debug.Log("依赖包2加载完毕");
            _assetBundleLoader = new SingleAssetBundleLoader(_abName,LoadComplete);
            StartCoroutine(_assetBundleLoader.LoadAssetBundleWWW());
        }

        private void LoadComplete(string abName) {
            Instantiate(_assetBundleLoader.LoadAsset(_assetName,false));
        }

        #endregion
    }
}