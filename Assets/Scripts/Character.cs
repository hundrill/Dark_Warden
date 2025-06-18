using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
using System.Linq;
using Unity.VisualScripting;
using static Character;
using UnityEngine.SceneManagement;
using static DataManager;
using UnityEngine.SocialPlatforms.Impl;
using VideoPokerKit;
using static CardDataManager;

public class Character : MonoBehaviour
{
	public GameObject weapon;
	public GameObject[] list_Weapon;
	HP_Ui hp_Com;

	int maxhp;

	public int myAtk;
	public int myDef;

	int _hp;
	public int hp
	{
		get { return _hp; }
		set
		{
			_hp = value;

			if (hp_Com)
				hp_Com.SetFill(_hp * 100 / maxhp * 0.01f);

			animator.SetInteger("hp", _hp);
		}
	}

	//public Animator[] animator;
	public Animator animator;
	public Rigidbody rb;
	public GameObject target;
	public GameObject target_pos;

	private List<GameObject> monsterList; // MONSTER 오브젝트 리스트

	public static Character instance;
	public float interval = 3;
	public float spped_run;

	/*public UnityAnimationEvent OnAnimationStart;
    public UnityAnimationEvent OnAnimationComplete;*/

	public delegate void onHit();
	public onHit OnHit;

	public delegate void onDie();
	public onDie OnDie;

	public delegate void onAniStart(string name_ani);
	public onAniStart OnAniStart;

	public delegate void onAniEnd(string name_ani);
	public onAniEnd OnAniEnd;

	public delegate void onSpawnArrive(Vector3 pos, MONSTER type);
	public onSpawnArrive OnSpawnArrive;

	Vector3 pos_Start;

	void Awake()
	{
		if (gameObject.scene.name.Contains("Lobby"))
			return;

		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		animator = GetComponent<Animator>();
		for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
		{
			AnimationClip clip = animator.runtimeAnimatorController.animationClips[i];

			AnimationEvent animationStartEvent = new AnimationEvent();
			animationStartEvent.time = 0;
			animationStartEvent.functionName = "AnimationStartHandler";
			animationStartEvent.stringParameter = clip.name;

			AnimationEvent animationEndEvent = new AnimationEvent();
			animationEndEvent.time = clip.length;
			animationEndEvent.functionName = "AnimationCompleteHandler";
			animationEndEvent.stringParameter = clip.name;

			clip.AddEvent(animationStartEvent);
			clip.AddEvent(animationEndEvent);
		}

		AddHpBar();

		StageManager.OnStateChange += OnStateChange;
		CardDataManager.OnCardDataChange += OnCardDataChange;
		//MainGame.newGame += NewGame;
	}

	/*void NewGame()
	{
		NextRound();
	}*/

	public void OnCardDataChange(CARDDATA type, float value)
	{
		switch (type)
		{
			case CARDDATA.HAND:
				if (hp_Com)
					hp_Com.SettingHand((int)value);
				break;

			case CARDDATA.DISCARD:
				if (hp_Com)
					hp_Com.SettingDiscard((int)value);
				break;
		}
	}

	public void UseHand()
	{
		if (hp_Com)
			hp_Com.UseHand();
	}

	public void UseDiscard()
	{
		if (hp_Com)
			hp_Com.UseDiscard();
	}

	public void AnimationStartHandler(string name)
	{
		if (name.ToLower().Contains("idle"))
		{
			Check_Skill_Use();
		}
		else if (name.ToLower().Contains("skill"))
		{
			Skill();
			
			ReserveSkill(0);
		}
		else if (name.ToLower().Contains("attack"))
		{
			interval = 2;
		}

		OnAniStart?.Invoke(name);

		Debug.Log($"{name} animation start.");
		//OnAnimationStart?.Invoke(name);
	}
	public void AnimationCompleteHandler(string name)
	{
		OnAniEnd?.Invoke(name);

		Debug.Log($"{name} animation complete.");
		//OnAnimationComplete?.Invoke(name);
	}

	public bool is_Fight;

	bool is_Running;
	private void Start()
	{
		if (gameObject.scene.name.Contains("Lobby"))
			return;

		// 초기 회전을 저장
		initialRotation = transform.rotation;
		pos_Start = transform.position;

		


		OnAniEnd += MyAniEnd;
		rb = GetComponent<Rigidbody>();

		//Initialize();

		if (DataManager.instance)
			DataManager.instance.OnItemEquip += OnItemEquip;

	}

