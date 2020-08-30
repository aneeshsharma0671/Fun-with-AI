using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public void PlayAI()
    {
        SceneManager.LoadScene("flappyBirdAI");
    }
    public void Playmanually()
    {
        SceneManager.LoadScene("flappyBirdmanual");
    }
    public void GameExit()
    {
        Application.Quit();
    }
    public void Close()
    {
        SceneManager.LoadScene("intro");
    }

}
