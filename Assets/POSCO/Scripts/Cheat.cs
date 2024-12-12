using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Player>(out Player player))
        {
            foreach(Monster monster in MonsterDataManager.Instance.allMonsterDataList)
            {
                monster.GetExp(1500);
                monster.damage = 200;
            }
        }
    }
}
