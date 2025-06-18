using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VideoPokerKit;


public enum CARDDATA
{
	ROUND,
	SCORE,
	CHIP,
	MULT,
	TIME,
	COUNT,
	HAND,
	DISCARD,
	ANTE,
	DOLLAR,
	SCORE_CLEAR,
	SCORE_ALL,
	MAX
}

public struct RUNINFO
{	
	public Wins wintype;
	public int mult;
	public int chips;
	public int level;
}

public class CardDataManager : MonoBehaviour
{
	public List<RUNINFO> list_RunInfo = new List<RUNINFO>();

	public Sprite[] img_Joker;
	public Sprite[] img_Planet;
	public Sprite[] img_Tarot;

	public static string[] name_Wins =
	{
		"���� ī��",//"HIGH CARD",
		"���",//"ONE_PAIR",
		"�����",//"TWO_PAIR",
		"Ʈ����",//"THREE_OF_A_KIND",
		"��Ʈ����Ʈ",//"STRAIGHT",
		"�÷���",//"FLUSH",
		"Ǯ�Ͽ콺",//"FULL_HOUSE",
		"��ī��",//"FOUR_OF_A_KIND",
		"��Ʈ����Ʈ �÷���",//"STRAIGHT_FLUSH",
		//"ROYAL_STRAIGHT_FLUSH",
	};

	public bool is_AutoPlay;

	public static CardDataManager instance;

	public delegate void onCardDataChange(CARDDATA type, float value);
	public static onCardDataChange OnCardDataChange;

	public delegate void onEvaluateChange(Wins type);
	public static onEvaluateChange OnEvaluateChange;

	private float[] _cardData = new float[(int)CARDDATA.MAX];

	public float this[CARDDATA index]
	{
		get => _cardData[(int)index];
		set
		{
			if (_cardData[(int)index] != value) // ���� ����� ���� �̺�Ʈ ȣ��
			{
				_cardData[(int)index] = value;
				OnCardDataChange?.Invoke(index, value);
			}
		}
	}

