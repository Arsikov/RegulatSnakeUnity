using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] ChangeMoveDirClips;
    public AudioClip[] EatFoodClips;
    public AudioClip GameOverSound;

    [SerializeField] private AudioSource ClipPlayer;
    [SerializeField] private AudioSource ChangeMoveDirClipsPlayer;

    private void Start()
    {
        GameEvents._GameEvents.OnEatFood += PlayEatSound;
        GameEvents._GameEvents.OnGameOver += PlayGameOverSound;
        GameEvents._GameEvents.OnSnakeChangeMoveDir += PlayChangeMoveDirClip;
    }

    private void PlayChangeMoveDirClip()
    {
        ChangeMoveDirClipsPlayer.PlayOneShot(ChangeMoveDirClips[Random.Range(0, ChangeMoveDirClips.Length)]);
    }

    private void PlayEatSound()
    {
        ClipPlayer.PlayOneShot(EatFoodClips[Random.Range(0, EatFoodClips.Length)]);
    }

    private void PlayGameOverSound()
    {
        ClipPlayer.PlayOneShot(GameOverSound);
    }
}
