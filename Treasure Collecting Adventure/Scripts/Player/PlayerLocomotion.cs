
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : PlayerAbstract
{
    [Header("Movement Flag")]
    public bool IsJumping;
    public bool IsSprinting;
    public bool IsGrounded;
    public bool IsWalking;
    public bool Is1D = false;

    [Header("Movement Speed")]
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float rotationSpeedTPS = 15f;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float jumpDamp = 0.8f;
    [SerializeField] private float airControl = 2.5f;
    [SerializeField] private float stepDown = 0.4f;
    [SerializeField] private float jumpheight = 3;
    [SerializeField] private float gravity = 30;
    [SerializeField] private float speedDecrease;
    [SerializeField] private Vector3 rootMotion;
    [SerializeField] public Vector3 velocity;
    private Vector3 movementDirection;
    private bool check = true;

    [Space(10)]
    [SerializeField, Range(0.1f, 5f)] private float historicalPositionDuration = 1;
    [SerializeField, Range(0.001f, 1f)] private float historicalPostionInterval = 0.1f;

    public Vector3 AverageVelocity
    {
        get
        {
            Vector3 average = Vector3.zero;
            foreach (Vector3 velocity in this.historicalVelocities)
            {
                average += velocity;
            }
            average.y = 0;

            return average / this.historicalVelocities.Count;
        }
    }

    private Queue<Vector3> historicalVelocities;
    private float lastPositionTime;
    private int maxQueueSize;

    protected override void Awake()
    {
        base.Awake();
        this.maxQueueSize = Mathf.CeilToInt(1f / this.historicalPostionInterval * this.historicalPositionDuration);
        this.historicalVelocities = new Queue<Vector3>(this.maxQueueSize);
    }

    private void Start()
    {
        if (this.playerCtrl.Character)
        {
            this.HandleSpeed();
            this.AirControl();
        }
    }

    public void SeOnDisableLocomation()
    {
        this.check = true;
        //if (this.playerCtrl.Character.EventAnimator)
        //{
        //    Debug.Log("Locomotion_disable");
        //    playerCtrl.Character.EventAnimator.OnAnimatorMoveEvent -= HandleAnimatorMoveEvent;
        //}

        this.ResetLocomotion();
    }

    public void HanldeUpdateAllMovement()
    {
        HandleJump();
        Handle1DMode();
        // HandleSprinting();

        if (this.lastPositionTime + this.historicalPostionInterval <= Time.time)
        {
            if (this.historicalVelocities.Count == this.maxQueueSize)
            {
                this.historicalVelocities.Dequeue();
            }

            this.historicalVelocities.Enqueue(this.playerCtrl.CharacterController.velocity);
            this.lastPositionTime = Time.time;
        }
    }

    public void HanldeAllMovementFix()
    {

        this.HandleRotation();
        this.HandleUpdateMove();
        // this.SetSpeed();
    }

    public void HandleSpeed()
    {
        float speed = 1;
        this.speed = speed * (10 / (10 - (float)playerCtrl.Character.CharacterData.Agility));
    }

    public void AirControl()
    {
        this.airControl = speed * 1.7f;
    }

    public void SetOnEventAnimator()
    {
        //Debug.Log("SetOnEventAnimator");
        //Debug.Log("check: " + this.check);
        if (this.playerCtrl.Character.EventAnimator && this.check == true)
        {
            this.check = false;
            Character chr = this.playerCtrl.Character;
            if (chr is Character_Xerath)
            {
                Debug.Log("EVENT");
                Character_Xerath xerath = chr as Character_Xerath;
                xerath.B_OnEventAnimator.OnAnimatorMoveEvent += HandleAnimatorMoveEvent;
                xerath.A_OnEventAnimator.OnAnimatorMoveEvent += HandleAnimatorMoveEvent;
            }
            else
            {
                chr.EventAnimator.OnAnimatorMoveEvent += HandleAnimatorMoveEvent;
            }
        }
    }

    //public void SetSpeed()
    //{
    //    if (playerCtrl.PlayerWeapon.PlayerWeaponActive.IsFiring)
    //    {
    //        speed /= speedDecrease;
    //    }
    //}
    private void Handle1DMode()
    {
        this.playerCtrl.Animator.SetFloat("TypeMove", this.Is1D ? 0 : 1);
        if (Input.GetKeyDown(KeyCode.Q) && playerCtrl.PlayerCamera.IsTPSCamera)
        {
            this.Is1D = !this.Is1D;
        }
        if (Is1D)
        {
            HandleMovement1D();
        }
    }

    private void HandleMovement1D()
    {
        this.movementDirection = new Vector3(this.playerCtrl.PlayerInput.MovementInput.x, 0f, this.playerCtrl.PlayerInput.MovementInput.y);
        this.movementDirection = Quaternion.AngleAxis(this.playerCtrl.PlayerCamera.MainCamera.transform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        this.movementDirection.Normalize();

        Vector3 velocity = this.movementDirection;
        this.playerCtrl.CharacterController.Move(this.speed * velocity * Time.deltaTime);
    }

    public void SetIsSprinting(bool SprintingInput)
    {
        bool canSprint = (playerCtrl.PlayerInput.MovementInput != Vector2.zero) &&
            (!playerCtrl.PlayerWeapon.PlayerWeaponActive.IsFiring) && (!playerCtrl.PlayerAim.IsAim) &&
            (!playerCtrl.PlayerWeapon.PlayerWeaponReload.IsReload);
        if (canSprint && SprintingInput)
        {
            this.IsSprinting = true;
        }
        else
        {
            this.IsSprinting = false;
        }
    }

    private void HandleJump()
    {
        if (playerCtrl.PlayerInput.JumpInput)
        {
            Jump();
            playerCtrl.PlayerInput.JumpInput = false;
        }
    }

    private void HandleUpdateMove()
    {
        if (IsJumping)
        {
            UpdateInAir();
        }
        else
        {
            UpdateOnGround();
        }
    }

    private void HandleRotation()
    {
        if (this.Is1D)
        {
            if (this.movementDirection != Vector3.zero || this.movementDirection == Vector3.forward)
            {
                Quaternion toRatation = Quaternion.LookRotation(movementDirection, Vector3.up);
                this.playerCtrl.PlayerTransform.rotation = Quaternion.RotateTowards(this.playerCtrl.PlayerTransform.rotation, toRatation, this.rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            float yawCamera = this.playerCtrl.PlayerCamera.MainCamera.transform.eulerAngles.y;
            this.playerCtrl.PlayerTransform.rotation = Quaternion.Slerp(this.playerCtrl.PlayerTransform.rotation,
                Quaternion.Euler(0, yawCamera, 0), rotationSpeedTPS * Time.fixedDeltaTime);
        }

    }


    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        var ray = new Ray(this.playerCtrl.PlayerTransform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.2f))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;

            if (adjustedVelocity.y < 0)
            {
                return adjustedVelocity;
            }
        }

        return velocity;
    }

    public Vector3 CalculateAircontrol()
    {
        if (IsSprinting)
        {
            return ((this.playerCtrl.PlayerTransform.forward * playerCtrl.PlayerInput.MovementInput.y) + (this.playerCtrl.PlayerTransform.right * playerCtrl.PlayerInput.MovementInput.x)) * (airControl / 65f);
        }
        return ((this.playerCtrl.PlayerTransform.forward * playerCtrl.PlayerInput.MovementInput.y) + (this.playerCtrl.PlayerTransform.right * playerCtrl.PlayerInput.MovementInput.x)) * (airControl / 90f);
    }

    public void HandleAnimatorMoveEvent()
    {
        if (this.playerCtrl.Animator == null) return;
        rootMotion += playerCtrl.Animator.deltaPosition;
    }

    private void UpdateInAir()
    {
        velocity.y -= gravity * Time.fixedDeltaTime;
        Vector3 displacement = velocity * Time.fixedDeltaTime;
        displacement += CalculateAircontrol();
        playerCtrl.CharacterController.Move(displacement);
        IsJumping = !this.playerCtrl.CharacterController.isGrounded;
        rootMotion = Vector3.zero;
        this.playerCtrl.Animator.SetBool("IsJumping", IsJumping);
    }
    private void UpdateOnGround()
    {
        Vector3 stepForwardAmount = rootMotion * speed;
        Vector3 stepDownAmount = Vector3.down * stepDown;
        playerCtrl.CharacterController.Move(stepForwardAmount + stepDownAmount);
        rootMotion = Vector3.zero;

        if (!this.playerCtrl.CharacterController.isGrounded) //falling
        {
            SetInAir(0);
        }
    }
    public void Jump()
    {
        if (!IsJumping)
        {
            float jumpvelocity = Mathf.Sqrt(2 * gravity * jumpheight);
            SetInAir(jumpvelocity);
        }
    }

    private void SetInAir(float jumpvelocity)
    {
        IsJumping = true;
        velocity = playerCtrl.Animator.velocity * jumpDamp * speed;
        velocity.y = jumpvelocity;
        this.playerCtrl.Animator.SetBool("IsJumping", true);
    }

    public void ResetLocomotion()
    {
        this.IsJumping = false;
        this.IsSprinting = false;
    }
}
