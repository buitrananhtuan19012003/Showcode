using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairTarget : MonoBehaviour
{
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hitInfo;
    private float maxCroissHairTargetDistance;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        if (DataManager.HasInstance)
        {
            maxCroissHairTargetDistance = DataManager.Instance.GlobalConfig.maxCroissHairTargetDistance;
        }
    }

    void Update()
    {
        ray.origin = mainCamera.transform.position;
        ray.direction = mainCamera.transform.forward;
        if(Physics.Raycast(ray, out hitInfo))
        {
            transform.position = hitInfo.point;
        }
        else
        {
            transform.position = ray.GetPoint(maxCroissHairTargetDistance);
        }
    }
}
