using UnityEngine;
using System.Collections;
using static UnityEngine.Rendering.GPUSort;
using System.Linq;
using Unity.VisualScripting;
using System.Collections.Generic;

namespace VideoPokerKit
{
	public class HandEvaluator
	{
		//*************
		// NOTE
		// This working class is used for evaluating a poker hand (5 cards)
		// and detect the best possible win (with the greatest outcome)
		// The methods and members are static so that they can be called directly
		// without the need to use an instance object
		//*************


		static CardData[] workCards;
		static int[] cardsValueCount = new int[(int)CardValue.VALUES_NO];
		static int[] cardsTypeCount = new int[(int)CardType.TYPES_NO];

		// all combinations of sorted cards for STRAIGHT 
		// (for this type of win, it is more simpler to do it like this instead of using an algorithm)
		static CardValue[][] straightSets = new CardValue[][] {
		new CardValue[] {CardValue.VALUE_2, CardValue.VALUE_3, CardValue.VALUE_4, CardValue.VALUE_5, CardValue.VALUE_A},
		new CardValue[] {CardValue.VALUE_2, CardValue.VALUE_3, CardValue.VALUE_4, CardValue.VALUE_5, CardValue.VALUE_6},
		new CardValue[] {CardValue.VALUE_3, CardValue.VALUE_4, CardValue.VALUE_5, CardValue.VALUE_6, CardValue.VALUE_7},
		new CardValue[] {CardValue.VALUE_4, CardValue.VALUE_5, CardValue.VALUE_6, CardValue.VALUE_7, CardValue.VALUE_8},
		new CardValue[] {CardValue.VALUE_5, CardValue.VALUE_6, CardValue.VALUE_7, CardValue.VALUE_8, CardValue.VALUE_9},
		new CardValue[] {CardValue.VALUE_6, CardValue.VALUE_7, CardValue.VALUE_8, CardValue.VALUE_9, CardValue.VALUE_10},
		new CardValue[] {CardValue.VALUE_7, CardValue.VALUE_8, CardValue.VALUE_9, CardValue.VALUE_10, CardValue.VALUE_J},
		new CardValue[] {CardValue.VALUE_8, CardValue.VALUE_9, CardValue.VALUE_10, CardValue.VALUE_J, CardValue.VALUE_Q},
		new CardValue[] {CardValue.VALUE_9, CardValue.VALUE_10, CardValue.VALUE_J, CardValue.VALUE_Q, CardValue.VALUE_K},
		new CardValue[] {CardValue.VALUE_10, CardValue.VALUE_J, CardValue.VALUE_Q, CardValue.VALUE_K, CardValue.VALUE_A}};

		//--------------------------------------------------------
		// internal class used to compare 2 cards
		public class CardComparer : IComparer
		{
			int IComparer.Compare(System.Object x, System.Object y)
			{
				return (((CardData)x).value >= ((CardData)y).value ? 1 : -1);
			}
		}

		//--------------------------------------------------------

		// from the specified array, hold the card with the passed value and type
		static void HoldCard(CardData[] cards, CardValue value, CardType type)
		{
			/*foreach (CardData card in cards)
			{
				if (card.value == value && card.type == type)
				{
					card.hold = true;
					return;
				}
			}*/
		}



		// from the specified array, hold all the cards with a specific value
		static void ValueCardsByValue(CardData[] cards, CardValue value)
		{
			foreach (CardData card in cards)
			{
				if (card.value == value)
					card.include = true;
			}
		}


		static void ValueCardsByType(CardData[] cards, CardType type)
		{
			foreach (CardData card in cards)
			{
				if (card.type == type)
					card.include = true;
			}
		}

		static void HoldAll(CardData[] cards)
		{
			/*foreach (CardData card in cards)
				card.hold = true;*/
		}

		//--------------------------------------------------------

		static void InitCardArray(CardData[] cards)
		{
			// count cards by value
			for (int i = 0; i < cardsValueCount.Length; i++)
				cardsValueCount[i] = 0; // first reset all counters
			for (int i = 0; i < cards.Length; i++)
			{
				if (cards[i].is_Select)
					cardsValueCount[(int)cards[i].value]++; // count

				cards[i].include = false;
			}

			// count types by type
			for (int i = 0; i < cardsTypeCount.Length; i++)
				cardsTypeCount[i] = 0;
			for (int i = 0; i < cards.Length; i++)
				if (cards[i].is_Select)
					cardsTypeCount[(int)cards[i].type]++;
		}

