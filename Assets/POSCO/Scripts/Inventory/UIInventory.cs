using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    //플레이어가 들고있는 몬스터정보 복사 
    public List<Monster> playerMonsterList = new List<Monster>();
    //선택된 몬스터가 임시로 저장되는 공간 
    public List<Monster> TempSelectedMonsterList = new List<Monster>();


    // 인벤토리에 띄울 몬스터 전체 리스트
    public List<GameObject> textureMonsterPrefabsList;
    // 인벤토리에 띄울 몬스터 리스트
    public List<GameObject> texturePlayerMonsterList;

    public GameObject invetoryCameraPrefab;
    public Sprite[] element;
    public Sprite[] elementBackground;
    
    private int choiceNum = 0;

    public RectTransform monsterCardPos;

    private Player player;
    private InventoryPopUp inventoryPopUp;

    public GameObject monsterCardPrefab;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        LoadMonsterPrefabs();
        player = FindObjectOfType<Player>();
        BringPlaterMonsterList();
        FindSameMonsters();
        InstantiatePlayerMonster();
    }

    private void Update()
    {
        
    }

    //GameObject-> Monster 변환 후 playerMonster   List에 저장 

    private void BringPlaterMonsterList()
    {
     
        foreach (GameObject monsterObj in player.playerMonsterPrefabList)
        {
            if (monsterObj.TryGetComponent<Monster>(out Monster monster))
            {
                playerMonsterList.Add(monster);
            }
        }
    }
    private void LoadMonsterPrefabs()
    {
        textureMonsterPrefabsList = new List<GameObject>(Resources.LoadAll<GameObject>("TextureRenderer"));

    }

    public void FindSameMonsters()
    {
       
   
        for (int i = 0; i < playerMonsterList.Count; i++)
        {
              
            foreach (var monster in textureMonsterPrefabsList)
            {
                   
                if (monster.name == playerMonsterList[i].name)
                {
                    //print("있다");
                    texturePlayerMonsterList.Add(monster);
                    break;
                }
            }
        }
        //print(texturePlayerMonsterList.Count);
    }

    public void InstantiatePlayerMonster()
    {
        //print("소환됨 2");
        UIMonster newTextureMonster;
        GameObject newMonsterCamera;
        //print(playerMonsterList.Count);
        //print(player.playerMonsterPrefabList.Count);
        //print(texturePlayerMonsterList.Count);
      
       
        for (int i = 0; i < playerMonsterList.Count; i++)
        {
            float posNum = i * 10f;
      
            newTextureMonster = Instantiate(texturePlayerMonsterList[i]).GetComponent<UIMonster>();
            newTextureMonster.transform.position = new Vector3(20 - posNum, 0, 0);
            newMonsterCamera = Instantiate(invetoryCameraPrefab);
            newMonsterCamera.transform.position = new Vector3(20 - posNum, 2, 4);
            newMonsterCamera.transform.rotation = Quaternion.Euler(15, 180, 0);
            

        }

    }
    //생성된 몬스터 카드 속성에 맞게 생성
    public void InstantiateMonsterCard(GameObject monsterCardBackgroundPrefab)
    {
        Transform target = monsterCardBackgroundPrefab.transform;
        target = monsterCardBackgroundPrefab.transform.Find("MonsterCardGirdLayoutGroup");
        monsterCardPos = target.GetComponent<RectTransform>();

        print(monsterCardPos.transform);
       
        { print("널입니다"); }
        print("진입함?");
      
        for (int i = 0; i < playerMonsterList.Count; i++)
        {
            GameObject monstercard = Instantiate(monsterCardPrefab, monsterCardPos);
            Transform targetBackgroundObject = monstercard.transform.Find("MonsterCard");
           
            Image backgroundImage = targetBackgroundObject.GetComponent<Image>();
            Transform targetElementObject = monstercard.transform.Find("MonsterCard/RoleIcon/Icon");
            Image elementIconObject = targetElementObject.GetComponent<Image>();



            switch (playerMonsterList[i].element)
            {
                case Element.Fire:
        print("진입함?3");
                    backgroundImage.sprite = elementBackground[1];
                    elementIconObject.sprite = element[1];
                    break;
                case Element.Water:
                    print("진입함?4");
                    backgroundImage.sprite = elementBackground[2];
                    elementIconObject.sprite = element[2];
                    break;
                case Element.Grass:
                    print("진입함?5");
                    backgroundImage.sprite = elementBackground[3];
                    elementIconObject.sprite = element[3];
                    break;
            
            }

        }



        //정보를 가지고있어


    }
    //    public void OnMonsterCard(int number)
    //{
    //    print($"{playerMonsterList[0]}");
    //    if (choiceNum < 3)
    //    {
    //        switch (number)
    //        {
    //            case 0:
    //                TempSelectedMonsterList.Add(playerMonsterList[0]);
    //                monsterCardNum[0].interactable = false;
    //                targetRawImage = monsterCardNum.GetComponentInChildren<RawImage>();
    //                print("첫번째 카드선택");
    //                break;
    //            case 1:
    //                TempSelectedMonsterList.Add(playerMonsterList[0]);
    //                monsterCardNum[0].interactable = false;
    //                targetRawImage = monsterCardNum[0].GetComponentInChildren<RawImage>();
    //                print("두번째 카드선택");
    //                break;
    //            case 2:
    //                TempSelectedMonsterList.Add(playerMonsterList[0]);
    //                monsterCardNum[0].interactable = false;
    //                targetRawImage = monsterCardNum[0].GetComponentInChildren<RawImage>();
    //                print("3번째 카드 선택");

    //                break;
    //            case 3:
    //                TempSelectedMonsterList.Add(playerMonsterList[0]);
    //                monsterCardNum[0].interactable = false;
    //                targetRawImage = monsterCardNum[0].GetComponentInChildren<RawImage>();
    //                print("4번째 카드 선택");

    //                break;
    //            case 4:
    //                TempSelectedMonsterList.Add(playerMonsterList[0]);
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
    //    //}
    //}







}


