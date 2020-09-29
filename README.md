![AssetBundle Manager](https://ftp.bmp.ovh/imgs/2020/04/94fdc6d248a8bea5.png)

## ❓什么是AssetBundle Manager

AssetBundle Manager是支持自动化标记资源打包路径、自动化打包加载AssetBundle、获取Asset资源、自动卸载以及包管理的Unity开发插件。

## 📕如何使用

使用给git工具，使用**git clone**命令将此项目clone到您的计算机上，将AssetBundleFramework/文件夹下的所有文件拷贝至您Unity项目的Asset/文件夹下。您也可以使用Clone or Download功能下载此项目的zip压缩包，按照上述过程将此项目部署即可。

## :airplane:快速入门

AssetBundle Manager提供编辑器拓展功能，在您的项目中正确安装后，您的Unity会出现一项新的名称为AssetBundle的新Menu。此Menu中提供了多种已经实现了相应功能的MenuItem，诸如：自动标记、多平台自动打包、自动清理已经完成的打包等功能，您只需要做极小的改动就能快速使用。

- 加载简单无依赖关系的资源:

  	此功能的实现是非常简单的，您只需使用AssetBundleManager.cs中的方法即可完成此项工作，下面给出了一个简单的实现供您参考：

  ``` C#
  public class Test_SingleAssetBundleLoader : MonoBehaviour {
      private SingleAssetBundleLoader _assetBundleLoader; // SingleAssetBundleLoader引用
      private string _abName = "test_scene/prefabs.ab"; // 需要加载的Ab包
      private string _simpleAssetName = "Cylinder.prefab"; // 需要加载的资源名称
      private void Start() {
          _assetBundleLoader = new SingleAssetBundleLoader(_abName, LoadComplete); // 创建实例
          StartCoroutine(_assetBundleLoader.LoadABWWW()); // 加载Ab包
      }
      private void LoadComplete(string abName) { // 完成Ab包加载的回调方法
          Object cloneObject = _assetBundleLoader.LoadAsset(_simpleAssetName, false); // 加载资源并实例化
          Instantiate(cloneObject);
      }
      private void OnDestroy() { _assetBundleLoader.DisposeAll(); } // 释放无用资源
  }
  ```
  
- 加载复杂带有依赖关系的资源

   此功能的实现是非常简单的，您只需使用SingleAssetBundleLoader.cs中的方法即可完成此项工作，下面给出了一个简单的实现供您参考：
   
   ``` C#
   public class Test_AssetBundleManager : MonoBehaviour {
       private string sceneName = "test_scene"; // 场景名称
       private string abName = "test_scene/prefabs.ab"; // ab包名称
       private string assetName = "Cube"; // 资源名称
       private void Start() {
           StartCoroutine(AssetBundleManager.GetInstance().LoadAssetBundlePack(sceneName, abName, LoadABComplete));
       }
       private void LoadABComplete(string abname) { // 所有的资源全部加载完成
           Object obj = AssetBundleManager.GetInstance().LoadAsset(sceneName, abName, assetName, false); // 提取资源
           if (obj!=null) { Instantiate(obj); } // 资源实例化
       }
       private void OnDestroy() { AssetBundleManager.GetInstance().DisposeAllAssets(sceneName); } // 释放资源
   }
   ```
   
   

## :hammer:需要做什么修改

此框架的耦合性已经在开发过程中尽力降低，故此您需要改动的地方极少：

1. Asset资源路径

   	您需要将所有的Asset资源放入“AB_Res”目录下，并按照场景和用途分类，分类后的资源相对路径将会类似于这样：Asset/AB_Res/Scene_1/Materials/DemoMat.mat，您也可以通过修改脚本PathTools.cs更改Asset资源存放的目录名称。

   ```C#
   public const string AB_RESOURCES = "您希望修改的目录名称";
   ```

2. 多平台支持

   其实本项目中考虑的平台已经较为全面，您也可以通过修改部分代码来添加更多的平台支持，为添加多平台支持，您需要修改两个文件的相关代码。

- 脚本[^BuildAssetBundle.cs]，添加对应平台的对应AssetBundle打包代码。

  ``` C#
  /// <summary>
  /// 某平台的打包方法，为方便起见，下面的平台名称均用#代替
  /// </summary>
  [MenuItem("AssetBundleTools/Build All AssetBundles/#", false, 20)]
  public static void BuildAssetBundles#() {
  	BuildAssetBundles(BuildTarget.#);
  }
  ```

- 脚本[^PathTools.cs]，添加相应平台的路径相关方法

  ``` C#
  /// <summary>
  /// 获取平台路径
  /// 为方便起见，下面增加的平台名称均用#代替
  /// </summary>
  /// <returns>平台路径</returns>
  private static string GetPlatformPath() {
  	//平台路径
  	string platformPath = String.Empty;
  	switch (Application.platform) {
  		......//pre codes...
  		case RuntimePlatform.#:
  			// 此处的路径应由平台性质决定
  			// 一般来说Liunx与Windows平台使用Application.streamingAssetsPath
  			// ios与Android平台使用Application.persistentDataPath
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
  		......//pre codes...
  		case RuntimePlatform.#:
  			platformName = "#";
  		break;
  	}
  	return platformName;
  }
  /// <summary>
  /// 获取Ab包的WWW路径
  /// </summary>
  /// <returns></returns>
  public static string GetWWWPath() {
  	string wwwPath = String.Empty;
  	switch (Application.platform) {
  		......//pre codes...
  		case RuntimePlatform.#:
  			// 此处的路径应由平台性质决定
  			// 一般来说Windows平台使用"file://" + GetAbOutputPath()
  			// Android平台使用"jar:file://" + GetAbOutputPath()
  			wwwPath = "jar:file://" + GetAbOutputPath();
  		break;
  	}
  	return wwwPath;
  }
  ```

## :grey_question:可能的问题

- [ ] AssetBundle资源的清理可能存在问题
