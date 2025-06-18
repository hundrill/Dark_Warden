using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VideoPokerKit;

public class UI_Tarot : MonoBehaviour
{
	public List<GameObject> list_Tarot_Reward = new List<GameObject>();

	public List<GameObject> list_Select;
	public GameObject item_Card;
	public Button btn_Exit;
	public Button btn_Use;
	public static UI_Tarot instance;
	public TAROT tarot_Select;
	int price_Select;
	int type_Select;

	public GameObject description_Tarot;

	private void Awake()
	{
		/*if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}*/


	}

	private void OnEnable()
	{
		if (UI_Card.instance)
			UI_Card.instance.is_Dlg_Open = true;

		if (instance == null)
		{
			instance = this;
		}

		description_Tarot.SetActive(true);

		//Setting_Select_Card(5);
		//Setting_Select_Card(UI_Reward.instance.tarot_Select.draw_count);
		Setting_Select_Card(UI_Reward.instance.tarot_Select.draw_count);

		//Setting_Select_Card(7);

		Setting_Tarot(true, 1);

		num_Select_Card = 0;

		if (btn_Exit)
			btn_Exit.gameObject.SetActive(false);

		bool is_Direct_Start = false;

		if (UI_Reward.instance.tarot_Select.list_Condition.Count > 0 && UI_Reward.instance.tarot_Select.list_Condition[0].todo.Length > 2)
		{
			string con1 = UI_Reward.instance.tarot_Select.list_Condition[0].todo[1].ToLower();
			string con2 = UI_Reward.instance.tarot_Select.list_Condition[0].todo[2].ToLower();
			is_Direct_Start = con1.Contains("change") && con2.Contains("random");

			if (btn_Use)
			{
				btn_Use.gameObject.SetActive(is_Direct_Start ? false : true);
				btn_Use.interactable = false;

				if (is_Direct_Start == false && UI_Reward.instance.tarot_Select.draw_count == 0)
				{
					btn_Use.gameObject.SetActive(true);
					btn_Use.interactable = true;
				}
			}

			if (is_Direct_Start)
			{
				//AutoSelect(5);
				StartEvent(0, 0, 0, 0, UI_Reward.instance.tarot_Select.random_count);
			}
		}

		if (!is_Direct_Start)
			if (btn_Use)
				btn_Use.gameObject.SetActive(true);
	}

	void AutoSelect(int num)
	{
		// 먼저 선택 여부를 초기화
		foreach (var card in list_Select)
		{
			var cardComp = card.GetComponent<Card>();
			if (cardComp != null)
			{
				cardComp.cardData.is_Select = false;
			}
		}

		// 카드 리스트를 섞고, 앞에서부터 num개 선택
		List<GameObject> shuffled = new List<GameObject>(list_Select);
		for (int i = 0; i < shuffled.Count; i++)
		{
			int rand = Random.Range(i, shuffled.Count);
			(shuffled[i], shuffled[rand]) = (shuffled[rand], shuffled[i]); // Fisher-Yates shuffle
		}

		// num개만 선택
		for (int i = 0; i < Mathf.Min(num, shuffled.Count); i++)
		{
			var cardComp = shuffled[i].GetComponent<Card>();
			if (cardComp != null)
			{
				cardComp.cardData.is_Select = true;
				// 필요한 경우 여기서 추가로 선택 연출 등을 넣을 수 있음
			}
		}
	}

