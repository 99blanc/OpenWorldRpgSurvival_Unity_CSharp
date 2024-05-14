using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public string titleName = "Title";
    public string loadName = "Load";

    [SerializeField] private GameObject pauseUI;

    private SaveAndLoadController SaveAndLoadController;

    public void CallMenu()
    {
        pauseUI.SetActive(true);
    }

    public void CloseMenu()
    {
        pauseUI.SetActive(false);
    }

    public void ClickTitle()
    {
        if (ThirdPersonController.pause)
        {
            Time.timeScale = 1;
            ThirdPersonController.root = true;
            Cursor.lockState = CursorLockMode.None;
            ThirdPersonController.pause = false;
        }

        SoundController.instance.StopBackgroundSE();
        SceneManager.LoadScene(titleName);
    }
    public void ClickReturn()
    {
        if (ThirdPersonController.pause)
        {
            CloseMenu();
            Time.timeScale = 1;
            ThirdPersonController.root = false;
            Cursor.lockState = CursorLockMode.Locked;
            ThirdPersonController.pause = false;
        }
    }

    public void ClickSave()
    {
        if (ThirdPersonController.pause)
        {
            CloseMenu();
            Time.timeScale = 1;
            SaveAndLoadController.instance.button = true;
            ThirdPersonController.root = false;
            Cursor.lockState = CursorLockMode.None;
            ThirdPersonController.pause = false;
            SaveAndLoadController.instance.SaveData();
            SceneManager.LoadScene(loadName);
        }
    }

    public void ClickQuit()
    {
        Application.Quit();
    }
}
