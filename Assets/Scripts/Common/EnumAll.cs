using System;
using UnityEngine;

public enum ITEM
{
	ID,
	ATK,
	DEF,
	HP,
	GRADE,
	TYPE,
	IMAGE,
	MAX
}

public enum ITEMGRADE
{
	NORMAL,
	RARE,
	LEGEND,
	ETERNAL,
}

public enum LANGUAGE
{
	TEXT_KO,
	TEXT_EN,
	MAX
}

public static class EnumUtil<T>
{
	public static T Parse(string s)
	{
		return (T)Enum.Parse(typeof(T), s);
	}

	public static bool TryParse(string s)
	{
		return Enum.IsDefined(typeof(T), s);
	}
}

public class ENUMALL
{

}
