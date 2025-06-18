using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using System;
using static UnityEngine.Rendering.DebugUI;

namespace VideoPokerKit
{
	public enum SORT
	{
		RANK,
		SUITE
	};

	public class CardsManager : MonoBehaviour
	{
		public static CardsManager the;


		public Sprite[] sprite_AllCard;

		// all the cards in the deck (set in the inspector)
		[Header("The deck")]
		public CardData[] cardLibrary;

		CardData[] cardLibraryBackup;// = new CardData[cardLibrary.Length];
									 // screen cards
		[Header("Player cards")]
		public Card[] gameCards;

		// temporary cards used for evaluating hand
		CardData[] workCards;

		// change these timers in the Inspector for faster/slower deals
		[Header("Deal settings")]
		public float firstHandDealDelayBetweenCards = 0.05f;
		public float secondHandDealDelayBetweenCards = 0.05f;

		private List<int> nextGameCards = new List<int>();

		public delegate void onCardSelect(int num);
		public static onCardSelect OnCardSelect;


		public SORT sort_Type;

		public delegate void on_Event_Change(bool is_event);
		public static on_Event_Change On_Event_Change;

		bool _is_Event_Moving;
		bool is_Event_Moving
		{
			get { return _is_Event_Moving; }
			set
			{
				_is_Event_Moving = value;
				On_Event_Change(_is_Event_Moving);
			}
		}

		private void Awake()
		{
			the = this;
		}

		//--------------------------------------------------------
		int num_Deck = 8;
		int num_Hand = 8;
		// Use this for initialization
		void Start()
		{
			cardLibraryBackup = BackupCardLibrary(cardLibrary);

			workCards = new CardData[num_Deck];
			for (int i = 0; i < num_Deck; i++)
				workCards[i] = new CardData();

			sort_Type = SORT.RANK;
			is_Event_Moving = false;
			Character.instance.OnSpawnArrive += OnSpawnArrive;

			StartCoroutine(CheckHoldAnimationFinished());

		}

		CardData[] BackupCardLibrary(CardData[] source)
		{
			CardData[] copy = new CardData[source.Length];
			for (int i = 0; i < source.Length; i++)
			{
				copy[i] = new CardData();
				copy[i].type = source[i].type;
				copy[i].value = source[i].value;
				copy[i].sprite = source[i].sprite;
			}
			return copy;
		}

		void RestoreCardLibrary(CardData[] target, CardData[] backup)
		{
			for (int i = 0; i < target.Length; i++)
			{
				target[i].type = backup[i].type;
				target[i].value = backup[i].value;
				target[i].sprite = backup[i].sprite;
			}
		}

		public void RestoreCardLibrary()
		{
			RestoreCardLibrary(cardLibrary, cardLibraryBackup);
		}

		public void OnSpawnArrive(Vector3 pos, MONSTER type)
		{
			#region timing_round_start
			StartCoroutine(JokerManager.instance.Start_Check_Joker(JOKERTIMING.ROUND_START));
			#endregion
		}

		public void OnEnable()
		{
			for (int i = 0; i < num_Deck; i++)
			{
				if (gameCards[i].cardFaceSprite != null)
				{
					gameCards[i].cardFaceSprite.color = new Color32(255, 255, 255, 255);
				}
			}
		}

		//----------------------------------------------------

		public void ClearHand()
		{
			// make cards vanish
			for (int i = 0; i < num_Hand; i++)
				gameCards[i].ClearAfterDeal();

			Debug.Log("AAA ClearAfterDeal_2");
		}

		//--------------------------------------------------------

		public void ResetDeck()
		{
			// mark all cards as not dealt
			for (int i = 0; i < cardLibrary.Length; i++)
				cardLibrary[i].dealt = false;
		}

		//--------------------------------------------------------

		public void PrintHeldCards()
		{
			for (int i = 0; i < gameCards.Length; i++)
			{
				Debug.Log($"Card {i} held: {gameCards[i].cardData.hold}");
			}
		}

		//--------------------------------------------------------

		public void InitX()
		{
			for (int i = 0; i < num_Hand; i++)
			{
				gameCards[i].InitX();
			}
		}

		public void InitY()
		{
			for (int i = 0; i < num_Hand; i++)
			{
				gameCards[i].InitY();
			}
		}

		public void InitScale()
		{
			for (int i = 0; i < num_Hand; i++)
			{
				gameCards[i].InitScale();
			}
		}

