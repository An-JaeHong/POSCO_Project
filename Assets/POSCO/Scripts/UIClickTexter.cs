using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIClickTexter : MonoBehaviour
{
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            foreach (RaycastResult result in results)
            {
                Debug.Log($"Clicked on UI element: {result.gameObject.name}, Type: {result.gameObject.GetType()}");

                // 버튼인지 확인
                if (result.gameObject.GetComponent<Button>() != null)
                {
                    Debug.Log($"Button clicked: {result.gameObject.name}");
                }
            }
        }
    }
}
