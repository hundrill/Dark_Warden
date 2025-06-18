using System.Collections;
using TMPro;
using UnityEngine;
using static CardDataManager;

public class UI_Timeline : MonoBehaviour
{
	public TextMeshProUGUI[] txt_Time;

	private void OnEnable()
	{
		//StartCoroutine(Setting());
		CardDataManager.OnCardDataChange += OnCardDataChange;
	}

	IEnumerator Setting()
	{
		while (true)
		{
			if (CardDataManager.instance != null)
			{
				CardDataManager.OnCardDataChange += OnCardDataChange;
				break;
			}

			yield return null;
		}
	}

	public void OnCardDataChange(CARDDATA type, float value)
	{
		if (type == CARDDATA.TIME)
		{
			int nowtime = (int)value;

			for (int i = 0; i < txt_Time.Length; i++)
			{
				if (i == nowtime)
					txt_Time[i].color = Color.blue;
				else
					txt_Time[i].color = Color.white;
			}
		}
	}

}
