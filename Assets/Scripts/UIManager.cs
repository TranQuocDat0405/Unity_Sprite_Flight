using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{

    [SerializeField] private UIDocument uiDocument;
    private Label scoreText;
    private Button restartButton;

    private Label bestScoreText;

    private int currentScore = 0;
    [Header("Hide Restart Button")]
    private bool isGameOver = false;
    void Start()
    {
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        bestScoreText = uiDocument.rootVisualElement.Q<Label>("BestScoreLabel");

        int bestScore = PlayerPrefs.GetInt("BestScore", 0); 
        bestScoreText.text = "Best : " + bestScore;

        restartButton.style.display = DisplayStyle.None;
        bestScoreText.style.display = DisplayStyle.None;

        restartButton.clicked += ReloadScene;

    }

    // Update is called once per frame
    private void OnEnable()
    {
        PlayerController.onScoreUpdated += UpdateScore;
        PlayerController.onPlayerDied += ShowGameOverUI;
    }
    private void OnDisable()
    {
        PlayerController.onScoreUpdated -= UpdateScore;
        PlayerController.onPlayerDied -= ShowGameOverUI;
    }

    private void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
        currentScore = score;
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void ShowGameOverUI()
    {
        isGameOver = true;
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
        }
        bestScoreText.style.display = DisplayStyle.Flex;
        restartButton.style.display = DisplayStyle.Flex;
        bestScoreText.text = "Best : " + bestScore;
    }
    public void HideRestartButton()
    {
        restartButton.style.display = DisplayStyle.None;
    }
    public void ShowRestartButton()
    {
        if (!isGameOver) return;
        restartButton.style.display = DisplayStyle.Flex;
    }

}
