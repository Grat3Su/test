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
	}

	public override void free()
	{

	}

	public override void draw(float dt)
	{
		drawBg(dt);

		drawPopTop(dt);
		drawPopPerson(dt);
		GUI.DrawTexture(new Rect(MainCamera.devWidth - 200, 150, texScrollView.width, texScrollView.height), texScrollView);
	}

	public override void key(iKeystate stat, iPoint point)
	{
		keyPopPerson(stat, point);
		if (keyPopTop(stat, point))
			return;

		keyBg(stat, point);

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
	iPopup popPerson;

	iStrTex stPerson;
	iImage[] imgPersonBtn;
	iStrTex[][] stPersonBtn;

	RenderTexture texScrollView;
	float offX, offY, maxY;
	string[] names;
	void createPopPerson()
	{
		iPopup pop = new iPopup();

		iImage img = new iImage();
		iStrTex st = new iStrTex(methodStPerson, 200, 500);
		img.add(st.tex);
		pop.add(img);

		imgPersonBtn = new iImage[100];
		stPersonBtn = new iStrTex[100][];
		for(int i=0; i<100; i++)
		{
			stPersonBtn[i] = new iStrTex[2];

			img = new iImage();
			for(int j=0; j<2; j++)
			{
				st = new iStrTex(methodStPersonBtn, 150, 50);
				st.setString(j+"\n"+"name");
				img.add(st.tex);

				stPersonBtn[i][j] = st;
			}
			img.position = new iPoint(0, 60 * i);
			imgPersonBtn[i] = img;
		}

		pop.style = iPopupStyle.move;
		pop.openPoint = new iPoint(MainCamera.devWidth, (MainCamera.devHeight - 500) / 2);
		pop.closePoint = new iPoint(MainCamera.devWidth - 210, (MainCamera.devHeight - 500) / 2);
		pop._aniDt = 0.5f;
		popPerson = pop;
	}

	public void methodStPerson(iStrTex st)
	{
		iStrTex.methodTexture(st, methodStPerson_);
	}
	public void methodStPerson_(iStrTex st)
	{
		int people = 100;
		for (int i = 0; i < people - 1; i++)
		{
			for (int j = 0; j < 2; j++)
				stPersonBtn[i][j].setString(j + "\n" + i + "번");
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

		if( index==0 )
			setRGBA(0.3f, 0.3f, 0.3f, 1);
		else 
			setRGBA(1, 1, 1, 1);
		fillRect(0, 0, 150, 50);

		setStringRGBA(0, 0, 0, 1);
		drawString(s, 150 / 2, 50 / 2, VCENTER | HCENTER);

	}

	void drawPopPerson(float dt)
	{
		stPerson.setString("" + offY);

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

		int i, j = -1;

		switch ( stat )
		{
			case iKeystate.Began:
				scroll = false;
				firstPoint = point;
				prevPoint = point;
				for(i=0; i<10; i++)
				{
					if( imgPersonBtn[i].touchRect(popPerson.closePoint, new iSize(0, 0)).containPoint(point) )
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
						scroll = true;
						prevPoint = point;
					}
				}

				if( scroll )
				{
					mp = point - prevPoint;
					offX += mp.x;
					offY += mp.y;
					// 최대값 최소값
				}
				break;

			case iKeystate.Ended:
				if (scroll==false)
				{
					if( popPerson.selected == -1 )
					{
						Debug.Log("눌름짐 " + popPerson.selected);
					}
				}
				break;
		}

		return true;
	}

	bool wheelPopPerson(iPoint point)
	{
		offY += 10.0f;
		// 최대값 최소값

		return true;
	}

	// ========================================================
	// popInfo
	// ========================================================
}