	void OnItemEquip(ITEMTYPE type, string id)
	{
		Recalc();
	}

	void Recalc()
	{
		int _atk = 1;
		int _def = 5;
		int _hp = 10;

		for (int i = 0; i < DataManager.instance.Equip.Length; i++)
		{
			_atk += DataManager.instance.GetItemValue(DataManager.instance.Equip[i], ITEM.ATK);
			_def += DataManager.instance.GetItemValue(DataManager.instance.Equip[i], ITEM.DEF);
			_hp += DataManager.instance.GetItemValue(DataManager.instance.Equip[i], ITEM.HP);
		}

		myAtk = _atk;
		myDef = _def;
		maxhp = _hp;

		if (hp_Com)
			hp_Com.SetFill(hp * 100 / maxhp * 0.01f);
	}

	void Initialize()
	{
		target_pos = null;
		transform.position = pos_Start;

		Recalc();
		InitHp();

		ResetAnimator();

		if (!gameObject.scene.name.Contains("Lobby"))
		{
			is_Running = false;
			is_Fight = false;

			if (goToEnemyCoroutine != null)
			{
				StopCoroutine(goToEnemyCoroutine); // 기존 코루틴 종료
			}

			goToEnemyCoroutine = StartCoroutine(GoToEnemy());
		}
	}

	void OnStateChange(STAGESTATE state)
	{
		switch (state)
		{
			case STAGESTATE.START:
				Initialize();
				break;

			case STAGESTATE.BOSS_APPEAR:
				StartCoroutine(Boss_Appear());
				break;
		}
	}

	IEnumerator Boss_Appear()
	{
		is_Running = false;
		SetAni("arrive");

		if (goToEnemyCoroutine != null)
		{
			StopCoroutine(goToEnemyCoroutine); // 기존 코루틴 종료
		}

		target = MonsterManager.instance.now_Monster;

		yield return null;

		StartCoroutine(Fight());
	}

	void MyAniEnd(string name)
	{
		if (name.ToLower().Contains("die"))
		{
			DialogManager.instance.ShowDialog(DIALOG.OVER);
		}
	}

	void InitHp()
	{
		/*if (DataManager.instance.TESTDATA._Monsterhp != 0)
			maxhp = hp = DataManager.instance.TESTDATA._Monsterhp;*/

		hp = maxhp;
	}

	void AddHpBar()
	{
		/*
		Transform headTransform = FindHeadTransform(transform);

		if (headTransform != null)
		{
			Debug.Log($"Head transform found: {headTransform.name}");
		}
		else
		{
			Debug.LogWarning("Head transform not found!");
			return;
		}

		GameObject hpcanvas = Instantiate(MonsterManager.instance.hpcanvas_hero);

		hpcanvas.transform.parent = headTransform;

		//hpcanvas.GetComponent<RectTransform>().localPosition = new Vector3(0, -headTransform.position.y, 0);        

		hpcanvas.GetComponent<RectTransform>().localPosition = new Vector3(0.55f, 0.1f, -2.0f);

		hp_Com = hpcanvas.GetComponentInChildren<HP_Ui>();
		hp_Com.fill.color = RGBToColor(255, 255, 0); // Cyan

		*/

		hp_Com = GetComponentInChildren<HP_Ui>();
		/*if (hp_Com && hp_Com.fill)
			hp_Com.fill.color = RGBToColor(255, 255, 0); // Cyan*/
	}

	public static Color RGBToColor(float r, float g, float b, float a = 255f)
	{
		return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
	}

	Transform FindHeadTransform(Transform root)
	{
		return transform;

		foreach (Transform child in root.GetComponentsInChildren<Transform>())
		{
			// "Head"라는 단어가 포함된 이름을 탐색
			if (child.name/*.ToLower()*/.Contains("Dummy_Camera"))
			{
				return child;
			}
		}
		return null; // 찾지 못하면 null 반환

	}
	/*void FindMonster()
    {
        // 씬에서 모든 MONSTER 태그 오브젝트 찾기
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");

        // 플레이어와의 거리를 기준으로 정렬
        monsterList = monsters
            .OrderBy(monster => Vector3.Distance(transform.position, monster.transform.position))
            .ToList();

        // 결과 출력
        foreach (var monster in monsterList)
        {
            Debug.Log($"Monster: {monster.name}, Distance: {Vector3.Distance(transform.position, monster.transform.position)}");
        }
    }*/

