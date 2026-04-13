using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private UIManager uiManager;
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        uiManager.HideRestartButton();
    }
    public void Home()
    { 
        SceneManager.LoadScene("HomeMenu");
        Time.timeScale = 1f;
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        uiManager.ShowRestartButton();
    }
}
