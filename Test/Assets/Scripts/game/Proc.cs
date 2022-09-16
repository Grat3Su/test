using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

public class Proc : gGUI
{
	public override void load()
	{
		loadBg();

		createPopTop();
		createPopPerson();
		createPopInfo();
		createNewDay();

		//popTop.show(true);
		popPerson.show(true);
		popPersonInfo.show(false);
		popNewDay.show(true);

		//�켱����
		MainCamera.addMethodMouse(new MethodMouse(keyPopInfo));
		MainCamera.addMethodMouse(new MethodMouse(keyPopPerson));
		MainCamera.addMethodMouse(new MethodMouse(keyPopTop));
		MainCamera.addMethodMouse(new MethodMouse(keyBg));
		MainCamera.addMethodMouse(new MethodMouse(keyNewDay));
	}

	public override void free()
	{

	}

	public override void draw(float dt)
	{
		drawBg(dt);
		drawPopTop(dt);
		drawPopPerson(dt);
        drawPopInfo(dt);
		drawNewDay(dt);
	}

	public override bool key(iKeystate stat, iPoint point)
	{
		//keyNewDay(stat, point);
		//keyPopInfo(stat, point);
		//keyPopPerson(stat, point);
		//keyBg(stat, point);
		//keyPopTop(stat, point)

		return false;
	}
	public override bool wheel(iPoint point)
	{
		wheelPopPerson(point);
		return false;
	}

	// ========================================================
	// bg
	// ========================================================
	void loadBg()
	{

	}

	void drawBg(float dt)
	{
		setRGBA(1, 1, 1, 1);
		fillRect(0, 0, MainCamera.devWidth, MainCamera.devHeight);
		
		setRGBA(1, 1, 0, 1);
		fillRect(100, 100, 300, 200);
	}

	bool keyBg(iKeystate stat, iPoint point)
	{
		return false;
	}

	// ========================================================
	// popTop Resource info
	// ========================================================
	iPopup popTop;
	iImage[] imgTopBtn;

	void createPopTop()
	{

	}

	void drawPopTop(float dt)
	{
		//popTop.paint(dt);

		setRGBA(0, 0, 0, 0.8f);
		fillRect(0, 0, MainCamera.devWidth, 60);

		iPoint p = new iPoint(5, 10);
		

		for (int i = 0; i < 4; i++)
		{
			p.x = 5 + i * 150;
			fillRect(p.x, p.y, 50,50);
		}

	}

	bool keyPopTop(iKeystate stat, iPoint point)
	{
		//if (popTop == null || popTop.bShow == false)
		//	return false;

		if (stat == iKeystate.Began)
		{

		}

		return false;
	}

	// ========================================================
	// popPerson
	// ========================================================
	iPopup popPerson = null;

	iStrTex stPerson;
	iImage[] imgPersonBtn;
	iStrTex[][] stPersonBtn;

	iPoint offPerson, offMin, offMax;
	string[] names;
	void createPopPerson()
	{
		iPopup pop = new iPopup();

		iImage img = new iImage();
		iStrTex st = new iStrTex(methodStPerson, 200, 500);
		img.add(st.tex);
		pop.add(img);
		stPerson = st;

		imgPersonBtn = new iImage[100];
		stPersonBtn = new iStrTex[100][];
		for(int i=0; i< people; i++)
		{
			stPersonBtn[i] = new iStrTex[2];

			img = new iImage();
			for(int j=0; j<2; j++)
			{
				st = new iStrTex(methodStPersonBtn, 150, 50);
				st.setString(j + "\n" + i + "��");
				img.add(st.tex);

				stPersonBtn[i][j] = st;
			}
			img.position = new iPoint(20, 10 + 60 * i);
			imgPersonBtn[i] = img;
		}

		pop.style = iPopupStyle.move;
		pop.openPoint = new iPoint(MainCamera.devWidth, (MainCamera.devHeight - 500) / 2);
		pop.closePoint = new iPoint(MainCamera.devWidth - 210, (MainCamera.devHeight - 500) / 2);
		pop._aniDt = 0.5f;
		popPerson = pop;

		offPerson = new iPoint(0, 0);
		offMin = new iPoint(0, 490 - 60 * 100);
		offMax = new iPoint(0, 0);
	}

