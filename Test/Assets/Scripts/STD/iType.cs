using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STD
{
	public enum iKeystate
	{
		Began = 0,	// pressed
		Moved,		// moved
		Ended,		// released
		Double,
	};

	public delegate bool MethodMouse(iKeystate stat, iPoint point);
	public delegate bool MethodWheel(iPoint wheel);

	public enum iKeyboard
	{
		Left = 0,// a, A, 4, <-
		Right,
		Up,
		Down,
		Space
	};

	public delegate bool MethodKeyboard(iKeystate stat, iKeyboard key);

}