		public void SetCardsForNextGame(int[] nextCards)
		{
			nextGameCards.Clear();
			foreach (var cardIndex in nextCards)
			{
				nextGameCards.Add(cardIndex);
			}
		}

		public void ChangeType(CardData target, CardType type)
		{
			target.is_Select = false;
			target.dealt = true;

			CardData foundCard = cardLibrary.FirstOrDefault(card =>
			card.type == target.type &&
			card.value == target.value);

			if (foundCard != null)
			{
				foundCard.ChangeType(type);
			}
		}

		public void AddCardToDeck(CardData cardToAdd)
		{
			var newList = cardLibrary.ToList();  // 배열을 List로 변환
			newList.Add(cardToAdd);              // 카드 추가
			cardLibrary = newList.ToArray();     // 다시 배열로 변환해 저장
		}

		public void DeleteCardFromDeck(CardData target)
		{
			target.is_Select = false;
			target.dealt = true;

			cardLibrary = cardLibrary
				.Where(card => !(card.type == target.type && card.value == target.value))
				.ToArray();
		}

		public void ChangeValue(CardData target, CardValue value)
		{
			target.is_Select = false;
			target.dealt = true;

			CardData foundCard = cardLibrary.FirstOrDefault(card =>
			card.type == target.type &&
			card.value == target.value);

			if (foundCard != null)
			{
				foundCard.ChangeValue(value);
			}
		}

		public void DeckCardGrowth(CardData target, CARDDATA type, float growth)
		{
			CardData foundCard = cardLibrary.FirstOrDefault(card =>
			card.type == target.type &&
			card.value == target.value);

			if (foundCard != null)
			{
				if (type == CARDDATA.MULT)
					foundCard.mult_growth += growth;
				else if (type == CARDDATA.CHIP)
					foundCard.chip_growth += growth;
			}
		}
		//--------------------------------------------------------

		public CardData GetNewCardFromTheDeck()
		{
			// If we have some cards already prepared, deal them
			if (nextGameCards.Count > 0)
			{
				int index = nextGameCards[0];
				nextGameCards.RemoveAt(0);

				// mark the new card as drawn
				cardLibrary[index].dealt = true;

				return cardLibrary[index];
			}

			if (Is_All_Card_Drawn())
			{
				ResetDeck();
				Debug.LogWarning("All Card Drawn!!!");
				//무한 루프 방지코드 - 여기 들어온것이 에러
			}

			// If we don't have prepared cards, return random cards
			int idx = 0;
			do
			{
				// get a random card
				idx = UnityEngine.Random.Range(0, 52);
			}
			while (cardLibrary[idx].dealt); // check to see if it's already drawn

			// mark the new card as drawn
			cardLibrary[idx].dealt = true;

			return cardLibrary[idx];
		}

		//--------------------------------------------------------
		bool Is_All_Card_Drawn()
		{
			foreach (var card in CardsManager.the.cardLibrary)
			{
				if (!card.dealt)
				{
					return false;
				}
			}

			return true;
		}

		int Remain_Card_In_Deck()
		{
			int num = 0;
			foreach (var card in CardsManager.the.cardLibrary)
			{
				if (!card.dealt)
				{
					num++;
				}
			}

			return num;
		}