		public static bool Check_Royal_Straight_Flush(CardData[] cards, bool autoHold, bool showWin)
		{
			bool bStraight = false;
			bool bFlush = false;

			bool bRoyalFlushStraight = false;

			System.Array.Sort(cards, new CardComparer());

			InitCardArray(cards);

			// check flush
			for (int i = 0; i < cardsTypeCount.Length; i++)
				if (cardsTypeCount[i] == 5)
				{
					bFlush = true;
				}

			// check straight
			for (int i = 0; i < straightSets.Length; i++)
			{
				int match = 0;
				for (int j = 0; j < cards.Length; j++)
					if (cards[j].value == straightSets[i][j])
						match++;

				if (match == 5)
				{
					// the last values in 'straightSets' has the potential of being a RoyalFlush
					if (i == straightSets.Length - 1)
					{
						bRoyalFlushStraight = true;
					}

					bStraight = true;
					break;
				}
			}

			// check royal flush
			if (bRoyalFlushStraight && bFlush)
			{
				/*if (showWin)
					Paytable.the.SetCurrentWin(Wins.WIN_ROYAL_FLUSH);*/

				foreach (var card in cards)
					if (cardsTypeCount[(int)card.type] == 5 && straightSets.Last().Contains(card.value))
						ValueCardsByValue(cards, card.value);


				Debug.Log("Result_RoyalStraightFlush");
				return true;
			}

			return false;
		}

		public static bool Check_Straight_Flush(CardData[] cards, bool autoHold, bool showWin)
		{
			bool bStraight = false;
			bool bFlush = false;
			bool bStraightFlush = false;

			System.Array.Sort(cards, new CardComparer());
			InitCardArray(cards);

			// Flush 체크 (선택된 카드들만 같은 무늬인지 확인)
			Dictionary<CardType, int> suitCount = new Dictionary<CardType, int>();
			foreach (var card in cards)
			{
				if (!card.is_Select) continue;
				if (!suitCount.ContainsKey(card.type))
					suitCount[card.type] = 0;
				suitCount[card.type]++;
			}

			foreach (var count in suitCount.Values)
			{
				if (count >= 5) // 5장 이상 같은 무늬일 때 Flush 성립
				{
					bFlush = true;
					break;
				}
			}

			// Straight 체크 (is_Select == true인 카드들만 연속된지 확인)
			int consecutiveCount = 1;
			int lastSelectedIndex = -1;
			int startIndex = -1;

			for (int i = 0; i < cards.Length; i++)
			{
				if (!cards[i].is_Select)
					continue;

				if (lastSelectedIndex != -1 && (int)cards[i].value == (int)cards[lastSelectedIndex].value + 1)
				{
					consecutiveCount++;
				}
				else
				{
					consecutiveCount = 1;
					startIndex = i;
				}

				lastSelectedIndex = i;

				if (consecutiveCount == 5)
				{
					bStraight = true;
					startIndex = lastSelectedIndex - 4;
					break;
				}
			}

			// 특수 케이스: A-2-3-4-5 스트레이트 처리
			List<CardValue> selectedValues = new List<CardValue>();
			foreach (var card in cards)
			{
				if (card.is_Select)
					selectedValues.Add(card.value);
			}

			if (!bStraight &&
				selectedValues.Contains(CardValue.VALUE_A) &&
				selectedValues.Contains(CardValue.VALUE_2) &&
				selectedValues.Contains(CardValue.VALUE_3) &&
				selectedValues.Contains(CardValue.VALUE_4) &&
				selectedValues.Contains(CardValue.VALUE_5))
			{
				bStraight = true;
				startIndex = 0;
			}

			// Straight + Flush → Straight Flush
			if (bStraight && bFlush)
			{
				bStraightFlush = true;
				if (showWin)
					Paytable.the.SetCurrentWin(Wins.WIN_STRAIGHT_FLUSH);

				int holdCount = 0;
				//for (int i = startIndex; i < cards.Length && holdCount < 5; i++)
				for (int i = 0; i < cards.Length && holdCount < 5; i++)
				{
					if (cards[i].is_Select)
					{
						cards[i].include = true;
						holdCount++;
					}
				}

				Debug.Log("Result_StraightFlush");
				return true;
			}

			return false;
		}


