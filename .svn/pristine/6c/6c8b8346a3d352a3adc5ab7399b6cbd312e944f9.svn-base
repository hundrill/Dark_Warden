using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MONSTER
{
	MEDUSA,
	UNDEAD,
	SCORPION,
	RAPTOR,
	ALTAMIR,
	SPECTER,
	SPIDER,
	ELIZABETH,
	CANNIBALFLOWER,
	SKELETON,
    SKELETON_ARCHER,
	SKELETON_AXE,
	SKELETON_QUIVER,
	SKELETON_SHIELD	
}

public class MonsterManager : MonoBehaviour
{
	public GameObject[] monster;

	public static MonsterManager instance;
	public GameObject hpcanvas;
	public GameObject hpcanvas_hero;

	public bool is_Group;

    List<MONSTER> list_MonType = new List<MONSTER>();

	public GameObject now_Monster;

	[HideInInspector]
	public List<GameObject> list_Monster;

	int idx_mon;
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		StageManager.OnStateChange += OnStateChange;
	}

	private void Start()
	{
		list_Monster = new List<GameObject>();
		
		Character.instance.OnSpawnArrive += OnSpawnArrive;
		

		//Initialize();
	}

	void OnStateChange(STAGESTATE state)
	{
		switch (state)
		{
			case STAGESTATE.START:
				Initialize();
				break;

			case STAGESTATE.BOSS_APPEAR:
				/*DestroyAllEnemy();
				SpawnBoss();	*/			
				break;
		}
	}

	void SpawnBoss()
	{
		SpawnMonster(MONSTER.UNDEAD, Character.instance.gameObject.transform.position + Character.instance.gameObject.transform.forward * 3, 1 , true);
	}

	private void Initialize()
	{
		DestroyAllEnemy();

		idx_mon = -1;

		list_MonType.Clear();

		string name = string.Format("{0}_{1}", StageManager.instance.num_Chapter + 1, StageManager.instance.num_Stage + 1);
		list_MonType = CsvManager.instance.GetStageMonsterList(name);
				
		SpawnManager.instance.ResetSpawnSpot(list_MonType.Count);

		/*list_MonType.Add(MONSTER.UNDEAD);
		list_MonType.Add(MONSTER.SKELETON);
		list_MonType.Add(MONSTER.MEDUSA);
		list_MonType.Add(MONSTER.SKELETON_ARCHER);
		list_MonType.Add(MONSTER.RAPTOR);
		list_MonType.Add(MONSTER.SCORPION);*/
	}

	public MONSTER GetNextMonsterType()
	{
		idx_mon++;

		if (idx_mon >= list_MonType.Count || idx_mon < 0)
			idx_mon = 0;

		return list_MonType[idx_mon];
	}

	void DestroyAllEnemy()
	{
		GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");

		// 각 오브젝트 삭제
		foreach (GameObject monster in monsters)
		{
			Destroy(monster);
		}
	}

	void DieAllEnemy()
	{
		GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");

		// 각 오브젝트 삭제
		foreach (GameObject monster in monsters)
		{
			monster.GetComponent<Monster>().Die_Immediately();
		}
	}

	public void OnSpawnArrive(Vector3 pos, MONSTER type)
	{
		int num = 1;

		/*switch(type)
		{
			case MONSTER.SKELETON: num = 2; break;
            
			case MONSTER.SKELETON_ARCHER:
                SpawnMonster(MONSTER.SKELETON, pos, 4);
				return;
        }*/

		bool is_Boss = SpawnManager.instance.Is_Boss_Spawn(Character.instance.target_pos);

		if (is_Boss) StageManager.instance.state = STAGESTATE.BOSS_APPEAR;

		SpawnMonster(type, pos, num ,is_Boss);
	}

	public void SpawnMonster(MONSTER _type, Vector3 _pos, int num_mon = 1 , bool is_Boss = false)
	{
        is_Group = num_mon == 1 ? false : true;

		list_Monster.Clear();

		for (int i = 0; i < num_mon; i++)
		{
			if (i >= 2)
				_type = MONSTER.SKELETON_ARCHER;
			//GameObject _temp = Instantiate(monster[(int)_type], _pos, Quaternion.Euler(0, 180, 0));
			GameObject _temp = ObjectPoolManager.instance.Spawn(_type.ToString());
			_temp.transform.SetPositionAndRotation(_pos, Quaternion.Euler(0, 180, 0));

			if (num_mon > 1)
			{
				switch (i)
				{
					case 0:
						_temp.transform.position = _temp.transform.position + _temp.transform.right * 1.2f + _temp.transform.forward * 0.7f; // 왼쪽
						break;

					case 1:
						_temp.transform.position = _temp.transform.position - _temp.transform.right * 0.8f + _temp.transform.forward * 0.7f; // 오른쪽
                        break;

                    case 2:
                        _temp.transform.position = _temp.transform.position + _temp.transform.right * 0.9f - _temp.transform.forward * 0.7f; // 왼쪽 / 뒤
                        break;

                    case 3:
                        _temp.transform.position = _temp.transform.position - _temp.transform.right * 0.9f - _temp.transform.forward * 0.7f; // 오른쪽 / 뒤
                        break;
                }
			}

			_temp.GetComponent<Monster>().type = _type;
			_temp.GetComponent<Monster>().is_Boss = is_Boss;
			_temp.tag = "MONSTER";
			list_Monster.Add(_temp);
		}

		now_Monster = FindNextEnemey();
	}

	public GameObject FindNextEnemey(GameObject currentTarget = null)
	{
		if (list_Monster == null || list_Monster.Count == 0)
		{
			Debug.LogWarning("Monster list is empty or null.");
			return null;
		}

		// currentTarget이 null일 경우 리스트의 첫 번째 요소 반환
		if (currentTarget == null)
		{
			return list_Monster[0]; // 리스트의 첫 번째 몬스터를 타겟으로 설정			
		}

		// 현재 타겟이 리스트에 있는지 확인
		int currentIndex = list_Monster.IndexOf(currentTarget);
		if (currentIndex == -1)
		{
			Debug.LogWarning("The current target is not in the list.");
			return null;
		}

		// 리스트에서 현재 타겟 제거
		list_Monster.RemoveAt(currentIndex);

		// 다음 타겟 반환
		if (list_Monster.Count > currentIndex)
		{
			return list_Monster[currentIndex]; // 현재 인덱스의 다음 몬스터			
		}
		else if (list_Monster.Count > 0)
		{
			return list_Monster[0]; // 리스트의 첫 번째 몬스터
		}
		else
		{
			return null;
		}
	}
}
