using UnityEngine;
using UnityEngine.UI;


public class item_one_joker : MonoBehaviour
{
	public delegate void onSelect(JOKER type);
	public onSelect OnSelect;

	public Image img;

	JOKER type;

	public void SetType(JOKER _type)
	{
		string id_joker;
		/*type = _type;

		if (img && CardDataManager.instance && CardDataManager.instance.img_Joker.Length > (int)_type)
			img.sprite = CardDataManager.instance.img_Joker[(int)_type];*/
	}

	public void Btn_Select()
	{
		OnSelect?.Invoke(type);
	}
}
