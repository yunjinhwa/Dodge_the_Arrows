using UnityEngine;

public class ItemGenerator : TimedSpawner
{
    [SerializeField] private GameObject healItemPrefab;

    protected override GameObject Prefab => healItemPrefab;
}