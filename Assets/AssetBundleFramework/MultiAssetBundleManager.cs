using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundleFramework.Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetBundleFramework {
    /// <summary>
    /// AssetBundle多包依赖关系管理类
    /// 获取AssetBundle包间的依赖和引用关系
    /// 管理AssetBundle包之间的自动连锁(递归)加载机制
    /// </summary>
    public class MultiAssetBundleManager {
        private SingleAssetBundleLoader _currSingleAssetBundleLoader; // 单包加载实现类

        private Dictionary<string, SingleAssetBundleLoader> _loaderCacheDict; // AssetBundle缓存集合,防止重复加载

        private string _currSceneName; // 当前场景名称
        private string _currAssetBundleName; // 当前AssetBundle名称
        private Dictionary<string, AssetBundleRelation> _relationDict; // AssetBundle依赖和引用
        private LoadComplete _loadAllComplete; // AssetBundle加载全部完成的委托

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="abName">AssetBundle名称</param>
        /// <param name="loadAllComplete">加载完成所有AssetBundle的委托</param>
        public MultiAssetBundleManager(string sceneName, string abName, LoadComplete loadAllComplete) {
            _currSceneName = sceneName;
            _currAssetBundleName = abName;
            _loaderCacheDict = new Dictionary<string, SingleAssetBundleLoader>();
            _relationDict = new Dictionary<string, AssetBundleRelation>();
            _loadAllComplete = loadAllComplete;
        }

        /// <summary>
        /// 完成名称为abName的AssetBundle的回调方法
        /// </summary>
        /// <param name="abName">AssetBundle名称</param>
        private void CompleteLoadSingle(string abName) {
            if (abName.Equals(_currAssetBundleName)) {
                _loadAllComplete?.Invoke(abName);
            }
        }

        /// <summary>
        /// 加载AssetBundle的方法
        /// </summary>
        /// <param name="abName">要加载AssetBundle的名称</param>
        /// <returns></returns>
        public IEnumerator LoadAssetBundle(string abName) {
            // AssetBundle关系的建立
            if (!_relationDict.ContainsKey(abName)) {
                AssetBundleRelation relation = new AssetBundleRelation(abName);
                _relationDict.Add(abName, relation);
            }

            AssetBundleRelation tmpRelation = _relationDict[abName];
            // 获得所有的依赖关系
            string[] depend = AssetBundleManifestLoader.GetInstance().RetrivalDepend(abName);
            foreach (string itemDepend in depend) {
                tmpRelation.AddDepend(itemDepend); // 添加依赖项
                yield return LoadReference(itemDepend, abName); // 添加引用项
            }

            // 进行AssetBundle加载
            if (_loaderCacheDict.ContainsKey(abName)) { // 加载过了
                yield return _loaderCacheDict[abName].LoadABWWW(); // 加载AssetBundle
            }
            else {
                _currSingleAssetBundleLoader = new SingleAssetBundleLoader(abName, CompleteLoadSingle);
                _loaderCacheDict.Add(abName, _currSingleAssetBundleLoader);
                yield return _currSingleAssetBundleLoader.LoadABWWW();
            }

            yield return null;
        }

        /// <summary>
        /// 添加引用AssetBundle
        /// </summary>
        /// <param name="abName">AssetBundle名称</param>
        /// <param name="refAbName">被引用的AssetBundle名称</param>
        /// <returns></returns>
        private IEnumerator LoadReference(string abName, string refAbName) {
            if (_relationDict.ContainsKey(abName)) { // AssetBundle已经加载
                AssetBundleRelation tmpRelation = _relationDict[abName];
                tmpRelation.AddReference(refAbName); // 添加引用关系
            }
            else {
                AssetBundleRelation tmpRelation = new AssetBundleRelation(abName);
                tmpRelation.AddReference(refAbName);
                _relationDict.Add(abName, tmpRelation);
                yield return LoadAssetBundle(abName); // 加载依赖包
            }

            yield return null;
        }

        /// <summary>
        /// 加载资源使用的方法
        /// </summary>
        /// <param name="abName">AssetBundle名称</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="isCache">是否使用资源缓存</param>
        /// <returns></returns>
        public Object LoadAsset(string abName, string assetName, bool isCache) {
            foreach (string itemAbName in _loaderCacheDict.Keys) {
                if (abName.Equals(itemAbName)) {
                    return _loaderCacheDict[itemAbName].LoadAsset(assetName, isCache);
                }
            }

            Debug.LogError($"{GetType()}/LoadAsset方法找不到资源名为{assetName}的资源,请检查AssetBundle名称{abName}");
            return null;
        }

        /// <summary>
        /// 释放所有资源,场景切换时进行调用
        /// </summary>
        public void DisposeAllAsset() {
            try {
                foreach (SingleAssetBundleLoader loader in _loaderCacheDict.Values) { // 逐一释放所有加载过的AssetBundle
                    loader.DisposeAll();
                }
            }
            finally {
                _loaderCacheDict.Clear(); // 释放加载集合缓存
                _loaderCacheDict = null;
                _relationDict.Clear(); // 释放其他资源
                _relationDict = null;
                _currSceneName = null;
                _currAssetBundleName = null;
                _loadAllComplete = null;
                Resources.UnloadUnusedAssets(); // 卸载无用资源
                System.GC.Collect(); // 垃圾收集
            }
        }
    } // Class_End
} // Namespace_End