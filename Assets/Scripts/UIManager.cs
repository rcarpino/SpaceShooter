using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    
    [SerializeField]
    private Image _LivesImg;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _restartText;

    private bool _gameIsOver = false;

    // Start is called before the first frame update
    void Start()
    {
        
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(_gameIsOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];
        
        if(currentLives == 0)
        {
            GameOverSequence();     
        }
        
    }

    void GameOverSequence()
    {
        _gameIsOver = true;
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
        
    }

}
