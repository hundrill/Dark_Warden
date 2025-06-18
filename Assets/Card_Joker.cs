using UnityEngine;
using VideoPokerKit;
using System.Collections;
using System.Linq;
using TMPro;

public class Card_Joker : MonoBehaviour
{
	//--------------------------------------------------
	public SHOWTYPE showType;
	public GameObject canvas;
	public GameObject card;
	public GameObject desc;
	public GameObject price;
	public TextMeshProUGUI txt_Name;
	public TextMeshProUGUI txt_Desc;
	public TextMeshProUGUI txt_Price;

	/*public JOKERTIMING timing;
	public string id;
	public string todo;
	public string condition;*/
	public Card_Joker copyJoker;

	int price_Joker;
	float time_Click;
	public SpriteRenderer sprite_Joker;
	bool is_Touched = false;
	bool is_Dragging = false;
	public bool reserve_effect = false;
	float initialX;
	float initialY;
	float initialZ;
	Vector3 dragStartPos;
	Vector3 objectStartPos;
	Vector3 updateStartPos;
	Vector3 targetPos;

	float offset_Z = -1;
	private Vector3 originalScale;

	public delegate void onSelect(JOKER joker, int price);
	public onSelect OnSelect;

	public delegate void onTouched(JOKER type);
	public onTouched OnTouched;

	[HideInInspector]
	public JOKER type;

	Animator animator;

	void Awake()
	{
		animator = GetComponent<Animator>();
		if (animator != null)
		{
			float blendValue = Random.Range(0f, 1f);
			animator.SetFloat("IDLE_BLEND", blendValue);

			float progress = Random.Range(0f, 1f);
			animator.Play("Idle", 0, progress);
		}

		copyJoker = null;
	}

	public void OnEnable()
	{
		/*Animator animator = GetComponent<Animator>();
		float randomValue = Random.Range(0f, 1f); // 0.0 ~ 1.0 ���� ����
		animator.SetFloat("IDLE_BLEND", randomValue);*/

		ShowDesc(false);
	}

	void SettingPrice(int price)
	{
		price_Joker = price;

		if (txt_Price)
			txt_Price.text = string.Format("${0}", price_Joker);
	}

	void SettingDesc(bool show)
	{
		if (showType == SHOWTYPE.PLAY)
		{
			if (UI_Card.instance && UI_Card.instance.description_Joker)
				UI_Card.instance.description_Joker.SetActive(show);
		}
		else
		{
			if (UI_Reward.instance && UI_Reward.instance.description_Reward)
				UI_Reward.instance.description_Reward.SetActive(show);
		}

		if (show == false)
			return;

		/*string[] desc =
	{
			"Smile Mask" , "<color=red>���</color>�� �÷����� ī��� ���� ��� �� <color=red>+4 </color> ����� ����ϴ�.",
			"Cry Mask" , "<color=red>���� ���� 3��</color>���� �÷����� ī��� ���� ��� �� <color=#2070EC>+20 </color> Ĩ�� ����ϴ�.",
			"Greedy Mask" , "<color=red>���̾Ƹ��</color> ���� ī��� �÷����ϸ� ���� ��� �� <color=red>+1 </color> ����� ����ϴ�.",
			//"Mirror Mask" , "Played cards with <color=red> Pair </color> give <color=red>+4 </color>Mult when scored",
			"Nirvana Mask" , "���� ���� �� <color=red>+1 </color> <color=blue>������</color> Ƚ���� ����ϴ�.",
			"Face Mask" , "<color=red>�� ī��</color>�� �÷����� ī��� ���� ��� �� <color=red>+2</color> ����� ����ϴ�.",
			//"Hiker Mask" , "Every played <color=cyan> card </color> permanently gains <color=red>+5 </color>Chips when scored",
			"Hiker Mask" , "�÷����� <color=red>��� ī��</color>�� ���� ��� �� <color=#2070EC>+5 </color> Ĩ�� ����ϴ�.",
		};*/

		/*int idx = (int)0;

		if (idx >= desc.Length / 2)
			return;*/

		string joker_description = JokerManager.instance.GetJokerDescription(type);

		if (txt_Name)
			txt_Name.text = type.name;

		if (txt_Desc)
		{
			txt_Desc.text = joker_description;
		}

		if (showType == SHOWTYPE.PLAY)
		{
			if (UI_Card.instance && UI_Card.instance.description_Joker)
			{
				UI_Card.instance.description_Joker.SetActive(true);
				UI_Card.instance.description_Joker.GetComponent<Description>().SetDesc(gameObject, type.name, joker_description, "�Ϲ�");
			}
		}
		else if (showType == SHOWTYPE.UI)
		{
			if (UI_Reward.instance && UI_Reward.instance.description_Reward)
			{
				UI_Reward.instance.description_Reward.SetActive(true);
				UI_Reward.instance.description_Reward.GetComponent<Description>().SetDesc(gameObject, type.name, joker_description, "�Ϲ�");
			}
		}
	}


