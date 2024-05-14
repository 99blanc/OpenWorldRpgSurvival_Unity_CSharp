using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private int random;

    [SerializeField] private int randomRange;
    [SerializeField] private GameObject WaterItem;

    public void GetWater(Vector3 pos)
    {
        var _pos = new Vector3(pos.x, pos.y, pos.z);
        random = Random.Range(0, 100);

        if (random <= randomRange)
        {
            Instantiate(WaterItem, _pos, Quaternion.identity);
        }
    }
}
