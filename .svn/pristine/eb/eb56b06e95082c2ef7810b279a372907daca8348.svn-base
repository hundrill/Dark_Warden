using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using VideoPokerKit;
using static CardDataManager;

public class Monster : MonoBehaviour
{
	public bool is_Boss;
	HP_Ui hp_Com;

	public Animator animator;

	public int maxhp;

	public MONSTER type;

	public delegate void onMonsterHpChange(int now, int total);
	public onMonsterHpChange OnMonsterHpChange;

	public delegate void onMonsterDamage(int damage);
	public onMonsterDamage OnMonsterDamage;
	
	int _hp;
	public int hp
	{
		get { return _hp; }
		set
		{
			_hp = value;

			float pct = _hp * 100 / maxhp * 0.01f;

			if (hp_Com)
				hp_Com.SetFill(pct);

			OnMonsterHpChange?.Invoke(_hp, maxhp);
		}
	}

	float interval_gap = 3;
	float interval_now;
	GameObject target;
	// Start is called before the first frame update

	public int GetHp()
	{
		int[] basehp =
		{
			200,//100,
			300,//200,
			500,//400,
			900,//800
		};

		if (basehp.Length > StageManager.instance.step_Call_Boss)
		{
			//return basehp[StageManager.instance.step_Call_Boss];
			return CalculateScore(basehp[StageManager.instance.step_Call_Boss], StageManager.instance.num_Stage);
		}

		
		return 100;
	}

	int CalculateScore(int base_score, int stage)
	{
		int score = base_score; // stage 1의 점수
		for (int i = 0; i < stage; i++)
		{
			score = (int)(score * 1.5); // 이전 점수의 1.5배
		}
		return score;
	}

	void Start()
	{
		OnMonsterHpChange += UI_Card.instance.OnMonsterHpChange;
		OnMonsterDamage += ui_Damage.instance.OnMonsterDamage;

		animator = GetComponent<Animator>();

		if (DataManager.instance.TESTDATA._Monsterhp != 0)
			hp = maxhp = DataManager.instance.TESTDATA._Monsterhp;

		hp = maxhp = GetHp();

		CardDataManager.instance.SetMyData(CARDDATA.SCORE_CLEAR, hp);

		/*if (is_Boss)
			hp *= 3;*/

		animator.SetInteger("hp", hp);

		interval_now = interval_gap;
		StartCoroutine(AI());

		target = Character.instance.transform.gameObject;

		int seed = gameObject.GetInstanceID();
		Random.InitState(seed);



		//AddHpBar();
	}

	void AddHpBar()
	{
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

		GameObject hpcanvas = Instantiate(MonsterManager.instance.hpcanvas);

		hpcanvas.transform.parent = headTransform;

		Vector3 plus_pos = GetHpBarPos(type);
		//hpcanvas.GetComponent<RectTransform>().localPosition = new Vector3(0, -headTransform.position.y, 0);
		hpcanvas.GetComponent<RectTransform>().localPosition = new Vector3(-0.5f, 0, 0) + plus_pos;
		hp_Com = hpcanvas.GetComponentInChildren<HP_Ui>();
	}

	Vector3 GetHpBarPos(MONSTER type)
	{
		switch (type)
		{
			case MONSTER.MEDUSA: return new Vector3(-0.4f, 0, 0);
			case MONSTER.UNDEAD: return new Vector3(0.0f, 0, 0);
			case MONSTER.SCORPION: return new Vector3(0.0f, 1, 0);
			case MONSTER.RAPTOR: return new Vector3(-0.3f, 0, 0);
			case MONSTER.SKELETON: return new Vector3(0.2f, 0, 0);
			case MONSTER.SKELETON_ARCHER: return new Vector3(0.2f, 0, 0);
		}

		return Vector3.zero;
	}

	Transform FindHeadTransform(Transform root)
	{
		foreach (Transform child in root.GetComponentsInChildren<Transform>())
		{
			// "Head"라는 단어가 포함된 이름을 탐색
			if (child.name.ToLower().Contains("head"))
			{
				return child;
			}
		}
		return null; // 찾지 못하면 null 반환
	}

