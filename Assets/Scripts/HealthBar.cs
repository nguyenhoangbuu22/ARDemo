using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform healthFill;
    private float maxHealth = 10f;
    private float currentHealth;
    private Vector3 originalScale;

    void Start()
    {
        currentHealth = maxHealth;
        originalScale = healthFill.localScale;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        float ratio = currentHealth / maxHealth;

        healthFill.DOScaleX(originalScale.x * ratio, 1f);
        float offsetX = (1 - ratio) * originalScale.x * 0.5f;
        healthFill.DOLocalMoveX(-offsetX, 1f);
    }
}