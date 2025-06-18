using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Equip_Ui : MonoBehaviour
{
	public Image icon;
	public TextMeshProUGUI txt_Name;
	public TextMeshProUGUI txt_Ability_1;
	public TextMeshProUGUI txt_Ability_2;
	public TextMeshProUGUI txt_Ability_3;

	public TextMeshProUGUI txt_Value_1;
	public TextMeshProUGUI txt_Value_2;
	public TextMeshProUGUI txt_Value_3;

	public void SetEquip(string id)
	{
		string[] name =
		{
			"armor",
			"helmet",
			"shoulder",
			"cloak",
			"gloves",
			"shoes"
		};

		//if(txt_Name) txt_Name.text = "";
		
		if (id == "")
		{
			if (icon) icon.gameObject.SetActive(false);

			if (txt_Ability_1) txt_Ability_1.text = "";
			if (txt_Ability_2) txt_Ability_2.text = "";
			if (txt_Ability_3) txt_Ability_3.text = "";

			if (txt_Value_1) txt_Value_1.text = "";
			if (txt_Value_2) txt_Value_2.text = "";
			if (txt_Value_3) txt_Value_3.text = "";
		}
		else
		{
			if (txt_Ability_1) txt_Ability_1.text = "공격력";
			if (txt_Ability_2) txt_Ability_2.text = "방어력";
			if (txt_Ability_3) txt_Ability_3.text = "HP";

			if (icon) icon.gameObject.SetActive(true);

			if (txt_Name) txt_Name.text = (string)CsvManager.instance.GetTextData(id);

			string iconname = CsvManager.instance.GetItem(id, ITEM.IMAGE).ToString();
			icon.sprite = DataManager.instance.GetSprite(iconname);

			int n;
			int.TryParse(CsvManager.instance.GetItem(id, ITEM.ATK).ToString() , out n);
			if (txt_Value_1) txt_Value_1.text = string.Format("{0}", n);

			int.TryParse(CsvManager.instance.GetItem(id, ITEM.DEF).ToString(), out n);
			if (txt_Value_2) txt_Value_2.text = string.Format("{0}", n);

			int.TryParse(CsvManager.instance.GetItem(id, ITEM.HP).ToString(), out n);
			if (txt_Value_3) txt_Value_3.text = string.Format("{0}", n);
		}
	}
}
