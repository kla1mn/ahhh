using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMaskRend : MonoBehaviour
{
    private SpriteMask mask;
    private SpriteRenderer rend;

    private void Awake()
    {
        mask = GetComponent<SpriteMask>();
        rend = GetComponent<SpriteRenderer>();
        mask.sprite = rend.sprite;
    }
}
