using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	/*public AudioClip hit;
    public AudioClip attack_1;
    public AudioClip attack_2;
    public AudioClip attack_3;
    public AudioClip skill_1;
    public AudioClip skill_2;
    public AudioClip skill_3;
    public AudioClip skill_4;
    public AudioClip skill_5;
    public AudioClip skill_6;*/

	public float bgm_base_volume = 1;

	private Dictionary<string, AudioClip> dict_Sound;

	public static SoundManager instance;

	private List<AudioSource> audioSourcePool = new List<AudioSource>();
	private int poolSize = 10; // 필요한 풀 크기 설정
	private GameObject audioSourceContainer;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		// Initialize the dictionary
		dict_Sound = new Dictionary<string, AudioClip>();

		// Load all .wav files from the Resources/Sound folder
		AudioClip[] audioClips = Resources.LoadAll<AudioClip>("Sound");

		foreach (AudioClip clip in audioClips)
		{
			if (clip != null)
			{
				// Use the clip name as the key
				dict_Sound[clip.name] = clip;
			}
		}

		// AudioSource를 보관할 컨테이너 오브젝트 생성
		audioSourceContainer = new GameObject("AudioSourceContainer");
		DontDestroyOnLoad(audioSourceContainer);

		// AudioSource 풀 초기화
		for (int i = 0; i < poolSize; i++)
		{
			AudioSource source = audioSourceContainer.AddComponent<AudioSource>();
			source.playOnAwake = false;
			source.loop = false;
			audioSourcePool.Add(source);
		}
	}

	public void PlaySound(string _snd)
	{
		// 숫자 스트링(_snd)이 "11"과 같은 경우 처리
		if (int.TryParse(_snd, out _))
		{
			string matchingKey = dict_Sound.Keys.FirstOrDefault(key => key.StartsWith(_snd + "_"));
			if (string.IsNullOrEmpty(matchingKey))
			{
				Debug.LogWarning($"No sound found starting with '{_snd}_'.");
				return;
			}
			_snd = matchingKey;
		}

		if (!dict_Sound.ContainsKey(_snd))
		{
			Debug.LogWarning($"Sound key '{_snd}' not found in dictionary.");
			return;
		}

		AudioClip clipToPlay = dict_Sound[_snd];

		// 사용 가능한 AudioSource 가져오기
		AudioSource source = GetAvailableAudioSource();
		if (source == null)
		{
			Debug.LogWarning("No available AudioSource in pool.");
			return;
		}

		// AudioSource 설정 및 재생
		source.clip = clipToPlay;
		source.volume = bgm_base_volume;
		source.Play();

		// 사운드 재생이 끝난 후 AudioSource 초기화
		StartCoroutine(ReturnToPoolAfterPlay(source, clipToPlay.length));
	}

	private AudioSource GetAvailableAudioSource()
	{
		// 사용 중이지 않은 AudioSource 검색
		foreach (AudioSource source in audioSourcePool)
		{
			if (!source.isPlaying)
			{
				return source;
			}
		}

		return null; // 사용 가능한 AudioSource가 없는 경우
	}

	private System.Collections.IEnumerator ReturnToPoolAfterPlay(AudioSource source, float delay)
	{
		yield return new WaitForSeconds(delay);
		source.clip = null; // 클립 초기화
	}
}
