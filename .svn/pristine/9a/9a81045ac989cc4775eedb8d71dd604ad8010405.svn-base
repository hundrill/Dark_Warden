using System.Collections;
using TMPro;
using UnityEngine;

public class Result : MonoBehaviour
{
	public TextMeshPro digit;
	private void Start()
	{
		StartCoroutine(Life());
	}

	IEnumerator Life()
	{
		yield return new WaitForSeconds(0.4f);

		Destroy(gameObject);
	}

	public void SetText(string num)
	{
		if (digit)
			digit.text = num;
	}
}
