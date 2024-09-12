using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _winPanel; 
    private AudioManager _audioManager;

    void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
    }

    private bool _isGamePaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOptionsPanel();
        }
    }
    public void DisplayGameOverScreen()
    {
        if (_gameOverPanel != null)
        {
            Invoke("ShowGameOver", 1f);
        }
    }
    private void ShowGameOver()
    {
        Time.timeScale = 0;
        _gameOverPanel.SetActive(true);
        _audioManager.PlaySoundEffect("lose");
    }
    public void HideGameOverScreen()
    {
        if (_gameOverPanel != null)
        {
            _gameOverPanel.SetActive(false);
        }
    }
    public void DisplayWinScreen()
    {
        if (_winPanel != null)
        {
            Invoke("ShowWin", 1f);
        }
    }
    public void ShowWin()
    {
        Time.timeScale = 0;
        _winPanel.SetActive(true);
        _audioManager.PlaySoundEffect("win");
    }
    public void HideWinScreen()
    {
        if (_winPanel != null)
        {
            _winPanel.SetActive(false);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ToggleOptionsPanel()
    {
        if (_optionsPanel != null)
        {
            _isGamePaused = !_isGamePaused;

            if (_isGamePaused)
            {
                _optionsPanel.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                _optionsPanel.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public void CloseOptionsPanel()
    {
        if (_optionsPanel != null)
        {
            _optionsPanel.SetActive(false);
            _isGamePaused = false;
            Time.timeScale = 1;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
