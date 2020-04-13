using System;
using System.Collections;

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
            _abName = abName;
            //委托的定义
            
            // TODO:下载路径的封装 + ab包名称
            _downloadPath = String.Empty;
        }

        /// <summary>
        /// 加载AssetBundle
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadAssetBundle() {
            yield return null;
        }

        /// <summary>
        /// 从AssetBundle中加载Asset资源
        /// </summary>
        /// <param name="assetName">Asset资源名称</param>
        /// <param name="isCache">是否缓存</param>
        /// <returns>Asset资源</returns>
        public Object LoadAsset(string assetName, bool isCache) {
            return null;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() { }
    }
}