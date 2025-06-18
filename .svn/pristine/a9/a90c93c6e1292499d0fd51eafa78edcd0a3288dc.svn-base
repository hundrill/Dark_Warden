using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;

public class UI_Show : MonoBehaviour
{
	public enum SHOWTYPE
	{
		SHOW,
		ACTIVE,
		DEACTIVE,
		OFF
	}

	public MAINUI mode;
	public SHOWTYPE show;

	public STAGESTATE stage_state;
	public SHOWTYPE show_state;

	//public Material grayMaterial;
	bool is_Connected = false;
	bool is_Call_Other;

	public float time_live;
	float _time_live;
	private void Awake()
	{
		//grayMaterial = Resources.Load<Material>("Effect/Shader/Custom_GrayButton");
		is_Call_Other = false;
	}

	IEnumerator Life()
    {
		while(true)
        {
			_time_live--;

			if (_time_live < 0)
				break;

			yield return null;
        }

		gameObject.SetActive(false);
    }

    private void OnEnable()
    {
		if (time_live != 0)
		{
			_time_live = time_live;
			StartCoroutine(Life());
		}
	}

    public void OnMainUiChange(MAINUI _mode)
	{
		if (mode == MAINUI.NONE)
			return;

		if (mode == _mode)
		{
			switch (show)
			{
				case SHOWTYPE.SHOW:
					gameObject.SetActive(true);
					if (GetComponent<Button>())
						GetComponent<Button>().interactable = true;
					break;

				case SHOWTYPE.ACTIVE:
					gameObject.SetActive(true);
					break;

				case SHOWTYPE.DEACTIVE:
					gameObject.SetActive(true);
					if (GetComponent<Button>())
						GetComponent<Button>().interactable = false;

					if (GetComponent<Image>())
						GetComponent<Image>().material = UI_Main.instance.grayMaterial;
					break;

				case SHOWTYPE.OFF:
					gameObject.SetActive(false);
					return;
			}
		}
		else
		{
			switch (show)
			{
				case SHOWTYPE.SHOW:
					gameObject.SetActive(false);
					if (GetComponent<Button>())
						GetComponent<Button>().interactable = false;
					return;

				case SHOWTYPE.ACTIVE:
					gameObject.SetActive(true);
					break;

				case SHOWTYPE.DEACTIVE:
					gameObject.SetActive(true);
					if (GetComponent<Button>())
						GetComponent<Button>().interactable = true;

					if (GetComponent<Image>())
						GetComponent<Image>().material = null;
					//grayMaterial
					break;

				case SHOWTYPE.OFF:
					gameObject.SetActive(true);
					break;
			}
		}

		if (is_Call_Other == false)
		{
			is_Call_Other = true;
			OnStateChange(StageManager.instance.state);
			is_Call_Other = false;
		}
	}

	public void OnStateChange(STAGESTATE _state)
	{
		if (stage_state == STAGESTATE.NONE)
			return;

		if (stage_state == _state)
		{
			switch (show_state)
			{
				case SHOWTYPE.SHOW:
					gameObject.SetActive(true);
					if (GetComponent<Button>())
						GetComponent<Button>().interactable = true;
					break;

				case SHOWTYPE.ACTIVE:
					gameObject.SetActive(true);
					break;

				case SHOWTYPE.DEACTIVE:
					gameObject.SetActive(true);
					if (GetComponent<Button>())
						GetComponent<Button>().interactable = false;

					if (GetComponent<Image>())
						GetComponent<Image>().material = UI_Main.instance.grayMaterial;
					break;

				case SHOWTYPE.OFF:
					gameObject.SetActive(false);
					return;
			}
		}
		else
		{
			switch (show_state)
			{
				case SHOWTYPE.SHOW:
					gameObject.SetActive(false);
					if (GetComponent<Button>())
						GetComponent<Button>().interactable = false;
					return;

				case SHOWTYPE.ACTIVE:
					gameObject.SetActive(true);
					break;

				case SHOWTYPE.DEACTIVE:
					gameObject.SetActive(true);
					if (GetComponent<Button>())
						GetComponent<Button>().interactable = true;

					if (GetComponent<Image>())
						GetComponent<Image>().material = null;
					//grayMaterial
					break;

				case SHOWTYPE.OFF:
					gameObject.SetActive(true);
					break;
			}
		}

		if (is_Call_Other == false)
		{
			is_Call_Other = true;
			OnMainUiChange(UI_Main.instance.mainType);
			is_Call_Other = false;
		}
	}
}
