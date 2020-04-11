using System;
using System.IO;
using UnityEngine;

namespace AssetBundleFramework.Tools {
    /// <summary>
    /// 路径工具类,包括文件的路径常量,以及路径的相关方法
    /// </summary>
    public class PathTools {
        // 路径常量
        public const string AB_RESOURCES = "AB_Res";

        // 路径相关方法
        /// <summary>
        /// 得到资源文件的相对路径 Assets/...
        /// </summary>
        /// <returns>资源文件的相对路径</returns>
        public static string GetAbResourcesPath() {
            return Path.Combine(Application.dataPath, AB_RESOURCES);
        }

        /// <summary>
        /// 获取Ab输出路径 平台路径+平台名称
        /// </summary>
        /// <returns></returns>
        public static string GetAbOutputPath() {
            return Path.Combine(GetPlatformPath(),GetPlatformName());
        }
        
        /// <summary>
        /// 获取平台路径
        /// </summary>
        /// <returns>平台路径</returns>
        private static string GetPlatformPath() {
            //平台路径
            string platformPath = String.Empty;
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
        private static string GetPlatformName() {
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
    }// Class_End
}// Namespace_End