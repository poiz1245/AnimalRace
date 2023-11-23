using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class LevelManager : NetworkBehaviour
    {
        [SerializeField] public GameObject dummyScreen;

        public const int LAUNCH_SCENE = 0;
        public const int LOBBY_SCENE = 1;

        public static LevelManager Instance { get; set; }


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);
        }

        public static void LoadMenu()
        {
            NetworkCallBack.NC.runner.SetActiveScene(LAUNCH_SCENE);
        }

        public void LoadTrack(int SceneIndex)
        {
            NetworkCallBack.NC.runner.SetActiveScene(SceneIndex);
        }
    }

}
