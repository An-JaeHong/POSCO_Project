using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class InventoryPopUp : MonoBehaviour
{

    private static InventoryPopUp instance;
    public static InventoryPopUp Instance { get { return instance; } }

    public GameObject ivnetoryMenuBackgoundPrefab;
    public GameObject showMonsterBackgoundPrefab;
    public GameObject showSelectedMonsterBackgoundPrefab;
    public GameObject showItemBackgoundPrefab;
    public GameObject canvasTransform;
    public GameObject monsterCardPrefab;
    public GameObject inventoryPrefab;
    public GameObject myBattleMonsterBackgroundPrefab;
    public GameObject informationPopUpprefab;
    public GameObject[] potionPrefab;

    public RectTransform inventoryPos;


    private bool isOpenInventory = false;

    private UIInventory uiInventory;
    private InventoryButton inventoryButton;

    private int choiceNum = 0;
    private UIPopupManager uiPopupManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        uiPopupManager = new UIPopupManager();
        inventoryButton = FindObjectOfType<InventoryButton>();
        uiInventory = FindObjectOfType<UIInventory>();

    }

    private void Update()
    {
        //open

        if (Input.GetKeyUp(KeyCode.I))
        {
            //isOpenInventory = true;
            //UIInventoryManager.Instance.OpenPopup(this.gameObject); 
            InstantiateInventoryMenu();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePopup();
        }

    }

    public void InstantiateInventoryMenu()
    {

        if (UIInventoryManager.Instance.IsPopupOpen() >= 1) // 팝업이 이미 열려 있는지 확인
        {
            print("이미열림");
            return; // 팝업이 열려 있으면 다시 열지 않음
        }

        inventoryPrefab = Instantiate(ivnetoryMenuBackgoundPrefab, inventoryPos);
        Transform monsterButton = inventoryPrefab.transform.Find("ShowMonsterButton");
        Transform itemButton = inventoryPrefab.transform.Find("ShowItemButton");
        Transform battleMonster = inventoryPrefab.transform.Find("ShowBattleMonster");
        Button onMonsterButton = monsterButton.GetComponentInChildren<Button>();
        Button onItemItemButton = itemButton.GetComponentInChildren<Button>();
        Button onBattleMonster = battleMonster.gameObject.GetComponentInChildren<Button>();

        onMonsterButton.onClick.AddListener(() => InstantiateShowMonster());
        onItemItemButton.onClick.AddListener(() => InstantiateShowItem());
        onBattleMonster.onClick.AddListener(() => ShowMyBattleMonster());

        UIInventoryManager.Instance.OpenPopup(inventoryPrefab);
    }

    public void InstantiateShowMonster()
    {
        if (UIInventoryManager.Instance.IsPopupOpen() >= 2) // 팝업이 이미 열려 있는지 확인
        {
            print("이미열림");
            return; // 팝업이 열려 있으면 다시 열지 않음
        }



        GameObject monsterCardBackgroundPrefab = Instantiate(showMonsterBackgoundPrefab, inventoryPos);


        uiInventory.InstantiateMonsterCard(monsterCardBackgroundPrefab);
        uiInventory.SetSelectButton(monsterCardBackgroundPrefab);

        UIInventoryManager.Instance.OpenPopup(monsterCardBackgroundPrefab);
    }


    public void InstantiateSelectedMonster()
    {
        if (UIInventoryManager.Instance.IsPopupOpen() >= 3) // 팝업이 이미 열려 있는지 확인
        {
            print("이미열림");
            return; // 팝업이 열려 있으면 다시 열지 않음
        }

        uiInventory.choiceNum = 0;
        uiInventory.tempSelectedMonsterList.Clear();
        uiInventory.currentSelectedMonsterList.Clear();
        GameObject selectedMonsterPrefab = Instantiate(showSelectedMonsterBackgoundPrefab, inventoryPos);
        uiInventory.selectedMonster = selectedMonsterPrefab;
        Transform selectButton = selectedMonsterPrefab.transform.Find("SelectButton");
        Transform resetButton = selectedMonsterPrefab.transform.Find("Reset");
        Button onSelectButton = selectButton.GetComponentInChildren<Button>();
        Button onresetButton = resetButton.GetComponentInChildren<Button>();
        onSelectButton.onClick.AddListener(() => inventoryButton.OnSelectBoutton());
        onresetButton.onClick.AddListener(() => inventoryButton.OnRestButton());

        uiInventory.OnCardButtonInteractable();
        UIInventoryManager.Instance.OpenPopup(selectedMonsterPrefab);
    }



    public void InstantiateShowItem()
    {
        if (UIInventoryManager.Instance.IsPopupOpen() >= 2) // 팝업이 이미 열려 있는지 확인
        {
            print("이미열림");
            return; // 팝업이 열려 있으면 다시 열지 않음
        }


        GameObject itemPrefab = Instantiate(showItemBackgoundPrefab, inventoryPos);
        Transform targetbutton;
        Button button;
        for (int i = 0; i < 3; i++)
        {
            targetbutton = itemPrefab.transform.Find($"MonsterCardGirdLayoutGroup/Potion{i + 1}");

            button = targetbutton.GetComponent<Button>();
            int parameterValue = i;
            button.onClick.AddListener(() => inventoryButton.OnOpenItemData(parameterValue));
        }
        UIInventoryManager.Instance.OpenPopup(itemPrefab);
    }

    public void ShowMyBattleMonster()
    {
        if (UIInventoryManager.Instance.IsPopupOpen() >= 2) // 팝업이 이미 열려 있는지 확인
        {
            print("이미열림");
            return; // 팝업이 열려 있으면 다시 열지 않음
        }
        GameObject MyBattleMonsterPrefab = Instantiate(myBattleMonsterBackgroundPrefab, inventoryPos);
        RectTransform rectTransform = MyBattleMonsterPrefab.GetComponent<RectTransform>();
        uiInventory.InstantiateMyBattleMonster(rectTransform);
        UIInventoryManager.Instance.OpenPopup(MyBattleMonsterPrefab);
    }

    public void ShowItemInfo()
    {

    }

    public void ClosePopup()
    {
        isOpenInventory = false;
        UIInventoryManager.Instance.ClosePopup(); // PopupManager를 사용하여 팝업 닫기
    }

    public void ShowItemDate(int number)
    {
        if (UIInventoryManager.Instance.IsPopupOpen() >= 3) // 팝업이 이미 열려 있는지 확인
        {
            print("이미열림");
            return; // 팝업이 열려 있으면 다시 열지 않음
        }
        GameObject itemBackground = Instantiate(informationPopUpprefab, inventoryPos);
        Transform target = itemBackground.transform.Find("ImagePos");
        RectTransform rect = target.GetComponent<RectTransform>();
        GameObject potion = Instantiate(potionPrefab[number], rect);
        target = itemBackground.transform.Find("InfoText");
        TextMeshProUGUI text = target.GetComponent<TextMeshProUGUI>();
        text.text = uiInventory.UpdateItemInfo(number);

        UIInventoryManager.Instance.OpenPopup(itemBackground);
    }

    public void ShowMonsterData(int number)
    {
        if (UIInventoryManager.Instance.IsPopupOpen() >= 3) // 팝업이 이미 열려 있는지 확인
        {
            print("이미열림");
            return; // 팝업이 열려 있으면 다시 열지 않음
        }

        GameObject itemBackground = Instantiate(informationPopUpprefab, inventoryPos);



        Transform target = itemBackground.transform.Find("RawImage");
        RawImage rawImage = target.GetComponent<RawImage>();
        rawImage.texture = uiInventory.renderTexture[number];

        target = itemBackground.transform.Find("InfoText");
        TextMeshProUGUI text = target.GetComponent<TextMeshProUGUI>();
        text.text = uiInventory.UpdateMonsterInfo(number);


        UIInventoryManager.Instance.OpenPopup(itemBackground);

    }
    public void ShowBattleMonsterData(int number)
    {
        if (UIInventoryManager.Instance.IsPopupOpen() >= 3) // 팝업이 이미 열려 있는지 확인
        {
            print("이미열림");
            return; // 팝업이 열려 있으면 다시 열지 않음
        }

        GameObject itemBackground = Instantiate(informationPopUpprefab, inventoryPos);

        for (int i = 0; i < 5; i++)
        {
            if (uiInventory.playerMonsterList[i].name == uiInventory.tempSelectedMonsterList[number].name)
            {
                Transform target = itemBackground.transform.Find("RawImage");
                RawImage rawImage = target.GetComponent<RawImage>();
                rawImage.texture = uiInventory.renderTexture[i];

                target = itemBackground.transform.Find("InfoText");
                TextMeshProUGUI text = target.GetComponent<TextMeshProUGUI>();
                text.text = uiInventory.UpdateSelectedMonsterInfo(number);

            }
            UIInventoryManager.Instance.OpenPopup(itemBackground);
        }
    }


}







