using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public static Storage storage;

	public int day;
	public int hour;
	public PeopleState[] ps;
	bool gameOver;

	void Start()
    {
		if(storage == null)
            storage = new Storage(0, 0, 0, 0, 0, 0);
		day = 0;
		hour = 0;
		ps = new PeopleState[100];
		gameOver = false;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnPeople()
    {
        GameObject go = new GameObject();

        go.AddComponent<PeopleState>();
    }

	enum DoEvent
	{
		Adventure,
		Hunt,
		Research,
		Defense,
		Disease,
		SkipDay
	}

	void doEvent(DoEvent type)
	{
		AddItem item = new AddItem(0);
		if (type == DoEvent.Adventure)
		{
			item.food = Random.Range(0, 2);
			item.people = Random.Range(0, 100) > 50 ? Random.Range(1, 2) : 0;			

			item.mapExp += 4;
			item.takeTime = 4;
		}
		else if (type == DoEvent.Hunt)
		{
			item.food = Random.Range(1, 3);
			item.people = Random.Range(0, 100) > 50 ? Random.Range(1, 2) : 0;
			item.takeTime = 4;
		}
		else if (type == DoEvent.Research)
		{
			int labLevel = storage.lab;
			item.labExp = labLevel < 5 ? Random.Range(1, 3) : Random.Range(2, 5);
			item.takeTime = 4;
		}
		else if (type == DoEvent.SkipDay)
		{
			item.takeTime = 12;			
		}

		updateEvent(item);
	}

	void updateEvent(AddItem item)
	{
        storage.people += item.people;
        storage.food += item.food;
        storage.labExp += item.labExp;
        storage.mapExp += item.mapExp;

        storage.update();

        for (int i = 0; i < storage.people; i++)
        {
            ps[i].takeTime += item.takeTime;
        }
    }
}

public class Storage//저장고. 주로 자원 보관.
{
	public int people, _people;
	public int food, _food;
	public int lab, map;
	public int labExp, mapExp;	

	public Storage(int p, int f, int l, int m, int le, int me)
	{
		people = p; 
		food = f;
		lab = l; labExp = le;
		map = m; mapExp = me;

		_people = lab * 3;
		_food = lab * 5;
	}
	public string getStorageText(int type)
	{
		int[] r = new int[] { people, food, lab, map, labExp, mapExp };
		int[] r0 = new int[] { _people, _food };

		int need = lab < 10 ? 4 * lab : 2 * lab + 20;
		if (type == 4)
			return r[type] + " / " + need;
		else if (type == 5)
			return r[type] + " / " + r[type] * 4;
		else if (type > 1)
			return r[type] + "레벨";

		return r[type] + " / " + r0[type];
	}

	public void update()
	{
		int need = lab < 10 ? 4 * lab : 2 * lab + 20;
		while (labExp > need)
		{
			labExp -= need;
			lab++;

			_people = lab * 3;
			_food =   lab * 5;

			need = lab < 10 ? 4 * lab : 2 * lab + 20;
		}

		if (people > _people)
			people = _people;

		if (food > _food)
			food = _food;

		//4 8 12 16 ...

		while (mapExp > map * 4)
		{
			map++;
		}

		if (people > _people)
			people = _people;
		else if (people < 0)
			people = 0;

		if (food > _food)
			food = _food;
		else if (food == 0)
			food = 0;
	}
}

public struct AddItem
{
	public int people;
	public int food;
	public int labExp;
	public int mapExp;
	public int takeTime;

	public AddItem(int t)
	{
		people = 0;
		food = 0;
		labExp = 0;
		mapExp = 0;
		takeTime = 0;
	}
}