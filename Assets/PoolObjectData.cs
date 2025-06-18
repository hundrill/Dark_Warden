using UnityEngine;
using KeyType = System.String;

/// <summary> Ǯ ��� ������Ʈ�� ���� ���� </summary>
[System.Serializable]
public class PoolObjectData
{
	public const int INITIAL_COUNT = 10;
	public const int MAX_COUNT = 50;

	public KeyType key;
	public GameObject prefab;
	public int initialObjectCount = INITIAL_COUNT; // ������Ʈ �ʱ� ���� ����
	public int maxObjectCount = MAX_COUNT;     // ť ���� ������ �� �ִ� ������Ʈ �ִ� ����
}