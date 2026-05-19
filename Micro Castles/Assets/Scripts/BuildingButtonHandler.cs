using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButtonHandler : MonoBehaviour
{
    [SerializeField] private BuildingObjectBase item;
    private Button _button;
    
    BuildingCreator _buildingCreator;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ButtonClicked);
        _buildingCreator = BuildingCreator.GetInstance();
    }

    private void ButtonClicked()
    {
        Debug.Log("The selected item is " + item.name);
    }
}
