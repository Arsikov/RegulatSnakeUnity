using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip GetMoreTenScore;
    public AudioClip EatFood;
    public AudioClip DeathSound;

    static AudioSource audioSourse;

    private void Start()
    {
        audioSourse = GetComponent<AudioSource>();
    }

    public void PlayEatFoodSound()
    {
        audioSourse.PlayOneShot(EatFood);
    }

    public void PlayerGetsMoreTenScore()
    {
        audioSourse.PlayOneShot(GetMoreTenScore);
    }

    public void PlayerDead()
    {
        audioSourse.PlayOneShot(DeathSound);
    }

}
