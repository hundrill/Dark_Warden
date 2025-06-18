using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

namespace VideoPokerKit
{
	public class Card_Deck : MonoBehaviour
	{
		public GameObject txt_Chip;
		public GameObject txt_Mult;

		public delegate void onSelect(bool select/*TAROT type, int price*/);
		public onSelect OnSelect;

		// card face sprite
		public SpriteRenderer cardFaceSprite;
		// hold marker
		public GameObject holdMarkerObj;

		// card animator
		Animator animator;
		// vanish duration
		float vanishTime = 0.33f;

		[HideInInspector]
		public CardData cardData = new CardData();
		// the card from the deck
		CardData libraryCard;

		AudioSource flipSound;

		public delegate void onAniStart(string name_ani);
		public onAniStart OnAniStart;

		public delegate void onAniEnd(string name_ani);
		public onAniEnd OnAniEnd;

		bool _is_GoTo_Target = false;
		bool is_GoTo_Target
		{
			get { return _is_GoTo_Target; }
			set
			{
				_is_GoTo_Target = value;
				if (value)
					NormalRotation();
			}
		}

		//bool is_Select;
		//----------------------------------------------------
		private void Awake()
		{
			animator = GetComponent<Animator>();
			if (animator)
			{
				for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
				{
					AnimationClip clip = animator.runtimeAnimatorController.animationClips[i];

					AnimationEvent animationStartEvent = new AnimationEvent();
					animationStartEvent.time = 0;
					animationStartEvent.functionName = "AnimationStartHandler";
					animationStartEvent.stringParameter = clip.name;

					AnimationEvent animationEndEvent = new AnimationEvent();
					animationEndEvent.time = clip.length;
					animationEndEvent.functionName = "AnimationCompleteHandler";
					animationEndEvent.stringParameter = clip.name;

					clip.AddEvent(animationStartEvent);
					clip.AddEvent(animationEndEvent);
				}
			}

			_rigidbody = GetComponent<Rigidbody>();
		}

		public void SetChipMultText(float chip, float mult)
		{
			if (txt_Chip && txt_Chip.GetComponent<TextMeshPro>())
				txt_Chip.GetComponent<TextMeshPro>().text = chip.ToString();

			if (txt_Mult && txt_Mult.GetComponent<TextMeshPro>())
				txt_Mult.GetComponent<TextMeshPro>().text = mult.ToString();
		}

		public void AnimationStartHandler(string name)
		{
			if (name.ToLower().Contains("cardhold"))
			{
				//pos_Final = transform.position;
			}

			OnAniStart?.Invoke(name);

			Debug.Log($"{name} animation start.");
			//OnAnimationStart?.Invoke(name);
		}

		Vector3 pos_Final;

		public void AnimationCompleteHandler(string name)
		{
			if (name.ToLower().Contains("cardhold"))
			{
				transform.position += new Vector3(0, 0.01f, 0);
			}
			else if (name.ToLower().Contains("carddown"))
			{
				transform.position -= new Vector3(0, 0.01f, 0);
			}

			OnAniEnd?.Invoke(name);

			Debug.Log($"{name} animation complete.");
			//OnAnimationComplete?.Invoke(name);
		}

		// Use this for initialization
		void Start()
		{
			animator = GetComponent<Animator>();
			flipSound = GetComponent<AudioSource>();


			pos_Final = transform.position;
			targetPos = transform.position;

			initialX = transform.position.x;
			initialY = transform.position.y;
			initialZ = transform.position.z;

			originalRotation = transform.rotation;
			randomOffsetX = Random.Range(0f, 7f * Mathf.PI);
			randomOffsetY = Random.Range(0f, 7f * Mathf.PI);

			/*if (!UI_Card.instance.Dlg_Tarot.activeSelf)
				StartCoroutine(SwayCoroutine());*/

			//_rigidbody = GetComponent<Rigidbody>();

			//_rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
		}


		float swayAmount = 11f; // 회전 각도 범위
		float swaySpeed = 1;  // 회전 속도

		private Quaternion originalRotation;
		private float randomOffsetX;
		private float randomOffsetY;

		IEnumerator SwayCoroutine()
		{
			while (true)
			{
				if (is_Dragging == false)
				{
					float swayX = Mathf.Sin(Time.time * swaySpeed + randomOffsetX) * swayAmount;
					float swayY = Mathf.Cos(Time.time * swaySpeed + randomOffsetY) * swayAmount * 0.5f;

					transform.rotation = originalRotation * Quaternion.Euler(swayY, swayX, 0);
				}

				yield return null;
			}
		}

