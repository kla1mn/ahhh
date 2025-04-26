using UnityEngine;

public class PlayerVoices : MonoBehaviour
{
    [SerializeField] private PlayerState playerState;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] attackSounds;
    [SerializeField] private AudioClip[] painSounds;
    
    void Update()
    {
        if (playerState.IsAttacking)
        {
            PlayRandomAttackSound();
        }

        if (playerState.IsHearting)
        {
            PlayRandomPainEffect();
        }
    }
    
    private void PlayRandomAttackSound()
    {
        PlayRandomEffect(attackSounds);
    }
    
    private void PlayRandomPainEffect()
    {
        PlayRandomEffect(painSounds);
    }

    private void PlayRandomEffect(AudioClip[] clips)
    {
        const float playChance = 1 / 3f;
        var randomValue = Random.value;

        if (true)// if (randomValue < playChance)
        {
            if (clips != null && clips.Length > 0)
            {
                if (!audioSource.isPlaying) // Проверка
                {
                    var randomIndex = UnityEngine.Random.Range(0, clips.Length);
                    var randomClip = clips[randomIndex];
                    audioSource.PlayOneShot(randomClip);
                }
            }
        }
    }
}
