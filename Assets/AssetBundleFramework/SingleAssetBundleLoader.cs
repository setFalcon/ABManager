using System.Collections;
using System.IO;
using System.Linq.Expressions;
using AssetBundleFramework.Tools;
using UnityEngine;
using Object = System.Object;

namespace AssetBundleFramework {
    /// <summary>
    /// 使用WWW/UnityWebRequestAssetBundle加载单一AssetBundle
    /// </summary>
    public class SingleAssetBundleLoader : System.IDisposable {
        // 引用:资源加载类,通过加载Asset的方法加载Ab包中的Asset
        private AssetLoader _loader;
        // 委托:

        // AssetBundle名称
        private string _abName;

        // AssetBundle下载路径
        private string _downloadPath;

        /// <summary>
        /// 构造函数,初始化字段
        /// </summary>
        public SingleAssetBundleLoader(string abName) {
            _loader = null;
            _abName = abName;
            // 委托的定义

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
        /// 释放资源
        /// </summary>
        public void Dispose() { }
    } // Class_End
} // Namespace_End