		public static bool Check_FourCard(CardData[] cards, bool autoHold, bool showWin)
		{
			System.Array.Sort(cards, new CardComparer());

			InitCardArray(cards);

			//for (int i = 0; i < cardsValueCount.Length; i++)
			for (int i = cardsValueCount.Length - 1; i >= 0; i--)
			{
				if (cardsValueCount[i] == 4)
				{
					if (showWin)
						Paytable.the.SetCurrentWin(Wins.WIN_FOUR_OF_A_KIND);

					ValueCardsByValue(cards, (CardValue)i);

					Debug.Log("Result_FourCard");
					return true;
				}
			}

			return false;
		}

		public static bool Check_FullHouse(CardData[] cards, bool autoHold, bool showWin)
		{
			bool bOnePair = false;
			bool bTwoPair = false;
			bool bThree = false;
			bool bStraight = false;
			bool bFlush = false;
			bool bFullHouse = false;
			bool bFour = false;
			bool bStraightFlush = false;
			bool bRoyalFlushStraight = false;

			int firstPairValue = 0;

			System.Array.Sort(cards, new CardComparer());

			InitCardArray(cards);

			//for (int i = 0; i < cardsValueCount.Length; i++)
			for (int i = cardsValueCount.Length - 1; i >= 0; i--)
			{
				if (cardsValueCount[i] == 2)
				{
					if (!bOnePair)
					{
						bOnePair = true;
						firstPairValue = i;
					}
				}

				if (cardsValueCount[i] == 3)
				{
					bThree = true;
				}
			}

			if (bOnePair && bThree)
			{
				bFullHouse = true;
				if (showWin)
					Paytable.the.SetCurrentWin(Wins.WIN_FULL_HOUSE);

				ValueCardsByValue(cards, (CardValue)firstPairValue);
				foreach (var card in cards)
					if (cardsValueCount[(int)card.value] == 3)
						ValueCardsByValue(cards, card.value);


				Debug.Log("Result_FullHouse");
				return true;
			}

			return false;
		}

		public static bool Check_Flush(CardData[] cards, bool autoHold, bool showWin)
		{
			bool bFlush = false;

			System.Array.Sort(cards, new CardComparer());

			InitCardArray(cards);

			for (int i = 0; i < cardsTypeCount.Length; i++)
				if (cardsTypeCount[i] == 5)
				{
					bFlush = true;
					if (showWin)
						Paytable.the.SetCurrentWin(Wins.WIN_FLUSH);

					ValueCardsByType(cards, (CardType)i);

					Debug.Log("Result_Flush");
					return true;
				}

			return false;
		}


		public static bool Check_Straight(CardData[] cards, bool autoHold, bool showWin)
		{
			bool bStraight = false;

			System.Array.Sort(cards, new CardComparer());
			InitCardArray(cards);

			int consecutiveCount = 1;
			int lastSelectedIndex = -1; // 마지막으로 선택된 카드의 인덱스
			int startIndex = -1; // 스트레이트의 시작 위치

			for (int i = 0; i < cards.Length; i++)
			{
				if (!cards[i].is_Select)
					continue;

				if (lastSelectedIndex != -1 && (int)cards[i].value == (int)cards[lastSelectedIndex].value + 1)
				{
					consecutiveCount++;
				}
				else
				{
					consecutiveCount = 1;
					startIndex = i;
				}

				lastSelectedIndex = i; // 현재 선택된 카드를 마지막 선택 카드로 설정

				if (consecutiveCount == 5)
				{
					bStraight = true;
					startIndex = lastSelectedIndex - 4; // 5장 연속이므로 시작 위치 조정
					break;
				}
			}

			// 특수 케이스: A-2-3-4-5 스트레이트 처리
			if (!bStraight)
			{
				List<CardValue> selectedValues = new List<CardValue>();
				foreach (var card in cards)
				{
					if (card.is_Select)
						selectedValues.Add(card.value);
				}

				if (selectedValues.Contains(CardValue.VALUE_A) &&
					selectedValues.Contains(CardValue.VALUE_2) &&
					selectedValues.Contains(CardValue.VALUE_3) &&
					selectedValues.Contains(CardValue.VALUE_4) &&
					selectedValues.Contains(CardValue.VALUE_5))
				{
					bStraight = true;
					startIndex = 0;
				}
			}

			if (bStraight)
			{
				if (showWin)
					Paytable.the.SetCurrentWin(Wins.WIN_STRAIGHT);

				int holdCount = 0;
				//for (int i = startIndex; i < cards.Length && holdCount < 5; i++)
				for (int i = 0; i < cards.Length && holdCount < 5; i++)
				{
					if (cards[i].is_Select)
					{
						cards[i].include = true;
						holdCount++;
					}
				}


				Debug.Log("Result_Straight");
				return true;
			}

			return false;
		}




