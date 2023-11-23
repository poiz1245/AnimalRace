using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingStartDetector : NetworkBehaviour
{
    [Networked] public bool IsSceneLoaded { get; set; }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Track1")
        {
            IsSceneLoaded = true;
            // 필요한 경우, 네트워크 이벤트를 전송하여 다른 클라이언트에게 알림
        }
    }
}
