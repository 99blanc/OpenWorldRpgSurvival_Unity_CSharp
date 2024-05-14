using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> colObjects;
    [SerializeField] private float rotRange;

    private GameObject empty;
    private Terrain terrain;
    private TerrainData terrainData;

    private SaveAndLoadController SaveAndLoadController;

    private void Start()
    {
        empty = new GameObject("Collections");
        terrain = GetComponentInChildren<Terrain>();
        terrainData = terrain.terrainData;
        SaveAndLoadController = FindObjectOfType<SaveAndLoadController>();

        if (SaveAndLoadController.instance.button)
        {
            {
                foreach (var terrainObj in terrainData.treeInstances)
                {
                    Vector3 _worldObjPos = Vector3.Scale(terrainObj.position, terrainData.size) + Terrain.activeTerrain.transform.position;

                    for (int i = 0; i < SaveAndLoadController.colPosition.Count; i++)
                    {
                        if (_worldObjPos == SaveAndLoadController.colPosition[i])
                        {
                            bool check = false;

                            for (int j = 0; j < SaveAndLoadController.destroyObjects.Count; j++)
                            {
                                if (SaveAndLoadController.colPosition[i] == SaveAndLoadController.destroyObjects[j])
                                {
                                    check = true;
                                    break;
                                }
                            }
                            if (!check)
                            {
                                GameObject tree = Instantiate(colObjects[SaveAndLoadController.colRand[i]], SaveAndLoadController.colPosition[i], Quaternion.identity, empty.transform);
                                tree.transform.rotation = Quaternion.AngleAxis(SaveAndLoadController.colRotRand[i], Vector3.up);

                                for (int j = 0; j < SaveAndLoadController.gatherObjects.Count; j++)
                                {
                                    if (SaveAndLoadController.colPosition[i] == SaveAndLoadController.gatherObjects[j])
                                    {
                                        tree.GetComponent<Collection>().HP = SaveAndLoadController.colHealth[j];
                                        break;
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
            }
        }
        else
        {
            foreach (var terrainObj in terrainData.treeInstances)
            {
                Vector3 _worldObjPos = Vector3.Scale(terrainObj.position, terrainData.size) + Terrain.activeTerrain.transform.position;
                int _rand = Random.Range(0, colObjects.Count);
                float _rotRand = Random.Range(-rotRange, rotRange);

                GameObject tree = Instantiate(colObjects[_rand], _worldObjPos, Quaternion.identity, empty.transform);
                tree.transform.rotation = Quaternion.AngleAxis(_rotRand, Vector3.up);

                SaveAndLoadController.colPosition.Add(_worldObjPos);
                SaveAndLoadController.colRand.Add(_rand);
                SaveAndLoadController.colRotRand.Add(_rand);
            }
        }

        Debug.Log("Render Setting : " + SaveAndLoadController.instance.button);

        terrain.treeDistance = 0;
    }
}
