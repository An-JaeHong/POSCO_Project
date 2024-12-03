using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryManager : MonoBehaviour
{
    private static UIInventoryManager instance;
    public static UIInventoryManager Instance {  get { return instance; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }


    private Stack<InventoryPopUp> inventoryPopup = new Stack<InventoryPopUp>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            
        }
    }

    public void OpenPopup(InventoryPopUp popup)
    {
        inventoryPopup.Push(popup);
    }




    //여기서 소환 정보를 가지고
}
