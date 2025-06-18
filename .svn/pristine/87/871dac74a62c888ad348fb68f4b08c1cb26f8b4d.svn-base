using TMPro;
using UnityEngine;

public class item_one_runinfo : MonoBehaviour
{
    public TextMeshProUGUI txt_Name;
	public TextMeshProUGUI txt_Level;
	public TextMeshProUGUI txt_Mult;
	public TextMeshProUGUI txt_Chips;
	public TextMeshProUGUI txt_Count;

	public void SetRuninfo(string name , int level , int mult, int chips, int count)
    { 
		if(txt_Name) txt_Name.text = name;
		if (txt_Level) txt_Level.text = string.Format("Lv.{0}", level + 1);
		if(txt_Mult) txt_Mult.text = string.Format("{0}", mult);
		if (txt_Chips) txt_Chips.text = string.Format("{0}", chips);
		if (txt_Count) txt_Count.text = string.Format("{0}", count);
	}

}
