using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnObject
{
    public string spawnName;
    public GameObject spawnPrefab;
    public bool spawnNight;
    public int maxCount;
    [HideInInspector] public int count = 0;
}

public class Spawner : MonoBehaviour
{
    public SpawnObject[] monster;
    public SpawnObject[] animal;

    [SerializeField] private float yOffset = 0.5f;
    [SerializeField] private float spawnDelay;

    private bool cancelSpawn = false;

    private float terrainWidth;
    private float terrainLength;

    private float _posX, _posZ, randX, randZ, yVal;

    public GameObject animals;
    public GameObject monsters;
    private Terrain terrain;
    private SaveAndLoadController SaveAndLoadController;

    private void Start()
    {
        animals = new GameObject("Animals");
        monsters = new GameObject("Monsters");
        terrain = GetComponentInChildren<Terrain>();
        SaveAndLoadController = FindObjectOfType<SaveAndLoadController>();

        terrainWidth = terrain.terrainData.size.x;
        terrainLength = terrain.terrainData.size.z;

        _posX = transform.position.x;
        _posZ = transform.position.z;

        if (SaveAndLoadController.instance.button)
        {
            for (int i = 0; i < SaveAndLoadController.animalName.Count; i++)
            {
                for (int j = 0; j < animal.Length; j++)
                {
                    if (SaveAndLoadController.animalName[i].Equals(animal[j].spawnName))
                    {
                        Instantiate(animal[j].spawnPrefab, SaveAndLoadController.animalPosition[i], Quaternion.identity, animals.transform);
                        animal[j].count++;
                    }
                }
            }

            for (int i = 0; i < SaveAndLoadController.monsterName.Count; i++)
            {
                for (int j = 0; j < monster.Length; j++)
                {
                    if (SaveAndLoadController.monsterName[i].Equals(monster[j].spawnName))
                    {
                        Instantiate(monster[j].spawnPrefab, SaveAndLoadController.monsterPosition[i], Quaternion.identity, monsters.transform);
                        monster[j].count++;
                    }
                }
            }
        }
    }

    private void Update()
    {
        CallSpawnObject();
    }

    public void CheckObject(string name)
    {
        for (int i = 0; i < animal.Length; i++)
        {
            if (animal[i].spawnName.Equals(name))
            {
                animal[i].count--;
            }
        }
        for (int i = 0; i < monster.Length; i++)
        {
            if (monster[i].spawnName.Equals(name))
            {
                monster[i].count--;
            }
        }
    }

    private void CallSpawnObject()
    {
        if (!cancelSpawn)
        {
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        try
        {
            if (!Day.isNight)
            {
                for (int i = 0; i < animal.Length; i++)
                {
                    if (!animal[i].spawnNight && animal[i].count < animal[i].maxCount)
                    {
                        RandomPos();
                        Instantiate(animal[i].spawnPrefab, new Vector3(randX, yVal, randZ), Quaternion.identity, animals.transform);
                        animal[i].count++;
                        SaveAndLoadController.animalName.Add(animal[i].spawnName);
                        SaveAndLoadController.animalPosition.Add(new Vector3(randX, yVal, randZ));
                    }
                }
                for (int i = 0; i < monster.Length; i++)
                {
                    if (!monster[i].spawnNight && monster[i].count < monster[i].maxCount)
                    {
                        RandomPos();
                        Instantiate(monster[i].spawnPrefab, new Vector3(randX, yVal, randZ), Quaternion.identity, monsters.transform);
                        monster[i].count++;
                        SaveAndLoadController.monsterName.Add(monster[i].spawnName);
                        SaveAndLoadController.monsterPosition.Add(new Vector3(randX, yVal, randZ));
                    }
                }
            }
            else
            {
                for (int i = 0; i < animal.Length; i++)
                {
                    if (animal[i].spawnNight && animal[i].count < animal[i].maxCount)
                    {
                        RandomPos();
                        Instantiate(animal[i].spawnPrefab, new Vector3(randX, yVal, randZ), Quaternion.identity, animals.transform);
                        animal[i].count++;
                        SaveAndLoadController.animalName.Add(animal[i].spawnName);
                        SaveAndLoadController.animalPosition.Add(new Vector3(randX, yVal, randZ));
                    }
                }
                for (int i = 0; i < monster.Length; i++)
                {
                    if (monster[i].spawnNight && monster[i].count < monster[i].maxCount)
                    {
                        RandomPos();
                        Instantiate(monster[i].spawnPrefab, new Vector3(randX, yVal, randZ), Quaternion.identity, monsters.transform);
                        monster[i].count++;
                        SaveAndLoadController.monsterName.Add(monster[i].spawnName);
                        SaveAndLoadController.monsterPosition.Add(new Vector3(randX, yVal, randZ));
                    }
                }
            }
        }
        catch (NullReferenceException)
        {

        }

        cancelSpawn = true;
        StartCoroutine(DelaySpawnObject());
    }

    private void RandomPos()
    {
        randX = UnityEngine.Random.Range(_posX, _posX + terrainWidth);
        randZ = UnityEngine.Random.Range(_posZ, _posZ + terrainLength);
        yVal = Terrain.activeTerrain.SampleHeight(new Vector3(randX, 0, randZ));
        yVal = yVal + yOffset;
    }

    private IEnumerator DelaySpawnObject()
    {
        yield return new WaitForSeconds(spawnDelay);

        cancelSpawn = false;
    }
}
