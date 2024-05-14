using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string ingameName = "Ingame";
    public string loadName = "Load";

    [SerializeField] private GameObject infoUI;

    private SaveAndLoadController SaveAndLoadController;

    public void ClickStart()
    {
        SceneManager.LoadScene(loadName);
        SaveAndLoadController.instance.button = false;
        ThirdPersonController.root = false;
        Cursor.lockState = CursorLockMode.Locked;
        ThirdPersonController.pause = false;
    }

    public void ClickLoad()
    {
        SceneManager.LoadScene(loadName);
        SaveAndLoadController.instance.button = true;
        ThirdPersonController.root = false;
        Cursor.lockState = CursorLockMode.Locked;
        ThirdPersonController.pause = false;
    }

    public void ClickInfo()
    {
        if (!infoUI.activeSelf)
        {
            infoUI.SetActive(true);
        }
        else
        {
            infoUI.SetActive(false);
        }
    }

    public void ClickQuit()
    {
        Application.Quit();
    }
}
