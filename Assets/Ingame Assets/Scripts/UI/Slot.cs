using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item;
    public int itemCount;
    public Image itemImage;

    [SerializeField] private bool check = false;

    [SerializeField] private Text textCount;
    [SerializeField] private GameObject CountImage;

    [SerializeField] private Tooltip tooltipImage;

    [SerializeField] private GameObject baseUI;
    [SerializeField] private GameObject gearUI;

    private InventoryController InventoryController;
    private ItemController ItemController;
    private ThirdPersonController ThirdPersonController;
    private EquipController EquipController;

    private Rect baseRect, gearRect;

    private void Start()
    {
        baseRect = baseUI.GetComponent<RectTransform>().rect;
        gearRect = gearUI.GetComponent<RectTransform>().rect;
        InventoryController = GetComponentInParent<InventoryController>();
        ItemController = FindObjectOfType<ItemController>();
        ThirdPersonController = FindObjectOfType<ThirdPersonController>();
        EquipController = FindObjectOfType<EquipController>();
    }

    public void AddItem(Item i, int count = 1)
    {
        item = i;
        itemCount = count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != Item.ItemType.Shield &&
            item.itemType != Item.ItemType.Weapon &&
            item.itemType != Item.ItemType.Helmet &&
            item.itemType != Item.ItemType.Chestplate &&
            item.itemType != Item.ItemType.Leggings &&
            item.itemType != Item.ItemType.Boots &&
            item.itemType != Item.ItemType.Cape &&
            item.itemType != Item.ItemType.Armor)
        {
            CountImage.SetActive(true);
            textCount.text = itemCount.ToString();
        }
        else
        {
            textCount.text = "0";
            CountImage.SetActive(false);
        }

        SetColor(1);
    }

    private void SetColor(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    public void SetSlotCount(int count)
    {
        itemCount += count;
        textCount.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);
        textCount.text = "0";
        CountImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData e)
    {
        try
        {
            if (e.button == PointerEventData.InputButton.Right)
            {
                if (item != null)
                {
                    if (item.itemType == Item.ItemType.Usable)
                    {
                        ItemController.UseItem(item);
                        SetSlotCount(-1);
                    }
                    if (item.itemType == Item.ItemType.Shield)
                    {
                        if (GameObject.ReferenceEquals(this.gameObject, InventoryController.gSlots[0].gameObject))
                        {
                            if (InventoryController.FlowItem())
                            {
                                InventoryController.AcquireItem(item, itemCount);
                                EquipController.choiceEquip(item, false);
                                SetSlotCount(-1);
                                ClearSlot();
                            }
                        }
                        else
                        {
                            if (InventoryController.gSlots[0].item == null)
                            {
                                InventoryController.gSlots[0].AddItem(item, itemCount);
                                EquipController.choiceEquip(item, true);
                                SetSlotCount(-1);
                                ClearSlot();
                            }
                        }
                    }
                    else if (item.itemType == Item.ItemType.Weapon)
                    {
                        if (GameObject.ReferenceEquals(this.gameObject, InventoryController.gSlots[1].gameObject))
                        {
                            if (InventoryController.FlowItem())
                            {
                                InventoryController.AcquireItem(item, itemCount);
                                EquipController.choiceEquip(item, false);
                                SetSlotCount(-1);
                                ClearSlot();
                            }
                        }
                        else
                        {
                            if (InventoryController.gSlots[1].item == null)
                            {
                                InventoryController.gSlots[1].AddItem(item, itemCount);
                                EquipController.choiceEquip(item, true);
                                SetSlotCount(-1);
                                ClearSlot();
                            }
                        }
                    }
                    else if (item.itemType == Item.ItemType.Helmet)
                    {
                        if (GameObject.ReferenceEquals(this.gameObject, InventoryController.gSlots[2].gameObject))
                        {
                            if (InventoryController.FlowItem())
                            {
                                InventoryController.AcquireItem(item, itemCount);
                                EquipController.choiceEquip(item, false);
                                SetSlotCount(-1);
                                ClearSlot();
                            }
                        }
                        else
                        {
                            if (InventoryController.gSlots[2].item == null)
                            {
                                InventoryController.gSlots[2].AddItem(item, itemCount);
                                EquipController.choiceEquip(item, true);
                                SetSlotCount(-1);
                                ClearSlot();
                            }
                        }
                    }
                    else if (item.itemType == Item.ItemType.Chestplate)
                    {
                        if (GameObject.ReferenceEquals(this.gameObject, InventoryController.gSlots[3].gameObject))
                        {
                            if (InventoryController.FlowItem())
                            {
                                InventoryController.AcquireItem(item, itemCount);
                                EquipController.choiceEquip(item, false);
                                SetSlotCount(-1);
                                ClearSlot();
                            }
                        }
                        else
                        {
                            if (InventoryController.gSlots[3].item == null)
                            {
                                InventoryController.gSlots[3].AddItem(item, itemCount);
                                EquipController.choiceEquip(item, true);
                                SetSlotCount(-1);
                                ClearSlot();
                            }
                        }
                    }
                    else if (item.itemType == Item.ItemType.Leggings)
                    {
                        if (GameObject.ReferenceEquals(this.gameObject, InventoryController.gSlots[4].gameObject))
                        {
                            if (InventoryController.FlowItem())
                            {
                                InventoryController.AcquireItem(item, itemCount);
                                EquipController.choiceEquip(item, false);
                                SetSlotCount(-1);
                                ClearSlot();
                            }
                        }
                        else
                        {
                            if (InventoryController.gSlots[4].item == null)
                            {
                                InventoryController.gSlots[4].AddItem(item, itemCount);
                                EquipController.choiceEquip(item, true);
                                SetSlotCount(-1);
                                ClearSlot();
                            }
                        }
                    }
                    else if (item.itemType == Item.ItemType.Boots)
                    {
                        if (GameObject.ReferenceEquals(this.gameObject, InventoryController.gSlots[5].gameObject))
                        {
                            if (InventoryController.FlowItem())
                            {
                                InventoryController.AcquireItem(item, itemCount);
                                EquipController.choiceEquip(item, false);
                                SetSlotCount(-1);
                                ClearSlot();
                            }
                        }
                        else
                        {
                            if (InventoryController.gSlots[5].item == null)
                            {
                                InventoryController.gSlots[5].AddItem(item, itemCount);
                                EquipController.choiceEquip(item, true);
                                SetSlotCount(-1);
                                ClearSlot();
                            }
                        }
                    }
                    else if (item.itemType == Item.ItemType.Cape)
                    {
                        if (GameObject.ReferenceEquals(this.gameObject, InventoryController.gSlots[6].gameObject))
                        {
                            if (InventoryController.FlowItem())
                            {
                                InventoryController.AcquireItem(item, itemCount);
                                EquipController.choiceEquip(item, false);
                                SetSlotCount(-1);
                                ClearSlot();
                            }
                        }
                        else
                        {
                            if (InventoryController.gSlots[6].item == null)
                            {
                                InventoryController.gSlots[6].AddItem(item, itemCount);
                                EquipController.choiceEquip(item, true);
                                SetSlotCount(-1);
                                ClearSlot();
                            }
                        }
                    }
                    else if (item.itemType == Item.ItemType.Armor)
                    {
                        if (GameObject.ReferenceEquals(this.gameObject, InventoryController.gSlots[7].gameObject))
                        {
                            if (InventoryController.FlowItem())
                            {
                                InventoryController.AcquireItem(item, itemCount);
                                EquipController.choiceEquip(item, false);
                                SetSlotCount(-1);
                                ClearSlot();
                            }
                        }
                        else
                        {
                            if (InventoryController.gSlots[7].item == null)
                            {
                                InventoryController.gSlots[7].AddItem(item, itemCount);
                                EquipController.choiceEquip(item, true);
                                SetSlotCount(-1);
                                ClearSlot();
                            }
                        }
                    }
                    else
                    {
                        AddItem(item, itemCount);
                    }
                }
            }
        }
        catch (NullReferenceException)
        {

        }
    }

    public void OnBeginDrag(PointerEventData e)
    {
        Vector3 _posO = transform.position;

        if (item != null)
        {
            Drag.instance.drag = this;
            Drag.instance.DragSetImage(itemImage);
            Drag.instance.transform.position = e.position;
        }
    }

    public void OnDrag(PointerEventData e)
    {
        if (item != null)
        {
            Drag.instance.transform.position = e.position;
        }
    }

    public void OnEndDrag(PointerEventData e)
    {
        if (!((Drag.instance.transform.localPosition.x > baseRect.xMin
            && Drag.instance.transform.localPosition.x < baseRect.xMax
            && Drag.instance.transform.localPosition.y > baseRect.yMin
            && Drag.instance.transform.localPosition.y < baseRect.yMax)
            ||
            (Drag.instance.transform.localPosition.x > gearRect.xMin
            && Drag.instance.transform.localPosition.x < gearRect.xMax
            && Drag.instance.transform.localPosition.y + baseUI.transform.localPosition.y > gearRect.yMin + gearUI.transform.localPosition.y
            && Drag.instance.transform.localPosition.y + baseUI.transform.localPosition.y < gearRect.yMax + gearUI.transform.localPosition.y)))
        {
            if (Drag.instance.drag != null && !check)
            {
                for (int i = 0; i < Drag.instance.drag.itemCount; i++)
                {
                    Instantiate(Drag.instance.drag.item.itemPrefab, ThirdPersonController.transform.position + ThirdPersonController.transform.forward + ThirdPersonController.transform.up, Quaternion.identity);
                }

                Drag.instance.drag.ClearSlot();
            }
        }

        Drag.instance.drag = null;
        Drag.instance.SetColor(0);
    }

    public void OnDrop(PointerEventData e)
    {
        if (Drag.instance.drag != null)
        {
            ChangeSlot();
        }
    }

    private void ChangeSlot()
    {
        Item tempItem = item;
        int tempItemCount = itemCount;

        try
        {
            if (check)
            {
                if (Drag.instance.drag.item.itemType == Item.ItemType.Shield)
                {
                    if (GameObject.ReferenceEquals(gameObject, InventoryController.gSlots[0].gameObject))
                    {
                        AddItem(Drag.instance.drag.item, Drag.instance.drag.itemCount);
                        EquipController.choiceEquip(tempItem, false);
                        EquipController.choiceEquip(Drag.instance.drag.item, true);

                        if (tempItem != null)
                        {
                            Drag.instance.drag.AddItem(tempItem, tempItemCount);
                        }
                        else
                        {
                            Drag.instance.drag.ClearSlot();
                        }
                    }
                }
                else if (Drag.instance.drag.item.itemType == Item.ItemType.Weapon)
                {
                    if (GameObject.ReferenceEquals(gameObject, InventoryController.gSlots[1].gameObject))
                    {
                        AddItem(Drag.instance.drag.item, Drag.instance.drag.itemCount);
                        EquipController.choiceEquip(tempItem, false);
                        EquipController.choiceEquip(Drag.instance.drag.item, true);

                        if (tempItem != null)
                        {
                            Drag.instance.drag.AddItem(tempItem, tempItemCount);
                        }
                        else
                        {
                            Drag.instance.drag.ClearSlot();
                        }
                    }
                }
                else if (Drag.instance.drag.item.itemType == Item.ItemType.Helmet)
                {
                    if (GameObject.ReferenceEquals(gameObject, InventoryController.gSlots[2].gameObject))
                    {
                        AddItem(Drag.instance.drag.item, Drag.instance.drag.itemCount);
                        EquipController.choiceEquip(tempItem, false);
                        EquipController.choiceEquip(Drag.instance.drag.item, true);

                        if (tempItem != null)
                        {
                            Drag.instance.drag.AddItem(tempItem, tempItemCount);
                        }
                        else
                        {
                            Drag.instance.drag.ClearSlot();
                        }
                    }
                }
                else if (Drag.instance.drag.item.itemType == Item.ItemType.Chestplate)
                {
                    if (GameObject.ReferenceEquals(gameObject, InventoryController.gSlots[3].gameObject))
                    {
                        AddItem(Drag.instance.drag.item, Drag.instance.drag.itemCount);
                        EquipController.choiceEquip(tempItem, false);
                        EquipController.choiceEquip(Drag.instance.drag.item, true);

                        if (tempItem != null)
                        {
                            Drag.instance.drag.AddItem(tempItem, tempItemCount);
                        }
                        else
                        {
                            Drag.instance.drag.ClearSlot();
                        }
                    }
                }
                else if (Drag.instance.drag.item.itemType == Item.ItemType.Leggings)
                {
                    if (GameObject.ReferenceEquals(gameObject, InventoryController.gSlots[4].gameObject))
                    {
                        AddItem(Drag.instance.drag.item, Drag.instance.drag.itemCount);
                        EquipController.choiceEquip(tempItem, false);
                        EquipController.choiceEquip(Drag.instance.drag.item, true);

                        if (tempItem != null)
                        {
                            Drag.instance.drag.AddItem(tempItem, tempItemCount);
                        }
                        else
                        {
                            Drag.instance.drag.ClearSlot();
                        }
                    }
                }
                else if (Drag.instance.drag.item.itemType == Item.ItemType.Boots)
                {
                    if (GameObject.ReferenceEquals(gameObject, InventoryController.gSlots[5].gameObject))
                    {
                        AddItem(Drag.instance.drag.item, Drag.instance.drag.itemCount);
                        EquipController.choiceEquip(tempItem, false);
                        EquipController.choiceEquip(Drag.instance.drag.item, true);

                        if (tempItem != null)
                        {
                            Drag.instance.drag.AddItem(tempItem, tempItemCount);
                        }
                        else
                        {
                            Drag.instance.drag.ClearSlot();
                        }
                    }
                }
                else if (Drag.instance.drag.item.itemType == Item.ItemType.Cape)
                {
                    if (GameObject.ReferenceEquals(gameObject, InventoryController.gSlots[6].gameObject))
                    {
                        AddItem(Drag.instance.drag.item, Drag.instance.drag.itemCount);
                        EquipController.choiceEquip(tempItem, false);
                        EquipController.choiceEquip(Drag.instance.drag.item, true);

                        if (tempItem != null)
                        {
                            Drag.instance.drag.AddItem(tempItem, tempItemCount);
                        }
                        else
                        {
                            Drag.instance.drag.ClearSlot();
                        }
                    }
                }
                else if (Drag.instance.drag.item.itemType == Item.ItemType.Armor)
                {
                    if (GameObject.ReferenceEquals(gameObject, InventoryController.gSlots[7].gameObject))
                    {
                        AddItem(Drag.instance.drag.item, Drag.instance.drag.itemCount);
                        EquipController.choiceEquip(tempItem, false);
                        EquipController.choiceEquip(Drag.instance.drag.item, true);

                        if (tempItem != null)
                        {
                            Drag.instance.drag.AddItem(tempItem, tempItemCount);
                        }
                        else
                        {
                            Drag.instance.drag.ClearSlot();
                        }
                    }
                }
            }
            else
            {
                if (item != null)
                {
                    if (Drag.instance.drag.item.itemType != item.itemType)
                    {
                        if (!Drag.instance.drag.check)
                        {
                            AddItem(Drag.instance.drag.item, Drag.instance.drag.itemCount);

                            if (tempItem != null)
                            {
                                Drag.instance.drag.AddItem(tempItem, tempItemCount);
                            }
                            else
                            {
                                Drag.instance.drag.ClearSlot();
                            }
                        }
                    }
                    else
                    {
                        AddItem(Drag.instance.drag.item, Drag.instance.drag.itemCount);
                        EquipController.choiceEquip(Drag.instance.drag.item, false);
                        EquipController.choiceEquip(tempItem, true);

                        if (tempItem != null)
                        {
                            Drag.instance.drag.AddItem(tempItem, tempItemCount);
                        }
                        else
                        {
                            Drag.instance.drag.ClearSlot();
                        }
                    }
                }
                else
                {
                    AddItem(Drag.instance.drag.item, Drag.instance.drag.itemCount);

                    if (Drag.instance.drag.check)
                    {
                        EquipController.choiceEquip(Drag.instance.drag.item, false);
                    }
                    if (tempItem != null)
                    {
                        Drag.instance.drag.AddItem(tempItem, tempItemCount);
                    }
                    else
                    {
                        Drag.instance.drag.ClearSlot();
                    }
                }
            }
        }
        catch (NullReferenceException)
        {

        }
    }

    public void OnPointerEnter(PointerEventData e)
    {
        if (item != null)
        {
            tooltipImage.ShowTooltip(item, e.position);
        }
    }

    public void OnPointerExit(PointerEventData e)
    {
        tooltipImage.HideTooltip();
    }
}
