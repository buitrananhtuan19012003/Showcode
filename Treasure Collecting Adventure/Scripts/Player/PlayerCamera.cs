using UnityEngine;
using Cinemachine;

public class PlayerCamera : PlayerAbstract
{
    private bool isTPSCamera = true;

    public int POV;
    public Camera MainCamera;
    public CinemachineFreeLook TPSCamera;
    public CinemachineVirtualCamera FPSCamera;
    public GameObject IgnoreRaycastZone;
    // public CinemachineCameraOffset CameraOffsetTPS;
    public bool IsTPSCamera { get => this.isTPSCamera; set => this.isTPSCamera = value; }

    public void HandleUpdateCamera()
    {
        SetOriginalCamera();
        HandleCamera();
    }

    private void SetOriginalCamera()
    {
        if (TPSCamera.gameObject.activeInHierarchy == true && playerCtrl.PlayerInput.Mouse1_ButtonDown)
        {
            isTPSCamera = true;
        }
        else if (FPSCamera.gameObject.activeInHierarchy == true && playerCtrl.PlayerInput.Mouse1_ButtonDown)
        {
            isTPSCamera = false;
        }
    }

    private void HandleCamera()
    {
        if (playerCtrl.PlayerAim.IsAim)
        {
            this.playerCtrl.PlayerCamera.ChangeFPSCam();
        }
        if (!playerCtrl.PlayerAim.IsAim)
        {
            if (this.isTPSCamera)
            {
                this.ChangeTPSCam();
            }
            if (!this.isTPSCamera)
            {
                this.ChangeFPSCam();
            }
        }
    }

    private void ChangeFPSCam()
    {
        if (this.TPSCamera == null || this.FPSCamera == null) return;

        this.IgnoreRaycastZone.SetActive(false);
        playerCtrl.PlayerLocomotion.Is1D = false;
        TPSCamera.gameObject.SetActive(false);
        FPSCamera.gameObject.SetActive(true);
    }

    private void ChangeTPSCam()
    {
        if (this.TPSCamera == null || this.FPSCamera == null) return;

        this.IgnoreRaycastZone.SetActive(true);
        TPSCamera.gameObject.SetActive(true);
        FPSCamera.gameObject.SetActive(false);
    }

    public void SetCameraTarget()
    {
        if (this.playerCtrl.Character == null || this.TPSCamera == null || this.FPSCamera == null) return;
        this.TPSCamera.Follow = this.PlayerCtrl.PlayerTransform;
        this.TPSCamera.LookAt = this.PlayerCtrl.Character.TPS_LookAt;
        this.FPSCamera.Follow = this.PlayerCtrl.Character.FPS_Follow;
    }

    public void SetActiveCineCamera(bool isActive)
    {
        if (this.TPSCamera == null || this.FPSCamera == null) return;

        this.TPSCamera.enabled = isActive;
        this.FPSCamera.enabled = isActive;
    }

    public void ChangeOriginalCamera()
    {
        isTPSCamera = !isTPSCamera;
    }

    public void ChangeSpeedFPSCam(float xAxis, float yAxis)
    {
        if (this.TPSCamera == null || this.FPSCamera == null) return;

        FPSCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = xAxis;
        FPSCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = yAxis;
    }

    public void ChangeSpeedTPSCam(float xAxis, float yAxis)
    {
        if (this.TPSCamera == null || this.FPSCamera == null) return;

        TPSCamera.m_XAxis.m_MaxSpeed = xAxis;
        TPSCamera.m_YAxis.m_MaxSpeed = yAxis;
        TPSCamera.m_Lens.FieldOfView = 60;
    }

    public void ChangePOVFPS(int pov)
    {
        if (this.TPSCamera == null || this.FPSCamera == null) return;

        FPSCamera.m_Lens.FieldOfView = pov;
    }

}
