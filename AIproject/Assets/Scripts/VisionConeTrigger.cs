using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VisionConeTrigger : MonoBehaviour
{
    private AIController ai;
    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponentInParent<AIController>();
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player"))
       {
           //set is in vision to true
           ai.SetPlayerInVisionCone(true);
       }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //set is in vision cone to false
            ai.SetPlayerInVisionCone(false);
        }
    }
}
