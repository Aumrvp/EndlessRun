using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int coinCount { get; private set; }
    public float distance { get; private set; }

    public int bestDistance { get; private set; }
    public int bestCoins { get; private set; }
    public bool newDistanceRecord { get; private set; }
    public bool newCoinRecord { get; private set; }

    public float speedMultiplier = 1f;
    public bool speedBoostActive { get; private set; }
    public float speedBoostRemaining { get; private set; }

    private bool isRunning = true;
    private Coroutine speedBoostRoutine;

    private const string KEY_BEST_DISTANCE = "BestDistance";
    private const string KEY_BEST_COINS = "BestCoins";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        bestDistance = PlayerPrefs.GetInt(KEY_BEST_DISTANCE, 0);
        bestCoins = PlayerPrefs.GetInt(KEY_BEST_COINS, 0);
    }

    void Update()
    {
        if (!isRunning) return;
        distance += Time.deltaTime * 10f * speedMultiplier;
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
    }

    public void StartSpeedBoost(float multiplier, float duration)
    {
        if (speedBoostRoutine != null) StopCoroutine(speedBoostRoutine);
        speedBoostRoutine = StartCoroutine(SpeedBoostRoutine(multiplier, duration));
    }

    System.Collections.IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        speedMultiplier = multiplier;
        speedBoostActive = true;
        speedBoostRemaining = duration;

        while (speedBoostRemaining > 0f)
        {
            speedBoostRemaining -= Time.deltaTime;
            yield return null;
        }

        speedMultiplier = 1f;
        speedBoostActive = false;
        speedBoostRemaining = 0f;
    }

    public void StopRun()
    {
        if (!isRunning) return;
        isRunning = false;

        int finalDistance = Mathf.FloorToInt(distance);

        if (finalDistance > bestDistance)
        {
            bestDistance = finalDistance;
            PlayerPrefs.SetInt(KEY_BEST_DISTANCE, bestDistance);
            newDistanceRecord = true;
        }

        if (coinCount > bestCoins)
        {
            bestCoins = coinCount;
            PlayerPrefs.SetInt(KEY_BEST_COINS, bestCoins);
            newCoinRecord = true;
        }

        PlayerPrefs.Save();
    }
}
