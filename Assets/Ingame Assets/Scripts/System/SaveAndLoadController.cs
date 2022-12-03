using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData
{
    public Vector3 playerPos, playerRot, playerEuler, dayRot;
    public List<Vector3> destroyObjects = new List<Vector3>();
    public List<Vector3> gatherObjects = new List<Vector3>();

    public bool root, delay, boot, reload, dead, spawn;

    public int playerHealth, playerStamina, playerThirst, playerEat, playerRest;
    public List<int> colHealth = new List<int>();
    public float currentFogDensity;
    public float currentFarClip;

    public List<int> inventoryArrayNumber = new List<int>();
    public List<string> inventoryItemName = new List<string>();
    public List<int> inventoryItemNumber = new List<int>();

    public List<int> gearArrayNumber = new List<int>();
    public List<string> gearItemName = new List<string>();
    public List<int> gearItemNumber = new List<int>();

    public List<Vector3> colPosition = new List<Vector3>();
    public List<int> colRand = new List<int>();
    public List<float> colRotRand = new List<float>();

    public List<string> animalName = new List<string>();
    public List<Vector3> animalPosition = new List<Vector3>();

    public List<string> monsterName = new List<string>();
    public List<Vector3> monsterPosition = new List<Vector3>();

    public List<string> constName = new List<string>();
    public List<Vector3> constPosition = new List<Vector3>();
    public List<Vector3> constRotation = new List<Vector3>();
}

public class SaveAndLoadController : MonoBehaviour
{
    [HideInInspector] public static SaveAndLoadController instance;

    public bool button;
    private SaveData saveData = new SaveData();
    public List<Vector3> gatherObjects = new List<Vector3>();
    public List<int> colHealth = new List<int>();

    public string SAVE_DATA_DIRECTORY;
    public string SAVE_FILENAME = "/PlayerData.txt";

    public List<Vector3> destroyObjects = new List<Vector3>();

    public List<Vector3> colPosition = new List<Vector3>();
    public List<int> colRand = new List<int>();
    public List<float> colRotRand = new List<float>();

    public List<string> animalName = new List<string>();
    public List<Vector3> animalPosition = new List<Vector3>();

    public List<string> monsterName = new List<string>();
    public List<Vector3> monsterPosition = new List<Vector3>();

    public List<string> constName = new List<string>();
    public List<Vector3> constPosition = new List<Vector3>();
    public List<Vector3> constRotation = new List<Vector3>();

    private ThirdPersonController control;
    private Animator player;
    private Transform view;
    private InventoryController inventory;
    private StatusController status;
    private Day day;
    private Camera clip;

    #region singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    #endregion singleton

