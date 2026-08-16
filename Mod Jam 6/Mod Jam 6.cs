using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using System.Reflection;
using System;
using UnityEngine;

namespace Mod_Jam_6
{
    public class ModJam6 : ModBehaviour
    {
        public static ModJam6 Instance;
        public static INewHorizons NewHorizons;

        public GameObject shipLogScreen;
        public ShipLogManager shipLogManager;

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
            
            NewHorizons.GetStarSystemLoadedEvent().AddListener((system) =>
            {
                ModHelper.Events.Unity.FireInNUpdates(() =>
                {
                    if (system == "VoidDimension")
                    {
                        Log("Looking for ship");
                        shipLogScreen = GameObject.Find("Ship_Body/Module_Cabin/Systems_Cabin/ShipLogPivot");
                        Log($"Ship is {(shipLogScreen == null ? "NULL" : "FOUND")}");

                        Log("Looking for log manager");
                        shipLogManager = Locator.GetShipLogManager();
                        Log($"Log is {(shipLogManager == null ? "NULL": "FOUND")}");
                    }
                }, 50);
            });
        }

        public static void RevealFact(string factID)
        {
            try
            {
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

