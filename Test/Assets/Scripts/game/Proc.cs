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

		//popTop.show(true);
		popPerson.show(true);
		popPersonInfo.show(false);
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
	}

	public override void key(iKeystate stat, iPoint point)
	{
		keyPopPerson(stat, point);

		keyBg(stat, point);
		keyPopInfo(stat, point);
		if (keyPopTop(stat, point))
			return;

		if (stat == iKeystate.Began)
		{
		//	Main.me.reset("Intro");

		}
	}
	public override void wheel(iPoint point)
	{
		wheelPopPerson(point);
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

	void keyBg(iKeystate stat, iPoint point)
	{

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

		return true;
	}

	// ========================================================
	// popPerson
	// ========================================================
	iPopup popPerson = null;

	iStrTex stPerson;
	iImage[] imgPersonBtn;
	iStrTex[][] stPersonBtn;

	float offX, offY, minY, maxY;
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
				st.setString(j + "\n" + i + "번");
				img.add(st.tex);

				stPersonBtn[i][j] = st;
			}
			img.position = new iPoint(30, 10 + 60 * i);
			imgPersonBtn[i] = img;
		}

		pop.style = iPopupStyle.move;
		pop.openPoint = new iPoint(MainCamera.devWidth, (MainCamera.devHeight - 500) / 2);
		pop.closePoint = new iPoint(MainCamera.devWidth - 210, (MainCamera.devHeight - 500) / 2);
		pop._aniDt = 0.5f;
		popPerson = pop;

		offX = 0;
		offY = 0;
		minY = 490 - 60 * 100;
		maxY = 0;
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
			//	stPersonBtn[i][j].setString(j + "\n" + i + "번");
			imgPersonBtn[i].frame = (popPerson.selected == i ? 1 : 0);
			imgPersonBtn[i].paint(0.0f, new iPoint(offX, offY));
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

	iKeystate statePerson_ = iKeystate.Moved;
	void drawPopPerson(float dt)
	{
		stPerson.setString(popPerson.selected+ " " +statePerson_ + "" + offY);// click, move

		popPerson.paint(dt);
	}

	bool scroll;
	iPoint prevPoint, firstPoint, mp;

	bool keyPopPerson(iKeystate stat, iPoint point)
	{
		if (popPerson.bShow == false)
			return false;
		if (popPerson.state != iPopupState.proc)
		{
			// 화면 안에 있을때
			return true;
			// 없을대 false
		}
		else
		{
			// 없을대 false
		}

		iPoint p;
		p = popPerson.closePoint;
		p.y += offY;

		statePerson_ = stat;

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
					if ( imgPersonBtn[i].touchRect(p, s).containPoint(point) )//클릭되면 ㅁ
					{
						j = i;
						
						break;
					}
				}
				if( j!=-1 )
				{
					// audio play button 효과음
					popPerson.selected = j;
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
					offY += mp.y;
					if (offY < minY)
						offY = minY;
					else if (offY > maxY)
						offY = maxY;
				}
				break;

			case iKeystate.Ended:
				if (scroll==false)
				{
					if (popPersonInfo.bShow == false)
					{
						popPersonInfo.show(true);
						popPersonInfo.openPoint = imgPersonBtn[popPerson.selected].center(p);
					}
				}
				break;
		}

		return true;
	}

	bool wheelPopPerson(iPoint point)
	{
		iPoint p = MainCamera.mousePosition();
		if (p.x > popPerson.closePoint.x && p.x < popPerson.closePoint.x + 200 &&
			p.y > popPerson.closePoint.y && p.y < popPerson.closePoint.y + 500)
			offY += point.y * 10.0f;
		
		if (offY < minY)
			offY = minY;
		else if (offY > maxY)
			offY = maxY;

		return true;
	}

	// ========================================================
	// popInfo
	// ========================================================
	iPopup popPersonInfo = null;

	iStrTex stPersonInfo;
	iImage[] imgPersonInfoBtn;
	iStrTex[][] stPersonInfoBtn;

	void createPopInfo()
	{
		iPopup pop = new iPopup();

		iImage img = new iImage();
		iStrTex st = new iStrTex(methodStPersonInfo, 700, 400);
		// st.setString imgPersonInfoBtn 아직 생성안함
		img.add(st.tex);
		pop.add(img);
		stPersonInfo = st;

		//닫는버튼 / 직업 바꾸기 버튼 두 개만 필요
		imgPersonInfoBtn = new iImage[2]; // 0 : 닫기 1 : 직업 바꾸기
		stPersonInfoBtn = new iStrTex[2][];//눌렸을 때

		for(int i=0; i<2; i++)
		{
			stPersonInfoBtn[i] = new iStrTex[2];

			img = new iImage();
			for (int j = 0; j < 2; j++)
			{
				//닫기 : 0
				if ( i==0 )
				{
					st = new iStrTex(methodStPersonInfoBtn, 50, 50);
					st.setString(j+"\n"+"X");
				}
				//직업 바꾸기 : 1
				else
				{
					st = new iStrTex(methodStPersonInfoBtn, 150, 50);
					st.setString(j + "\n" + " 직업 ");
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
		drawRect(50, 50, 300, 300);//이미지

		drawString("이름 : " + popPerson.selected + "번", new iPoint(450, 100), VCENTER | HCENTER);
		drawString("레벨", new iPoint(450, 150), VCENTER | HCENTER);
		drawString("동작", new iPoint(450, 200), VCENTER | HCENTER);

		for (int i = 0; i < 2; i++)
		{
			//for (int j = 0; j < 2; j++)
			//	stPersonBtn[i][j].setString(j + "\n" + i + "번");
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
		stPersonInfo.setString(popPerson.selected + "" + popPersonInfo.selected);
		popPersonInfo.paint(dt);
	}

	bool keyPopInfo(iKeystate stat, iPoint point)
	{
		//if (popTop == null || popTop.bShow == false)
		//	return false;

		if (popPersonInfo.bShow == false)
			return false;
		if (popPersonInfo.state != iPopupState.proc)
		{
			// 화면 안에 있을때
			return true;
			// 없을대 false
		}
		else
		{
			// 없을대 false
		}

		statePerson_ = stat;

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
					// audio play button 효과음
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
						popPersonInfo.show(false);
					}
					
					popPersonInfo.selected = -1;
				}
				break;
		}

		return true;
	}
}
