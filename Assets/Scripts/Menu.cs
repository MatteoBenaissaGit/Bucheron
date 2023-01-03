using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _escapeMenu;
    
    private void Start()
    {
        _escapeMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            _escapeMenu.SetActive(_escapeMenu.activeInHierarchy == false);
        }
    }
}
