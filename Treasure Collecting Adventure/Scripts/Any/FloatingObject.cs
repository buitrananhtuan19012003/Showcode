using UnityEngine;

public class FloatingObject : SaiMonoBehaviour
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private float minY = 0f;
    [SerializeField] private float maxY = 0.25f;
    [SerializeField] private float moveSpeed = 0.25f;
    [SerializeField] private float rotateSpeed = 30f;
    [SerializeField] private float direction = 1f;
    [SerializeField] private bool isFloating = true;
    [SerializeField] private bool isRotating = true;
    [SerializeField] private bool isRandomMoveSpeed = true;
    [SerializeField] private bool isRandomRotateSpeed = true;
    [SerializeField] private bool isGizmo;

    private float randomMoveSpeed;
    private float randomRotateSpeed;
    private float targetMinY;
    private float targetMaxY;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.targetObject == null)
            this.targetObject = transform;
    }

    private void Start()
    {
        this.randomMoveSpeed = Random.Range(this.moveSpeed - 0.1f, this.moveSpeed);
        this.randomRotateSpeed = Random.Range(this.rotateSpeed - 5f, this.rotateSpeed + 5f);

        this.targetMinY = this.targetObject.transform.localPosition.y + this.minY;
        this.targetMaxY = this.targetObject.transform.localPosition.y + this.maxY;
    }

    private void FixedUpdate()
    {
        this.Float();
        this.Rotate();
    }

    private void Float()
    {
        if (!this.isFloating) return;

        float movementSpeed = this.isRandomMoveSpeed ? this.randomMoveSpeed : this.moveSpeed;
        float newY = this.targetObject.localPosition.y + this.direction * movementSpeed * Time.fixedDeltaTime;
        newY = Mathf.Clamp(newY, this.targetMinY, this.targetMaxY);

        this.targetObject.localPosition = new Vector3(this.targetObject.localPosition.x, newY, this.targetObject.localPosition.z);

        if (Mathf.Approximately(newY, this.targetMinY) || Mathf.Approximately(newY, this.targetMaxY))
        {
            this.direction *= -1f;
        }
    }

    private void Rotate()
    {
        if (!this.isRotating) return;

        float rotationSpeed = this.isRandomRotateSpeed ? this.randomRotateSpeed : this.rotateSpeed;
        this.targetObject.Rotate(Vector3.up * rotationSpeed * Time.fixedDeltaTime);
    }

    private void OnDrawGizmos()
    {
        if (isGizmo)
        {
            // Draw sphere at minY position
            Vector3 minYPosition = this.targetObject.TransformPoint(new Vector3(0f, this.minY, 0f));
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(minYPosition, 0.05f);
            Gizmos.DrawLine(minYPosition - Vector3.right * 0.5f, minYPosition + Vector3.right * 0.5f);

            // Draw sphere at maxY position
            Vector3 maxYPosition = this.targetObject.TransformPoint(new Vector3(0f, this.maxY, 0f));
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(maxYPosition, 0.05f);
            Gizmos.DrawLine(maxYPosition - Vector3.right * 0.5f, maxYPosition + Vector3.right * 0.5f);
        }
    }
}