	public void Damage()
	{
		if (Is_Idle())
		{
			animator.SetTrigger("hit");
		}

		if (hp > 0)
			hp -= 1;

		animator.SetTrigger("die");
		OnHit?.Invoke();

		/*if (hp <= 0)
		{
			OnDie?.Invoke();
		}
		else
		{
			animator.SetTrigger("die");
			OnHit?.Invoke();
		}*/
	}

	public void Attack()
	{
		SetAni("hit");
	}

	public void Hit()
	{
		if (Is_Skill_Name() != "")
			Hitted_Cri();
		else
			Hitted();
	}

	public void Hit_Cri()
	{
		Hitted_Cri();
	}

	public void Sound(int num)
	{
		if (SoundManager.instance)
			SoundManager.instance.PlaySound(num.ToString());
	}

	public void Sound(string soundKey)
	{
		if (SoundManager.instance)
			SoundManager.instance.PlaySound(soundKey);
	}

	public void GreateSword()
	{
		//list_Weapon[1].SetActive(true);

		if (weapon != null)
			weapon = list_Weapon[1];
	}

	public void Shake()
	{
		if (CameraLogic.instance)
			CameraLogic.instance.Shake(1.0f, 0.1f);
	}

	public void Hitted()
	{
		if (target && target.GetComponent<Monster>())
			target.GetComponent<Monster>().Hitted();
	}

	public void Hitted_Cri()
	{
		if (target && target.GetComponent<Monster>())
			target.GetComponent<Monster>().Hitted_Cri();
	}

	public void SetAni(string name)
	{
		animator.SetTrigger(name);
	}

	public bool MonsterDie()
	{
		int remain_hp = 0;
		if (MonsterManager.instance && MonsterManager.instance.now_Monster)
		{
			if (MonsterManager.instance.now_Monster.GetComponent<Monster>())
				remain_hp = MonsterManager.instance.now_Monster.GetComponent<Monster>().hp;
		}
		else
			return true;

		Debug.Log("<color=yellow>GGG:remainhp : </color>" + remain_hp.ToString());

		if (remain_hp <= 0)
		{
			return true;
		}

		return false;
	}

	public bool LastHit()
	{
		int remain_hp = 0;
		if (MonsterManager.instance && MonsterManager.instance.now_Monster)
		{
			if (MonsterManager.instance.now_Monster.GetComponent<Monster>())
				remain_hp = MonsterManager.instance.now_Monster.GetComponent<Monster>().hp;
		}

		Debug.Log("<color=yellow>GGG:remainhp : </color>" + remain_hp.ToString());

		int nowdamage = (int)CardDataManager.instance.GetNowDamage();
		Debug.Log("<color=yellow>GGG:nowdamage : </color>" + nowdamage.ToString());
		if (nowdamage >= remain_hp)
		{
			return true;
		}

		return false;
	}

	public bool Attack_Manual(int type)
	{
		int idx = 0;

		switch (type)
		{
			case 0:
				idx = 7;
				break;

			case 1:
				idx = 0;
				break;

			case 2:
				idx = 1;
				break;

			case 3:
				idx = 2;
				break;

			case 4:
				idx = 3;
				break;

			case 5: idx = 4; break;
			case 6: idx = 5; break;
			case 7: idx = 6; break;

			case 8: idx = 6; break;
		}


		if (idx == 0)
		{
			StartCoroutine(Interval());
			totalAttack = 2;
		}
		else
			ReserveSkill(idx);

		return LastHit();
	}

	IEnumerator Interval()
	{
		interval = -1;

		yield return new WaitForSeconds(0.1f);

		interval = 0;
	}

	private void Update()
	{
		if (gameObject.scene.name.Contains("Lobby"))
			return;

		LookAtTarget(target);

		return;

		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0은 기본 레이어

		if (stateInfo.IsName("Idle"))
		{
			Debug.Log("Current Animation State: Idle");
		}
		else if (stateInfo.IsName("Skill_01_TwoHandSword"))
		{
			Debug.Log("Current Animation State: Skill_01_TwoHandSword");
		}
		else if (stateInfo.IsName("Skill_02_Swing"))
		{
			Debug.Log("Current Animation State: Skill_02_Swing");
		}
		else if (stateInfo.IsName("Skill_03_JumpAttack"))
		{
			Debug.Log("Current Animation State: Skill_03_JumpAttack");
		}
		else if (stateInfo.IsName("hit"))
		{
			Debug.Log("Current Animation State: hit");
		}
		else if (stateInfo.IsName("run"))
		{
			Debug.Log("Current Animation State: run");
		}
		else if (stateInfo.IsName("attack"))
		{
			Debug.Log("Current Animation State: attack");
		}
		else
		{
			Debug.Log("Unknown State");
		}


		/*if (Input.GetMouseButtonDown(0) == true)
		{
			StartCoroutine(GoToEnemy());
		}*/

		/*if (Input.GetKeyDown(KeyCode.Space) == true)
		{
			SetAni("attack");
			
		}
		if (Input.GetMouseButtonDown(1) == true)
		{
			SetAni("arrive");

			is_Running = false;
		}*/
	}

