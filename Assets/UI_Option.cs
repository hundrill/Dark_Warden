using TMPro;
using UnityEngine;

public class UI_Option : MonoBehaviour
{
    int speed_Max = 3;

    public TextMeshProUGUI txt_Speed;
    private void Awake()
	{
        
    }

	private void OnEnable()
	{
		if (UI_Card.instance)
			UI_Card.instance.is_Dlg_Open = true;
	}

	private void Start()
	{
        GetComponentInChildren<Canvas>().worldCamera = GameObject.Find("Camera_Card").GetComponent<Camera>();

        RefreshText();
    }

	void RefreshText()
    {
        if (txt_Speed)
            txt_Speed.text = string.Format("{0}", OptionManager.instance.Speed + 1);
    }

	public void Exit()
	{
		gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

	private void OnDisable()
	{
		UI_Card.instance.is_Dlg_Open = false;
	}

	public void Btn_Speed_Left()
    {
        OptionManager.instance.Speed--;

        if (OptionManager.instance.Speed < 0)
            OptionManager.instance.Speed = speed_Max;

        RefreshText();
    }

    public void Btn_Speed_Right()
    {
        OptionManager.instance.Speed++;

        if (OptionManager.instance.Speed > speed_Max)
            OptionManager.instance.Speed = 0;

        RefreshText();
    }
}
