using UnityEngine;
using TMPro;

public class UI_AutoPlay : MonoBehaviour
{
	public TextMeshProUGUI txt_AutoPlay;

	public GameObject[] list_Grade;

	private void Start()
	{
		RefreshText();
		RefreshList();
	}

	private void OnEnable()
	{
		RefreshText();
		RefreshList();
	}

	void RefreshList()
	{
		if (CardDataManager.instance)
			foreach (GameObject go in list_Grade)
			{
				go.SetActive(CardDataManager.instance.is_AutoPlay);
			}
	}

	void RefreshText()
	{
		if (CardDataManager.instance && txt_AutoPlay)
		{
			if (CardDataManager.instance.is_AutoPlay)
				txt_AutoPlay.text = "자동플레이";
			else
				txt_AutoPlay.text = "수동플레이";
		}
	}

	public void Btn_AutoPlay()
	{
		if (CardDataManager.instance)
		{
			CardDataManager.instance.is_AutoPlay = !CardDataManager.instance.is_AutoPlay;

			RefreshText();
			RefreshList();
		}
	}
}
