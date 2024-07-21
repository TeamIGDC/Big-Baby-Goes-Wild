using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Game.Entity;

public class GameManager : MonoBehaviour
{
    private Player player;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").GetComponent<Player>();

        if(player == null)
        {
            enabled = false;
        }

        if (loseScreen != null)
        {
            loseScreen.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        if (menuButton != null)
        {
            menuButton.onClick.AddListener(ReturnToMenu);
        }

        player.OnEntityStatusChange.AddListener(CheckPlayerHealth);
    }

    private void CheckPlayerHealth(Entity.EntityStatus status)
    {
        if (status == Entity.EntityStatus.Dead)
        {
            ShowLoseScreen();
        }
    }

    private void ShowLoseScreen()
    {
        if (loseScreen != null)
        {
            loseScreen.SetActive(true);
        }

    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart the current scene
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu"); // Load the menu scene (ensure you have a scene named "MenuScene")
    }

    private void OnDestroy()
    {
        player.OnEntityStatusChange.RemoveListener(CheckPlayerHealth);
    }
}