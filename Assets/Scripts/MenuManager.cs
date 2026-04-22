using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject startPanel;
    public GameObject historyPanel;
    public GameObject optionsPanel;

    [Header("History Texts")]
    public TextMeshProUGUI historyDistanceText;
    public TextMeshProUGUI historyCoinText;

    private bool isSoundOn = true;
    private static bool skipStartScreen = false;

    void Start()
    {
        if (skipStartScreen)
        {
            skipStartScreen = false;
            startPanel.SetActive(false);
            historyPanel.SetActive(false);
            optionsPanel.SetActive(false);
            Time.timeScale = 1f;
            return;
        }
        ShowStartScreen();
    }

    
    public void ShowStartScreen()
    {
        Time.timeScale = 0f; 
        startPanel.SetActive(true);
        historyPanel.SetActive(false);
        optionsPanel.SetActive(false);
    }

    public void PlayGame()
    {
        Time.timeScale = 1f; 
        startPanel.SetActive(false);
    }

    
    public void ShowHistory()
    {
        startPanel.SetActive(false);
        historyPanel.SetActive(true);
        
        
        if (GameManager.Instance != null)
        {
            historyDistanceText.text = "Best Distance: " + GameManager.Instance.bestDistance + " m";
            historyCoinText.text = "Best Coins: " + GameManager.Instance.bestCoins;
        }
    }

    public void CloseHistory()
    {
        historyPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    
    public void ShowOptions()
    {
        startPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        AudioListener.volume = isSoundOn ? 1f : 0f; 
    }

    
    public void RestartGame()
    {
        skipStartScreen = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToLobby()
    {
        skipStartScreen = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}