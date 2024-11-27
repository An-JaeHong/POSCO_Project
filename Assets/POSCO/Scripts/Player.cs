using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //플레이어가 시작시 가지고 있는 몬스터
    public List<GameObject> playerMonsters = new List<GameObject>();
    //플레이어가 전투를 위해 선택한 몬스터
    public List<GameObject> sellectedMonsters = new List<GameObject>();

    public void SetSellectedMonsters()
    {
    
    }
}