		public int DealCards(bool firstHand, bool discard = false)
		{
			int num = 0;
			num = Remain_Card_In_Deck();

			// if first hand
			if (firstHand)
			{
				// set the state as DEALING
				MainGame.the.gameState = MainGame.STATE_DEALING;

				// set cards values
				for (int i = 0; i < num_Hand; i++)
				{
					// extract new card from the deck and attach it to the screen card
					var newCard = GetNewCardFromTheDeck();

					gameCards[i].SetLibraryCard(newCard);
					gameCards[i].Copy_From_Library();
					gameCards[i].InitX();
					gameCards[i].InitY();
					gameCards[i].InitScale();

					/* TEST ROYAN FLUSH 
                    // Add there test cards if you want to test a hand
                    CardData newCard = null;
                    switch (i)
                    {
                        case 0:
                            newCard = cardLibrary[32];
                            break;
                        case 1:
                            newCard = cardLibrary[36];
                            break;
                        case 2:
                            newCard = cardLibrary[40];
                            break;
                        case 3:
                            newCard = cardLibrary[44];
                            break;
                        case 4:
                            newCard = cardLibrary[48];
                            break;
                    }
                    newCard.dealt = true;
                    */


				}

				Sort_Card();

				// start deal animations for all five cards with a small delay in between
				for (int i = 0; i < num_Hand; i++)
					gameCards[i].DealWithDelay(i * firstHandDealDelayBetweenCards);

				// we deal 5 cards
				return num_Hand;
			}
			else
			{
				/*// set the new dealing state
				MainGame.the.gameState = MainGame.STATE_DEALING2;

				// gameCards 리스트를 x 값 기준으로 정렬
				gameCards = gameCards.OrderBy(card => card.transform.position.x).ToArray();

				int cardsDealt = 0; // count separately the non-holded cards

				for (int i = 0; i < num_Hand; i++)
				{
					bool disappear = gameCards[i].cardData.is_Select;

					// deal only the non-holded cards
					if (disappear)
					{
						// generate a new random card from the deck
						// and assign it to the card on the screen
						gameCards[i].SetLibraryCard(GetNewCardFromTheDeck());

						// start vanish animation and prepare the deal
						//gameCards[i].VanishAndDealAgainWithDelay(cardsDealt * secondHandDealDelayBetweenCards);
						gameCards[i].ClearAfterDeal();

						cardsDealt++;

						if (!discard)
							gameCards[i].ResetHold();
					}
					else
					{
						gameCards[i].ResetHold(); // remove hold marker for holded cards
					}
				}

				return cardsDealt;
				*/

				if (discard)
					StartCoroutine(Vanishing_And_Sort_Deal(discard));
				else
					StartCoroutine(Event_Play_Hand(discard));

				return MainGame.the.cardsToDeal;
			}
		}

		public bool Is_Have_Number(CardValue _value, string where, out int result, float growth = 0)
		{
			int num = 0;

			// position.x 기준으로 gameCards 정렬
			var sortedCards = gameCards.OrderBy(card => card.transform.position.x).ToArray();

			switch (where)
			{
				case "playingcard":
					JokerManager.instance.realCountCards.AddRange(
						sortedCards.Where(card =>
							card.cardData.is_Select == true &&
							card.cardData.include == true &&
							card.cardData.value == _value &&
							card.cardData.scoring == true));

					num = JokerManager.instance.realCountCards.Count;
					break;

				case "playcard":
					JokerManager.instance.realCountCards.AddRange(
						sortedCards.Where(card =>
							card.cardData.is_Select == true &&
							card.cardData.include == true &&
							card.cardData.value == _value));

					num = JokerManager.instance.realCountCards.Count;
					break;

				case "handcard":
					JokerManager.instance.realCountCards.AddRange(
						sortedCards.Where(card =>
							card.cardData.is_Select == false &&
							card.cardData.value == _value));

					num = JokerManager.instance.realCountCards.Count;
					break;

				case "deckcardall":
					JokerManager.instance.realCountCards.AddRange(
						sortedCards.Where(card =>
							card.cardData.is_Select == true &&
							card.cardData.include == true &&
							JokerManager.instance.realCountCards.Contains(card) == false));

					num = cardLibrary.Count(card => card.value == _value);
					break;

				case "deckcardremain":
					JokerManager.instance.realCountCards.AddRange(
						sortedCards.Where(card =>
							card.cardData.is_Select == true &&
							card.cardData.include == true &&
							JokerManager.instance.realCountCards.Contains(card) == false));

					num = cardLibrary.Count(card => card.value == _value && !card.dealt);
					break;
			}

			result = num;

			return num > 0;
		}

		public bool Is_Have_Suite(CardType _type, string where, out int result, float growth)
		{
			int num = 0;

			var sortedCards = gameCards.OrderBy(card => card.transform.position.x).ToArray();

			switch (where)
			{
				case "playingcard":
					JokerManager.instance.realCountCards.AddRange(
						sortedCards.Where(card =>
							card.cardData.is_Select == true &&
							card.cardData.include == true &&
							card.cardData.type == _type &&
							card.cardData.scoring == true));

					num = JokerManager.instance.realCountCards.Count;
					break;

				case "playcard":
					JokerManager.instance.realCountCards.AddRange(
						sortedCards.Where(card =>
							card.cardData.is_Select == true &&
							card.cardData.include == true &&
							card.cardData.type == _type));

					num = JokerManager.instance.realCountCards.Count;
					break;

				case "handcard":
					JokerManager.instance.realCountCards.AddRange(
						sortedCards.Where(card =>
							card.cardData.is_Select == false &&
							card.cardData.type == _type));

					num = JokerManager.instance.realCountCards.Count;
					break;

				case "deckcardall":
					JokerManager.instance.realCountCards.AddRange(
						sortedCards.Where(card =>
							card.cardData.is_Select == true &&
							card.cardData.include == true &&
							JokerManager.instance.realCountCards.Contains(card) == false));

					num = cardLibrary.Count(card => card.type == _type);
					break;

				case "deckcardremain":
					JokerManager.instance.realCountCards.AddRange(
						sortedCards.Where(card =>
							card.cardData.is_Select == true &&
							card.cardData.include == true &&
							JokerManager.instance.realCountCards.Contains(card) == false));

					//중복 방지
					/*JokerManager.instance.realCountCards.AddRange(
						sortedCards.Where(card =>
							card.cardData.is_Select == true &&
							card.cardData.include == true &&
							JokerManager.instance.realCountCards.Contains(card) == false));*/

					num = cardLibrary.Count(card => card.type == _type && !card.dealt);
					break;
			}

			result = num;

			return num > 0;
		}

