using UnityEngine;

public class PlayerTypeSwitcher : MonoBehaviour
{
    public PlayerState State;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad7))
            State.RecreatePlayer(PlayerState.ControllerType.OnlyMove);

        if (Input.GetKeyDown(KeyCode.Keypad8))
            State.RecreatePlayer(PlayerState.ControllerType.WithoutAttack);

        if (Input.GetKeyDown(KeyCode.Keypad9))
            State.RecreatePlayer(PlayerState.ControllerType.FullController);

    }
}
