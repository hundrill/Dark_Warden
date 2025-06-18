using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static Character;

namespace VideoPokerKit
{
	public class MainGame : MonoBehaviour
	{
		// reference for use in static calls
		public static MainGame the;

		public delegate void onCardGameStateChange(byte gs);
		public static onCardGameStateChange OnCardGameStateChange;

		// game states
		byte _gameState;
		[HideInInspector]
		public byte gameState
		{
			set
			{
				_gameState = value;
				OnCardGameStateChange?.Invoke(value);
			}

			get { return _gameState; }
		}
		public static byte STATE_IDLE = 0;  // idle state: waiting for player to press DEAL
		public static byte STATE_DEALING = 1; // first hand deal is animating
		public static byte STATE_WAIT_HOLD = 2; // waiting for the player to hold cards
		public static byte STATE_DEALING2 = 3; // second hand deal is animating
		public static byte STATE_SHOW_RESULTS = 4; // compute & show results

		// starting value for the credit
		[HideInInspector]
		public float playerCredit = 100;

		// current bet used by the player
		[HideInInspector]
		public float playerBet = 1;

		// value to hold the win from the current game
		[HideInInspector]
		public float currentWin = 0;

		// object that flashes showing the current win amount
		[Header("Win amount message")]
		bool showWinAmount = false;
		public GameObject winAmountObj;

		// reference to win amount text
		public TextMesh winAmountText;

		// bet settings, set them in the Inspector
		[Header("Bet settings")]
		public float betStep = 1;
		public float minBet = 1;
		public float maxBet = 10;

		// bet changes callback
		public delegate void BetUpdatedCallback(float newValue);
		public static BetUpdatedCallback BetUpdated;

		// credit changes callback
		public delegate void CreditUpdatedCallback(float newValue);
		public static CreditUpdatedCallback CreditUpdated;

		// win amount changes callback
		public delegate void WinAmountUpdatedCallback(float newValue);
		public static WinAmountUpdatedCallback WinAmountUpdated;

		// win messages callback
		public delegate void NewWinTypeCallback(Wins newWinName);
		public static NewWinTypeCallback newWinType;

		public delegate void NewGameCallback();
		public static NewGameCallback newGame;

		public CardsManager cardsManager;

		public int cardsToDeal = 0;

		[Header("Info panel")]
		public GameObject infoPanelObj;

		//--------------------------------------------

		// Use this for initialization
		void Start()
		{
			// save script reference in a static field so 
			// we can access the class from anywhere in the project
			MainGame.the = this;

			// set starting bet at 5
			playerBet = 5;
			// notify bet listeners that the bet has changed
			if (BetUpdated != null)
				BetUpdated(playerBet);

			if (StageManager.instance)
				StageManager.OnStateChange += OnStateChange;

			if (Character.instance)
				Character.instance.OnSpawnArrive += OnSpawnArrive;
			//StartCoroutine(StartGame());

			//StartCoroutine(CheckAllIdle());
		}

		public void OnSpawnArrive(Vector3 pos, MONSTER type)
		{
			StartCoroutine(StartGame());
		}

		void OnStateChange(STAGESTATE state)
		{
			switch (state)
			{
				case STAGESTATE.START:
					//StartCoroutine(StartGame());
					break;
			}
		}

		IEnumerator StartGame()
		{
			yield return null;

			/*if (CardsManager.the)
			{
				CardsManager.the.ClearHand();
				yield return new WaitForSeconds(0.5f);
			}*/

			NewGame();
		}

		public void NewGame()
		{
			CardDataManager.instance.SetMyData(CARDDATA.CHIP, 0);
			CardDataManager.instance.SetMyData(CARDDATA.MULT, 0);

			if (newGame != null)
				newGame();

			//if (StageManager.instance.step_Call_Boss == 0)
			DealCards();
		}


		public bool Waiting_User_Input()
		{
			if (gameState != MainGame.STATE_WAIT_HOLD)
				return false;

			if (CardsManager.the.Is_Event_Moving())
				return false;

			return true;
		}

		//--------------------------------------------

		public void Update()
		{
			// flash the win amount if needed
			if (showWinAmount)
				winAmountObj.SetActive(Time.time % 0.5f > 0.25f ? true : false);
		}

		//--------------------------------------------