	void Setting_Tarot(bool show, int num = 3)
	{
		if (show == false)
		{
			for (int i = 0; i < list_Tarot_Reward.Count; i++)
			{
				list_Tarot_Reward[i].SetActive(false);
			}
			return;
		}

		/*btn_Buy.interactable = false;
		if (UI_Main.instance)
			btn_Buy.image.material = UI_Main.instance.grayMaterial;*/

		if (DataManager.instance)
		{
			float width_one = 0.5f;
			float width_gap = 0.05f;

			float startx = 1.1f;

			for (int i = list_Tarot_Reward.Count - 1; i >= num; i--)
			{
				list_Tarot_Reward[i].SetActive(false);
			}

			TAROT[] list_Temp = JokerManager.instance.GetTarotType_Reward(num);

			if (list_Temp.Length == 0)
				return;

			if (num > list_Temp.Length)
				num = list_Temp.Length;

			for (int i = 0; i < num; i++)
			{
				float pos = startx + i * (width_one + width_gap);
				Vector3 newPos = new Vector3(pos, 0.60f, -1);

				if (i < list_Tarot_Reward.Count)
				{
					list_Tarot_Reward[i].SetActive(true);
					list_Tarot_Reward[i].transform.position = newPos;
					list_Tarot_Reward[i].GetComponent<Card_Tarot>().SetTarotType(list_Temp[i]);
				}
				else if (JokerManager.instance.item_tarot)
				{
					GameObject item = Instantiate(JokerManager.instance.item_tarot, newPos, Quaternion.identity, gameObject.transform);

					item.GetComponent<Card_Tarot>().OnSelect += OnSelect;
					item.GetComponent<Card_Tarot>().OnTouched += OnTouched;
					//item.GetComponent<Card_Tarot>().SetTarotType(list_Temp[i]);
					item.GetComponent<Card_Tarot>().SetTarotType(UI_Reward.instance.tarot_Select);
					//item.GetComponent<Card_Tarot>().SetTarotType(TAROT.type_9);
					item.GetComponent<Card_Tarot>().SetOrderLayer(3);
					item.GetComponent<Card_Tarot>().SetShowType(SHOWTYPE.UI);
					foreach (SpriteRenderer sr in item.GetComponentsInChildren<SpriteRenderer>(true))
					{
						sr.sortingLayerName = "DIALOG"; // 원하는 레이어 이름			
						sr.sortingOrder = 2;
					}

					foreach (Canvas canvas in item.GetComponentsInChildren<Canvas>(true))
					{
						canvas.sortingLayerName = "DIALOG";
						canvas.sortingOrder = 2;
					}

					/*SetLayerRecursively(item, LayerMask.NameToLayer("DIALOG"));
					SetSpriteRendererSortingLayer(item);*/

					list_Tarot_Reward.Add(item);

					item.GetComponent<Card_Tarot>().ShowDesc(true);
					item.GetComponent<Card_Tarot>().OnSelect.Invoke(item.GetComponent<Card_Tarot>().type, item.GetComponent<Card_Tarot>().price_Joker);
				}
			}
		}
	}

	void OnTouched(TAROT joker)
	{
		//Off_Desc_Ohters();
	}

	void OnSelect(TAROT joker, int price)
	{
		tarot_Select = joker;
		price_Select = price;

		/*int mine = CardDataManager.instance.GetMyData(CARDDATA.DOLLAR);

		if (mine >= price)
		{
			is_Select = true;
			tarot_Select = joker;
			price_Select = price;
			btn_Buy.interactable = true;
			btn_Buy.image.material = null;
			type_Select = 2;
			//StageManager.instance.state = STAGESTATE.REWARD_FINISH;
			//ShowReward(false);
		}
		else
		{
			is_Select = true;
			tarot_Select = joker;
			price_Select = price;
			btn_Buy.interactable = false;
			btn_Buy.image.material = UI_Main.instance.grayMaterial;
		}*/
	}

	void Setting_Select_Card(int num)
	{
		list_Select.Clear();

		for (int i = 0; i < num; i++)
		{
			GameObject newCard = Instantiate(item_Card);
			var newCardData = CardsManager.the.GetNewCardFromTheDeck();

			float width_one = 0.5f;
			float default_gap = 0.05f;
			float totalWidthAvailable = 3.5f;

			float totalWidth = num * width_one + (num - 1) * default_gap;

			float width_gap;

			// 폭을 넘지 않으면 기본 간격 유지, 넘으면 자동 계산
			if (totalWidth <= totalWidthAvailable)
			{
				width_gap = default_gap;
			}
			else
			{
				width_gap = (totalWidthAvailable - (num * width_one)) / (num - 1f);
			}

			totalWidth = num * width_one + (num - 1) * width_gap;
			float startx = -totalWidth / 2f + width_one / 2f + 1.1f;

			float posX = startx + i * (width_one + width_gap);
			Vector3 newPos = new Vector3(posX, 1.4f, -1);

			Card cardComponent = newCard.GetComponent<Card>();
			if (cardComponent)
			{
				cardComponent.SetLibraryCard(newCardData);
				cardComponent.Copy_From_Library();
				cardComponent.SetTargetPos(newPos);
				cardComponent.transform.position = newPos;
				cardComponent.ResetY();
				cardComponent.InitScale();
				cardComponent.OnSelect += OnSelect;
				cardComponent.showType = SHOWTYPE.UI;
				StartCoroutine(cardComponent.Set_Ani("CardIdle"));
				SetLayerRecursively(newCard, LayerMask.NameToLayer("DIALOG"));
				SetSpriteRendererSortingLayer(newCard, 3 + i);
			}

			list_Select.Add(newCard);
		}
	}

