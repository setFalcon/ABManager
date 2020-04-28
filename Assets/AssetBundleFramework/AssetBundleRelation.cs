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
            if (!string.IsNullOrEmpty(abName)) {
                _abName = abName;
            }

            _dependAssetBundleList = new List<string>();
            _referenceAssetBundleList = new List<string>();
        }

        ///////////////////////////管理AssetBundle依赖关系///////////////////////////
        /// <summary>
        /// 对_abName包增加依赖的AssetBundle
        /// </summary>
        /// <param name="abName">添加的依赖包名称</param>
        public void AddDepend(string abName) {
            if (!string.IsNullOrEmpty(abName) && !_dependAssetBundleList.Contains(abName)) { // 未包含此包的依赖项
                _dependAssetBundleList.Add(abName);
            }
        }

        /// <summary>
        /// 对_abName包移除依赖的AssetBundle
        /// </summary>
        /// <param name="abName">移除的依赖包名称</param>
        /// <returns>true : AssetBundle没有依赖项, false : AssetBundle还存在依赖项</returns>
        public bool RemoveDepend(string abName) {
            if (_dependAssetBundleList.Contains(abName)) { // 未包含此包的依赖项
                _dependAssetBundleList.Remove(abName);
            }

            return _dependAssetBundleList.Count == 0;
        }

        /// <summary>
        /// 获取_abName包所有依赖的AssetBundle列表
        /// </summary>
        /// <returns>依赖的AssetBundle列表</returns>
        public List<string> GetAllDepend() {
            return _dependAssetBundleList;
        }

        ///////////////////////////管理AssetBundle引用关系///////////////////////////
        /// <summary>
        /// 对_abName包增加引用的AssetBundle
        /// </summary>
        /// <param name="abName">添加的引用包名称</param>
        public void AddReference(string abName) {
            if (!string.IsNullOrEmpty(abName) && !_referenceAssetBundleList.Contains(abName)) { // 未包含此包的引用项
                _referenceAssetBundleList.Add(abName);
            }
        }

        /// <summary>
        /// 对_abName包移除引用的AssetBundle
        /// </summary>
        /// <param name="abName">移除的引用包名称</param>
        /// <returns>true : AssetBundle没有引用项, false : AssetBundle还存在引用项</returns>
        public bool RemoveReference(string abName) {
            if (_referenceAssetBundleList.Contains(abName)) { // 未包含此包的引用项
                _referenceAssetBundleList.Remove(abName);
            }

            return _referenceAssetBundleList.Count == 0;
        }

        /// <summary>
        /// 获取_abName包所有依赖的AssetBundle列表
        /// </summary>
        /// <returns>依赖的AssetBundle列表</returns>
        public List<string> GetAllReference() {
            return _referenceAssetBundleList;
        }
    } // Class_End
} // Namespace_End