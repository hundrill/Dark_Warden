using UnityEngine;

public class OptionManager : MonoBehaviour
{
	public static OptionManager instance;

	public float Speed_Time;
	int _Speed;
	public int Speed
	{
		get { return _Speed; }
		set
		{
			_Speed = value;
			Speed_Time = 1.1f - (float)(_Speed * 0.25f);
		}
	}

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);


		Speed = 1;
	}
}
