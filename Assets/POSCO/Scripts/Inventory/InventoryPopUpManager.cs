using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPopUpManager : MonoBehaviour
{

    private static InventoryPopUpManager instance;
    public static InventoryPopUpManager Instance { get { return instance; } }

    public GameObject ivnetoryMenuBackgoundPrefab;
    public GameObject showMonsterBackgoundPrefab;
    public GameObject showSelectedMonsterBackgoundPrefab;

    public GameObject showItemBackgoundPrefab;
    //[SerializeField] private GameObject
    public GameObject canvasTransform;

    public RectTransform inventoryPos;

    private bool isOpenInventory = false;



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


    }

    private void Update()
    {
        //open

        if (isOpenInventory == false && Input.GetKeyUp(KeyCode.I))
        {
            print("´©¸§");
            InstantiateInventoryMenu();
            isOpenInventory = true;
        }

        //close
        if (isOpenInventory == true && Input.GetKeyUp(KeyCode.Escape))
        {
            isOpenInventory = false;
        }
    }

    public void OnshowMonster()
    {
        InstantiateShowMonster();
    }

    public void OnshowSelectedMonster()
    {
        InstantiateSelectedMonster();
    }

    public void OnshowItem()
    {
        InstantiateShowItem();
    }

    public void InstantiateInventoryMenu()
    {
        GameObject InventoryPrefab = Instantiate(ivnetoryMenuBackgoundPrefab, inventoryPos);
    }

    public void InstantiateShowMonster()
    {
        GameObject InventoryPrefab = Instantiate(showMonsterBackgoundPrefab, inventoryPos);
    }

    public void InstantiateSelectedMonster()
    {
        GameObject InventoryPrefab = Instantiate(showSelectedMonsterBackgoundPrefab, inventoryPos);
    }


    public void InstantiateShowItem()
    {
        GameObject InventoryPrefab = Instantiate(showItemBackgoundPrefab, inventoryPos);
    }

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