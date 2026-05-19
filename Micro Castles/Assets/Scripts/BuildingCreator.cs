using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class BuildingCreator : Singleton<BuildingCreator>
{
    [SerializeField] private Tilemap previewMap;
    private PlayerInput _playerInput;

    private TileBase _tileBase;
    private BuildingObjectBase _selectedObj;

    private Vector2 _mousePos;
    private Vector3Int _currentGridPosition;
    private Vector3Int _previousGridPosition;

    private Camera _camera;

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
        _playerInput.Gameplay.MouseLeftClick.performed += OnMouseLeftClick;
        _playerInput.Gameplay.MouseRightClick.performed += OnMouseRightClick;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
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
        // if something is selected, show preview
        if (_selectedObj is null) return;
        
        var pos = _camera.ScreenToWorldPoint(_mousePos);
        var gridPos = previewMap.WorldToCell(pos);

        if (gridPos == _currentGridPosition) return;
        
        _previousGridPosition = _currentGridPosition;
        _currentGridPosition = gridPos;

        // update preview
    }
    
    private void OnMouseMove(InputAction.CallbackContext ctx)
    {
        _mousePos = ctx.ReadValue<Vector2>();
    }
    
    private void OnMouseLeftClick(InputAction.CallbackContext ctx) {}
    
    private void OnMouseRightClick(InputAction.CallbackContext ctx) {}

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
}
