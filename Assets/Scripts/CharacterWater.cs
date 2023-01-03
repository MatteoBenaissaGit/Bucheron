using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

public class CharacterWater : MonoBehaviour
{
    [SerializeField, Range(0, 3)] private float _cursorDistance;
    [SerializeField] private GameObject _waterAim;
    [SerializeField] private WaterUI _waterUI;
    [ReadOnly] public bool IsWatering;

    private void Start()
    {
        Cursor.visible = false;
        _waterAim.SetActive(false);
    }

    private void Update()
    {
        PlaceCursorToMouse();
        HandleInput();
        Water();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetWaterMode();
        }
    }

    private void SetWaterMode()
    {
        if (IsWatering)
        {
            _waterUI.Unselect();
            _waterAim.SetActive(false);
        }

        if (IsWatering == false)
        {
            _waterUI.Select();
            _waterAim.SetActive(true);
            _waterAim.transform.localScale = Vector3.zero;
            _waterAim.transform.DOScale(Vector3.one, 0.3f);
        }

        IsWatering = IsWatering == false;
    }

    private void PlaceCursorToMouse()
    {
        if (_waterAim != null)
        {
            _waterAim.transform.position = GetCursorPosition();
        }
    }

    private Vector2 GetCursorPosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10));
        //cursor position
        Vector2 cursorPosition = mouseWorldPosition;
        return cursorPosition;
    }

    private void Water()
    {
        if (Input.GetMouseButtonUp(0) && IsWatering)
        {
            RaycastHit2D[] results = Physics2D.BoxCastAll(GetCursorPosition(), Vector2.one * 0.5f, 0, transform.forward, Single.PositiveInfinity);
            foreach (RaycastHit2D hit in results)
            {
                IWaterable waterable = hit.collider.gameObject.GetComponent<IWaterable>();
                if (waterable != null)
                {
                    waterable.Water();
                    SetWaterMode();
                    return;
                }
            }
        }
    }
}
