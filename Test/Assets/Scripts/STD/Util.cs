using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;// for SerializedObject

namespace STD
{
	public class iNumber
	{
		public int num, nS, nE;
		int min, max;
		float delta, _delta;

		public iNumber(int min_, int max_, int n = 0, float dt = 0.2f)
		{
			min = min_;
			max = max_;
			num = n;
			nE = n;
			delta = 0.0f;
			_delta = dt;
		}

		public void add(int n)
		{
			nS = num;
			nE = nE + n;
			if (nE < min)		nE = min;
			else if (nE > max)	nE = max;
			delta = 0.0f;
		}

		public int get(float dt)
		{
			if (num == nE)
				return num;

			delta += dt;
			if (delta > _delta)
				delta = _delta;

			num = (int)Math.linear(delta / _delta, nS, nE);
			return num;
		}

		public static void displayGauge(ref iRect rt, 
			ref iRect rtCurr, ref iRect rtMinus, ref iRect rtPlus,
			int interval, int hp, int hpS, int hpE, int hpMax)
		{
			float width = rt.size.width - interval * 2;

			rtCurr.origin = rt.origin + new iPoint(interval, interval);
			rtCurr.size = new iSize(width * hp / hpMax, rt.size.height - interval * 2);

			float x = interval + width * hpE / hpMax;
			float w = width * (hp - hpE) / hpMax;
			rtMinus.origin = new iPoint(x, interval);
			rtMinus.size = new iSize(w, rt.size.height - interval * 2);

			x = interval + width * hpS / hpMax;
			w = width * (hp - hpS) / hpMax;
			rtPlus.origin = new iPoint(x, interval);
			rtPlus.size = new iSize(w, rt.size.height - interval * 2);
		}
	}

	public class Util
	{
		public static GameObject createPrefabs(string nameInHierachy)
		{
			GameObject go = new GameObject(nameInHierachy);
			go.hideFlags = HideFlags.HideInHierarchy;
			go.SetActive(false);

			go.transform.localScale = new Vector3(1, 1, 1);
			SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
			sr.color = new Color(1, 1, 1);// multy
#if false
			go.AddComponent<T>();
#else// diff <>
			var type = System.Type.GetType(nameInHierachy);// "Hello" => Hello
			if (type != null)
				go.AddComponent(type);
#endif

#if true// 1unit == 1pixel, camera.viewport, screen.setResolution
			Texture2D tex = new Texture2D(1, 1);
			Color[] colors = new Color[1 * 1];
			for (int i = 0; i < 1 * 1; i++)
				colors[i] = new Color(1, 1, 1, 1);
#else
			Texture2D tex = new Texture2D(2, 2);
			// default alpha = 1.0f
			Color[] colors = new Color[2 * 2]
			{
				new Color(1, 1, 1, 1),
				new Color(1, 0, 0, 1),
				new Color(0, 1, 0, 1),
				new Color(0, 0, 1, 1),
			};
#endif
			tex.SetPixels(colors);
			//tex.wrapMode = TextureWrapMode.Clamp;
			//tex.filterMode = FilterMode.Point;// nearest, linear
			tex.Apply();/////////////////////////////////////////////// ????
			sr.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
				new Vector2(0.5f, 0.5f), 1f);

			return go;
		}

		public static GameObject createPrefabs<T>(string nameInHierachy) where T : Component
		{
			GameObject go = new GameObject(nameInHierachy);
			go.hideFlags = HideFlags.HideInHierarchy;
			go.SetActive(false);

			go.transform.localScale = new Vector3(1, 1, 1);
			SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
			sr.color = new Color(1, 1, 1);// multy
			go.AddComponent<T>();

#if true// 1unit == 1pixel, camera.viewport, screen.setResolution
			Texture2D tex = new Texture2D(1, 1);
			Color[] colors = new Color[1 * 1];
			for (int i = 0; i < 1 * 1; i++)
				colors[i] = new Color(1, 1, 1, 1);
#else
			Texture2D tex = new Texture2D(2, 2);
			// default alpha = 1.0f
			Color[] colors = new Color[2 * 2]
			{
				new Color(1, 1, 1, 1),
				new Color(1, 0, 0, 1),
				new Color(0, 1, 0, 1),
				new Color(0, 0, 1, 1),
			};
#endif
			tex.SetPixels(colors);
			//tex.wrapMode = TextureWrapMode.Clamp;
			//tex.filterMode = FilterMode.Point;// nearest, linear
			tex.Apply();/////////////////////////////////////////////// ????
			sr.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
				new Vector2(0.5f, 0.5f), 1f);

