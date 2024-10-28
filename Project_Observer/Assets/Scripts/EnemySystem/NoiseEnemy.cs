using System.Collections.Generic;
using System.Threading.Tasks;
using FL;
using UnityEngine;

public class NoiseEnemy : BaseEnemy
{
    #region Variables

    [Header("Enemy Data"), SerializeField]
    SoundEnemyType soundEnemyType = SoundEnemyType.Burst;

    public float moveSpeed = 3f;

    [Header("Audio Data"), SerializeField]
    AudioSource audioSource;

    [SerializeField]
    List<AudioClip> burstClips = new List<AudioClip>();

    [SerializeField]
    List<AudioClip> movingClips = new List<AudioClip>();

    #endregion

    #region Start Functions

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        PlayEnemySound();
    }

    private void Start()
    {
        DamageSanity();
    }

    #endregion

    #region Update Functions

    private void Update()
    {
        if (soundEnemyType == SoundEnemyType.Moving)
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    #endregion

    #region Base Functions

    void PlayEnemySound()
    {
        switch (soundEnemyType)
        {
            case SoundEnemyType.Burst:
                audioSource.minDistance = 2f;
                audioSource.maxDistance = 3.5f;

                audioSource.clip = burstClips[Random.Range(0, burstClips.Count)];

                audioSource.spatialBlend = 1;
                audioSource.loop = false;
                audioSource.Play();
                // DestroySoundAfterDelay(2);
                break;
            case SoundEnemyType.Moving:
                audioSource.minDistance = 2f;
                audioSource.maxDistance = 3.5f;

                audioSource.clip = movingClips[Random.Range(0, movingClips.Count)];

                audioSource.loop = true;
                audioSource.Play();
                DestroySoundAfterDelay(6);
                break;
            case SoundEnemyType.Delayd:
                break;
            case SoundEnemyType.SlowRelease:
                break;
            default:
                break;
        }
    }

    async void DestroySoundAfterDelay(int delay)
    {
        await Task.Delay(delay * 1000);
        Destroy(gameObject);
    }

    #endregion
}
