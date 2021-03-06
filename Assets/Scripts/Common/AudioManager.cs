using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance => instance;
    
    public AudioSource OSTAudioSource;
    public AudioSource SoundAudioSource;
    public AudioSource RatAudioSource;
    public AudioClip[] OSTClips;
    public AudioClip Rats;
    public AudioClip[] ShopEnterClips;
    public AudioClip[] ShopLeaveClips;
    
    void Awake()
    {
        // Handle singleton instantiation
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (!OSTAudioSource.isPlaying)
        {
            AudioClip clipToPlay = OSTClips.Shuffle().First(clip => clip != OSTAudioSource.clip);
            OSTAudioSource.clip = clipToPlay;
            OSTAudioSource.Play();
        }
    }

    public void PlayShopEnterClip()
    {
        SoundAudioSource.PlayOneShot(ShopEnterClips.Shuffle().First());
    }
    
    public void PlayRat()
    {
        RatAudioSource.PlayOneShot(Rats);
    }
    
    public void StopRat()
    {
        RatAudioSource.Stop();
    }
    
    public void PlayShopLeaveClip()
    {
        SoundAudioSource.PlayOneShot(ShopLeaveClips.Shuffle().First());
    }
}
