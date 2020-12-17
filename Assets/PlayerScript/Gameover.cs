using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour
{
    public void EndGame(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("GameOver");
    }

    public void Restart(){
        SceneManager.LoadScene("Main");
    }

    public void LoadMenu(){
        SceneManager.LoadScene("StartMenu");
    }
    public void ContinueGame(){
		Player.LoadPlayer();
		SceneManager.LoadScene("Main");
			
	}

}
