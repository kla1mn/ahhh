using UnityEngine;

public class PlayerVoices : MonoBehaviour
{
    [SerializeField] private PlayerState playerState;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] attackSounds;
    [SerializeField] private AudioClip[] painSounds;
    [SerializeField] private AudioClip[] jumpSounds;

    private bool wasAttacking = false;
    private bool wasHearting = false;
    private bool wasJumping = false;
    private const int SoundEffectChance = 15;

    void Update()
    {
        if (playerState.IsAttacking)
        {
            if (!wasAttacking)
            {
                wasAttacking = true;
                PlayRandomAttackSound();
            }
        }
        else
        {
            wasAttacking = false;
        }

        if (playerState.IsHearting)
        {
            if (!wasHearting)
            {
                wasHearting = true;
                PlayRandomPainEffect();
            }
        }
        else
        {
            wasHearting = false;
        }

        if (playerState.IsJumping)
        {
            if (!wasJumping)
            {
                wasJumping = true;
                PlayRandomJumpEffect();
            }
        }
        else
        {
            wasJumping = false;
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

    private void PlayRandomJumpEffect()
    {
        PlayRandomEffect(jumpSounds);
    }

    private void PlayRandomEffect(AudioClip[] clips)
    {
        int randomValue = UnityEngine.Random.Range(0, 100); // Случайное число от 0 до 99

        if (randomValue < SoundEffectChance)
        {
            if (clips != null && clips.Length > 0 && audioSource != null)
            {
                if (!audioSource.isPlaying) // Проверка чтобы не наслаивать звуки
                {
                    int randomIndex = UnityEngine.Random.Range(0, clips.Length);
                    AudioClip randomClip = clips[randomIndex];
                    audioSource.PlayOneShot(randomClip);
                }
            }
        }
    }
}
