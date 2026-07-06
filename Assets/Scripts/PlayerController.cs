using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Lane Settings")]
    [SerializeField] private float laneDistance = 3f;
    [SerializeField] private float laneSwitchSpeed = 10f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float gravityMultiplier = 2.5f;

    [Header("Slide Settings")]
    [SerializeField] private float slideDuration = 0.8f;
    [SerializeField] private float normalHeight = 2f;
    [SerializeField] private float slideHeight = 1f;

    [Header("Forward Movement")]
    [SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float speedIncrement = 0.5f;
    [SerializeField] private float maxSpeed = 30f;

    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;
    private int _currentLane = 1;
    private Vector3 _targetPosition;
    private bool _isGrounded = true;
    private bool _isSliding = false;
    private float _slideTimer = 0f;
    private float _currentForwardSpeed;
    private Vector3 _defaultColliderCenter;
    private float _defaultColliderHeight;

    public bool IsGrounded => _isGrounded;
    public float CurrentSpeed => _currentForwardSpeed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _rb.useGravity = true;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _defaultColliderHeight = _capsuleCollider.height;
        _defaultColliderCenter = _capsuleCollider.center;
        _currentForwardSpeed = forwardSpeed;
    }

    private void Start()
    {
        _targetPosition = transform.position;
    }

    private void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsPlaying) return;

        HandleLaneSwitch();
        HandleJump();
        HandleSlide();
        IncreaseSpeedOverTime();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsPlaying) return;

        ApplyForwardMovement();
        ApplyExtraGravity();
        SmoothLaneTransition();
    }

    private void HandleLaneSwitch()
    {
        if (InputManager.Instance == null) return;

        if (InputManager.Instance.MoveLeft && _currentLane > 0)
        {
            _currentLane--;
            UpdateTargetPosition();
        }
        else if (InputManager.Instance.MoveRight && _currentLane < 2)
        {
            _currentLane++;
            UpdateTargetPosition();
        }
    }

    private void UpdateTargetPosition()
    {
        float targetX = (_currentLane - 1) * laneDistance;
        _targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
    }

    private void SmoothLaneTransition()
    {
        Vector3 currentPos = _rb.position;
        Vector3 desiredPos = new Vector3(
            Mathf.Lerp(currentPos.x, _targetPosition.x, laneSwitchSpeed * Time.fixedDeltaTime),
            currentPos.y,
            currentPos.z
        );
        _rb.MovePosition(desiredPos);
    }

    private void HandleJump()
    {
        if (InputManager.Instance == null) return;

        if (InputManager.Instance.Jump && _isGrounded && !_isSliding)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, jumpForce, _rb.velocity.z);
            _isGrounded = false;
        }
    }

    private void HandleSlide()
    {
        if (InputManager.Instance == null) return;

        if (InputManager.Instance.Slide && _isGrounded && !_isSliding)
        {
            StartSlide();
        }

        if (_isSliding)
        {
            _slideTimer -= Time.deltaTime;
            if (_slideTimer <= 0f)
            {
                EndSlide();
            }
        }
    }

    private void StartSlide()
    {
        _isSliding = true;
        _slideTimer = slideDuration;
        _capsuleCollider.height = slideHeight;
        _capsuleCollider.center = new Vector3(_defaultColliderCenter.x, _defaultColliderCenter.y - (normalHeight - slideHeight) * 0.5f, _defaultColliderCenter.z);
    }

    private void EndSlide()
    {
        _isSliding = false;
        _capsuleCollider.height = _defaultColliderHeight;
        _capsuleCollider.center = _defaultColliderCenter;
    }

    private void ApplyForwardMovement()
    {
        Vector3 forwardVelocity = new Vector3(0f, _rb.velocity.y, _currentForwardSpeed);
        _rb.velocity = forwardVelocity;
    }

    private void ApplyExtraGravity()
    {
        if (!_isGrounded && _rb.velocity.y < 0)
        {
            _rb.AddForce(Vector3.down * gravityMultiplier * Physics.gravity.magnitude, ForceMode.Acceleration);
        }
    }

    private void IncreaseSpeedOverTime()
    {
        if (_currentForwardSpeed < maxSpeed)
        {
            _currentForwardSpeed += speedIncrement * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance?.GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            GameManager.Instance?.AddCoin();
            other.gameObject.SetActive(false);
        }
    }

    public void ResetPlayer()
    {
        _currentLane = 1;
        _currentForwardSpeed = forwardSpeed;
        _isSliding = false;
        _isGrounded = true;
        _slideTimer = 0f;
        EndSlide();
        UpdateTargetPosition();
        _rb.velocity = Vector3.zero;
        transform.position = new Vector3(0f, transform.position.y, transform.position.z);
    }
}
