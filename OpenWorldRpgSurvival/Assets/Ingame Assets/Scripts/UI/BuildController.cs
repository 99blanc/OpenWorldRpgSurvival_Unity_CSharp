using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Build
{
    public string buildName;
    public Sprite buildImage;
    public string buildDescription;
    public string[] buildNeedItem;
    public int[] buildNeedItemCount;
    public GameObject originalPrefab;
    public GameObject previewPrefab;
    public bool needWorkbench;
}

public class BuildController : MonoBehaviour
{
    public static BuildController instance;

    public static bool buildActive = false;
    public static bool isPreviewActive = false;

    [SerializeField] private float range;
    [SerializeField] private float searchDelay;

    private bool search = false;
    public static bool setWorkbench, setFurnace, setBrewery, setBonfire = false;

    private RaycastHit hitInfo;

    private int tabNumber = 0;
    private int page = 1;
    private int selectedSlotNumber;
    private Build[] selectedTab;

    private GameObject preview;
    private GameObject original;
    public GameObject build;

    [SerializeField] private Build[] construct;
    [SerializeField] private Build[] craft;
    [SerializeField] private Build[] alchemy;

    [SerializeField] private GameObject[] slots;
    [SerializeField] private Image[] slotImage;
    [SerializeField] private Text[] slotName;
    [SerializeField] private Text[] slotDescription;
    [SerializeField] private Text[] slotNeedItem;

    [SerializeField] private GameObject buildUI;
    [SerializeField] private Transform view;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private string clickSound;
    [SerializeField] private string buildSound;
    [SerializeField] private string craftSound;
    [SerializeField] private string alchemySound;
    [SerializeField] private string restSound;

    private InventoryController InventoryController;
    private ThirdPersonController ThirdPersonController;
    private SaveAndLoadController SaveAndLoadController;

    private void Start()
    {
        build = new GameObject("Constructions");
        InventoryController = FindObjectOfType<InventoryController>();
        ThirdPersonController = FindObjectOfType<ThirdPersonController>();
        SaveAndLoadController = FindObjectOfType<SaveAndLoadController>();
        tabNumber = 0;
        page = 1;
        TabSlotSetting(construct);

        if (SaveAndLoadController.instance.button)
        {
            for (int i = 0; i < SaveAndLoadController.constName.Count; i++)
            {
                for (int j = 0; j < construct.Length; j++)
                {
                    if (SaveAndLoadController.constName[i].Equals(construct[j].buildName))
                    {
                        GameObject obj = Instantiate(construct[j].originalPrefab, SaveAndLoadController.constPosition[i], Quaternion.identity, build.transform);
                        obj.transform.eulerAngles = SaveAndLoadController.constRotation[i];
                    }
                }
            }
        }
    }

    private void Update()
    {
        Setup();
        Cursor();
        Search();
    }

    private void Search()
    {
        if (!search)
        {
            search = true;
            StartCoroutine(Bonfire());
        }
    }

    private IEnumerator Bonfire()
    {
        yield return new WaitForSeconds(searchDelay);

        SearchBonfire();
    }

    private void SearchBonfire()
    {
        try
        {
            List<GameObject> Objects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Object"));

            for (int i = 0; i < Objects.Count; i++)
            {
                if (Objects[i].GetComponent<Construction>().setBonfire)
                {
                    float _dir = Vector3.Distance(Objects[i].transform.position, ThirdPersonController.transform.position);

                    if (_dir <= 5f)
                    {
                        SoundController.instance.PlaySE(restSound);
                        setBonfire = true;
                        search = false;
                        return;
                    }
                }
            }

            setBonfire = false;
            search = false;
        }
        catch (NullReferenceException)
        {

        }
    }

    private void SearchWorkbench()
    {
        try
        {
            List<GameObject> Objects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Object"));

            for (int i = 0; i < Objects.Count; i++)
            {
                var _dir = Vector3.Distance(Objects[i].GetComponent<Construction>().transform.position, ThirdPersonController.transform.position);

                if (_dir <= 2f && Objects[i].GetComponent<Construction>().setWorkbench)
                {
                    setWorkbench = true;
                    TabSlotSetting(construct);
                    OpenWindow();
                    return;
                }
            }

            setWorkbench = false;
            TabSlotSetting(construct);
            OpenWindow();
        }
        catch (NullReferenceException)
        {

        }
    }

