using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class rWeapon
{
    public string rWeaponName;
    public GameObject rWeaponPrefab;
    public bool setDefault;
}

[System.Serializable]
public class lWeapon
{
    public string lWeaponName;
    public GameObject lWeaponPrefab;
    public bool setDefault;
}

[System.Serializable]
public class Helmet
{
    public string helmetName;
    public GameObject helmetPrefab;
    public bool setDefault;
}

[System.Serializable]
public class Chestplate
{
    public string chestplateName;
    public GameObject handPrefab;
    public GameObject chestplatePrefab;
    public bool setDefault;
}

[System.Serializable]
public class Leggings
{
    public string leggingsName;
    public GameObject leggingsPrefab;
    public bool setDefault;
}

[System.Serializable]
public class Boots
{
    public string bootsName;
    public GameObject bootsPrefab;
    public bool setDefault;
}

[System.Serializable]
public class Cape
{
    public string capeName;
    public GameObject capePrefab;
    public bool setDefault;
}

public class EquipController : MonoBehaviour
{
    public static int weapon = 0;

    private int helmetArmor, chestplateArmor, leggingsArmor, bootsArmor, capeArmor = 0;

    public rWeapon[] rWeapon;
    public lWeapon[] lWeapon;

    [SerializeField] private Helmet[] helmet;
    [SerializeField] private Chestplate[] chestplate;
    [SerializeField] private Leggings[] leggings;
    [SerializeField] private Boots[] boots;
    [SerializeField] private Cape[] cape;

    private StatusController StatusController;
    private InventoryController InventoryController;

    private const int Hand = 0, Weapon = 1, Bow = 2, Rig = 3;

    public void Start()
    {
        StatusController = FindObjectOfType<StatusController>();
        InventoryController = FindObjectOfType<InventoryController>();

        for (int i = 0; i < InventoryController.gSlots.Length; i++)
        {
            if (InventoryController.gSlots[i].item != null)
            {
                choiceEquip(InventoryController.gSlots[i].item, true);
            }
        }
    }

