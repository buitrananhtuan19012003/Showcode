using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Ignore Raycast")))
        {
            other.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Default")))
        {
            other.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }
}