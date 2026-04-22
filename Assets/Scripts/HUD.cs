using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI hpText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;

    public GameObject greenPotionGroup;
    public Slider greenPotionBar;
    public TextMeshProUGUI greenPotionTimeText;

    private PlayerController playerController;
    private bool gameOverShown;

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (greenPotionGroup != null) greenPotionGroup.SetActive(false);
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        distanceText.text = "Distance: " + Mathf.FloorToInt(GameManager.Instance.distance) + " m";
        coinText.text = "Coins: " + GameManager.Instance.coinCount;

        if (hpText != null)
        {
            string hearts = "";
            for (int i = 0; i < playerController.currentHP; i++) hearts += "<color=#FF5252>\u2665</color>";
            for (int i = playerController.currentHP; i < playerController.maxHP; i++) hearts += "<color=#555555>\u2665</color>";
            hpText.text = "HP: " + hearts;
        }

        UpdateGreenPotionBar();

        if (playerController.gameOver && !gameOverShown)
        {
            gameOverShown = true;
            ShowGameOver();
        }
    }

    void UpdateGreenPotionBar()
    {
        if (greenPotionGroup == null) return;

        var gm = GameManager.Instance;
        bool active = gm.speedBoostActive && gm.speedBoostDuration > 0f;

        if (greenPotionGroup.activeSelf != active) greenPotionGroup.SetActive(active);
        if (!active) return;

        float remaining = Mathf.Max(0f, gm.speedBoostRemaining);
        float ratio = Mathf.Clamp01(remaining / gm.speedBoostDuration);

        if (greenPotionBar != null) greenPotionBar.value = ratio;
        if (greenPotionTimeText != null) greenPotionTimeText.text = remaining.ToString("0.0") + "s";
    }

    void ShowGameOver()
    {
        if (gameOverPanel == null || gameOverText == null) return;

        gameOverPanel.SetActive(true);
        var gm = GameManager.Instance;

        int dist = Mathf.FloorToInt(gm.distance);
        string newTag = gm.newDistanceRecord ? "  <color=#FFD54A>NEW!</color>" : "";
        string newTagCoin = gm.newCoinRecord ? "  <color=#FFD54A>NEW!</color>" : "";

        gameOverText.text =
            "<b><color=#FF5252>GAME OVER</color></b>\n\n" +
            "<color=#FFFFFF><nobr>Distance: " + dist + "m" + newTag + "</nobr></color>\n" +
            "<color=#80D8FF>Best: " + gm.bestDistance + "m</color>\n\n" +
            "<color=#FFFFFF><nobr>Coins: " + gm.coinCount + newTagCoin + "</nobr></color>\n" +
            "<color=#80D8FF>Best: " + gm.bestCoins + "</color>";
    }
}
