using UnityEngine;

public class ArrowGenerator : TimedSpawner
{
    [SerializeField] private GameObject[] arrowPrefabs;

    protected override GameObject Prefab
    {
        get
        {
            if (arrowPrefabs == null || arrowPrefabs.Length == 0)
                return null;

            int index = Random.Range(0, arrowPrefabs.Length);
            return arrowPrefabs[index];
        }
    }
}