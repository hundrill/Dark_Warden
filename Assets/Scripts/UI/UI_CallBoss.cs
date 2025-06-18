using UnityEngine;
using TMPro;
using static StageManager;
using UnityEngine.UI;

public class UI_CallBoss : MonoBehaviour
{
	public TextMeshProUGUI txt;
	public Image image_Step;
	private void Start()
	{
		if (StageManager.instance)
			StageManager.OnStateChange += OnStateChange;

		if (StageManager.instance)
			StageManager.instance.OnStepCallBoss += OnStepCallBoss;

	}

	public void OnStepCallBoss(int step)
	{
		if (image_Step)
			image_Step.fillAmount = step * 100 / 3 * 0.01f;
	}

	public void Boss()
	{
		if(StageManager.instance.step_Call_Boss >= 3)
			StageManager.instance.state = STAGESTATE.BOSS_APPEAR;

		return;

		switch (StageManager.instance.state)
		{
			case STAGESTATE.BOSS_CAN_CALL:
				StageManager.instance.state = STAGESTATE.BOSS_APPEAR;
				break;
		}
	}

	public void OnStateChange(STAGESTATE _state)
	{
		if (txt == null)
			return;

		switch (_state)
		{
			case STAGESTATE.START:
				txt.text = "보스 대기";
				break;

			case STAGESTATE.BOSS_CAN_CALL:
				txt.text = "보스 소환";
				break;

			case STAGESTATE.BOSS_APPEAR:
				txt.text = "전투중";
				break;
		}
	}
}
