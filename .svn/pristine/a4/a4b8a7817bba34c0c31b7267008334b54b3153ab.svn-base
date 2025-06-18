using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Dice : MonoBehaviour
{
	public string item_Summon;

	public GameObject ui_ItemReplace;
	public TextMeshProUGUI txt_Num_Dice;
	public GameObject img_Gear;
	public GameObject img_Dice;
	bool is_Auto;
	float time_Auto = 0;
	bool is_ActiveDice_OneTime;
	int num_Dice;
	ITEMGRADE item_Summon_Grade;
	public int Num_Dice
	{
		set
		{
			num_Dice = value;
			if (txt_Num_Dice)
				txt_Num_Dice.text = num_Dice.ToString();
		}
		get
		{
			return num_Dice;
		}
	}


	private void Start()
	{
		is_Auto = false;
		is_ActiveDice_OneTime = false;

		Num_Dice = 30;
		ui_ItemReplace.SetActive(false);

		StartCoroutine(AutoDice());
	}

	float maxRotationSpeed = 360f; // 최대 회전 속도 (초당 각도)
	float accelerationTime = 5f; // 가속 지속 시간 (초)

	private float currentSpeed = 0f; // 현재 회전 속도
	//private float elapsedTime = 0f; // 경과 시간

	void RotateDice()
	{
		// 가속 시간 내에서 속도를 증가시킴
		if (time_Auto < accelerationTime)
		{
			time_Auto += Time.deltaTime;
			currentSpeed = Mathf.Lerp(0, maxRotationSpeed, time_Auto / accelerationTime);
		}
		else
		{
			// 최대 속도로 유지
			currentSpeed = maxRotationSpeed;
		}

		// 회전 적용 (Z축 기준)
		img_Dice.GetComponent<RectTransform>().Rotate(0f, 0f, currentSpeed * Time.deltaTime);
	}

	IEnumerator AutoDice()
	{
		time_Auto = 0;

		while (true)
		{
			if (is_ActiveDice_OneTime && !ui_ItemReplace.activeSelf)
			{
				time_Auto += Time.deltaTime;

				RotateDice();

				if (time_Auto > accelerationTime)
				{
					time_Auto = 0;

					SummonItem();
					ShowNewItem();

					is_ActiveDice_OneTime = is_Auto;

					if (is_ActiveDice_OneTime)
						ActiveDice();
				}
			}

			if (is_Auto && img_Gear != null)
			{
				// 중심 기준으로 Z축을 기준으로 회전
				img_Gear.GetComponent<RectTransform>().Rotate(0f, 0f, 150 * Time.deltaTime);
			}

			yield return null;
		}
	}

	ITEMGRADE SummonGrade()
	{
		float[] pct_grade =
		{
			70,
			95,
			98,
			100
		};

		float pct = Random.Range(0, 100);

		for (int i = 0; i < pct_grade.Length; i++)
		{
			if (pct <= pct_grade[i])
				return (ITEMGRADE)i;
		}

		return (ITEMGRADE)0;
	}

	void SummonItem()
	{
		item_Summon_Grade = SummonGrade();

		ITEMTYPE type = (ITEMTYPE)(Random.Range(0, 100) % (int)ITEMTYPE.MAX);

		item_Summon = CsvManager.instance.GetItemID(type, item_Summon_Grade).ToString();
		ui_ItemReplace.GetComponent<UI_ItemReplace>().item_new_id = item_Summon;
	}

	public void Btn_Auto()
	{
		is_Auto = !is_Auto;

		if (is_Auto)
		{
			ActiveDice();
		}
		else
		{
			is_ActiveDice_OneTime = false;
		}
	}

	public void Btn_Dice()
	{
		ActiveDice();
	}

	void ActiveDice()
	{
		if (ui_ItemReplace.activeSelf)
			return;

		if (Num_Dice > 0)
		{
			Num_Dice--;

			time_Auto = 0;
			is_ActiveDice_OneTime = true;
		}
		else
		{
			is_Auto = false;
			is_ActiveDice_OneTime = false;
		}
	}

	void ShowNewItem()
	{
		if (ui_ItemReplace.activeSelf)
			return;

		if (is_Auto)
		{
			if((int)item_Summon_Grade < (int)ITEMGRADE.LEGEND)
			{
				return;
			}
		}

		ui_ItemReplace.SetActive(true);
	}
}
