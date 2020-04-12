using System.IO;
using AssetBundleFramework.Tools;
using UnityEditor;

namespace AssetBundleFramework.Editor {
    /// <summary>
    /// 删除已经打包的Ab包文件
    /// </summary>
    public static class DeleteAssetBundle {
        /// <summary>
        /// 批量删除已经打包的Ab包文件
        /// </summary>
        [MenuItem("AssetBundleTools/Delete All AssetBundle", false, 40)]
        public static void DeleteAllAssetBundle() {
            // Ab包的输出目录
            string delDir = PathTools.GetAbOutputPath();
            if (!string.IsNullOrEmpty(delDir)) {
                // 删除路径下的所有文件和*.meta文件
                Directory.Delete(delDir, true);
            }
            // 刷新Asset/
            AssetDatabase.Refresh();
        }
    } // Class_End
} // Namespace_End