		//----------------------------------------------------

		public void SetLibraryCard(CardData kd)
		{
			// set a link to a deck card, so that we know what 
			// card we need to work with
			libraryCard = kd;
		}

		//----------------------------------------------------

		// this function is not starting the flipping animation, 
		// instead it is called as an event FROM THE flipping animation which is already runnning
		public void StartFlippingCard()
		{
			// the flipping animation started, so it is time
			// to copy the data from the deck into the actual local card
			//CopyInfoFrom(libraryCard); //여기를 deal 하기 직전타이밍으로 옮긴다.
			flipSound.Play();
		}

		//----------------------------------------------------
		public void Copy_From_Library()
		{
			CopyInfoFrom(libraryCard);
		}

		void CopyInfoFrom(CardData other)
		{
			cardData.type = other.type;
			cardData.value = other.value;
			cardData.sprite = other.sprite;
			cardData.hold = other.hold;
			cardData.chip_growth = other.chip_growth;
			cardData.mult_growth = other.mult_growth;

			// update sprite
			UpdateCardFaceSprite();
		}

		//----------------------------------------------------

		public void UpdateCardFaceSprite()
		{
			cardFaceSprite.sprite = cardData.sprite;
		}

		//----------------------------------------------------

		public void SetResultsState(bool bWin)
		{
			// if it's a winner card, we start a specific animation for the card (flashing)
			// otherwise we play a 'not winner' fade-out animation

			if (bWin)
				animator.SetTrigger("trResultsWin");
			else
				animator.SetTrigger("trResultsIgnore");
		}

		//----------------------------------------------------

		public void DealWithDelay(float delay)
		{
			if (vanishTime + delay > 0)
				Invoke("Deal", delay); // postpone the dealing
									   //Invoke("Deal", vanishTime + delay); // postpone the dealing
			else
				Deal(); // start dealing in an instant
		}


		//----------------------------------------------------

		public void InitX()
		{
			transform.position = new Vector3(initialX, initialY, initialZ);
		}

		public void InitY()
		{
			transform.position = new Vector3(transform.position.x, initialY, initialZ);
		}

		public void ResetY()
		{
			initialY = transform.position.y;
		}

		float normalScale = 1.1f;
		public void InitScale()
		{
			//transform.localScale = new Vector3(1, 1, 1);
			transform.localScale = new Vector3(normalScale, normalScale, normalScale);
		}

		public void Deal()
		{
			//position reset
			//transform.position = new Vector3(transform.position.x, pos_Final.y, transform.position.y);
			//transform.position = new Vector3(transform.position.x, pos_Final.y, -0.6f);
			transform.position = new Vector3(transform.position.x, initialY, -0.6f);


			//select reset
			cardData.is_Select = false;

			// start the DEAL animation
			animator.SetTrigger("trDeal");
			Debug.Log("<color=yellow>ggg:trDeal</color>");
			// play deal sound
			SoundsManager.the.dealCardSound.Play();
		}

		//--------------------------------------------------

		public void ResetHold()
		{
			// disable HOLD
			cardData.hold = false;
			// hide hold marker
			holdMarkerObj.SetActive(cardData.hold);
		}

		//--------------------------------------------------

		public bool IsHolded()
		{
			return cardData.hold;
		}

		//--------------------------------------------------

		public bool Is_Card_Idle()
		{
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			return stateInfo.IsName("CardIdle");
		}