		public void Card_Growth(CARDDATA type, float growth)
		{
			/*var sortedCards = gameCards.OrderBy(card => card.transform.position.x).ToArray();

			// is_Select가 true인 카드만 새로운 배열로 저장
			var selectedCards = sortedCards.Where(card => card.cardData.is_Select).ToArray();*/

			foreach (var card in JokerManager.instance.realCountCards)
			{
				if (card.cardData.include)
				{
					if(type == CARDDATA.MULT)
						card.cardData.mult_growth += growth;
					else if (type == CARDDATA.CHIP)
						card.cardData.chip_growth += growth;

					DeckCardGrowth(card.cardData, type, growth);
				}
			}
		}

		int retrigger_step;
		List<int> list_Retrigger = new List<int>();

		float hand_Event_Start_Y;
		IEnumerator Event_Play_Hand(bool discard)
		{
			float speed = OptionManager.instance.Speed_Time;

			is_Event_Moving = true;

			yield return new WaitForSeconds(0.5f);

			hand_Event_Start_Y = 0.6f;

			//float spacing = 0.52f; // 카드 간 간격
			float spacing = 0.54f; // 카드 간 간격
			int selectedCount = gameCards.Count(card => card.cardData.is_Select); // 선택된 카드 수
			float centerX = 0f; // 중앙 기준 X 좌표
								//float startX = centerX - (spacing * (selectedCount) / 2) + 0.72f; // 첫 번째 카드의 X 위치
			float startX = centerX - (spacing * (selectedCount) / 2) + 1.347f; // 첫 번째 카드의 X 위치

			// position.x 기준으로 gameCards 정렬
			var sortedCards = gameCards.OrderBy(card => card.transform.position.x).ToArray();

			// is_Select가 true인 카드만 새로운 배열로 저장
			var selectedCards = sortedCards.Where(card => card.cardData.is_Select).ToArray();

			foreach (var card in selectedCards)
			{
				card.cardData.scoring = false;
			}

			Debug.Log("<color=yellow>AAA:SelectCard Num : </color>" + selectedCount.ToString());

			int index = 0;
			foreach (var card in selectedCards)
			{
				float posX = startX + (index * spacing); // X 위치 계산
														 //card.transform.position = new Vector3(card.transform.position.x, card.transform.position.y, -1f);
				Vector3 targetPos = new Vector3(posX, hand_Event_Start_Y, card.transform.position.z - 0.1f);
				card.SetTargetPos(targetPos, 0.1f * speed);
				index++;
			}

			yield return new WaitForSeconds(0.5f * speed);

			index = 0;
			foreach (var card in selectedCards)
			{
				if (card.cardData.include)
				{
					Vector3 targetPos = new Vector3(card.transform.position.x, hand_Event_Start_Y + 0.2f, -0.1f);
					card.SetTargetPos(targetPos, 0.1f * speed);
					index++;
				}
			}

			Debug.Log("<color=yellow>AAA:Included Num : </color>" + index.ToString());

			yield return new WaitForSeconds(0.5f * speed);


			#region timing_hand_play
			yield return StartCoroutine(JokerManager.instance.Start_Check_Joker(JOKERTIMING.HAND_PLAY));
			#endregion


			

			retrigger_step = 0;
			list_Retrigger.Clear();

			//goto_retrigger:

			foreach (var card in selectedCards)
			{
				if (card.cardData.include)
				{
					card.cardData.scoring = true;
					Vector3 originalPos = card.transform.position;

					if (EffectManager.instance)
					{
						GameObject obj = EffectManager.instance.CreateEffect(0, card.transform.position + new Vector3(0, 0.5f, 0)/*, card.transform*/);

						float value_chip = 0;
						float value_mult = 0;

						if (card.cardData.value <= CardValue.VALUE_10)
							value_chip = (int)card.cardData.value + 2;
						else if (card.cardData.value == CardValue.VALUE_A)
							value_chip = 11;
						else
							value_chip = 10;

						value_chip += card.cardData.chip_growth;
						value_mult += card.cardData.mult_growth;

						//카드 효과 흔들기============================================================
						if (obj && obj.GetComponent<Count>())
						{
							obj.GetComponent<Count>().SetDigit(value_chip);
							//obj.GetComponent<Count>().SetOriginColor();
							//obj.GetComponent<Count>().SetBackColor(Color.blue);
						}

						if (SoundManager.instance)
							SoundManager.instance.PlaySound("Card_Select");

						CardDataManager.instance.SetMyData(CARDDATA.CHIP, value_chip, CALC.PLUS);
						CardDataManager.instance.SetMyData(CARDDATA.MULT, value_mult, CALC.PLUS);

						StartCoroutine(ShakeCard(card));
						//============================================================카드 효과 흔들기

						StartCoroutine(EndShake(card, originalPos));

					}

					yield return new WaitForSeconds(0.5f * speed);

					#region timing_scoring
					yield return StartCoroutine(JokerManager.instance.Start_Check_Joker(JOKERTIMING.SCORING));
					#endregion

					card.cardData.scoring = false;
				}
			}

			#region timing_after_scoring
			yield return StartCoroutine(JokerManager.instance.Start_Check_Joker(JOKERTIMING.AFTER_SCORING));

			/*if (retrigger_step == 0)
				yield return StartCoroutine(JokerManager.instance.Start_Check_Joker(JOKERTIMING.AFTER_SCORING));
			else if (retrigger_step == 1)
			{
				retrigger_step++;
				goto goto_retrigger;
			}*/

			#endregion

			//joker event==================================================================

			Wins win_Type = (Wins)Paytable.the.GetCurrentWinIndex();

			switch (win_Type)
			{
				case Wins.WIN_ONE_PAIR:
				case Wins.WIN_TWO_PAIR:
					JokerManager.instance.Event_Joker("JOKER.type_1");
					yield return new WaitForSeconds(0.5f * speed);
					break;

				case Wins.WIN_THREE_OF_A_KIND:
					JokerManager.instance.Event_Joker("JOKER.type_2");
					yield return new WaitForSeconds(0.5f * speed);
					break;
			}

			//==================================================================joker event


			/*if (EffectManager.instance)
			{
				GameObject obj = EffectManager.instance.CreateEffect(1, new Vector3(centerX + 0.40f, hand_Event_Start_Y + 0.63f, 0)*//*, card.transform*//*);
				if (obj && obj.GetComponent<Result>())
				{
					if (Enum.IsDefined(typeof(Wins), win_Type))
						obj.GetComponent<Result>().SetText(CardDataManager.name_Wins[(int)win_Type]);
				}
			}*/

			CardDataManager.instance.ScoreCount();


			lastHit = Character.instance.Attack_Manual((int)win_Type);

			yield return new WaitForSeconds(0.1f * speed);

			if (lastHit)
			{
				ClearHand();
				is_Event_Moving = false;
			}
			else
				StartCoroutine(Vanishing_And_Sort_Deal(discard));

			yield return null;

			UI_Card.instance.SetWinsName(CardDataManager.instance.GetNowDamage().ToString());

			Paytable.the.EnhanceCount(win_Type);

			/*yield return new WaitForSeconds(1);

			CardDataManager.instance.SetMyData(CARDDATA.CHIP, 0);
			CardDataManager.instance.SetMyData(CARDDATA.MULT, 0);
			CardDataManager.OnEvaluateChange((Wins)(-1));*/

			JokerManager.instance.DeleteAllJokerEffect();
		}

