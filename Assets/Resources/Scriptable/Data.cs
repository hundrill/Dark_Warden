using UnityEngine;

[CreateAssetMenu(fileName = "TESTDATA", menuName = "TESTDATA")]

[System.Serializable]
public class StringDropdown
{
    [HideInInspector] public string[] options = new string[] { "TwoHandSword", "Sting", "JumpAttack", "Upper", "Dagger", "GreatSword", "Debuff", "Buff" };
    public string selectedOption;
}

public class IndentAttribute : PropertyAttribute
{
    public int Level { get; private set; }

    public IndentAttribute(int level = 1)
    {
        Level = level; // 들여쓰기 수준
    }
}

public class Data : ScriptableObject
{
    [Header("몬스터 체력")]
    [SerializeField]
    int Monsterhp; // 몬스터 체력
    public int _Monsterhp
    {
        get { return Monsterhp; }
    }

    [Space(2)]
    [Header("쿨타임 간격")]
    [SerializeField]
    float Cooltime_Interval; // 쿨타임 간격

    public float _Cooltime_Interval
    {
        get { return Cooltime_Interval; }
    }

    //[Space(2)]
    [Header("     첫번째 스킬")]
    [Header("     타입")]
    [SerializeField]
    private StringDropdown skill_1 = new StringDropdown();
    [Header("가격")]
    [Range(1,5)]
    public int value_1;

    [Header("     두번째 스킬")]
    [Header("     타입")]
    [SerializeField]
    private StringDropdown skill_2 = new StringDropdown();
    [Header("가격")]
    [Range(1, 5)]
    public int value_2;


    [Header("     세번째 스킬")]
    [Header("     타입")]
    [SerializeField]
    private StringDropdown skill_3 = new StringDropdown();
    [Header("가격")]
    [Range(1, 5)]
    public int value_3;

    public int Convert_Skill(int idx)
    {
        switch(idx)
        {
            case 0: return FindIdx(skill_1.selectedOption);
            case 1: return FindIdx(skill_2.selectedOption);
            case 2: return FindIdx(skill_3.selectedOption);
        }

        return 0;        
    }

    public string[] options = new string[] { "TwoHandSword", "Sting", "JumpAttack", "Upper", "Dagger", "GreatSword", "Debuff", "Buff" };

    int FindIdx(string name)
    {
        int num = 0;
        foreach(var obj in options)
        {
            num++;

            if (obj.Contains(name))
                return num;
        }

        return -1;
    }

    /*// 드롭다운에 표시될 스킬 타입 옵션들
    public string[] SkillOptions = { "TwoHandSword", "Sting", "JumpAttack", "Upper", "Dagger", "GreatSword", "Debuff", "Buff" };*/
}