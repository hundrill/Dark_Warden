using System;
using UnityEngine;

namespace VideoPokerKit
{
    // all wins, listed from lower to bigger
    public enum Wins
    {
        WIN_HIGH_CARD,
		WIN_ONE_PAIR,
        WIN_TWO_PAIR,
        WIN_THREE_OF_A_KIND,
        WIN_STRAIGHT,
        WIN_FLUSH,
        WIN_FULL_HOUSE,
        WIN_FOUR_OF_A_KIND,
        WIN_STRAIGHT_FLUSH,
        WINS_NO
    };

    //-------------------------------------------------------------------

    public class Paytable : MonoBehaviour
    {
        // static reference to the object instance (set in Awake)
        public static Paytable the;

        // set this in the Inspector to the desired value
        public bool flashWinnerLabel = true;

        // set this in the Inspector to the desired value
        public bool appendMultiplierToName = true;

        // please assign this array in the inspector
        // using the order of Wins enum above
        [Header("Prize labels")]
        public TextMesh[] entries;

        // the index of the current win
        int currentWinIndex = -1;

        // wins names
        string[] prizeTitle = new string[] {
                                "High Card",
								"One Pairs",
                                "Two Pairs",
                                "Three of a Kind",
                                "Straight",
                                "Flush",
                                "Full House",
                                "Four of a Kind",
                                "Straight Flush"
                                };
        // multipliers for each win
        int[] prizeMultipliers = new int[] {
                                1,
                                2,
                                2,
                                3,
                                4,
                                4,
                                5,
                                7,
                                8,
        };

		int[] baseChips = new int[] {
								5,  //highcard
								10, //onepair
								20, //twopair
								30, //triple
								30, //straight
								35, //flush
								50, //fullhouse
								60, //fourofakind
								100, //straightflush                              
        };

		int[] baseLevel = new int[] {
								0,  //highcard
								0, //onepair
								0, //twopair
								0, //triple
								0, //straight
								0, //flush
								0, //fullhouse
								0, //fourofakind
								0, //straightflush                              
        };

		int[] baseCount = new int[] {
								0,  //highcard
								0, //onepair
								0, //twopair
								0, //triple
								0, //straight
								0, //flush
								0, //fullhouse
								0, //fourofakind
								0, //straightflush                              
        };

		//-------------------------------------

		// Use this for initialization
		void Awake()
        {
            Paytable.the = this;
            Init();
        }

        //-------------------------------------

        // Update is called once per frame
        void Update()
        {
            return; //rsh_temp

            // check if we have a win
            if (currentWinIndex >= 0)
            {
                // flash the current win 2 times per second
                if (flashWinnerLabel)
                    entries[currentWinIndex].gameObject.SetActive(Time.time % 0.5f > 0.25f ? true : false);
            }
        }

        //-------------------------------------

        public int[] GetMultipliers()
        {
            // returns the vector with all the multipliers
            return prizeMultipliers;
        }

        public int GetLevel(Wins type)
        {
            return baseLevel[(int)type];
		}
        //-------------------------------------

        public void Init()
        {
            // setup visual paytable
            for (int i = 0; i < entries.Length; i++)
            {
                // set prize name
                entries[i].text = prizeTitle[i];
                // append the multiplier with a different colour
                if (appendMultiplierToName)
                    entries[i].text += "  <color=white>" + prizeMultipliers[i] + "X</color>";
            }
        }

        //-------------------------------------

        public void SetCurrentWin(Wins currWin)
        {
            currentWinIndex = (int)currWin;
        }

        //-------------------------------------

        public Wins GetCurrentWinIndex()
        {
            return (Wins)currentWinIndex;
        }

        //-------------------------------------

        public void EnhanceRunInfo(PLANET type)
        {
			Paytable.the.EnhanceMultiplier(Wins.WIN_ONE_PAIR, 3);
			Paytable.the.EnhanceChips(Wins.WIN_ONE_PAIR, 3);
		}

        public void EnhanceLevel(Wins type)
        {
			baseLevel[(int)type]++;
		}

		public void EnhanceCount(Wins type)
		{
			baseCount[(int)type]++;
		}

		public void EnhanceMultiplier(Wins type , int plus_mult)
        {
            int idx = (int)type;

			if (idx < prizeMultipliers.Length && idx >= 0)
				prizeMultipliers[idx] += plus_mult;
		}

		public void EnhanceChips(Wins type, int plus_chip)
		{
			int idx = (int)type;

			if (idx < baseChips.Length && idx >= 0)
				baseChips[idx] += plus_chip;
		}

		public int GetCurrentWinMultiplier()
        {
            if (currentWinIndex >= 0)
                return prizeMultipliers[currentWinIndex];
            return 0;
        }

		public int GetCurrentChips()
		{
			if (currentWinIndex >= 0)
				return baseChips[currentWinIndex];
			return 0;
		}

		public int GetWinMultiplier(Wins type)
		{
			int index = (int)type;

			if (index >= 0)
				return prizeMultipliers[index];

			return 0;
		}

		public int GetChips(Wins type)
		{
			int index = (int)type;

			if (index >= 0)
				return baseChips[index];

			return 0;
		}

		public int GetCount(Wins type)
		{
			int index = (int)type;

			if (index >= 0)
				return baseCount[index];

			return 0;
		}

		//-------------------------------------

		public void ResetWins()
        {
            currentWinIndex = -1;
            return; //rsh_temp

            // because the win is flashing, we have to
            // make sure we turn ON the latest win object
            if (currentWinIndex >= 0)
                entries[currentWinIndex].gameObject.SetActive(true);
            // set no current win index
            currentWinIndex = -1;
        }

        //-------------------------------------
    }
}