	void ResetAnimator()
	{
		animator.enabled = false; // Animator 비활성화
		animator.Rebind();        // Animator의 모든 상태를 재설정
		animator.Update(0f);      // 즉시 상태 반영
		animator.enabled = true;  // Animator 활성화
	}

	IEnumerator GoToEnemy()
	{
		yield return null;

		//ResetAnimator();
		target_pos = SpawnManager.instance.FindTarget(target_pos);

		while (true)
		{
			if (Is_Idle())
			{
				ResetAnimator();
				break;
			}

			if (Is_Run())
				break;

			yield return null;
		}

		while (true)
		{
			if (StageManager.instance.state == STAGESTATE.REWARD_FINISH || StageManager.instance.state == STAGESTATE.START
				 || StageManager.instance.state == STAGESTATE.BOSS_CAN_CALL)
				break;

			yield return null;
		}

		if (target_pos != null)
		{
			CardDataManager.instance.InitHandDiscard();

			is_Running = true;

			//target = monsterList[0];

			if (!Is_Run())
				SetAni("run");

			bool is_mon_appear = false;

			while (true)
			{
				if (is_mon_appear == false && Vector3.Distance(transform.position, target_pos.transform.position) < 6)
				{
					OnSpawnArrive?.Invoke(target_pos.transform.position, MonsterManager.instance.GetNextMonsterType());
					is_mon_appear = true;
				}
				else if (Vector3.Distance(transform.position, target_pos.transform.position) < 3f)
				{
					SetAni("arrive");
					is_Running = false;
					break;
				}

				yield return null;
			}

			while (true)
			{
				if (MonsterManager.instance.now_Monster && MonsterManager.instance.now_Monster.GetComponent<Monster>().Is_Idle())
				{
					JokerManager.instance.StartFight();
					StartCoroutine(Fight());
					break;
				}

				yield return null;
			}

		}
	}

	public int totalAttack;

	public void ReserveSkill(int count)
	{
		int convert = count;// DataManager.instance.TESTDATA.Convert_Skill(count - 2);
		animator.SetInteger("reserve_skill", convert);

		switch(convert)
		{
			case 1: totalAttack = 2; break;
			case 2: totalAttack = 1; break;
			case 3: totalAttack = 1; break;
			case 4: totalAttack = 2; break;
			case 5: totalAttack = 2; break;
			case 6: totalAttack = 2; break;
			case 7: totalAttack = 1; break;
			case 8: totalAttack = 2; break;
		}
	}

	public string skill_name_last = "last";
	int next_skill_idx = -1;
	bool skill_changed;
	IEnumerator Fight()
	{
		target = MonsterManager.instance.now_Monster;

		interval = 0;
		is_Fight = true;
		/*SetAni("attack");
		Hitted();*/
		float skill_interval = 0f;

		skill_changed = true;
		animator.SetBool("fight", is_Fight);
		while (true)
		{
			if (target == null)
			{
				is_Fight = false;
			}

			if (is_Fight == false)
			{
				animator.SetBool("fight", is_Fight);
				break;
			}

			//interval -= Time.deltaTime;

			if (skill_interval > 0)
				skill_interval -= Time.deltaTime;

			animator.SetFloat("Interval", interval);

			if (target && target.GetComponent<Monster>() && target.GetComponent<Monster>().hp > 0)
			{

			}
			else
			{
				FindNextEnemey();

				if (target == null && Is_Idle())
				{
					ResetAnimator();
					is_Fight = false;
					break;
				}
			}

			Check_Skill_Use();
			yield return null;
		}

		//SetAni("idle");
	}

	void FindNextEnemey()
	{
		target = MonsterManager.instance.FindNextEnemey(target);
	}

