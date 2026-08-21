using UnityEngine;
using UnityEngine.UI;

namespace Mod_Jam_6
{
    public class NHTranslatable : MonoBehaviour
    {
        public string translationKey;

        private void Start()
        {
            Text text = GetComponent<Text>();
            text.text = ModJam6.NewHorizons.GetTranslationForOtherText(translationKey);
        }
    }
}
