using UnityEngine;

public class PlayerVoices : MonoBehaviour
{
    [SerializeField] private PlayerState playerState;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;
    
    void Update()
    {
        if (playerState.IsAttacking)
        {
            PlayRandomAttackSound();
        }
    }
    
    private void PlayRandomAttackSound()
    {
        const float playChance = 1 / 3f;
        var randomValue = Random.value;

        if (true)// if (randomValue < playChance)
        {
            if (audioClips != null && audioClips.Length > 0)
            {
                if (!audioSource.isPlaying) // Проверка
                {
                    var randomIndex = UnityEngine.Random.Range(0, audioClips.Length);
                    var randomClip = audioClips[randomIndex];
                    audioSource.PlayOneShot(randomClip);
                }
            }
        }
    }
}
