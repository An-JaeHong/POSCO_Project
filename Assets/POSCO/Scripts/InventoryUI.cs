using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    public GameObject inventory;
    public GameObject monsterCardBackground;
    public GameObject showSelectMonsterBackground;

    public Button showMonster;
    public Button[] monsterCardNum;
    public Button choiceBattleMonster;
    public Button Sellect;
    public Button Reset;

    public List<GameObject> textureMonsterPrefabsList;// 모든 몬스터(UI에 TextureMonster를 List로 저장)
    public List<GameObject> texturePlayerMonsterList; // UI에 띄울 몬스터 리스트

    public List<Monster> playerMonsterList = new List<Monster>();
    public List<Monster> tempSelectedMonsterList = new List<Monster>();

    private bool isOpenInventory = false;
    private int choiceNum = 0; //최대3마리 몬스터 

    private Player player;



    private void init()
    {
        choiceNum = 0;

        foreach (var button in monsterCardNum)
        {
            button.interactable = false;
        }

        tempSelectedMonsterList.Clear();
    }

    private void Awake()
    {
        //player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        
        LoadMonsterPrefabs();

        player = FindObjectOfType<Player>();
        init();
        inventory.SetActive(false);
        monsterCardBackground.SetActive(false);
        showSelectMonsterBackground.SetActive(false);

       


        //시작하면 GameObject를 Monster형태로 바꿔줘야함
        ConvertGameObjectToMonster();

        //사진으로 출력될 몬스터 프리팹 소환
        FindSameMonsters();
        InstantiatePrefab();
    }


    //Resours -> TextureRenderer 폴더 내의 모든 프리팹을 로드하여 리스트에 담기
    private void LoadMonsterPrefabs()
    {
        textureMonsterPrefabsList = new List<GameObject>(Resources.LoadAll<GameObject>("TextureRenderer"));

    }


    //player에서 넘어온 Object형태를 Monster형태로 바꿔주는 함수
    private void ConvertGameObjectToMonster()
    {
        //일단 Inventory에 있는 리스트를 초기화
        playerMonsterList.Clear();
        foreach (GameObject monsterObj in player.GetMonsterPrefabList())
        {
            if (monsterObj.TryGetComponent<Monster>(out Monster monster))
            {
                print($"{monster.name}");
                playerMonsterList.Add(monster);
            }
        }
    }

    //private void OnEnable()
    //{
    //    init();
    //}

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            if (isOpenInventory == false)
            {
                OpenInventory();
                isOpenInventory = true;
            }
            else
            {
                inventory.SetActive(false);
                monsterCardBackground.SetActive(false);
                showSelectMonsterBackground.SetActive(false);
                isOpenInventory = false;
            }

        }

    }

    private void OpenInventory()
    {
        init();// 수정필요

        //inventory 활성화하면 ShowMonsterButton 활성화
        inventory.SetActive(true);

    }
    public void OnShowMonsterButton()
    {
        //monsterCardBackground 활성화하면 cardButton 활성화
        monsterCardBackground.SetActive(true);
        choiceBattleMonster.interactable = true;
    }

    public void OnChoiceBattleMonsterButton()
    {
        foreach (var button in monsterCardNum)
        {
            button.interactable = true;
        }
        showSelectMonsterBackground.SetActive(true);
        choiceBattleMonster.interactable = false;
    }

    public void OnCardButton(int number)
    {

        print($"{playerMonsterList[0]}");
        if (choiceNum < 3)
        {
            switch (number)
            {
                case 0:
                    tempSelectedMonsterList.Add(playerMonsterList[0]);
                    monsterCardNum[0].interactable = false;
                    print("첫번째 카드 비활성화");
                    print($"{playerMonsterList[0]} 임시저장됨");

                    break;
                case 1:
                    tempSelectedMonsterList.Add(playerMonsterList[1]);
                    monsterCardNum[1].interactable = false;
                    print("두번째 카드 비활성화");
                    print($"{playerMonsterList[1]} 임시저장됨");
                    break;
                case 2:
                    tempSelectedMonsterList.Add(playerMonsterList[2]);
                    monsterCardNum[2].interactable = false;
                    print($"{playerMonsterList[3]} 임시저장됨");
                    print("세번째 카드 비활성화");
                    break;
                case 3:
                    tempSelectedMonsterList.Add(playerMonsterList[3]);
                    monsterCardNum[3].interactable = false;
                    print("네번째카드 비활성화");
                    print($"{playerMonsterList[4]} 임시저장됨");
                    break;
                case 4:
                    tempSelectedMonsterList.Add(playerMonsterList[4]);
                    monsterCardNum[4].interactable = false;
                    print("다섯번째카드 비활성화");
                    print($"{playerMonsterList[4]} 임시저장됨");
                    break;
            }
            choiceNum++;
        }
        else
        {
            print("3개까지만 선택가능");
        }
    }

    public void OnSellectBoutton()
    {
        print("Sellect 누름");

        if (choiceNum == 3)
        {


            print("선택됨");

            //플레이어 몬스터에 정보 넣어야함
            player.SetSelectedMonsters(tempSelectedMonsterList);



        }


        //삭제예정 stack으로 구현할 예정입니다.
        //지금은 선택누를시 모두 종료됨

        inventory.SetActive(false);
        monsterCardBackground.SetActive(false);
        showSelectMonsterBackground.SetActive(false);

    }

    public void OnRestetButton()
    {
        print("초기화됨");
        choiceNum = 0;

        foreach (var button in monsterCardNum)
        {
            button.interactable = true;
        }

        tempSelectedMonsterList.Clear();
    }







    public void FindSameMonsters()
    {
        print("들어옴1");
        for (int i = 0; i < playerMonsterList.Count; i++)
        {
            foreach (var monster in textureMonsterPrefabsList)
            {
                if (monster.name == playerMonsterList[i].name)
                {
                    texturePlayerMonsterList.Add(monster);
                    break;
                }
            }
        }
    }

    public void InstantiatePrefab()
    {
        print("들어옴2");
        GameObject prefabToInstantiate;
        UIMonster newTextureMonster;
        for (int i = 0; i < playerMonsterList.Count; i++)
        {
            float posNum = i * 10f;
            prefabToInstantiate = textureMonsterPrefabsList[i];
            newTextureMonster = Instantiate(prefabToInstantiate).GetComponent<UIMonster>();
            newTextureMonster.transform.position = new Vector3(20 - posNum, 0, 0);
        }

    }






}
    