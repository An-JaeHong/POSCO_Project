using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealArea : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player�� �浹");
            MonsterDataManager monsterDataManager = MonsterDataManager.Instance;
            foreach (Monster allMonstr in monsterDataManager.allMonsterDataList)
            {
                allMonstr.hp = allMonstr.maxHp;

            }
            print("���ü�� ȸ��");

            Destroy(gameObject);
        }
    }



}
