using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    public int HP;
    [SerializeField] private int itemCount;
    [SerializeField] private float destroyTime;

    [SerializeField] private GameObject original;
    [SerializeField] private GameObject destruct;
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private GameObject dropItem;

    [SerializeField] private CapsuleCollider cCollider;

    private SaveAndLoadController SaveAndLoadController;

    [SerializeField] private string originalSound;
    [SerializeField] private string destructSound;

    private void Start()
    {
        SaveAndLoadController = FindObjectOfType<SaveAndLoadController>();
    }

    public void Gather(int damage)
    {
        SoundController.instance.PlaySE(originalSound);

        if (SaveAndLoadController.gatherObjects.Count == 0)
        {
            SaveAndLoadController.gatherObjects.Add(gameObject.transform.position);
            SaveAndLoadController.colHealth.Add(HP);
        }
        else
        {
            bool check = false;

            for (int i = 0; i < SaveAndLoadController.gatherObjects.Count; i++)
            {
                if (SaveAndLoadController.gatherObjects[i] == gameObject.transform.position)
                {
                    SaveAndLoadController.colHealth[i] = HP;
                    check = true;
                }
            }

            if (!check)
            {
                SaveAndLoadController.gatherObjects.Add(gameObject.transform.position);
                SaveAndLoadController.colHealth.Add(HP);
            }
        }

        var clone = Instantiate(effectPrefab, cCollider.bounds.center, Quaternion.identity);

        Destroy(clone, destroyTime);
        HP -= damage;

        if (HP <= 0)
        {
            Delete(name);
        }
    }

    private void Delete(string name)
    {
        SoundController.instance.PlaySE(destructSound);
        SaveAndLoadController.destroyObjects.Add(gameObject.transform.position);

        for (int i = 0; i < SaveAndLoadController.gatherObjects.Count; i++)
        {
            if (SaveAndLoadController.gatherObjects[i] == gameObject.transform.position)
            {
                SaveAndLoadController.gatherObjects.RemoveAt(i);
                SaveAndLoadController.colHealth.RemoveAt(i);
                break;
            }
        }

        cCollider.enabled = false;
        Destroy(original);

        for (int i = 0; i < itemCount; i++)
        {
            Instantiate(dropItem, original.transform.position, Quaternion.identity);
        }

        destruct.SetActive(true);
        Destroy(destruct, destroyTime);
    }
}
