using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private GameObject tooltip;

    [SerializeField] private Text Logo;
    [SerializeField] private Text Print;
    [SerializeField] private Text Outline;
    private bool tip = false;

    private void Update()
    {
        if (InventoryController.gearActive)
        {
            tip = false;
        }
        if (!InventoryController.gearActive && !tip)
        {
            tip = true;
            HideTooltip();
        }
        if (!InventoryController.inventoryActive && !InventoryController.gearActive)
        {
            HideTooltip();
        }
    }

    public void ShowTooltip(Item item, Vector3 pos)
    {
        tooltip.SetActive(true);
        pos += new Vector3(tooltip.GetComponent<RectTransform>().rect.width * 1.5f,
            tooltip.GetComponent<RectTransform>().rect.height * 1.5f, 0f);
        tooltip.transform.position = pos;
        Logo.text = item.itemName;
        Print.text = item.itemDesc;

        if (item.itemType == Item.ItemType.Armor)
        {
            Outline.text = "장비(탄약)";
        }
        else if (item.itemType == Item.ItemType.Helmet)
        {
            Outline.text = "장비(머리)";
        }
        else if (item.itemType == Item.ItemType.Chestplate)
        {
            Outline.text = "장비(상의)";
        }
        else if (item.itemType == Item.ItemType.Leggings)
        {
            Outline.text = "장비(하의)";
        }
        else if (item.itemType == Item.ItemType.Boots)
        {
            Outline.text = "장비(신발)";
        }
        else if (item.itemType == Item.ItemType.Cape)
        {
            Outline.text = "장비(망토)";
        }
        else if (item.itemType == Item.ItemType.Weapon)
        {
            Outline.text = "장비(무기)";
        }
        else if (item.itemType == Item.ItemType.Shield)
        {
            Outline.text = "장비(방패)";
        }
        else if (item.itemType == Item.ItemType.Usable)
        {
            Outline.text = "기타(소비)";
        }
        else if (item.itemType == Item.ItemType.Bow)
        {
            Outline.text = "장비(활)";
        }
        else if (item.itemType == Item.ItemType.Ingredient)
        {
            Outline.text = "기타(재료)";
        }
        else
        {
            Outline.text = "";
        }
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
