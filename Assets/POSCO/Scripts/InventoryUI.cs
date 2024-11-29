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

    public List<GameObject> textureMonsterPrefabsList;// ��� ����(UI�� TextureMonster�� List�� ����)
    public List<GameObject> texturePlayerMonsterList; // UI�� ��� ���� ����Ʈ

    public List<Monster> playerMonsterList = new List<Monster>();
    public List<Monster> tempSelectedMonsterList = new List<Monster>();

    private bool isOpenInventory = false;
    private int choiceNum = 0; //�ִ�3���� ���� 

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
        InstantiatePrefab();
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
                    print("ù��° ī�� ��Ȱ��ȭ");
                    print($"{playerMonsterList[0]} �ӽ������");

                    break;
                case 1:
                    tempSelectedMonsterList.Add(playerMonsterList[1]);
                    monsterCardNum[1].interactable = false;
                    print("�ι�° ī�� ��Ȱ��ȭ");
                    print($"{playerMonsterList[1]} �ӽ������");
                    break;
                case 2:
                    tempSelectedMonsterList.Add(playerMonsterList[2]);
                    monsterCardNum[2].interactable = false;
                    print($"{playerMonsterList[3]} �ӽ������");
                    print("����° ī�� ��Ȱ��ȭ");
                    break;
                case 3:
                    tempSelectedMonsterList.Add(playerMonsterList[3]);
                    monsterCardNum[3].interactable = false;
                    print("�׹�°ī�� ��Ȱ��ȭ");
                    print($"{playerMonsterList[4]} �ӽ������");
                    break;
                case 4:
                    tempSelectedMonsterList.Add(playerMonsterList[4]);
                    monsterCardNum[4].interactable = false;
                    print("�ټ���°ī�� ��Ȱ��ȭ");
                    print($"{playerMonsterList[4]} �ӽ������");
                    break;
            }
            choiceNum++;
        }
        else
        {
            print("3�������� ���ð���");
        }
    }





    //Monster card���� ���� ���ý� ������ ���� ����
    private void SetCelectMonster()
    {
        switch(choiceNum)
        {
            case 0://all clear
                break;
            case 1:
                break;
            case 2:
                break;
                case 3:
            break;
        }
    }



public void OnSellectBoutton()
    {
        print("Sellect ����");

        if (choiceNum == 3)
        {


            print("���õ�");

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
        print("�ʱ�ȭ��");
        choiceNum = 0;

        foreach (var button in monsterCardNum)
        {
            button.interactable = true;
        }

        tempSelectedMonsterList.Clear();
    }







    public void FindSameMonsters()
    {
        print("����1");
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
        print("����2");
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
    