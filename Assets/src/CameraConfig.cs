﻿
using System;
using UnityEngine;

public class CameraConfig:MonoBehaviour
{
    // Singleton
    public static CameraConfig Instance
    {
        get
        {
            if (s_instance == null)
            {
                GameObject go = Instantiate(PrefabReferences.Instance.CameraConfig);
                DontDestroyOnLoad(go);
                s_instance = go.GetComponent<CameraConfig>();
            }
            return s_instance;
        }

    }
    private static CameraConfig s_instance;

    public GameObject ReferenceCanvas;

    [Serializable]
    public class CameraArray
    {
        public GameObject[] Cameras;
    }

    [SerializeField]
    public CameraArray[] OrthographicCameras;
    [SerializeField]
    public CameraArray[] PerspectiveCameras;

    public void InitializePlayer(Transform player, int playerIndex, int totalPlayers)
    {
        GameObject cameraGo = Instantiate(OrthographicCameras[totalPlayers - 1].Cameras[playerIndex]);
        Camera camera = cameraGo.GetComponent<Camera>();
        GameObject canvasGo = Instantiate(ReferenceCanvas);
        SetLayerRecursively(canvasGo, LayerMask.NameToLayer("layer2d_j" + (playerIndex+1)));
        canvasGo.GetComponent<Canvas>().worldCamera = camera;
        canvasGo.transform.SetParent(cameraGo.transform);

        cameraGo = Instantiate(PerspectiveCameras[totalPlayers - 1].Cameras[playerIndex]);
        cameraGo.transform.SetParent(player);
    }

    private static void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
}