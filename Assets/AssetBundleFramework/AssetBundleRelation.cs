using System.Collections.Generic;

namespace AssetBundleFramework {
    /// <summary>
    /// 管理AssetBundle引用关系的辅助类
    /// 用于存储指定AssetBundle的所有依赖关系包和引用关系包
    /// </summary>
    public class AssetBundleRelation {
        private string _abName; // 当前AssetBundle名称

        private List<string> _dependAssetBundleList; // 本包所有的依赖包集合
        private List<string> _referenceAssetBundleList; // 本包所有的引用包集合

        /// <summary>
        /// 构造函数,初始化包名和引用列表
        /// </summary>
        /// <param name="abName">AssetBundle名称</param>
        public AssetBundleRelation(string abName) {
            _abName = abName;
        }

        /*依赖关系*/
        // 增加依赖
        // 移除依赖
        // 获取所有依赖
        
        /*引用关系*/
        // 增加引用
        // 移除引用
        // 获取所有引用
    } // Class_End
} // Namespace_End