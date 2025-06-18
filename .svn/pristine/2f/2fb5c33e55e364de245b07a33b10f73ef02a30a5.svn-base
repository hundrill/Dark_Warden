using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using VideoPokerKit;
using static Ashkatchap.AnimatorEvents.EventSMB;
using static Cinemachine.CinemachineFreeLook;
using static StageManager;
using System.Text.RegularExpressions;

public enum CALC
{
	EQUAL,
	PLUS,
	MINUS,
	MULTIPLE,
	RETRIGGER,
	COPY,
	MAX
}

public enum TARGET
{
	SELF,
	PLAYCARD,
	PLAYINGCARD,
	HANDCARD,
	DECKCARD,
	TOTAL,
	MAX
}


public struct TAROT_CONDITION
{
	public string[] todo;
}

public struct JOKER_CONDITION
{
	public JOKERTIMING timing;
	public string[] todo;
}

public struct TAROT
{
	public string id;
	public string name;
	public List<TAROT_CONDITION> list_Condition;
	public string description;
	public string functionvalue;
	
	public int price;
	public int sprite;
	public int draw_count;
	public int select_count;
	public int random_count;

	public float basevalue;
	public float increase;
	public float decrease;
	public float self_mults;
	public float self_chips;

	public string GetDescription()
	{
		string _description = description;
		/*_description = _description.Replace("[basevalue]", basevalue.ToString());
		_description = _description.Replace("[increase]", increase.ToString());
		_description = _description.Replace("[decrease]", decrease.ToString());*/
		return _description;
	}
}

public struct JOKER
{
	public string id;
	public string name;
	public List<JOKER_CONDITION> list_Condition;
	public string description;
	public int price;
	public int sprite;
	public float basevalue;
	public float increase;
	public float decrease;
	public float self_mults;
	public float self_chips;

	public string GetDescription()
	{
		string _description = description;
		_description = _description.Replace("[basevalue]", basevalue.ToString());
		_description = _description.Replace("[increase]", increase.ToString());
		_description = _description.Replace("[decrease]", decrease.ToString());
		return _description;
	}
}
/*
public enum JOKER
{
	type_1,
	type_2,
	type_3,
	type_4,
	type_5,
	type_6,
	//NONE
}
*/
public enum PLANET
{
	type_1,
	type_2,
	type_3,
	type_4,
	type_5,
	type_6,
	type_7,
	type_8,
	type_9,
	//NONE
}

/*public enum TAROT
{
	type_1,
	type_2,
	type_3,
	type_4,
}*/

public enum SHOWTYPE
{
	PLAY,
	UI
}


public struct PLANETINFO
{
	public PLANET type;
	public Wins wintype;
	public int mult;
	public int chips;
}

public struct TAROTINFO
{
	public TAROT type;
	public Wins wintype;
	public int mult;
	public int chips;
}

public enum JOKERTIMING
{
	DRAW,
	ROUND_START,
	HAND_PLAY,
	SCORING,
	AFTER_SCORING,
	FOLD,
	ROUND_CLEAR,
	TAROT_CARD_USE,
	PLANET_CARD_USE,
	MAX
}


public class JokerManager : MonoBehaviour
{
	[SerializeField]
	string testJoker;

	public static JokerManager instance;
	Camera Camera_Card;

	public List<PLANETINFO> list_PlanteInfo = new List<PLANETINFO>();
	public List<TAROTINFO> list_TarotInfo = new List<TAROTINFO>();

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
		StageManager.OnStateChange += OnStateChange;
		MainGame.OnCardGameStateChange += OnCardGameStateChange;
		CardDataManager.OnEvaluateChange += OnEvaluateChange;

