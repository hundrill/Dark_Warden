using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;
using VideoPokerKit;

public class UI_DeckInfo : MonoBehaviour
{
	public Image sprite_Remain;
	public Image sprite_Full;

	public TextMeshProUGUI[] txt_Num_Suite;
	public TextMeshProUGUI[] txt_Num_Rank;

	public GameObject base_Suite;

	List<CardData>[] separatedLists = new List<CardData>[4];

	private void Start()
	{
		GetComponentInChildren<Canvas>().worldCamera = GameObject.Find("Camera_Card").GetComponent<Camera>();
	}

	private void OnEnable()
	{
		if (UI_Card.instance)
			UI_Card.instance.is_Dlg_Open = true;

		is_Remain_Deck = true;
		sprite_Remain.color = !is_Remain_Deck ? Color.gray : Color.red;
		sprite_Full.color = is_Remain_Deck ? Color.gray : Color.red;


		ClearOldCards();
		ParseCards();
		SettingCard();
		SettingNum();
	}

	void SettingNum()
	{
		int[] num_Suite = GetNum_Card_Suite();

		for (int i = 0; i < txt_Num_Suite.Length; i++)
			txt_Num_Suite[i].text = string.Format("{0}", num_Suite[i]);

		int[] num_Rank = GetNum_Card_Rank();

		for (int i = 0; i < num_Rank.Length; i++)
			txt_Num_Rank[i].text = string.Format("{0}", num_Rank[i]);
	}

	int[] GetNum_Card_Suite()
	{
		int[] num = new int[txt_Num_Suite.Length];

		foreach (var card in CardsManager.the.cardLibrary)
		{
			if (is_Remain_Deck)
				if (card.dealt)
					continue;

			switch (card.value)
			{
				case CardValue.VALUE_A:
					num[0]++;
					break;

				case CardValue.VALUE_K:
				case CardValue.VALUE_Q:
				case CardValue.VALUE_J:
					num[1]++;
					break;

				default:
					num[2]++;
					break;
			}
		}

		for (int i = 0; i < separatedLists.Length; i++)
		{
			foreach (var card in separatedLists[i])
			{
				if (is_Remain_Deck)
					if (card.dealt)
						continue;

				num[3 + i]++;
			}
		}

		return num;
	}

	int[] GetNum_Card_Rank()
	{
		int[] num = new int[txt_Num_Rank.Length];

		foreach (var card in CardsManager.the.cardLibrary)
		{
			if (is_Remain_Deck)
				if (card.dealt)
					continue;

			int idx = txt_Num_Rank.Length - (int)card.value - 1;
			if (num.Length > idx)
				num[idx]++;
		}

		return num;
	}

	void ClearOldCards()
	{
		foreach (Transform child in base_Suite.transform)
		{
			Destroy(child.gameObject);
		}
	}

	void ParseCards()
	{
		// 리스트 배열 초기화
		for (int i = 0; i < 4; i++)
		{
			separatedLists[i] = new List<CardData>();
		}

		// 카드 분류
		foreach (var card in CardsManager.the.cardLibrary)
		{
			int typeIndex = (int)card.type;
			separatedLists[typeIndex].Add(card);
		}

		// 각 리스트를 value 기준으로 내림차순 정렬 (A, K, Q, ... 2)
		for (int i = 0; i < 4; i++)
		{
			separatedLists[i] = separatedLists[i]
				.OrderByDescending(card => card.value)
				.ToList();
		}
	}

	bool is_Remain_Deck;

	public void RemainDeck()
	{
		is_Remain_Deck = true;
		sprite_Remain.color = !is_Remain_Deck ? Color.gray : Color.red;
		sprite_Full.color = is_Remain_Deck ? Color.gray : Color.red;

		ClearOldCards();
		ParseCards();
		SettingCard();
		SettingNum();
	}

	public void FullDeck()
	{
		is_Remain_Deck = false;
		sprite_Remain.color = !is_Remain_Deck ? Color.gray : Color.red;
		sprite_Full.color = is_Remain_Deck ? Color.gray : Color.red;

		ClearOldCards();
		ParseCards();
		SettingCard();
		SettingNum();
	}

	float startX = 0.08f;       // 왼쪽 정렬 시작 위치
	float startY = 1.63f;
	float yGap = 0.55f;
	float cardScale = 170f;

	void SettingCard()
	{
		float maxWidth = 4.2f;

		int total = 0;

		for (int i = 0; i < separatedLists.Length; i++)
		{
			int count = separatedLists[i].Count;
			if (count == 0) continue;

			float spacing = 0f;

			if (count == 1)
				spacing = 0f;
			else
				spacing = (maxWidth - count) / (count - 1);

			for (int j = 0; j < count; j++)
			{
				float x = startX + j * (1f + spacing);  // 카드 1 너비 + 간격
				Vector3 pos = new Vector3(x, startY - i * yGap, 0);

				GameObject card = Instantiate(JokerManager.instance.item_card, pos, Quaternion.identity, base_Suite.transform);
				card.transform.localScale = new Vector3(cardScale, cardScale, 0);
				SetSpriteRendererSortingLayer(card, total++);
				
				var data = separatedLists[i][j];
				card.GetComponent<Card_Deck>().cardData.ChangeType(data.type, data.value);
				card.GetComponent<Card_Deck>().UpdateCardFaceSprite();
				card.GetComponent<Card_Deck>().SetChipMultText(data.chip_growth, data.mult_growth);


				if (is_Remain_Deck == true)
				{
					// 반투명 처리
					SpriteRenderer sr = card.GetComponent<Card_Deck>().cardFaceSprite;
					if (data.dealt)
					{
						sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.4f); // 40% 투명도
					}
					else
					{
						sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f); // 완전 불투명
					}
				}
			}
		}
	}

	void SetSpriteRendererSortingLayer(GameObject parent, int order)
	{
		string targetLayerName = "DIALOG";

		SpriteRenderer[] renderers = parent.GetComponentsInChildren<SpriteRenderer>(true);

		foreach (var renderer in renderers)
		{
			renderer.sortingLayerName = targetLayerName;
			renderer.sortingOrder = 3 + order;
		}

		TextMeshPro[] texts = parent.GetComponentsInChildren<TextMeshPro>(true);

		foreach (var text in texts)
		{
			text.sortingLayerID = SortingLayer.NameToID(targetLayerName);
			text.sortingOrder = 3 + order + 1;
		}
	}

	void SetLayerRecursively(GameObject obj, int layer)
	{
		obj.layer = layer;
		foreach (Transform child in obj.transform)
		{
			SetLayerRecursively(child.gameObject, layer);
		}
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
