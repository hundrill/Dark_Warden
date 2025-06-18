using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;


public class UI_Reward : MonoBehaviour
{
	public GameObject Content;
	public GameObject baseItem;
	public Button btn_Buy;

	public static UI_Reward instance;
	public GameObject description_Reward;

	private void Awake()
	{
		/*if (instance == null)
		{
			instance = this;
		}*//*
		else if (instance != this)
		{
			Destroy(gameObject);
		}*/
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void OnEnable()
	{
		if (instance == null)
		{
			instance = this;
		}

		ShowReward(true, 2);

		if (description_Reward == null)
			description_Reward = GetComponentInChildren<Description>().gameObject;

		if (description_Reward)
			description_Reward.SetActive(false);

		if (SoundManager.instance)
			SoundManager.instance.PlaySound("Card_Popup_open");
	}

	public List<GameObject> list_Joker_Reward = new List<GameObject>();
	public List<GameObject> list_Planet_Reward = new List<GameObject>();
	public List<GameObject> list_Tarot_Reward = new List<GameObject>();

	bool is_Select = false;
	JOKER joker_Select;
	PLANET planet_Select;
	public TAROT tarot_Select;
	int price_Select;
	int type_Select;

	void ShowReward(bool show, int num = 3)
	{
		if (show == false)
		{
			for (int i = 0; i < list_Joker_Reward.Count; i++)
			{
				list_Joker_Reward[i].SetActive(false);
			}
			return;
		}

		btn_Buy.interactable = false;
		if (UI_Main.instance)
			btn_Buy.image.material = UI_Main.instance.grayMaterial;

		if (DataManager.instance)
		{
			float width_one = 0.5f;
			float width_gap = 0.05f;

			float startx = -(num - 1) * (width_one + width_gap) / 2;
			startx += 1.49f;

			for (int i = list_Joker_Reward.Count - 1; i >= num; i--)
			{
				list_Joker_Reward[i].SetActive(false);
			}

			JOKER[] list_Temp = JokerManager.instance.GetJokerType_Reward(num);

			if (list_Temp.Length == 0)
				return;

			if (num > list_Temp.Length)
				num = list_Temp.Length;

			for (int i = 0; i < num; i++)
			{
				float xPos = startx + i * (width_one + width_gap);
				Vector3 pos = new Vector3(xPos, 5.7f, -1);

				if (i < list_Joker_Reward.Count)
				{
					list_Joker_Reward[i].SetActive(true);
					list_Joker_Reward[i].transform.position = pos;
					list_Joker_Reward[i].GetComponent<Card_Joker>().SetJokerType(list_Temp[i]);
				}
				else if (JokerManager.instance.item_joker)
				{
					GameObject item = Instantiate(JokerManager.instance.item_joker, pos, Quaternion.identity, gameObject.transform);

					item.GetComponent<Card_Joker>().OnSelect += OnSelect;
					item.GetComponent<Card_Joker>().OnTouched += OnTouched;
					item.GetComponent<Card_Joker>().SetJokerType(list_Temp[i]);
					item.GetComponent<Card_Joker>().SetOrderLayer(3);
					item.GetComponent<Card_Joker>().SetShowType(SHOWTYPE.UI);
					foreach (SpriteRenderer sr in item.GetComponentsInChildren<SpriteRenderer>(true))
					{
						sr.sortingLayerName = "DIALOG"; // 원하는 레이어 이름	
						sr.sortingOrder = 1;
					}

					foreach (Canvas canvas in item.GetComponentsInChildren<Canvas>(true))
					{
						canvas.sortingLayerName = "DIALOG";
						canvas.sortingOrder = 1;
					}

					list_Joker_Reward.Add(item);
				}
			}

			Setting_Planet(true, 1);
			Setting_Tarot(true, 2);
			Reposition_Joker();
		}
	}

	void Setting_Planet(bool show, int num = 3)
	{
		if (show == false)
		{
			for (int i = 0; i < list_Planet_Reward.Count; i++)
			{
				list_Planet_Reward[i].SetActive(false);
			}
			return;
		}

		btn_Buy.interactable = false;
		if (UI_Main.instance)
			btn_Buy.image.material = UI_Main.instance.grayMaterial;

		if (DataManager.instance)
		{
			float width_one = 0.5f;
			float width_gap = 0.05f;

			float startx = 2.05f - 0.62f;

			for (int i = list_Planet_Reward.Count - 1; i >= num; i--)
			{
				list_Planet_Reward[i].SetActive(false);
			}

			PLANET[] list_Temp = JokerManager.instance.GetPlanetType_Reward(num);

			if (list_Temp.Length == 0)
				return;

			if (num > list_Temp.Length)
				num = list_Temp.Length;

			for (int i = 0; i < num; i++)
			{
				float pos = startx + i * (width_one + width_gap);
				Vector3 newPos = new Vector3(pos, 0.8f, -1);

				if (i < list_Planet_Reward.Count)
				{
					list_Planet_Reward[i].SetActive(true);
					list_Planet_Reward[i].transform.position = newPos;
					list_Planet_Reward[i].GetComponent<Card_Planet>().SetPlanetType(list_Temp[i]);
				}
				else if (JokerManager.instance.item_planet)
				{
					GameObject item = Instantiate(JokerManager.instance.item_planet, newPos, Quaternion.identity, gameObject.transform);

					item.GetComponent<Card_Planet>().OnSelect += OnSelect;
					item.GetComponent<Card_Planet>().OnTouched += OnTouched;
					item.GetComponent<Card_Planet>().SetPlanetType(list_Temp[i]);
					//item.GetComponent<Card_Planet>().SetPlanetType(PLANET.type_9);
					item.GetComponent<Card_Planet>().SetOrderLayer(3);
					item.GetComponent<Card_Planet>().SetShowType(SHOWTYPE.UI);
					foreach (SpriteRenderer sr in item.GetComponentsInChildren<SpriteRenderer>(true))
					{
						sr.sortingLayerName = "DIALOG"; // 원하는 레이어 이름						
						sr.sortingOrder = 1;
					}

					foreach (Canvas canvas in item.GetComponentsInChildren<Canvas>(true))
					{
						canvas.sortingLayerName = "DIALOG";
						canvas.sortingOrder = 1;
					}

					list_Planet_Reward.Add(item);
				}
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

		btn_Buy.interactable = false;
		if (UI_Main.instance)
			btn_Buy.image.material = UI_Main.instance.grayMaterial;

		if (DataManager.instance)
		{
			float width_one = 0.5f;
			float width_gap = 0.05f;

			float startx = 2.05f - 0.065f;

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
				Vector3 newPos = new Vector3(pos, 0.8f, -1);

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
					item.GetComponent<Card_Tarot>().SetTarotType(list_Temp[i]);
					//item.GetComponent<Card_Tarot>().SetTarotType(TAROT.type_9);
					item.GetComponent<Card_Tarot>().SetOrderLayer(3);
					item.GetComponent<Card_Tarot>().SetShowType(SHOWTYPE.UI);
					foreach (SpriteRenderer sr in item.GetComponentsInChildren<SpriteRenderer>(true))
					{
						sr.sortingLayerName = "DIALOG"; // 원하는 레이어 이름
						sr.sortingOrder = 1;
					}

					foreach (Canvas canvas in item.GetComponentsInChildren<Canvas>(true))
					{
						canvas.sortingLayerName = "DIALOG";
						canvas.sortingOrder = 1;
					}

					list_Tarot_Reward.Add(item);
				}
			}
		}
	}

	void Reposition_Joker()
	{
		int num = list_Joker_Reward.Count(obj => obj.activeSelf);

		float width_one = 0.5f;
		float width_gap = 0.05f;

		float startx = -(num - 1) * (width_one + width_gap) / 2;
		//startx += 1.22f;
		startx += 0.6f;

		int i = 0;

		foreach (var obj in list_Joker_Reward)
		{
			if (obj.activeSelf)
			{
				float xPos = startx + i * (width_one + width_gap);
				Vector3 pos = new Vector3(xPos, 0.8f, -1);

				obj.transform.position = pos;

				i++;
			}
		}
	}

	void Off_Desc_Ohters()
	{
		if (UI_Tarot.instance)
			return;

		foreach (var obj in list_Joker_Reward)
		{
			obj.GetComponent<Card_Joker>().ShowDesc(false);
		}

		GameObject[] spawnObjects = Object.FindObjectsByType<Card_Planet>(FindObjectsSortMode.None)
								  .Select(c => c.gameObject)
								  .ToArray();

		foreach (var obj in spawnObjects)
			obj.GetComponent<Card_Planet>().ShowDesc(false);


		spawnObjects = Object.FindObjectsByType<Card_Tarot>(FindObjectsSortMode.None)
								  .Select(c => c.gameObject)
								  .ToArray();

		foreach (var obj in spawnObjects)
			obj.GetComponent<Card_Tarot>().ShowDesc(false);

	}

	/*void InitScrollContents()
	{
		ClearChildren(Content);

		for (int i = 0; i < 3; i++)
		{
			GameObject item = Instantiate(baseItem, new Vector3(0, 0, 0), Quaternion.identity);
			item.transform.SetParent(Content.transform);
			item.transform.localScale = new Vector3(1, 1, 1);
			item.transform.localPosition = new Vector3(0, 0, 0);
			item.GetComponent<item_one_joker>().SetType(JOKER.type_1 + i);
			item.GetComponent<item_one_joker>().OnSelect += OnSelect;

		}
	}*/

	void OnSelect(JOKER joker, int price)
	{
		int mine = (int)CardDataManager.instance.GetMyData(CARDDATA.DOLLAR);

		if (mine >= price)
		{
			is_Select = true;
			joker_Select = joker;
			price_Select = price;
			btn_Buy.interactable = true;
			btn_Buy.image.material = null;
			type_Select = 0;
			//StageManager.instance.state = STAGESTATE.REWARD_FINISH;
			//ShowReward(false);
		}
		else
		{
			is_Select = true;
			joker_Select = joker;
			price_Select = price;
			btn_Buy.interactable = false;
			btn_Buy.image.material = UI_Main.instance.grayMaterial;
		}
	}

	void OnSelect(PLANET joker, int price)
	{
		int mine = (int)CardDataManager.instance.GetMyData(CARDDATA.DOLLAR);

		if (mine >= price)
		{
			is_Select = true;
			planet_Select = joker;
			price_Select = price;
			btn_Buy.interactable = true;
			btn_Buy.image.material = null;
			type_Select = 1;
			//StageManager.instance.state = STAGESTATE.REWARD_FINISH;
			//ShowReward(false);
		}
		else
		{
			is_Select = true;
			planet_Select = joker;
			price_Select = price;
			btn_Buy.interactable = false;
			btn_Buy.image.material = UI_Main.instance.grayMaterial;
		}
	}

	void OnSelect(TAROT joker, int price)
	{
		int mine = (int)CardDataManager.instance.GetMyData(CARDDATA.DOLLAR);

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
		}
	}

