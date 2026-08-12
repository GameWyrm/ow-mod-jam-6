using System.Collections;
using UnityEngine;

namespace Mod_Jam_6
{
    public class TeleportShiplog : MonoBehaviour
    {
        public GameObject targetLocation;

        private Vector3 offsetRotation = new Vector3(-8f, 180, 0);
        private Vector3 offsetPosition = new Vector3(0.22f, -0.58f, -0.82f);

        private void Start()
        {
            GetComponentInChildren<OWTriggerVolume>().OnEntry += Teleport;
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
            yield return new WaitForSeconds(0.5f);
            ModJam6.Instance.ModHelper.Console.WriteLine("Moving ship log (supposedly)", OWML.Common.MessageType.Info);
            ModJam6.Instance.shipLogScreen.transform.parent = targetLocation.transform;
            ModJam6.Instance.shipLogScreen.transform.position = targetLocation.transform.position + offsetPosition;
            ModJam6.Instance.shipLogScreen.transform.eulerAngles = targetLocation.transform.eulerAngles + offsetRotation;
        }
    }
}
