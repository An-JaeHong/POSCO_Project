using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealArea : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player와 충돌");
            MonsterDataManager monsterDataManager = MonsterDataManager.Instance;
            foreach (Monster allMonstr in monsterDataManager.allMonsterDataList)
            {
                allMonstr.hp = allMonstr.maxHp;

            }
            print("모든체력 회복");

            Destroy(gameObject);
        }
    }



}
