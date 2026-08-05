using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mod_Jam_6
{
    public class TeleportShiplog : MonoBehaviour
    {
        [SerializeField] GameObject targetLocation;
        //in EotP i have a set object thats refrenced here, but it could be done diffrentlly
        [SerializeField] GameObject shipLog;

        private void Start()
        {
            GetComponent<OWTriggerVolume>().OnEntry += Teleport;
        }

        private void Teleport(GameObject hitObject)
        {
            if (hitObject.CompareTag("PlayerDetector"))
            {
                StartCoroutine(TeleportWait());
            }
        }

        private IEnumerator TeleportWait()
        {
            yield return new WaitForSeconds(3);
            ModJam6.Instance.ModHelper.Console.WriteLine("Moving ship log (supposedly)", OWML.Common.MessageType.Info);
            shipLog.transform.position = targetLocation.transform.position;
            shipLog.transform.rotation = targetLocation.transform.rotation;
        }
    }
}