		public static bool Check_ThreeCard(CardData[] cards, bool autoHold, bool showWin)
		{
			System.Array.Sort(cards, new CardComparer());

			InitCardArray(cards);

			//for (int i = 0; i < cardsValueCount.Length; i++)
			for (int i = cardsValueCount.Length - 1; i >= 0; i--)
			{
				if (cardsValueCount[i] == 3)
				{
					if (showWin)
						Paytable.the.SetCurrentWin(Wins.WIN_THREE_OF_A_KIND);

					ValueCardsByValue(cards, (CardValue)i);

					Debug.Log("Result_ThreeCard");
					return true;
				}
			}

			return false;
		}

		public static bool Check_TwoPair(CardData[] cards, bool autoHold, bool showWin)
		{
			bool bOnePair = false;
			bool bTwoPair = false;

			int firstPairValue = 0;

			System.Array.Sort(cards, new CardComparer());

			InitCardArray(cards);

			//for (int i = 0; i < cardsValueCount.Length; i++)
			for (int i = cardsValueCount.Length - 1; i >= 0; i--)
			{
				if (cardsValueCount[i] == 2)
				{
					if (!bOnePair)
					{
						bOnePair = true;
						firstPairValue = i;
					}
					else
					{
						bTwoPair = true;

						if (showWin)
							Paytable.the.SetCurrentWin(Wins.WIN_TWO_PAIR);

						ValueCardsByValue(cards, (CardValue)i);
						ValueCardsByValue(cards, (CardValue)firstPairValue);

						int cardnum = cards.Count(card => card.include);
						if (cardnum != 4)
							cardnum = cardnum;

						Debug.Log("<color=yellow>AAA:before Num : </color>" + cardnum.ToString());
						Debug.Log("Result_TwoPair");
						return true;
					}
				}
			}

			return false;
		}

		public static bool Check_OnePair(CardData[] cards, bool autoHold, bool showWin)
		{
			bool bOnePair = false;
			bool bTwoPair = false;

			int firstPairValue = 0;

			System.Array.Sort(cards, new CardComparer());

			InitCardArray(cards);

			//for (int i = 0; i < cardsValueCount.Length; i++)
			for (int i = cardsValueCount.Length - 1; i >= 0; i--)
			{
				if (cardsValueCount[i] == 2)
				{
					if (!bOnePair)
					{
						bOnePair = true;
						firstPairValue = i;
						if (showWin)
							Paytable.the.SetCurrentWin(Wins.WIN_ONE_PAIR);

						ValueCardsByValue(cards, (CardValue)i);

						Debug.Log("Result_OnePair");
						return true;
					}
				}
			}

			return false;
		}

		public static bool Check_HighCard(CardData[] cards, bool autoHold, bool showWin)
		{
			if (cards == null || cards.Length == 0)
				return false;

			System.Array.Sort(cards, new CardComparer()); // 카드 정렬 (오름차순)

			CardValue highestValue = CardValue.VALUES_NO; // 초기값을 VALUES_NO로 설정

			int select_idx = -1;
			// 먼저 선택된 카드 중 가장 높은 값을 찾기
			for (int i = 0; i < cards.Length; i++)
			{
				if (cards[i].is_Select)
				{
					if (highestValue == CardValue.VALUES_NO)
					{
						highestValue = cards[i].value;
						select_idx = i;
					}
					else if (cards[i].value > highestValue)
					{
						highestValue = cards[i].value;
						select_idx = i;
					}
				}
			}

			if (select_idx != -1)
			{
				if (showWin)
					Paytable.the.SetCurrentWin(Wins.WIN_HIGH_CARD);

				cards[select_idx].include = true;

				Debug.Log("Result_HighCard");

				return true;
			}

			return false;
		}

