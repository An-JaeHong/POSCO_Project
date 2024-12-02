    using System.Collections;
using System.Collections.Generic;
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
    public GameObject MonsterCardPrefab;

    public RectTransform MonsterCardPos;
    public RectTransform inventoryPos;

    private bool isOpenInventory = false;
    private UIInventory inventory;

    public Image[] element;
    public Image[] elementBackground;


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

        inventory = FindObjectOfType<UIInventory>();
    }

    private void Update()
    {
        //open

        if (isOpenInventory == false && Input.GetKeyUp(KeyCode.I))
        {
            print("¥©∏ß");
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


  
    public void InstantiateMonsterCard()
    {
        for (int i = 0; i < inventory.playerMonsterList.Count; i++)
        {
            GameObject monstercard = Instantiate(MonsterCardPrefab, MonsterCardPos);
            Transform targetobject = transform.Find("Background");
            Image image = targetobject.GetComponent<Image>();
            image = element[1];
        }
    }


    // Transform targetObject = transform.Find("ParentName/ChildName/GrandchildName");
    public void FindSameMonsters()
    {
        print("º“»Øµ 1");
        for (int i = 0; i < inventory.playerMonsterList.Count; i++)
        {

            foreach (var monster in inventory.textureMonsterPrefabsList)
            {
                if (monster.name == inventory.playerMonsterList[i].name)
                {
                    print(monster.name);
                    inventory.texturePlayerMonsterList.Add(monster);
                    break;
                }
            }
        }
    }

    public void InstantiatePlayerMonster()
    {
        print("º“»Øµ  2");
        UIMonster newTextureMonster;
        UIMonster NewMonsterCamera;
        for (int i = 0; i < inventory.playerMonsterList.Count; i++)
        {
            float posNum = i * 10f;

            newTextureMonster = Instantiate(inventory.texturePlayerMonsterList[i]).GetComponent<UIMonster>();
            newTextureMonster.transform.position = new Vector3(20 - posNum, 0, 0);

            NewMonsterCamera = Instantiate(inventory.cameraForMonster[i]).GetComponent<UIMonster>();
        }

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