	public void RepositionCard()
	{
		int num = list_Select.Count;

		if (num == 0)
			return;

		float width_one = 0.5f;
		float default_gap = 0.05f;
		float totalWidthAvailable = 4f;

		float totalWidth = num * width_one + (num - 1) * default_gap;
		float width_gap = (totalWidth <= totalWidthAvailable)
			? default_gap
			: (totalWidthAvailable - (num * width_one)) / (num - 1f);

		totalWidth = num * width_one + (num - 1) * width_gap;
		float startx = -totalWidth / 2f + width_one / 2f + 1.1f;

		for (int i = 0; i < num; i++)
		{
			GameObject card = list_Select[i];
			Vector3 newPos = new Vector3(startx + i * (width_one + width_gap), card.transform.position.y, -1);

			Card cardComponent = card.GetComponent<Card>();
			if (cardComponent)
			{
				cardComponent.objectStartPos = new Vector3(newPos.x, 1.4f, newPos.z);
				cardComponent.transform.position = newPos;

				//cardComponent.SetTargetPos(newPos);

				//cardComponent.ResetY();
				//cardComponent.InitScale();

				cardComponent.OnSelect += OnSelect;
				cardComponent.showType = SHOWTYPE.UI;
				StartCoroutine(cardComponent.Set_Ani("CardIdle"));
				SetLayerRecursively(card, LayerMask.NameToLayer("DIALOG"));

				SetSpriteRendererSortingLayer(card, 3 + i);

				cardComponent.initialY = 1.4f;
			}
		}
	}

	int _num_Select_Card;

	public int num_Select_Card
	{
		get { return _num_Select_Card; }
		set
		{
			_num_Select_Card = value;

			if (list_Tarot_Reward.Count > 0 && list_Tarot_Reward[0])
			{
				if (_num_Select_Card > 0)
				{
					list_Tarot_Reward[0].GetComponent<Card_Tarot>().Set_Btn_Use(true);
					btn_Use.interactable = true;
				}
				else
				{
					list_Tarot_Reward[0].GetComponent<Card_Tarot>().Set_Btn_Use(false);
					btn_Use.interactable = false;
				}
			}
		}
	}

	void OnSelect(bool select)
	{
		num_Select_Card += select ? 1 : -1;
	}

	public void StartEvent(int idx, int max_chip, int max_mult, int max_level, int auto_select_num = 0)
	{
		StartCoroutine(TarotEvent(idx, max_chip, max_mult, max_level, auto_select_num));
	}

