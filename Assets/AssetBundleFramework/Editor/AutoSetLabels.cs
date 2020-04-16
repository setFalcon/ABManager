using System;
using System.IO;
using AssetBundleFramework.Tools;
using UnityEditor;
using UnityEngine;

namespace AssetBundleFramework.Editor {
    /// <summary>
    /// 自动为资源文件添加标记的工具类,此工具类会为资源文件夹下的所有Asset设置包标记
    /// 资源文件应该存储到路径PathTools.GetAbResourcesPath()方法的返回值对应的文件夹中
    /// 在打包前应保证所有的Asset资源都被正确标记上,否则在后续流程中会出现错误
    /// </summary>
    public static class AutoSetLabels {
        /// <summary>
        /// 自动为PathTools.GetAbResourcesPath()文件夹下的所有Asset设置包标记
        /// 包标记的规则为: 场景名称/资源类型    ab/u3d
        /// </summary>
        [MenuItem("AssetBundleTools/Set AB Label", false, 0)]
        public static void SetAbLabel() {
            string needSetLabelRoot = PathTools.GetAbResourcesPath(); // Asset存储的根目录
            DirectoryInfo[] resDirs = new DirectoryInfo(needSetLabelRoot).GetDirectories(); // 获取资源文件夹的所有子文件夹
            AssetDatabase.RemoveUnusedAssetBundleNames(); // 清空所有的无用标记
            foreach (DirectoryInfo resDir in resDirs) { // 遍历所有场景资源文件夹
                DirectoryInfo tmpResDirInfo = new DirectoryInfo(resDir.FullName); // 分类文件夹的信息
                string resDirName = resDir.FullName; // 资源文件夹的全称
#if UNITY_EDITOR_WIN
                resDirName = resDirName.Replace('\\', '/'); // Windows下将所有的\分隔符转为成/
#endif
                int tmpIndex = resDirName.LastIndexOf("/", StringComparison.Ordinal); // 场景名称字符串位置
                string tmpSceneName = resDirName.Substring(tmpIndex + 1); // 场景名称 
                JudgeTypeByRecursive(tmpResDirInfo, tmpSceneName); // 递归调用判定方法
            }

            AssetDatabase.Refresh(); // 刷新Project视图
            Debug.Log("本次操作设置标记完成！"); // 提示打包完成信息
        }

        /// <summary>
        /// 递归考察文件夹下所有的文件以及文件夹
        /// </summary>
        /// <param name="currDir">当前考察的文件夹</param>
        /// <param name="sceneName">场景名称</param>
        private static void JudgeTypeByRecursive(DirectoryInfo currDir, string sceneName) {
            if (!currDir.Exists) { // 要考察的文件夹不存在
                Debug.LogError($"文件目录名称:{currDir}不存在,请检查");
                return;
            }

            FileSystemInfo[] directories = currDir.GetFileSystemInfos(); // 获取此目录下的文件信息集合
            foreach (FileSystemInfo fileInfo in directories) {
                if (fileInfo is FileInfo info) { // 资源是文件类型
                    SetFileAbLabel(info, sceneName); // 修改相应的Asset包标签
                }
                else { // 资源是目录类型,递归调用当前方法,继续深入子文件夹进行判定
                    JudgeTypeByRecursive(fileInfo as DirectoryInfo, sceneName);
                }
            }
        }

        /// <summary>
        /// 使用AssetImporter进行文件Ab包标签设置
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="sceneName">场景名称</param>
        private static void SetFileAbLabel(FileInfo fileInfo, string sceneName) {
            if (fileInfo.Extension.Equals(".meta") || fileInfo.Extension.Equals(".Ds_store")) // 去除所有的系统生成文件
                return;


            int assetDataPathPos = fileInfo.FullName
                .IndexOf("Asset", StringComparison.Ordinal); // 获取文件名称中Asset的位置,便于获取相对路径
            AssetImporter importer = AssetImporter
                .GetAtPath(fileInfo.FullName.Substring(assetDataPathPos)); // 资源相对路径对应的AssetImporter
            importer.assetBundleName = GetAbName(fileInfo, sceneName); // 设定前缀
            importer.assetBundleVariant = GetAbVariant(fileInfo); // 设定后缀
        }

        /// <summary>
        /// 获取Ab包名称的方法
        /// 包名规则: 场景名+用途分类+文件名称
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="sceneName">场景名称</param>
        /// <returns>Ab包名称</returns>
        private static string GetAbName(FileInfo fileInfo, string sceneName) {
            string abName = String.Empty;// 最终获得的Ab包名称
            string assetFileFullName = fileInfo.FullName;// 资源的全路径
#if UNITY_EDITOR_WIN
            assetFileFullName = assetFileFullName.Replace('\\', '/');// Windows下将所有的\分隔符转为成/
#endif
            int sceneNamePos = assetFileFullName.IndexOf(sceneName, StringComparison.Ordinal) + sceneName.Length;// 定位场景名字符位置
            string abFileNameArea = assetFileFullName.Substring(sceneNamePos + 1); // 截取用途分类字符串
            if (abFileNameArea.Contains("/")) {// 检查是否符合命名规范
                string[] tmpSpilt = abFileNameArea.Split('/');
                abName = sceneName + "/" + tmpSpilt[0];// 场景名+用途分类
            }

            return abName;
        }

        /// <summary>
        /// 设置Ab包的扩展名称
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <returns>Ab包扩展名</returns>
        private static string GetAbVariant(FileInfo fileInfo) {
            switch (fileInfo.Extension) {
                case ".unity": // 场景类型文件
                    return "u3d";
                default:// 其他类型文件
                    return "ab";
            }
        }
    } // Class_End
} // Namespace_End