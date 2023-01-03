using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private GameObject _stack;
    [SerializeField] private TextMeshProUGUI _stackTextMesh;
    [SerializeField] private Sprite _iconSprite;
    
    private float _stackSize;

    private void Awake()
    {
        _stack.SetActive(false);
        _stackTextMesh.text = "0";
        _iconImage.sprite = _iconSprite;
        _iconImage.gameObject.SetActive(false);
    }

    public void SetStack(int size)
    {
        _stackSize += size;
        _stack.SetActive(_stackSize > 1);
        _iconImage.gameObject.SetActive(_stackSize > 0);
        
        //min / max
        if (_stackSize < 0) _stackSize = 0;
        if (_stackSize > 99) _stackSize = 99;
        
        //animation
        _stackTextMesh.text = _stackSize.ToString();
        _iconImage.transform.DOComplete();
        _iconImage.transform.DOPunchScale(Vector3.one * 0.25f, 0.5f);
        _iconImage.transform.DORotate(new Vector3(0, 0, 360), 0.5f).SetEase(Ease.OutFlash);
    }
}
