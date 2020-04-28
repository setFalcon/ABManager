using System;
using System.Collections;
using System.IO;
using AssetBundleFramework.Tools;
using UnityEngine;

namespace AssetBundleFramework {
    /// <summary>
    /// 读取AssetBundle依赖关系清单文件(PlatformName.manifest)的单例辅助类
    /// </summary>
    public class AssetBundleManifestLoader : IDisposable {
        private static AssetBundleManifestLoader _instance; // 单例
        private AssetBundleManifest _manifest; // AssetBundle清单文件的引用
        private string _manifestPath; // AssetBundle清单文件位置
        private AssetBundle _readManifest; // 读取Ab清单文件的AssetBundle
        public bool IsLoadFinish { get; set; } // 加载是否完成的属性

        /// <summary>
        /// 构造函数,初始化路径/文件引用等
        /// </summary>
        private AssetBundleManifestLoader() {
            _manifestPath = Path.Combine(PathTools.GetWWWPath(), PathTools.GetPlatformName()); // 初始化清单文件路径
            _manifest = null;
            _readManifest = null;
            IsLoadFinish = false;
        }

        /// <summary>
        /// AssetBundleManifestLoader单例获取方法
        /// </summary>
        /// <returns></returns>
        public static AssetBundleManifestLoader GetInstance() {
            return _instance ?? (_instance = new AssetBundleManifestLoader()); // 懒加载单例
        }

        /// <summary>
        /// 加载Manifest清单文件,清单文件在一个特殊的AssetBundle中存储
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadManifestFile() {
            using (WWW www = new WWW(_manifestPath)) {
                yield return www;
                if (www.progress >= 1) { // 请求完成,获取AssetBundle
                    AssetBundle tmpManifest = www.assetBundle;
                    if (tmpManifest != null) { // 获取成功
                        _readManifest = tmpManifest;
                        _manifest = _readManifest.LoadAsset("AssetBundleManifest")
                            as AssetBundleManifest; // 获取AssetBundle清单,AssetBundleManifest是固定常量
                        IsLoadFinish = true; // 加载失败
                    }
                    else { // 获取失败
                        Debug.LogError($"{GetType()}/LoadManifestFile方法加载清单文件失败,请检查清单文件路径 : {_manifestPath} 是否正确");
                    }
                }
            }
        }

        /// <summary>
        /// 获取具有AssetBundle依赖关系的实例对象
        /// </summary>
        /// <returns>存储AssetBundle依赖关系的实例</returns>
        public AssetBundleManifest GetAsstBundleManifest() {
            if (IsLoadFinish && _manifest != null) { // 成功加载实例
                return _manifest;
            }

            Debug.LogError( //加载实例失败
                $"{GetType()}/GetAsstBundleManifest方法加载manifest失败,可能是加载尚未完毕或者_manifest对象为空," +
                $"请检查这两个参数 : IsLoadFinish = {IsLoadFinish}, _manifest = {_manifest} ");
            return null;
        }

        /// <summary>
        /// 获取abName包依赖的所有AssetBundle名称
        /// </summary>
        /// <param name="abName">要查询的AssetBundle名称</param>
        /// <returns>所有的依赖包名称</returns>
        public string[] RetrivalDepend(string abName) { }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() { }
    } // Class_End
} // Namespace_End