		public bool lastHit;

		IEnumerator EndShake(Card card, Vector3 originalPos)
		{
			yield return new WaitForSeconds(0.5f * OptionManager.instance.Speed_Time);

			card.transform.position = originalPos; // 원래 위치로 복구

			Vector3 targetPos = new Vector3(card.transform.position.x, hand_Event_Start_Y + 0.1f, 1.0f);
			card.SetTargetPos(targetPos, 0.1f * OptionManager.instance.Speed_Time);
		}

		// 카드 흔들기 효과
		IEnumerator ShakeCard(Card card)
		{
			Vector3 originalPos = card.transform.position;
			float shakeAmount = 0.02f; // 흔들리는 정도
			float shakeSpeed = 0.02f;  // 흔들리는 속도

			for (int i = 0; i < 3; i++) // 3번 흔들기
			{
				card.transform.position = originalPos + new Vector3(shakeAmount, 0, 0);
				yield return new WaitForSeconds(shakeSpeed * OptionManager.instance.Speed_Time);
				card.transform.position = originalPos - new Vector3(shakeAmount, 0, 0);
				yield return new WaitForSeconds(shakeSpeed * OptionManager.instance.Speed_Time);
			}

			/*card.transform.position = originalPos; // 원래 위치로 복구

			Vector3 targetPos = new Vector3(card.transform.position.x, hand_Event_Start_Y + 0.1f, 1.0f);
			card.SetTargetPos(targetPos, 0.1f);*/
		}

