using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using AssetBundleFramework.Tools;

namespace AssetBundleFramework.Editor {
    /// <summary>
    /// 自动为资源文件添加标记的工具类
    /// 开发思路：
    ///     1. 定位需要打包资源的文件夹根目录
    ///     2. 遍历所有场景资源文件夹
    ///         - 遍历场景目录下所有的场景文件
    ///             * 如果是文件夹，仍需要继续递归访问
    ///             * 找到文件，使用AssetImporter修改标记包名后缀名
    /// </summary>
    public static class AutoSetLabels {
        /// <summary>
        /// 设置Ab包名称
        /// </summary>
        [MenuItem("AssetBundleTools/Set AB Label")]
        public static void SetAbLabel() {
            // 需要给Asset做标记的根目录
            string needSetLabelRoot = PathTools.GetAbResourcesPath();
            // 需要打包的资源目录
            DirectoryInfo[] resDirs = new DirectoryInfo(needSetLabelRoot).GetDirectories();
            // 清空无用标记
            AssetDatabase.RemoveUnusedAssetBundleNames();

            // 遍历所有场景资源文件夹
            foreach (DirectoryInfo resDir in resDirs) {
                DirectoryInfo tmpResDirInfo = new DirectoryInfo(resDir.FullName);
                // 确定资源文件夹名称
                string resDirName = resDir.FullName;
#if UNITY_EDITOR_WIN
                resDirName = resDirName.Replace('\\', '/');
#endif
                // 获得场景名称
                int tmpIndex = resDirName.LastIndexOf("/", StringComparison.Ordinal);
                string tmpSceneName = resDirName.Substring(tmpIndex + 1);
                // 递归调用判定方法
                JudgeTypeByRecursive(tmpResDirInfo, tmpSceneName);
            }

            //刷新Assset，提示打包完成信息
            AssetDatabase.Refresh();
            Debug.Log("本次操作设置标记完成！");
        }

        /// <summary>
        /// 递归判断文件类型是文件夹还是实际文件，修改标记
        /// </summary>
        /// <param name="currDir">当前考察的文件夹</param>
        /// <param name="sceneName">设定的前缀名称</param>
        private static void JudgeTypeByRecursive(DirectoryInfo currDir, string sceneName) {
            // 参数检查
            if (!currDir.Exists) {
                Debug.LogError($"文件目录名称:{currDir}不存在,请检查");
                return;
            }

            // 获取下一目录文件信息集合
            FileSystemInfo[] directories = currDir.GetFileSystemInfos();
            foreach (FileSystemInfo fileInfo in directories) {
                if (fileInfo is FileInfo) { //文件
                    // 修改文件的AssetBundle标签
                    SetFileAbLabel(fileInfo as FileInfo, sceneName);
                }
                else { // 目录,递归调用当前方法,继续深入下层文件夹进行判定
                    JudgeTypeByRecursive(fileInfo as DirectoryInfo, sceneName);
                }
            }
        }

        /// <summary>
        /// 使用AssetImporter进行文件Ab包名称标记
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="sceneName">场景名称</param>
        private static void SetFileAbLabel(FileInfo fileInfo, string sceneName) {
            // 参数检查,去除meta文件
            if (fileInfo.Extension.Equals(".meta"))
                return;
                
            // 将文件全名分解,分解为 Asset/... 目录
            int assetDataPathPos = fileInfo.FullName.IndexOf("Asset", StringComparison.Ordinal);
            // 为资源文件设置ab包名称和后缀名
            AssetImporter importer = AssetImporter.GetAtPath(fileInfo.FullName.Substring(assetDataPathPos));
            Debug.Log(GetAbName(fileInfo, sceneName));
            importer.assetBundleName = GetAbName(fileInfo, sceneName);
            importer.assetBundleVariant = GetAbVariant(fileInfo);
        }

        /// <summary>
        /// 获取Ab包名称:"二级目录名称+三级目录名称+文件名称"
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="sceneName">场景名称</param>
        /// <returns>Ab包名称</returns>
        private static string GetAbName(FileInfo fileInfo, string sceneName) {
            // 最终的ab包名称
            string abName = String.Empty;
            // 资源的全路径
            string assetFileFullName = fileInfo.FullName;
#if UNITY_EDITOR_WIN
            assetFileFullName = assetFileFullName.Replace('\\', '/');
#endif
            // 定位"场景名称"后的字符位置
            int sceneNamePos = assetFileFullName.IndexOf(sceneName, StringComparison.Ordinal) + sceneName.Length;
            // ab包中"类型名称"区域
            string abFileNameArea = assetFileFullName.Substring(sceneNamePos + 1);

            if (abFileNameArea.Contains("/")) {
                string[] tmpSpilt = abFileNameArea.Split('/');
                abName = sceneName + "/" + tmpSpilt[0];
            }

            return abName;
        }

        /// <summary>
        /// 设置Ab包扩展名称
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <returns>Ab包扩展名</returns>
        private static string GetAbVariant(FileInfo fileInfo) {
            switch (fileInfo.Extension) {
                case ".unity": // 场景类型文件
                    return "u3d";
                default:
                    return "ab";
            }
        }
    }// Class_End
}// Namespace_End