using System;
using UnityEngine;
using ExtensionMethods;

public class TileAudio : MonoBehaviour
{
    private static AudioSource audioSource;

    private readonly AudioClip swipeAudio      = GetAudio("Swipe");
    private readonly AudioClip ironAudio       = GetAudio("Iron");
    private readonly AudioClip chipAudio       = GetAudio("Chip");
    private readonly AudioClip brickBreakAudio = GetAudio("Break");
    private readonly AudioClip obstructedAudio = GetAudio("Obstruct");
    private const string dir = "Audio";

    public void Start() => audioSource = this.Get<AudioSource>();

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public void PlaySwipe()    => PlayClip(swipeAudio);
    public void PlayIron()     => PlayClip(ironAudio);
    public void PlayChip()     => PlayClip(chipAudio);
    public void PlayBreak()    => PlayClip(brickBreakAudio);
    public void PlayObstruct() => PlayClip(obstructedAudio);

    private void PlayClip(AudioClip clip)
    {
        if (audioSource == null) audioSource = new AudioSource();

        audioSource.clip = clip;
        audioSource.Play();
    }
    private static AudioClip GetAudio(string name) => null;
    //private static AudioClip GetAudio(string name) => Resources.Load<AudioClip>($"{dir}/{name}");
}
