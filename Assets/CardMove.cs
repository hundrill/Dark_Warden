using UnityEngine;
using System.Collections;

public class CardMove : MonoBehaviour
{
	float swayAmount = 13f; // 회전 각도 범위
	float swaySpeed = 1;  // 회전 속도

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
