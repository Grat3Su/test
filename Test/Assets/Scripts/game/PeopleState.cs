using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleState : MonoBehaviour
{
    public string pName;
    public int job;
    public int level;
    public float exp;

    void Start()
    {
        //�̸��� �ܺο��� ����
        job = 0;
        level = 0;
        exp = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void jobUpdate(int newJob)
	{
        job = newJob;
	}

    void jobAction()// 0 : ��� / 1 : Ž�谡 / 2 : �ϲ� / 3 : ��� / 4 : ������
	{
		int bonus = level * 2;
		AddItem item = new AddItem(0);
		if (job == 1)//Ž�谡
		{
			//���� Ž���� �� ����ġ�� �ø��� ���� ����� ���ؿ´�
			if (Random.Range(0, 100) > (80 - bonus))
				item.people = 1;

			item.stageExp = 2 + bonus;//������ ���� ��� �� �޶���

		}
		else if (job == 2)//�ϲ�
		{
			//Ž���� ���� �������� ���� �ڿ��� �����Ѵ�
			int mount = Random.Range(0, 5);
			item.food = mount;

		}
		else if (job == 3)//���
		{
			// ��縦 ���� ���������� �ķ��� �����Ѵ�

		}
		else if (job == 4)//������
		{
			//�����ؼ� ���� ����ġ�� �ø���.
			//���� ������ ������ �ڿ� ���ѷ��� ������.
		}
	}
}

public struct AddItem
{
	public int   people;
	public int   food;
	public float labExp;
	public float stageExp;

	public AddItem(int t)
	{
		people = 0;
		food = 0;
		labExp = 0;
		stageExp = 0;
	}

}