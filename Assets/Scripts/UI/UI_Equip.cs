using System;
using UnityEngine;
using static DataManager;

public class UI_Equip : MonoBehaviour
{
	public Equip_Ui[] equip;

	private void Start()
	{
		Refresh();

		DataManager.instance.OnItemEquip += OnItemEquip;
}

	void Refresh()
	{
		for (int i = 0; i < equip.Length; i++)
		{
			equip[i].GetComponent<Equip_Ui>().SetEquip(DataManager.instance.Equip[i]);
		}
	}

	void OnItemEquip(ITEMTYPE type, string id)
	{
		Refresh();
	}
}
