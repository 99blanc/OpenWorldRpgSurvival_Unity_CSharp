using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construction : MonoBehaviour
{
    public bool setWorkbench;
    public bool setFurnace;
    public bool setBrewery;
    public bool setBonfire;
    public bool canDestroy;
    [HideInInspector] public Transform workbenchPos;
    [SerializeField] private string buildSound;

    private int check = 2;

    private SaveAndLoadController SaveAndLoadController;

    private void Start()
    {
        SaveAndLoadController = FindObjectOfType<SaveAndLoadController>();

        workbenchPos = transform;
    }

    public void SetDestroy()
    {
        SoundController.instance.PlaySE(buildSound);
        check--;

        if (check <= 0)
        {
            for (int i = 0; i < SaveAndLoadController.constPosition.Count; i++)
            {
                if (SaveAndLoadController.constPosition[i] == gameObject.transform.position)
                {
                    SaveAndLoadController.constName.RemoveAt(i);
                    SaveAndLoadController.constPosition.RemoveAt(i);
                    SaveAndLoadController.constRotation.RemoveAt(i);
                }
            }
            
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(DestroyCheck());
        }
    }

    private IEnumerator DestroyCheck()
    {
        yield return new WaitForSeconds(5f);

        check = 2;
    }
}
