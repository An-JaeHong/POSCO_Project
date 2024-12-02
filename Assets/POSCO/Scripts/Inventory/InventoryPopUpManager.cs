using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPopUpManager : MonoBehaviour
{

    private InventoryPopUpManager instance;
    public static InventoryPopUpManager Instance { get { return Instance; } }

    [SerializeField] private GameObject BackgroundPrefab;
    //[SerializeField] private GameObject
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject canvasTransform;

    private void Start()
    {

        //open
        if (Input.GetKeyUp(KeyCode.I))
        {

        }

        //close
        if (Input.GetKeyUp(KeyCode.Escape))
        {

        }
    }
    
}
