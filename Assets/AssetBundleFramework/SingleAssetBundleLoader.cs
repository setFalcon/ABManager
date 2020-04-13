using System.Collections;
using System.IO;
using AssetBundleFramework.Tools;
using UnityEngine;

namespace AssetBundleFramework {
    /// <summary>
    /// 使用WWW/UnityWebRequestAssetBundle加载单一AssetBundle
    /// </summary>
    public class SingleAssetBundleLoader : System.IDisposable {
        // 引用:资源加载类,通过加载Asset的方法加载Ab包中的Asset
        private AssetLoader _loader;

        // 委托:AssetBundle资源加载完成
        private LoadComplete _loadCompleteHandler;

        // AssetBundle名称
        private string _abName;

        // AssetBundle下载路径
        private string _downloadPath;

        /// <summary>
        /// 构造函数,初始化字段
        /// </summary>
        public SingleAssetBundleLoader(string abName, LoadComplete completeHandler) {
            _loader = null;
            _abName = abName;
            _loadCompleteHandler = completeHandler;
            _downloadPath = Path.Combine(PathTools.GetWWWPath(), _abName);
        }

        /// <summary>
        /// 加载AssetBundle
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadAssetBundleWWW() {
#pragma warning disable 618
            using (WWW www = new WWW(_downloadPath)) {
                yield return www;
                // 下载完毕
                if (www.progress >= 1) {
                    AssetBundle downloadAb = www.assetBundle;
                    if (downloadAb != null) {
                        // 实例化引用
                        _loader = new AssetLoader(downloadAb);
                        // AssetBundle下载完毕,调用委托方法
                        _loadCompleteHandler?.Invoke(_abName);
                    }
                    else {
                        Debug.LogError($"{GetType()}/LoadAssetBundleWWW方法使用参数路径" +
                                       $"_downloadPath = {_downloadPath}下载AssetBundle失败,请检查输入");
                    }
                }
            } // using_End
#pragma warning restore 618
        }

        /// <summary>
        /// 从AssetBundle中加载Asset资源
        /// </summary>
        /// <param name="assetName">Asset资源名称</param>
        /// <param name="isCache">是否缓存</param>
        /// <returns>Asset资源</returns>
        public Object LoadAsset(string assetName, bool isCache) {
            if (_loader != null) {
                return _loader.LoadAsset(assetName, isCache);
            }

            // 加载失败
            Debug.Log($"{GetType()}/LoadAsset方法加载位于" +
                      $"{_loader}加载器中的{assetName}资源失败");
            return null;
        }

        /// <summary>
        /// 卸载Ab包中的资源
        /// </summary>
        /// <param name="asset">要卸载的资源</param>
        public void UnloadAsset(Object asset) {
            // 参数检查:加载器检查
            if (_loader != null) {
                _loader.UnloadAsset(asset);
            }
            else {
                Debug.LogError($"{GetType()}/UnloadAsset方法因为加载器没有赋值故此无法卸载指定的资源");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() {
            if (_loader != null) {
                _loader.Dispose();
                _loader = null;
            }
            else {
                Debug.LogError($"{GetType()}/Dispose方法因为资源加载器没有赋值故此无法卸载回收相应资源");
            }
        }

        /// <summary>
        /// 卸载当前的AssetBundle资源包且卸载与其相关的所有资源
        /// </summary>
        public void DisposeAll() {
            if (_loader != null) {
                _loader.DisposeAll();
                _loader = null;
            }
            else {
                Debug.LogError($"{GetType()}/DisposeAll方法因为资源加载器没有赋值故此无法卸载回收相应资源");
            }
        }

        /// <summary>
        /// 查询当前AssetBundle包中所包含的所有资源
        /// </summary>
        /// <returns>所有资源的名称数组</returns>
        public string[] RetrivalAllAssetName() {
            return _loader?.RetrivalAllAssetName();
        }
    } // Class_End
} // Namespace_End