using System.Collections.Generic;
using Audio;
using Consequences;
using Inventory;
using KinematicCharacterController.Walkthrough.RootMotionExample;
using Player;
using Player.Death;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public Animator CharacterAnimator;

        public MyPlayer PlayerHandler;
        public GameObject playerPrefab;
        public GameObject RagdollPrefab;
        public bool enableCheatyDevShortcuts = false;

        protected PlayerRespawner playerRespawner;
        protected PlayerInventory playerInventory;
        private static readonly int _isCrouching = Animator.StringToHash("IsCrouching");
        
        public AudioClip[] audioClipArray;

        private void OnEnable()
        {
            if (instance == null)
            {
                instance = this;
            } else
            {
                Destroy(this.gameObject);
            }
        
            if (PlayerHandler == null)
            {
                PlayerHandler = FindObjectOfType<MyPlayer>();
            }


            if (playerRespawner == null)
            {
                initializeRespawner();
            }
            if (playerInventory == null)
            {
                this.playerInventory = this.AddComponent<PlayerInventory>();
            }

            if (CharacterAnimator == null)
            {
                CharacterAnimator = FindObjectOfType<FinalCharacterController>().CharacterAnimator;
            }

        }

        private void initializeRespawner()
        {
            this.playerRespawner = this.gameObject.AddComponent<PlayerRespawner>();
            playerRespawner.Initialize(PlayerHandler, playerPrefab, RagdollPrefab, PlayerHandler.Character.transform.position);

        }

        public void CommitDie(string reason)
        {
            this.CommitDie(new Dictionary<string, object>(){{reason, null}});
        }

        private Dictionary<string, object> AddStanceDetails(Dictionary<string, object> rawCharacteristics)
        {
            if (CharacterAnimator != null)
            {
                if (CharacterAnimator.GetBool(_isCrouching))
                {
                    rawCharacteristics.Add("stance", "crouched");
                }
                else
                {
                    rawCharacteristics.Add("stance", "standing");
                }
            }
            return rawCharacteristics;
        }

        public void CommitDie(Dictionary<string, object> rawCharacteristics)
        {
            this.CommitDie(new DeathCharacteristics(AddStanceDetails(rawCharacteristics)));
        }

        protected void CommitDie(DeathCharacteristics deathCharacteristics)
        {
            SendSound(deathCharacteristics);
            if (LevelScoreManager.instance != null)
            {
                LevelScoreManager.instance.RecordScoreEvent(deathCharacteristics);
            }
            this.playerRespawner.OnPlayerDeath(deathCharacteristics);
        }
        
        private void SendSound(DeathCharacteristics deathCharacteristics)
        {
            AudioClip soundToPlay = GetSoundToPlay(deathCharacteristics);
            if (soundToPlay == null) return;
            var soundEffectPlayer = FindObjectOfType<SoundEffectPlayer>();
            soundEffectPlayer.PlaySound(soundToPlay);
        }

        private AudioClip GetSoundToPlay(DeathCharacteristics deathCharacteristics)
        {
            string reason = deathCharacteristics.getReason();
            switch (reason)
            {
                case "explosion":
                    return audioClipArray[0];
                case "freezing":
                    return audioClipArray[1];
                case "burning":
                    return audioClipArray[2];
                case "electrocution":
                    return audioClipArray[3];
                default: Debug.LogWarning($"{reason} is not defined for sound on death;");
                    return null;
            }
        }

        private bool devToolsEnabled()
        {
            return this.enableCheatyDevShortcuts;
        }

        public void Update()
        {
            if (devToolsEnabled())
            {
                if (Input.GetKeyDown(KeyCode.K))
                {
                    CommitDie("Keyboard shortcut");
                }

                if (Input.GetKeyDown(KeyCode.P))
                {
                    CommitDie("electrocution");

                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            }

        }

        public PlayerRespawner getRespawner()
        {
            return this.playerRespawner;
        }

        public PlayerInventory getInventory()
        {
            return this.playerInventory;
        }
    }

    /*
    [CustomEditor(typeof(GameManager))]
    public class SFXRoster : Editor
    {
        public override void OnInspectorGUI()
        {
            
        }
    }
    */
}