	void OnTouched(JOKER joker)
	{
		Off_Desc_Ohters();
	}

	void OnTouched(PLANET joker)
	{
		Off_Desc_Ohters();
	}

	void OnTouched(TAROT joker)
	{
		Off_Desc_Ohters();
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

	public void Btn_NextRound()
	{
		ShowReward(false);
		StageManager.instance.state = STAGESTATE.REWARD_FINISH;

		if (SoundManager.instance)
			SoundManager.instance.PlaySound("Card_Next");
	}

	public void Btn_Reroll()
	{
		ShowReward(false);
		StageManager.instance.state = STAGESTATE.REWARD_FINISH;

		if (SoundManager.instance)
			SoundManager.instance.PlaySound("Card_Reroll");
	}

	public void Btn_Buy()
	{
		int mine = (int)CardDataManager.instance.GetMyData(CARDDATA.DOLLAR);

		if (mine >= price_Select)
		{
			if (type_Select == 0)
			{
				JokerManager.instance.Achieve(joker_Select);

				foreach (var item in list_Joker_Reward)
				{
					if (item.GetComponent<Card_Joker>().type.id == joker_Select.id)
					{
						item.SetActive(false);
						break;
					}
				}

				Reposition_Joker();
			}
			else if (type_Select == 1)
			{
				JokerManager.instance.Achieve(planet_Select);

				foreach (var item in list_Planet_Reward)
				{
					if (item.GetComponent<Card_Planet>().type == planet_Select)
					{
						item.SetActive(false);
						break;
					}
				}

				Reposition_Joker();
			}
			else if (type_Select == 2)
			{
				//JokerManager.instance.Achieve(tarot_Select);

				foreach (var item in list_Tarot_Reward)
				{
					if (item.GetComponent<Card_Tarot>().type.id == tarot_Select.id)
					{
						item.SetActive(false);
						break;
					}
				}

				Reposition_Joker();

				if (UI_Card.instance.Dlg_Tarot)
					UI_Card.instance.Dlg_Tarot.SetActive(true);
			}

			if (SoundManager.instance)
				SoundManager.instance.PlaySound("Card_Buy");

			ResetButton();
			/*ShowReward(false);
			StageManager.instance.state = STAGESTATE.REWARD_FINISH;*/

			CardDataManager.instance.SetMyData(CARDDATA.DOLLAR, price_Select, CALC.MINUS);

			if (description_Reward)
				description_Reward.SetActive(false);
		}
	}

	void ResetButton()
	{
		btn_Buy.interactable = false;
		btn_Buy.image.material = UI_Main.instance.grayMaterial;
	}

	public void SelectReward(int idx)
	{
		ShowReward(false);
		StageManager.instance.state = STAGESTATE.REWARD_FINISH;
	}
}
