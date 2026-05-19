using UnityEngine;
using UnityEngine.Tilemaps;

public enum Category
{
    Path,
    Forbidden
}

[CreateAssetMenu(fileName = "Buildable", menuName = "BuildingObjects/Create Buildable")]
public class BuildingObjectBase : ScriptableObject
{
    [SerializeField] private Category category;
    [SerializeField] private TileBase tileBase;

    public TileBase TileBase => tileBase;
    public Category Category => category;
}
