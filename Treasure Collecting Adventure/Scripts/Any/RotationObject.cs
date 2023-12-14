using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Vector3 axis = Vector3.zero;
    private void FixedUpdate()
    {
        transform.Rotate(this.axis, this.rotationSpeed * Time.fixedDeltaTime);
    }
}
