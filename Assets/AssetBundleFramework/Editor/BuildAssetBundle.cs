using System;
using System.IO;
using AssetBundleFramework.Tools;
using UnityEditor;

namespace AssetBundleFramework.Editor {
    /// <summary>
    /// 对标记的资源进行AssetBundle打包
    /// </summary>
    public class BuildAssetBundle {
        private static void BuildAssetBundles(BuildTarget target) {
            // Ab包的输出路径
            string abOutputPath = PathTools.GetAbOutputPath();
            // 检查Asset/ 下是否存在StreamingAssetsPath
            if (!Directory.Exists(abOutputPath)) {
                Directory.CreateDirectory(abOutputPath);
            }

            //进行打包
            BuildPipeline.BuildAssetBundles(abOutputPath, BuildAssetBundleOptions.None, target);
        }

        [MenuItem("AssetBundleTools/Build All AssetBundles/Linux", false, 20)]
        public static void BuildAssetBundlesLinux() {
            BuildAssetBundles(BuildTarget.StandaloneLinux64);
        }

        [MenuItem("AssetBundleTools/Build All AssetBundles/Windows", false, 20)]
        public static void BuildAssetBundlesWindows() {
            BuildAssetBundles(BuildTarget.StandaloneWindows64);
        }
    } // Class_End
} // Namespace_End