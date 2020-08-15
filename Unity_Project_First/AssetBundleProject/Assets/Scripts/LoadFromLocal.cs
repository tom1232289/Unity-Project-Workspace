using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoadFromLocal : MonoBehaviour
{
    private IEnumerator Start()
    {
        //AssetBundle ab = AssetBundle.LoadFromFile("AssetBundles/wall.ab");

        string uri = @"http://localhost/AssetBundles/cubewall.unity3d";
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri);
        yield return request.SendWebRequest();
        AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);

        GameObject wallPrefab = ab.LoadAsset<GameObject>("CubeWall");
        Instantiate(wallPrefab);
    }
}
