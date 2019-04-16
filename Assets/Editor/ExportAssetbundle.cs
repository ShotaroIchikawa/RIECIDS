using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class ExportAssetbundle
{

    [MenuItem("Export/Assetbundle")]
    static void Export()
    {
        Directory.CreateDirectory(Application.streamingAssetsPath);
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
    }
}