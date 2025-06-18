using System.Collections.Generic;
using UnityEngine;

public enum MAINUI
{
	PROFILE,
	DUNGEON,
	QUEST,
	SHOP,
	SKILL,
	NONE
}

public class UI_Main : MonoBehaviour
{
	public List<GameObject> list_Profile_Off = new List<GameObject>();
	public List<GameObject> list_Profile_On = new List<GameObject>();

	public static UI_Main instance;

	public GameObject ui_equip;

	public delegate void onMainUiChange(MAINUI _mainui);
	public onMainUiChange OnMainUiChange;

	MAINUI _mainType;
	public Material grayMaterial;
	public MAINUI mainType
	{
		set
		{
			_mainType = value;

			OnMainUiChange?.Invoke(_mainType);
		}
		get { return _mainType; }
	}

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		//grayMaterial = Resources.Load<Material>("Effect/Shader/Custom_GrayButton");

		// Main Camera를 비활성화
		GameObject mainCamera = GameObject.Find("Main Camera");
		if (mainCamera != null)
		{
			list_Profile_On.Add(mainCamera);
			Debug.Log("Main Camera has been disabled.");
		}
		else
		{
			Debug.LogWarning("Main Camera not found!");
		}

		// Directional Light를 비활성화
		GameObject directionalLight = GameObject.Find("Directional Light");
		if (directionalLight != null)
		{
			list_Profile_On.Add(directionalLight);
			Debug.Log("Directional Light has been disabled.");
		}
		else
		{
			Debug.LogWarning("Directional Light not found!");
		}
	}

	private void Start()
	{
		/*ui_equip = GameObject.Find("ui_equip");
		if (ui_equip)
			ui_equip.SetActive(false);*/

		UI_Show[] objectsWithAScript = FindObjectsByType<UI_Show>(FindObjectsSortMode.None);

		foreach (UI_Show a in objectsWithAScript)
		{
			OnMainUiChange += a.OnMainUiChange;
		}

		mainType = MAINUI.NONE;
	}

	public void Btn_Profile()
	{
		if (mainType == MAINUI.NONE)
		{
			mainType = MAINUI.PROFILE;
			TurnLobbyObj(true);
		}
		else if (mainType == MAINUI.PROFILE)
		{
			mainType = MAINUI.NONE;
			TurnLobbyObj(false);
		}
	}

	public void TurnLobbyObj(bool on)
	{
		if (ui_equip)
			ui_equip.SetActive(on);

		foreach (var obj in list_Profile_Off)
			obj.SetActive(on);

		foreach (var obj in list_Profile_On)
			obj.SetActive(!on);
	}
}
