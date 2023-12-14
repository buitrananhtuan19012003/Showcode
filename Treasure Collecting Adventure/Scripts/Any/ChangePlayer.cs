using Cinemachine;
using System.Collections;
using UnityEngine;

public class ChangePlayer : MonoBehaviour
{
    [SerializeField] private float transformTime = 2.5f;
    [SerializeField] private ParticleSystem transformFX;
    [SerializeField] private ParticleSystem timeoutFX;
    [SerializeField] private CinemachineFreeLook TPSCamera;
    [SerializeField] private CinemachineVirtualCamera FPSCamera;
    [SerializeField] private PlayerCtrl playerCtrl;

    [SerializeField] private GameObject player1;
    [SerializeField] private Transform p1_TPS_LookAt;
    [SerializeField] private Transform p1_FPS_Follow;

    [SerializeField] private GameObject player2;
    [SerializeField] private Transform p2_TPS_LookAt;
    [SerializeField] private Transform p2_FPS_Follow;

    private bool check = false;
    private bool ready = true;

    private void Awake()
    {
        this.TPSCamera.Follow = this.check ? this.player1.transform : this.player2.transform;
        this.TPSCamera.LookAt = this.check ? this.p1_TPS_LookAt : this.p2_TPS_LookAt;
        this.FPSCamera.Follow = this.check ? this.p1_FPS_Follow : this.p2_FPS_Follow;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && this.ready)
        {
            StartCoroutine(this.TransformationCoroutine());
        }
    }

    private IEnumerator TransformationCoroutine()
    {
        this.ready = false;
        Animator animator = player2.GetComponent<Animator>();
        animator.SetTrigger("Transformation");
        yield return new WaitForSeconds(this.transformTime);

        PlayTimeoutFX(this.player1.transform);
        yield return new WaitForSeconds(0.5f);

        Devolution();
    }

    private void Devolution()
    {
        this.ready = true;
        this.SetActivePlayer(false);
    }

    private void PlayTransformFX(Vector3 position)
    {
        this.transformFX.transform.position = position;
        this.transformFX.Play();
    }

    private void PlayTimeoutFX(Transform parent)
    {
        Vector3 offset = new Vector3(0f, 1f, 0f);
        this.timeoutFX.transform.SetParent(parent);
        this.timeoutFX.transform.position = parent.position + offset;
        this.timeoutFX.Play();
    }

    public void Evolution()
    {
        this.SetActivePlayer(true);
        this.player1.GetComponent<Animator>().SetTrigger("Transformation");

        this.PlayTransformFX(this.player1.transform.position);
    }

    private void SetActivePlayer(bool check)
    {
        //if (check == true)    => Active player1
        //if (check == false)   => Active player2

        if (check)
        {
            //this.playerCtrl.SetPlayerTransform(this.player1.transform);
            this.player1.transform.position = this.player2.transform.position;
            this.player1.transform.rotation = this.player2.transform.rotation;
        }
        else
        {
            //this.playerCtrl.SetPlayerTransform(this.player2.transform);
            this.player2.transform.position = this.player1.transform.position;
            this.player2.transform.rotation = this.player1.transform.rotation;
        }

        this.player1.SetActive(check);
        this.player2.SetActive(!check);

        this.TPSCamera.Follow = check ? this.player1.transform : this.player2.transform;
        this.TPSCamera.LookAt = check ? this.p1_TPS_LookAt : this.p2_TPS_LookAt;
        this.FPSCamera.Follow = check ? this.p1_FPS_Follow : this.p2_FPS_Follow;

        this.check = check;
    }
}
