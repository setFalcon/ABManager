using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetBundleFramework.Test {
    /// <summary>
    /// 框架功能整体测试类
    /// </summary>
    public class Test_AssetBundleManager : MonoBehaviour {
        // 场景名称
        private string sceneName = "test_scene";

        // ab包名称
        private string abName = "test_scene/prefabs.ab";

        // 资源名称
        private string assetName = "Cube";

        private void Start() {
            StartCoroutine(AssetBundleManager.GetInstance().LoadAssetBundlePack(sceneName, abName, LoadABComplete));
        }

        // 所有的资源全部加载完成
        private void LoadABComplete(string abname) {
            // 提取资源
            Object obj = AssetBundleManager.GetInstance().LoadAsset(sceneName, abName, assetName, false);
            if (obj!=null) {
                Instantiate(obj);
            }
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.A)) {
                AssetBundleManager.GetInstance().DisposeAllAssets(sceneName);
            }
        }
    }
}