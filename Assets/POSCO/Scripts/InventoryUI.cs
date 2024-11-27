using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    public GameObject menu;
    public GameObject monsterCard;
    public GameObject showSellectMonster;

    public Button showMonster;
    public Button[] monsterCardNum;
    public Button choiceBattleMonster;
    public Button Sellect;
    public Button Reset;

    public List<GameObject> playerMonsters = new List<GameObject>();
    public List<GameObject> tempMonsters = new List<GameObject>();

    private int choiceNum = 0; //�ִ�3���� ���� 

    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }
    private void Start()
    {
        menu.SetActive(false);
        monsterCard.SetActive(false);
        showSellectMonster.SetActive(false);
        playerMonsters = player.playerMonsters;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            OpenMemu();
        }

    }

    private void OpenMemu()
    {
        menu.SetActive(true);
    }


    public void OnShowMonsterButton()
    {
        monsterCard.SetActive(true);
    }

    public void OnCardButton(int number)
    {
       
        print(number);
        if (choiceNum < 3)
        {
            switch (number)
            {
                case 0:
                    tempMonsters.Add(playerMonsters[0]);
                    monsterCardNum[0].interactable = false;
                    print("ù��° ī�� ��Ȱ��ȭ");

                    break;
                case 1:
                    tempMonsters.Add(playerMonsters[1]);
                    monsterCardNum[1].interactable = false;
                    print("�ι�° ī�� ��Ȱ��ȭ");
                    break;
                case 2:
                    tempMonsters.Add(playerMonsters[2]);

                    monsterCardNum[2].interactable = false;
                    print("����° ī�� ��Ȱ��ȭ");
                    break;
                case 3:
                    tempMonsters.Add(playerMonsters[3]);

                    monsterCardNum[3].interactable = false;
                    print("�׹�°ī�� ��Ȱ��ȭ");
                    break;
                case 4:
                    tempMonsters.Add(playerMonsters[4]);

                    monsterCardNum[4].interactable = false;
                    print("�ټ���°ī�� ��Ȱ��ȭ");
                    break;
            }
            choiceNum++;
        }
        else
        {
            print("3�������� ���ð���");
        }
    }







}
