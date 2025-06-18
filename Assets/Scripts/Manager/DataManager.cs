using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static StageManager;

public enum ITEMTYPE
{
	HELMET,
	ARMOR,
	SHOULDER,
	CLOAK,
	GLOVES,
	SHOES,
	MAX
}

public enum MYDATA
{
	LEVEL,
	LEVEL_DICE,
	GOLD,
	DIAMOND,
	MAX
}

public class DataManager : MonoBehaviour
{
	public Data TESTDATA; // 정적 변수로 ScriptableObject 접근
	public static DataManager instance;

	public string[] Equip = new string[(int)ITEMTYPE.MAX];

	public delegate void onItemEquip(ITEMTYPE type, string id);
	public onItemEquip OnItemEquip;

	public delegate void onMyDataChange(MYDATA type, int value);
	public onMyDataChange OnMyDataChange;

	public int[] myData = new int[(int)MYDATA.MAX];

	public List<JOKER> list_Joker = new List<JOKER>();

	public delegate void onMyJokerListChange(List<JOKER> list);
	public onMyJokerListChange OnMyJokerListChange;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}

		UI_MyData[] objectsWithAScript = FindObjectsByType<UI_MyData>(FindObjectsSortMode.None);

		foreach (UI_MyData a in objectsWithAScript)
		{
			OnMyDataChange += a.OnMyDataChange;
		}

	}

	

	private void Start()
	{
		Initialize();

		SetMyData(MYDATA.LEVEL, 1);
		SetMyData(MYDATA.LEVEL_DICE, 1);
		SetMyData(MYDATA.GOLD, 0);
		SetMyData(MYDATA.DIAMOND, 0);
	}

	void Initialize()
	{
		InitData();
		LoadData();
	}

	public void SetMyData(MYDATA type, int value)
	{
		int idx = (int)type;

		if (myData.Length > idx)
		{
			myData[idx] += value;

			OnMyDataChange?.Invoke(type, myData[idx]);
		}
	}

	void InitData()
	{
		list_Joker.Clear();
	}

	void LoadData()
	{
		//EquipItem(ITEMTYPE.HELMET , "id_helmet_1");
	}

	public void AddJoker(JOKER joker)
	{
		list_Joker.Add(joker);

		OnMyJokerListChange?.Invoke(list_Joker);
	}

	public int GetItemValue(string id, ITEM type)
	{
		int n = 0; // n의 기본값 설정

		// GetItem 결과 처리
		string itemValue = CsvManager.instance.GetItem(id, type)?.ToString();

		if (string.IsNullOrEmpty(itemValue))
		{
			Debug.LogWarning($"GetItem returned null or empty for id: {id}, type");
			return n; // 기본값 반환
		}

		if (!int.TryParse(itemValue, out n))
		{
			Debug.LogError($"Failed to parse type value for id: {id}, value: {itemValue}");
			return 0; // 파싱 실패 시 기본값 반환
		}

		return n; // 정상적으로 파싱된 값 반환
	}

	public void EquipItem(ITEMTYPE type, string id)
	{
		Equip[(int)type] = id;

		OnItemEquip?.Invoke(type, id);
	}

	public Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

	public Sprite GetSprite(string filename)
	{
		string relativePath = "Project/UI/Item/Equip/";
		string filePath = Path.Combine(Application.dataPath, relativePath, filename + ".png");

		// 경로 구분자를 슬래시('/')로 통일
		filePath = filePath.Replace("\\", "/");

		if (spriteCache.ContainsKey(filePath))
		{
			return spriteCache[filePath]; // 이미 캐싱된 스프라이트 반환
		}
		else
		{
			Sprite sprite = LoadSpriteFromFile(filePath);
			if (sprite != null)
			{
				spriteCache[filePath] = sprite; // 캐시에 저장
			}
			return sprite;
		}
	}

	private Sprite LoadSpriteFromFile(string filePath)
	{
		if (!File.Exists(filePath))
		{
			Debug.LogError("파일이 존재하지 않습니다: " + filePath);
			return null;
		}

		byte[] fileData = File.ReadAllBytes(filePath);
		Texture2D texture = new Texture2D(2, 2);
		if (texture.LoadImage(fileData))
		{
			return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
		}
		Debug.LogError("이미지를 로드할 수 없습니다: " + filePath);
		return null;
	}


}