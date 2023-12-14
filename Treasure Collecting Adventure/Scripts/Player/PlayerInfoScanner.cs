using UnityEngine;

public class PlayerInfoScanner : PlayerAbstract
{
    [SerializeField] private float scanDistance = 7.5f;
    [SerializeField] private LayerMask scanLayer;

    public IInfoScanner GetInfoScannerObjectByRaycast()
    {
        Physics.Raycast(this.playerCtrl.PlayerCamera.MainCamera.transform.position, this.playerCtrl.PlayerCamera.MainCamera.transform.forward, 
            out RaycastHit hitInfo, this.scanDistance, this.scanLayer);
        if (hitInfo.collider != null && hitInfo.transform.TryGetComponent(out IInfoScanner inforScanner))
        {
            if (inforScanner.CanScan())
                return inforScanner;
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        Camera mainCamera = this.playerCtrl.PlayerCamera.MainCamera;

        if (mainCamera == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(mainCamera.transform.position, mainCamera.transform.position + 
            (mainCamera.transform.forward * this.scanDistance));
    }
}
