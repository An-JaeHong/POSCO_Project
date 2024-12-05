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

    public void OnSelectBoutton()
    {
        uiInventory.SetSelectMonster();
        UIInventoryManager.Instance.ClosePopup();
    }

    public void OnRestButton()
    {
        uiInventory.ResetCelectedMonster();
    }

    public void OnShowMyBattleMonster()
    {
        inventoryPopUp.ShowMyBattleMonster();
    }

}
