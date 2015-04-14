using UnityEngine;

/// <summary>
/// 背景音乐管理器
/// </summary>
public class MusicManager : MonoBehaviour
{
    /// <summary>
    /// 先播放1遍0，再单曲循环1。
    /// </summary>
    public AudioClip Music0, MusicBoss;

    void Awake()
    {
        //Invoke("StartPlayMusic", 0.02f);
        //AudioSource.PlayClipAtPoint(Music0, transform.position);
        //audio.clip = Music1;
        //audio.PlayDelayed(Music0.length);
        //Invoke("StartLoopMusic1", Music0.length);
    }

    void Start()
    {
        //GameManager.Instance.DidBossCome += OnBossCome;
        //GameManager.Instance.DidBossDie += OnAllBossDie;
        audio.clip = Music0;
        audio.loop = true;
        audio.volume = 0.001f;
        audio.Play();
        //TweenVolume.Begin(gameObject, 1f, 1f).delay = 0.5f;
    }

    void OnBossCome()
    {
        //TweenVolume.Begin(gameObject, 0.2f, 0f);
        Invoke("PlayMusicBoss", 0.2f);
    }
    void PlayMusicBoss()
    {
        audio.clip = MusicBoss;
        audio.Play();
        //TweenVolume.Begin(gameObject, 0.2f, 1f);
    }

    void OnAllBossDie()
    {
        //TweenVolume.Begin(gameObject, 0.2f, 0f);
        Invoke("PlayMusic0", 0.2f);
    }
    void PlayMusic0()
    {
        audio.clip = Music0;
        audio.Play();
        //TweenVolume.Begin(gameObject, 0.2f, 1f);
    }
}