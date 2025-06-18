using System.Collections;
using TMPro;
using UnityEngine;
using VideoPokerKit;

public class Count : MonoBehaviour
{
	[SerializeField]
	public float lifeTime;

	public SpriteRenderer back;
	public TextMeshPro digit;
	Color origin;
	private void Start()
	{
		origin = back.color;
		StartCoroutine(Life());
	}

	IEnumerator Life()
	{
		float speed = OptionManager.instance.Speed_Time;
		yield return new WaitForSeconds(lifeTime * speed);

		Destroy(gameObject);
	}

	public void SetDigit(float num)
	{
		if (digit)
			digit.text = string.Format("+{0}", num);
	}

	public void SetOriginColor()
	{
		if (back)
			back.color = new Color(0,0, 1);
	}

	public void SetBackColor(Color color)
	{
		if (back)
			back.color = color;
	}

	public void SetString(string text)
	{
		if (digit)
			digit.text = string.Format("{0}", text);
	}
}
