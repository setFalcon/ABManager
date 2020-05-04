using System.Collections.Generic;
using AssetBundleFramework.Tools;

namespace AssetBundleFramework {
    /// <summary>
    /// AssetBundle多包依赖关系管理类
    /// 获取AssetBundle包间的依赖和引用关系
    /// 管理AssetBundle包之间的自动连锁(递归)加载机制
    /// </summary>
    public class MutilAssetBundleManager {
        private SingleAssetBundleLoader _currSingleAssetBundleLoader; // 单包加载实现类

        private Dictionary<string, SingleAssetBundleLoader> _loaderDict =
            new Dictionary<string, SingleAssetBundleLoader>(); // AssetBundle缓存集合,防止重复加载

        private string _currSceneName; // 当前场景名称
        private string _currAssetBundleName; // 当前AssetBundle名称
        private Dictionary<string, AssetBundleRelation> _relationDict = new Dictionary<string, AssetBundleRelation>();
        private LoadComplete _loadAllComplete; // AssetBundle加载全部完成的委托
    }
}