	public void methodStPerson(iStrTex st)
	{
		iStrTex.methodTexture(st, methodStPerson_);
	}
	int people = 100;
	public void methodStPerson_(iStrTex st)
	{
		setRGBA(0.5f, 0.5f, 0.5f, 0.5f);
		fillRect(0, 0, 300, 600);
		
		for (int i = 0; i < people; i++)
		{
			//for (int j = 0; j < 2; j++)
			//	stPersonBtn[i][j].setString(j + "\n" + i + "��");
			imgPersonBtn[i].frame = (popPerson.selected == i ? 1 : 0);
			imgPersonBtn[i].paint(0.0f, offPerson);
		}

		iRect rt = checkScrollbar(	200 - 20,
									500 - 40);
		// ���� ��ũ�ѹ�
		float x = 200 - 20;
		float y = 10;
		float w = 10;
		float h = 500 - 20;
		setRGBA(0, 0, 0, 1f);
		fillRect(x + w / 2 - 2, y, 4, h);

		// ������
		y += 10 + rt.origin.y;
		h = rt.size.height;
		fillRect(x, y, w, h);
	}
	iRect checkScrollbar(int barW, int barH)
	{
		// ���� ũ�� / �� ũ��
		int miniWidth = 200;
		int miniHeight = 500;

		int mapWidth = 200;
		int mapHeight = 60 * 100;

		// ĭ��
		float numW = 1.0f * mapWidth / miniWidth;
		float numH = 1.0f * mapHeight / miniHeight;

		//int bW = barW / bNumW;
		//int bH = barH / bNumH;
		int bW = barW * miniWidth / mapWidth;
		int bH = barH * miniHeight / mapHeight;

		int bX = (int)Math.linear(offPerson.x / offMin.x, 0, bW * (numW - 1));
		int bY = (int)Math.linear(offPerson.y / offMin.y, 0, bH * (numH - 1));

		return new iRect(bX, bY, bW, bH);
	}

	class Scroll //�׷����ϴ� ��ġ. ��ũ�ѹ� ũ��, 
	{
		public iPoint off, offMin, offMax;
		iRect drawRt;
		iSize barSize;

		public Scroll(iRect rt, iSize size, iSize bs)//�׷����ϴ� ��ġ, ũ��?
		{
			off = new iPoint(0,0);
			offMax = new iPoint(0,0);
			offMin = new iPoint(rt.size.width - size.width, rt.size.height - size.height);

			drawRt = rt;
			barSize = bs;
		}
		
		public void scrollMouse(iPoint mp)
		{
			off.y += mp.y;
			if (off.y < offMin.y)
				off.y = offMin.y;
			else if (off.y > offMax.y)
				off.y = offMax.y;
		}

		public void scrollWheel(iPoint mp)
		{
			off.y += mp.y * 10.0f;

			if (off.y < offMin.y)
				off.y = offMin.y;
			else if (off.y > offMax.y)
				off.y = offMax.y;
		}

		public iRect checkScrollbar(int total)
		{
			// ���� ũ�� / �� ũ��
			int miniWidth = (int)drawRt.size.width;
			int miniHeight = (int)drawRt.size.height;

			int mapWidth = (int)drawRt.size.width;
			int mapHeight = total;//�� ũ��

			// ĭ��
			float numW = 1.0f * mapWidth / miniWidth;
			float numH = 1.0f * mapHeight / miniHeight;


			int bW = (int)barSize.width * miniWidth / mapWidth;
			int bH = (int)barSize.height * miniHeight / mapHeight;

			int bX = (int)Math.linear(off.x / offMin.x, 0, bW * (numW - 1));
			int bY = (int)Math.linear(off.y / offMin.y, 0, bH * (numH - 1));

			return new iRect(bX, bY, bW, bH);
		}
	}


	public void methodStPersonBtn(iStrTex st)
	{
		iStrTex.methodTexture(st, methodStPersonBtn_);
	}
	public void methodStPersonBtn_(iStrTex st)
	{
		string[] strs = st.str.Split("\n");
		int index = int.Parse(strs[0]);
		string s = strs[1];

		GUI.color = Color.white;
		if ( index==0 )
			setRGBA(1, 1, 1, 1);
		else 
			setRGBA(0.3f, 0.3f, 0.3f, 1);
		
		fillRect(0, 0, 150, 50);

		setStringRGBA(0, 0, 0, 1);
		drawString(s, 150 / 2, 50 / 2, VCENTER | HCENTER);

	}

	void drawPopPerson(float dt)
	{
		stPerson.setString(popPerson.selected+ " " + offPerson.y);// click, move

		popPerson.paint(dt);
	}

	bool scroll;
	iPoint prevPoint, firstPoint, mp;

