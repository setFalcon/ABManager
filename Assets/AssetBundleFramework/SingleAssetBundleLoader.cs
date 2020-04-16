using System;
using System.Collections;
using System.IO;
using AssetBundleFramework.Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetBundleFramework {
    /// <summary>
    /// 单一AssetBundle加载器,使用SingleAssetBundleLoader可以加载位于存储设备中的AssetBundle
    /// 加载AssetBundle的方式采用了WWW/UnityWebRequest实现,并添加了事件处理机制
    /// SingleAssetBundleLoader包含了AssetLoader对象的引用
    /// 所以SingleAssetBundleLoader不仅能够加载AssetBundle,也可以实现对Asset的加载卸载等
    /// 在使用SingleAssetBundleLoader加载Asset时无需考虑AssetLoader加载方法的具体实现
    /// ----------------------------------------------------------------------------------
    /// 注意:
    ///     SingleAssetBundleLoader只适用于单一AssetBundle资源的加载
    ///     对于有依赖关系的AssetBundle的加载不适合用SingleAssetBundleLoader
    ///     使用SingleAssetBundleLoader加载有依赖关系的AssetBundle需要您手动管理依赖关系
    ///     且加载过程十分复杂,详情请参考Test_SingleAssetBundleLoader的相关代码
    /// </summary>
    public class SingleAssetBundleLoader : IDisposable {
        private AssetLoader _assetLoader; // AssetLoader的引用,使用其加载AssetBundle包中的Asset
        private LoadComplete _loadCompleteHandler; // AssetBundle加载完成的委托
        private string _abName; // 要加载的AssetBundle名称
        private string _downloadPath; // AssetBundle的存储路径,使用WWW/UnityWebRequest加载AssetBundle

        /// <summary>
        /// SingleAssetBundleLoader的构造函数
        /// </summary>
        public SingleAssetBundleLoader(string abName, LoadComplete completeHandler) {
            _assetLoader = null;
            _abName = abName;
            _loadCompleteHandler = completeHandler;
            _downloadPath = Path.Combine(PathTools.GetWWWPath(), _abName);
        }

        /// <summary>
        /// 使用WWW类加载本地AssetBundle的协程,由于WWW被弃用,所以此方法已经不再推荐使用了
        /// </summary>
        /// <returns></returns>
        [Obsolete("Replace with LoadABUnityWebRequestAssetBundle")]
        public IEnumerator LoadABWWW() {
            using (WWW www = new WWW(_downloadPath)) { // 创建下载Ab包使用的WWW对象
                yield return www;
                if (www.progress >= 1) { // 下载完毕
                    AssetBundle downloadAb = www.assetBundle; // 从下载的资源中提取出AssetBundle
                    if (downloadAb != null) {
                        _assetLoader = new AssetLoader(downloadAb); // AssetLoader实例化,用于加载资源
                        _loadCompleteHandler?.Invoke(_abName); // AssetBundle下载完毕,调用委托方法
                    }
                    else {
                        Debug.LogError($"{GetType()}/LoadAssetBundleWWW方法使用参数路径" +
                                       $"_downloadPath = {_downloadPath}下载AssetBundle失败,请检查输入");
                    }
                }
            } // using_End
        }

        /// <summary>
        /// 从SingleAssetBundleLoader实例中加载相应AssetBundle中的Asset资源
        /// </summary>
        /// <param name="assetName">Asset资源名称</param>
        /// <param name="isCache">是否缓存</param>
        /// <returns>加载完毕的Asset资源</returns>
        public Object LoadAsset(string assetName, bool isCache) {
            if (_assetLoader != null) {
                return _assetLoader.LoadAsset(assetName, isCache); // 返回加载完成的Asset资源
            }

            // 加载失败
            Debug.Log($"{GetType()}/LoadAsset方法加载位于" +
                      $"{_assetLoader}加载器中的{assetName}资源失败");
            return null;
        }

        /// <summary>
        /// 卸载已经加载的Asset
        /// </summary>
        /// <param name="asset">要卸载的Asset</param>
        public void UnloadAsset(Object asset) {
            if (_assetLoader != null) { // 检查加载器是否为空
                _assetLoader.UnloadAsset(asset); // 卸载资源
            }
            else {
                Debug.LogError($"{GetType()}/UnloadAsset方法因为加载器没有赋值故此无法卸载指定的资源");
            }
        }

        /// <summary>
        /// 释放AssetBundle镜像资源
        /// </summary>
        public void Dispose() {
            if (_assetLoader != null) {
                _assetLoader.Dispose();
                _assetLoader = null;
            }
            else {
                Debug.LogError($"{GetType()}/Dispose方法因为资源加载器没有赋值故此无法卸载回收相应资源");
            }
        }

        /// <summary>
        /// 卸载AssetBundle的镜像资源和内存资源
        /// </summary>
        public void DisposeAll() {
            if (_assetLoader != null) {
                _assetLoader.DisposeAll();
                _assetLoader = null;
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
            return _assetLoader?.RetrivalAllAssetName();
        }
    } // Class_End
} // Namespace_End