    private void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Save/";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
        }
    }

    public void SaveData()
    {
        try
        {
            control = FindObjectOfType<ThirdPersonController>();
            player = control.GetComponentInChildren<Animator>();
            view = FindObjectOfType<AudioListener>().transform.parent;
            inventory = FindObjectOfType<InventoryController>();
            status = FindObjectOfType<StatusController>();
            day = FindObjectOfType<Day>();
            clip = view.GetComponentInChildren<Camera>();

            saveData.playerPos = control.transform.position;
            saveData.playerEuler = player.transform.rotation.eulerAngles;
            saveData.playerRot = view.transform.rotation.eulerAngles;
            saveData.dayRot = day.transform.rotation.eulerAngles;

            saveData.playerHealth = status.setHealth;
            saveData.playerStamina = status.setStamina;
            saveData.playerThirst = status.setThirst;
            saveData.playerEat = status.setEat;
            saveData.playerRest = status.setRest;

            saveData.root = ThirdPersonController.root;
            saveData.delay = ThirdPersonController.delay;
            saveData.boot = ThirdPersonController.boot;
            saveData.reload = ThirdPersonController.reload;
            saveData.dead = ThirdPersonController.dead;
            saveData.spawn = ThirdPersonController.spawn;

            saveData.currentFogDensity = RenderSettings.fogDensity;
            saveData.currentFarClip = clip.farClipPlane;

            saveData.destroyObjects = destroyObjects;
            saveData.gatherObjects = gatherObjects;
            saveData.colPosition = colPosition;
            saveData.colRand = colRand;
            saveData.colRotRand = colRotRand;
            saveData.colHealth = colHealth;

            saveData.constName = constName;
            saveData.constPosition = constPosition;
            saveData.constRotation = constRotation;

            saveData.animalName = animalName;
            saveData.animalPosition = animalPosition;

            saveData.monsterName = monsterName;
            saveData.monsterPosition = monsterPosition;

            saveData.inventoryArrayNumber.Clear();
            saveData.inventoryItemName.Clear();
            saveData.inventoryItemNumber.Clear();

            saveData.gearArrayNumber.Clear();
            saveData.gearItemName.Clear();
            saveData.gearItemNumber.Clear();

            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.slots[i].item != null)
                {
                    saveData.inventoryArrayNumber.Add(i);
                    saveData.inventoryItemName.Add(inventory.slots[i].item.itemName);
                    saveData.inventoryItemNumber.Add(inventory.slots[i].itemCount);
                }
            }
            for (int i = 0; i < inventory.gSlots.Length; i++)
            {
                if (inventory.gSlots[i].item != null)
                {
                    saveData.gearArrayNumber.Add(i);
                    saveData.gearItemName.Add(inventory.gSlots[i].item.itemName);
                    saveData.gearItemNumber.Add(inventory.gSlots[i].itemCount);
                }
            }

            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);
            button = true;

            Debug.Log("Save");
            Debug.Log(json);
        }
        catch (NullReferenceException)
        {

        }
    }

    public void LoadData()
    {
        try
        {
            if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
            {
                string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
                saveData = JsonUtility.FromJson<SaveData>(loadJson);
                button = true;

                control = FindObjectOfType<ThirdPersonController>();
                player = control.GetComponentInChildren<Animator>();
                view = FindObjectOfType<AudioListener>().transform.parent;
                inventory = FindObjectOfType<InventoryController>();
                status = FindObjectOfType<StatusController>();
                day = FindObjectOfType<Day>();
                clip = view.GetComponentInChildren<Camera>();

                control.transform.position = saveData.playerPos;
                player.transform.eulerAngles = saveData.playerEuler;
                view.transform.eulerAngles = saveData.playerRot;
                day.transform.eulerAngles = saveData.dayRot;

                status.setHealth = saveData.playerHealth;
                status.setStamina = saveData.playerStamina;
                status.setThirst = saveData.playerThirst;
                status.setEat = saveData.playerEat;
                status.setRest = saveData.playerRest;

                ThirdPersonController.root = saveData.root;
                ThirdPersonController.delay = saveData.delay;
                ThirdPersonController.boot = saveData.boot;
                ThirdPersonController.reload = saveData.reload;
                ThirdPersonController.dead = saveData.dead;
                ThirdPersonController.spawn = saveData.spawn;

                RenderSettings.fogDensity = saveData.currentFogDensity;
                clip.farClipPlane = saveData.currentFarClip;

                destroyObjects = saveData.destroyObjects;
                gatherObjects = saveData.gatherObjects;
                colPosition = saveData.colPosition;
                colRand = saveData.colRand;
                colRotRand = saveData.colRotRand;
                colHealth = saveData.colHealth;

                constName = saveData.constName;
                constPosition = saveData.constPosition;
                constRotation = saveData.constRotation;

                animalName = saveData.animalName;
                animalPosition = saveData.animalPosition;

                monsterName = saveData.monsterName;
                monsterPosition = saveData.monsterPosition;

                for (int i = 0; i < inventory.slots.Length; i++)
                {
                    inventory.slots[i].ClearSlot();
                }
                for (int i = 0; i < inventory.gSlots.Length; i++)
                {
                    inventory.gSlots[i].ClearSlot();
                }

                for (int i = 0; i < saveData.inventoryItemName.Count; i++)
                {
                    inventory.LoadInventory(saveData.inventoryArrayNumber[i], saveData.inventoryItemName[i], saveData.inventoryItemNumber[i]);
                }

                for (int i = 0; i < saveData.gearItemName.Count; i++)
                {
                    inventory.LoadGear(saveData.gearArrayNumber[i], saveData.gearItemName[i], saveData.gearItemNumber[i]);
                }
                Debug.Log("Inventory Save Slots : " + saveData.inventoryItemName.Count);
                Debug.Log("Gear Save Slots : " + saveData.gearItemName.Count);
                Debug.Log("Load");
                Debug.Log(loadJson);
            }
            else
            {
                button = false;
                Debug.Log("Load Error");
            }
        }
        catch (NullReferenceException)
        {

        }
    }
}
