using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Ui : MonoBehaviour
{
	public delegate void onSkillReady(int idx_preset);
	public onSkillReady OnSkillReady;

	public delegate void onSkillClose(int idx_preset);
	public onSkillClose OnSkillClose;

	public Image cooltime;
	public Image use;

	public Image toggle;

	[SerializeField]
	float duration;

	[SerializeField]
	int myidx_skill;

	bool is_Ready_Use;
	private void OnEnable()
	{
		LoadToggle();

		StartCoroutine(Setting());
	}

	public void ToggleAuto()
	{
		cooltime.fillAmount = SkillManager.instance.ToggleUseSkill() ? 1 : 0;
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
				SkillManager.instance.OnSkillUse += OnSkillUse;
				OnSkillReady += SkillManager.instance.OnSkillReady;
				OnSkillClose += SkillManager.instance.OnSkillClose;

				StartCoroutine(CoolTime());
				break;
			}

			yield return null;
		}
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
			cooltime.fillAmount = Random.Range(0, 100) * 0.01f;
		}
		else
		{
			cooltime.fillAmount = 1;
			OnSkillClose?.Invoke(myidx_skill);
		}
	}

	IEnumerator CoolTime()
	{
		is_Ready_Use = false;
		float fillweight = 1 / (duration * 60); //frame count
		cooltime.fillAmount = Random.Range(0, 100) * 0.01f;

		if (use)
			use.fillAmount = 0;

		SettingToggle();

		while (true)
		{
			if (toggle_use)
			{
				if (is_Ready_Use == false)
					cooltime.fillAmount -= fillweight;

				if (cooltime.fillAmount <= 0)
				{
					if (is_Ready_Use == false)
					{
						OnSkillReady?.Invoke(myidx_skill);
						is_Ready_Use = true;
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
