using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public int maxFrameRate = 200;
    public static bool IsPaused;
    public GameObject pauseMenuUI;
    public GameObject hud;
    public GameObject hudControl;
    public GameObject quest;
    public GameObject abilities;
    public GameObject resources;
    public GameObject dialogueWindow;
    public GameObject potionUI;
    public GameObject shop;
    public AudioSource audioSource;

    public static bool IsInputEnabled = true;
    private bool hasDialogue = false;
    void Start() {
        pauseMenuUI.SetActive(false);
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        IsPaused = false;
        Application.targetFrameRate = maxFrameRate;

    }
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape) || (shop.activeSelf && IsPaused == false)){
            if(IsPaused){
                Resume();
            }else{
                audioSource.Stop();
                Pause();
            }
        }

        if(Input.GetKeyDown(KeyCode.F1) && IsInputEnabled){
            hud.SetActive(!hud.activeSelf);
            hudControl.SetActive(!hud.activeSelf);
        }
        if(Input.GetKeyDown(KeyCode.Tab) && IsInputEnabled){
            quest.SetActive(!quest.activeSelf);
        }
    }
    public void disableInput(){
        IsInputEnabled = false;
    }
    public void EnableInput(){
        IsInputEnabled = true;
    }
    public void Resume (){
        IsInputEnabled = true;
        pauseMenuUI.SetActive(false);
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        IsPaused = false;
        Debug.Log("Continue");
        abilities.SetActive(true);
        hudControl.SetActive(true);
        resources.SetActive(true);
        potionUI.SetActive(true);
        if(hasDialogue){
            dialogueWindow.SetActive(true);
        }

    }

    public void Pause(){
        IsInputEnabled = false;
        pauseMenuUI.SetActive(true);
        // unLock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        IsPaused = true;
        abilities.SetActive(false);
        hudControl.SetActive(false);
        resources.SetActive(false);
        potionUI.SetActive(false);
        hasDialogue = dialogueWindow.activeSelf;
        dialogueWindow.SetActive(false);


    }

    public void LoadMenu(){
        Debug.Log("loading menu");
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }
    public void QuitGame(){

        Debug.Log("Quitting");
        Application.Quit();
    }

    public void tempPause(){
        
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
    public void tempResume(){
        IsInputEnabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
}
