using UnityEngine;

public class GODMODE : MonoBehaviour
{
    [SerializeField] PlayerAttackSistem PlayerAttackSistem;
    [SerializeField] PlayerHealth PlayerHealth;

    [SerializeField] private bool isGodMod;

    [Header("Values")]
    [SerializeField] private float dmg;

    private void Start()
    {
        if (!isGodMod)
            return;

        PlayerAttackSistem.CurrentDamage = dmg;
        PlayerHealth.MaxHealth = 2000;
        PlayerHealth.CurrentHealth = 2000;
    }
}