		IEnumerator ShakeCard(GameObject card)
		{
			Vector3 originalPos = card.transform.position;
			float shakeAmount = 0.02f; // 흔들리는 정도
			float shakeSpeed = 0.02f;  // 흔들리는 속도

			for (int i = 0; i < 3; i++) // 3번 흔들기
			{
				card.transform.position = originalPos + new Vector3(shakeAmount, 0, 0);
				yield return new WaitForSeconds(shakeSpeed * OptionManager.instance.Speed_Time);
				card.transform.position = originalPos - new Vector3(shakeAmount, 0, 0);
				yield return new WaitForSeconds(shakeSpeed * OptionManager.instance.Speed_Time);
			}

			card.transform.position = originalPos; // 원래 위치로 복구

			/*Vector3 targetPos = new Vector3(card.transform.position.x, hand_Event_Start_Y + 0.1f, 1.0f);
			card.SetTargetPos(targetPos, 0.1f);*/
		}

		IEnumerator Vanishing_And_Sort_Deal(bool discard)
		{
			is_Event_Moving = true;

			if (discard)
				yield return new WaitForSeconds(0.5f);

			MainGame.the.gameState = MainGame.STATE_DEALING2;

			// gameCards 리스트를 x 값 기준으로 정렬
			gameCards = gameCards.OrderBy(card => card.transform.position.x).ToArray();

			int cardsDealt = 0; // count separately the non-holded cards

			for (int i = 0; i < num_Hand; i++)
			{
				bool disappear = gameCards[i].cardData.is_Select;

				// deal only the non-holded cards
				if (disappear)
				{
					// generate a new random card from the deck
					// and assign it to the card on the screen

					gameCards[i].SetLibraryCard(GetNewCardFromTheDeck());

					// start vanish animation and prepare the deal
					//gameCards[i].VanishAndDealAgainWithDelay(cardsDealt * secondHandDealDelayBetweenCards);
					gameCards[i].ClearAfterDeal();
					Debug.Log("AAA ClearAfterDeal");
					cardsDealt++;

					if (!discard)
						gameCards[i].ResetHold();
				}
				else
				{
					gameCards[i].ResetHold(); // remove hold marker for holded cards
				}
			}

			MainGame.the.cardsToDeal = cardsDealt;
			yield return new WaitForSeconds(0.33f * OptionManager.instance.Speed_Time);

			/*//trVanish 가 모두 끝날때까지 기다림
			while (true)
			{
				if(MainGame.the.cardsToDeal == 0)
					break;

				yield return null;
			}*/


			cardsDealt = 0;

			for (int i = 0; i < num_Hand; i++)
			{
				bool disappear = gameCards[i].cardData.is_Select;

				gameCards[i].InitX();
				gameCards[i].InitY();
				gameCards[i].InitScale();

				if (disappear)
				{
					cardsDealt++;

					gameCards[i].Copy_From_Library();
				}
				else
				{
					gameCards[i].ResetHold(); // remove hold marker for holded cards
				}
			}

			//InitX();
			Sort_Card();

			cardsDealt = 1;
			for (int i = 0; i < num_Hand; i++)
			{
				bool disappear = gameCards[i].cardData.is_Select;

				if (disappear)
				{
					gameCards[i].DealWithDelay(cardsDealt * firstHandDealDelayBetweenCards);

					cardsDealt++;
				}
				else
				{
					gameCards[i].ResetHold(); // remove hold marker for holded cards
				}
			}

			is_Event_Moving = false;
		}

		public bool Is_Event_Moving()
		{
			return is_Event_Moving;
		}

		public void Sort_Card(bool direct = false)
		{
			switch (sort_Type)
			{
				case SORT.RANK:
					Sort_Rank_And_Move(direct);
					break;

				case SORT.SUITE:
					Sort_Suite_And_Move(direct);
					break;
			}
		}

