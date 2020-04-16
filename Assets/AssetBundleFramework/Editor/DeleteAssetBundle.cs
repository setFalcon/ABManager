using System.IO;
using AssetBundleFramework.Tools;
using UnityEditor;

namespace AssetBundleFramework.Editor {
    /// <summary>
    /// 删除输出路径下的所有的打包文件
    /// 防止已有的AssetBundle对新的AssetBundle产生印象
    /// </summary>
    public static class DeleteAssetBundle {
        /// <summary>
        /// 批量删除已经打包的Ab包文件
        /// </summary>
        [MenuItem("AssetBundleTools/Delete All AssetBundle", false, 40)]
        public static void DeleteAllAssetBundle() {
            string delDir = PathTools.GetAbOutputPath(); // Ab包的输出目录
            if (!string.IsNullOrEmpty(delDir)) {
                Directory.Delete(delDir, true); // 删除输出目录下的所有文件和*.meta文件
            }

            AssetDatabase.Refresh(); // 刷新Project面板
        }
    } // Class_End
} // Namespace_End