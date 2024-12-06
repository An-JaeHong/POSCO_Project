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
    
    //버튼들
    public Button showMonster;
    public Button[] monsterCardNum;
    public Button choiceBattleMonster;
    public Button Sellect;
    public Button Reset;

    public List<GameObject> textureMonsterPrefabsList;// 인벤토리에 띄울 몬스터 전체 리스트
    public List<GameObject> texturePlayerMonsterList; // 인벤토리에 띄울 몬스터 리스트

    public GameObject[] selectedMonsterBackground;
    public Image[] elementBackgroundImage;

    public Image[] elementPrefabs;

    public List<GameObject> colectedCard;
    public List<GameObject> ShowColectedMonster;
    public RenderTexture emptyTexureRenderer;
    public RawImage targetRawImage;
    

    public List<Monster> playerMonsterList = new List<Monster>();
    public List<Monster> tempSelectedMonsterList = new List<Monster>();

    private bool isOpenInventory = false;
    private int choiceNum = 0; //최대 3마리까지 선택을 위해 3보다 작음을 확인
    public GameObject[] cameraForMonster;

    private Player player;
    private GameObject targetGameObject;
    private RawImage rawImage;


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


       //GameObject-> Monster 변환후 저장
        ConvertGameObjectToMonster();

      //Inventory에 띄울 몬스터 소환(화면 밖에 소환됨)
        FindSameMonsters();
        InstantiatePlayerMonster();

    }


    //Resours -> TextureRenderer iventory에 표현할 전체 몬스터 정보 리스트에 저장
    private void LoadMonsterPrefabs()
    {
        textureMonsterPrefabsList = new List<GameObject>(Resources.LoadAll<GameObject>("TextureRenderer"));

    }


    //GameObject-> Monster 변환후 저장
    private void ConvertGameObjectToMonster()
    {
        //초기화 한번 후 진행
        playerMonsterList.Clear();


        //foreach (Monster monsterObj in player.GetMonsterPrefabList())
        //{


        //        playerMonsterList.Add(monsterObj);
            
        //}
        //playerMonsterList[0] = player.playerMonsterPrefabList[0].GetComponent<Monster>();
        //playerMonsterList[1] = player.playerMonsterPrefabList[1].GetComponent<Monster>();
        //playerMonsterList[2] = player.playerMonsterPrefabList[2].GetComponent<Monster>();
        //playerMonsterList[3] = player.playerMonsterPrefabList[3].GetComponent<Monster>();
        //playerMonsterList[4] = player.playerMonsterPrefabList[4].GetComponent<Monster>();

        //print(playerMonsterList[1].name);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            if (isOpenInventory == false)
            {
                OpenInventory();
                isOpenInventory = true;
                print(playerMonsterList[0].element);
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
        init();
        inventory.SetActive(true);

    }
    public void OnShowMonsterButton()
    {
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
        for (int i = 0; i < 3; i++)
        {
            targetGameObject = ShowColectedMonster[i];
            rawImage = targetGameObject.GetComponent<RawImage>();
            rawImage.texture = emptyTexureRenderer;
        }
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
                    targetRawImage = monsterCardNum[0].GetComponentInChildren<RawImage>();
                    print("첫번째 카드선택");
                    print($"{playerMonsterList[0]} 저장됨");
                    print(targetRawImage);
                    break;
                case 1:
                    tempSelectedMonsterList.Add(playerMonsterList[1]);
                    monsterCardNum[1].interactable = false;
                    targetRawImage = monsterCardNum[1].GetComponentInChildren<RawImage>();
                    print("두번째 카드선택");
                    print($"{playerMonsterList[1]} 저장됨");
                    print(targetRawImage);
                    break;
                case 2:
                    tempSelectedMonsterList.Add(playerMonsterList[2]);
                    monsterCardNum[2].interactable = false;
                    targetRawImage = monsterCardNum[2].GetComponentInChildren<RawImage>();
                    print("3번째 카드 선택");
                    print($"{playerMonsterList[3]} 저장됨");
                    break;
                case 3:
                    tempSelectedMonsterList.Add(playerMonsterList[3]);
                    monsterCardNum[3].interactable = false;
                    targetRawImage = monsterCardNum[3].GetComponentInChildren<RawImage>();
                    print("4번째 카드 선택");
                    print($"{playerMonsterList[4]} 저장됨");
                    break;
                case 4:
                    tempSelectedMonsterList.Add(playerMonsterList[4]);
                    monsterCardNum[4].interactable = false;
                    targetRawImage = monsterCardNum[4].GetComponentInChildren<RawImage>();
                    print("5번째 카드 선택");
                    print($"{playerMonsterList[4]} 저장됨");
                    break;
            }
        
           choiceNum++;

        }
        else
        {
            print("배틀 몬스터는최대 3마리 입니다.");
        }

        ShowSetCelectMonster(choiceNum - 1);
    }


    //Monster card에 선택한 몬스터 표현
    private void ShowSetCelectMonster(int num)
    {
        targetGameObject = ShowColectedMonster[num];
        rawImage = targetGameObject.GetComponent<RawImage>();
        rawImage.texture = targetRawImage.texture;
    }


    public void OnSelectButton()
    {

        if (choiceNum == 3)
        {

            //�÷��̾� ���Ϳ� ���� �־����
            player.SetSelectedMonsters(tempSelectedMonsterList);
        }

        //�������� stack���� ������ �����Դϴ�.
        //������ ���ô����� ��� �����

        inventory.SetActive(false);
        monsterCardBackground.SetActive(false);
        showSelectMonsterBackground.SetActive(false);

    }

    public void OnRestetButton()
    {
        choiceNum = 0;

        foreach (var button in monsterCardNum)
        {
            button.interactable = true;
        }
        tempSelectedMonsterList.Clear();
        for(int i = 0; i <3;i++)
            {
            targetGameObject = ShowColectedMonster[i];
            rawImage = targetGameObject.GetComponent<RawImage>();
            rawImage.texture = emptyTexureRenderer;
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

            NewMonsterCamera= Instantiate(cameraForMonster[i]).GetComponent<UIMonster>();
        }

    }



}
    