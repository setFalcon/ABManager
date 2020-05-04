using System.Collections;
using System.Collections.Generic;
using AssetBundleFramework.Tools;

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
        public MultiAssetBundleManager(string sceneName,string abName,LoadComplete loadAllComplete) {
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
            // 获得所有的依赖关系
            // 进行AssetBundle加载
            yield return null;
        }
    }
}