	public void SetShowType(SHOWTYPE type)
	{
		showType = type;

		if (showType == SHOWTYPE.UI)
			ShowPrice(true);
		else
			ShowPrice(false);
	}

	public void SetOrderLayer(int layer)
	{
		gameObject.GetComponent<SpriteRenderer>().sortingOrder = layer;
		canvas.GetComponent<Canvas>().sortingOrder = layer;
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

	public void SetJokerType(JOKER _type)
	{
		type = _type;
		SettingPrice(_type.price);
		//if (sprite_Joker && CardDataManager.instance && CardDataManager.instance.img_Joker.Length > _type.sprite)
			sprite_Joker.sprite = CardDataManager.instance.img_Joker[_type.sprite % CardDataManager.instance.img_Joker.Length];
	}

	public void Btn_Select()
	{
		OnSelect?.Invoke(type, price_Joker);
	}


	private void Start()
	{
		originalScale = transform.localScale;
	}

	void OnMouseDown()
	{
		if (UI_Card.instance.is_Dlg_Open && UI_Card.instance.is_Dlg_Open == true && showType == SHOWTYPE.PLAY)
			return;

		/*if (StageManager.instance && StageManager.instance.state == STAGESTATE.REWARD_START && showType == SHOWTYPE.PLAY)
			return;*/

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

		// ī�޶� �������� offset_Z ��ŭ �̵�
		transform.position += cam.transform.forward * offset_Z;

		updateStartPos = transform.position;

		// �巡�� ���� �� ��ġ ��ġ�� ������Ʈ ��ġ ����
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
		Vector3 delta = currentMousePos - dragStartPos; // �巡�� ������ ��� ��ȭ��
		Vector3 newPosition = updateStartPos + delta;


		/*
		// ���� �̵� ���� Ȯ��
		bool movingRight = newPosition.x > objectStartPos.x;

		var sortedCards = movingRight
			? CardsManager.the.GetAllCards().OrderBy(c => c.transform.position.x).ToList() // ������ �̵�: �������� ����
			: CardsManager.the.GetAllCards().OrderByDescending(c => c.transform.position.x).ToList(); // ���� �̵�: �������� ����

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
		if (UI_Card.instance.is_Dlg_Open && UI_Card.instance.is_Dlg_Open == true && showType == SHOWTYPE.PLAY)
			return;

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
			StartCoroutine(ReturnToStartPosition()); // ���� �ڸ��� ���ư���
		else
			CardSelect();*/

		//if (showType == SHOWTYPE.UI)
		ShowDesc(!desc.activeSelf);

		StartCoroutine(ReturnToStartPosition()); // ���� �ڸ��� ���ư���
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
			transform.position = Vector3.Lerp(startPos, objectStartPos, t); // �ε巴�� �̵�
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

		// ���� ��ġ ���� (X, Y ���� ����, Z�� ���� �� ����)
		transform.position = new Vector3(objectStartPos.x, objectStartPos.y, -0.1f);
	}

	void CardSelect()
	{
		Btn_Select();
	}

	//--------------------------------------------------
}
