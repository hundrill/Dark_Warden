using UnityEngine;
using VideoPokerKit;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class Card_Tarot : MonoBehaviour
{
	//--------------------------------------------------
	public SHOWTYPE showType;
	public GameObject canvas;
	public GameObject card;
	public GameObject desc;
	public GameObject price;
	public GameObject btn_Use;
	public TextMeshProUGUI txt_Name;
	public TextMeshProUGUI txt_Desc;
	public TextMeshProUGUI txt_Price;
	public int price_Joker;
	float time_Click;
	public SpriteRenderer sprite_Joker;
	bool is_Touched = false;
	bool is_Dragging = false;

	float initialX;
	float initialY;
	float initialZ;
	Vector3 dragStartPos;
	Vector3 objectStartPos;
	Vector3 updateStartPos;
	Vector3 targetPos;

	float offset_Z = -1;
	private Vector3 originalScale;

	public delegate void onSelect(TAROT type, int price);
	public onSelect OnSelect;

	public delegate void onTouched(TAROT type);
	public onTouched OnTouched;

	[HideInInspector]
	public TAROT type;

	public void OnEnable()
	{
		ShowDesc(false);

		GetComponentInChildren<Canvas>().worldCamera = GameObject.Find("Camera_Card").GetComponent<Camera>();
	}

	void SettingPrice(TAROT tarot)
	{
		price_Joker = tarot.price;

		if (txt_Price)
			txt_Price.text = string.Format("${0}", price_Joker);
	}

	public void Set_Btn_Use(bool interactable)
	{
		btn_Use.GetComponent<Button>().interactable = interactable;
	}

	public void Btn_Use()
	{
		TAROTINFO info = JokerManager.instance.list_TarotInfo.FirstOrDefault(p => p.type.id == type.id);

		int chips = info.chips + Paytable.the.GetChips(info.wintype);

		int mult = info.mult + Paytable.the.GetWinMultiplier(info.wintype);

		int level = Paytable.the.GetLevel(info.wintype) + 2;

		//UI_Card.instance.Runinfo_Event(8 - (int)info.wintype, chips, mult, level);
		UI_Card.instance.Dlg_Tarot.GetComponent<UI_Tarot>().StartEvent(0, 0, 0, 0);

		JokerManager.instance.UseTarot(type);
	}

	void SettingDesc(bool show)
	{
		if (showType == SHOWTYPE.PLAY)
		{
			if (UI_Card.instance && UI_Card.instance.description_TarotPlanet)
				UI_Card.instance.description_TarotPlanet.SetActive(show);
		}
		else
		{
			if (UI_Tarot.instance && UI_Tarot.instance.description_Tarot && UI_Tarot.instance.description_Tarot.activeSelf)
				UI_Tarot.instance.description_Tarot.SetActive(show);
			else if (UI_Reward.instance && UI_Reward.instance.description_Reward)
				UI_Reward.instance.description_Reward.SetActive(show);
		}

		if (show == false)
			return;

		if (txt_Name)
			txt_Name.text = type.name;

		if (txt_Desc)
			txt_Desc.text = type.description;

		if (showType == SHOWTYPE.PLAY)
		{
			UI_Card.instance.description_TarotPlanet.GetComponent<Description>().SetDesc(gameObject, type.name, type.description, "일반");
		}
		else if (showType == SHOWTYPE.UI)
		{
			if (UI_Tarot.instance && UI_Tarot.instance.description_Tarot && UI_Tarot.instance.description_Tarot.activeSelf)
			{
				UI_Tarot.instance.description_Tarot.SetActive(true);
				UI_Tarot.instance.description_Tarot.GetComponent<Description>().SetDesc(gameObject, type.name, type.description, "일반");
			}
			if (UI_Reward.instance && UI_Reward.instance.description_Reward)
			{
				UI_Reward.instance.description_Reward.SetActive(true);
				UI_Reward.instance.description_Reward.GetComponent<Description>().SetDesc(gameObject, type.name, type.description, "일반");
			}
		}
	}

	public void SetShowType(SHOWTYPE type)
	{
		showType = type;

		if (showType == SHOWTYPE.UI)
		{
			ShowPrice(true);
			ShowUse(false);
		}
		else
		{
			ShowPrice(false);
			ShowUse(true);
		}
	}

	public void SetOrderLayer(int layer)
	{
		gameObject.GetComponent<SpriteRenderer>().sortingOrder = layer;
		canvas.GetComponent<Canvas>().sortingOrder = layer;
	}


	public void ShowUse(bool show)
	{
		show = false;
		btn_Use.SetActive(show);
	}

	public void ShowPrice(bool show)
	{
		price.SetActive(show);
	}

	public void ShowDesc(bool show)
	{
		SettingDesc(show);

		show = false;

		desc.SetActive(show);
	}

	public void SetTarotType(TAROT _type)
	{
		type = _type;
		SettingPrice(type);
		if (sprite_Joker && CardDataManager.instance && CardDataManager.instance.img_Tarot.Length > _type.sprite)
			sprite_Joker.sprite = CardDataManager.instance.img_Tarot[_type.sprite];
	}

	public void Btn_Select()
	{
		/*if (showType == SHOWTYPE.PLAY)
			ShowUse(!btn_Use.activeSelf);*/

		OnSelect?.Invoke(type, price_Joker);
	}


	private void Start()
	{
		originalScale = transform.localScale;
	}

	void OnMouseDown()
	{
		if (Time.timeScale == 0)
			return;

		if (!MainGame.the.Waiting_User_Input())
			return;

		OnTouched?.Invoke(type);

		/*if (cardData.is_Select == false)
			if (CardsManager.the.Count_Num_Select() >= 5)
			{
				is_Touched = false;
				return;
			}*/

		is_Touched = true;

		transform.localScale = originalScale * 1.1f;

		time_Click = Time.time;

		Camera cam = GameObject.Find("Camera_Card").GetComponent<Camera>();


		objectStartPos = transform.position;

		// 카메라 방향으로 offset_Z 만큼 이동
		transform.position += cam.transform.forward * offset_Z;

		updateStartPos = transform.position;

		// 드래그 시작 시 터치 위치와 오브젝트 위치 저장
		dragStartPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.WorldToScreenPoint(transform.position).z));


	}

	Vector3 lastpos;
	void OnMouseDrag()
	{
		if (StageManager.instance && StageManager.instance.state == STAGESTATE.REWARD_START)
			return;

		if (!is_Touched)
			return;

		is_Dragging = true;

		Camera cam = GameObject.Find("Camera_Card").GetComponent<Camera>();
		Vector3 currentMousePos = cam.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.WorldToScreenPoint(transform.position).z));
		Vector3 delta = currentMousePos - dragStartPos; // 드래그 시작점 대비 변화량
		Vector3 newPosition = updateStartPos + delta;


		/*
		// 현재 이동 방향 확인
		bool movingRight = newPosition.x > objectStartPos.x;

		var sortedCards = movingRight
			? CardsManager.the.GetAllCards().OrderBy(c => c.transform.position.x).ToList() // 오른쪽 이동: 오름차순 정렬
			: CardsManager.the.GetAllCards().OrderByDescending(c => c.transform.position.x).ToList(); // 왼쪽 이동: 내림차순 정렬

		foreach (var card in sortedCards)
		{
			if (card != this) // Skip the dragged card itself
			{
				float newX = card.GetPositionX();

				if (movingRight && newPosition.x > newX && objectStartPos.x < newX)
				{
					card.SetTargetPos(new Vector3(objectStartPos.x, card.transform.position.y, card.transform.position.z));
					objectStartPos.x = newX;
					dragStartPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.WorldToScreenPoint(newPosition).z));
					updateStartPos = newPosition;
					targetPos = objectStartPos;
					break;
				}
				else if (!movingRight && newPosition.x < newX && objectStartPos.x > newX)
				{
					card.SetTargetPos(new Vector3(objectStartPos.x, card.transform.position.y, card.transform.position.z));
					objectStartPos.x = newX;
					dragStartPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.WorldToScreenPoint(newPosition).z));
					updateStartPos = newPosition;
					targetPos = objectStartPos;
					break;
				}
			}
		}
		*/
		lastpos = transform.position = newPosition;
	}


	public float GetPositionX()
	{
		return targetPos.x;
	}

	Coroutine cc;
	public void SetTargetPos(Vector3 target, float interval = 0.05f)
	{
		if (cc != null)
			StopCoroutine(cc);

		cc = StartCoroutine(MoveToTarget(target, interval)); // Start the smooth movement coroutine with a duration of 0.2 seconds
	}

	IEnumerator MoveToTarget(Vector3 target, float duration)
	{
		targetPos = target;
		Vector3 startPos = transform.position;
		float elapsed = 0f;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float t = Mathf.Clamp01(elapsed / duration); // Ensure t stays between 0 and 1

			transform.position = Vector3.Lerp(startPos, target, t); // Smoothly interpolate the position
			yield return null;
		}

		// Final position correction (to avoid overshooting)
		//transform.position = target;
		transform.position = new Vector3(target.x, target.y, -0.1f);
		//objectStartPos.x = transform.position.x;
	}

	private void OnMouseUp()
	{
		if (!is_Touched)
			return;

		float elapsedTime = Time.time - time_Click;

		float distance = Vector2.Distance(new Vector2(objectStartPos.x, objectStartPos.y), new Vector2(transform.position.x, transform.position.y));

		transform.localScale = originalScale;

		float distance_Return_Minimum = 0.20f;

		Debug.Log("Distance : " + distance);

		bool select = true;

		if (elapsedTime > 0.3f || distance > distance_Return_Minimum)
			select = false;

		/*if (cardData.is_Select == false)
			if (CardsManager.the.Count_Num_Select() >= 5)
				select = false;*/

		/*if (StageManager.instance && StageManager.instance.state == STAGESTATE.START)
			select = true;

		if (select == false)
			//StartCoroutine(GoToPosition_Select(!cardData.is_Select));
			StartCoroutine(ReturnToStartPosition()); // 원래 자리로 돌아가기
		else
			CardSelect();*/

		//if (showType == SHOWTYPE.UI)
		ShowDesc(!desc.activeSelf);

		StartCoroutine(ReturnToStartPosition()); // 원래 자리로 돌아가기
		CardSelect();

		is_Dragging = false;
		is_Touched = false;
	}


	IEnumerator ReturnToStartPosition()
	{
		float duration = 0.12f;
		float elapsed = 0f;
		Vector3 startPos = transform.position;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / duration;
			transform.position = Vector3.Lerp(startPos, objectStartPos, t); // 부드럽게 이동
			transform.position = new Vector3(transform.position.x, transform.position.y, startPos.z);
			yield return null;
		}

		transform.position = new Vector3(objectStartPos.x, objectStartPos.y, objectStartPos.z);
	}

	IEnumerator GoToPosition_Select(bool select)
	{
		float duration = 0.12f;
		float elapsed = 0f;
		Vector3 startPos = transform.position;
		//objectStartPos = new Vector3(objectStartPos.x, objectStartPos.y + 0.12f * (select ? 1 : -1), objectStartPos.z);
		objectStartPos = new Vector3(objectStartPos.x, initialY + 0.2f * (select ? 1 : 0), objectStartPos.z);

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / duration;
			transform.position = new Vector3(
				Mathf.Lerp(startPos.x, objectStartPos.x, t),
				Mathf.Lerp(startPos.y, objectStartPos.y, t),
				offset_Z
			//Mathf.Lerp(startPos.z, objectStartPos.z, t)
			);
			yield return null;
		}

		// 최종 위치 보정 (X, Y 값만 보정, Z는 현재 값 유지)
		transform.position = new Vector3(objectStartPos.x, objectStartPos.y, -0.1f);
	}

	void CardSelect()
	{
		Btn_Select();
	}

	//--------------------------------------------------
}
