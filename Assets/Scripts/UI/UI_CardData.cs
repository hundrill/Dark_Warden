using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CardData : MonoBehaviour
{
	public CARDDATA myType;
	TextMeshProUGUI txt_Value;

	void Awake()
	{
		txt_Value = GetComponent<TextMeshProUGUI>();
	}

	public void OnCardDataChange(CARDDATA type, float value)
	{
		if (txt_Value == null)
			return;

		if (myType == type)
		{
			txt_Value.text = string.Format("{0}", value);
		}
	}
}