		Camera_Card = GameObject.Find("Camera_Card").GetComponent<Camera>();
	}

	public void DeleteAllJokerEffect()
	{
		ReserveAllJokerEffect(false);
		Apply_JokerEffect();
	}

	void ReserveAllJokerEffect(bool value)
	{
		for (int i = 0; i < list_Joker_Active.Count; i++)
		{
			Card_Joker card = list_Joker_Active[i].GetComponent<Card_Joker>();

			if (card != null)
			{
				card.reserve_effect = value;
			}
		}
	}

	void Apply_JokerEffect()
	{
		for (int i = 0; i < list_Joker_Active.Count; i++)
		{
			Card_Joker card = list_Joker_Active[i].GetComponent<Card_Joker>();

			if (card != null)
			{
				// 자식 중에 ParticleSystem이 있는지 확인
				ParticleSystem[] particles = card.GetComponentsInChildren<ParticleSystem>(true);

				if (card.reserve_effect)
				{
					if (particles.Length == 0)
						EffectManager.instance.CreateEffect(4, card.transform.position, card.transform);
				}
				else
				{
					foreach (var ps in particles)
					{
						GameObject.Destroy(ps.gameObject);
					}
				}
			}
		}
	}

	/*void Check_Joker_With_Card()
	{
		Card[] selectedCards = CardsManager.the.SelectCards();

		foreach (var card in selectedCards)
		{
			if (card.cardData.include)
			{
				int value = 0;
				if (card.cardData.value <= CardValue.VALUE_10)
					value = (int)card.cardData.value + 2;
				else if (card.cardData.value == CardValue.VALUE_A)
					value = 11;
				else
					value = 10;

				if (card.cardData.type == CardType.TYPE_DIAMONDS)
				{
					CheckJokerType("JOKER.type_3", true);
				}

				if (card.cardData.value == CardValue.VALUE_J || card.cardData.value == CardValue.VALUE_Q || card.cardData.value == CardValue.VALUE_K)
				{
					CheckJokerType("JOKER.type_5", true);
				}

				CheckJokerType("JOKER.type_6", true);
			}
		}
	}*/

	void Check_Joker_With_Result(Wins type)
	{
		int result;
		for (int i = 0; i < list_Joker_Active.Count; i++)
		{
			Card_Joker card = list_Joker_Active[i].GetComponent<Card_Joker>();

			for (int j = 0; j < card.type.list_Condition.Count; j++)
				if (Check_Joker_Condition(card.type.list_Condition[i], out result))
				{

				}
		}
	}

	public void OnEvaluateChange(Wins type)
	{
		ReserveAllJokerEffect(false);

		if (type < 0)
		{
			Apply_JokerEffect();
			return;
		}

		Check_Joker(JOKERTIMING.MAX, true);

		/*Check_Joker_With_Card();
		Check_Joker_With_Result(type);*/

		Apply_JokerEffect();
	}

	private void Start()
	{
		list_All_Joker = CsvManager.instance.GetJokerList();
		list_All_Tarot = CsvManager.instance.GetTarotList();

		//rsh_temp
		/*list_All_Tarot.RemoveAll(t =>
					t.id == "1" ||
					t.id == "2" ||
					t.id == "3" ||
					t.id == "4" ||
					t.id == "5" ||
					t.id == "6" ||
					t.id == "7" ||
					t.id == "8" ||
					t.id == "9" ||
					t.id == "10"
					);*/

		string[] test = testJoker.Split('/');

		for (int i = 0; i < test.Length; i++)
		{
			int num = String_To_Int(test[i]) - 1;

			if (num >= 0 && num < list_All_Joker.Count)
				Achieve(list_All_Joker[num]);
		}

		Init_Planet_Info();

		//Achieve(list_All_Joker[32]);
		/*Achieve(list_All_Joker[33]);
		Achieve(list_All_Joker[34]);
		Achieve(list_All_Joker[35]);*/

		//Achieve(list_All_Joker[23]);
		/*Achieve(list_All_Joker[2]);
		Achieve(list_All_Joker[3]);
		Achieve(list_All_Joker[4]);
		Achieve(list_All_Joker[5]);
		Achieve(list_All_Joker[6]);*/

		/*Achieve(list_All_Joker[10]);
		Achieve(list_All_Joker[11]);
		Achieve(list_All_Joker[12]);
		Achieve(list_All_Joker[13]);*/
	}



	public List<GameObject> list_Joker_Active = new List<GameObject>();
	public List<GameObject> list_Planet_Active = new List<GameObject>();
	public List<GameObject> list_Tarot_Active = new List<GameObject>();
	List<JOKER> list_All_Joker = new List<JOKER>();
	List<JOKER> list_My_Joker = new List<JOKER>();
	List<PLANET> list_My_Planet = new List<PLANET>();
	List<TAROT> list_All_Tarot = new List<TAROT>();
	List<TAROT> list_My_Tarot = new List<TAROT>();

	public GameObject item_joker;
	public GameObject item_planet;
	public GameObject item_tarot;
	public GameObject item_card;

	public void Achieve(JOKER joker)
	{
		if (list_My_Joker.Count >= 5)
			return;

		list_My_Joker.Add(joker);
		SettingJoker();
	}

	public void Achieve(PLANET joker)
	{
		if (list_My_Planet.Count >= 2)
			return;

		list_My_Planet.Add(joker);
		SettingPlanet();
	}

	public void Achieve(TAROT joker)
	{
		if (list_My_Tarot.Count >= 2)
			return;

		list_My_Tarot.Add(joker);
		SettingTarot();

	}

	public void Start_Planet_Event(PLANET type)
	{
		PLANETINFO info = JokerManager.instance.list_PlanteInfo.FirstOrDefault(p => p.type == type);

		int chips = info.chips + Paytable.the.GetChips(info.wintype);

		int mult = info.mult + Paytable.the.GetWinMultiplier(info.wintype);

		int level = Paytable.the.GetLevel(info.wintype) + 2;

		UI_Card.instance.Runinfo_Event(8 - (int)info.wintype, chips, mult, level);
	}


	public PLANETINFO UsePlanet(PLANET type , bool simple = false)
	{
		PLANETINFO info = list_PlanteInfo.FirstOrDefault(p => p.type == type);

		Paytable.the.EnhanceLevel(info.wintype);
		Paytable.the.EnhanceMultiplier(info.wintype, info.mult);
		Paytable.the.EnhanceChips(info.wintype, info.chips);

		//결과 갱신-------------------------------
		CardsManager.the.EvaluateHand();
		CardDataManager.instance.SetResult();
		CardsManager.the.Count_Num_Select();

		if (simple == false)
		{
			list_My_Planet.Remove(type);
			SettingPlanet();
		}

		return info;
	}

	public TAROTINFO UseTarot(TAROT type)
	{
		TAROTINFO info = list_TarotInfo.FirstOrDefault(p => p.type.id == type.id);

		/*Paytable.the.EnhanceLevel(info.wintype);
		Paytable.the.EnhanceMultiplier(info.wintype, info.mult);
		Paytable.the.EnhanceChips(info.wintype, info.chips);*/

		//결과 갱신-------------------------------
		CardsManager.the.EvaluateHand();
		CardDataManager.instance.SetResult();
		CardsManager.the.Count_Num_Select();

		list_My_Tarot.Remove(type);
		SettingTarot();

		return info;
	}

	public void InitMyJoker()
	{
		list_My_Joker.Clear();
		SettingJoker();
	}


	void Init_Planet_Info()
	{
		/*string[] desc =
		{
			"지구" , "(Lv.1)<color=#F08D00>하이 카드</color> 을(를)\n레벨업합니다.\n <color=red>+1 </color> 배수 및\n<color=#206BE3>+10 </color> 개의 칩",
			"수성" , "(Lv.1)<color=#F08D00>페어</color> 을(를)\n레벨업합니다.\n <color=red>+1 </color> 배수 및\n<color=#206BE3>+15 </color> 개의 칩",
			"천왕성" , "(Lv.1)<color=#F08D00>투 페어</color> 을(를)\n레벨업합니다.\n <color=red>+1 </color> 배수 및\n<color=#206BE3>+20 </color> 개의 칩",
			"금성" , "(Lv.1)<color=#F08D00>트리플</color> 을(를)\n레벨업합니다.\n <color=red>+2 </color> 배수 및\n<color=#206BE3>+20 </color> 개의 칩",
			"토성" , "(Lv.1)<color=#F08D00>스트레이트</color> 을(를) 레벨업합니다.\n <color=red>+3 </color> 배수 및\n<color=#206BE3>+30 </color> 개의 칩",
			"목성" , "(Lv.1)<color=#F08D00>플러시</color> 을(를)\n레벨업합니다.\n <color=red>+2 </color> 배수 및\n<color=#206BE3>+15 </color> 개의 칩",			
			"화성" , "(Lv.1)<color=#F08D00>풀하우스</color> 을(를)\n레벨업합니다.\n <color=red>+3 </color> 배수 및\n<color=#206BE3>+15 </color> 개의 칩",
			"명왕성" , "(Lv.1)<color=#F08D00>포카드</color> 을(를)\n레벨업합니다.\n <color=red>+3 </color> 배수 및\n<color=#206BE3>+30 </color> 개의 칩",
			"해왕성" , "(Lv.1)<color=#F08D00>스트레이트플러시</color>을(를) 레벨업합니다.\n <color=red>+4 </color> 배수 및\n<color=#206BE3>+40 </color> 개의 칩",
		};*/

		AddPlanetInfo(PLANET.type_1, Wins.WIN_HIGH_CARD, 1, 10);
		AddPlanetInfo(PLANET.type_2, Wins.WIN_ONE_PAIR, 1, 15);
		AddPlanetInfo(PLANET.type_3, Wins.WIN_TWO_PAIR, 1, 20);
		AddPlanetInfo(PLANET.type_4, Wins.WIN_THREE_OF_A_KIND, 2, 20);
		AddPlanetInfo(PLANET.type_5, Wins.WIN_STRAIGHT, 3, 30);
		AddPlanetInfo(PLANET.type_6, Wins.WIN_FLUSH, 2, 15);
		AddPlanetInfo(PLANET.type_7, Wins.WIN_FULL_HOUSE, 3, 15);
		AddPlanetInfo(PLANET.type_8, Wins.WIN_FOUR_OF_A_KIND, 3, 30);
		AddPlanetInfo(PLANET.type_9, Wins.WIN_STRAIGHT_FLUSH, 4, 40);


		for (int i = 0; i < list_All_Tarot.Count; i++)
		{
			switch (list_All_Tarot[i].id)
			{
				case "4":
				case "5":
				case "6":
				case "7":
					AddTarotInfo(list_All_Tarot[i], Wins.WIN_HIGH_CARD, 1, 10);
					break;
			}
		}

		/*AddTarotInfo(TAROT.type_2, Wins.WIN_ONE_PAIR, 1, 15);
		AddTarotInfo(TAROT.type_3, Wins.WIN_TWO_PAIR, 1, 20);
		AddTarotInfo(TAROT.type_4, Wins.WIN_THREE_OF_A_KIND, 2, 20);*/
		/*AddTarotInfo(TAROT.type_5, Wins.WIN_STRAIGHT, 3, 30);
		AddTarotInfo(TAROT.type_6, Wins.WIN_FLUSH, 2, 15);
		AddTarotInfo(TAROT.type_7, Wins.WIN_FULL_HOUSE, 3, 15);
		AddTarotInfo(TAROT.type_8, Wins.WIN_FOUR_OF_A_KIND, 3, 30);
		AddTarotInfo(TAROT.type_9, Wins.WIN_STRAIGHT_FLUSH, 4, 40);*/
	}

	void AddPlanetInfo(PLANET type, Wins wintype, int mult, int chips)
	{
		PLANETINFO info = new PLANETINFO();
		info.type = type;
		info.wintype = wintype;
		info.mult = mult;
		info.chips = chips;

		list_PlanteInfo.Add(info);
	}

	void AddTarotInfo(TAROT type, Wins wintype, int mult, int chips)
	{
		TAROTINFO info = new TAROTINFO();
		info.type = type;
		info.wintype = wintype;
		info.mult = mult;
		info.chips = chips;

		list_TarotInfo.Add(info);
	}

	void OnStateChange(STAGESTATE state)
	{
		switch (state)
		{
			case STAGESTATE.PRE_START:
				//InitMyJoker();
				break;

			case STAGESTATE.START:

				break;
		}
	}

	void OnCardGameStateChange(byte gamestate)
	{
		if (gamestate == MainGame.STATE_WAIT_HOLD)
		{

		}
	}

	public void StartFight()
	{
		//Event_Joker("JOKER.type_4");
	}



	/*JOKER GetRealJoker(string id)
	{
		for (int i = 0; i < list_My_Joker.Count; i++)
		{
			if (list_My_Joker[i].id == id)
			{
				return list_My_Joker[i];
			}
		}
	}*/


	public string GetJokerDescription(JOKER _joker)
	{
		for (int i = 0; i < list_My_Joker.Count; i++)
		{
			if (list_My_Joker[i].id == _joker.id)
			{
				JOKER joker = list_My_Joker[i];

				return joker.GetDescription();
			}
		}

		return _joker.GetDescription();
	}

	/*string UpdateDescription(JOKER joker)
	{
		string description = joker.description;
		description = description.Replace("[basevalue]", joker.basevalue.ToString());
		description = description.Replace("[increase]", joker.increase.ToString());
		return description;
	}*/

	float GetValue(JOKER type, string valuetype)
	{
		float value = 0;

		for (int i = 0; i < list_My_Joker.Count; i++)
		{
			if (list_My_Joker[i].id == type.id)
			{
				JOKER joker = list_My_Joker[i];

				if (valuetype == "[basevalue]")
					value = joker.basevalue;
				else if (valuetype == "[increase]")
					value = joker.increase;
				else if (valuetype == "[decrease]")
					value = joker.decrease;
				else
				{
					if (valuetype.Contains("@"))
					{
						string[] randvalue = valuetype.Split('@');

						int min = int.Parse(randvalue[0]);
						int max = int.Parse(randvalue[1]);

						value = UnityEngine.Random.Range(min, max + 1); // max 포함하려면 +1
					}
					else
						value = String_To_Float(valuetype);
				}

				break;
			}
		}

		return value;
	}

	float GetGrowth(JOKER type, CARDDATA data)
	{
		float value = 0;

		for (int i = 0; i < list_My_Joker.Count; i++)
		{
			if (list_My_Joker[i].id == type.id)
			{
				JOKER joker = list_My_Joker[i];

				if (data == CARDDATA.MULT)
					value = joker.self_mults;

				if (data == CARDDATA.CHIP)
					value = joker.self_chips;

				break;
			}
		}

		return value;
	}

	void SelfGrowth(JOKER type, CARDDATA data, float value = 0, CALC calc_type = CALC.EQUAL)
	{
		for (int i = 0; i < list_My_Joker.Count; i++)
		{
			if (list_My_Joker[i].id == type.id)
			{
				JOKER joker = list_My_Joker[i];

				switch (calc_type)
				{
					case CALC.EQUAL:
						joker.basevalue = value;
						break;

					case CALC.PLUS:
						joker.basevalue += value;
						break;

					case CALC.MINUS:
						joker.basevalue -= value;
						break;

					case CALC.MULTIPLE:
						joker.basevalue *= value;
						break;
				}

				if (joker.basevalue < 1)
					joker.basevalue = 1;

				/*if(data == CARDDATA.MULT)
					joker.self_mults += value;

				if (data == CARDDATA.CHIP)
					joker.self_chips += value;*/

				list_My_Joker[i] = joker;

				break;
			}
		}
	}

	public CardValue CalcValue(CardValue now, CALC calc, int value)
	{
		int currentIndex = (int)now;
		int newIndex = calc == CALC.PLUS ? currentIndex + value : currentIndex - value;

		int min = (int)CardValue.VALUE_2;
		int max = (int)CardValue.VALUE_A;

		newIndex = Math.Clamp(newIndex, min, max);

		return (CardValue)newIndex;
	}

	bool Check_Left_Active(int idx)
	{
		if (list_Joker_Active.Count > idx)
			return list_Joker_Active[idx].GetComponent<Card_Joker>().reserve_effect;

		return false;
	}

	public List<Card> realCountCards = new List<Card>();
	int joker_check_order;
	public IEnumerator Start_Check_Joker(JOKERTIMING timing, bool add_effect = false)
	{
		joker_check_order = 0;
		float speed = OptionManager.instance.Speed_Time;

		if (add_effect)
			speed = 0;

		bool firstjoker = true;

		for (int i = 0; i < list_Joker_Active.Count; i++)
		{
			joker_check_order = i;

			Card_Joker card = list_Joker_Active[i].GetComponent<Card_Joker>();

			bool is_copyJoker = false;

			if (card.copyJoker)
			{
				card = card.copyJoker;
				is_copyJoker = true;
			}

			//JOKER real_my_joker = GetRealJoker(card.type.id);

			var conditions = FindMatchingCondition(card, timing);

			int result = 1;

			foreach (var condition in conditions)
			{
				realCountCards.Clear();

				if (condition.todo != null)
				{
					CALC calc_type = String_To_Calc(condition.todo[0]);
					TARGET target_type = String_To_Target(condition.todo[3]);
					CARDDATA carddata = String_To_CardData(condition.todo[1]);
					TARGET where = String_To_Target(condition.todo[7]);
					float value = GetValue(card.type, condition.todo[2]);

					if (calc_type == CALC.COPY)
					{
						if (joker_check_order == 0)
							continue;
						else
						{
							//conditions = CopyCondition(list_Joker_Active[joker_check_order - 1].GetComponent<Card_Joker>(), card);
							card.copyJoker = list_Joker_Active[joker_check_order - 1].GetComponent<Card_Joker>();

							/*list_Joker_Active[i].GetComponent<Card_Joker>().type.basevalue = list_Joker_Active[joker_check_order - 1].GetComponent<Card_Joker>().type.basevalue;
							list_Joker_Active[i].GetComponent<Card_Joker>().type.increase = list_Joker_Active[joker_check_order - 1].GetComponent<Card_Joker>().type.increase;
							list_Joker_Active[i].GetComponent<Card_Joker>().type.decrease = list_Joker_Active[joker_check_order - 1].GetComponent<Card_Joker>().type.decrease;
							list_Joker_Active[i].GetComponent<Card_Joker>().type.self_mults = list_Joker_Active[joker_check_order - 1].GetComponent<Card_Joker>().type.self_mults;
							list_Joker_Active[i].GetComponent<Card_Joker>().type.self_chips = list_Joker_Active[joker_check_order - 1].GetComponent<Card_Joker>().type.self_chips;*/

							continue;
						}
					}

					if (Check_Joker_Condition(condition, out result, target_type == TARGET.PLAYINGCARD ? value : 0))
					{
						if (add_effect)
						{
							list_Joker_Active[i].GetComponent<Card_Joker>().reserve_effect = add_effect;
						}
						else
						{

							if (firstjoker == true)
							{
								firstjoker = false;
								yield return new WaitForSeconds(CsvManager.instance.GetEvent_Time(CSV_EVENT.JOKER_DELAY) * speed);
							}

							if (where != TARGET.PLAYINGCARD)
								if (condition.todo[4] == "by")
									value *= result;

							//float value_total = value + GetGrowth(card.type, carddata);

							//joker 자신이 증가
							if (target_type != TARGET.PLAYINGCARD)
							{
								GameObject obj = EffectManager.instance.CreateEffect(0,
																					  list_Joker_Active[i].transform.position - new Vector3(0, 0.5f, 0)
								);

								if (obj && obj.GetComponent<Count>())
								{
									obj.GetComponent<Count>().SetString(string.Format("{0}{1}", String_To_Symbol(calc_type), value));
									obj.GetComponent<Count>().SetBackColor(new Color(32f / 255f, 112f / 255f, 236f / 255f));
								}
							}

							switch (target_type)
							{
								case TARGET.PLAYINGCARD:
									CardsManager.the.Card_Growth(carddata, value);
									break;

								case TARGET.TOTAL:
									CardDataManager.instance.SetMyData(carddata, value, calc_type);
									break;

								case TARGET.SELF:
									if (!is_copyJoker)
										SelfGrowth(card.type, carddata, value, calc_type);
									break;
							}

							if (list_Joker_Active[i].GetComponent<Card_Joker>().GetComponent<Animator>())
								list_Joker_Active[i].GetComponent<Card_Joker>().GetComponent<Animator>().Play("Card_Jocker_Active");

							if (SoundManager.instance)
								SoundManager.instance.PlaySound("Card_Jocker_Active_01");

							yield return new WaitForSeconds(0.5f * speed);
						}
					}
				}
			}
		}
	}

	public void Check_Joker(JOKERTIMING timing, bool add_effect = false)
	{
		var sortedCards = CardsManager.the.gameCards.OrderBy(card => card.transform.position.x).ToArray();

		foreach (var card in sortedCards)
		{
			card.cardData.scoring = true;
		}

		for (int i = 0; i < list_Joker_Active.Count; i++)
		{
			Card_Joker card = list_Joker_Active[i].GetComponent<Card_Joker>();

			if (card.copyJoker)
				card = card.copyJoker;

			var conditions = FindMatchingCondition(card, timing);
			int result;
			foreach (var condition in conditions)
			{
				realCountCards.Clear();

				if (condition.todo != null && Check_Joker_Condition(condition, out result))
				{
					if (add_effect)
					{
						list_Joker_Active[i].GetComponent<Card_Joker>().reserve_effect = add_effect;
					}
					else
					{
						if (card.GetComponent<Animator>())
							card.GetComponent<Animator>().Play("Card_Jocker_Active");
					}
				}
			}
		}

		foreach (var card in sortedCards)
		{
			card.cardData.scoring = false;
		}
	}

	public List<JOKER_CONDITION> FindMatchingCondition(Card_Joker card, JOKERTIMING timing)
	{
		if (card != null)
		{
			if (timing == JOKERTIMING.MAX)
				return card.type.list_Condition.ToList(); // 모든 조건 리턴

			return card.type.list_Condition
				.Where(cond => cond.timing == timing)
				.ToList(); // 일치하는 조건만 리턴
		}

		return new List<JOKER_CONDITION>(); // 카드가 null일 경우 빈 리스트 반환
	}

	public List<JOKER_CONDITION> CopyCondition(Card_Joker card_from, Card_Joker card_to)
	{
		if (card_from != null && card_to != null)
		{
			// 새로운 리스트 생성
			List<JOKER_CONDITION> result = new List<JOKER_CONDITION>();

			// card_to의 첫 번째 조건 유지
			if (card_to.type.list_Condition.Count > 0)
			{
				result.Add(card_to.type.list_Condition[0]); // 얕은 복사 아닌 값 복사
			}

			// card_from의 모든 조건을 복사
			foreach (var cond in card_from.type.list_Condition)
			{
				result.Add(cond); // struct이므로 깊은 복사
			}

			// 복사된 리스트를 card_to에 할당
			card_to.type.list_Condition = result;

			return result;
		}

		return new List<JOKER_CONDITION>();
	}

	/*public JOKER_CONDITION? FindMatchingCondition(Card_Joker card, JOKERTIMING timing)
	{
		if (card != null)
		{
			if (timing == JOKERTIMING.MAX)
				return card.type.list_Condition[0];

			return card.type.list_Condition.FirstOrDefault(cond => cond.timing == timing);
		}
		return null;
	}*/

	public bool Check_Joker_Condition(JOKER_CONDITION condition, out int result, float growth = 0)
	{
		result = 0;

		string condition_main = condition.todo[4];
		string condition_sub = "";
		string condition_num = "";

		// count 조건 파싱
		if (condition_main.StartsWith("count"))
		{
			// 연산자 리스트
			string[] operators = { "=", ">", "<" };

			foreach (string op in operators)
			{
				int opIndex = condition_main.IndexOf(op);
				if (opIndex != -1)
				{
					condition_sub = op;
					condition_num = condition_main.Substring(opIndex + 1); // 숫자 부분
					condition_main = "count"; // 메인 조건은 count로 고정
					break;
				}
			}
		}

		/*if (condition.todo[0].Contains("copy"))
		{
			return Check_Left_Active(joker_check_order);
		}*/

		switch (condition_main)
		{
			case "nocondition":
				result = 1;
				return true;

			case "count":
				if (condition.todo[5] == "suite")
				{
					Check_Joker_Suite(condition.todo[6], condition.todo[7], out result, growth);
					return Check_Count(result, condition_sub, condition_num) && Check_Count(CardsManager.the.Count_Num_Select(), condition_sub, condition_num);
				}
				else if (condition.todo[5] == "number")
				{
					Check_Joker_Number(condition.todo[6], condition.todo[7], out result, growth);
					return Check_Count(result, condition_sub, condition_num) && Check_Count(CardsManager.the.Count_Num_Select(), condition_sub, condition_num);
				}
				else if (condition.todo[5] == "remain")
				{
					Check_Joker_Remain(condition.todo[6], condition.todo[7], out result, growth);
					return Check_Count(result, condition_sub, condition_num);
				}
				else if (condition.todo[5] == "exist")
				{
					return Check_Joker_Exist(condition.todo[6], condition.todo[7], out result, growth);
				}
				break;

			case "by":
				if (condition.todo[5] == "suite")
				{
					return Check_Joker_Suite(condition.todo[6], condition.todo[7], out result, growth);
				}
				else if (condition.todo[5] == "number")
				{
					return Check_Joker_Number(condition.todo[6], condition.todo[7], out result, growth);
				}
				else if (condition.todo[5] == "remain")
				{
					return Check_Joker_Remain(condition.todo[6], condition.todo[7], out result, growth);
				}
				else if (condition.todo[5] == "exist")
				{
					return Check_Joker_Exist(condition.todo[6], condition.todo[7], out result, growth);
				}
				break;

			case "include":
				if (condition.todo[5] == "rank")
				{
					return Check_Joker_Rank(condition.todo[6], condition.todo[7]);
				}
				else if (condition.todo[5] == "suite")
				{
					return Check_Joker_Suite(condition.todo[6], condition.todo[7], out result, growth);
				}
				else if (condition.todo[5] == "number")
				{
					return Check_Joker_Number(condition.todo[6], condition.todo[7], out result, growth);
				}
				else if (condition.todo[5] == "remain")
				{
					return Check_Joker_Remain(condition.todo[6], condition.todo[7], out result, growth);
				}
				break;

			case "exclude":
				break;
		}

		return false;
	}

	public bool Check_Count(int remain, string symbol, string count_num)
	{
		int num = String_To_Int(count_num);
		switch (symbol)
		{
			case "=":
				return remain == num;

			case ">":
				return remain > num;

			case "<":
				return remain < num;
		}

		return false;
	}

	public bool Check_Joker_Rank(string ranklist, string where)
	{
		Wins win_Type = (Wins)Paytable.the.GetCurrentWinIndex();

		string[] split = ranklist.Split('/');

		for (int i = 0; i < split.Length; i++)
		{
			Wins temp = String_To_Wins(split[i]);

			if (where == "playcard")
			{
				if (temp == win_Type)
					return true;
			}
			else
			{
				if (CardsManager.the.Evaluate_InMyHand(temp))
					return true;
			}
		}

		return false;
	}

	public bool Check_Joker_Remain(string suitelist, string where, out int result, float growth = 0)
	{
		result = 0;

		switch (suitelist)
		{
			case "hand":
				result = (int)CardDataManager.instance.GetMyData(CARDDATA.HAND);
				return true;

			case "discard":
				result = (int)CardDataManager.instance.GetMyData(CARDDATA.DISCARD);
				return true;
		}

		return false;
	}


	public bool Check_Joker_Exist(string suitelist, string where, out int result, float growth = 0)
	{
		result = 0;
		bool hasAny = false;

		string[] split = suitelist.Split('/');


		//string input = "52-all";

		// 정규표현식으로 숫자 또는 문자열 + 연산자 + 숫자 또는 문자열 파싱
		Match match = Regex.Match(split[0], @"(.+?)([+\-*/])(.+)");
		if (match.Success)
		{
			string left = match.Groups[1].Value;
			string op = match.Groups[2].Value;
			string right = match.Groups[3].Value;

			split = "1/2/3/4/5/6/7/8/9/10/11/12/13/14/15".Split('/');

			for (int i = 0; i < split.Length; i++)
			{
				CardValue value = String_To_CardValue(split[i]);

				if (CardsManager.the.Is_Have_Number(value, where, out int partial, growth))
				{
					hasAny = true; // 하나라도 있으면 true
					result += partial; // 결과 누적
				}
			}

			result = JokerManager.instance.realCountCards.Count;
			result = Calc(String_To_Int(left), op, result);

			hasAny = result > 0;
		}
		else
		{
			Console.WriteLine("파싱 실패");
		}

		return hasAny;
	}

	int Calc(int one, string op, int two)
	{
		switch (op)
		{
			case "+":
				return one + two;

			case "-":
				return one - two;

			case "*":
				return one * two;

			case "/":
				return one / two;
		}

		return 0;
	}

	public bool Check_Joker_Number(string suitelist, string where, out int result, float growth = 0)
	{
		result = 0;
		bool hasAny = false;

		string[] split = suitelist.Split('/');

		for (int i = 0; i < split.Length; i++)
		{
			CardValue value = String_To_CardValue(split[i]);

			if (CardsManager.the.Is_Have_Number(value, where, out int partial, growth))
			{
				hasAny = true; // 하나라도 있으면 true
				result += partial; // 결과 누적
			}
		}

		result = JokerManager.instance.realCountCards.Count;
		return hasAny;
	}

	public bool Check_Joker_Suite(string suitelist, string where, out int result, float growth = 0)
	{
		result = 0;
		string[] split = suitelist.Split('/');

		for (int i = 0; i < split.Length; i++)
		{
			CardType type = String_To_CardType(split[i]);

			if (CardsManager.the.Is_Have_Suite(type, where, out int partial, growth))
			{
				result += partial;
			}
		}

		result = JokerManager.instance.realCountCards.Count;
		return result > 0;
	}

	string String_To_Symbol(CALC calc_type)
	{
		switch (calc_type)
		{
			case CALC.EQUAL: return "=";
			case CALC.PLUS: return "+";
			case CALC.MINUS: return "-";
			case CALC.MULTIPLE: return "*";
		}

		return "";
	}

	TARGET String_To_Target(string name)
	{
		if (Enum.TryParse<TARGET>(name, ignoreCase: true, out var target))
		{
			// 성공적으로 파싱됨
			return target;
		}
		else
		{
			Console.WriteLine("Target 변환 실패");
			return TARGET.TOTAL;
		}
	}

	public CALC String_To_Calc(string name)
	{
		switch (name)
		{
			case "add":
			case "increase":
			case "plus":
				return CALC.PLUS;

			case "subtract":
			case "decrease":
			case "minus":
				return CALC.MINUS;

			case "multiple":
				return CALC.MULTIPLE;

			case "retrigger":
				return CALC.RETRIGGER;

			case "copy":
				return CALC.COPY;
		}

		return CALC.EQUAL;
	}

	CARDDATA String_To_CardData(string name)
	{
		switch (name)
		{
			case "mults":
				return CARDDATA.MULT;

			case "chips":
				return CARDDATA.CHIP;
		}

		return CARDDATA.MAX;
	}

	public CardType String_To_CardType(string name)
	{
		switch (name)
		{
			case "spade":
				return CardType.TYPE_SPADES;

			case "diamond":
				return CardType.TYPE_DIAMONDS;

			case "heart":
				return CardType.TYPE_HEARTS;

			case "club":
				return CardType.TYPE_CLUBS;

			case "random":
				int idx = UnityEngine.Random.Range(0, 4);
				return CardType.TYPE_SPADES + idx;
		}

		return CardType.TYPES_NO;
	}

	public CardValue String_To_CardValue(string name)
	{
		int num = String_To_Int(name);

		switch (num)
		{
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
				return (num - 2 + CardValue.VALUE_2);

			case 11:
				return CardValue.VALUE_J;

			case 12:
				return CardValue.VALUE_Q;

			case 13:
				return CardValue.VALUE_K;

			case 1: return CardValue.VALUE_A;
		}

		return CardValue.VALUES_NO;
	}

	public int String_To_Int(string input)
	{
		object numobj = String_To_Number(input);
		int value = Convert.ToInt32(numobj);
		return value;
	}

	public float String_To_Float(string input)
	{
		object numobj = String_To_Number(input);
		float value = Convert.ToSingle(numobj);
		return value;
	}

	public static object String_To_Number(string input)
	{
		// 먼저 int 시도
		if (int.TryParse(input, out int intResult))
			return intResult;

		// 다음 float 시도 (float.TryParse는 소수점까지 허용)
		if (float.TryParse(input, out float floatResult))
			return floatResult;

		// 숫자로 파싱 불가할 경우 null 반환
		return 0;
	}

	Wins String_To_Wins(string name)
	{
		switch (name)
		{
			case "onepair":
				return Wins.WIN_ONE_PAIR;

			case "twopair":
				return Wins.WIN_TWO_PAIR;

			case "triple":
				return Wins.WIN_THREE_OF_A_KIND;

			case "fourcard":
				return Wins.WIN_FOUR_OF_A_KIND;

			case "flush":
			case "fivecard":
				return Wins.WIN_FLUSH;

			case "fullhouse":
				return Wins.WIN_FULL_HOUSE;

			case "flushfive":
				return Wins.WIN_STRAIGHT_FLUSH;

			case "straight":
				return Wins.WIN_STRAIGHT;

			case "highcard":
				return Wins.WIN_HIGH_CARD;
		}

		return Wins.WINS_NO;
	}

	public int CheckJokerType(string id_joker, bool add_effect = false)
	{
		/*
		for (int i = 0; i < list_Joker_Active.Count; i++)
		{
			Card_Joker card = list_Joker_Active[i].GetComponent<Card_Joker>();

			if (card != null && card.id == id_joker)  // `card` 객체만 사용
			{
				if (add_effect)
				{
					card.reserve_effect = add_effect;
				}

				return i;
			}
		}
		*/
		return -1;
	}

	public void Event_Joker(string id_joker)
	{
		/*
		int idx = CheckJokerType(id_joker);

		if (idx == -1)
			return;

		GameObject card_joker = list_Joker_Active[idx];
		GameObject obj = EffectManager.instance.CreateEffect(0, card_joker.transform.position - new Vector3(0, 0.5f, 0));

		if (obj && obj.GetComponent<Count>())
		{
			switch (id_joker)
			{
				case "JOKER.type_1":
					obj.GetComponent<Count>().SetString("+4 Mult");
					obj.GetComponent<Count>().SetBackColor(Color.red);

					CardDataManager.instance.SetMyData(CARDDATA.MULT, 4, true);
					break;

				case "JOKER.type_2":
					obj.GetComponent<Count>().SetString("+20");
					obj.GetComponent<Count>().SetBackColor(new Color(32f / 255f, 112f / 255f, 236f / 255f));

					CardDataManager.instance.SetMyData(CARDDATA.CHIP, 20, true);
					break;

				case "JOKER.type_3":
					Destroy(obj);
					CardDataManager.instance.SetMyData(CARDDATA.MULT, 1, true);
					break;

				case "JOKER.type_4":
					obj.GetComponent<Count>().SetString("+1 Discard");
					obj.GetComponent<Count>().SetBackColor(Color.red);

					CardDataManager.instance.SetMyData(CARDDATA.DISCARD, 1, true);
					break;

				case "JOKER.type_5":
					Destroy(obj);
					CardDataManager.instance.SetMyData(CARDDATA.MULT, 2, true);
					break;

				case "JOKER.type_6":
					Destroy(obj);
					CardDataManager.instance.SetMyData(CARDDATA.CHIP, 5, true);
					break;
			}

			if (card_joker.GetComponent<Animator>())
				card_joker.GetComponent<Animator>().Play("Card_Jocker_Active");

			//StartCoroutine(ShakeCard(card_joker));
		}
		*/
	}

	IEnumerator ShakeCard(GameObject card)
	{
		Vector3 originalPos = card.transform.position;
		float shakeAmount = 0.02f; // 흔들리는 정도
		float shakeSpeed = 0.02f;  // 흔들리는 속도

		for (int i = 0; i < 3; i++) // 3번 흔들기
		{
			card.transform.position = originalPos + new Vector3(shakeAmount, 0, 0);
			yield return new WaitForSeconds(shakeSpeed);
			card.transform.position = originalPos - new Vector3(shakeAmount, 0, 0);
			yield return new WaitForSeconds(shakeSpeed);
		}

		card.transform.position = originalPos; // 원래 위치로 복구

		/*Vector3 targetPos = new Vector3(card.transform.position.x, hand_Event_Start_Y + 0.1f, 1.0f);
		card.SetTargetPos(targetPos, 0.1f);*/
	}

	void SettingJoker(/*int num = 3*/)
	{
		int num = list_My_Joker.Count;
		float width_one = 0.4f;
		float width_gap = 0.05f;

		float startx = -(num - 1) * (width_one + width_gap) / 2;
		startx -= 0.81f;

		for (int i = list_Joker_Active.Count - 1; i >= num; i--)
		{
			list_Joker_Active[i].SetActive(false);
		}

		//JOKER[] list_Temp = JokerManager.instance.GetJokerType_Reward(num);

		for (int i = 0; i < list_My_Joker.Count; i++)
		{
			float xPos = startx + i * (width_one + width_gap);
			Vector3 pos = new Vector3(xPos, 1.9f, 0);

			if (i < list_Joker_Active.Count)
			{
				list_Joker_Active[i].SetActive(true);
				list_Joker_Active[i].transform.position = pos;
				list_Joker_Active[i].GetComponent<Card_Joker>().SetJokerType(list_My_Joker[i]);
			}
			else if (JokerManager.instance.item_joker)
			{
				GameObject item = Instantiate(JokerManager.instance.item_joker, pos, Quaternion.identity);
				item.GetComponent<Card_Joker>().OnSelect += OnSelect;
				item.GetComponent<Card_Joker>().SetJokerType(list_My_Joker[i]);
				//item.GetComponent<Card_Joker>().SetJokerType(list_Temp[i]);
				item.GetComponent<Card_Joker>().SetShowType(SHOWTYPE.PLAY);
				item.GetComponent<Card_Joker>().SetOrderLayer(2);
				list_Joker_Active.Add(item);
			}
		}
	}

	void SettingPlanet(/*int num = 3*/)
	{
		int num = list_My_Planet.Count;
		float width_one = 0.4f;
		float width_gap = 0.18f;

		float startx = -(num - 1) * (width_one + width_gap) / 2;
		startx += 2.50f;

		for (int i = list_Planet_Active.Count - 1; i >= num; i--)
		{
			list_Planet_Active[i].SetActive(false);
		}

		//JOKER[] list_Temp = JokerManager.instance.GetJokerType_Reward(num);

		for (int i = 0; i < list_My_Planet.Count; i++)
		{
			float xPos = startx + i * (width_one + width_gap);
			Vector3 pos = new Vector3(xPos, 1.9f, 0);

			if (i < list_Planet_Active.Count)
			{
				list_Planet_Active[i].SetActive(true);
				list_Planet_Active[i].transform.position = pos;
				list_Planet_Active[i].GetComponent<Card_Planet>().SetPlanetType(list_My_Planet[i]);
			}
			else if (JokerManager.instance.item_planet)
			{
				GameObject item = Instantiate(JokerManager.instance.item_planet, pos, Quaternion.identity);
				item.GetComponent<Card_Planet>().OnSelect += OnSelect;
				item.GetComponent<Card_Planet>().SetPlanetType(list_My_Planet[i]);
				//item.GetComponent<Card_Planet>().SetJokerType(list_Temp[i]);
				item.GetComponent<Card_Planet>().SetShowType(SHOWTYPE.PLAY);
				item.GetComponent<Card_Planet>().SetOrderLayer(2);
				list_Planet_Active.Add(item);
			}
		}
	}

	void SettingTarot(/*int num = 3*/)
	{
		int num = list_My_Tarot.Count;
		float width_one = 0.4f;
		float width_gap = 0.18f;

		float startx = -(num - 1) * (width_one + width_gap) / 2;
		startx += 2.50f;

		for (int i = list_Tarot_Active.Count - 1; i >= num; i--)
		{
			list_Tarot_Active[i].SetActive(false);
		}

		//JOKER[] list_Temp = JokerManager.instance.GetJokerType_Reward(num);

		for (int i = 0; i < list_My_Tarot.Count; i++)
		{
			float xPos = startx + i * (width_one + width_gap);
			Vector3 pos = new Vector3(xPos, 1.9f, 0);

			if (i < list_Tarot_Active.Count)
			{
				list_Tarot_Active[i].SetActive(true);
				list_Tarot_Active[i].transform.position = pos;
				list_Tarot_Active[i].GetComponent<Card_Tarot>().SetTarotType(list_My_Tarot[i]);
			}
			else if (JokerManager.instance.item_tarot)
			{
				GameObject item = Instantiate(JokerManager.instance.item_tarot, pos, Quaternion.identity);
				item.GetComponent<Card_Tarot>().OnSelect += OnSelect;
				item.GetComponent<Card_Tarot>().SetTarotType(list_My_Tarot[i]);
				//item.GetComponent<Card_Tarot>().SetJokerType(list_Temp[i]);
				item.GetComponent<Card_Tarot>().SetShowType(SHOWTYPE.PLAY);
				item.GetComponent<Card_Tarot>().SetOrderLayer(2);
				list_Tarot_Active.Add(item);
			}
		}
	}

	void Update()
	{
		/*if (Input.touchCount > 0)
		{
			//foreach (Touch touch in Input.touches)
			{
				foreach (var obj in list_Joker_Active)
				{
					obj.GetComponent<Card_Joker>().ShowDesc(false);
				}
			}
		}*/

		if (Input.GetMouseButtonDown(0))
		{
			foreach (var obj in list_Joker_Active)
			{
				obj.GetComponent<Card_Joker>().ShowDesc(false);
			}

			foreach (var obj in list_Planet_Active)
			{
				obj.GetComponent<Card_Planet>().ShowDesc(false);
			}
		}
	}
	/*private bool isTouched = false;

	void Update()
	{
		isTouched = false;

		if (Input.touchCount > 0)
		{
			foreach (Touch touch in Input.touches)
			{
				CheckTouchOrClick(touch.position);
			}
		}

		if (Input.GetMouseButtonDown(0))
		{
			CheckTouchOrClick(Input.mousePosition);
		}

		if (!isTouched)
		{
			foreach (var obj in list_Joker_Active)
			{
				obj.GetComponent<Card_Joker>().ShowDesc(false);
			}
		}
	}

	void CheckTouchOrClick(Vector2 screenPosition)
	{
		PointerEventData eventData = new PointerEventData(EventSystem.current);
		eventData.position = screenPosition;

		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, results);

		foreach (RaycastResult result in results)
		{
			if (result.gameObject.tag.Contains("JOKER"))
			{
				isTouched = true;
				return;
			}
		}
	}
	*/
	void OnSelect(JOKER _type, int price)
	{
		foreach (var obj in list_Joker_Active)
		{
			Card_Joker card = obj.GetComponent<Card_Joker>();

			if (card != null && card.type.id == _type.id)  // `card` 객체만 사용
			{
				card.ShowDesc(true);
			}
		}
	}

	void OnSelect(PLANET type, int price)
	{
		foreach (var obj in list_Planet_Active)
		{
			Card_Planet card = obj.GetComponent<Card_Planet>();

			if (card != null && card.type == type)  // `card` 객체만 사용
			{
				card.ShowDesc(true);
			}
		}
	}

	void OnSelect(TAROT type, int price)
	{
		foreach (var obj in list_Tarot_Active)
		{
			Card_Tarot card = obj.GetComponent<Card_Tarot>();

			if (card != null && card.type.id == type.id)  // `card` 객체만 사용
			{
				card.ShowDesc(true);
			}
		}
	}

	public void Count_Event()
	{
		//list_Joker_Active
	}

	public JOKER[] GetJokerType_Reward(int num)
	{
		/*JOKER[] allJokers = (JOKER[])Enum.GetValues(typeof(JOKER)); // 모든 JOKER enum 값 가져오기
		List<JOKER> jokerList = new List<JOKER>(allJokers); // 리스트로 변환
		jokerList.RemoveAll(j => list_My_Joker.Contains(j)); // 이미 가진 JOKER 제거*/

		List<JOKER> jokerList = new List<JOKER>(list_All_Joker);
		jokerList.RemoveAll(j => list_My_Joker.Contains(j)); // 이미 가진 JOKER 제거
															 // 남아 있는 JOKER 개수 내에서 최대 num개 선택
		JOKER[] temp = new JOKER[Mathf.Min(num, jokerList.Count)];

		for (int i = 0; i < temp.Length; i++)
		{
			int randomIndex = UnityEngine.Random.Range(0, jokerList.Count); // 랜덤 인덱스 선택
			temp[i] = jokerList[randomIndex]; // 선택된 값 저장
			jokerList.RemoveAt(randomIndex); // 선택된 값 제거 (중복 방지)
		}

		return temp;
	}

	public PLANET[] GetPlanetType_Reward(int num)
	{
		PLANET[] allJokers = (PLANET[])Enum.GetValues(typeof(PLANET)); // 모든 JOKER enum 값 가져오기
		List<PLANET> jokerList = new List<PLANET>(allJokers); // 리스트로 변환
		jokerList.RemoveAll(j => list_My_Planet.Contains(j)); // 이미 가진 JOKER 제거

		// 남아 있는 JOKER 개수 내에서 최대 num개 선택
		PLANET[] temp = new PLANET[Mathf.Min(num, jokerList.Count)];

		for (int i = 0; i < temp.Length; i++)
		{
			int randomIndex = UnityEngine.Random.Range(0, jokerList.Count); // 랜덤 인덱스 선택
			temp[i] = jokerList[randomIndex]; // 선택된 값 저장
			jokerList.RemoveAt(randomIndex); // 선택된 값 제거 (중복 방지)
		}

		return temp;
	}

	public TAROT[] GetTarotType_Reward(int num)
	{
		// 전체 타로 목록을 복사
		List<TAROT> allJokers = new List<TAROT>(list_All_Tarot);

		// 내가 가진 타로들의 string ID를 모아 HashSet으로
		HashSet<string> myTarotIds = new HashSet<string>(list_My_Tarot.Select(t => t.id));

		// 내가 가진 타로 ID에 해당하는 카드 제거
		allJokers.RemoveAll(j => myTarotIds.Contains(j.id));

		// 무작위로 num개 선택
		TAROT[] temp = new TAROT[Mathf.Min(num, allJokers.Count)];
		for (int i = 0; i < temp.Length; i++)
		{
			int randomIndex = UnityEngine.Random.Range(0, allJokers.Count);
			temp[i] = allJokers[randomIndex];
			allJokers.RemoveAt(randomIndex); // 중복 방지
		}

		return temp;
	}

}
