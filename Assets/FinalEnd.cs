using UnityEngine;

public class FinalEnd : MonoBehaviour
{

    [SerializeField] private EnemyHealth h;
    private bool fin;
    // Update is called once per frame
    void Update()
    {
        if (h.IsDead && !fin)
        {
            Invoke(nameof(Lol), 2f);

        }
    }

    private void Lol()
    {
        SceneLoader.LoadScene(3);

    }
}