	void Idle()
	{
		//SetTrigger("attack");
	}

	public int GetNum_Attack()
	{
		AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0); // 0은 기본 레이어

		if (clipInfos.Length > 0)
		{
			AnimationClip currentClip = clipInfos[0].clip;

			int hitEventCount = currentClip.events.Count(e => e.functionName == "Hit");
			Debug.Log($"현재 애니메이션 '{currentClip.name}'의 Hit 이벤트 개수: {hitEventCount}");
			return hitEventCount;
		}
		else
		{
			Debug.Log("현재 재생 중인 클립이 없습니다.");
			return 1;
		}
/*
		if (stateInfo.IsName("Skill_01_TwoHandSword"))
			return 2;
		else if (stateInfo.IsName("Skill_02_Sting"))
			return 1;
		else if (stateInfo.IsName("Skill_03_JumpAttack"))
			return 1;
		else if (stateInfo.IsName("skill_04_Upper"))
			return 2;
		else if (stateInfo.IsName("skill_dagger_01"))
			return 2;
		else if (stateInfo.IsName("skill_Greatsword_01"))
			return 2;
		else if (stateInfo.IsName("skill_05_Debuff"))
			return 1;
		else if (stateInfo.IsName("skill_06_Buff"))
			return 2;
		else if (stateInfo.IsName("attack"))
			return 2;

		return 1;*/
	}

	string Is_Skill_Name()
	{
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0은 기본 레이어

		if (stateInfo.IsTag("Skill"))
		{
			if (stateInfo.IsName("Skill_01_TwoHandSword"))
				return "Skill_01_TwoHandSword";
			else if (stateInfo.IsName("Skill_02_Sting"))
				return "Skill_02_Sting";
			else if (stateInfo.IsName("Skill_03_JumpAttack"))
				return "Skill_03_JumpAttack";
			else if (stateInfo.IsName("skill_04_Upper"))
				return "skill_04_Upper";
			else if (stateInfo.IsName("skill_05_Debuff"))
				return "skill_05_Debuff";
			else if (stateInfo.IsName("skill_06_Buff"))
				return "skill_06_Buff";
		}

		return "";
	}

	bool Is_Skill_Finishing()
	{
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0은 기본 레이어

		if (stateInfo.IsTag("Skill"))
		{
			if (stateInfo.normalizedTime >= 0.8f)
			{
				return true;
			}
		}

		return false;
	}

	void Check_Skill_Use()
	{
		if (target && target.GetComponent<Monster>() && target.GetComponent<Monster>().hp > 0)
		{
			if (animator.GetInteger("reserve_skill") == 0 && SkillManager.instance.Is_Can_Use_Skill())
			{
				ReserveSkill(SkillManager.instance.list_SkillReady[0]);
			}
		}
	}

	bool Is_Idle()
	{
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0은 기본 레이어        
		return stateInfo.IsName("Idle");
	}

	bool Is_Run()
	{
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0은 기본 레이어        
		return stateInfo.IsName("run");
	}



	public void Atk()
	{
		animator.SetTrigger("attack");
		skill_changed = false;
	}

	public void Skill()
	{

		/*float pct = Random.Range(0, 100);

        if (pct < 30)
            animator.SetTrigger("skill_01");
        else if (pct < 60)
            animator.SetTrigger("skill_02");
        else
            animator.SetTrigger("skill_03");*/

		if (SkillManager.instance && SkillManager.instance.Is_Can_Use_Skill())
		{
			int idx = SkillManager.instance.Use_Next_Skill();

			if (idx != -1)
			{
				/*string name_skill_trigger = string.Format("skill_0{0}", idx);
                animator.SetTrigger(name_skill_trigger);
                skill_changed = false;*/
			}
		}
	}

	public float rotationSpeed = 2f; // 회전 속도 (값이 클수록 빠르게 회전)
	private Quaternion initialRotation; // 초기 정면 방향 저장

	void LookAtTarget(GameObject target)
	{
		if (MonsterManager.instance.is_Group == false)
			return;

		Quaternion targetRotation;

		// 타겟이 없으면 초기 방향으로 복귀
		if (target == null)
		{
			targetRotation = initialRotation;

			if (/*!Is_Idle() && */!is_Running)
				return;

		}
		else
		{
			// 타겟 방향 계산
			Vector3 direction = target.transform.position - transform.position;

			// 방향 벡터가 0이 아닌 경우만 계산
			if (direction.sqrMagnitude > 0.001f)
			{
				targetRotation = Quaternion.LookRotation(direction);
			}
			else
			{
				targetRotation = transform.rotation; // 현재 회전 유지
			}
		}

		// 정면 방향과 타겟 방향의 중간 지점을 계산 (50% 보도록 설정)
		float rot_Pct = 0.5f;

		if (target != null)
		{
			// 자신의 forward 벡터
			Vector3 forward = transform.forward;

			// target과 자신의 위치 차이 벡터
			Vector3 toTarget = target.transform.position - transform.position;

			// cross product를 이용해 방향 계산
			float _direction = Vector3.Cross(forward, toTarget).y;

			if (_direction > 0)
			{
				rot_Pct = 0.5f;
			}
			else if (_direction < 0)
			{
				rot_Pct = 0.3f;
			}
		}

		Quaternion halfwayRotation = Quaternion.Slerp(initialRotation, targetRotation, rot_Pct);

		// 부드럽게 회전
		transform.rotation = Quaternion.Slerp(transform.rotation, halfwayRotation, Time.deltaTime * rotationSpeed);
	}

	private Coroutine goToEnemyCoroutine;

	public void DieTarget()
	{
		//if (MonsterManager.instance.list_Monster.Count == 0)
		StageManager.instance.state = STAGESTATE.REWARD_START;
		//StageManager.instance.state = STAGESTATE.REWARD_FINISH;

		int reward_dollar = (int)CardDataManager.instance.GetMyData(CARDDATA.HAND);
		CardDataManager.instance.SetMyData(CARDDATA.DOLLAR, reward_dollar, CALC.PLUS);

		/*if (StageManager.instance)
			if (StageManager.instance.state == STAGESTATE.BOSS_APPEAR)
			{*/
		if (StageManager.instance.step_Call_Boss == 3)
		{
			StageManager.instance.NextStage();
			return;
		}


		/*if (StageManager.instance)
			StageManager.instance.step_Call_Boss++;*/

		/*if (monsterList.Count > 0)
		{
			GameObject firstMonster = monsterList[0];
			Debug.Log($"Deleting Monster: {firstMonster.name}");

			// 리스트에서 제거
			monsterList.RemoveAt(0);

			// 씬에서 삭제
			Destroy(firstMonster);
		}*/

		if (target == null)
		{
			StartCoroutine(Waiting_Round_End());
		}
	}

	IEnumerator Waiting_Round_End()
	{
		while (true)
		{
			//if (MainGame.the.Waiting_User_Input())
			if (!CardsManager.the.Is_Event_Moving())
				break;

			yield return null;
		}

		#region timing_round_clear
		StartCoroutine(JokerManager.instance.Start_Check_Joker(JOKERTIMING.ROUND_CLEAR));
		#endregion

		NextRound();
	}

	void NextRound()
	{
		if (goToEnemyCoroutine != null)
		{
			StopCoroutine(goToEnemyCoroutine); // 기존 코루틴 종료
		}

		goToEnemyCoroutine = StartCoroutine(GoToEnemy());

		//MainGame.the.ResetGame(true);

		StageManager.instance.step_Call_Boss++;

		/*CardDataManager.instance.SetMyData(CARDDATA.SCORE, 0, false);
		CardDataManager.instance.SetMyData(CARDDATA.HAND, 4);
		CardDataManager.instance.SetMyData(CARDDATA.DISCARD, 4);*/
	}


	void FixedUpdate()
	{
		if (gameObject.scene.name.Contains("Lobby"))
			return;

		/*
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (animator.IsInTransition(0)) // 현재 레이어에서 상태 전환 중인지 확인
        {
            AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0); // 다음 상태 정보
            Debug.Log("Transitioning to: " + nextInfo.shortNameHash);

            if (nextInfo.IsName("idle"))
            {
                Debug.Log("idle");
            }
            else if (nextInfo.IsName("run"))
            {
                Debug.Log("run");
            }
        }
        else
        {
            if (currentInfo.IsName("idle"))
            {
                Debug.Log("idle");
            }
            else if (currentInfo.IsName("run"))
            {
                Debug.Log("run");
            }
        }*/

		// Run 애니메이션 중일 때 캐릭터 이동
		if (is_Running) // "Run"은 Animator의 상태 이름
		{
			Vector3 forwardMovement = transform.forward * spped_run * Time.fixedDeltaTime;
			rb.MovePosition(rb.position + forwardMovement);
		}

	}
}
