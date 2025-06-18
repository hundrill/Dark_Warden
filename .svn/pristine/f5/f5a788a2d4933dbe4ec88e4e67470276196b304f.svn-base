using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Event_CardData_Change : MonoBehaviour
{

	public Animation anim;

	[SerializeField]
	CARDDATA myCardData;

	public void OnCardDataChange(CARDDATA type, float value)
	{
		if (type == myCardData)
		{
			if (value > 0)
				if (anim && anim.clip != null)
				{
					anim.Stop();
					anim.Play();
				}

			if (type == CARDDATA.SCORE && value > 0)
				StartCoroutine(IncreaseCount((int)value));
		}
	}

	IEnumerator IncreaseCount(int value)
	{
		TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

		int count = 5;
		int gap = value / count;
		int nowdamage = 0;

		if (text)
		{
			for (int i = 0; i < count; i++)
			{
				if (i == count - 1)
					nowdamage = value;
				else
					nowdamage = gap * (i + 1);

				text.text = string.Format("{0}", nowdamage);
				yield return new WaitForSeconds(0.05f);
			}
		}
	}
}
