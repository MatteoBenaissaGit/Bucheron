using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WaterUI : MonoBehaviour
{
    [SerializeField] private Transform _e;
    [SerializeField] private Transform _water;
    [SerializeField] private GameObject _selected;
    public bool _isSelected = false;

    private void Update()
    {
        
    }

    public void Select()
    {
        _e.DOPunchScale(Vector3.one * 0.25f, 0.3f);
        _selected.SetActive(true);
        _selected.transform.localScale = Vector3.zero;
        _selected.transform.DOScale(Vector3.one, 0.3f);
        _water.DOPunchRotation(new Vector3(0, 0, 30f), 0.4f);
    }
    
    public void Unselect()
    {
        _e.DOPunchScale(Vector3.one * 0.25f, 0.3f);
        _selected.SetActive(false);
    }
}
