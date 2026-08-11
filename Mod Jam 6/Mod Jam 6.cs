using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using System.Reflection;
using UnityEngine;

namespace Mod_Jam_6
{
    public class ModJam6 : ModBehaviour
    {
        public static ModJam6 Instance;
        public INewHorizons NewHorizons;

        public void Awake()
        {
            Instance = this;
            // You won't be able to access OWML's mod helper in Awake.
            // So you probably don't want to do anything here.
            // Use Start() instead.
        }

        public void Start()
        {
            // Starting here, you'll have access to OWML's mod helper.
            ModHelper.Console.WriteLine($"My mod {nameof(ModJam6)} is loaded!", MessageType.Success);

            // Get the New Horizons API and load configs
            NewHorizons = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            NewHorizons.LoadConfigs(this);

            new Harmony("GameWyrm.Mod Jam 6").PatchAll(Assembly.GetExecutingAssembly());

            // Example of accessing game code.
            OnCompleteSceneLoad(OWScene.TitleScreen, OWScene.TitleScreen); // We start on title screen
            LoadManager.OnCompleteSceneLoad += OnCompleteSceneLoad;
        }

        public void OnCompleteSceneLoad(OWScene previousScene, OWScene newScene)
        {
            if (newScene != OWScene.SolarSystem) return;
            ModHelper.Console.WriteLine("Loaded into solar system!", MessageType.Success);

            NewHorizons.GetStarSystemLoadedEvent().AddListener((system) =>
            {
                ModHelper.Events.Unity.FireInNUpdates(() =>
                {
                    if (system == "VoidDimension")
                    {
                        var shipLogScreen = GameObject.Find("Ship_Body/Module_Cabin/Systems_Cabin/ShipLogPivot");
                        var shipLogbody = GameObject.Find("shiplog_parent");
                        shipLogScreen.transform.parent = shipLogbody.transform;
                        shipLogScreen.transform.localPosition = Vector3.zero;
                        shipLogScreen.transform.localEulerAngles = Vector3.zero;
                    }
                }, 50);
            });
        }
    }
}

