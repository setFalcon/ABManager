using System.IO;
using AssetBundleFramework.Tools;
using UnityEditor;

namespace AssetBundleFramework.Editor {
    /// <summary>
    /// 对资源文件夹下的所有标记的资源进行打包
    /// 在打包前请确保您所有的资源已经存入PathTools.GetAbResourcesPath()路径下,并按照用途进行分类处理
    /// 如果您的资源不符合上述条件可能不会被正确打包
    /// </summary>
    public static class BuildAssetBundle {
        /// <summary>
        /// 所有平台通用的打包方法
        /// </summary>
        /// <param name="target">要打包的目标平台</param>
        private static void BuildAssetBundles(BuildTarget target) {
            string abOutputPath = PathTools.GetAbOutputPath();// Ab包的输出路径
            if (!Directory.Exists(abOutputPath)) {// 检查Asset/ 下是否存在StreamingAssetsPath
                Directory.CreateDirectory(abOutputPath);// 输出路径不存在则需要创建输出路径
            }

            BuildPipeline.BuildAssetBundles(abOutputPath, BuildAssetBundleOptions.None, target); // 进行打包
            AssetDatabase.Refresh(); // 打包完成后刷新Project面板
        }

        /// <summary>
        /// Linux平台的打包方法
        /// </summary>
        [MenuItem("AssetBundleTools/Build All AssetBundles/Linux", false, 20)]
        public static void BuildAssetBundlesLinux() {
            BuildAssetBundles(BuildTarget.StandaloneLinux64);
        }

        /// <summary>
        /// Windows平台的打包方法
        /// </summary>
        [MenuItem("AssetBundleTools/Build All AssetBundles/Windows", false, 20)]
        public static void BuildAssetBundlesWindows() {
            BuildAssetBundles(BuildTarget.StandaloneWindows64);
        }
        
        /*
         * 由上述打包方法可见,扩展不同平台的打包方法容易
         * 只需要在本类中添加一个新平台的打包方法并在方法内调用通用的打包方法BuildAssetBundles(BuildTarget target)即可
         * 您可以自己添加更多的打包方法使其适用于您的工程,同样,删除不必要的打包方法也是允许的
         */
    } // Class_End
} // Namespace_End