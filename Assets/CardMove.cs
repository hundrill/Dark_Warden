using UnityEngine;
using System.Collections;

public class CardMove : MonoBehaviour
{
	float swayAmount = 13f; // ȸ�� ���� ����
	float swaySpeed = 1;  // ȸ�� �ӵ�

	private Quaternion originalRotation;
	private float randomOffsetX;
	private float randomOffsetY;

	void Start()
	{
		originalRotation = transform.rotation;
		randomOffsetX = Random.Range(0f, 7f * Mathf.PI);
		randomOffsetY = Random.Range(0f, 7f * Mathf.PI);
		StartCoroutine(SwayCoroutine());
	}

	IEnumerator SwayCoroutine()
	{
		while (true)
		{
			float swayX = Mathf.Sin(Time.time * swaySpeed + randomOffsetX) * swayAmount;
			float swayY = Mathf.Cos(Time.time * swaySpeed + randomOffsetY) * swayAmount * 0.5f;

			transform.rotation = originalRotation * Quaternion.Euler(swayY, swayX, 0);

			yield return null;
		}
	}
}
