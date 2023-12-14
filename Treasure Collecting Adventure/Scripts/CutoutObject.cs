using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : SaiMonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform targetObject;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float cutoutSize = 0.1f;
    [SerializeField] private float falloffSize = 0.05f;
    [SerializeField] private Material cutoutMaterial;
    private Dictionary<Transform, Material> originalMaterials = new Dictionary<Transform, Material>();

    public Transform TargetObject { get => this.targetObject; set => this.targetObject = value; }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.mainCamera == null)
            this.mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        if (this.targetObject == null) return;

        Vector2 cutoutPosition = this.mainCamera.WorldToViewportPoint(this.targetObject.position);
        cutoutPosition.y /= (Screen.width / Screen.height);

        Vector3 offset = this.targetObject.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, this.wallMask);
        foreach (RaycastHit hit in hitObjects)
        {
            Transform hitTransform = hit.transform;
            if (hitTransform.TryGetComponent<Renderer>(out Renderer renderer))
            {
                if (!this.originalMaterials.TryGetValue(hitTransform, out Material material))
                {
                    material = renderer.material;
                    this.originalMaterials.Add(hitTransform, material);
                    this.cutoutMaterial.SetTexture("_MainTexture", material.mainTexture);
                    hitTransform.GetComponent<Renderer>().material = this.cutoutMaterial;
                }

                Material currentMaterial = renderer.material;
                currentMaterial.SetVector("_CutoutPos", cutoutPosition);
                currentMaterial.SetFloat("_CutoutSize", this.cutoutSize);
                currentMaterial.SetFloat("_FalloffSize", this.falloffSize);
            }
        }

        List<Transform> transformsToRemove = new List<Transform>();
        foreach (KeyValuePair<Transform, Material> kvp in this.originalMaterials)
        {
            Transform transformToRemove = kvp.Key;
            if (!System.Array.Exists(hitObjects, hit => hit.transform == transformToRemove))
            {
                transformToRemove.GetComponent<Renderer>().material = kvp.Value;
                transformsToRemove.Add(transformToRemove);
            }
        }
        foreach (Transform transformToRemove in transformsToRemove)
        {
            this.originalMaterials.Remove(transformToRemove);
        }
    }

    //void OnDrawGizmos()
    //{
    //    if (this.targetObject == null)
    //        return;

    //    Vector3 offset = this.targetObject.position - transform.position;
    //    RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, this.wallMask);

    //    Gizmos.color = Color.red;
    //    foreach (var hit in hitObjects)
    //    {
    //        Gizmos.DrawLine(transform.position, hit.point);
    //        Gizmos.DrawSphere(hit.point, 0.1f);
    //    }
    //}
}
