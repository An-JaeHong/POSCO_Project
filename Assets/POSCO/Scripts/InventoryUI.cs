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
    private int choiceNum = 0; //최대3마리 몬스터 

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
      
        if (choiceNum < 3)
        {
            switch (number)
            {
                case 0:
                    tempSelectedMonsters.Add(playerMonsters[0]);
                    monsterCardNum[0].interactable = false;
                    print("첫번째 카드 비활성화");
                    print($"{playerMonsters[0]} 임시저장됨");

                    break;
                case 1:
                    tempSelectedMonsters.Add(playerMonsters[1]);
                    monsterCardNum[1].interactable = false;
                    print("두번째 카드 비활성화");
                    print($"{playerMonsters[1]} 임시저장됨");
                    break;
                case 2:
                    tempSelectedMonsters.Add(playerMonsters[2]);
                    monsterCardNum[2].interactable = false;
                    print($"{playerMonsters[3]} 임시저장됨");
                    print("세번째 카드 비활성화");
                    break;
                case 3:
                    tempSelectedMonsters.Add(playerMonsters[3]);
                    monsterCardNum[3].interactable = false;
                    print("네번째카드 비활성화");
                    print($"{playerMonsters[4]} 임시저장됨");
                    break;
                case 4:
                    tempSelectedMonsters.Add(playerMonsters[4]);
                    monsterCardNum[4].interactable = false;
                    print("다섯번째카드 비활성화");
                    print($"{playerMonsters[4]} 임시저장됨");
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
            player.SetSelectedMonsters(tempSelectedMonsters);


         
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

        tempSelectedMonsters.Clear();
    }


}
    