using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HP_Ui : MonoBehaviour
{
	public Image fill;
	public Image fill_shadow;
	public TextMeshProUGUI txt_Hp;

	public GameObject[] list_Hand;
	public GameObject[] list_Discard;

	private void OnEnable()
	{
		if (fill)
			fill.fillAmount = 1;
	}

	public void SetFill(float pct, bool shadow = false)
	{
		if (fill)
		{
			if(pct < 0) pct = 0;

			fill.fillAmount = pct;

			//if (shadow)
			if (fill_shadow)
				StartCoroutine(Shadow(pct));
		}
	}

	IEnumerator Shadow(float targetPct)
	{
		if (fill_shadow == null)
			yield break;

		float duration = 1f;
		float elapsed = 0f;
		float startPct = fill_shadow.fillAmount;

		yield return new WaitForSeconds(0.1f);

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / duration;
			t = EaseOutExpo(t); // 빠르게 시작하고 부드럽게 감속
			fill_shadow.fillAmount = Mathf.Lerp(startPct, targetPct, t);
			yield return null;
		}

		fill_shadow.fillAmount = targetPct;
	}

	float EaseOutExpo(float t)
	{
		return t >= 1f ? 1f : 1 - Mathf.Pow(2, -10 * t);
	}


	public void SetText(int now, int maxhp)
	{
		if (txt_Hp)
			txt_Hp.text = string.Format("{0} / {1}", now, maxhp);
	}

	public void SettingHand(int num)
	{
		StartCoroutine(SettingHand_Delay(num));
	}

	IEnumerator SettingHand_Delay(int num)
	{
		float delay = 0.1f;
		int activeCount = list_Hand.Count(obj => obj.activeSelf);


		for (int i = 0; i < list_Hand.Length; i++)
		{
			if (list_Hand[i])
			{
				if (!list_Hand[i].activeSelf && i < num)
				{
					yield return new WaitForSeconds(delay);
					EffectManager.instance.CreateEffect(2, list_Hand[i].transform.position, list_Hand[i].transform);
				}

				list_Hand[i].SetActive(i < num);
			}
		}
	}

	public void SettingDiscard(int num)
	{
		StartCoroutine(SettingHand_Discard(num));
	}

	IEnumerator SettingHand_Discard(int num)
	{
		float delay = 0.1f;
		int activeCount = list_Discard.Count(obj => obj.activeSelf);


		for (int i = 0; i < list_Discard.Length; i++)
		{
			if (list_Discard[i])
			{
				if (!list_Discard[i].activeSelf && i < num)
				{
					yield return new WaitForSeconds(delay);
					EffectManager.instance.CreateEffect(3, list_Discard[i].transform.position, list_Discard[i].transform);
				}

				list_Discard[i].SetActive(i < num);
			}
		}
	}

	public void PlusHand(int num)
	{
		for (int i = 0; i < list_Hand.Length; i++)
		{
			if (list_Hand[i] && !list_Hand[i].activeSelf)
			{
				list_Hand[i].SetActive(true);
				break;
			}
		}
	}

	public void PlusDiscard(int num)
	{
		for (int i = 0; i < list_Discard.Length; i++)
		{
			if (list_Discard[i] && !list_Discard[i].activeSelf)
			{
				list_Discard[i].SetActive(true);
				break;
			}
		}
	}

	public void UseHand()
	{
		for (int i = list_Hand.Length - 1; i >= 0; i--)
		{
			if (list_Hand[i] && list_Hand[i].activeSelf)
			{
				EffectManager.instance.CreateEffect(2, list_Hand[i].transform.position);
				break;
			}
		}
	}

	public void UseDiscard()
	{
		for (int i = list_Discard.Length - 1; i >= 0; i--)
		{
			if (list_Discard[i] && list_Discard[i].activeSelf)
			{
				EffectManager.instance.CreateEffect(3, list_Discard[i].transform.position);
				list_Discard[i].SetActive(false);
				break;
			}
		}
	}
}
