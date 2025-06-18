using UnityEngine;
using TMPro;

public class UI_StageData : MonoBehaviour
{
    public MYDATA myType;
    TextMeshProUGUI txt_Value;

    void Awake()
    {
        txt_Value = GetComponent<TextMeshProUGUI>();
    }

    public void OnStageDataChange(int chapter, int stage)
    {
        if (txt_Value == null)
            return;

        txt_Value.text = string.Format("ROUND {0} - {1}", chapter + 1, stage + 1);
    }
}