	bool keyPopPerson(iKeystate stat, iPoint point)
	{
		if (popPerson.bShow == false)
			return false;
		
		Debug.Log("popPerson");

		if (popPerson.state != iPopupState.proc)
		{
			// ȭ�� �ȿ� ������
			return true;
			// ������ false
		}

		iPoint p;
		p = popPerson.closePoint;
		p.y += offPerson.y;

		int i, j = -1;
		iSize s = new iSize(0, 0);

		switch ( stat )
		{
			case iKeystate.Began:
				scroll = false;
				firstPoint = point;
				prevPoint = point;
				for(i=0; i<people; i++)
				{
					if ( imgPersonBtn[i].touchRect(p, s).containPoint(point) )//Ŭ���Ǹ� ��
					{
						j = i;
						break;
					}
				}
				if( j!=-1 )
				{
					// audio play button ȿ����
					popPerson.selected = j;					
					select = j;
				}
				break;

			case iKeystate.Moved:
				if( scroll==false )
				{
					mp = point - firstPoint;
					if (Mathf.Sqrt(mp.x * mp.x + mp.y * mp.y) > 5)
					{
						if (point.x > popPerson.closePoint.x && point.x < popPerson.closePoint.x + 200 &&
							point.y > popPerson.closePoint.y && point.y < popPerson.closePoint.y + 500)
							scroll = true;
						prevPoint = point;
												
						popPerson.selected = -1;						
					}
				}

				if( scroll )
				{
					mp = point - prevPoint;
					prevPoint = point;

					//offX += mp.x;
					offPerson.y += mp.y;
					if (offPerson.y < offMin.y)
						offPerson.y = offMin.y;
					else if (offPerson.y > offMax.y)
						offPerson.y = offMax.y;

				}
				break;

			case iKeystate.Ended:
				if (scroll==false)
				{
					if (popPersonInfo.bShow == false)
					{
						if (popPerson.selected != -1)
						{
							if (!popNewDay.bShow)
								popPersonInfo.show(true);

							popPersonInfo.openPoint = imgPersonBtn[popPerson.selected].center(p);
						}
					}
				}
				break;
		}

		return false;
	}

	bool wheelPopPerson(iPoint point)
	{
		if (popNewDay.bShow)
			return false;

		iPoint p = MainCamera.mousePosition();
		if (p.x > popPerson.closePoint.x && p.x < popPerson.closePoint.x + 200 &&
			p.y > popPerson.closePoint.y && p.y < popPerson.closePoint.y + 500)
			offPerson.y += point.y * 10.0f;
		
		if (offPerson.y < offMin.y)
			offPerson.y = offMin.y;
		else if (offPerson.y > offMax.y)
			offPerson.y = offMax.y;

		return true;
	}



	// ========================================================
	// popInfo
	// ========================================================
	iPopup popPersonInfo = null;

	iStrTex stPersonInfo;
	iImage[] imgPersonInfoBtn;
	iStrTex[][] stPersonInfoBtn;
	int select;
	void createPopInfo()
	{
		iPopup pop = new iPopup();

		iImage img = new iImage();
		iStrTex st = new iStrTex(methodStPersonInfo, 700, 400);
		// st.setString imgPersonInfoBtn ���� ��������
		img.add(st.tex);
		pop.add(img);
		stPersonInfo = st;
		select = -1;

		//�ݴ¹�ư / ���� �ٲٱ� ��ư �� ���� �ʿ�
		imgPersonInfoBtn = new iImage[2]; // 0 : �ݱ� 1 : ���� �ٲٱ�
		stPersonInfoBtn = new iStrTex[2][];//������ ��

		for(int i=0; i<2; i++)
		{
			stPersonInfoBtn[i] = new iStrTex[2];

			img = new iImage();
			for (int j = 0; j < 2; j++)
			{
				//�ݱ� : 0
				if ( i==0 )
				{
					st = new iStrTex(methodStPersonInfoBtn, 50, 50);
					st.setString(j+"\n"+"X");
				}
				//���� �ٲٱ� : 1
				else
				{
					st = new iStrTex(methodStPersonInfoBtn, 150, 50);
					st.setString(j + "\n" + " ���� ");
				}
				img.add(st.tex);

				stPersonInfoBtn[i][j] = st;
			}
			if(i == 0)
				img.position = new iPoint(700 - 70, 10);
			else
				img.position = new iPoint(450, 250);
			imgPersonInfoBtn[i] = img;
		}

		pop.style = iPopupStyle.zoom;
		pop.openPoint = new iPoint(MainCamera.devWidth, MainCamera.devHeight );
		pop.closePoint = new iPoint(MainCamera.devWidth/2-350, MainCamera.devHeight/2-200);
		pop._aniDt = 0.5f;
		popPersonInfo = pop;
	}
	public void methodStPersonInfo(iStrTex st)
	{
		iStrTex.methodTexture(st, methodStPersonInfo_);
	}

	public void methodStPersonInfo_(iStrTex st)
	{
		setRGBA(0.5f, 0.5f, 0.5f, 0.5f);
		fillRect(0, 0, 700, 400);

		setRGBA(1, 1, 1, 1);
		drawRect(50, 50, 300, 300);//�̹���
				
		drawString("�̸� : " + select + "��", new iPoint(450, 100), VCENTER | HCENTER);
		drawString("����", new iPoint(450, 150), VCENTER | HCENTER);
		drawString("����", new iPoint(450, 200), VCENTER | HCENTER);

		for (int i = 0; i < 2; i++)
		{
			//for (int j = 0; j < 2; j++)
			//	stPersonBtn[i][j].setString(j + "\n" + i + "��");
			imgPersonInfoBtn[i].frame = (popPersonInfo.selected == i ? 1 : 0);
			imgPersonInfoBtn[i].paint(0.0f, new iPoint(0, 0));
		}
	}

