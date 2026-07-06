using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public static TrackManager Instance { get; private set; }

    [Header("Track Settings")]
    [SerializeField] private int initialTrackCount = 5;
    [SerializeField] private float trackLength = 30f;
    [SerializeField] private float spawnDistance = 60f;
    [SerializeField] private float despawnDistance = 30f;

    [Header("Lane Settings")]
    [SerializeField] private float laneWidth = 3f;

    [Header("Prefabs (Set in Inspector)")]
    [SerializeField] private GameObject trackSegmentPrefab;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject trainPrefab;

    private List<GameObject> _activeSegments = new List<GameObject>();
    private List<GameObject> _activeObstacles = new List<GameObject>();
    private List<GameObject> _activeCoins = new List<GameObject>();
    private List<GameObject> _activeTrains = new List<GameObject>();

    private float _nextSpawnZ = 0f;
    private Transform _playerTransform;

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
        _playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (_playerTransform == null)
        {
            Debug.LogError("Player not found! Make sure player has 'Player' tag.");
            return;
        }

        InitializeTrack();
    }

    private void Update()
    {
        if (_playerTransform == null) return;

        float playerZ = _playerTransform.position.z;

        // Spawn new segments ahead
        while (_nextSpawnZ < playerZ + spawnDistance)
        {
            SpawnTrackSegment(_nextSpawnZ);
            _nextSpawnZ += trackLength;
        }

        // Recycle old segments behind
        RecycleOldSegments(playerZ);
    }

    private void InitializeTrack()
    {
        for (int i = 0; i < initialTrackCount; i++)
        {
            SpawnTrackSegment(_nextSpawnZ);
            _nextSpawnZ += trackLength;
        }
    }

    private void SpawnTrackSegment(float zPosition)
    {
        Vector3 spawnPos = new Vector3(0f, 0f, zPosition);
        GameObject segment = ObjectPool.Instance?.SpawnFromPool("TrackSegment", spawnPos, Quaternion.identity);

        if (segment == null)
        {
            segment = GameObject.Instantiate(trackSegmentPrefab, spawnPos, Quaternion.identity);
            segment.name = "TrackSegment_Fallback";
        }

        segment.transform.localScale = new Vector3(laneWidth * 3f + 2f, 1f, trackLength);
        _activeSegments.Add(segment);

        // Spawn obstacles and coins on this segment
        SpawnSegmentContent(zPosition);
    }

    private void SpawnSegmentContent(float segmentZ)
    {
        int obstacleCount = Random.Range(0, 3);
        for (int i = 0; i < obstacleCount; i++)
        {
            float z = segmentZ + Random.Range(5f, trackLength - 5f);
            int lane = Random.Range(0, 3);
            float x = (lane - 1) * laneWidth;

            Vector3 pos = new Vector3(x, 1f, z);
            GameObject obstacle = ObjectPool.Instance?.SpawnFromPool("Obstacle", pos, Quaternion.identity);
            if (obstacle == null)
            {
                obstacle = GameObject.Instantiate(obstaclePrefab, pos, Quaternion.identity);
            }
            obstacle.transform.localScale = new Vector3(2f, 2f, 2f);
            _activeObstacles.Add(obstacle);
        }

        int coinCount = Random.Range(3, 8);
        for (int i = 0; i < coinCount; i++)
        {
            float z = segmentZ + Random.Range(2f, trackLength - 2f);
            int lane = Random.Range(0, 3);
            float x = (lane - 1) * laneWidth;

            Vector3 pos = new Vector3(x, 1.5f, z);
            GameObject coin = ObjectPool.Instance?.SpawnFromPool("Coin", pos, Quaternion.identity);
            if (coin == null)
            {
                coin = GameObject.Instantiate(coinPrefab, pos, Quaternion.identity);
            }
            _activeCoins.Add(coin);
        }

        // 30% chance to spawn a train
        if (Random.value < 0.3f)
        {
            float z = segmentZ + Random.Range(5f, trackLength - 10f);
            int lane = Random.Range(0, 3);
            float x = (lane - 1) * laneWidth;

            Vector3 pos = new Vector3(x, 1.5f, z);
            GameObject train = ObjectPool.Instance?.SpawnFromPool("Train", pos, Quaternion.identity);
            if (train == null)
            {
                train = GameObject.Instantiate(trainPrefab, pos, Quaternion.identity);
            }
            train.transform.localScale = new Vector3(2.8f, 3f, 12f);
            _activeTrains.Add(train);
        }
    }

    private void RecycleOldSegments(float playerZ)
    {
        // Recycle segments
        for (int i = _activeSegments.Count - 1; i >= 0; i--)
        {
            if (_activeSegments[i] == null)
            {
                _activeSegments.RemoveAt(i);
                continue;
            }

            if (_activeSegments[i].transform.position.z < playerZ - despawnDistance)
            {
                ObjectPool.Instance?.ReturnToPool(_activeSegments[i]);
                _activeSegments.RemoveAt(i);
            }
        }

        // Recycle obstacles
        for (int i = _activeObstacles.Count - 1; i >= 0; i--)
        {
            if (_activeObstacles[i] == null)
            {
                _activeObstacles.RemoveAt(i);
                continue;
            }

            if (_activeObstacles[i].transform.position.z < playerZ - despawnDistance)
            {
                ObjectPool.Instance?.ReturnToPool(_activeObstacles[i]);
                _activeObstacles.RemoveAt(i);
            }
        }

        // Recycle coins
        for (int i = _activeCoins.Count - 1; i >= 0; i--)
        {
            if (_activeCoins[i] == null)
            {
                _activeCoins.RemoveAt(i);
                continue;
            }

            if (_activeCoins[i].transform.position.z < playerZ - despawnDistance)
            {
                ObjectPool.Instance?.ReturnToPool(_activeCoins[i]);
                _activeCoins.RemoveAt(i);
            }
        }

        // Recycle trains
        for (int i = _activeTrains.Count - 1; i >= 0; i--)
        {
            if (_activeTrains[i] == null)
            {
                _activeTrains.RemoveAt(i);
                continue;
            }

            if (_activeTrains[i].transform.position.z < playerZ - despawnDistance)
            {
                ObjectPool.Instance?.ReturnToPool(_activeTrains[i]);
                _activeTrains.RemoveAt(i);
            }
        }
    }

    public void ResetTrack()
    {
        // Return all active objects to pool
        foreach (var seg in _activeSegments)
        {
            if (seg != null) ObjectPool.Instance?.ReturnToPool(seg);
        }
        foreach (var obs in _activeObstacles)
        {
            if (obs != null) ObjectPool.Instance?.ReturnToPool(obs);
        }
        foreach (var coin in _activeCoins)
        {
            if (coin != null) ObjectPool.Instance?.ReturnToPool(coin);
        }
        foreach (var train in _activeTrains)
        {
            if (train != null) ObjectPool.Instance?.ReturnToPool(train);
        }

        _activeSegments.Clear();
        _activeObstacles.Clear();
        _activeCoins.Clear();
        _activeTrains.Clear();
        _nextSpawnZ = 0f;

        InitializeTrack();
    }
}
