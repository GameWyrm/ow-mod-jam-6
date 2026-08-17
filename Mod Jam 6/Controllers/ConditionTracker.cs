using UnityEngine;
using System.Collections;

namespace Mod_Jam_6.Controllers
{
    public class ConditionTracker : MonoBehaviour
    {
        public static ConditionTracker Instance;

        private string[] colorPrefixes = ["BLUE", "GREEN", "PURPLE"];
        private string[] typeSuffixes = ["WISP", "ECHO", "RIPPLE"];

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
