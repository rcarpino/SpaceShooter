using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;

    public void Update()
    {
        if (_isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1); //Current Game Scene
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }


}
