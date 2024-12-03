using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public RenderTexture[] renderTexture;
    public RenderTexture emptyRenderTexture;
    
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
            Camera camera = newMonsterCamera.GetComponent<Camera>();
            camera.targetTexture = renderTexture[i];




        }

    }
    //생성된 몬스터 카드 속성에 맞게 생성
    public void InstantiateMonsterCard(GameObject monsterCardBackgroundPrefab)
    {
        Transform target = monsterCardBackgroundPrefab.transform;
        target = monsterCardBackgroundPrefab.transform.Find("MonsterCardGirdLayoutGroup");
        monsterCardPos = target.GetComponent<RectTransform>();

        print(monsterCardPos.transform);
            
        for (int i = 0; i < playerMonsterList.Count; i++)
        {
            GameObject monstercard = Instantiate(monsterCardPrefab, monsterCardPos);
            Image backgroundImage = monstercard.GetComponent<Image>();
            Transform targetElementObject = monstercard.transform.Find("RoleIcon/Icon");
            Image elementIconObject = targetElementObject.GetComponent<Image>();
            Transform targetTexture = monstercard.transform.Find("MonsterCardButton");
            RawImage rawImage = targetTexture.GetComponent<RawImage>();
            rawImage.texture = renderTexture[i];
            Transform targetText = monstercard.transform.Find("TextName");
            TMP_Text inputText = targetText.GetComponent<TMP_Text>();
            print (texturePlayerMonsterList[i].name);
            inputText.text = texturePlayerMonsterList[i].name;

            switch (playerMonsterList[i].element)
            {
                case Element.Fire:
        print("진입함?3");
                    backgroundImage.sprite = elementBackground[0];
                    elementIconObject.sprite = element[0];
                    break;
                case Element.Water:
                    print("진입함?4");
                    backgroundImage.sprite = elementBackground[1];
                    elementIconObject.sprite = element[1];
                    break;
                case Element.Grass:
                    print("진입함?5");
                    backgroundImage.sprite = elementBackground[2];
                    elementIconObject.sprite = element[2];
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