		public void Sort_Rank_And_Move(bool direct = false)
		{
			List<Vector3> sortedPositions = new List<Vector3>(); // x 위치 기준 정렬된 좌표 저장

			// 현재 카드들의 위치를 x 값 기준으로 정렬하여 저장
			sortedPositions = gameCards
					.OrderBy(card => card.transform.position.x)  // x 위치 기준 정렬
					.Select(card => card.transform.position)     // 위치만 저장
					.ToList();

			// 카드 리스트를 CardValue 내림차순, CardType 내림차순 정렬
			gameCards = gameCards
				.OrderByDescending(card => card.cardData.value) // 1차 정렬: CardValue 내림차순
				.ThenByDescending(card => card.cardData.type)   // 2차 정렬: CardType 내림차순
				.ToArray();

			// 정렬된 카드들을 정렬된 x 좌표 순서로 이동
			for (int i = 0; i < gameCards.Length; i++)
			{
				Vector3 targetPosition = new Vector3(sortedPositions[i].x, gameCards[i].transform.position.y, gameCards[i].transform.position.z);

				if (direct)
				{
					gameCards[i].transform.position = targetPosition;
				}
				else
					gameCards[i].SetTargetPos(targetPosition, 0.13f);
			}
		}


		public void Sort_Suite_And_Move(bool direct = false)
		{
			List<Vector3> sortedPositions = new List<Vector3>(); // x 위치 기준 정렬된 좌표 저장

			// 현재 카드들의 위치를 x 값 기준으로 정렬하여 저장
			sortedPositions = gameCards
					.OrderBy(card => card.transform.position.x)  // x 위치 기준 정렬
					.Select(card => card.transform.position)     // 위치만 저장
					.ToList();

			// 카드 리스트를 CardValue 내림차순, CardType 내림차순 정렬
			gameCards = gameCards
				.OrderBy(card => card.cardData.type) // 1차 정렬: CardValue 내림차순
				.ThenByDescending(card => card.cardData.value)   // 2차 정렬: CardType 내림차순
				.ToArray();

			// 정렬된 카드들을 정렬된 x 좌표 순서로 이동
			for (int i = 0; i < gameCards.Length; i++)
			{
				Vector3 targetPosition = new Vector3(sortedPositions[i].x, gameCards[i].transform.position.y, gameCards[i].transform.position.z);

				if (direct)
				{
					gameCards[i].transform.position = targetPosition;
				}
				else
					gameCards[i].SetTargetPos(targetPosition, 0.13f);
			}
		}

		public void InitHold()
		{
			for (int i = 0; i < num_Hand; i++)
			{
				/*if (!gameCards[i].IsHolded())
				{
				}
				else*/
				gameCards[i].ResetHold();
			}
		}

		public void InitSelect()
		{
			for (int i = 0; i < num_Hand; i++)
			{
				gameCards[i].cardData.is_Select = false;
			}

			lastHit = false;
		}

		public void InitGrowth()
		{
			for (int i = 0; i < num_Hand; i++)
			{
				gameCards[i].cardData.chip_growth = 0;
				gameCards[i].cardData.mult_growth = 0;
			}

			lastHit = false;
		}

		public int Count_Num_Select(bool invoke = true)
		{
			int num = 0;
			for (int i = 0; i < num_Hand; i++)
			{
				if (gameCards[i].cardData.is_Select)
					num++;
			}

			if (invoke)
				OnCardSelect?.Invoke(num);

			return num;
		}

		public bool Is_All_Idle()
		{
			int num = 0;
			for (int i = 0; i < num_Hand; i++)
			{
				if (gameCards[i].Is_Card_Idle() == false)
					return false;
			}

			return true;
		}

		public Card[] GetAllCards()
		{
			return gameCards;
		}

		public Card[] SelectCards()
		{
			// position.x 기준으로 gameCards 정렬
			var sortedCards = gameCards.OrderBy(card => card.transform.position.x).ToArray();

			// is_Select가 true인 카드만 새로운 배열로 저장
			var selectedCards = sortedCards.Where(card => card.cardData.is_Select).ToArray();

			return selectedCards;
		}
		//--------------------------------------------------------

