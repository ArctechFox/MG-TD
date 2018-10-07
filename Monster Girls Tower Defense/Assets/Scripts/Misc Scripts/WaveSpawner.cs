using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{

    public Text TextWaveCountdown;
    public Text TextCurrentWave;

    public Transform EnemyPrefab;
    public Transform SpawnPoint;

    public int NumberOfWaves = 3;
    public int CurrentWave = 0;

    public float TimeBetweenWaves = 3f;
    public float TimeBetweenEnemySpawns = 0.55f;
    public float EnemyCountMultiplierPerWave = 1.5f;
    public int EnemyCountToSpawn = 3;

    private float _countDown;
    private bool _ready = false;

    // Use this for initialization
    void Start()
    {
        TextWaveCountdown.text = "";
        TextCurrentWave.text = string.Format("Wave: {0}/{1}", CurrentWave, NumberOfWaves);

        ResetCountdown();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentWave < NumberOfWaves)
        {
            if (_ready)
            {
                TextWaveCountdown.enabled = true;
                _countDown -= Time.deltaTime;
                TextWaveCountdown.text = Mathf.Floor(_countDown + 1).ToString();

                if (_countDown <= 0)
                {
                    IncrementCurrentWave();

                    TextWaveCountdown.enabled = false;
                    _ready = false;

                    StartCoroutine(SpawnWave());
                    ResetCountdown();
                }
            }
        }
    }

    private void ResetCountdown()
    {
        _countDown = TimeBetweenWaves;
    }

    private void IncrementCurrentWave()
    {
        CurrentWave++;
        TextCurrentWave.text = string.Format("Wave: {0}/{1}", CurrentWave, NumberOfWaves);
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < EnemyCountToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(TimeBetweenEnemySpawns);
        }

        // TODO: Make this better?
        EnemyCountToSpawn = Mathf.CeilToInt(EnemyCountToSpawn * EnemyCountMultiplierPerWave);
        _ready = false;
    }

    private void SpawnEnemy()
    {
        Instantiate(EnemyPrefab, SpawnPoint.position, SpawnPoint.rotation);
    }

    public void StartWave()
    {
        _ready = true;
    }
}
