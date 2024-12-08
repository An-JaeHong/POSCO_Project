using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIPopup : MonoBehaviour
{
    //UIText
    [SerializeField] private TextMeshProUGUI titleText;
    //필요한 내용이 더 있으면 주석 해제
    //[SerializeField] private TextMeshProUGUI contentText;
    //Horizontal이 꼭 있는 오브젝트여야함
    [SerializeField] private RectTransform buttonContainer;

    public RectTransform ButtonContainer => buttonContainer;

    public void SetTitle(string content)
    {
        titleText.text = content;
    }
}
