using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsPlaying { get; private set; }
    public int Score { get; private set; }
    public int Coins { get; private set; }
    public int HighScore { get; private set; }

    [Header("Score Settings")]
    [SerializeField] private float scoreMultiplier = 10f;

    private PlayerController _playerController;
    private float _startTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        LoadHighScore();
        IsPlaying = false;
    }

    private void Update()
    {
        if (IsPlaying)
        {
            UpdateScore();
        }
    }

    public void StartGame()
    {
        IsPlaying = true;
        Score = 0;
        Coins = 0;
        _startTime = Time.time;

        UIManager.Instance?.HideAllScreens();
        UIManager.Instance?.UpdateScore(0);
        UIManager.Instance?.UpdateCoins(0);

        _playerController?.ResetPlayer();
        TrackManager.Instance?.ResetTrack();
    }

    public void GameOver()
    {
        if (!IsPlaying) return;
        IsPlaying = false;

        if (Score > HighScore)
        {
            HighScore = Score;
            SaveHighScore();
        }

        UIManager.Instance?.ShowGameOverScreen(Score, HighScore);
    }

    public void RestartGame()
    {
        StartGame();
    }

    public void AddCoin()
    {
        Coins++;
        UIManager.Instance?.UpdateCoins(Coins);
    }

    private void UpdateScore()
    {
        if (_playerController == null) return;
        float distance = _playerController.transform.position.z;
        Score = Mathf.FloorToInt(distance * scoreMultiplier);
        UIManager.Instance?.UpdateScore(Score);
        UIManager.Instance?.UpdateSpeed(_playerController.CurrentSpeed);
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", HighScore);
        PlayerPrefs.Save();
    }

    private void LoadHighScore()
    {
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
    }
}
