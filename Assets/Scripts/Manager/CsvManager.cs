using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Windows;
using static Ashkatchap.AnimatorEvents.EventSMB;
using static UnityEngine.Rendering.DebugUI;


public enum CSV_JOKER
{
	id,
	nameText,
	descText,
	basevalue,
	increase,
	decrease,
	timing_draw,
	timing_round_start,
	timing_hand_play,
	timing_scoring,
	timing_after_scoring,
	timing_fold,
	timing_round_clear,
	timing_tarot_card_use,
	timing_planet_card_use,
	price,
	sprite
}


public enum CSV_EVENT
{
	JOKER_DELAY,
	MAX
}

public class CsvManager : MonoBehaviour
{
	public List<Dictionary<string, object>> list_csv_item;
	public List<Dictionary<string, object>> list_csv_text;
	public List<Dictionary<string, object>> list_csv_stage;
	public List<Dictionary<string, object>> list_csv_joker;
	public List<Dictionary<string, object>> list_csv_tarot;
	public List<Dictionary<string, object>> list_csv_event;

	public static CsvManager instance;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		list_csv_item = CSVReader.Read("Csv/item");
		list_csv_text = CSVReader.Read("Csv/text");
		list_csv_stage = CSVReader.Read("Csv/stage");
		list_csv_joker = CSVReader.Read("Csv/joker");
		list_csv_tarot = CSVReader.Read("Csv/tarot");
		list_csv_event = CSVReader.Read("Csv/event");
	}

	public float GetEvent_Time(CSV_EVENT type , string field = "time")
	{
		if (list_csv_event == null)
			return 0;

		string find_id = type.ToString();

		object value = null;
		foreach (var line in list_csv_event)
		{
			if (!line.ContainsKey(field))
				break;

			if (line["id"].ToString().Equals(find_id))
			{
				value = line[field];
				break;
			}
		}

		return String_To_Float(value.ToString());
	}

	public List<TAROT> GetTarotList()
	{
		List<TAROT> jokerList = new List<TAROT>();
		int value;

		foreach (var line in list_csv_tarot)
		{
			TAROT one = new TAROT();

			one.id = line["id"].ToString();
			one.name = line["nameText"].ToString();
			one.description = line["descText"].ToString();
			one.functionvalue = line["functionvalue"].ToString();
			one.list_Condition = new List<TAROT_CONDITION>();

			/*one.basevalue = String_To_Float(line["basevalue"].ToString());
			one.increase = String_To_Float(line["increase"].ToString());
			one.decrease = String_To_Float(line["decrease"].ToString());*/

			one.self_mults = 0;
			one.self_chips = 0;

			int.TryParse(line["selectcount"].ToString(), out value);
			one.select_count = value;

			int.TryParse(line["drawnumber"].ToString(), out value);
			one.draw_count = value;

			int.TryParse(line["randomcount"].ToString(), out value);
			one.random_count = value;

			string[] all = Parsing_Condition(line["functiontype"].ToString());
			TAROT_CONDITION condition = new TAROT_CONDITION();						
			condition.todo = all;

			one.list_Condition.Add(condition);

			int.TryParse(line["price"].ToString(), out value);
			one.price = value;
			int.TryParse(line["sprite"].ToString(), out value);
			one.sprite = value;

			jokerList.Add(one);
		}

		return jokerList;
	}

	public List<JOKER> GetJokerList()
	{
		List<JOKER> jokerList = new List<JOKER>();
		int value;

		foreach (var line in list_csv_joker)
		{
			JOKER one = new JOKER();

			one.id = line["id"].ToString();
			one.name = line["nameText"].ToString();
			one.description = line["descText"].ToString();
			one.list_Condition = new List<JOKER_CONDITION>();

			one.basevalue = String_To_Float(line["basevalue"].ToString());
			one.increase = String_To_Float(line["increase"].ToString());
			one.decrease = String_To_Float(line["decrease"].ToString());

			one.self_mults = 0;
			one.self_chips = 0;

			for (int i = 0; i < (int)JOKERTIMING.MAX; i++)
			{
				string field_name = (CSV_JOKER.timing_draw + i).ToString();
				if (line[field_name].ToString().Length > 0)
				{
					string[] all_condition = line[field_name].ToString().Split(',');

					for (int j = 0; j < all_condition.Length; j++)
					{
						string[] all = Parsing_Condition(all_condition[j].ToString());
						JOKER_CONDITION condition = new JOKER_CONDITION();
						condition.timing = JOKERTIMING.DRAW + i;
						condition.todo = all;

						one.list_Condition.Add(condition);
					}
				}
			}

			int.TryParse(line["price"].ToString(), out value);
			one.price = value;
			int.TryParse(line["sprite"].ToString(), out value);
			one.sprite = value;

			

			jokerList.Add(one);
		}

		return jokerList;
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

	string[] Parsing_Condition(string condition)
	{
		string[] parts = condition.Split('_');

		return parts;
	}

	public object GetJoker(string find_id, CSV_JOKER csv_field)
	{
		if (list_csv_joker == null)
			return null;

		string field = csv_field.ToString();

		object value = null;
		foreach (var line in list_csv_joker)
		{
			if (!line.ContainsKey(field))
				break;

			if (line["id"].ToString().Equals(find_id))
			{
				value = line[field];
				break;
			}
		}

		return value;
	}

	public List<MONSTER> GetStageMonsterList(string stageId)
	{
		List<MONSTER> monsterList = new List<MONSTER>();

		int index = 0;
		while (true)
		{
			index++;
			string monsterType = GetStage(stageId, index.ToString())?.ToString();

			if (string.IsNullOrEmpty(monsterType))
				break;

			// 문자열을 MONSTER enum으로 변환
			if (System.Enum.TryParse(monsterType, true, out MONSTER monster))
			{
				monsterList.Add(monster);
			}
			else
			{
				// 변환 실패 시 로그 또는 오류 처리
				Console.WriteLine($"Invalid monster type: {monsterType}");
			}
		}

		return monsterList;
	}


	public object GetStage(string find_id, string spawn_num)
	{
		if (list_csv_stage == null)
			return null;

		string field = spawn_num.ToString().ToLower();

		object value = null;
		foreach (var line in list_csv_stage)
		{
			if (!line.ContainsKey(field))
				break;

			if (line["id"].ToString().Equals(find_id))
			{
				value = line[field];
				break;
			}
		}

		return value;
	}

	public object GetItem(string find_id, ITEM type)
	{
		if (list_csv_item == null)
			return null;

		string field = type.ToString().ToLower();

		object value = null;
		foreach (var line in list_csv_item)
		{
			if (!line.ContainsKey(field))
				break;

			if (line["id"].ToString().Equals(find_id))
			{
				value = line[field];
				break;
			}
		}

		return value;
	}

	public object GetItemID(ITEMTYPE type, ITEMGRADE grade)
	{
		if (list_csv_item == null)
			return null;

		string _type = type.ToString().ToLower();
		string _grade = grade.ToString().ToLower();

		object value = null;

		foreach (var line in list_csv_item)
		{
			if (!line.ContainsKey("type"))
				if (!line.ContainsKey("grade"))
					break;

			if (line["type"].ToString().Equals(_type))
				if (line["grade"].ToString().Equals(_grade))
				{
					value = line["id"];
					break;
				}
		}
		return value;
	}

	public string GetTextData(string find_id, LANGUAGE language = LANGUAGE.TEXT_KO, bool is_ConvertShapString = true)
	{
		//language = DATA.LANGUAGE.TEXT_EN; //rsh_temp
		//language = OptionLogic.instance.nowLangauge;

		if (list_csv_text == null)
			return "";

		string field = language.ToString().ToLower();

		object value = null;
		foreach (var line in list_csv_text)
		{
			if (!line.ContainsKey("id"))
				break;

			if (line["id"].Equals(find_id))
			{
				value = line[field];
				break;
			}
		}

		if (value == null)
		{
			Debug.LogWarning("Error in text.csv : " + find_id);
			return "";
			//return string.Format("text.csv 파일에 {0} 이 없음", find_id);
		}

		string sentence = value.ToString();
		if (is_ConvertShapString)
		{
			sentence = TextGlobal.CheckConvert(sentence);
		}

		return sentence;
	}
}
