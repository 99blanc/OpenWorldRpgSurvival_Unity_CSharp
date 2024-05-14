using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    private List<Collider> colliderList = new List<Collider>();

    [SerializeField] private int layerGround;

    [SerializeField] private Material green;
    [SerializeField] private Material red;

    private const int ignore_raycast_layer = 2;

    private void Start()
    {
        
    }

    private void Update()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        if (colliderList.Count > 0)
        {
            SetColor(red);
        }
        else
        {
            SetColor(green);
        }
    }

    private void SetColor(Material mat)
    {
        try
        {
            if (transform.childCount > 0)
            {
                foreach (Transform child in transform)
                {
                    Material[] newMaterials = new Material[child.GetComponent<MeshRenderer>().materials.Length];

                    for (int i = 0; i < newMaterials.Length; i++)
                    {
                        newMaterials[i] = mat;
                    }

                    child.GetComponent<MeshRenderer>().materials = newMaterials;
                }
            }
            else
            {
                Material[] newMaterials = new Material[GetComponent<MeshRenderer>().materials.Length];

                for (int i = 0; i < newMaterials.Length; i++)
                {
                    newMaterials[i] = mat;
                }

                GetComponent<MeshRenderer>().materials = newMaterials;
            }
        }
        catch (NullReferenceException)
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != ignore_raycast_layer)
        {
            colliderList.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != ignore_raycast_layer)
        {
            colliderList.Remove(other);
        }
    }

    public bool SetBuildable()
    {
        return colliderList.Count == 0;
    }
}
