using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    private static BackGroundMusic instance;
    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();

        if (instance != null && instance.source.clip != source.clip)
        {
            Destroy(instance.source);
            instance = null;
        }

        if (instance == null)
        {
            instance = this;
            gameObject.transform.parent = null;
            DontDestroyOnLoad(gameObject); // Не уничтожать при загрузке
        }
        else
        {
            Destroy(gameObject); // Уничтожить дубликат
        }
    }
}
