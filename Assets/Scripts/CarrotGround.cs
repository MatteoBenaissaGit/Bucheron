using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

public class CarrotGround : Destructible, IWaterable
{
    [SerializeField] private Carrot _carrotPrefab;
    [SerializeField] private List<Sprite> _treeSprites;
    [SerializeField] private ParticleSystem _waterParticle;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _treeSprites[Life];
    }

    public override void TakeDamage()
    {
        
    }
    
    public void Water()
    {
        Life++;
        if (Life >= 2)
        {
            Instantiate(_carrotPrefab, transform.position, Quaternion.identity);
            _spriteRenderer.sprite = _treeSprites[0];
            Life = 0;
        }
        _spriteRenderer.sprite = _treeSprites[Life];

        //anim
        _waterParticle.Play();
        transform.DOPunchScale(Vector3.one * 0.25f, 0.25f);
    }
}