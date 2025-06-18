using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;

public class UI_MyData : MonoBehaviour
{
	public MYDATA myType;
	TextMeshProUGUI txt_Value;
	
	void Awake()
	{
		txt_Value = GetComponent<TextMeshProUGUI>();
	}

	public void OnMyDataChange(MYDATA type, int value)
	{
		if (txt_Value == null)
			return;
		
		if (myType == type)
		{
			txt_Value.text = string.Format("{0}" , value);
		}
	}
}
