using System;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class BuildingCreator : Singleton<BuildingCreator>
{
    [SerializeField] private Tilemap previewMap, defaultMap;
    private PlayerInput _playerInput;

    private TileBase _tileBase;
    private BuildingObjectBase _selectedObj;

    private Vector2 _mousePos;
    private Vector3Int _currentGridPosition;
    private Vector3Int _previousGridPosition;
    private bool _holdActive;

    private Camera _camera;
    [SerializeField] private float cameraZoomRate;

    protected override void Awake()
    {
        base.Awake();
        _playerInput = new PlayerInput();
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Gameplay.MousePosition.performed += OnMouseMove;
        
        _playerInput.Gameplay.MouseLeftClick.started += OnMouseLeftClick;
        _playerInput.Gameplay.MouseLeftClick.canceled += OnMouseLeftClick;
            
        _playerInput.Gameplay.MouseRightClick.performed += OnMouseRightClick;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.Gameplay.MousePosition.performed -= OnMouseMove;
        
        _playerInput.Gameplay.MouseLeftClick.started -= OnMouseLeftClick;
        _playerInput.Gameplay.MouseLeftClick.canceled -= OnMouseLeftClick;
        
        _playerInput.Gameplay.MouseRightClick.performed -= OnMouseRightClick;
    }

    private BuildingObjectBase SelectedObj {
        set
        {
            _selectedObj = value; 
            _tileBase = _selectedObj != null ? _selectedObj.TileBase : null;
            UpdatePreview();
        }
    }

    private void Update()
    {
        if (_camera.orthographicSize < 5) _camera.orthographicSize += cameraZoomRate;
        
        // if something is selected, show preview
        if (!_selectedObj) return;
        var pos = _camera.ScreenToWorldPoint(_mousePos);
        var gridPos = previewMap.WorldToCell(pos);

        if (gridPos == _currentGridPosition) return;
        _previousGridPosition = _currentGridPosition;
        _currentGridPosition = gridPos;
        UpdatePreview();
        
        if (_holdActive) HandleDrawing();
    }
    
    private void OnMouseMove(InputAction.CallbackContext ctx)
    {
        _mousePos = ctx.ReadValue<Vector2>();
    }

    private void OnMouseLeftClick(InputAction.CallbackContext ctx)
    {
        if (!_selectedObj || EventSystem.current.IsPointerOverGameObject()) return;

        if (ctx.phase == InputActionPhase.Started)
        {
            _holdActive = true;
            HandleDrawing();
        }
        else if (ctx.phase == InputActionPhase.Canceled)
        {
            _holdActive = false;
        }
    }

    private void OnMouseRightClick(InputAction.CallbackContext ctx)
    {
        SelectedObj = null;
    }

    public void ObjectSelected(BuildingObjectBase obj)
    {
        SelectedObj = obj;
        
        // set preview of selected tile
        // on click, draw
        // on right click, remove preview
    }

    private void UpdatePreview()
    {
        // remove preview of the old tile
        previewMap.SetTile(_previousGridPosition, null);
        
        // set current tile to current mouse positions tile
        previewMap.SetTile(_currentGridPosition, _tileBase);
    }

    private void HandleDrawing()
    {
        DrawItem();
    }

    private void DrawItem()
    {
        defaultMap.SetTile(_currentGridPosition, _tileBase);
    }
}
