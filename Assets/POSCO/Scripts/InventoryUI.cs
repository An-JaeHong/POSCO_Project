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

    private int choiceNum = 0; //최대3마리 몬스터 

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
                    print("첫번째 카드 비활성화");

                    break;
                case 1:
                    tempMonsters.Add(playerMonsters[1]);
                    monsterCardNum[1].interactable = false;
                    print("두번째 카드 비활성화");
                    break;
                case 2:
                    tempMonsters.Add(playerMonsters[2]);

                    monsterCardNum[2].interactable = false;
                    print("세번째 카드 비활성화");
                    break;
                case 3:
                    tempMonsters.Add(playerMonsters[3]);

                    monsterCardNum[3].interactable = false;
                    print("네번째카드 비활성화");
                    break;
                case 4:
                    tempMonsters.Add(playerMonsters[4]);

                    monsterCardNum[4].interactable = false;
                    print("다섯번째카드 비활성화");
                    break;
            }
            choiceNum++;
        }
        else
        {
            print("3개까지만 선택가능");
        }
    }







}
