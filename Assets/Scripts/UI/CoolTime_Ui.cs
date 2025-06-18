using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static SkillManager;
public class CoolTime_Ui : MonoBehaviour
{
	public delegate void onCoolTime_Full();
	public onCoolTime_Full OnCoolTime_Full;

	public Image cooltime;
	public Image use;

	public Image toggle;

	public TextMeshProUGUI txt_Num;

	//[SerializeField]
	float duration;

	[SerializeField]
	int myidx_skill;

	bool is_Ready_Use;

	private void OnEnable()
	{
		LoadToggle();

		StartCoroutine(Setting());

		duration = 30;
	}

	private void SaveToggle()
	{
		string ToggleKey = string.Format("ToggleKey{0}", myidx_skill);
		PlayerPrefs.SetInt(ToggleKey, toggle_use ? 1 : 0); // bool -> int 변환
		PlayerPrefs.Save(); // 저장 강제 실행
		Debug.Log("Toggle Saved: " + toggle_use);
	}

	// 로드 메서드
	private void LoadToggle()
	{
		string ToggleKey = string.Format("ToggleKey{0}", myidx_skill);
		// 저장된 값이 없으면 기본값(true) 사용
		toggle_use = PlayerPrefs.GetInt(ToggleKey, 1) == 1; // int -> bool 변환
	}

	IEnumerator Setting()
	{
		while (true)
		{
			if (SkillManager.instance)
			{
				SkillManager.instance.OnCoolTime_Remain += OnCoolTime_Remain;
				OnCoolTime_Full += SkillManager.instance.OnCoolTime_Full;
				txt_Num.text = SkillManager.instance.Num_CoolTime.ToString();

				StartCoroutine(CoolTime());
				break;
			}

			yield return null;
		}
	}

	public void OnCoolTime_Remain(int remain_cooltime)
	{
		if (txt_Num)
			txt_Num.text = remain_cooltime.ToString();
	}

	public void OnSkillUse(int idx_skill)
	{
		if (myidx_skill == idx_skill)
		{
			StartCoroutine(UseEffect());
		}
	}

	bool _toggle_use;

	bool toggle_use
	{
		get
		{
			return _toggle_use;
		}

		set
		{
			_toggle_use = value;


			SettingToggle();
			SaveToggle();
		}
	}

	public void Toggle_Usable()
	{
		toggle_use = !toggle_use;
	}

	void SettingToggle()
	{
		if (toggle_use)
		{
			cooltime.fillAmount = 1;
		}
		else
		{
			cooltime.fillAmount = 1;
			//OnCoolTime_Close?.Invoke();
		}
	}

	IEnumerator CoolTime()
	{
		is_Ready_Use = false;
		
		cooltime.fillAmount = 1;// Random.Range(0, 100) * 0.01f;

		if (use)
			use.fillAmount = 0;

		SettingToggle();

		int duration_default = 10;

		while (true)
		{
			if (DataManager.instance != null && DataManager.instance.TESTDATA != null)
			{
				duration = DataManager.instance.TESTDATA._Cooltime_Interval != 0
					? DataManager.instance.TESTDATA._Cooltime_Interval
					: duration_default; // 기본값 설정
			}
			else
			{
				Debug.LogWarning("DataManager or TESTDATA is null.");
				duration = duration_default; // 기본값 설정
			}

			if (StageManager.instance.state != STAGESTATE.REWARD_START)
			{
				float fillweight = 1 / (duration * 60); //frame count

				if (toggle_use)
				{
					if (is_Ready_Use == false)
						cooltime.fillAmount -= fillweight;

					if (cooltime.fillAmount <= 0)
					{
						if (is_Ready_Use == false)
						{
							OnCoolTime_Full?.Invoke();
							cooltime.fillAmount = 1;
							//OnSkillReady?.Invoke(myidx_skill);
							//is_Ready_Use = true;


						}
					}
				}
			}

			yield return null;
		}
	}

	IEnumerator UseEffect()
	{
		float fillweight = 0.005f;

		while (true)
		{
			use.fillAmount += fillweight;

			if (use.fillAmount >= 0.74f)
			{
				use.fillAmount = 0;

				is_Ready_Use = false;
				cooltime.fillAmount = 1;
				break;
			}

			yield return null;
		}
	}
}