		// 'direction' can be positive or negative to increase/decrease the bet
		public void ChangeBet(int direction)
		{
			// we are allowed to change the bet only in the IDLE state, between games
			if (gameState == STATE_SHOW_RESULTS)
				ResetGame();
			if (gameState != STATE_IDLE)
				return;

			// update bet to new value using the current bet step
			playerBet += betStep * direction;

			// check bet limits
			if (playerBet > maxBet)
				playerBet = maxBet;
			if (playerBet < minBet)
				playerBet = minBet;

			// notify bet listeners that the bet has changed
			if (BetUpdated != null)
				BetUpdated(playerBet);
		}

		//--------------------------------------------

		public void ChangeCredit(float amount)
		{
			// changing credit not allowed in other states different than IDLE
			if (gameState == STATE_SHOW_RESULTS)
				ResetGame();
			if (gameState != STATE_IDLE)
				return;

			//*********
			// NOTE
			// Instead of adding new credit, you can open a panel with: buy chips/credit/coins, in-app purchase, etc
			//*********

			// credit is updated with the new amount
			playerCredit += amount;

			// notify credit listeners that the credit has changed
			if (CreditUpdated != null)
				CreditUpdated(playerCredit);
		}

		//--------------------------------------------

		// the ResetGame function is clearing the old game: removes cards, hides win meesage, resets the deci
		public void ResetGame(bool must = false)
		{
			if (must == false)
				// we are allowed to reset the game's state only from RESULTS stage
				if (gameState != STATE_SHOW_RESULTS)
					return;

			// go to IDLE state
			gameState = STATE_IDLE;

			// clear the drawn cards, reset the deck and hide wins
			//cardsManager.RestoreCardLibrary();
			cardsManager.ResetDeck();
			cardsManager.ClearHand();
			//Paytable.the.ResetWins();
			// set no current win
			currentWin = 0;
			showWinAmount = false;
			winAmountObj.SetActive(false);

			cardsManager.InitHold();
			cardsManager.InitSelect();
			cardsManager.InitGrowth();
			cardsManager.Count_Num_Select();
		}

		//--------------------------------------------

		public bool MoveCard_Hand()
		{
			if (cardsToDeal > 0)
				return false;

			CheckResults();

			return true;
		}


		public void PlayHand()
		{
			if (gameState == STATE_WAIT_HOLD)
				if (CardDataManager.instance && CardDataManager.instance.Can_Hand())
					if (MoveCard_Hand())
					{
						CardDataManager.instance.Use_Hand();

						/*bool monster_die = Character.instance.Attack_Manual(0);
						if (monster_die)
						{
							//ResetGame();
							MoveCard_Discard();
						}
						else*/
						MoveCard_Discard();
					}
		}

		IEnumerator ResetGame_Delay()
		{
			yield return new WaitForSeconds(1);
			ResetGame();
		}

		public bool PlayDiscard()
		{
			if (CardDataManager.instance && CardDataManager.instance.Can_Hand() == false)
				return false;

			if (CardDataManager.instance && CardDataManager.instance.Can_Discard())
				if (MoveCard_Discard(false, true))
				{
					CardDataManager.instance.Use_Discard();
					return true;
				}

			return false;
		}

		public bool MoveCard_Discard(bool newturn = false, bool discard = false)
		{
			if (cardsToDeal > 0)
				return false;

			gameState = STATE_WAIT_HOLD;

			if (ChargeCreditForBet())
			{
				cardsToDeal = cardsManager.DealCards(newturn, discard);
				/*// special case where all cards were holded, just check results
				if (cardsToDeal == 0)
					CheckResults();*/

				return true;
			}

			return false;
		}

		/*IEnumerator ClearCard()
		{
			//yield return new WaitForSeconds(5);
			yield return new WaitForSeconds(1.5f);

			if (CardDataManager.instance && CardDataManager.instance.Can_Hand())
				NewGame();
			else
			{
				if (StageManager.instance)
					StageManager.instance.StartStage();
			}
		}*/

