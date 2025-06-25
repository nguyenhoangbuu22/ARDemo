using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform healthFill;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material RedMaterial;

    private float maxHealth = 10f;
    private float currentHealth;
    private Vector3 originalScale;

    public Action unbloodyAction;

    void Start()
    {
        originalScale = healthFill.localScale;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
        }
    }

    public void Init(float health)
    {
        currentHealth = health;
        maxHealth = health;
    }

    public void ChangeMaterial(bool isSelect)
    {
        meshRenderer.material = isSelect ? greenMaterial : RedMaterial;
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        float ratio = currentHealth / maxHealth;

        healthFill.DOScaleX(originalScale.x * ratio, 1f);
        float offsetX = (1 - ratio) * originalScale.x * 0.5f;
        healthFill.DOLocalMoveX(-offsetX, 1f).OnComplete(()=>
        {
            if(currentHealth <= 0) unbloodyAction?.Invoke();
        });
    }
}