using UnityEngine;

/// <summary>
/// 音频全在我上面
/// </summary>
public class AudioHolder : MonoBehaviour
{
    public static AudioHolder Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public AudioClip[] EnemyBowAttack;
    public AudioClip[] HeroDead;
    public AudioClip[] ButtonPush;
    public AudioClip[] Fire;
    public AudioClip[] HeroUseItem;
    public AudioClip[] Water;
    public AudioClip[] HeroAttack;
    public AudioClip[] EnemyAttack;
    public AudioClip[] EnemyAttackHit;

    public AudioClip[] SoundThunderAppear;
    public AudioClip[] SoundThunderAtk;

    public static void PlayAudio(AudioClip[] audioClips)
    {
        if (audioClips == null || audioClips.Length == 0) return;
        var audioClip = audioClips[Random.Range(0, audioClips.Length)];
//        NGUITools.PlaySound(audioClip);
    }
}