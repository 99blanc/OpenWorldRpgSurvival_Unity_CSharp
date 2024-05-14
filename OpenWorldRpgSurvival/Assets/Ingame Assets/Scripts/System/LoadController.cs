using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadController : MonoBehaviour
{
    public string ingameName = "Ingame";

    [SerializeField] private Slider slider;

    private AsyncOperation operation;

    private void Start()
    {
        StartCoroutine(LoadCoroutine());
    }

    private IEnumerator LoadCoroutine()
    {
        operation = SceneManager.LoadSceneAsync(ingameName);
        operation.allowSceneActivation = false;

        float timer = 0f;

        while (!operation.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (operation.progress < 0.9f)
            {
                slider.value = Mathf.Lerp(operation.progress, 1f, timer);

                if (slider.value >= operation.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                slider.value = Mathf.Lerp(slider.value, 1f, timer);

                if (slider.value >= 0.99f)
                {
                    operation.allowSceneActivation = true;
                }
            }
        }

        Debug.Log("Load Setting : " + SaveAndLoadController.instance.button);
    }
}