		public void ClearAfterDeal()
		{
			// we vanish only from some states (not from dealing)
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			if (stateInfo.IsName("CardIdle") ||
				stateInfo.IsName("CardHold") ||
				stateInfo.IsName("CardResultsIgnore") || stateInfo.IsName("CardResultsWin"))
			{
				animator.SetTrigger("trVanish");
				Debug.Log("<color=yellow>ggg:trVanish </color>");
			}
			else if (stateInfo.IsName("CardHold"))
				Debug.Log("<color=yellow>ggg:CardHold </color>");
			else if (stateInfo.IsName("CardHold2"))
				Debug.Log("<color=yellow>ggg:CardHold2 </color>");
			else if (stateInfo.IsName("CardIdleBeforeDeal1"))
				Debug.Log("<color=yellow>ggg:CardIdleBeforeDeal1 </color>");
			else if (stateInfo.IsName("CardIdleBeforeDeal2"))
				Debug.Log("<color=yellow>ggg:CardIdleBeforeDeal2 </color>");
			else if (stateInfo.IsName("Card_Rot_Down"))
				Debug.Log("<color=yellow>ggg:Card_Rot_Down </color>");
			else if (stateInfo.IsName("Card_Rot_Up"))
				Debug.Log("<color=yellow>ggg:Card_Rot_Up </color>");
			else if (stateInfo.IsName("CardVanish"))
				Debug.Log("<color=yellow>ggg:CardVanish </color>");
			else if (stateInfo.IsName("Deal"))
				Debug.Log("<color=yellow>ggg:Deal </color>");
			else if (stateInfo.IsName("FlipCard1"))
				Debug.Log("<color=yellow>ggg:FlipCard1 </color>");
			else if (stateInfo.IsName("FlipCard2"))
				Debug.Log("<color=yellow>ggg:FlipCard2 </color>");
			else
			{
				animator.SetTrigger("trVanish");
				Debug.Log("<color=yellow>ggg:What Animation</color>");
			}

		}

		public IEnumerator Set_Ani(string name, float speed = 1)
		{
			yield return new WaitForSeconds(0.1f);


			if (speed != 1)
			{
				animator.Update(0);

				// 2. 애니메이션을 끝에서 시작하게 설정 (1.0 = 끝)
				animator.Play(name);

				// 3. 속도를 -1로 해서 거꾸로 재생
				animator.speed = speed;
			}
			else
				animator.Play(name);
		}
		//--------------------------------------------------

		public void VanishAndDealAgainWithDelay(float delay)
		{
			ClearAfterDeal();
			DealWithDelay(vanishTime + delay);
		}

		//--------------------------------------------------

		float time_Click;

		bool is_Touched = false;
		bool is_Dragging = false;

		float initialX;
		float initialY;
		float initialZ;
		Vector3 dragStartPos;
		Vector3 objectStartPos;
		Vector3 updateStartPos;
		Vector3 targetPos;

		//float offset_Z = -1;
		float offset_Z = -2f;


		void OnMouseDown()
		{
			return; //덱에서는 노무빙

			if (Time.timeScale == 0)
				return;

			if (!MainGame.the.Waiting_User_Input())
				return;

			/*if (cardData.is_Select == false)
				if (CardsManager.the.Count_Num_Select() >= 5)
				{
					is_Touched = false;
					return;
				}*/

			is_Touched = true;

			transform.localScale = new Vector3(normalScale + 0.15f, normalScale + 0.15f, 1);

			time_Click = Time.time;

			Camera cam = GameObject.Find("Camera_Card").GetComponent<Camera>();


			objectStartPos = transform.position;

			// 카메라 방향으로 offset_Z 만큼 이동
			transform.position += cam.transform.forward * offset_Z;

			updateStartPos = transform.position;

			// 드래그 시작 시 터치 위치와 오브젝트 위치 저장
			dragStartPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.WorldToScreenPoint(transform.position).z));


