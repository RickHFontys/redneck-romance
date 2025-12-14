using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private AudioSource endingMusicAudioSource;
    [SerializeField] private UnityEngine.UI.Image endingImage;

    [SerializeField] private AudioClip[] endingMusic;
    [SerializeField] private Sprite[] endingImages;

    private void Start()
    {
        if (GameManager.Instance.GameEnding == GameEnding.good)
        {
            endingImage.sprite = endingImages[0];
            endingMusicAudioSource.volume = 0.3f;
            PlayMusic(endingMusic[0]);
        }
        else if (GameManager.Instance.GameEnding == GameEnding.bad)
        {
            endingImage.sprite = endingImages[1];
            endingMusicAudioSource.volume = 1;
            PlayMusic(endingMusic[1]);
        }
        else
        {
            Debug.LogWarning("Ending was not specified!!!");
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        endingMusicAudioSource.clip = clip;
        endingMusicAudioSource.loop = true;
        endingMusicAudioSource.Play();
    }
}
