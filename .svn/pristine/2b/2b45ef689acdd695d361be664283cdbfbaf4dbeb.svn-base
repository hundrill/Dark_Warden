using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public delegate void onSkillUse(int idx_preset);
    public onSkillUse OnSkillUse;

    public delegate void onCoolTime_Remain(int remain_cooltime);
    public onCoolTime_Remain OnCoolTime_Remain;

    public static SkillManager instance;

    public List<int> list_SkillReady = new List<int>();

    int num_CoolTime;
    public int Num_CoolTime
    {
        set
        {
            num_CoolTime = value;
            OnCoolTime_Remain?.Invoke(num_CoolTime);
        }
        get { return num_CoolTime; }
    }

    int now_skill_idx;
    public bool Changed(int kk)
    {
        if(now_skill_idx == kk)
                return true;

        return false;
    }

    bool is_Skill_Auto;
	public bool ToggleUseSkill()
    {
        is_Skill_Auto = !is_Skill_Auto;

        return is_Skill_Auto;
	}

	private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        list_SkillReady.Clear();

        now_skill_idx = -1;

        Num_CoolTime = 0;

        is_Skill_Auto = false;

	}

    private void Start()
    {
        Num_CoolTime = 0;
    }

    public void OnCoolTime_Full()
    {
        Num_CoolTime++;
    }


    public void OnSkillClose(int idx_skill)
    {
        list_SkillReady.RemoveAll(x => x == idx_skill);
    }

    public void OnSkillReady(int idx_skill)
    {
        /*switch (idx_skill)
        {
            case 3:
            case 4:
                return;
        }*/

        list_SkillReady.Add(idx_skill);

        //Character.instance.ReserveSkill(list_SkillReady[0]);
    }

    public bool Is_Can_Use_Skill()
    {
        if (list_SkillReady.Count > 0)
            return true;

        return false;
    }

    public int Use_Next_Skill()
    {
        if (list_SkillReady.Count > 0)
        {
            int firstValue = list_SkillReady[0];
            
            OnSkillUse?.Invoke(firstValue);

            list_SkillReady.RemoveAt(0);

            now_skill_idx = firstValue;
            /*switch (firstValue)
            {
                case 4:
                case 5:
                    firstValue -= 2;
                    break;
            }*/

            return firstValue;
        }

        return -1;
    }
}
