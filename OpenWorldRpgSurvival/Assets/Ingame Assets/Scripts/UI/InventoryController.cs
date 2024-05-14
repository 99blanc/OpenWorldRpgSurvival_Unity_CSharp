using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector] public static InventoryController instance;

    public static bool inventoryActive = false;
    public static bool gearActive = false;

    [SerializeField] private GameObject inventoryImage;
    [SerializeField] private GameObject gearImage;
    [SerializeField] private GameObject gridSlot;
    [SerializeField] private GameObject gridGearSlot;
    [SerializeField] private Item[] items;
    [SerializeField] private string clickSound;

    private ThirdPersonController ThirdPersonController;
    private EquipController EquipController;
    public Slot[] slots;
    public Slot[] gSlots;

    private void Start()
    {
        slots = gridSlot.GetComponentsInChildren<Slot>();
        gSlots = gridGearSlot.GetComponentsInChildren<Slot>();
        ThirdPersonController = FindObjectOfType<ThirdPersonController>();
        EquipController = FindObjectOfType<EquipController>();

        Debug.Log("Inventory Slots : " + slots.Length);
        Debug.Log("Gear Slots : " + gSlots.Length);
    }

    private void Update()
    {
        Action();
        Cursor();
    }

    private void Cursor()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && (inventoryActive || gearActive))
        {
            SoundController.instance.PlaySE(clickSound);
        }
    }

    private void Action()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
        {
            inventoryImage.SetActive(false);
            gearImage.SetActive(false);
            inventoryActive = false;
            gearActive = false;
        }
        if ((Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.E)) && !BuildController.isPreviewActive)
        {
            if (!inventoryActive && !BuildController.buildActive)
            {
                inventoryImage.SetActive(true);
                gearImage.SetActive(true);
                inventoryActive = true;
                gearActive = true;
                ThirdPersonController.root = true;
            }
            else
            {
                inventoryImage.SetActive(false);
                gearImage.SetActive(false);
                inventoryActive = false;
                gearActive = false;
                ThirdPersonController.root = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!gearActive && !BuildController.buildActive)
            {
                gearImage.SetActive(true);
                gearActive = true;
                ThirdPersonController.root = true;
            }
            else
            {
                gearImage.SetActive(false);
                gearActive = false;
                if (inventoryActive)
                {
                    ThirdPersonController.root = true;
                }
                else
                {
                    ThirdPersonController.root = false;
                }
            }
        }
    }

    public bool FlowItem()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return true;
            }
        }

        return false;
    }

    public void AcquireItem(Item item, int count = 1)
    {
        if (Item.ItemType.Shield != item.itemType &&
            Item.ItemType.Weapon != item.itemType &&
            Item.ItemType.Helmet != item.itemType &&
            Item.ItemType.Chestplate != item.itemType &&
            Item.ItemType.Leggings != item.itemType &&
            Item.ItemType.Boots != item.itemType &&
            Item.ItemType.Cape != item.itemType &&
            Item.ItemType.Armor != item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == item.itemName)
                    {
                        slots[i].SetSlotCount(count);
                        return;
                    }
                }
            }
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(item, count);
                return;
            }
        }
        if (!FlowItem())
        {
            for (int i = 0; i < count; i++)
            {
                Instantiate(item.itemPrefab, ThirdPersonController.transform.position + ThirdPersonController.transform.forward + ThirdPersonController.transform.up, Quaternion.identity);
            }
        }
    }

    public void DropItem()
    {
        try
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].itemCount > 1)
                {
                    for (int j = 0; j < slots[i].itemCount; j++)
                    {
                        Instantiate(slots[i].item.itemPrefab, ThirdPersonController.transform.position + ThirdPersonController.transform.forward + ThirdPersonController.transform.up, Quaternion.identity);
                    }

                    slots[i].ClearSlot();
                }
                else
                {
                    Instantiate(slots[i].item.itemPrefab, ThirdPersonController.transform.position + ThirdPersonController.transform.forward + ThirdPersonController.transform.up, Quaternion.identity);
                    slots[i].ClearSlot();
                }
            }

            for (int i = 0; i < gSlots.Length; i++)
            {
                if (slots[i].itemCount > 1)
                {
                    for (int j = 0; j < gSlots[i].itemCount; j++)
                    {
                        Instantiate(gSlots[i].item.itemPrefab, ThirdPersonController.transform.position + ThirdPersonController.transform.forward + ThirdPersonController.transform.up, Quaternion.identity);
                    }

                    gSlots[i].ClearSlot();
                }
                else
                {
                    Instantiate(gSlots[i].item.itemPrefab, ThirdPersonController.transform.position + ThirdPersonController.transform.forward + ThirdPersonController.transform.up, Quaternion.identity);
                    gSlots[i].ClearSlot();
                }
            }
        }
        catch (NullReferenceException)
        {

        }
    }

    public int GetItemCount(string itemName)
    {
        int temp = SearchSlotItem(slots, itemName);

        return temp;
    }

    private int SearchSlotItem(Slot[] slots, string itemName)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                if (itemName == slots[i].item.itemName)
                {
                    return slots[i].itemCount;
                }
            }
        }

        return 0;
    }

    public void SetItemCount(string itemName, int itemCount)
    {
        if (!ItemCountAdjust(slots, itemName, itemCount))
        {
            return;
        }
    }

    private bool ItemCountAdjust(Slot[] slots, string itemName, int itemCount)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                if (itemName == slots[i].item.itemName)
                {
                    slots[i].SetSlotCount(-itemCount);
                    return true;
                }
            }
        }
        return false;
    }

    public void LoadInventory(int array, string itemName, int itemCount)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName == itemName)
            {
                slots[array].AddItem(items[i], itemCount);
            }
        }
    }

    public void LoadGear(int array, string itemName, int itemCount)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName == itemName)
            {
                gSlots[array].AddItem(items[i], itemCount);
            }
        }
    }

    public Slot[] GetSlots()
    {
        return slots;
    }
    public Slot[] GetGearSlots() 
    {
        return gSlots;
    }
}
