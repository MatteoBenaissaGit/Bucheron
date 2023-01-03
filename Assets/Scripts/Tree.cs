using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using DG.Tweening;
using Random = System.Random;

public class Tree : Destructible, IWaterable
{
    [SerializeField] private Wood _woodPrefab;
    [SerializeField] private SpriteRenderer _topTreePrefab;
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
        transform.DOPunchScale(Vector3.one * 0.25f, 0.25f);

        if (Life > 0)
        {
            DropWood();
            _spriteRenderer.sprite = _treeSprites[Life - 1];
        }
        if (Life == 3)
        {
            DropTop();
        }
        
        base.TakeDamage();
    }

    private void DropTop()
    {
        SpriteRenderer top = Instantiate(_topTreePrefab, transform.position + new Vector3(0,0.5f,0), Quaternion.identity);
        const float animTime = 1.5f;
        
        Ease ease = Ease.InOutCubic;
        top.transform.DOScale(Vector3.one * 0.5f, animTime).SetEase(ease);
        top.DOFade(0, animTime).SetEase(ease);
        top.transform.DORotate(new Vector3(0,0,100), animTime).SetEase(ease);

        Random random = new Random();
        double randomNumberX = random.NextDouble();
        double randomNumberY = random.NextDouble();
        top.GetComponent<Rigidbody2D>().velocity = new Vector2(randomNumberX < 0.5f ? -2 : 2,randomNumberY < 0.5f ? 5.5f : 6.5f);
    }

    private void DropWood()
    {
        if (_woodPrefab != null)
        {
            Wood wood = Instantiate(_woodPrefab, transform.position, Quaternion.identity);
        }
    }

    public override void Die()
    {
        base.Die();
    }

    public void Water()
    {
        if (Life >= 3)
        {
            return;
        }

        Life++;
        if (Life >= 0)
        {
            _spriteRenderer.sprite = _treeSprites[Life];
        }
        
        //anim
        _waterParticle.Play();
        transform.DOPunchScale(Vector3.one * 0.25f, 0.25f);
    }
}
