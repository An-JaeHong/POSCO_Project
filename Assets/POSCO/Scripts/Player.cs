using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //플레이어가 시작시 가지고 있는 몬스터
    public List<GameObject> playerMonsters = new List<GameObject>();
    //플레이어가 전투를 위해 선택한 몬스터
    [SerializeField]
    private List<GameObject> selectedMonsters = new List<GameObject>();

    public bool canMove = true; //true : 움직일 수 있음, false : 움직일 수 없음


    public void SetSelectedMonsters(List<GameObject> selecedMonater)
    {
        List<GameObject> temp = new List<GameObject>();
        temp = selecedMonater;
        this.selectedMonsters = temp;
        print(selectedMonsters[1].name);
    }




    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.U))
        {
            //nt($"{selectedMonsters[0].name}");
            if(selectedMonsters.Count == 0)
            {
                print("전투몬스터를 설정하세요");
            }
            else
            {
                
                    print("1");

                
            }

        }
    }

}
