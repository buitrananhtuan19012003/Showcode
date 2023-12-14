using UnityEngine;

public class PlayerInteract : PlayerAbstract
{
    [SerializeField] private float interactDistance = 7.5f;
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private Transform raycastOriginal;

    public Transform InteractableRaycastObject;

    private void Update()
    {
        if(playerCtrl.PlayerCamera.TPSCamera.gameObject.activeInHierarchy == true)
        {
            this.raycastOriginal = InteractableRaycastObject.transform;
        }
        else
        {
            this.raycastOriginal = this.playerCtrl.PlayerCamera.MainCamera.transform;
        }
    }

    public void Interact()
    {
        IInteractable interactable = this.GetInteractableObjectByRaycast();
        if (interactable != null)
        {
            interactable.Interact(transform);
        }
    }

    public IInteractable GetInteractableObjectByRaycast()
    {
        if (this.raycastOriginal == null) return null;

        Physics.Raycast(this.raycastOriginal.transform.position, this.raycastOriginal.transform.forward, 
            out RaycastHit hitInfo, this.interactDistance, this.pickupLayer);
        if (hitInfo.collider != null && hitInfo.transform.TryGetComponent(out IInteractable interactable))
        {
            if (interactable.CanInteract())
                return interactable;
            else
                return null;
        }
        return null;
    }

    //[SerializeField] private float interactRange = 2f;
    //public IInteractable GetInteractableObject()
    //{
    //    List<IInteractable> interactableList = new List<IInteractable>();
    //    Collider[] colliderArray = Physics.OverlapSphere(transform.position, this.interactRange);
    //    foreach (Collider collider in colliderArray)
    //    {
    //        if (collider.TryGetComponent(out IInteractable interactable))
    //        {
    //            if (interactable.CanInteract())
    //                interactableList.Add(interactable);
    //        }
    //    }

    //    IInteractable closestInteractable = null;
    //    foreach (IInteractable interactable in interactableList)
    //    {
    //        if (closestInteractable == null)
    //        {
    //            closestInteractable = interactable;
    //        }
    //        else
    //        {
    //            if (Vector3.Distance(transform.position, interactable.GetTransform().position) <
    //                Vector3.Distance(transform.position, closestInteractable.GetTransform().position))
    //            {
    //                closestInteractable = interactable;
    //            }
    //        }
    //    }

    //    return closestInteractable;
    //}
    //private void OnDrawGizmos()
    //{
    //    Camera mainCamera = this.playerCtrl.PlayerCamera.MainCamera;
    //    if (mainCamera == null) return;

    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawLine(mainCamera.transform.position, mainCamera.transform.position + 
    //        (mainCamera.transform.forward * this.interactDistance));
    //}
}