			lastpos = transform.position;

		}

		Vector3 lastpos;
		void OnMouseDrag()
		{
			if (StageManager.instance && StageManager.instance.state == STAGESTATE.REWARD_START)
				return;

			if (!is_Touched)
				return;

			Camera cam = GameObject.Find("Camera_Card").GetComponent<Camera>();
			Vector3 currentMousePos = cam.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.WorldToScreenPoint(transform.position).z));
			Vector3 delta = currentMousePos - dragStartPos; // 드래그 시작점 대비 변화량
			Vector3 newPosition = updateStartPos + delta;

			if (Mathf.Abs(lastpos.x - newPosition.x) > 0.1f)
				is_Dragging = true;

			// 현재 이동 방향 확인
			bool movingRight = newPosition.x > objectStartPos.x;
			bool movingUp = newPosition.y > objectStartPos.y;

			// 방향에 따라 정렬 방식 결정
			/*var sortedCards = movingRight
				? CardsManager.the.GetAllCards().OrderBy(c => c.GetPositionX()).ToList() // 오른쪽 이동: 오름차순 정렬
				: CardsManager.the.GetAllCards().OrderByDescending(c => c.GetPositionX()).ToList(); // 왼쪽 이동: 내림차순 정렬*/

			var sortedCards = movingRight
				? CardsManager.the.GetAllCards().OrderBy(c => c.transform.position.x).ToList() // 오른쪽 이동: 오름차순 정렬
				: CardsManager.the.GetAllCards().OrderByDescending(c => c.transform.position.x).ToList(); // 왼쪽 이동: 내림차순 정렬

			foreach (var card in sortedCards)
			{
				if (card != this) // Skip the dragged card itself
				{
					float newX = card.GetPositionX();

					if (movingRight && newPosition.x > newX && objectStartPos.x < newX)
					{
						card.SetTargetPos(new Vector3(objectStartPos.x, card.transform.position.y, card.transform.position.z));
						objectStartPos.x = newX;
						dragStartPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.WorldToScreenPoint(newPosition).z));
						updateStartPos = newPosition;
						targetPos = objectStartPos;
						break;
					}
					else if (!movingRight && newPosition.x < newX && objectStartPos.x > newX)
					{
						card.SetTargetPos(new Vector3(objectStartPos.x, card.transform.position.y, card.transform.position.z));
						objectStartPos.x = newX;
						dragStartPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.WorldToScreenPoint(newPosition).z));
						updateStartPos = newPosition;
						targetPos = objectStartPos;
						break;
					}
				}
			}

			/*blend tree=========================================
			if (newPosition.x == objectStartPos.x)
				animator.SetFloat("horizontal", 0);
			else if (movingRight)
				animator.SetFloat("horizontal", -1);
			else 
				animator.SetFloat("horizontal", 1);

			if (newPosition.y == objectStartPos.y)
				animator.SetFloat("vertical", 0);
			else if (movingUp)
				animator.SetFloat("vertical", -1);
			else
				animator.SetFloat("vertical", 1);
			*/

			//if (Mathf.Abs(lastpos.x - newPosition.x) > 0.1f)
			//		Swing();


			lastpos = transform.position = newPosition;
		}

		private void LateUpdate()
		{
			if (is_Dragging)
				Swing();
		}

		void NormalRotation()
		{
			_rigidbody.rotation = Quaternion.identity;
		}

		private void OnMouseUp()
		{
			NormalRotation();

			if (!is_Touched)
				return;

			animator.SetFloat("horizontal", 0);

			float elapsedTime = Time.time - time_Click;

			float distance = Vector2.Distance(new Vector2(objectStartPos.x, objectStartPos.y), new Vector2(transform.position.x, transform.position.y));

			//transform.localScale = new Vector3(1f, 1f, 1);

			float distance_Return_Minimum = 0.20f;

			Debug.Log("Distance : " + distance);

			bool select = true;

			if (elapsedTime > 0.3f || distance > distance_Return_Minimum)
				select = false;

			if (cardData.is_Select == false)
			{
				if (StageManager.instance.state != STAGESTATE.REWARD_START)
				{
					if (CardsManager.the.Count_Num_Select() >= 5)
						select = false;
				}
				else
				{
					if (UI_Tarot.instance && UI_Tarot.instance.num_Select_Card >= UI_Tarot.instance.tarot_Select.select_count)
						select = false;
				}
			}

			if (select == false)
				//StartCoroutine(GoToPosition_Select(!cardData.is_Select));
				StartCoroutine(ReturnToStartPosition()); // 원래 자리로 돌아가기
			else
				CardSelect();

			is_Dragging = false;
			is_Touched = false;
		}


		private float tiltSpeedFactor = 300f; // 속도에 따른 기울어지는 정도 조절
		private float smoothTime = 0.05f; // 부드럽게 회전하는 속도 (가속도)
		private float maxTiltAngle = 40f; // 최대 기울임 각도
		private float currentAngle_Z = 0f; // 초기 Z축 각도
		private float angleVelocity_Z = 0f; // Z축 각도 변화 속도
		Vector3 oldPosition;
		Rigidbody _rigidbody;

		void Swing()
		{
			if (Is_Card_Idle() == false)
				return;

			if (is_GoTo_Target)
			{
				return;
			}

			var difference = transform.position - oldPosition;

			// 좌우(X) 이동에 따라 Z축 기울기 적용 (반대 방향)
			var speed_X = difference.x * tiltSpeedFactor;
			float targetAngle_Z = Mathf.Clamp(-speed_X, -maxTiltAngle, maxTiltAngle);

			// 부드러운 회전 적용
			currentAngle_Z = Mathf.SmoothDamp(currentAngle_Z, targetAngle_Z, ref angleVelocity_Z, smoothTime);

			// Z축 회전 적용
			_rigidbody.rotation = Quaternion.Euler(new Vector3(0, 0, currentAngle_Z));

			// 이전 위치 업데이트
			oldPosition = transform.position;
		}


		/*
		public float tiltSpeedFactor = 100f; // 속도에 따른 기울어지는 정도 조절
		public float smoothTime = 0.1f; // 부드럽게 회전하는 속도 (가속도)
		public float maxTiltAngle = 45f; // 최대 기울임 각도
		private float currentAngle = 180f; // 초기 각도
		private float angleVelocity = 0f; // 각도 변화 속도
		Vector3 oldPosition;
		Rigidbody _rigidbody;
		float currentAngle_X, currentAngle_Y, angleVelocity_X, angleVelocity_Y;
		void Swing()
		{
			var difference = transform.position - oldPosition;

			// 좌우(X) 기울기 (반대 방향 적용)
			var speed_X = difference.x * (tiltSpeedFactor * 5f);
			float targetAngle_Y = Mathf.Clamp(-speed_X, -30f, 30f); // 반대 방향 기울기

			// 위아래(Y) 기울기 (반대 방향 적용)
			var speed_Y = difference.y * (tiltSpeedFactor * 5f);
			float targetAngle_X = Mathf.Clamp(speed_Y, -20f, 20f); // 반대 방향 기울기

			// 부드러운 회전 적용
			currentAngle_Y = Mathf.SmoothDamp(currentAngle_Y, targetAngle_Y, ref angleVelocity_Y, smoothTime);
			currentAngle_X = Mathf.SmoothDamp(currentAngle_X, targetAngle_X, ref angleVelocity_X, smoothTime);

			// 회전 적용
			_rigidbody.rotation = Quaternion.Euler(new Vector3(currentAngle_X, currentAngle_Y, 0));

			// 이전 위치 업데이트
			oldPosition = transform.position;

			_rigidbody.linearVelocity = Vector3.zero;

		}
		*/
		public float GetPositionX()
		{
			return targetPos.x;
		}

		Coroutine cc;
		public void SetTargetPos(Vector3 target, float interval = 0.05f)
		{
			if (cc != null)
				StopCoroutine(cc);

			cc = StartCoroutine(MoveToTarget(target, interval)); // Start the smooth movement coroutine with a duration of 0.2 seconds
		}

		IEnumerator MoveToTarget(Vector3 target, float duration)
		{
			is_GoTo_Target = true;
			targetPos = target;
			Vector3 startPos = transform.position;
			float elapsed = 0f;

			CheckMoveAni(startPos, target);

			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				float t = Mathf.Clamp01(elapsed / duration); // Ensure t stays between 0 and 1

				transform.position = Vector3.Lerp(startPos, target, t); // Smoothly interpolate the position
				yield return null;
			}

			StopMoveaAni();

			// Final position correction (to avoid overshooting)
			//transform.position = target;
			transform.position = new Vector3(target.x, target.y, objectStartPos.z);
			//objectStartPos.x = transform.position.x;

			is_GoTo_Target = false;
		}

		void StopMoveaAni()
		{
			animator.SetFloat("horizontal", 0);
			animator.SetFloat("vertical", 0);
		}

		void CheckMoveAni(Vector3 start, Vector3 target)
		{
			if (start.x == target.x)
				animator.SetFloat("horizontal", 0);
			else if (start.x > target.x)
				animator.SetFloat("horizontal", -1);
			else
				animator.SetFloat("horizontal", 1);

			if (start.y == target.y)
				animator.SetFloat("vertical", 0);
			else if (start.y > target.y)
				animator.SetFloat("vertical", -1);
			else
				animator.SetFloat("vertical", 1);
		}


		IEnumerator ReturnToStartPosition()
		{
			is_GoTo_Target = true;

			float duration = 0.12f;
			float elapsed = 0f;
			Vector3 startPos = transform.position;

			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				float t = elapsed / duration;
				transform.position = Vector3.Lerp(startPos, objectStartPos, t); // 부드럽게 이동
				transform.position = new Vector3(transform.position.x, transform.position.y, offset_Z);
				yield return null;
			}

			transform.position = new Vector3(objectStartPos.x, objectStartPos.y, objectStartPos.z);

			Vector3 targetScale = new Vector3(normalScale, normalScale, normalScale); // 최종 목표 스케일
			float scaleDuration = 0.1f;
			float scaleElapsed = 0f;
			Vector3 initialScale = transform.localScale;

			while (scaleElapsed < scaleDuration)
			{
				scaleElapsed += Time.deltaTime;
				float t = scaleElapsed / scaleDuration;
				transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
				yield return null;
			}

			// 최종 스케일 보정
			transform.localScale = targetScale;

			is_GoTo_Target = false;
		}

		IEnumerator GoToPosition_Select(bool select)
		{
			is_GoTo_Target = true;

			float duration = 0.12f;
			float elapsed = 0f;
			Vector3 startPos = transform.position;
			Vector3 startScale = transform.localScale; // 현재 스케일 저장
			Vector3 targetScale = new Vector3(normalScale, normalScale, normalScale); // 최종 목표 스케일

			// 위치 보정
			objectStartPos = new Vector3(objectStartPos.x, initialY + 0.2f * (select ? 1 : 0), objectStartPos.z);

			CheckMoveAni(startPos, objectStartPos);

			// 위치 이동 애니메이션
			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				float t = elapsed / duration;
				transform.position = new Vector3(
					Mathf.Lerp(startPos.x, objectStartPos.x, t),
					Mathf.Lerp(startPos.y, objectStartPos.y, t),
					objectStartPos.z
				);
				yield return null;
			}

			StopMoveaAni();

			// 최종 위치 보정 (X, Y 값만 보정, Z는 현재 값 유지)
			transform.position = new Vector3(objectStartPos.x, objectStartPos.y, objectStartPos.z);

			// ? 추가된 부분: 0.1초 동안 스케일을 1로 복귀
			float scaleDuration = 0.1f;
			float scaleElapsed = 0f;
			Vector3 initialScale = transform.localScale;

			while (scaleElapsed < scaleDuration)
			{
				scaleElapsed += Time.deltaTime;
				float t = scaleElapsed / scaleDuration;
				transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
				yield return null;
			}

			// 최종 스케일 보정
			transform.localScale = targetScale;

			is_GoTo_Target = false;
		}


		public void CardSelect()
		{
			if (Is_Card_Idle() == false)
				return;

			//transform.position = objectStartPos;

			if (cardData.is_Select == false)
			{
				if (StageManager.instance.state != STAGESTATE.REWARD_START)
				{
					if (CardsManager.the.Count_Num_Select() >= 5)
						return;
				}
				else
				{
					if (UI_Tarot.instance && UI_Tarot.instance.num_Select_Card >= UI_Tarot.instance.tarot_Select.select_count)
						return;
				}

				animator.SetTrigger("trUp");
				StartCoroutine(GoToPosition_Select(true)); //선택된 위치로 

			}
			else
			{
				animator.SetTrigger("trDown");
				StartCoroutine(GoToPosition_Select(false)); //원래 위치로 
			}


			cardData.is_Select = !cardData.is_Select;

			SoundsManager.the.holdSound.Play();

			CardsManager.the.EvaluateHand();
			CardDataManager.instance.SetResult();
			CardsManager.the.Count_Num_Select();

			OnSelect?.Invoke(cardData.is_Select);
		}

		//--------------------------------------------------

		/*Vector3 lastPos;
		private void LateUpdate()
		{
			Vector3 startPos = transform.position;
			CheckMoveAni(lastPos.x > startPos.x, lastPos.y > startPos.y);
			lastPos = transform.position;

		}*/

		public void ToggleHold()
		{
			// change hold status
			cardData.hold = !cardData.hold;

			// update HOLD marker visibility
			holdMarkerObj.SetActive(cardData.hold);

			// play HOLD animation if needed
			if (cardData.hold)
			{
				// start playing the HOLD animation
				animator.SetTrigger("trHold");
				// play HOLD sound
				SoundsManager.the.holdSound.Play();
			}
		}

		//--------------------------------------------------

		// this callback is called from the DEAL animation (as an Animation event)
		void DealAnimationFinished()
		{
			MainGame.the.CardAnimationFinished();
		}

		//--------------------------------------------------
	}
}