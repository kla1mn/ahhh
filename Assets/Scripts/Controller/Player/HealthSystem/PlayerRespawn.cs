using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private int sceneLoad;

    [SerializeField] private PlayerState state;

    private void Update()
    {
        if (state.IsDead)
            SceneLoader.LoadScene(sceneLoad);
    }
}
