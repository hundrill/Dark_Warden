using System.Collections.Generic;
using UnityEngine;
using System;

using KeyType = System.String;
using System.Text;

/// <summary> 
/// ������Ʈ Ǯ ���� �̱���
/// </summary>
[DisallowMultipleComponent]
public class ObjectPoolManager: MonoBehaviour
{
	[SerializeField]
	private List<PoolObjectData> _poolObjectDataList = new List<PoolObjectData>(4);

	private Dictionary<KeyType, GameObject> _sampleDict;   // Key - ������ ������Ʈ ����
	private Dictionary<KeyType, PoolObjectData> _dataDict; // Key - Ǯ ����
	private Dictionary<KeyType, Stack<GameObject>> _poolDict;         // Key - Ǯ
	private Dictionary<GameObject, Stack<GameObject>> _clonePoolDict; // ������ ���ӿ�����Ʈ - Ǯ

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

		// 1. Dictionary ����
		_sampleDict = new Dictionary<KeyType, GameObject>(len);
		_dataDict = new Dictionary<KeyType, PoolObjectData>(len);
		_poolDict = new Dictionary<KeyType, Stack<GameObject>>(len);
		_clonePoolDict = new Dictionary<GameObject, Stack<GameObject>>(len * PoolObjectData.INITIAL_COUNT);

		// 2. Data�κ��� ���ο� Pool ������Ʈ ���� ����
		foreach (var data in _poolObjectDataList)
		{
			RegisterInternal(data);
		}
	}

	/// <summary> Pool �����ͷκ��� ���ο� Pool ������Ʈ ���� ��� </summary>
	private void RegisterInternal(PoolObjectData data)
	{
		// �ߺ� Ű�� ��� �Ұ���
		if (_poolDict.ContainsKey(data.key))
		{
			return;
		}

		// 1. ���� ���ӿ�����Ʈ ����, PoolObject ������Ʈ ���� Ȯ��
		GameObject sample = Instantiate(data.prefab);
		sample.AddComponent<Duration>();
		sample.name = data.prefab.name;
		sample.SetActive(false);

		// 2. Pool Dictionary�� Ǯ ���� + Ǯ�� �̸� ������Ʈ�� ����� ��Ƴ���
		Stack<GameObject> pool = new Stack<GameObject>(data.maxObjectCount);
		for (int i = 0; i < data.initialObjectCount; i++)
		{
			GameObject clone = Instantiate(data.prefab);
			clone.AddComponent<Duration>();
			clone.SetActive(false);
			pool.Push(clone);

			_clonePoolDict.Add(clone, pool); // Clone-Stack ĳ��
		}

		// 3. ��ųʸ��� �߰�
		_sampleDict.Add(data.key, sample);
		_dataDict.Add(data.key, data);
		_poolDict.Add(data.key, pool);
	}

	/// <summary> ���� ������Ʈ �����ϱ� </summary>
	private GameObject CloneFromSample(KeyType key)
	{
		if (!_sampleDict.TryGetValue(key, out GameObject sample)) return null;

		return Instantiate(sample);
	}

	/// <summary> Ǯ���� �������� </summary>
	public GameObject Spawn(KeyType key)
	{
		// Ű�� �������� �ʴ� ��� null ����
		if (!_poolDict.TryGetValue(key, out var pool))
		{
			return null;
		}

		GameObject go;

		// 1. Ǯ�� ��� �ִ� ��� : ��������
		if (pool.Count > 0)
		{
			go = pool.Pop();
		}
		// 2. ��� ���� ��� ���÷κ��� ����
		else
		{
			go = CloneFromSample(key);
			_clonePoolDict.Add(go, pool); // Clone-Stack ĳ��
		}

		if (go == null)
		{
			StringBuilder sb = new StringBuilder();

#if UNITY_EDITOR
			//ToastLogic.CreateToast(sb.AppendFormat("{0} �� ObjectPool ���� Spawn ���� �ʾ���", key.ToString()).ToString());
#endif
			return null;
		}

		go.SetActive(true);
		go.transform.SetParent(null);

		return go;
	}

	/// <summary> Ǯ�� ����ֱ� </summary>
	public void Despawn(GameObject go)
	{
		// ĳ�̵� ���ӿ�����Ʈ�� �ƴ� ��� �ı�
		if (!_clonePoolDict.TryGetValue(go, out var pool))
		{
			Destroy(go);
			return;
		}

		// ����ֱ�
		go.SetActive(false);
		pool.Push(go);
	}
}