namespace AssetBundleFramework.Tools {

    /// <summary>
    /// AssetBundle加载完成后使用的委托
    /// </summary>
    /// <param name="abName">加载的AssetBundle包名称</param>
    public delegate void LoadComplete(string abName);
    
    /// <summary>
    /// 所有的常量以及枚举类型的封装
    /// </summary>
    public class AssetBundleDefine {
        public const string ASSETBUNDLE_MANIFEST = "AssetBundleManifest";
    }
}