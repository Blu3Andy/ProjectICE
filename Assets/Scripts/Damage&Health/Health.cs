using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private UnityEvent hit;
    [SerializeField] private UnityEvent death;

    [SerializeField] private UnityEngine.UI.Image healthBar;

    [SerializeField] private float health = 100f;

    [SerializeField] private float invicibleTime = 1f;

    private float invicibleTimer;

    private float maxHealth;

    [SerializeField] private bool destoryOnDeath = false;

    private bool invicible = false;

    private void Awake()
    {
        maxHealth = health;
    }

    private void Update()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth;
        }

        if (health > maxHealth)
        {
            HealMax();
        }

        if (health <= 0)
        {
            Death();
        }

        if (invicibleTimer > 0)
        {
            invicibleTimer -= Time.deltaTime;
            invicible = true;
        }
        else
        {
            invicible = false;
        }
    }

    public void Heal(float healAmount)
    {
        health += healAmount;
    }

    public void HealMax()
    {
        health = maxHealth;
    }

    public void TakeDamage(float dmg)
    {
        if (invicible) return;
        health -= dmg;

        invicibleTimer = invicibleTime;
        hit.Invoke();
    }

    private void Death()
    {
        death.Invoke();
        if (destoryOnDeath) Destroy(gameObject);
    }

    
}