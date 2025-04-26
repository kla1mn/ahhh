using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MicroSpeacer : MonoBehaviour
{
    private bool isSpeacking;

    private TMP_Text speachesText;

    private AudioSource speachesSource;

    private Animation speachesAnimation;

    public bool IsSpeacking => isSpeacking;

    private void Awake()
    {
        speachesSource = GetComponent<AudioSource>();
        speachesAnimation = GetComponent<Animation>();
        speachesText = GetComponentInChildren<TMP_Text>();
    }

    public void StartSpeach(MicroSpeach speach)
    {
        if (isSpeacking)
            return;

        isSpeacking = true;
        speachesText.text = speach.SpeachText;
        speachesAnimation.Play("TextOpen");

        if (speach.SpeachSound != null)
        {
            speachesSource.clip = speach.SpeachSound;
            speachesSource.Play();
        }

        Invoke("EndSpeach", speach.SpeachTime);
    }


    private void EndSpeach()
    {
        isSpeacking = false;
        speachesAnimation.Play("TextClose");
    }
}

[System.Serializable]
public class MicroSpeach
{
    [TextArea]
    public string SpeachText;
    public AudioClip SpeachSound;
    [Range(.1f, 8f)]public float SpeachTime;
}
