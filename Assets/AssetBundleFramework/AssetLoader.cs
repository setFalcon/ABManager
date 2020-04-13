using System.Collections;
using UnityEngine;

namespace AssetBundleFramework {
    /// <summary>
    /// 管理/加载(cache)/卸载/查看指定的Ab包中存储的资源
    /// </summary>
    public class AssetLoader : System.IDisposable {
        // 当前的AssetBundle包
        private AssetBundle _currentAssetBundle;

        // AssetBundle包中加载过的Asset的缓存容器
        private Hashtable _cachedAsset;

        /// <summary>
        /// 构造器,进行初始化
        /// </summary>
        /// <param name="loadedAb">已经通过UnityWebRequestAssetBundle或者WWW加载的AssetBundle资源</param>
        public AssetLoader(AssetBundle loadedAb) {
            // 参数检查,检查是否传入空的AssetBundle
            if (loadedAb != null) {
                // 参数初始化
                _currentAssetBundle = loadedAb;
                _cachedAsset = new Hashtable();
            }
            else {
                // 未传入AssetBundle的情况
                Debug.LogError($"{GetType()}/构造函数中传入的参数loadAb=null,请检查参数输入");
            }
        }

        /// <summary>
        /// 加载Ab包中的指定资源
        /// </summary>
        /// <param name="assetName">要加载的资源名称</param>
        /// <param name="isCache">是否进行缓存处理</param>
        /// <returns>Ab包中的指定资源</returns>
        public Object LoadAsset(string assetName, bool isCache = false) {
            return LoadResource<Object>(assetName, isCache);
        }


        /// <summary>
        /// 加载Ab包中的指定资源
        /// </summary>
        /// <param name="assetName">要加载的资源名称</param>
        /// <param name="isCache">是否进行缓存处理</param>
        /// <typeparam name="T">加载的资源类型</typeparam>
        /// <returns></returns>
        private T LoadResource<T>(string assetName, bool isCache = false) where T : Object {
            // 检查缓存中是否存储了资源
            if (_cachedAsset.Contains(assetName)) {
                //返回缓存中的内容
                return _cachedAsset[assetName] as T;
            }

            // 资源未缓存,进行资源加载
            T loadedAsset = _currentAssetBundle.LoadAsset<T>(assetName);
            // 判断是否加入缓存集合
            if (loadedAsset != null && isCache) {
                // 成功加载
                _cachedAsset.Add(assetName, loadedAsset);
            }
            else if (loadedAsset == null) {
                // 未能成功加载
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
            // 参数检查,判定asset是否为空
            if (asset != null) {
                // 卸载Asset资源
                Resources.UnloadAsset(asset);
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
        /// 查询当前AssetBundle包中所包含的所有资源
        /// </summary>
        public string[] RetrivalAllAssetName() {
            return _currentAssetBundle.GetAllAssetNames();
        }
    } // Class_End
} // Namespace_End