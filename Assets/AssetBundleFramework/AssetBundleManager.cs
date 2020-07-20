using System.Collections;
using System.Collections.Generic;
using AssetBundleFramework.Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetBundleFramework {
    /// <summary>
    /// 框架入口，对所有的AssetBundle进行管理的入口脚本
    ///     读取Manifest清单文件，缓存至此脚本
    ///     以“场景”为单位，管理整个项目中的所有AssetBundle包
    /// </summary>
    public class AssetBundleManager : MonoBehaviour {
        // 单例实例
        private static AssetBundleManager _Instance;

        // 场景集合
        private Dictionary<string, MultiAssetBundleManager> _sencesDict =
            new Dictionary<string, MultiAssetBundleManager>();

        // AssetBundle清单文件
        private AssetBundleManifest _manifest = null;

        private AssetBundleManager() { }

        // 获取类的实例
        public static AssetBundleManager GetInstance() {
            if (_Instance == null) {
                _Instance = new GameObject("AssetBundleManager").AddComponent<AssetBundleManager>();
            }

            return _Instance;
        }

        private void Awake() {
            // 加载Manifest清单文件
            StartCoroutine(AssetBundleManifestLoader.GetInstance().LoadManifestFile());
        }

        /// <summary>
        /// 下载AssetBundle指定的包
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="abName">AssetBundle名称</param>
        /// <param name="loadAllCompleteHandle">加载完成的委托</param>
        /// <returns></returns>
        public IEnumerator LoadAssetBundlePack(string sceneName, string abName, LoadComplete loadAllCompleteHandle) {
            // 参数检查
            if (string.IsNullOrEmpty(sceneName) || string.IsNullOrEmpty(abName)) {
                Debug.LogError($"{GetType()}/LoadAssetBundlePack()方法传入的场景名称和包名称参数为空，无法加载");
                yield break;
            }

            // 等待Manifest加载完毕
            while (!AssetBundleManifestLoader.GetInstance().IsLoadFinish) {
                yield return null;
            }

            // ab包中的所有依赖项
            _manifest = AssetBundleManifestLoader.GetInstance().GetAsstBundleManifest();
            if (_manifest == null) {
                Debug.LogError($"{GetType()}/LoadAssetBundlePack()方法_manifest为空，清单文件出现问题无法加载依赖项");
                yield break;
            }

            // 场景的包管理类加入到集合中
            if (!_sencesDict.ContainsKey(sceneName)) {
                MultiAssetBundleManager multiMannger =
                    new MultiAssetBundleManager(sceneName, abName, loadAllCompleteHandle);
                _sencesDict.Add(sceneName, multiMannger);
            }

            // 获取多包管理类
            MultiAssetBundleManager multiManngerInDict = _sencesDict[sceneName];
            if (multiManngerInDict == null) {
                Debug.LogError($"{GetType()}/LoadAssetBundlePack()中multiManngerInDict为空，场景的多包管理类创建失败");
                yield break;

            }

            // 加载指定Ab包
            yield return multiManngerInDict?.LoadAssetBundle(abName);
        }

        /// <summary>
        /// 加载AssetBundle中的资源
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="abName">ab包名称</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="isCache">缓存与否</param>
        /// <returns></returns>
        public Object LoadAsset(string sceneName, string abName,string assetName, bool isCache) {
            if (_sencesDict.ContainsKey(sceneName)) {
                return _sencesDict[sceneName].LoadAsset(abName, assetName, isCache);
            }
            Debug.LogError($"{GetType()}/LoadAsset()中的sceneName无法找到，无法成功加载资源");
            return null;
        }

        /// <summary>
        /// 释放所有资源
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        public void DisposeAllAssets(string sceneName) {
            if (_sencesDict.ContainsKey(sceneName)) {
                _sencesDict[sceneName].DisposeAllAssets();
            }
            else {
                Debug.LogError($"{GetType()}/DisposeAllAssets()中的sceneName无法找到,无法成功卸载资源");

            }
        }
    }
}