	IEnumerator TarotEvent(int idx, int max_chip, int max_mult, int max_level, int auto_select_num = 0)
	{/*
		if (btn_Use)
			btn_Use.gameObject.SetActive(true);
*/
		if (auto_select_num > 0)
		{
			// 먼저 선택 여부를 초기화
			foreach (var card in list_Select)
			{
				var cardComp = card.GetComponent<Card>();
				if (cardComp != null)
				{
					cardComp.cardData.is_Select = false;
				}
			}

			// 카드 리스트를 섞고, 앞에서부터 num개 선택
			List<GameObject> shuffled = new List<GameObject>(list_Select);
			for (int i = 0; i < shuffled.Count; i++)
			{
				int rand = Random.Range(i, shuffled.Count);
				(shuffled[i], shuffled[rand]) = (shuffled[rand], shuffled[i]); // Fisher-Yates shuffle
			}

			yield return new WaitForSeconds(1f);

			// num개만 선택
			for (int i = 0; i < Mathf.Min(auto_select_num, shuffled.Count); i++)
			{
				var cardComp = shuffled[i].GetComponent<Card>();
				if (cardComp != null)
				{
					cardComp.Auto_MouseDown();
					yield return new WaitForSeconds(0.01f);
					cardComp.Auto_MouseUp();
					yield return new WaitForSeconds(0.1f);
					//cardComp.cardData.is_Select = true;
					// 필요한 경우 여기서 추가로 선택 연출 등을 넣을 수 있음
				}
			}
		}

		string con_2 = "";

		string con_0 = UI_Reward.instance.tarot_Select.list_Condition[0].todo[0].ToLower();
		string con_1 = UI_Reward.instance.tarot_Select.list_Condition[0].todo[1].ToLower();

		if (UI_Reward.instance.tarot_Select.list_Condition[0].todo.Length > 2)
			con_2 = UI_Reward.instance.tarot_Select.list_Condition[0].todo[2].ToLower();

		if (btn_Exit)
			btn_Exit.gameObject.SetActive(false);

		if (btn_Use)
			btn_Use.gameObject.SetActive(false);

		//btn_Exit.interactable = false;

		GameObject eff = EffectManager.instance.CreateEffect(2, list_Tarot_Reward[0].transform.position);
		eff.transform.localScale = new Vector3(0.18f, 0.20f, 1);
		SetLayerRecursively(eff, LayerMask.NameToLayer("DIALOG"));
		SetParticleSortingLayer(eff);

		yield return new WaitForSeconds(0.5f);

		foreach (var obj in list_Tarot_Reward)
		{
			if (obj != null)
			{
				Destroy(obj);
			}
		}

		description_Tarot.SetActive(false);

		bool no_Card_Rotate = con_0.Contains("card") && con_1.Contains("delete");
		bool is_Create_Planet = con_0.Contains("create") && con_1.Contains("planet");

		if (!no_Card_Rotate)
		{
			foreach (var card in list_Select)
			{
				var cardComp = card.GetComponent<Card>();
				if (cardComp && cardComp.cardData.is_Select)
				{
					StartCoroutine(RotateCard(card.transform, true));
				}
			}
		}

		yield return new WaitForSeconds(0.5f);

		CardType randomType = CardType.TYPES_NO;

		int count = list_Select.Count;

		int selectedCount = list_Select
			.Count(card => card.GetComponent<Card>()?.cardData.is_Select == true);
		int randomIndex = UnityEngine.Random.Range(0, selectedCount);
		int active = 0;

		for (int i = 0; i < count; i++)
		{
			var card = list_Select[i]; // 현재 카드
			var cardComp = card.GetComponent<Card>();
			if (cardComp && cardComp.cardData.is_Select)
			{
				int value = JokerManager.instance.String_To_Int(UI_Reward.instance.tarot_Select.functionvalue.ToLower());

				if (con_0.Contains("suite"))
				{
					if (randomType == CardType.TYPES_NO)
						randomType = JokerManager.instance.String_To_CardType(con_2);

					CardsManager.the.ChangeType(cardComp.cardData, randomType);
					cardComp.cardData.is_Select = true;
					cardComp.cardData.ChangeType(randomType);
					cardComp.UpdateCardFaceSprite();
				}
				else if (con_0.Contains("number"))
				{
					CardValue to_CardValue = cardComp.cardData.value;
					CALC calc = JokerManager.instance.String_To_Calc(con_1);

					to_CardValue = JokerManager.instance.CalcValue(to_CardValue, calc, value);
					CardsManager.the.ChangeValue(cardComp.cardData, to_CardValue);
					cardComp.cardData.is_Select = true;
					cardComp.cardData.ChangeValue(to_CardValue);
					cardComp.UpdateCardFaceSprite();
				}
				else if (con_0.Contains("card") && con_1.Contains("delete"))
				{
					CardsManager.the.DeleteCardFromDeck(cardComp.cardData);
					Destroy(card);
				}
				else if (con_0.Contains("card") && con_1.Contains("copy"))
				{
					if (randomIndex == active)
					{
						// 카드 복사
						GameObject copyCard = Instantiate(card, card.transform.parent);

						CardsManager.the.AddCardToDeck(cardComp.cardData);
						list_Select.Add(copyCard);
						yield return new WaitForEndOfFrame();

						RepositionCard();

						yield return new WaitForSeconds(1f);
					}
				}
				else if (con_0.Contains("create"))
				{
					if (randomType == CardType.TYPES_NO)
						randomType = JokerManager.instance.String_To_CardType(con_2);

					CardsManager.the.ChangeType(cardComp.cardData, randomType);
					cardComp.cardData.is_Select = true;
					cardComp.cardData.ChangeType(randomType);
					cardComp.UpdateCardFaceSprite();
				}

				/*if (!no_Card_Rotate)
					StartCoroutine(RotateCard(card.transform, false));*/

				active++;

			}
		}

		if (!no_Card_Rotate)
		{
			foreach (var card in list_Select)
			{
				var cardComp = card.GetComponent<Card>();
				if (cardComp && cardComp.cardData.is_Select)
				{
					StartCoroutine(RotateCard(card.transform, false));
				}
			}
		}

		if (is_Create_Planet == false)
		{
			yield return new WaitForSeconds(1.0f);

			num_Select_Card = 0;

			foreach (var card in list_Select)
			{
				if (card == null) continue;

				var cardComp = card.GetComponent<Card>();
				if (cardComp && cardComp.cardData.is_Select)
				{
					cardComp.CardSelect();
				}
			}

			yield return new WaitForSeconds(0.7f);

			//foreach (var card in list_Select)
			for (int i = list_Select.Count - 1; i >= 0; i--)
			{
				if (list_Select[i] == null) continue;

				GameObject card = list_Select[i];
				var cardComp = card.GetComponent<Card>();
				if (cardComp)
				{
					cardComp.SetTargetPos(new Vector3(3.78f, -0.582f, 0), 0.15f);
					StartCoroutine(RotateCard(card.transform, true, 0.1f)); // 앞으로 뒤집기
					yield return new WaitForSeconds(0.1f);
				}
			}

			yield return new WaitForSeconds(0.3f);

			foreach (var obj in list_Select)
			{
				if (obj != null)
				{
					Destroy(obj);
				}
			}
		}

		if (is_Create_Planet)
		{
			PLANET[] allPlanets = (PLANET[])System.Enum.GetValues(typeof(PLANET));
			int select = UnityEngine.Random.Range(0, allPlanets.Length);
			JokerManager.instance.Start_Planet_Event(allPlanets[select]);
			JokerManager.instance.UsePlanet(allPlanets[select], true);
		}

		FinishEvent();
	}

