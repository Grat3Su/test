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
        //이름은 외부에서 지정
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

    void jobAction()// 0 : 백수 / 1 : 탐험가 / 2 : 일꾼 / 3 : 농부 / 4 : 연구원
	{
		int bonus = level * 2;
		AddItem item = new AddItem(0);
		if (job == 1)//탐험가
		{
			//맵을 탐험해 맵 경험치를 늘리고 가끔 사람을 구해온다
			if (Random.Range(0, 100) > (80 - bonus))
				item.people = 1;

			item.stageExp = 2 + bonus;//레벨에 따라 얻는 량 달라짐

		}
		else if (job == 2)//일꾼
		{
			//탐험한 맵을 바탕으로 맵의 자원을 수집한다
			int mount = Random.Range(0, 5);
			item.food = mount;

		}
		else if (job == 3)//농부
		{
			// 농사를 지어 안정적으로 식량을 수급한다

		}
		else if (job == 4)//연구원
		{
			//연구해서 연구 경험치를 올린다.
			//연구 레벨이 오르면 자원 상한량이 오른다.
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