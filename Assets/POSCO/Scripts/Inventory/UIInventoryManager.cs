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

    private Stack<InventoryPopUpManager> inventoryPopup = new Stack<InventoryPopUpManager>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            
        }
    }

    public void OpenPopup(InventoryPopUpManager popup)
    {
        inventoryPopup.Push(popup);
    }



}
