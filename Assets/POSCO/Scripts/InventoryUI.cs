using System.Collections;
using System.Collections.Generic;
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

    public List<GameObject> playerMonsters = new List<GameObject>();
    public List<GameObject> tempSelectedMonsters = new List<GameObject>();

    private bool isOpenInventory = false;
    private int choiceNum = 0; //�ִ�3���� ���� 

    private Player player;

    

    private void init()
    {
        choiceNum = 0;

        foreach ( var button in monsterCardNum)
        {
           button.interactable = false;
        }

        tempSelectedMonsters.Clear();
    }

    private void Awake()
    {
        //player = FindObjectOfType<Player>();
    }
    private void Start()
    {
        player = FindObjectOfType<Player>();
        init();
        inventory.SetActive(false);
        monsterCardBackground.SetActive(false);
        showSelectMonsterBackground.SetActive(false);
        playerMonsters = player.playerMonsters;
    }

    private void OnEnable()
    {
        init();
    }

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
      
        if (choiceNum < 3)
        {
            switch (number)
            {
                case 0:
                    tempSelectedMonsters.Add(playerMonsters[0]);
                    monsterCardNum[0].interactable = false;
                    print("ù��° ī�� ��Ȱ��ȭ");
                    print($"{playerMonsters[0]} �ӽ������");

                    break;
                case 1:
                    tempSelectedMonsters.Add(playerMonsters[1]);
                    monsterCardNum[1].interactable = false;
                    print("�ι�° ī�� ��Ȱ��ȭ");
                    print($"{playerMonsters[1]} �ӽ������");
                    break;
                case 2:
                    tempSelectedMonsters.Add(playerMonsters[2]);
                    monsterCardNum[2].interactable = false;
                    print($"{playerMonsters[3]} �ӽ������");
                    print("����° ī�� ��Ȱ��ȭ");
                    break;
                case 3:
                    tempSelectedMonsters.Add(playerMonsters[3]);
                    monsterCardNum[3].interactable = false;
                    print("�׹�°ī�� ��Ȱ��ȭ");
                    print($"{playerMonsters[4]} �ӽ������");
                    break;
                case 4:
                    tempSelectedMonsters.Add(playerMonsters[4]);
                    monsterCardNum[4].interactable = false;
                    print("�ټ���°ī�� ��Ȱ��ȭ");
                    print($"{playerMonsters[4]} �ӽ������");
                    break;
            }
            choiceNum++;
        }
        else
        {
            print("3�������� ���ð���");
        }
    }

    public void OnSellectBoutton()
    {
        print("Sellect ����");

        if (choiceNum == 3)
        {


            print("���õ�");

            //�÷��̾� ���Ϳ� ���� �־����
            player.SetSelectedMonsters(tempSelectedMonsters);


         
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

        tempSelectedMonsters.Clear();
    }


}
    