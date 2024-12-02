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
    public GameObject MonsterCardPrefab;

    public RectTransform MonsterCardPos;
    public RectTransform inventoryPos;

    private bool isOpenInventory = false;
    private UIInventory uiInventory;


    public Sprite[] element;
    public Sprite[] elementBackground;

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
            print("누름");
            InstantiateInventoryMenu();
            InstantiateMonsterCard();
            isOpenInventory = true;
        }

        //close
        if (isOpenInventory == true && Input.GetKeyUp(KeyCode.Escape))
        {
            isOpenInventory = false;
        }
    }
    
    public void OnShowMonster()
    {
        InstantiateShowMonster();
    }

    public void OnShowSelectedMonster()
    {
        InstantiateSelectedMonster();
    }

    public void OnShowItem()
    {
        InstantiateShowItem();
    }

    //public void OnMonsterCard(int number)
    //{
    //    print($"{uiInventory.playerMonsterList[0]}");
    //    if (choiceNum < 3)
    //    {
    //        switch (number)
    //        {
    //            case 0:
    //                uiInventory.TempSelectedMonsterList.Add(uiInventory.playerMonsterList[0]);
    //                monsterCardNum[0].interactable = false;
    //                targetRawImage = monsterCardNum.GetComponentInChildren<RawImage>();
    //                print("첫번째 카드선택");
    //                break;
    //            case 1:
    //                uiInventory.TempSelectedMonsterList.Add(uiInventory.playerMonsterList[0]);
    //                monsterCardNum[0].interactable = false;
    //                targetRawImage = monsterCardNum[0].GetComponentInChildren<RawImage>();
    //                print("두번째 카드선택");
    //                break;
    //            case 2:
    //                uiInventory.TempSelectedMonsterList.Add(uiInventory.playerMonsterList[0]);
    //                monsterCardNum[0].interactable = false;
    //                targetRawImage = monsterCardNum[0].GetComponentInChildren<RawImage>();
    //                print("3번째 카드 선택");
                
    //                break;
    //            case 3:
    //                uiInventory.TempSelectedMonsterList.Add(uiInventory.playerMonsterList[0]);
    //                monsterCardNum[0].interactable = false;
    //                targetRawImage = monsterCardNum[0].GetComponentInChildren<RawImage>();
    //                print("4번째 카드 선택");
              
    //                break;
    //            case 4:
    //                uiInventory.TempSelectedMonsterList.Add(uiInventory.playerMonsterList[0]);
    //                monsterCardNum[0].interactable = false;
    //                targetRawImage = monsterCardNum[0].GetComponentInChildren<RawImage>();
    //                print("5번째 카드 선택");
             
    //                break;
    //        }

    //        choiceNum++;

    //    }
    //    else
    //    {
    //        print("배틀 몬스터는최대 3마리 입니다.");
    //    }

    //    ShowSetCelectMonster(choiceNum - 1);
    //}

    //private void ShowSetCelectMonster(int num)
    //{
    //    targetGameObject = ShowColectedMonster[num];
    //    rawImage = targetGameObject.GetComponent<RawImage>();
    //    rawImage.texture = targetRawImage.texture;
    //}

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
        for (int i = 0; i < uiInventory.playerMonsterList.Count; i++)
        {
            GameObject monstercard = Instantiate(MonsterCardPrefab, MonsterCardPos);
            Transform targetBackgroundObject = transform.Find("Background");
            Image backgroundImage = targetBackgroundObject.GetComponent<Image>();
            Transform targetElementObject = transform.Find("Background/RoleIcon/RoleIcon");
            Image elementIconObject = targetElementObject.GetComponent<Image>();

            switch (uiInventory.playerMonsterList[i].element)
            {
                case Element.Fire:
                    backgroundImage.sprite = elementBackground[1];
                    elementIconObject.sprite = element[1];
                    break;
                case Element.Water:
                    backgroundImage.sprite = elementBackground[2];
                    elementIconObject.sprite = element[2];
                    break;
                case Element.Grass:
                    backgroundImage.sprite = elementBackground[3];
                    elementIconObject.sprite = element[3];
                    break;
            }
            
        }
    }


    // Transform targetObject = transform.Find("ParentName/ChildName/GrandchildName");
    public void FindSameMonsters()
    {
        print("소환됨1");
        for (int i = 0; i < uiInventory.playerMonsterList.Count; i++)
        {

            foreach (var monster in uiInventory.textureMonsterPrefabsList)
            {
                if (monster.name == uiInventory.playerMonsterList[i].name)
                {
                    print(monster.name);
                    uiInventory.texturePlayerMonsterList.Add(monster);
                    break;
                }
            }
        }
    }

    public void InstantiatePlayerMonster()
    {
        print("소환됨 2");
        UIMonster newTextureMonster;
        UIMonster NewMonsterCamera;
        for (int i = 0; i < uiInventory.playerMonsterList.Count; i++)
        {
            float posNum = i * 10f;

            newTextureMonster = Instantiate(uiInventory.texturePlayerMonsterList[i]).GetComponent<UIMonster>();
            newTextureMonster.transform.position = new Vector3(20 - posNum, 0, 0);

            NewMonsterCamera = Instantiate(uiInventory.cameraForMonster[i]).GetComponent<UIMonster>();
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