    public void choiceEquip(Item item, bool s)
    {
        try
        {
            if (item.itemType.Equals(Item.ItemType.Weapon))
            {
                if (s)
                {
                    for (int i = 0; i < rWeapon.Length; i++)
                    {
                        if (item.itemName.Equals(rWeapon[i].rWeaponName))
                        {
                            rWeapon[i].rWeaponPrefab.SetActive(s);
                            if (item.itemName.Contains("곡괭이") || item.itemName.Contains("도끼") || item.itemName.Contains("망치"))
                            {
                                weapon = Rig;
                            }
                            else
                            {
                                weapon = Weapon;
                            }
                        }
                        if (rWeapon[i].setDefault)
                        {
                            rWeapon[i].rWeaponPrefab.SetActive(!s);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < rWeapon.Length; i++)
                    {
                        if (item.itemName.Equals(rWeapon[i].rWeaponName))
                        {
                            rWeapon[i].rWeaponPrefab.SetActive(s);
                        }
                        if (rWeapon[i].setDefault)
                        {
                            rWeapon[i].rWeaponPrefab.SetActive(!s);
                            weapon = Hand;
                        }
                    }
                }
            }
            if (item.itemType.Equals(Item.ItemType.Shield))
            {

            }
            if (item.itemType.Equals(Item.ItemType.Bow))
            {

            }
            if (item.itemType.Equals(Item.ItemType.Helmet))
            {
                if (s)
                {
                    for (int i = 0; i < helmet.Length; i++)
                    {
                        if (item.itemName.Equals(helmet[i].helmetName))
                        {
                            helmet[i].helmetPrefab.SetActive(s);
                            helmetArmor = helmet[i].helmetPrefab.GetComponent<Armor>().armor;
                        }
                        if (helmet[i].setDefault)
                        {
                            helmet[i].helmetPrefab.SetActive(!s);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < helmet.Length; i++)
                    {
                        if (item.itemName.Equals(helmet[i].helmetName))
                        {
                            helmet[i].helmetPrefab.SetActive(s);
                        }
                        if (helmet[i].setDefault)
                        {
                            helmet[i].helmetPrefab.SetActive(!s);
                            helmetArmor = 0;
                        }
                    }
                }
            }
            if (item.itemType.Equals(Item.ItemType.Chestplate))
            {
                if (s)
                {
                    for (int i = 0; i < chestplate.Length; i++)
                    {
                        if (item.itemName.Equals(chestplate[i].chestplateName))
                        {
                            chestplate[i].chestplatePrefab.SetActive(s);
                            chestplate[i].handPrefab.SetActive(s);
                            chestplateArmor = chestplate[i].chestplatePrefab.GetComponent<Armor>().armor;
                        }
                        if (chestplate[i].setDefault)
                        {
                            chestplate[i].chestplatePrefab.SetActive(!s);
                            chestplate[i].handPrefab.SetActive(!s);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < chestplate.Length; i++)
                    {
                        if (item.itemName.Equals(chestplate[i].chestplateName))
                        {
                            chestplate[i].chestplatePrefab.SetActive(s);
                            chestplate[i].handPrefab.SetActive(s);
                        }
                        if (chestplate[i].setDefault)
                        {
                            chestplate[i].chestplatePrefab.SetActive(!s);
                            chestplate[i].handPrefab.SetActive(!s);
                            chestplateArmor = 0;
                        }
                    }
                }
            }
            if (item.itemType.Equals(Item.ItemType.Leggings))
            {
                if (s)
                {
                    for (int i = 0; i < leggings.Length; i++)
                    {
                        if (item.itemName.Equals(leggings[i].leggingsName))
                        {
                            leggings[i].leggingsPrefab.SetActive(s);
                            leggingsArmor = leggings[i].leggingsPrefab.GetComponent<Armor>().armor;
                        }
                        if (leggings[i].setDefault)
                        {
                            leggings[i].leggingsPrefab.SetActive(!s);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < leggings.Length; i++)
                    {
                        if (item.itemName.Equals(leggings[i].leggingsName))
                        {
                            leggings[i].leggingsPrefab.SetActive(s);
                        }
                        if (leggings[i].setDefault)
                        {
                            leggings[i].leggingsPrefab.SetActive(!s);
                            leggingsArmor = 0;
                        }
                    }
                }
            }
            if (item.itemType.Equals(Item.ItemType.Boots))
            {
                if (s)
                {
                    for (int i = 0; i < boots.Length; i++)
                    {
                        if (item.itemName.Equals(boots[i].bootsName))
                        {
                            boots[i].bootsPrefab.SetActive(s);
                            bootsArmor = boots[i].bootsPrefab.GetComponent<Armor>().armor;
                        }
                        if (boots[i].setDefault)
                        {
                            boots[i].bootsPrefab.SetActive(!s);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < boots.Length; i++)
                    {
                        if (item.itemName.Equals(boots[i].bootsName))
                        {
                            boots[i].bootsPrefab.SetActive(s);
                        }
                        if (boots[i].setDefault)
                        {
                            boots[i].bootsPrefab.SetActive(!s);
                            bootsArmor = 0;
                        }
                    }
                }
            }
            if (item.itemType.Equals(Item.ItemType.Cape))
            {
                if (s)
                {
                    for (int i = 0; i < cape.Length; i++)
                    {
                        if (item.itemName.Equals(cape[i].capeName))
                        {
                            cape[i].capePrefab.SetActive(s);
                            capeArmor = cape[i].capePrefab.GetComponent<Armor>().armor;
                        }
                        if (cape[i].setDefault)
                        {
                            cape[i].capePrefab.SetActive(!s);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < cape.Length; i++)
                    {
                        if (item.itemName.Equals(cape[i].capeName))
                        {
                            cape[i].capePrefab.SetActive(s);
                        }
                        if (cape[i].setDefault)
                        {
                            cape[i].capePrefab.SetActive(!s);
                            capeArmor = 0;
                        }
                    }
                }
            }
        }
        catch (NullReferenceException)
        {

        }

        StatusController.armor = helmetArmor + chestplateArmor + leggingsArmor + bootsArmor + capeArmor;
    }
}
