using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Canvas")]
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private CanvasScaler canvasScaler;

    [Header("Desktop UI")]
    [SerializeField] private GameObject desktopPanel;
    [SerializeField] private TextMeshProUGUI desktopScoreText;
    [SerializeField] private TextMeshProUGUI desktopCoinText;
    [SerializeField] private TextMeshProUGUI desktopSpeedText;

    [Header("Mobile UI")]
    [SerializeField] private GameObject mobilePanel;
    [SerializeField] private TextMeshProUGUI mobileScoreText;
    [SerializeField] private TextMeshProUGUI mobileCoinText;
    [SerializeField] private TextMeshProUGUI mobileSpeedText;

    [Header("Common UI")]
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button startButton;

    private TextMeshProUGUI _activeScoreText;
    private TextMeshProUGUI _activeCoinText;
    private TextMeshProUGUI _activeSpeedText;
    private GameObject _activePanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetupUIForDevice();

        if (startButton != null)
            startButton.onClick.AddListener(OnStartClicked);
        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);

        ShowStartScreen();
    }

    private void SetupUIForDevice()
    {
        bool isMobile = DeviceDetector.IsMobile();

        if (isMobile)
        {
            SetupMobileUI();
        }
        else
        {
            SetupDesktopUI();
        }
    }

    private void SetupMobileUI()
    {
        if (canvasScaler != null)
        {
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1080f, 1920f);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 0f;
        }

        if (mainCanvas != null)
        {
            mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        if (desktopPanel != null) desktopPanel.SetActive(false);
        if (mobilePanel != null) mobilePanel.SetActive(true);

        _activePanel = mobilePanel;
        _activeScoreText = mobileScoreText;
        _activeCoinText = mobileCoinText;
        _activeSpeedText = mobileSpeedText;

        ApplySafeArea();
        ScaleMobileButtons();
    }

    private void SetupDesktopUI()
    {
        if (canvasScaler != null)
        {
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 1f;
        }

        if (mainCanvas != null)
        {
            mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        if (desktopPanel != null) desktopPanel.SetActive(true);
        if (mobilePanel != null) mobilePanel.SetActive(false);

        _activePanel = desktopPanel;
        _activeScoreText = desktopScoreText;
        _activeCoinText = desktopCoinText;
        _activeSpeedText = desktopSpeedText;
    }

    private void ApplySafeArea()
    {
        RectTransform panelRect = mobilePanel?.GetComponent<RectTransform>();
        if (panelRect == null) return;

        Rect safeArea = Screen.safeArea;
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        panelRect.anchorMin = anchorMin;
        panelRect.anchorMax = anchorMax;
    }

    private void ScaleMobileButtons()
    {
        Button[] buttons = mobilePanel?.GetComponentsInChildren<Button>(true);
        if (buttons == null) return;

        float scaleFactor = DeviceDetector.GetPixelRatio();
        foreach (Button btn in buttons)
        {
            RectTransform rect = btn.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.localScale = Vector3.one * Mathf.Max(1f, scaleFactor * 0.8f);
            }
        }
    }

    public void UpdateScore(int score)
    {
        if (_activeScoreText != null)
            _activeScoreText.text = $"Score: {score}";
    }

    public void UpdateCoins(int coins)
    {
        if (_activeCoinText != null)
            _activeCoinText.text = $"Coins: {coins}";
    }

    public void UpdateSpeed(float speed)
    {
        if (_activeSpeedText != null)
            _activeSpeedText.text = $"Speed: {speed:F1}";
    }

    public void ShowStartScreen()
    {
        if (startScreen != null) startScreen.SetActive(true);
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
    }

    public void ShowGameOverScreen(int score, int highScore)
    {
        if (gameOverScreen != null) gameOverScreen.SetActive(true);
        if (finalScoreText != null) finalScoreText.text = $"Final Score: {score}";
        if (highScoreText != null) highScoreText.text = $"High Score: {highScore}";
    }

    public void HideAllScreens()
    {
        if (startScreen != null) startScreen.SetActive(false);
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
    }

    private void OnStartClicked()
    {
        GameManager.Instance?.StartGame();
    }

    private void OnRestartClicked()
    {
        GameManager.Instance?.RestartGame();
    }
}