		public static void Evaluate(CardData[] cards, bool autoHold, bool showWin)
		{
			// define all win flags needed in video poker
			bool bOnePair = false;
			bool bTwoPair = false;
			bool bThree = false;
			bool bStraight = false;
			bool bFlush = false;
			bool bFullHouse = false;
			bool bFour = false;
			bool bStraightFlush = false;
			bool bRoyalFlushStraight = false;

			int firstPairValue = 0;

			System.Array.Sort(cards, new CardComparer());

			for (int i = 0; i < cardsValueCount.Length; i++)
				cardsValueCount[i] = 0;
			for (int i = 0; i < cards.Length; i++)
				cardsValueCount[(int)cards[i].value]++;

			for (int i = 0; i < cardsTypeCount.Length; i++)
				cardsTypeCount[i] = 0;
			for (int i = 0; i < cards.Length; i++)
				cardsTypeCount[(int)cards[i].type]++;

			for (int i = 0; i < cardsValueCount.Length; i++)
			{
				if (cardsValueCount[i] == 2)
				{
					if (!bOnePair)
					{
						bOnePair = true;
						firstPairValue = i;
						if (showWin)
							Paytable.the.SetCurrentWin(Wins.WIN_ONE_PAIR);

						ValueCardsByValue(cards, (CardValue)i);
					}
					else
					{
						bTwoPair = true;
						if (showWin)
							Paytable.the.SetCurrentWin(Wins.WIN_TWO_PAIR);

						ValueCardsByValue(cards, (CardValue)i);
						ValueCardsByValue(cards, (CardValue)firstPairValue);

					}
				}
				if (cardsValueCount[i] == 3)
				{
					bThree = true;
					if (showWin)
						Paytable.the.SetCurrentWin(Wins.WIN_THREE_OF_A_KIND);

					ValueCardsByValue(cards, (CardValue)i);
				}
				if (cardsValueCount[i] == 4)
				{
					bFour = true;
					if (showWin)
						Paytable.the.SetCurrentWin(Wins.WIN_FOUR_OF_A_KIND);

					ValueCardsByValue(cards, (CardValue)i);
				}
			}

			for (int i = 0; i < cardsTypeCount.Length; i++)
				if (cardsTypeCount[i] == 5)
				{
					bFlush = true;
					if (showWin)
						Paytable.the.SetCurrentWin(Wins.WIN_FLUSH);

					ValueCardsByType(cards, (CardType)i);
				}

			for (int i = 0; i < straightSets.Length; i++)
			{
				int match = 0;
				for (int j = 0; j < cards.Length; j++)
					if (cards[j].value == straightSets[i][j])
						match++;

				if (match == 5)
				{
					if (i == straightSets.Length - 1)
					{
						bRoyalFlushStraight = true;
					}

					bStraight = true;
					if (showWin)
						Paytable.the.SetCurrentWin(Wins.WIN_STRAIGHT);

					foreach (var card in cards)
						if (straightSets[i].Contains(card.value))
							ValueCardsByValue(cards, card.value);

					break;
				}
			}

			if (bOnePair && bThree)
			{
				bFullHouse = true;
				if (showWin)
					Paytable.the.SetCurrentWin(Wins.WIN_FULL_HOUSE);

				ValueCardsByValue(cards, (CardValue)firstPairValue);
				foreach (var card in cards)
					if (cardsValueCount[(int)card.value] == 3)
						ValueCardsByValue(cards, card.value);

			}

			if (bStraight && bFlush)
			{
				bStraightFlush = true;
				if (showWin)
					Paytable.the.SetCurrentWin(Wins.WIN_STRAIGHT_FLUSH);

				foreach (var card in cards)
					if (cardsTypeCount[(int)card.type] == 5 && straightSets.Any(set => set.Contains(card.value)))
						ValueCardsByValue(cards, card.value);

			}

			if (bRoyalFlushStraight && bFlush)
			{
				/*if (showWin)
					Paytable.the.SetCurrentWin(Wins.WIN_ROYAL_FLUSH);*/

				foreach (var card in cards)
					if (cardsTypeCount[(int)card.type] == 5 && straightSets.Last().Contains(card.value))
						ValueCardsByValue(cards, card.value);

			}
		}
	}
}