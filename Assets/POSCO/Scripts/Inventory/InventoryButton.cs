using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{

    //��ư ����
    private UIInventory uiInventory;
    private InventoryPopUp inventoryPopUp;
    

    private void Awake()
    {

    }
    private void Start()
    {
       
        uiInventory = FindObjectOfType<UIInventory>();
        inventoryPopUp = FindObjectOfType<InventoryPopUp>();
    }

    public void OnShowMonster()
    {

        inventoryPopUp.InstantiateShowMonster();
    }

    public void OnShowSelectedMonster()
    {

        inventoryPopUp.InstantiateSelectedMonster();
    }

    public void OnShowItem()
    {
     
        inventoryPopUp.InstantiateShowItem();
    }


    public void OnChioseBattleMonsterButton()
    {

        inventoryPopUp.InstantiateSelectedMonster();
    }

    public void OnSelectCardButton(int num)
    {
        uiInventory.OnMonsterCard(num);
    }

    public void OnShowMonsterInfoButton(int num)
    {
        
        inventoryPopUp.ShowMonsterData(num);
    }

    public void OnSelectBoutton()
    {
        if (uiInventory.choiceNum == 3)
        {
            uiInventory.SetSelectMonster();
            UIInventoryManager.Instance.ClosePopup();
        }
        else
        { print("3마리를 선택하세요"); }
    }

    public void OnRestButton()
    {
        uiInventory.ResetCelectedMonster();
    }

    public void OnShowMyBattleMonster()
    {
        inventoryPopUp.ShowMyBattleMonster();
    }

    public void OnOpenItemData(int number)
    {print (number);
        inventoryPopUp.ShowItemDate(number);
    }
    
    //public void OnOpenMonsterData(int number)
    //{
    //    inventoryPopUp.ShowMonsterData(number);
    //}

}
