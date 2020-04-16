using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetBundleFramework {
    /// <summary>
    /// 管理AssetBundle包中资源加载的封装类
    /// AssetBundleLoader中包含AssetLoader类型的实例引用
    /// AssetBundleLoader会使用AssetLoader中的加载方法加载AssetBundle中的所有资源
    /// 最终的接口会暴露在AssetBundleLoader对象的实例中
    /// 即加载AssetBundle中的资源时,您完全不需要考虑AssetLoader中资源加载方法的的实现细节
    /// </summary>
    public class AssetLoader : IDisposable {
        private AssetBundle _currentAssetBundle; // 资源加载的来源:AssetBundle包的引用
        private Hashtable _cachedAsset; // _currentAssetBundle包中已经载入过的Asset实例缓存池

        /// <summary>
        /// AssetLoader构造器,进行资源加载相关的初始化
        /// </summary>
        /// <param name="loadedAb">已经通过UnityWebRequestAssetBundle或者WWW方式加载完毕的AssetBundle资源</param>
        public AssetLoader(AssetBundle loadedAb) {
            if (loadedAb != null) { // 检查传入的AssetBundle是否有效
                _currentAssetBundle = loadedAb; // 有效则初始化AssetBundle引用以及Asset缓存池
                _cachedAsset = new Hashtable();
            }
            else {
                Debug.LogError($"{GetType()}/构造函数中传入的参数loadAb=null,请检查参数输入"); // 未传入有效的AssetBundle
            }
        }

        /// <summary>
        /// 加载AssetBundle包中的指定名称的资源,并表明是否需要进行缓存
        /// </summary>
        /// <param name="assetName">加载的资源名称</param>
        /// <param name="isCache">缓存处理</param>
        /// <returns>Ab包中的指定资源</returns>
        public Object LoadAsset(string assetName, bool isCache = false) {
            return LoadResource<Object>(assetName, isCache);
        }


        /// <summary>
        /// 加载AssetBundle包中的指定名称的资源的泛型实现
        /// </summary>
        /// <param name="assetName">加载的资源名称</param>
        /// <param name="isCache">缓存处理</param>
        /// <typeparam name="T">加载的资源类型</typeparam>
        /// <returns></returns>
        private T LoadResource<T>(string assetName, bool isCache = false) where T : Object {
            if (_cachedAsset.Contains(assetName)) { // 缓存中存储了要加载的资源
                return _cachedAsset[assetName] as T; //返回缓存中的内容
            }

            T loadedAsset = _currentAssetBundle.LoadAsset<T>(assetName); // 资源未缓存,进行资源加载
            if (loadedAsset != null && isCache) { // 判断是否需要将加载的资源加入缓存集合
                _cachedAsset.Add(assetName, loadedAsset); // 资源需要被缓存
            }
            else if (loadedAsset == null) { // 资源加载失败
                Debug.LogError(
                    $"{GetType()}/LoadResource<T>方法根据参数assetName={assetName}" +
                    $"无法在AssetBundle:{_currentAssetBundle}中加载相关资源");
            }

            return loadedAsset;
        }

        /// <summary>
        /// 卸载指定的Asset资源
        /// </summary>
        /// <param name="asset">要卸载的资源</param>
        /// <returns>卸载是否成功</returns>
        public bool UnloadAsset(Object asset) {
            if (asset != null) {// 判定传入的asset是否为空
                Resources.UnloadAsset(asset); // 卸载此Asset资源
                return true;
            }

            // 释放Asset资源失败
            Debug.LogError($"{GetType()}/UnlLoadAsset方法传入的参数asset=null,无法卸载空资源");
            return false;
        }

        /// <summary>
        /// 释放当前的AssetBundle镜像资源
        /// </summary>
        public void Dispose() {
            _currentAssetBundle.Unload(false);
        }

        /// <summary>
        /// 释放当前的AssetBundle镜像资源和内存资源
        /// </summary>
        public void DisposeAll() {
            _currentAssetBundle.Unload(true);
        }

        /// <summary>
        /// 查询当前AssetBundle包中所包含的所有资源名称
        /// </summary>
        public string[] RetrivalAllAssetName() {
            return _currentAssetBundle.GetAllAssetNames();
        }
    } // Class_End
} // Namespace_End