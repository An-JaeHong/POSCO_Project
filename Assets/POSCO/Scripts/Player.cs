using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //�÷��̾ ���۽� ������ �ִ� ����
    public List<GameObject> playerMonsters = new List<GameObject>();
    //�÷��̾ ������ ���� ������ ����
    [SerializeField]
    private List<GameObject> selectedMonsters = new List<GameObject>();

    public bool canMove = true; //true : ������ �� ����, false : ������ �� ����


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
                print("�������͸� �����ϼ���");
            }
            else
            {
                
                    print("1");

                
            }

        }
    }

}
