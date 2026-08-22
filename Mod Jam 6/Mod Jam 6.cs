using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using System.Reflection;
using System;
using UnityEngine;
using NewHorizons.Utility;
using NewHorizons.Utility.Files;

namespace Mod_Jam_6
{
    public class ModJam6 : ModBehaviour
    {
        public static ModJam6 Instance;
        public static INewHorizons NewHorizons;

        public GameObject shipLogScreen;
        public ShipLogManager shipLogManager;

        private AssetBundle _DeityFlashbackBundle;

        public void Awake()
        {
            Instance = this;
            // You won't be able to access OWML's mod helper in Awake.
            // So you probably don't want to do anything here.
            // Use Start() instead.
        }

        public void Start()
        {
            // Get the New Horizons API and load configs
            NewHorizons = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            NewHorizons.LoadConfigs(this);

            new Harmony("GameWyrm.Mod Jam 6").PatchAll(Assembly.GetExecutingAssembly());

            LoadManager.OnStartSceneLoad += OnStartSceneLoad;
            
            NewHorizons.GetStarSystemLoadedEvent().AddListener((system) =>
            {
                ModHelper.Events.Unity.FireInNUpdates(() =>
                {
                    RiggedATPPatch.hasWarpedToSystem = false;
                    if (system == "VoidDimension")
                    {
                        Log("Looking for ship");
                        shipLogScreen = GameObject.Find("Ship_Body/Module_Cabin/Systems_Cabin/ShipLogPivot");
                        Log($"Ship is {(shipLogScreen == null ? "NULL" : "FOUND")}");

                        Log("Looking for log manager");
                        shipLogManager = Locator.GetShipLogManager();
                        Log($"Log is {(shipLogManager == null ? "NULL": "FOUND")}");

                        PlaceFlashback();
                    }
                }, 50);
            });
        }

        private void PlaceFlashback()
        {
            var FlashbackCamera = GameObject.Find("FlashbackCamera");
            FlashbackCamera.GetComponent<Camera>().farClipPlane = 100000;
            FlashbackCamera.FindChild("Mask").transform.localScale = Vector3.zero;
            //FlashbackCamera.FindChild("Effects_NOM_FlashBackStreams").transform.localScale = Vector3.zero;

            if (_DeityFlashbackBundle == null)
            {
                _DeityFlashbackBundle = ModHelper.Assets.LoadBundle("assets/bundles/newsequence");
            }
            GameObject DeityFlashback = Instantiate(_DeityFlashbackBundle.LoadAsset<GameObject>("Assets/Mod Jam 6/maskAnim/NewSequence.prefab"));
            AssetBundleUtilities.ReplaceShaders(DeityFlashback);
            DeityFlashback.transform.parent = FlashbackCamera.transform;
            DeityFlashback.transform.localPosition = new Vector3(0, 0, 0);
            DeityFlashback.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        private void OnStartSceneLoad(OWScene previousScene, OWScene newScene)
        {
            if (previousScene == OWScene.TitleScreen && PlayerData.GetPersistentCondition("PH_PLAY_MOD"))
            {
                NewHorizons.SetDefaultSystem("VoidDimension");
            }
        }

        public static void RevealFact(string factID)
        {
            try
            {
                if (factID.Contains("$")) factID = factID.Replace("$", "");
                Instance.shipLogManager.RevealFact(factID);
            }
            catch (Exception e)
            {
                LogError($"Failed to teach fact \"{factID}\".\n{e.Message}");
            }
        }

        public static void Log(string message)
        {
            Instance.ModHelper.Console.WriteLine(message);
        }

        public static void LogError(string message)
        {
            Instance.ModHelper.Console.WriteLine(message, MessageType.Error);
        }

        public static void LogWarning(string message)
        {
            Instance.ModHelper.Console.WriteLine(message, MessageType.Warning);
        }
    }
}

