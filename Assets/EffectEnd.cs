using UnityEngine;

public class EffectEnd : MonoBehaviour
{
	private Animator animator;

	void OnEnable()
	{
		animator = GetComponent<Animator>();
		StartCoroutine(WaitForAnimationToEnd());
	}

	System.Collections.IEnumerator WaitForAnimationToEnd()
	{
		// 현재 재생 중인 상태 정보 가져오기
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

		// 애니메이션이 아직 시작 안 했을 수도 있으니 조금 대기
		yield return null;

		// 현재 상태의 애니메이션 길이만큼 대기
		yield return new WaitForSeconds(stateInfo.length);

		// 다 끝났으면 비활성화
		gameObject.SetActive(false);
	}
}
