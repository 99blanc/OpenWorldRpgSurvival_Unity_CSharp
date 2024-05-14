using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearObject : MonoBehaviour
{
    [SerializeField] private float clearDelay;

    private bool check = false;
    
    private void Start()
    {
        
    }
    
    private void Update()
    {
        Clear();
    }

    private void Clear()
    {
        var objects = GameObject.FindGameObjectsWithTag("Item");

        if (!check)
        {
            check = true;

            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].transform.parent == null)
                {
                    Destroy(objects[i]);
                }
            }

            StartCoroutine(LogicClear());
        }
    }

    private IEnumerator LogicClear()
    {
        yield return new WaitForSeconds(clearDelay);

        check = false;
    }
}
