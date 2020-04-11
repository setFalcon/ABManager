using System;
using System.IO;
using AssetBundleFramework.Tools;
using UnityEditor;

namespace AssetBundleFramework.Editor {
    /// <summary>
    /// 对标记的资源进行AssetBundle打包
    /// </summary>
    public class BuildAssetBundle {
        [MenuItem("AssetBundleTools/Build All AssetBundles")]
        public static void BuildAssetBundles() {
            // Ab包的输出路径
            string abOutputPath = PathTools.GetAbOutputPath();
            // 检查Asset/ 下是否存在StreamingAssetsPath
            if (!Directory.Exists(abOutputPath)) {
                Directory.CreateDirectory(abOutputPath);
            }

            //进行打包
            BuildPipeline.BuildAssetBundles(abOutputPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneLinux64);
        }
    }// Class_End
}// Namespace_End