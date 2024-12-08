using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopupQueueManager : MonoBehaviour
{
    private static PopupQueueManager instance;
    public static PopupQueueManager Instance { get { return instance; } }

    private Queue<string> popupQueue = new Queue<string>();
    private bool isPopupActive = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnqueuePopup(string message)
    {
        popupQueue.Enqueue(message);
        if (!isPopupActive)
        {
            ShowNextPopup();
        }
    }

    public void ShowNextPopup()
    {
        if (popupQueue.Count > 0)
        {
            isPopupActive = true;
            string message = popupQueue.Dequeue();
            var button = new Dictionary<string, UnityAction>
            {
                {
                    "È®ÀÎ",
                    () =>
                    {
                        UIPopupManager.Instance.ClosePopup();
                        isPopupActive = false;
                        ShowNextPopup();
                    }

                }

            };
            UIPopupManager.Instance.ShowPopup(message, button);
        }
    }
}
