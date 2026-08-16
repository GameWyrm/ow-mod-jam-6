using UnityEngine;
using System.Collections.Generic;

namespace Mod_Jam_6
{
    public class RevealVolume : MonoBehaviour
    {
        public List<string> factsToReveal = new();

        private OWTriggerVolume volume;

        private void Start()
        {
            volume = GetComponent<OWTriggerVolume>();
            volume.OnEntry += RevealFacts;
        }

        private void RevealFacts(GameObject hitObject)
        {
            if (!hitObject.CompareTag("PlayerDetector")) return;
            foreach (var fact in factsToReveal)
            {
                ModJam6.RevealFact(fact);
            }
        }

        private void OnValidate()
        {
            string nameAddendum = "Reveal";
            foreach (var fact in factsToReveal)
            {
                nameAddendum += " " + fact.ToString();
            }
            if (gameObject.name != nameAddendum) gameObject.name = nameAddendum;
        }
    }
}
