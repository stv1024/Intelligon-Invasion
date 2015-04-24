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
        GetComponent<AudioSource>().clip = Music0;
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().volume = 0.001f;
        GetComponent<AudioSource>().Play();
        //TweenVolume.Begin(gameObject, 1f, 1f).delay = 0.5f;
    }

    void OnBossCome()
    {
        //TweenVolume.Begin(gameObject, 0.2f, 0f);
        Invoke("PlayMusicBoss", 0.2f);
    }
    void PlayMusicBoss()
    {
        GetComponent<AudioSource>().clip = MusicBoss;
        GetComponent<AudioSource>().Play();
        //TweenVolume.Begin(gameObject, 0.2f, 1f);
    }

    void OnAllBossDie()
    {
        //TweenVolume.Begin(gameObject, 0.2f, 0f);
        Invoke("PlayMusic0", 0.2f);
    }
    void PlayMusic0()
    {
        GetComponent<AudioSource>().clip = Music0;
        GetComponent<AudioSource>().Play();
        //TweenVolume.Begin(gameObject, 0.2f, 1f);
    }
}