	void FinishEvent()
	{
		list_Select.Clear();
		list_Tarot_Reward.Clear();

		gameObject.SetActive(false);
	}

	IEnumerator RotateCard(Transform cardTransform, bool toFront, float _duration = 0.3f)
	{
		Quaternion startRot = cardTransform.rotation;
		Quaternion endRot;

		if (toFront)
			endRot = Quaternion.Euler(0, 180, 0); // 앞면
		else
			endRot = Quaternion.Euler(0, 0, 0);   // 뒷면

		float time = 0f;
		float duration = _duration; // 회전 시간

		while (time < duration)
		{
			time += Time.deltaTime;
			float t = time / duration;
			cardTransform.rotation = Quaternion.Lerp(startRot, endRot, t);
			yield return null;
		}

		cardTransform.rotation = endRot; // 정확하게 맞춤
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
			renderer.sortingOrder = 3;
		}
	}

	void SetSpriteRendererSortingLayer(GameObject parent, int order = 0)
	{
		string targetLayerName = "DIALOG";

		SpriteRenderer[] renderers = parent.GetComponentsInChildren<SpriteRenderer>(true);

		foreach (var renderer in renderers)
		{
			renderer.sortingLayerName = targetLayerName;

			renderer.sortingOrder = order == 0 ? 3 : order;
		}
	}

	private void Start()
	{
		GetComponentInChildren<Canvas>().worldCamera = GameObject.Find("Camera_Card").GetComponent<Camera>();


		//RefreshText();
	}

	public void Use()
	{
		if (list_Tarot_Reward.Count > 0 && list_Tarot_Reward[0])
		{
			list_Tarot_Reward[0].GetComponent<Card_Tarot>().Btn_Use();
			btn_Use.gameObject.SetActive(false);

			if (SoundManager.instance)
				SoundManager.instance.PlaySound("Card_Buy");
		}
	}

	public void Exit()
	{
		foreach (var obj in list_Tarot_Reward)
		{
			if (obj != null)
			{
				Destroy(obj);
			}
		}

		foreach (var obj in list_Select)
		{
			if (obj != null)
			{
				Destroy(obj);
			}
		}

		FinishEvent();
		Time.timeScale = 1f;
	}

	private void OnDisable()
	{
		UI_Card.instance.is_Dlg_Open = false;
	}
}