	public void methodStPersonInfoBtn(iStrTex st)
	{
		iStrTex.methodTexture(st, methodStPersonInfoBtn_);
	}
	public void methodStPersonInfoBtn_(iStrTex st)
	{
		string[] strs = st.str.Split("\n");
		int index = int.Parse(strs[0]);
		string s = strs[1];

		GUI.color = Color.white;
		if (index == 0)
			setRGBA(1, 0, 0, 1);
		else
			setRGBA(0.3f, 0.3f, 0.3f, 1);

		fillRect(0, 0, 150, 50);

		setStringRGBA(0, 0, 0, 1);
		drawString(s, 150 / 2, 50 / 2, VCENTER | HCENTER);

	}

	void drawPopInfo(float dt)
	{
		if (popNewDay.bShow)
		{
			popPerson.selected = -1;
			return;
		}
		stPersonInfo.setString(popPerson.selected + "" + popPersonInfo.selected + "" +select);
		popPersonInfo.paint(dt);
	}

	bool keyPopInfo(iKeystate stat, iPoint point)
	{
		if (popPersonInfo.bShow == false)
			return false;

		Debug.Log("popInfo");

		if (popPersonInfo.state != iPopupState.proc)
		{
			// ȭ�� �ȿ� ������
			return true;
			// ������ false
		}
		else
		{
			// ������ false
		}

		int i, j = -1;
		iPoint p;
		p = popPersonInfo.closePoint;
		iSize s = new iSize(0, 0);

		switch (stat)
		{
			case iKeystate.Began:
				scroll = false;
				firstPoint = point;
				prevPoint = point;
				for (i = 0; i < 2; i++)
				{
					if (imgPersonInfoBtn[i].touchRect(p, s).containPoint(point))
					{
						j = i;
						break;
					}
				}
				if (j != -1)
				{
					// audio play button ȿ����
					popPersonInfo.selected = j;
				}
				break;

			case iKeystate.Moved:				
				break;

			case iKeystate.Ended:
				if (popPersonInfo.selected != -1)
				{
					if (popPersonInfo.selected == 0)
					{
						popPerson.selected = -1;
						select = -1;						
						popPersonInfo.show(false);
					}
					
					popPersonInfo.selected = -1;
				}
				break;
		}

		return true;
	}

	iPopup popNewDay = null;

	iStrTex stNewDay;
	void createNewDay()//���ο� �� ui : ���� �ڿ� , �� �ڿ�, �̺�Ʈ ���?
	{
		iPopup pop = new iPopup();

		iImage img = new iImage();
		iStrTex st = new iStrTex(methodStNewDay, MainCamera.devWidth - 200, MainCamera.devHeight - 200);
		st.setString("0");
		img.add(st.tex);
		pop.add(img);
		stNewDay = st;

		int w = MainCamera.devWidth;
		pop.style = iPopupStyle.zoom;
		pop.openPoint = new iPoint(w/ 2, MainCamera.devHeight/2);
		pop.closePoint = new iPoint((w - (w - 200))/2, 100);
		pop._aniDt = 0.5f;
		popNewDay = pop;
	}

	void methodStNewDay(iStrTex st)
	{
		iStrTex.methodTexture(st, methodStNewDay_);
	}

	public void methodStNewDay_(iStrTex st)//��¥ �׸��°�
	{
		setRGBA(0f, 0f, 0f, 0.8f);
		int w = MainCamera.devWidth - 200;
		int h = MainCamera.devHeight - 200;
		fillRect(0, 0, w, h);

		setStringRGBA(1, 1, 1, 1);
		float size = getStringSize();
		setStringSize(80);
		drawString("New Day", w/2, h/2 - 150, VCENTER|HCENTER);
		setStringSize(30);
		
		for (int i = 0; i<4; i++)
			drawString("0 ��", w /4 * (i +1) - 150, h / 2 + 150, VCENTER | HCENTER);

		setStringSize(size);
	}

	void drawNewDay(float dt)
	{				
		popNewDay.paint(dt);
	}

	bool keyNewDay(iKeystate stat, iPoint point)
	{
		if (popNewDay.bShow == false)
			return false;
		
		Debug.Log("popNewDay");

		switch (stat)
		{
			case iKeystate.Began:
				popNewDay.show(false);
				break;

			case iKeystate.Moved:

				break;

			case iKeystate.Ended:

				break;
		}

		return true;
	}
}