		public void DealCards()
		{
			// if we are in the results stage, make sure we cleanup the old game
			//if (gameState == STATE_SHOW_RESULTS)
			ResetGame(true);

			// if we need to deal the first hand
			if (gameState == STATE_IDLE)
			{
				// If you want to deal custom cards, uncomment this call
				//int[] nextCards = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
				//cardsManager.SetCardsForNextGame(nextCards);

				// try to substract the credit needed for the deal
				// if successfull, deal the cards
				if (ChargeCreditForBet())
					cardsToDeal = cardsManager.DealCards(true);

				// we receive the number of cards that will be dealt in the 'cardsToDeal' var
				// we use it later to count the cards that are completing the deal animation
				// so that we know when the dealing is finished
			}
			// second hand
			if (gameState == STATE_WAIT_HOLD)
			{
				// Show in the console the cards held by the player
				//cardsManager.PrintHeldCards();

				cardsToDeal = cardsManager.DealCards(false);
				// special case where all cards were holded, just check results
				if (cardsToDeal == 0)
					CheckResults();
			}
		}

		//--------------------------------------------

		bool ChargeCreditForBet()
		{
			// check if we have enough credit left
			if (playerCredit >= playerBet)
			{
				// substract the current bet
				playerCredit -= playerBet;

				Debug.Log("CreditBet : " + playerCredit.ToString());
				// notify credit listeners that the credit has changed
				if (CreditUpdated != null)
					CreditUpdated(playerCredit);
				// returns OK
				return true;
			}
			else
			{
				Debug.Log("CreditBet : No More");
				return false; // no credit, cannot continue
			}
		}

		//--------------------------------------------
		IEnumerator CheckAllIdle()
		{
			while (true)
			{
				if (gameState == STATE_DEALING || gameState == STATE_DEALING2)
					if (CardsManager.the.Is_All_Idle())
					{
						Now_All_Idle_Do_Next();
					}

				yield return null;
			}
		}

		// this function is called everytime a card finished the deal animation
		public void CardAnimationFinished()
		{
			Debug.Log("<color=yellow>ggg = Finished </color> num : " + cardsToDeal.ToString());
			// count a new card that has finished the deal animation
			cardsToDeal--;

			// check if all cards were dealt
			if (cardsToDeal == 0/* || CardsManager.the.Is_All_Idle()*/) //cardsToDeal == 0 만 있었으나 애니메이션이 안끝나는 카드가 있음  //CardsManager.the.Is_All_Idle() 로 우선 체크
			{
				Now_All_Idle_Do_Next();

				//CardDataManager.instance.SetResult();
			}

		}

		void Now_All_Idle_Do_Next()
		{
			CardsManager.the.InitY();
			CardsManager.the.InitScale();

			Debug.Log("<color=yellow>ggg = ColorcardsToDeal </color> num : " + cardsToDeal.ToString());

			if (CardDataManager.instance.Check_GameOver() == false)
			{
				// if we are at the first hand
				if (gameState == STATE_DEALING)
				{
					// wait for the player to hold cards
					gameState = STATE_WAIT_HOLD;
					// make a first evaluation of the hand, so that we can
					// auto-hold the cards that give a sure win
					cardsManager.EvaluateHand();
				}
				else if (gameState == STATE_DEALING2)
				{
					// wait for the player to hold cards
					gameState = STATE_WAIT_HOLD;
					// make a first evaluation of the hand, so that we can
					// auto-hold the cards that give a sure win
					cardsManager.EvaluateHand();
				}
			}
		}

		//--------------------------------------------

		public void CheckResults()
		{
			// change state to RESULTS
			gameState = STATE_SHOW_RESULTS;
			// search for wins
			cardsManager.EvaluateHand();

			// send the new win to win messages listeners (even if it's -1)
			// this triggers the overlay win messages
			if (newWinType != null)
				newWinType(Paytable.the.GetCurrentWinIndex());

			// get current win
			int currWinMultiplier = Paytable.the.GetCurrentWinMultiplier();
			if (currWinMultiplier > 0)
			{
				//CardDataManager.instance.SetResult(true);

				// compute the current win amount
				currentWin = playerBet * currWinMultiplier;
				// add it to the player's credit
				playerCredit += currentWin;
				// notify credit listeners that the credit has changed
				if (CreditUpdated != null)
					CreditUpdated(playerCredit);

				// update win amount text on the screen
				winAmountText.text = "$" + currentWin.ToString("#.00");
				showWinAmount = true;

				// play win sound
				SoundsManager.the.youWinSound.Play();
			}

			//cardsManager.InitHold();
		}

		//--------------------------------------------

		public void OpenInfoPanel(bool on)
		{
			// show/hide info panel
			infoPanelObj.SetActive(on);
		}
	}
}