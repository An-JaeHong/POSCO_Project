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
        //if (UIInventoryManager.Instance.IsPopupOpen())
        //{
        //    print("Monster 팝업이 이미 열려 있습니다.");
        //    return;
        //}
        inventoryPopUp.InstantiateShowMonster();
    }

    public void OnShowSelectedMonster()
    {
        //if (UIInventoryManager.Instance.IsPopupOpen())
        //{

        //    return;
        //}
        inventoryPopUp.InstantiateSelectedMonster();
    }

    public void OnShowItem()
    {
        //if (UIInventoryManager.Instance.IsPopupOpen())
        //{
        //    return;
        //}
        inventoryPopUp.InstantiateShowItem();
    }


    public void OnChioseBattleMonsterButton()
    {

        uiInventory.OnCardButtonInteractable();
        inventoryPopUp.InstantiateSelectedMonster();
    }

    public void OnSelectCardButton(int num)
    {
  
        uiInventory.OnMonsterCard(num);
    }

    public void OnSelectBoutton()
    {
        uiInventory.SetSelectMonster();

    }

    public void OnRestButton()
    {
        uiInventory.ResetCelectedMonster();
    }

  
}