	float countDownTime;
	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}

		UI_CardData[] objectsWithAScript = FindObjectsByType<UI_CardData>(FindObjectsSortMode.None);

		foreach (UI_CardData a in objectsWithAScript)
		{
			OnCardDataChange += a.OnCardDataChange;
		}

		Event_CardData_Change[] objectsWithAScript2 = FindObjectsByType<Event_CardData_Change>(FindObjectsSortMode.None);

		foreach (Event_CardData_Change a in objectsWithAScript2)
		{
			OnCardDataChange += a.OnCardDataChange;
		}

		is_AutoPlay = false;

		Init_User_Check();
		StageManager.OnStateChange += OnStateChange;

		//SetMyData(CARDDATA.DOLLAR, 4);
		//SetMyData(CARDDATA.DOLLAR, 100); //rsh_temp
		//SetMyData(CARDDATA.DOLLAR, 10);

		Init_Run_Info();
	}

	void Init_Run_Info()
	{
		/*string[] desc =
		{
			"����" , "(Lv.1)<color=#F08D00>���� ī��</color> ��(��)\n�������մϴ�.\n <color=red>+1 </color> ��� ��\n<color=#206BE3>+10 </color> ���� Ĩ",
			"����" , "(Lv.1)<color=#F08D00>���</color> ��(��)\n�������մϴ�.\n <color=red>+1 </color> ��� ��\n<color=#206BE3>+15 </color> ���� Ĩ",
			"õ�ռ�" , "(Lv.1)<color=#F08D00>�� ���</color> ��(��)\n�������մϴ�.\n <color=red>+1 </color> ��� ��\n<color=#206BE3>+20 </color> ���� Ĩ",
			"�ݼ�" , "(Lv.1)<color=#F08D00>Ʈ����</color> ��(��)\n�������մϴ�.\n <color=red>+2 </color> ��� ��\n<color=#206BE3>+20 </color> ���� Ĩ",
			"�伺" , "(Lv.1)<color=#F08D00>��Ʈ����Ʈ</color> ��(��) �������մϴ�.\n <color=red>+3 </color> ��� ��\n<color=#206BE3>+30 </color> ���� Ĩ",
			"��" , "(Lv.1)<color=#F08D00>�÷���</color> ��(��)\n�������մϴ�.\n <color=red>+2 </color> ��� ��\n<color=#206BE3>+15 </color> ���� Ĩ",			
			"ȭ��" , "(Lv.1)<color=#F08D00>Ǯ�Ͽ콺</color> ��(��)\n�������մϴ�.\n <color=red>+3 </color> ��� ��\n<color=#206BE3>+15 </color> ���� Ĩ",
			"��ռ�" , "(Lv.1)<color=#F08D00>��ī��</color> ��(��)\n�������մϴ�.\n <color=red>+3 </color> ��� ��\n<color=#206BE3>+30 </color> ���� Ĩ",
			"�ؿռ�" , "(Lv.1)<color=#F08D00>��Ʈ����Ʈ�÷���</color>��(��) �������մϴ�.\n <color=red>+4 </color> ��� ��\n<color=#206BE3>+40 </color> ���� Ĩ",
		};*/

		AddRunInfo(Wins.WIN_HIGH_CARD, 1, 10);
		AddRunInfo(Wins.WIN_ONE_PAIR, 1, 15);
		AddRunInfo(Wins.WIN_TWO_PAIR, 1, 20);
		AddRunInfo(Wins.WIN_THREE_OF_A_KIND, 2, 15);
		AddRunInfo(Wins.WIN_STRAIGHT, 2, 20);
		AddRunInfo(Wins.WIN_FLUSH, 3, 30);
		AddRunInfo(Wins.WIN_FULL_HOUSE, 3, 15);
		AddRunInfo(Wins.WIN_FOUR_OF_A_KIND, 3, 30);
		AddRunInfo(Wins.WIN_STRAIGHT_FLUSH, 3, 30);
	}

	void AddRunInfo(Wins type, int mult, int chips)
	{
		RUNINFO info = new RUNINFO();
		info.level = 0;
		info.wintype = type;
		info.mult = mult;
		info.chips = chips;

		list_RunInfo.Add(info);
	}

	public List<Wins> list_User_Checked = new List<Wins>();
	public bool Is_User_Checked(Wins type)
	{
		return list_User_Checked.Contains(type);
	}

	public void Set_User_Check(params Wins[] types)
	{
		list_User_Checked.Clear();
		list_User_Checked.AddRange(types);
	}

	public void Reset_User_Check(Wins type, bool plus)
	{
		if (plus)
		{
			// �ߺ� ���� �߰�
			if (!list_User_Checked.Contains(type))
			{
				list_User_Checked.Add(type);
			}
		}
		else
		{
			// �����ϸ� ����
			list_User_Checked.Remove(type);
		}
	}

	public void InitMultAndChips()
	{
		SetMyData(CARDDATA.CHIP, 0);
		SetMyData(CARDDATA.MULT, 0);
		OnEvaluateChange((Wins)(-1));
	}

	void Init_User_Check()
	{
		Set_User_Check(
			Wins.WIN_HIGH_CARD,
			Wins.WIN_ONE_PAIR,
			Wins.WIN_TWO_PAIR,
			Wins.WIN_THREE_OF_A_KIND,
			Wins.WIN_STRAIGHT,
			Wins.WIN_FLUSH,
			Wins.WIN_FULL_HOUSE,
			Wins.WIN_FOUR_OF_A_KIND,
			Wins.WIN_STRAIGHT_FLUSH
			//Wins.WIN_ROYAL_FLUSH
			);
	}

	

	void InitGameData()
	{
		SetMyData(CARDDATA.ROUND, 1);
		SetMyData(CARDDATA.SCORE, 0);
		SetMyData(CARDDATA.CHIP, 0);
		SetMyData(CARDDATA.MULT, 0);
		SetMyData(CARDDATA.TIME, 0);
		SetMyData(CARDDATA.COUNT, 30);
		SetMyData(CARDDATA.HAND, 0);
		SetMyData(CARDDATA.DISCARD, 0);
		SetMyData(CARDDATA.ANTE, 0);
		//SetMyData(CARDDATA.DOLLAR, 0);
		SetMyData(CARDDATA.SCORE_CLEAR, 0);
		SetMyData(CARDDATA.SCORE_ALL, 0);

		SetMyData(CARDDATA.DOLLAR, 4);
	}

	void InitRoundData()
	{
		SetMyData(CARDDATA.SCORE, 0);
		/*SetMyData(CARDDATA.HAND, 4);
		SetMyData(CARDDATA.DISCARD, 4);*/
	}

	public void InitHandDiscard()
	{
		SetMyData(CARDDATA.HAND, 4);
		SetMyData(CARDDATA.DISCARD, 4);
	}

	private void Start()
	{
		Initialize();
		StartCoroutine(Count());
	}

	void OnStateChange(STAGESTATE state)
	{
		switch (state)
		{
			case STAGESTATE.PRE_START:
				InitGameData();
				break;

			case STAGESTATE.START:

				break;

			case STAGESTATE.REWARD_FINISH:
				InitRoundData();
				break;
		}
	}

	bool refresh_Turn = true;
	IEnumerator Count()
	{
		countDownTime = 30;

		while (true)
		{
			if (refresh_Turn)
			{
				if (countDownTime < 0)
				{
					countDownTime = 0;
					refresh_Turn = false;
					StartCoroutine(NewTurn());
				}
				else
				{
					countDownTime -= Time.deltaTime;
					this[CARDDATA.COUNT] = (int)countDownTime;
				}
			}

			yield return null;
		}
	}

	public bool Check_GameOver()
	{
		Debug.Log("<color=yellow>GGG:Check Game Over</color>");

		bool monster_die = Character.instance.MonsterDie();

		if (monster_die == true)
		{
			Debug.Log("<color=yellow>GGG:Monster Die</color>");
			return false;
		}

		if (Can_Hand() == false)
		{
			//if (StageManager.instance)
			//StageManager.instance.StartStage();

			//DialogManager.instance.ShowDialog(DIALOG.OVER);
			Character.instance.hp = 0;
			Debug.Log("<color=yellow>GGG:HP Change : 0</color>");
			return true;
		}

		return false;
	}

	public void SetResult(bool plus_score = false)
	{
		int chips = Paytable.the.GetCurrentChips();
		int currWinMultiplier = Paytable.the.GetCurrentWinMultiplier();
		CardDataManager.instance.SetMyData(CARDDATA.CHIP, chips);
		CardDataManager.instance.SetMyData(CARDDATA.MULT, currWinMultiplier);

		if (plus_score)
			ScoreCount();

		OnEvaluateChange?.Invoke(Paytable.the.GetCurrentWinIndex());
	}

	public void ScoreCount()
	{
		float score = GetNowDamage();

		CardDataManager.instance.SetMyData(CARDDATA.SCORE, score, CALC.EQUAL);
		CardDataManager.instance.SetMyData(CARDDATA.SCORE_ALL, score, CALC.PLUS);
	}

	public float GetNowDamage()
	{
		float chips = GetMyData(CARDDATA.CHIP);
		float currWinMultiplier = GetMyData(CARDDATA.MULT);

		return chips * currWinMultiplier;
	}

	IEnumerator NewTurn()
	{
		yield return new WaitForSeconds(1);

		countDownTime = 30;
		SetMyData(CARDDATA.COUNT, 30);

		this[CARDDATA.TIME]++;

		if (this[CARDDATA.TIME] == 4)
			this[CARDDATA.TIME] = 0;

		refresh_Turn = true;
	}

	public void SetMyData(CARDDATA type, float value, CALC calc_type = CALC.EQUAL)
	{
		switch(calc_type)
		{
			case CALC.EQUAL:
				this[type] = value;
				break;

			case CALC.PLUS:
				this[type] += value;
				break;

			case CALC.MINUS:
				this[type] -= value;
				break;

			case CALC.MULTIPLE:
				this[type] *= value;
				break;
		}

		if (this[type] < 0)
			this[type] = 0;

		OnCardDataChange?.Invoke(type, this[type]);
	}

	public float GetMyData(CARDDATA type)
	{
		if (Enum.IsDefined(typeof(CARDDATA), type))
			return this[type];

		return 0;
	}

	public bool Can_Discard()
	{
		if (this[CARDDATA.DISCARD] > 0)
		{
			return true;
		}

		return false;
	}

	public void Use_Discard()
	{
		if (this[CARDDATA.DISCARD] > 0)
		{
			Character.instance.UseDiscard();

			this[CARDDATA.DISCARD]--;
		}
	}

	public bool Can_Hand()
	{
		if (this[CARDDATA.HAND] > 0)
		{
			return true;
		}

		return false;
	}

	public void Use_Hand()
	{
		if (this[CARDDATA.HAND] > 0)
		{
			Character.instance.UseHand();

			this[CARDDATA.HAND]--;
		}
	}

	void Initialize()
	{
		LoadData();

	}




	void LoadData()
	{
		//EquipItem(ITEMTYPE.HELMET , "id_helmet_1");
	}

}