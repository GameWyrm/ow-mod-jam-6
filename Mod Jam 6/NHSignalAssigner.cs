using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mod_Jam_6
{
    public class NHSignalAssigner : MonoBehaviour
    {
        [Tooltip("Path of the audio source (use for base game sounds)")]
        public string audio;

        [Tooltip("Audio clip to use, overrides Audio")]
        public AudioClip audioClip;

        [Tooltip("Name of the signal. Use the translation string.")]
        public string name;

        [Tooltip("Name of the frequency. Use the translation string.")]
        public string frequency;

        [Tooltip("Area where the signal scope will show the signal as full there. Red.")]
        public float sourceRadius = 1;

        [Tooltip("Distance from which you get 'unidentified signal nearby'. Green")]
        public float detectionRadius = 20;

        [Tooltip("Distance from which you can identify the signal. Blue")]
        public float identificationRadius = 10;

        [Tooltip("tbh idk what this does your sol")]
        public bool insideCloak = false;

        [Tooltip("If only the signal scope can hear it or if you can too. think like the travelers.")]
        public bool onlyAudibleToScope = true;

        [Tooltip("The fact it reveals")]
        public string reveals;

        [Tooltip("The min size of the audio source when not using onlyAudibleToScope. Yellow.")]
        public int minDistance = 0;

        [Tooltip("The max size of the audio source when not using onlyAudibleToScope. Magenta.")]
        public int maxDistance = 20;

        private void Awake()
        {
            ModJam6.NewHorizons.SpawnSignal(ModJam6.Instance, this.gameObject, audio, ModJam6.NewHorizons.GetTranslationForUI(name), ModJam6.NewHorizons.GetTranslationForUI(frequency), sourceRadius, detectionRadius, identificationRadius, insideCloak, onlyAudibleToScope, reveals);
            GetComponentInChildren<AudioSource>().minDistance = minDistance;
            GetComponentInChildren<AudioSource>().maxDistance = maxDistance;
        }

        private void Start()
        {
            if (audioClip != null)
            {
                GetComponentInChildren<AudioSource>().clip = audioClip;
                GetComponentInChildren<OWAudioSource>().clip = audioClip;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, sourceRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, detectionRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, identificationRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.transform.position, minDistance);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(this.transform.position, maxDistance);
        }
    }
}
