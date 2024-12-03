using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopUpManager : MonoBehaviour
{

    private static InventoryPopUpManager instance;
    public static InventoryPopUpManager Instance { get { return instance; } }

    public GameObject ivnetoryMenuBackgoundPrefab;
    public GameObject showMonsterBackgoundPrefab;
    public GameObject showSelectedMonsterBackgoundPrefab;
    public GameObject showItemBackgoundPrefab;
    public GameObject canvasTransform;
    public GameObject monsterCardPrefab;

    public RectTransform monsterCardPos;
    public RectTransform inventoryPos;
    

    private bool isOpenInventory = false;

    private UIInventory uiInventory;

    private int choiceNum = 0;

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

        uiInventory = FindObjectOfType<UIInventory>();
    }

    private void Update()
    {
        //open

        if (isOpenInventory == false && Input.GetKeyUp(KeyCode.I))
        {
            isOpenInventory = true;
            print("누름");
            uiInventory.InstantiateMonsterCard(monsterCardPrefab, monsterCardPos);
            InstantiateInventoryMenu();
           

        }

        //close
        if (isOpenInventory == true && Input.GetKeyUp(KeyCode.Escape))
        {
            isOpenInventory = false;
        }
    }
    
    public void InstantiateInventoryMenu()
    {
        GameObject inventoryPrefab = Instantiate(ivnetoryMenuBackgoundPrefab, inventoryPos);
        Transform monsterButton = transform.Find("ShowMonsterButton");
        Transform itemButton = transform.Find("ShowItemButton");
        Button buttonMonster = inventoryPrefab.GetComponentInChildren<Button>();
        Button buttonItem = inventoryPrefab.GetComponentInChildren<Button>();

        buttonMonster = monsterButton.GetComponentInChildren<Button>();
        buttonItem = itemButton.GetComponentInChildren<Button>();

        buttonMonster.onClick.AddListener(() => InstantiateShowMonster());
        buttonItem.onClick.AddListener(() => InstantiateShowItem());


 
    }

    public void InstantiateShowMonster()
    {
        GameObject monsterCardBackgroundPrefab = Instantiate(showMonsterBackgoundPrefab, inventoryPos);
    }

    public void InstantiateSelectedMonster()
    {
        GameObject selectedMonsterPrefab = Instantiate(showSelectedMonsterBackgoundPrefab, inventoryPos);
    }


    public void InstantiateShowItem()
    {
        GameObject itemPrefab = Instantiate(showItemBackgoundPrefab, inventoryPos);
    }

    //버튼을 소환(이미지)
    //public void InstantiateMonsterCard()
    //{
    //    GameObject monsterCardprefab = Instantiate(monsterCardPrefab, monsterCardPos);
       
    //}
}




//InventoryPopUpManager inventoryManager= InventoryPrefab.GetComponent<InventoryPopUpManager>();
////UIInventoryManager.Instance.OpenPopup(inventoryManager);
//if (inventoryManager != null)
//{
//    // Call OpenPopup on the UIInventoryManager singleton
//    UIInventoryManager.Instance.OpenPopup(inventoryManager);
//}
//else
//{
//    Debug.LogError("InventoryPopUpManager component not found on instantiated prefab.");
//