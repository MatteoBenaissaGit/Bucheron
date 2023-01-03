using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

[RequireComponent(typeof(SpriteView))]
[RequireComponent(typeof(SpriteRenderer))]
public class Skeleton : Destructible, IWaterable
{
    [SerializeField] private Sprite _deathSprite;
    [SerializeField] private ParticleSystem _waterParticle;
    private SpriteView _spriteView;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteView = GetComponent<SpriteView>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void TakeDamage()
    {
        if (Life > 0)
        {
            _spriteView.PlayAction("Damage");
        }
        base.TakeDamage();
    }

    public override void Die()
    {
        Life = -1;
        _spriteView.PlayAction("Die");
        _spriteView.OnActionEnd.AddListener(DestroyObject);
        base.Die();
    }

    private void DestroyObject()
    {
        _spriteView.PlayState("Dead");
        _spriteView.OnActionEnd.RemoveListener(DestroyObject);
    }

    public void Water()
    {
        if (Life > 0) return;
        _spriteView.PlayState("Idle");
        _spriteView.PlayAction("Spawn");
        _waterParticle.Play();
        Life = 3;
    }
}
