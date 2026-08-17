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
    }
}