			return go;
		}

		public static void resize(GameObject go, float x, float y, float z)
		{
			go.transform.localScale = new Vector3(x, y, z);
		}

		public static void color(GameObject go, float r, float g, float b, float a = 1f)
		{
			SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
			sr.material.color = new Color(r, g, b, a);
		}

		//
		// sprite ??????,
		// 1) pixels per unit ==> 1
		// 2) order in layer 
		//
		public static void sprite(GameObject go, Sprite sprite)
		{
			SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
			sr.sprite = sprite;
		}

		public static iPoint topLeft(GameObject go)
		{
			SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
			return new iPoint(	Mathf.Abs(go.transform.localScale.x) * sr.sprite.rect.width / 2,
								go.transform.localScale.y * sr.sprite.rect.height / 2);
		}
#if false
		public static void topLeft(GameObject go, ref iPoint p)
		{
			SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
			p.x += Mathf.Abs(go.transform.localScale.x) * sr.sprite.rect.width / 2;
			p.y += go.transform.localScale.y * sr.sprite.rect.height / 2;
		}
#endif

		public static GameObject findGameObjectWithTag(string tag)
		{
			addNewTag(tag);
			GameObject go = GameObject.FindGameObjectWithTag(tag);

			return go;
		}

		private static void addNewTag(string tag)
		{
			SerializedObject so = new SerializedObject(
				AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
			SerializedProperty sp = so.FindProperty("tags");

			// ???? ???????? ????
			SerializedProperty p;
			int n = sp.arraySize;
			for (int i=0; i< n; i++)
			{
				p = sp.GetArrayElementAtIndex(i);
				if (p.stringValue.Equals(tag))
					return;
			}
			// ???? ????
			sp.InsertArrayElementAtIndex(n);
			p = sp.GetArrayElementAtIndex(n);
			p.stringValue = tag;

			so.ApplyModifiedProperties();
		}

		struct SpriteInfo
		{
			public string path;
			public Sprite[] sprites;
		}
		static List<SpriteInfo> listSprite = null;

		public static Sprite[] loadAll(string path)
		{
			//return Resources.LoadAll<Sprite>(path);

			if (listSprite == null) 
				listSprite = new List<SpriteInfo>();

			for (int i = 0; i < listSprite.Count; i++)
			{
				if (listSprite[i].path == path)
					return listSprite[i].sprites;
			}

			SpriteInfo si = new SpriteInfo();
			si.path = path;
			si.sprites = Resources.LoadAll<Sprite>(path);
			listSprite.Add(si);

			return si.sprites;
		}

		public static void freeAllSprites()
		{
			//for (int i = 0; i < listSprite.Count; i++)
			//{
			//	listSprite[i].path = null;
			//	listSprite[i].sprites = null;
			//}
			listSprite.Clear();
			//listSprite = null;
		}
	}

	class Math
	{
		public static float linear(float rate, float a, float b)
		{
			rate = Mathf.Clamp(rate, 0, 1);
			return a * (1-rate) + b * rate;
		}

		public static float easeIn(float rate, float a, float b)
		{// y = x^2
			rate = Mathf.Clamp(rate, 0, 1);
			rate = rate * rate * rate;
			return a * (1 - rate) + b * rate;
		}

		public static float easeOut(float rate, float a, float b)
		{// y = 1 - (x-1)^2
			rate = Mathf.Clamp(rate, 0, 1);
#if false// #issue bug
			rate = rate - 1;
			rate = 1f - rate * rate * rate;
#else
			rate = Mathf.Sin(90 * rate * Mathf.Deg2Rad);
			rate = Mathf.Sin(90 * rate * Mathf.Deg2Rad);
#endif
			return a * (1 - rate) + b * rate;
		}


		public static float angleDirection(iPoint s, iPoint e)
		{
			return angleDirection(s.x, s.y, e.x, e.y);
		}
		public static float angleDirection(float sx, float sy, float ex, float ey)
		{
			sy = MainCamera.devHeight - sy;
			ey = MainCamera.devHeight - ey;

			iPoint v;
			v.x = ex - sx;
			v.y = ey - sy;
			//return Mathf.Atan2(v.y, v.x) * 180 / Mathf.PI;
			return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
		}

		public static float angleRotate(float currDegree, float targetDegree, float speed)
		{
			if( currDegree==targetDegree )
				return currDegree;

			float diff = targetDegree - currDegree;
			float ad = Mathf.Abs(diff);
			if (ad > 180)
			{
				//ad = 180;
				ad = Mathf.Abs(ad - 360);
			}
			float r = speed / ad;
			if (r < 1.0f)
			{
				//currDegree = Math.angleRate(currDegree, targetDegree, r);
				if (diff > 360) diff -= 360;
				if (diff > 180) diff -= 360;
				currDegree = currDegree + diff * r;
			}
			else
				currDegree = targetDegree;

			return currDegree;
		}
	}

}

