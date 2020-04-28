using System;
using System.IO;
using UnityEngine;

namespace AssetBundleFramework.Tools {
    /// <summary>
    /// 路径相关的工具类,封装了资源输入路径以及AssetBundle输出路径
    /// 所有的路径相关功能方法的封装类型,减少耦合
    /// </summary>
    public class PathTools {
        public const string AB_RESOURCES = "AB_Res"; // 资源文件夹的名称

        /// <summary>
        /// 获得资源文件夹的相对路径
        /// 相对路径格式: Assets/val(AB_RESOURCES)
        /// </summary>
        /// <returns>资源文件夹的相对路径</returns>
        public static string GetAbResourcesPath() {
            return Path.Combine(Application.dataPath, AB_RESOURCES);
        }

        /// <summary>
        /// 获取AssetBundle包的输出路径
        /// 输出路径格式: 与平台相关的数据路径+平台名称
        /// </summary>
        /// <returns>Ab包的输出路径</returns>
        public static string GetAbOutputPath() {
            return Path.Combine(GetPlatformPath(), GetPlatformName());
        }

        /// <summary>
        /// 获取与平台相关的数据路径
        /// </summary>
        /// <returns>平台相关的数据路径</returns>
        private static string GetPlatformPath() {
            string platformPath = String.Empty; // 平台相关的数据路径
            switch (Application.platform) {
                // Windows | Linux
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxPlayer:
                    platformPath = Application.streamingAssetsPath;
                    break;
                // IOS | Android
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                    platformPath = Application.persistentDataPath;
                    break;
            }

            return platformPath;
        }

        /// <summary>
        /// 获取平台的名称
        /// </summary>
        /// <returns>平台名称</returns>
        public static string GetPlatformName() {
            string platformName = String.Empty;
            switch (Application.platform) {
                // Windows
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    platformName = "Windows";
                    break;
                // Linux
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxPlayer:
                    platformName = "Linux";
                    break;
                // IOS
                case RuntimePlatform.IPhonePlayer:
                    platformName = "ios";
                    break;
                // Android
                case RuntimePlatform.Android:
                    platformName = "Android";
                    break;
            }

            return platformName;
        }

        /// <summary>
        /// 获取AssetBundle的存储路径并将其转换为WWW的加载路径
        /// </summary>
        /// <returns>本地加载Ab包所使用的WWW路径</returns>
        public static string GetWWWPath() {
            string wwwPath = String.Empty; // 本地加载Ab包所使用的WWW路径
            switch (Application.platform) {
                // Windows
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    wwwPath = "file://" + GetAbOutputPath();
                    break;
                // Linux
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxPlayer:
                    wwwPath = "file://" + GetAbOutputPath();
                    break;
                // IOS
                case RuntimePlatform.IPhonePlayer:
                    wwwPath = "file://" + GetAbOutputPath();
                    break;
                // Android
                case RuntimePlatform.Android:
                    wwwPath = "jar:file://" + GetAbOutputPath();
                    break;
            }

            return wwwPath;
        }
    } // Class_End
} // Namespace_End