		public void EvaluateHand()
		{
			Paytable.the.ResetWins();

			// copy cards into a separate array of cards
			// (they will be sorted and better not mess up original cards)
			for (int i = 0; i < num_Hand; i++)
				workCards[i].CopyInfoFrom(gameCards[i].cardData);

			/*
			// evaluate the temp hand
			HandEvaluator.Evaluate(workCards,
									   true, // auto-hold enabled
									true); // show wins only in the RESULTS stage MainGame.the.gameState == MainGame.STATE_SHOW_RESULTS
			*/

			//HandEvaluator.Check_Royal_Straight_Flush(workCards, true, true);
			if (HandEvaluator.Check_Straight_Flush(workCards, false, true) == false)
				if (HandEvaluator.Check_FourCard(workCards, false, true) == false)
					if (HandEvaluator.Check_FullHouse(workCards, false, true) == false)
						if (HandEvaluator.Check_Flush(workCards, false, true) == false)
							if (HandEvaluator.Check_Straight(workCards, false, true) == false)
								if (HandEvaluator.Check_ThreeCard(workCards, false, true) == false)
									if (HandEvaluator.Check_TwoPair(workCards, false, true) == false)
										if (HandEvaluator.Check_OnePair(workCards, false, true) == false)
											HandEvaluator.Check_HighCard(workCards, false, true);

			bool cardsWereHolded = false;

			// apply auto holds to the cards on the screen or highlight winner cards
			foreach (CardData workCard in workCards)
			{
				foreach (Card screenCard in gameCards)
				{
					// compare the work cards and the screen cards
					if (workCard.sprite == screenCard.cardData.sprite)
					{
						screenCard.cardData.include = workCard.include;
						// if we are after first deal
						if (MainGame.the.gameState == MainGame.STATE_WAIT_HOLD)
						{
							// if the card was auto-holded, mark it on the screen
							if (workCard.hold)
							{
								screenCard.ToggleHold();
								cardsWereHolded = true;
							}
						}
						else // game end
							if (MainGame.the.gameState == MainGame.STATE_SHOW_RESULTS)
						{
							//screenCard.SetResultsState(workCard.hold);
						}

						//*****************
						// NOTE
						// the cards are 'holded' internally in the second hand too,
						// but are not updated on the screen with the HOLD tag because
						// we don't need this in the final results stage
						//*****************

						break;
					}
				}
			}

			// play autohold sound
			if (cardsWereHolded)
				SoundsManager.the.autoHoldSound.Play();
		}


		public bool Evaluate_InMyHand(Wins win_comp)
		{
			for (int i = 0; i < num_Hand; i++)
			{
				workCards[i].CopyInfoFrom(gameCards[i].cardData);
				workCards[i].is_Select = !workCards[i].is_Select;  //reverse
			}

			switch (win_comp)
			{
				case Wins.WIN_STRAIGHT_FLUSH:
					return HandEvaluator.Check_Straight_Flush(workCards, false, false);

				case Wins.WIN_FOUR_OF_A_KIND:
					return HandEvaluator.Check_FourCard(workCards, false, false);

				case Wins.WIN_FULL_HOUSE:
					return HandEvaluator.Check_FullHouse(workCards, false, false);

				case Wins.WIN_FLUSH:
					return HandEvaluator.Check_Flush(workCards, false, false);

				case Wins.WIN_STRAIGHT:
					return HandEvaluator.Check_Straight(workCards, false, false);

				case Wins.WIN_THREE_OF_A_KIND:
					return HandEvaluator.Check_ThreeCard(workCards, false, false);

				case Wins.WIN_TWO_PAIR:
					return HandEvaluator.Check_TwoPair(workCards, false, false);

				case Wins.WIN_ONE_PAIR:
					return HandEvaluator.Check_OnePair(workCards, false, false);

				case Wins.WIN_HIGH_CARD:
					return HandEvaluator.Check_HighCard(workCards, false, false);
			}
			return false;
		}
		//--------------------------------------------------------

		IEnumerator CheckHoldAnimationFinished()
		{
			while (true)
			{
				bool all_idle = true;
				foreach (var obj in gameCards)
				{
					if (obj.Is_Card_Idle() == false)
					{
						all_idle = false;
						break;
					}
				}

				if (all_idle)
				{
					if (CardDataManager.instance.is_AutoPlay)
					{
						/*int damage = CardDataManager.instance.GetNowDamage();
						if (damage == 0)*/
						if (CardDataManager.instance.Is_User_Checked(Paytable.the.GetCurrentWinIndex()))
							MainGame.the.PlayHand();
						else
						{
							if (MainGame.the.PlayDiscard() == false)
								MainGame.the.PlayHand();
						}
					}
				}

				yield return null;
			}
		}
	}
}