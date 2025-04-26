using UnityEngine;

public class FinalScript : MonoBehaviour
{
    [SerializeField] PlayerState state;
    void Update()
    {
        if (state.IsDead)
            SceneLoader.LoadScene(0);
    }
}
