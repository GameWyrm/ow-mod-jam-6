using UnityEngine;

namespace Mod_Jam_6
{
    public class AppearOnCondition : MonoBehaviour
    {
        public string condition;
        public bool alsoSavePersistent;

        private void Start()
        {
            GlobalMessenger<string, bool>.AddListener("DialogueConditionChanged", GetConditionChanged);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            GlobalMessenger<string, bool>.RemoveListener("DialogueConditionChanged", GetConditionChanged);
        }

        private void GetConditionChanged(string condition, bool state)
        {
            if (condition == this.condition && state)
            {
                gameObject.SetActive(true);
                if (alsoSavePersistent) PlayerData.SetPersistentCondition(condition.Replace("_T", "_P"), state);
            }
        }
    }
}
