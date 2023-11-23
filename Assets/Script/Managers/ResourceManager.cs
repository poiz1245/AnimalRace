using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public GameUI hudPrefab;
    public KartDefinition[] kartDefinitions;
    public RenderTexture[] kartImage;

    public static ResourceManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
