using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VideoPokerKit;
using static CardDataManager;
using static StageManager;

//using static UnityEngine.Rendering.DebugUI;
using static VideoPokerKit.CardsManager;
using static VideoPokerKit.MainGame;

public class UI_Card : MonoBehaviour
{
	public GameObject description_Joker;
	public GameObject description_TarotPlanet;
	public TextMeshProUGUI[] txt_CardData;
	public TextMeshProUGUI txt_Name;
	public HP_Ui hp_Mon;

	public Button btn_Hand;
	public Button btn_Discard;
	public Button btn_Sort_Rank;
	public Button btn_Sort_Suite;

	public static UI_Card instance;
	public GameObject Dlg_Option;
	public GameObject Dlg_RunInfo;
	public GameObject Dlg_Tarot;
	public GameObject Dlg_DeckInfo;

	public bool is_Dlg_Open;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}


		MainGame.newGame += NewGame;
		CardDataManager.OnCardDataChange += OnCardDataChange;
		CardDataManager.OnEvaluateChange += OnEvaluateChange;
		CardsManager.OnCardSelect += OnCardSelect;
		CardsManager.On_Event_Change += On_Event_Change;
		StageManager.OnStateChange += OnStateChange;

		MainGame.OnCardGameStateChange += OnCardGameStateChange;

		GetComponent<Canvas>().worldCamera = GameObject.Find("Camera_Card").GetComponent<Camera>();

		is_Dlg_Open = false;
	}

	public void OnMonsterHpChange(int now, int maxhp)
	{
		if (hp_Mon)
		{
			float pct = now * 100 / maxhp * 0.01f;

			hp_Mon.SetFill(pct, Character.instance.totalAttack == 0);
			if (now < 0) now = 0;
			hp_Mon.SetText(now, maxhp);
		}
	}

	void On_Event_Change(bool isevent)
	{
		Check_Hand_Discard_Button_State();
	}

	void OnCardGameStateChange(byte gamestate)
	{
		Check_Hand_Discard_Button_State();
	}

	void Check_Hand_Discard_Button_State()
	{
		bool active = true;

		if (MainGame.the)
			active = MainGame.the.Waiting_User_Input();

		if (CardsManager.the)
			if (CardsManager.the.Count_Num_Select(false) == 0)
				active = false;

		if (CardsManager.the)
			if (CardsManager.the.lastHit)
				active = false;

		if (btn_Hand)
		{
			btn_Hand.interactable = active;

			if (CardDataManager.instance.GetMyData(CARDDATA.HAND) <= 0)
				btn_Hand.interactable = false;
		}

		if (btn_Discard)
		{
			btn_Discard.interactable = active;

			if (CardDataManager.instance.GetMyData(CARDDATA.DISCARD) <= 0)
				btn_Discard.interactable = false;
		}

		if (btn_Discard)
		{
			btn_Discard.interactable = active;

			if (CardDataManager.instance.GetMyData(CARDDATA.DISCARD) <= 0)
				btn_Discard.interactable = false;
		}

		if (MainGame.the)
			active = MainGame.the.Waiting_User_Input();

		if (btn_Sort_Rank)
		{
			btn_Sort_Rank.interactable = active;
		}

		if (btn_Sort_Suite)
		{
			btn_Sort_Suite.interactable = active;
		}
	}

	private void Update()
	{
		if (CardsManager.the.Is_Event_Moving() || !CardsManager.the.Is_All_Idle())
			return;

		if (Input.GetMouseButtonDown(0))
		{
			Camera cam = GameObject.Find("Camera_Card").GetComponent<Camera>();
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				GameObject clickedObj = hit.collider.gameObject;

				if (clickedObj.name.ToLower().Contains("deck")) // 태그가 "Deck"이면
				{
					// 클릭한 오브젝트 처리
					if (Dlg_DeckInfo)
						Dlg_DeckInfo.SetActive(true);

					if (SoundManager.instance)
						SoundManager.instance.PlaySound("Card_Popup_open");
				}
			}
		}
	}

	void OnCardSelect(int num)
	{
		Check_Hand_Discard_Button_State();

		/*if (num == 0 && btn_Hand && btn_Hand.GetComponent<Image>())
			btn_Hand.GetComponent<Image>().material = UI_Main.instance.grayMaterial;
		else
			btn_Hand.GetComponent<Image>().material = null;

		if (num == 0 && btn_Discard && btn_Discard.GetComponent<Image>())
			btn_Discard.GetComponent<Image>().material = UI_Main.instance.grayMaterial;
		else
			btn_Discard.GetComponent<Image>().material = null;*/
	}

	public void Sort_Rank()
	{
		CardsManager.the.sort_Type = SORT.RANK;
		CardsManager.the.Sort_Card();
	}

	public void Sort_Suite()
	{
		CardsManager.the.sort_Type = SORT.SUITE;
		CardsManager.the.Sort_Card();
	}

	public void NewGame()
	{
		SetWinsName("");
	}

	public void SetWinsName(string name)
	{
		if (txt_Name != null)
			txt_Name.text = name;
	}

	public void OnEvaluateChange(Wins type)
	{
		if (type < 0)
		{
			SetWinsName("");
			return;
		}

		if (name_Wins.Length > (int)type)
		{
			int level = Paytable.the.GetLevel(type);
			SetWinsName(string.Format("{0} Lv.{1}", name_Wins[(int)type], level + 1));
		}
	}

	void OnStateChange(STAGESTATE state)
	{
		//Check_Hand_Discard_Button_State();
	}

	public void OnCardDataChange(CARDDATA type, float value)
	{
		if (txt_CardData.Length > (int)type && txt_CardData[(int)type])
		{
			string concat = "";
			switch (type)
			{
				case CARDDATA.DOLLAR:
					concat = "$";
					break;

				case CARDDATA.HAND:
					Check_Hand_Discard_Button_State();
					/*if (btn_Hand && btn_Hand.GetComponent<Image>())
					{
						if (value == 0)
							btn_Hand.GetComponent<Image>().material = UI_Main.instance.grayMaterial;
						else
							btn_Hand.GetComponent<Image>().material = null;
					}*/
					break;

				case CARDDATA.DISCARD:
					Check_Hand_Discard_Button_State();
					/*if (btn_Discard && btn_Discard.GetComponent<Image>())
					{
						if (value == 0)
							btn_Discard.GetComponent<Image>().material = UI_Main.instance.grayMaterial;
						else
							btn_Discard.GetComponent<Image>().material = null;
					}*/
					break;
			}

			txt_CardData[(int)type].text = string.Format("{0}{1}", concat, value);
		}
	}

	void RefreshButton(bool value)
	{
		if (btn_Hand && btn_Hand.GetComponent<Image>())
		{
			if (value == false)
				btn_Hand.GetComponent<Image>().material = UI_Main.instance.grayMaterial;
			else
				btn_Hand.GetComponent<Image>().material = null;
		}
		if (btn_Discard && btn_Discard.GetComponent<Image>())
		{
			if (value == false)
				btn_Discard.GetComponent<Image>().material = UI_Main.instance.grayMaterial;
			else
				btn_Discard.GetComponent<Image>().material = null;
		}
	}

	public void Discard()
	{
		MainGame.the.PlayDiscard();
		JokerManager.instance.DeleteAllJokerEffect();
	}

	public void Hand()
	{
		MainGame.the.PlayHand();
	}

	public void Exit()
	{

	}

	public void DeckInfo()
	{

	}

	public void Option()
	{
		if (Dlg_Option)
		{
			Time.timeScale = 0f;
			Dlg_Option.SetActive(true);

			if (SoundManager.instance)
				SoundManager.instance.PlaySound("Card_Popup_open");
		}
	}

	public void Runinfo()
	{
		if (Dlg_RunInfo)
		{
			Time.timeScale = 0f;
			Dlg_RunInfo.SetActive(true);

			if (SoundManager.instance)
				SoundManager.instance.PlaySound("Card_Popup_open");
		}
	}

	public void Runinfo_Event(int idx, int max_chip, int max_mult, int max_level)
	{
		if (Dlg_RunInfo)
		{
			Time.timeScale = 0f;
			Dlg_RunInfo.SetActive(true);

			Dlg_RunInfo.GetComponent<UI_RunInfo>().StartEvent(idx, max_chip, max_mult, max_level);

			if (SoundManager.instance)
				SoundManager.instance.PlaySound("Card_Popup_open");
		}
	}

	public void Tarot()
	{
		if (Dlg_Tarot)
		{
			Time.timeScale = 0f;
			Dlg_Tarot.SetActive(true);

			if (SoundManager.instance)
				SoundManager.instance.PlaySound("Card_Popup_open");
		}
	}

	public void Tarot_Event(int idx, int max_chip, int max_mult, int max_level)
	{
		if (Dlg_Tarot)
		{
			Time.timeScale = 0f;
			Dlg_Tarot.SetActive(true);

			if (SoundManager.instance)
				SoundManager.instance.PlaySound("Card_Popup_open");
			//Dlg_Tarot.GetComponent<UI_Tarot>().StartEvent(idx, max_chip, max_mult, max_level);
		}
	}

	public void Meta()
	{

	}
}
