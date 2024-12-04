using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
            UIInventoryManager.Instance.OpenPopup(this.gameObject); // 팝업 열기
            InstantiateInventoryMenu();
            

        }

    }
    
    public void InstantiateInventoryMenu()
    {
   

        inventoryPrefab = Instantiate(ivnetoryMenuBackgoundPrefab, inventoryPos);
        Transform monsterButton = inventoryPrefab.transform.Find("ShowMonsterButton");
        Transform itemButton = inventoryPrefab.transform.Find("ShowItemButton");
        Button showMonsterButton = monsterButton.GetComponentInChildren<Button>();
        Button showItemItemButton = itemButton.GetComponentInChildren<Button>();

        showMonsterButton.onClick.AddListener(() => InstantiateShowMonster());
        showItemItemButton.onClick.AddListener(() => InstantiateShowItem());
        //UIInventoryManager.Instance.OpenPopup(this.gameObject);
    }

    public void InstantiateShowMonster()
    {
  


        GameObject monsterCardBackgroundPrefab = Instantiate(showMonsterBackgoundPrefab, inventoryPos);
        
       
        uiInventory.InstantiateMonsterCard(monsterCardBackgroundPrefab);
        uiInventory.SetSelectButton(monsterCardBackgroundPrefab);
        //UIInventoryManager.Instance.OpenPopup(this.gameObject);
    }

    public void InstantiateSelectedMonster()
    {
  

        GameObject selectedMonsterPrefab = Instantiate(showSelectedMonsterBackgoundPrefab, inventoryPos);
        uiInventory.selectedMonster = selectedMonsterPrefab;
        //UIInventoryManager.Instance.OpenPopup(this.gameObject);
    }



    public void InstantiateShowItem()
    {
    

        GameObject itemPrefab = Instantiate(showItemBackgoundPrefab, inventoryPos);
        //UIInventoryManager.Instance.OpenPopup(this.gameObject);
    }

}




