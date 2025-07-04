using System.Collections.Generic;
using UnityEngine;
using System;

using KeyType = System.String;
using System.Text;

/// <summary> 
/// 오브젝트 풀 관리 싱글톤
/// </summary>
[DisallowMultipleComponent]
public class ObjectPoolManager: MonoBehaviour
{
	[SerializeField]
	private List<PoolObjectData> _poolObjectDataList = new List<PoolObjectData>(4);

	private Dictionary<KeyType, GameObject> _sampleDict;   // Key - 복제용 오브젝트 원본
	private Dictionary<KeyType, PoolObjectData> _dataDict; // Key - 풀 정보
	private Dictionary<KeyType, Stack<GameObject>> _poolDict;         // Key - 풀
	private Dictionary<GameObject, Stack<GameObject>> _clonePoolDict; // 복제된 게임오브젝트 - 풀

	public static ObjectPoolManager instance;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		Init();
	}

	private void Init()
	{
		int len = _poolObjectDataList.Count;
		if (len == 0) return;

		// 1. Dictionary 생성
		_sampleDict = new Dictionary<KeyType, GameObject>(len);
		_dataDict = new Dictionary<KeyType, PoolObjectData>(len);
		_poolDict = new Dictionary<KeyType, Stack<GameObject>>(len);
		_clonePoolDict = new Dictionary<GameObject, Stack<GameObject>>(len * PoolObjectData.INITIAL_COUNT);

		// 2. Data로부터 새로운 Pool 오브젝트 정보 생성
		foreach (var data in _poolObjectDataList)
		{
			RegisterInternal(data);
		}
	}

	/// <summary> Pool 데이터로부터 새로운 Pool 오브젝트 정보 등록 </summary>
	private void RegisterInternal(PoolObjectData data)
	{
		// 중복 키는 등록 불가능
		if (_poolDict.ContainsKey(data.key))
		{
			return;
		}

		// 1. 샘플 게임오브젝트 생성, PoolObject 컴포넌트 존재 확인
		GameObject sample = Instantiate(data.prefab);
		sample.AddComponent<Duration>();
		sample.name = data.prefab.name;
		sample.SetActive(false);

		// 2. Pool Dictionary에 풀 생성 + 풀에 미리 오브젝트들 만들어 담아놓기
		Stack<GameObject> pool = new Stack<GameObject>(data.maxObjectCount);
		for (int i = 0; i < data.initialObjectCount; i++)
		{
			GameObject clone = Instantiate(data.prefab);
			clone.AddComponent<Duration>();
			clone.SetActive(false);
			pool.Push(clone);

			_clonePoolDict.Add(clone, pool); // Clone-Stack 캐싱
		}

		// 3. 딕셔너리에 추가
		_sampleDict.Add(data.key, sample);
		_dataDict.Add(data.key, data);
		_poolDict.Add(data.key, pool);
	}

	/// <summary> 샘플 오브젝트 복제하기 </summary>
	private GameObject CloneFromSample(KeyType key)
	{
		if (!_sampleDict.TryGetValue(key, out GameObject sample)) return null;

		return Instantiate(sample);
	}

	/// <summary> 풀에서 꺼내오기 </summary>
	public GameObject Spawn(KeyType key)
	{
		// 키가 존재하지 않는 경우 null 리턴
		if (!_poolDict.TryGetValue(key, out var pool))
		{
			return null;
		}

		GameObject go;

		// 1. 풀에 재고가 있는 경우 : 꺼내오기
		if (pool.Count > 0)
		{
			go = pool.Pop();
		}
		// 2. 재고가 없는 경우 샘플로부터 복제
		else
		{
			go = CloneFromSample(key);
			_clonePoolDict.Add(go, pool); // Clone-Stack 캐싱
		}

		if (go == null)
		{
			StringBuilder sb = new StringBuilder();

#if UNITY_EDITOR
			//ToastLogic.CreateToast(sb.AppendFormat("{0} 가 ObjectPool 에서 Spawn 되지 않았음", key.ToString()).ToString());
#endif
			return null;
		}

		go.SetActive(true);
		go.transform.SetParent(null);

		return go;
	}

	/// <summary> 풀에 집어넣기 </summary>
	public void Despawn(GameObject go)
	{
		// 캐싱된 게임오브젝트가 아닌 경우 파괴
		if (!_clonePoolDict.TryGetValue(go, out var pool))
		{
			Destroy(go);
			return;
		}

		// 집어넣기
		go.SetActive(false);
		pool.Push(go);
	}
}