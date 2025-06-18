using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duration : MonoBehaviour
{
	float lifetime;

	private void OnEnable()
	{
		if (GetComponent<ParticleSystem>())
		{
			lifetime = GetComponent<ParticleSystem>().main.duration;
			StartCoroutine(CheckFinish());
		}
	}
	

	IEnumerator CheckFinish()
	{
		yield return new WaitForSeconds(lifetime);

		ObjectPoolManager.instance.Despawn(gameObject);
	}
}
