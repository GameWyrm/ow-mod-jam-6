using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mod_Jam_6.Controllers
{
    public class ConditionTracker : MonoBehaviour
    {
        public static ConditionTracker Instance;

        private List<string> colorPrefixes = new() { "BLUE", "GREEN", "PURPLE" };
        private List<string> typeSuffixes = new() { "WISP", "ECHO", "RIPPLE" };

        private void Start()
        {
            Instance = this;

            StartCoroutine(Setup());
            GlobalMessenger<string, bool>.AddListener("DialogueConditionChanged", CheckConditions);
        }

        private void CheckConditions(string condition, bool value)
        {
            ModJam6.Log($"Condition {condition} changed to {value}");

            if (!value) return;
            switch (condition)
            {
                case "PH_BLUE_TALK":
                    if (GetPersistentCondition("PH_BLUE_ECHO_P") && GetPersistentCondition("PH_BLUE_RIPPLE_P"))
                    {
                        StartCoroutine(SetPersistentCondition("PH_BLUE_COMPLETE"));
                    }
                    StartCoroutine(SetTempCondition("PH_BLUE_TALK"));
                    break;
                case "PH_PURPLE_TALK":
                    if (GetPersistentCondition("PH_PURPLE_ECHO_P") && GetPersistentCondition("PH_PURPLE_RIPPLE_P"))
                    {
                        StartCoroutine(SetPersistentCondition("PH_PURPLE_COMPLETE"));
                    }
                    StartCoroutine(SetTempCondition("PH_PURPLE_TALK"));
                    break;
                case "PH_GREEN_TALK":
                    if (GetPersistentCondition("PH_GREEN_ECHO_P") && GetPersistentCondition("PH_GREEN_RIPPLE_P"))
                    {
                        StartCoroutine(SetPersistentCondition("PH_GREEN_COMPLETE"));
                    }
                    StartCoroutine(SetTempCondition("PH_GREEN_TALK"));
                    break;
                case "PH_LEARN_MEDITATION":
                    GameObject.Find("PauseMenu").transform.Find("PauseMenuCanvas/PauseMenuBlock/PauseMenuItems/PauseMenuItemsLayout/Button-EndCurrentLoop").gameObject.SetActive(true);
                    PlayerData.SetPersistentCondition("KNOWS_MEDITATION", true);
                    break;
            }
        }

        private void OnDestroy()
        {
            GlobalMessenger<string, bool>.RemoveListener("DialogueConditionChanged", CheckConditions);
        }

        private IEnumerator Setup()
        {
            yield return new WaitForEndOfFrame();
            DialogueConditionManager manager = DialogueConditionManager.SharedInstance;
            foreach (var color in colorPrefixes)
            {
                foreach(var type in typeSuffixes)
                {
                    if (PlayerData.GetPersistentCondition($"PH_{color}_{type}_P"))
                    {
                        string condition = $"PH_{color}_{type}_T";
                        
                        if (!manager.ConditionExists(condition)) manager.AddCondition(condition);
                        manager.SetConditionState(condition, true);
                    }
                }
            }
        }

        private IEnumerator SetPersistentCondition(string condition)
        {
            ModJam6.Log($"Setting Persistent Condition {condition}");
            yield return new WaitForEndOfFrame();
            PlayerData.SetPersistentCondition(condition, true);
        }

        private IEnumerator SetTempCondition(string condition)
        {
            yield return new WaitForEndOfFrame();
            DialogueConditionManager.SharedInstance.SetConditionState(condition, false);
        }

        private bool GetPersistentCondition(string condition)
        {
            ModJam6.Log($"Testing {condition}, it is {PlayerData.GetPersistentCondition(condition)}");

            return PlayerData.PersistentConditionExists(condition) && PlayerData.GetPersistentCondition(condition);
        }
    }
}
