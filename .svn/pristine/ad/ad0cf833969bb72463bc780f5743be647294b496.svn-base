using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using VideoPokerKit;
using static CardDataManager;
using static UI_Main;

public class UI_Grade : MonoBehaviour
{
	public GameObject Content;
	public GameObject baseItem;

	public delegate void onGradeSelect(Wins type);
	public onGradeSelect OnGradeSelect;

	private void OnEnable()
	{
		CardDataManager.OnEvaluateChange += OnEvaluateChange;

		//StartCoroutine(Setting()); //작은 ui 안하는걸로 변경
	}

	IEnumerator Setting()
	{
		while (true)
		{
			if (Paytable.the)
			{
				InitScrollContents();
				break;
			}
			else
				yield return null;
		}
	}

	public void OnEvaluateChange(Wins type)
	{
		OnGradeSelect?.Invoke(type);
	}

	void InitScrollContents()
	{
		ClearChildren(Content);

		for (int i = 0; i < 9; i++)
		{
			GameObject item = Instantiate(baseItem, new Vector3(0, 0, 0), Quaternion.identity);
			item.transform.SetParent(Content.transform);
			item.transform.localScale = new Vector3(1, 1, 1);
			item.transform.localPosition = new Vector3(0, 0, 0);
			//item.GetComponent<Image>().sprite = list_Card[i % list_Card.Length];
			item.GetComponent<UI_One_Grade>().SetWinsType(Wins.WIN_STRAIGHT_FLUSH - i);

		}


		UI_One_Grade[] objectsWithAScript = FindObjectsByType<UI_One_Grade>(FindObjectsSortMode.None);

		foreach (UI_One_Grade a in objectsWithAScript)
		{
			OnGradeSelect += a.OnGradeSelect;
		}

		OnGradeSelect?.Invoke(Wins.WINS_NO);
	}

	public void ClearChildren(GameObject parent)
	{
		while (true)
		{
			if (parent.transform.childCount > 0)
			{
				var child = parent.transform.GetChild(0);

				if (child == null)
					break;

				//child.gameObject.GetComponent<UI_Icon_Summon>().OnItemSelect -= OnSelect;
				UnityEngine.Object.DestroyImmediate(child.gameObject);
			}
			else
				break;
		}
	}
}