    private void Setup()
    {
        if ((Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.B)) && !isPreviewActive && !InventoryController.inventoryActive && !InventoryController.gearActive)
        {
            Action();
        }
        if (isPreviewActive)
        {
            PreviewPositionUpdate();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Build();
        }
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            Cancel();
        }
    }

    private void Cursor()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && buildActive)
        {
            SoundController.instance.PlaySE(clickSound);
        }
    }

    private void Action()
    {
        if (!buildActive)
        {
            SearchWorkbench();
            ThirdPersonController.root = true;
        }
        else
        {
            CloseWindow();
            ThirdPersonController.root = false;
        }
    }

    public void TabSetting(int tab)
    {
        tabNumber = tab;
        page = 1;

        switch (tabNumber)
        {
            case 0:
                TabSlotSetting(construct);
                break;
            case 1:
                TabSlotSetting(craft);
                break;
            case 2:
                TabSlotSetting(alchemy);
                break;
        }
    }

    private void ClearSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slotImage[i].sprite = null;
            slotName[i].text = "";
            slotDescription[i].text = "";
            slotNeedItem[i].text = "";
            slots[i].SetActive(false);
        }
    }

    private void TabSlotSetting(Build[] tab)
    {
        ClearSlot();
        selectedTab = tab;

        int startSlotNumber = (page - 1) * slots.Length;

        for (int i = startSlotNumber; i < selectedTab.Length; i++)
        {
            if (i == page * slots.Length)
            {
                break;
            }
            if (selectedTab[i].needWorkbench && !setWorkbench)
            {
                break;
            } 

            slots[i - startSlotNumber].SetActive(true);
            slotImage[i - startSlotNumber].sprite = selectedTab[i].buildImage;
            slotName[i - startSlotNumber].text = selectedTab[i].buildName;
            slotDescription[i - startSlotNumber].text = selectedTab[i].buildDescription;

            for (int j = 0; j < selectedTab[i].buildNeedItem.Length; j++)
            {
                slotNeedItem[i - startSlotNumber].text += selectedTab[i].buildNeedItem[j];
                slotNeedItem[i - startSlotNumber].text += " x " + selectedTab[i].buildNeedItemCount[j] + "\n";
            }
        }
    }

    public void RightPageSetting()
    {
        if (page < selectedTab.Length / slots.Length + 1)
        {
            page++;
        }
        else
        {
            page = 1;
        }

        TabSlotSetting(selectedTab);
    }

    public void LeftPageSetting()
    {
        if (page != 1)
        {
            page--;
        }
        else
        {
            page = selectedTab.Length / slots.Length + 1;
        }

        TabSlotSetting(selectedTab);
    }

    private void OpenWindow()
    {
        buildActive = true;
        buildUI.SetActive(true);
    }

    private void CloseWindow()
    {
        buildActive = false;
        buildUI.SetActive(false);
    }

    public void SlotClick(int slot)
    {
        if (selectedTab == construct)
        {
            ConstructSlotClick(slot);
        }
        else if (selectedTab == craft)
        {
            CraftSlotClick(slot);
        }
        else if (selectedTab == alchemy)
        {
            AlchemySlotClick(slot);
        }
        else
        {

        }
    }

    public void ConstructSlotClick(int slot)
    {
        selectedSlotNumber = slot + (page - 1) * slots.Length;

        if (!CheckIngredient())
        {
            return;
        }

        preview = Instantiate(selectedTab[selectedSlotNumber].previewPrefab, view.position + view.forward, Quaternion.identity);
        original = selectedTab[selectedSlotNumber].originalPrefab;
        isPreviewActive = true;
        buildActive = false;
        buildUI.SetActive(false);
        ThirdPersonController.root = false;
    }

    public void CraftSlotClick(int slot)
    {
        selectedSlotNumber = slot + (page - 1) * slots.Length;

        if (!CheckIngredient())
        {
            return;
        }

        UseIngredient();
        SoundController.instance.PlaySE(craftSound);
        InventoryController.AcquireItem(selectedTab[selectedSlotNumber].originalPrefab.GetComponent<ItemPickUp>().item);
        buildActive = false;
        buildUI.SetActive(false);
        ThirdPersonController.root = false;
    }

    public void AlchemySlotClick(int slot)
    {
        selectedSlotNumber = slot + (page - 1) * slots.Length;

        if (!CheckIngredient())
        {
            return;
        }

        UseIngredient();
        SoundController.instance.PlaySE(alchemySound);
        InventoryController.AcquireItem(selectedTab[selectedSlotNumber].originalPrefab.GetComponent<ItemPickUp>().item);
        buildActive = false;
        buildUI.SetActive(false);
        ThirdPersonController.root = false;
    }

    private bool CheckIngredient()
    {
        for (int i = 0; i < selectedTab[selectedSlotNumber].buildNeedItem.Length; i++)
        {
            if (InventoryController.GetItemCount(selectedTab[selectedSlotNumber].buildNeedItem[i]) < selectedTab[selectedSlotNumber].buildNeedItemCount[i])
            {
                return false;
            }
        }

        return true;
    }

    private void UseIngredient()
    {
        for (int i = 0; i < selectedTab[selectedSlotNumber].buildNeedItem.Length; i++)
        {
            InventoryController.SetItemCount(selectedTab[selectedSlotNumber].buildNeedItem[i], selectedTab[selectedSlotNumber].buildNeedItemCount[i]);
        }
    }

    private void PreviewPositionUpdate()
    {
        if (Physics.Raycast(view.position, view.forward, out hitInfo, (range + 3f), layerMask))
        {
            if (hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    preview.transform.Rotate(0f, -30f, 0f);
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    preview.transform.Rotate(0f, +30f, 0f);
                }

                preview.transform.position = _location;
            }
        }
    }

    private void Cancel()
    {
        if (isPreviewActive)
        {
            Destroy(preview);
        }

        buildActive = false;
        isPreviewActive = false;

        preview = null;
        original = null;

        buildUI.SetActive(false);
    }

    private void Build()
    {
        if (isPreviewActive && preview.GetComponent<PreviewObject>().SetBuildable())
        {
            UseIngredient();
            SaveAndLoadController.constName.Add(selectedTab[selectedSlotNumber].buildName);
            Instantiate(original, preview.transform.position, preview.transform.rotation, build.transform);

            if (original.GetComponent<Construction>().setBonfire)
            {
                search = true;
                StartCoroutine(Bonfire());
            }

            SaveAndLoadController.constPosition.Add(preview.transform.position);
            SaveAndLoadController.constRotation.Add(preview.transform.eulerAngles);
            Destroy(preview);
            SoundController.instance.PlaySE(buildSound);
            buildActive = false;
            isPreviewActive = false;
            preview = null;
            original = null;
        }
    }
}
