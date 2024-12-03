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

    public GameObject[] cameraForMonster;

    public Sprite[] element;
    public Sprite[] elementBackground;

    private int choiceNum = 0;

    private Player player;

    private void Start()
    {
       
    }

    private void OnEnable()
    {
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
        print(player.playerMonsterPrefabList.Count);
        foreach (GameObject monsterObj in player.playerMonsterPrefabList)
        {
            if (monsterObj.TryGetComponent<Monster>(out Monster monster))
            {
                playerMonsterList.Add(monster);
            }
        }
    }

    public void FindSameMonsters()
    {
        print("소환됨1");
        for (int i = 0; i < playerMonsterList.Count; i++)
        {

            foreach (var monster in textureMonsterPrefabsList)
            {
                if (monster.name == playerMonsterList[i].name)
                {
                    print(monster.name);
                    texturePlayerMonsterList.Add(monster);
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
        for (int i = 0; i < playerMonsterList.Count; i++)
        {
            float posNum = i * 10f;

            newTextureMonster = Instantiate(texturePlayerMonsterList[i]).GetComponent<UIMonster>();
            newTextureMonster.transform.position = new Vector3(20 - posNum, 0, 0);

            NewMonsterCamera = Instantiate(cameraForMonster[i]).GetComponent<UIMonster>();
        }

    }
    //생성된 몬스터 카드 속성에 맞게 생성
    public void InstantiateMonsterCard(GameObject monsterCardPrefab, RectTransform monsterCardPos)
    {
        print("진입함?");
        for (int i = 0; i < playerMonsterList.Count; i++)
        {
            GameObject monstercard = Instantiate(monsterCardPrefab, monsterCardPos);
            Transform targetBackgroundObject = transform.Find("MonsterCard");
            Image backgroundImage = targetBackgroundObject.GetComponent<Image>();
            Transform targetElementObject = transform.Find("MonsterCard/RoleIcon/Icon");
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


