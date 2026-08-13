using UnityEngine;

namespace Mod_Jam_6
{
    public class VoidOrb : MonoBehaviour
    {
        public GameObject endDeathField;

        [HideInInspector]
        public bool shrinking;
        public float shrinkAmountPerSecond = 0.05f;

        private void Start()
        {
            TimeManager.instance.timeEvents[6].AddListener(StartContracting);
            //TODO set endDeathField to an ending volume from NH
        }

        private void Update()
        {
            if (shrinking)
            {
                float size = transform.localScale.x;
                transform.localScale = Vector3.one * (size - (shrinkAmountPerSecond * Time.deltaTime));
                if (transform.localScale.x <= 0f)
                {
                    shrinking = false;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && Vector3.Distance(other.transform.position, transform.position) < 1000)
            {
                if (TimeLoop._timeLoopEnabled)
                {
                    ModJam6.Log("Killing player");
                    Locator.GetDeathManager().KillPlayer(DeathType.Energy);
                }
                else
                {
                    ModJam6.Log("Ending player");
                    endDeathField.transform.position = Locator.GetPlayerBody().transform.position;
                }
            }
        }

        private void StartContracting()
        {
            shrinking = true;
        }

        
    }
}
