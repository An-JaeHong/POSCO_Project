using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[Serializable]
public enum Element
{
    FIRE,
    WATER,
    GRASS
}
public class UIMonster : MonoBehaviour
{
    public Element element;
    public string name;
}
