using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupController : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private string pickupSound;

    private bool pickupActive = false;
    private RaycastHit hit;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Text actionText;

    [SerializeField] private InventoryController InventoryController;
    private AnimatorController AnimatorController;

    private void Start()
    {
        AnimatorController = GetComponentInParent<AnimatorController>();
    }

    private void Update()
    {
        CheckItem();
        Action();
    }

    private void Action()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickupActive)
        {
            if (hit.transform != null)
            {
                AnimatorController.LoadPickUpAnimation(pickupActive);
                SoundController.instance.PlaySE(pickupSound);
                if (InventoryController.FlowItem())
                {
                    InventoryController.AcquireItem(hit.transform.GetComponent<ItemPickUp>().item);
                    Destroy(hit.transform.gameObject);
                    ItemInfoDisable();
                }
            }
        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), 
            out hit, (range + 3f), layerMask))
        {
            if (hit.transform.tag == "Item")
            {
                ItemInfoEnable();
            }
        }
        else
        {
            ItemInfoDisable();
        }
    }

    private void ItemInfoEnable()
    {
        pickupActive = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hit.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 "
            + "<color=yellow>" + "(F)" + "</color>";
    }

    private void ItemInfoDisable()
    {
        pickupActive = false;
        actionText.gameObject.SetActive(false);
    }
}
