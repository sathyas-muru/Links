using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [Header("Wrong")]
    [SerializeField] AudioClip wrong;
    [SerializeField][Range(0f, 1f)] float Volume = 1f;

    [Header("Dealing")]
    [SerializeField] AudioClip Dealing;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayWrongCard()
    {
        if (wrong != null)
        {
            AudioSource.PlayClipAtPoint(wrong, Camera.main.transform.position, Volume);
        }
    }

    public void StartDealingSound()
    {
        if (Dealing != null)
        {
            audioSource.clip = Dealing;
            audioSource.loop = true;
            audioSource.volume = Volume;
            audioSource.Play();
        }
    }

    public void StopDealingSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
