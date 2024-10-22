using System.Collections.Generic;
using FL;
using UnityEngine;

public class NoiseEnemy : MonoBehaviour
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
                audioSource.spatialBlend = 0;
                audioSource.loop = false;
                audioSource.Play();
                break;
            case SoundEnemyType.Moving:
                audioSource.clip = movingClips[Random.Range(0, burstClips.Count)];
                audioSource.loop = true;
                audioSource.Play();
                break;
            case SoundEnemyType.Delayd:
                break;
            case SoundEnemyType.SlowRelease:
                break;
            default:
                break;
        }
    }

    #endregion
}
