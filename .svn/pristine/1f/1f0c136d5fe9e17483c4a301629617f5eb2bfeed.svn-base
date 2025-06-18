using UnityEngine;

namespace VideoPokerKit
{
	//--------------------------------------------


	// card suits
	public enum CardType
	{
		TYPE_SPADES,
		TYPE_DIAMONDS,
		TYPE_HEARTS,
		TYPE_CLUBS,
		TYPES_NO
	}

	//--------------------------------------------

	// card values
	public enum CardValue
	{
		VALUE_2,
		VALUE_3,
		VALUE_4,
		VALUE_5,
		VALUE_6,
		VALUE_7,
		VALUE_8,
		VALUE_9,
		VALUE_10,
		VALUE_J,
		VALUE_Q,
		VALUE_K,
		VALUE_A,
		VALUES_NO
	}

	//--------------------------------------------

	[System.Serializable]
	public class CardData
	{
		// card suit and value (set in the Inspector)
		public CardType type = CardType.TYPE_HEARTS;
		public CardValue value = CardValue.VALUE_2;
		public float chip_growth = 0;
		public float mult_growth = 0;
		public bool scoring;
		public bool include;
		// image
		public Sprite sprite;

		// if was dealt or not from the deck (used only for library cards)
		[HideInInspector]
		public bool dealt = false;
		[HideInInspector]
		public bool hold = false;
		[HideInInspector]
		public bool is_Select = false;

		//----------------------------------

		// used to make a copy of another card
		public void CopyInfoFrom(CardData other)
		{
			type = other.type;
			value = other.value;
			sprite = other.sprite;
			hold = other.hold;
			is_Select = other.is_Select;
		}

		public void ChangeType(CardType newtype, CardValue _value = CardValue.VALUES_NO)
		{
			if (_value != CardValue.VALUES_NO)
				value = _value;

			type = newtype;

			UpdateSprite();
		}

		public void ChangeValue(CardValue _value)
		{
			value = _value;

			UpdateSprite();
		}

		public void UpdateSprite()
		{
			sprite = CardsManager.the.sprite_AllCard[(int)value * 4 + (int)type];
		}
	}

	//--------------------------------------------
}