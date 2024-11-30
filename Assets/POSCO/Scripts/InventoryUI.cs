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

    public List<GameObject> textureMonsterPrefabsList;// 인벤토리에 띄울 몬스터 전체 리스트
    public List<GameObject> texturePlayerMonsterList; // 인벤토리에 띄울 몬스터 리스트


    public GameObject[] colectedCard;//d
    public Texture[] colectedMonsterTexure;
    public Texture emptyTexureRenderer;
    public RawImage targetRawImage;


    public List<Monster> playerMonsterList = new List<Monster>();
    public List<Monster> tempSelectedMonsterList = new List<Monster>();

    private bool isOpenInventory = false;
    private int choiceNum = 0; //최대 3마리까지 선택을 위해 3보다 작음을 확인

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




        //�����ϸ� GameObject�� Monster���·� �ٲ������
        ConvertGameObjectToMonster();

        //�������� ��µ� ���� ������ ��ȯ
        FindSameMonsters();
        InstantiatePlayerMonster();
    }


    //Resours -> TextureRenderer ���� ���� ��� �������� �ε��Ͽ� ����Ʈ�� ���
    private void LoadMonsterPrefabs()
    {
        textureMonsterPrefabsList = new List<GameObject>(Resources.LoadAll<GameObject>("TextureRenderer"));

    }


    //player���� �Ѿ�� Object���¸� Monster���·� �ٲ��ִ� �Լ�
    private void ConvertGameObjectToMonster()
    {
        //�ϴ� Inventory�� �ִ� ����Ʈ�� �ʱ�ȭ
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
        init();// �����ʿ�

        //inventory Ȱ��ȭ�ϸ� ShowMonsterButton Ȱ��ȭ
        inventory.SetActive(true);

    }
    public void OnShowMonsterButton()
    {
        //monsterCardBackground Ȱ��ȭ�ϸ� cardButton Ȱ��ȭ
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
                   
                    print("첫번째 카드선택");
                    print($"{playerMonsterList[0]} 저장됨");


                    break;
                case 1:
                    tempSelectedMonsterList.Add(playerMonsterList[1]);
                    monsterCardNum[1].interactable = false;
                   
                    print("두번째 카드선택");
                    print($"{playerMonsterList[1]} 저장됨");
                    break;
                case 2:
                    tempSelectedMonsterList.Add(playerMonsterList[2]);
                    monsterCardNum[2].interactable = false;
              
                    print("3번째 카드 선택");
                    print($"{playerMonsterList[3]} 저장됨");
                    break;
                case 3:
                    tempSelectedMonsterList.Add(playerMonsterList[3]);
                    monsterCardNum[3].interactable = false;
    
                    print("4번째 카드 선택");
                    print($"{playerMonsterList[4]} 저장됨");
                    break;
                case 4:
                    tempSelectedMonsterList.Add(playerMonsterList[4]);
                    monsterCardNum[4].interactable = false;
   
                    print("5번째 카드 선택");
                    print($"{playerMonsterList[4]} 저장됨");
                    break;
            }
            targetRawImage = this.GetComponent<RawImage>();

            choiceNum++;

           
            //SetCelectMonster();
        }
        else
        {
            print("공격 몬스터를 설정하세요");
        }
    }





    //Monster card���� ���� ���ý� ������ ���� ����
    private void SetCelectMonster()
    {
        switch(choiceNum)
        {
            case 1:
                colectedMonsterTexure[0] = targetRawImage.texture; 
                break;
            case 2:
                colectedMonsterTexure[1] = targetRawImage.texture;
                break;
            case 3:
                colectedMonsterTexure[2] = targetRawImage.texture;
                break;
        }
    }


    public void OnSellectBoutton()
    {
        //print("Sellect ����");

        if (choiceNum == 3)
        {


            //print("���õ�");

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
        //print("�ʱ�ȭ��");
        choiceNum = 0;

        foreach (var button in monsterCardNum)
        {
            button.interactable = true;
        }
        tempSelectedMonsterList.Clear();

        for(int i = 0; i< 3 ; i++)
        {
            colectedMonsterTexure[i]= targetRawImage.texture;

        }


    }







    public void FindSameMonsters()
    {
        //print("����1");
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

    public void InstantiatePlayerMonster()
    {
        //print("����2");
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
    