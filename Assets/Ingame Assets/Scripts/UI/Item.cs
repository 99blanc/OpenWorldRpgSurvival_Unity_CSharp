using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    [TextArea] public string itemDesc;
    public ItemType itemType;
    public Sprite itemImage;
    public GameObject itemPrefab;
    public string WeaponType;

    public enum ItemType
    {
        Armor,
        Helmet,
        Chestplate,
        Leggings,
        Boots,
        Cape,
        Weapon,
        Bow,
        Shield,
        Usable,
        Ingredient,
    }
}
