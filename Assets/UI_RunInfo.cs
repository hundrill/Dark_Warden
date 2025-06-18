using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using VideoPokerKit;

public class UI_RunInfo : MonoBehaviour
{
	public GameObject[] list_RunInfo;
	private void Awake()
	{
	}

	private void OnEnable()
	{
		if (UI_Card.instance)
			UI_Card.instance.is_Dlg_Open = true;

		for (int i = 0; i < list_RunInfo.Length; i++)
		{
			Wins winType = Wins.WIN_STRAIGHT_FLUSH - i;
			list_RunInfo[i].GetComponent<item_one_runinfo>().SetRuninfo(
				CardDataManager.name_Wins[(int)winType],
				Paytable.the.GetLevel(winType),
				Paytable.the.GetWinMultiplier(winType),
				Paytable.the.GetChips(winType),
				Paytable.the.GetCount(winType)
				);
		}
	}

	public void StartEvent(int idx, int max_chip, int max_mult, int max_level)
	{
		StartCoroutine(LevelUpEvent(idx, max_chip, max_mult, max_level));
	}

	IEnumerator LevelUpEvent(int idx , int max_chip, int max_mult , int max_level)
	{
		Time.timeScale = 1f;

		yield return new WaitForSeconds(0.2f);

		GameObject eff = EffectManager.instance.CreateEffect(2, list_RunInfo[idx].GetComponent<item_one_runinfo>().txt_Chips.transform.position/*, list_RunInfo[0].transform*/);
		eff.transform.localScale *= 1.8f;
		SetLayerRecursively(eff, LayerMask.NameToLayer("DIALOG"));
		SetParticleSortingLayer(eff);

		yield return StartCoroutine(AnimateChips(list_RunInfo[idx].GetComponent<item_one_runinfo>().txt_Chips, max_chip, 0.6f));

		eff = EffectManager.instance.CreateEffect(3, list_RunInfo[idx].GetComponent<item_one_runinfo>().txt_Mult.transform.position/*, list_RunInfo[0].transform*/);
		eff.transform.localScale *= 1.8f;
		SetLayerRecursively(eff, LayerMask.NameToLayer("DIALOG"));
		SetParticleSortingLayer(eff);

		yield return StartCoroutine(AnimateChips(list_RunInfo[idx].GetComponent<item_one_runinfo>().txt_Mult, max_mult, 0.6f));

		eff = EffectManager.instance.CreateEffect(3, list_RunInfo[idx].GetComponent<item_one_runinfo>().txt_Level.transform.position/*, list_RunInfo[0].transform*/);
		eff.transform.localScale *= 1.8f;
		SetLayerRecursively(eff, LayerMask.NameToLayer("DIALOG"));
		SetParticleSortingLayer(eff);

		yield return StartCoroutine(AnimateChips(list_RunInfo[idx].GetComponent<item_one_runinfo>().txt_Level, max_level, 0.6f));

		Time.timeScale = 0f;
	}

	IEnumerator AnimateChips(TextMeshProUGUI txt, int targetValue, float duration)
	{
		string originalText = txt.text;
		Match match = Regex.Match(originalText, @"\d+");

		if (!match.Success)
		{
			Debug.LogWarning("텍스트에 숫자가 없어요! : " + originalText);
			yield break;
		}

		int startValue = int.Parse(match.Value);
		float elapsed = 0f;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float t = Mathf.Clamp01(elapsed / duration);
			int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, t));

			// 숫자 부분만 대체
			txt.text = Regex.Replace(originalText, @"\d+", currentValue.ToString());
			yield return null;
		}

		// 정확히 맞춰주기
		txt.text = Regex.Replace(originalText, @"\d+", targetValue.ToString());
	}

	void SetLayerRecursively(GameObject obj, int layer)
	{
		obj.layer = layer;

		foreach (Transform child in obj.transform)
		{
			SetLayerRecursively(child.gameObject, layer);
		}
	}

	void SetParticleSortingLayer(GameObject parent)
	{
		string targetLayerName = "DIALOG";
	
		ParticleSystemRenderer[] renderers = parent.GetComponentsInChildren<ParticleSystemRenderer>(true);

		foreach (var renderer in renderers)
		{
			renderer.sortingLayerName = targetLayerName;
		}
	}

	private void Start()
	{
		GetComponentInChildren<Canvas>().worldCamera = GameObject.Find("Camera_Card").GetComponent<Camera>();

		
		//RefreshText();
	}

	public void Exit()
	{
		gameObject.SetActive(false);
		Time.timeScale = 1f;
	}

	private void OnDisable()
	{
		UI_Card.instance.is_Dlg_Open = false;
	}
}