	public bool Is_Idle()
	{
		if (animator == null)
			return false;

		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0은 기본 레이어        
		return stateInfo.IsName("Idle");
	}

	IEnumerator AI()
	{
		while (true)
		{
			LookAtTarget(target);

			if (Is_Can_Attack())
			{
				//interval_now -= Time.deltaTime;

				if (interval_now <= 0)
				{
					interval_now = interval_gap;

					if (Random.Range(0, 100) < 50)
						Atk();
					else
						Skill();
				}
			}

			if (hp <= 0)
				break;

			yield return null;
		}
	}

	public float rotationSpeed = 2f; // 회전 속도 (값이 클수록 빠르게 회전)
	private Quaternion initialRotation; // 초기 정면 방향 저장

	void LookAtTarget(GameObject target)
	{
		if (target == null)
			return;

		Quaternion targetRotation;

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

		// 정면 방향과 타겟 방향의 중간 지점을 계산 (50% 보도록 설정)
		Quaternion halfwayRotation = Quaternion.Slerp(initialRotation, targetRotation, 0.5f);

		// 부드럽게 회전
		transform.rotation = Quaternion.Slerp(transform.rotation, halfwayRotation, Time.deltaTime * rotationSpeed);
	}

	bool Is_Can_Attack()
	{
		float distance = Vector3.Distance(transform.position, Character.instance.transform.position);

		//if (distance < 4)
		{
			return true;
		}

		return false;
	}

	public void Attack()
	{
		Character.instance.Attack();
	}

	public void Atk()
	{
		animator.SetTrigger("attack");
	}

	public void Hit()
	{
		Character.instance.Damage();
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

	public void Shake()
	{
		if (CameraLogic.instance)
			CameraLogic.instance.Shake(1.0f, 0.1f);
	}

	public void Skill()
	{
		if (Random.Range(0, 100) < 50)
			animator.SetTrigger("skill_1");
		else
			animator.SetTrigger("skill_2");
	}

	public void Hitted()
	{
		StartCoroutine(Damage(true));
	}

	public void Hitted_Cri()
	{
		StartCoroutine(Damage(true));
	}

	public void Die_Immediately()
	{
		hp = 0;
		animator.SetTrigger("die");
	}

	IEnumerator Damage(bool is_cri = false)
	{
		yield return new WaitForSeconds(0.0f);

		//hp--;
		//hp -= Character.instance.myAtk;

		int count_num_attack = Character.instance.GetNum_Attack();
		Character.instance.totalAttack--;

		int damage = (int)CardDataManager.instance.GetNowDamage();
		hp -= damage / count_num_attack;

		OnMonsterDamage?.Invoke(damage);

		if (Is_Idle())
		{
			if (is_cri && animator.parameters.Any(param => param.name == "hitted_cri" && param.type == AnimatorControllerParameterType.Trigger))
				animator.SetTrigger("hitted_cri");
			else
				animator.SetTrigger("hitted");
		}
		else
		{
			animator.ResetTrigger("hitted_cri");
			animator.SetTrigger("hitted_cri");
		}

		animator.SetInteger("hp", hp);


		if (hp <= 0)
			StartCoroutine(Die());

		yield return new WaitForSeconds(1.0f);

		if (Character.instance.totalAttack == 0)
			CardDataManager.instance.InitMultAndChips();
	}

	IEnumerator Die()
	{
		//animator.SetTrigger("die");

		// Trigger로 전환된 상태 대기
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
		int dieHash = Animator.StringToHash("die");

		while (stateInfo.shortNameHash != dieHash)
		{
			stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			yield return null;
		}

		// 애니메이션 종료 대기
		while (stateInfo.normalizedTime < 1.0f)
		{
			stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			yield return null;
		}

		Character.instance.DieTarget();

		Destroy(gameObject);
	}

	private void OnDestroy()
	{
		OnMonsterDamage -= ui_Damage.instance.OnMonsterDamage;
	}
}


