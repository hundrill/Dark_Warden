using System.Collections;
using UnityEngine;
using VideoPokerKit;

public enum STAGESTATE
{
	PRE_START,
	START,
	REWARD_START,
	REWARD_FINISH,
	BOSS_CAN_CALL,
	BOSS_APPEAR,
	BOSS_CLEAR,
	NONE,
}

public class StageManager : MonoBehaviour
{
	public static StageManager instance;

	public delegate void onStateChange(STAGESTATE _state);
	public static onStateChange OnStateChange;

	public delegate void onStageDataChange(int chapter, int stage);
	public onStageDataChange OnStageDataChange;

	int _num_Chapter;
	public int num_Chapter
	{
		get { return _num_Chapter; }
		set
		{
			_num_Chapter = value;
			OnStageDataChange?.Invoke(num_Chapter, num_Stage);
		}
	}

	int _num_Stage;
	public int num_Stage
	{
		get { return _num_Stage; }
		set
		{
			_num_Stage = value;
			OnStageDataChange?.Invoke(num_Chapter, num_Stage);
		}
	}

	int _step_Call_Boss;
	public int step_Call_Boss
	{
		get { return _step_Call_Boss; }
		set
		{
			_step_Call_Boss = value;
			OnStepCallBoss?.Invoke(_step_Call_Boss);

			if (state == STAGESTATE.BOSS_APPEAR)
			{
				NextStage();
				return;
			}

			/*if (_step_Call_Boss == 3)
			{
				state = STAGESTATE.BOSS_CAN_CALL;
			}*/
		}
	}

	public delegate void onStepCallBoss(int step);
	public onStepCallBoss OnStepCallBoss;

	STAGESTATE _state;
	[HideInInspector]
	public STAGESTATE state
	{
		get { return _state; }

		set
		{
			_state = value;

			OnStateChange?.Invoke(_state);
		}
	}

	private void Awake()
	{
		QualitySettings.vSyncCount = 0;  // VSync 비활성화
		Application.targetFrameRate = 60; // 60FPS로 제한

		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		UI_StageData[] objectsWithAScript = FindObjectsByType<UI_StageData>(FindObjectsSortMode.None);

		foreach (UI_StageData a in objectsWithAScript)
		{
			OnStageDataChange += a.OnStageDataChange;
		}

		num_Chapter = 0;
		num_Stage = 0;

		_state = STAGESTATE.START;
	}

	public void StartStage()
	{
		StartCoroutine(InitStage());
	}
	public void NextStage()
	{
		num_Stage++;
		MainGame.the.ResetGame(true);
		StartCoroutine(InitStage());
	}

	void Start()
	{
		UI_Show[] objectsWithAScript = Resources.FindObjectsOfTypeAll<UI_Show>();

		foreach (UI_Show a in objectsWithAScript)
		{
			OnStateChange += a.OnStateChange;
		}

		StartStage();
	}

	IEnumerator InitStage()
	{
		/*if (CardsManager.the)
		{
			CardsManager.the.ClearHand();
			yield return new WaitForSeconds(0.5f);
		}*/

		state = STAGESTATE.PRE_START;

		yield return null;


		//rsh_temp
		/*if (CardsManager.the)
		{
			CardsManager.the.ClearHand();
			yield return new WaitForSeconds(0.5f);
		}*/

		state = STAGESTATE.START;
		step_Call_Boss = 0;


		/*yield return null;

		state = STAGESTATE.BOSS_CAN_CALL;*/
	}

}
