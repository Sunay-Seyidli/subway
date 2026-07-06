using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [Header("Swipe Settings")]
    [SerializeField] private float swipeThreshold = 50f;
    [SerializeField] private float swipeTimeThreshold = 0.5f;

    private Vector2 _fingerDownPosition;
    private Vector2 _fingerUpPosition;
    private float _fingerDownTime;
    private bool _isSwiping = false;

    public bool MoveLeft { get; private set; }
    public bool MoveRight { get; private set; }
    public bool Jump { get; private set; }
    public bool Slide { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        ResetInputs();

        if (DeviceDetector.IsDesktop())
        {
            HandleDesktopInput();
        }
        else
        {
            HandleMobileInput();
        }
    }

    private void ResetInputs()
    {
        MoveLeft = false;
        MoveRight = false;
        Jump = false;
        Slide = false;
    }

    private void HandleDesktopInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            MoveLeft = true;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            MoveRight = true;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            Jump = true;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            Slide = true;
    }

    private void HandleMobileInput()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                _fingerDownPosition = touch.position;
                _fingerUpPosition = touch.position;
                _fingerDownTime = Time.time;
                _isSwiping = true;
                break;

            case TouchPhase.Moved:
                if (!_isSwiping) return;
                _fingerUpPosition = touch.position;
                DetectSwipe();
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                if (!_isSwiping) return;
                _fingerUpPosition = touch.position;
                DetectSwipe();
                _isSwiping = false;
                break;
        }
    }

    private void DetectSwipe()
    {
        float elapsedTime = Time.time - _fingerDownTime;
        if (elapsedTime > swipeTimeThreshold) return;

        Vector2 swipeDelta = _fingerUpPosition - _fingerDownPosition;
        float horizontal = Mathf.Abs(swipeDelta.x);
        float vertical = Mathf.Abs(swipeDelta.y);

        if (horizontal < swipeThreshold && vertical < swipeThreshold) return;

        if (horizontal > vertical)
        {
            if (swipeDelta.x > 0)
                MoveRight = true;
            else
                MoveLeft = true;
        }
        else
        {
            if (swipeDelta.y > 0)
                Jump = true;
            else
                Slide = true;
        }

        _isSwiping = false;
    }
}
