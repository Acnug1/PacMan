using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(Restart);
        _exitButton.onClick.AddListener(Exit);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(Restart);
        _exitButton.onClick.RemoveListener(Exit);
    }

    private void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    private void Exit()
    {
        Application.Quit();
    }
}
