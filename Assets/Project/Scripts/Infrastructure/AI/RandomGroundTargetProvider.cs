using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomGroundTargetProvider : MonoBehaviour, ITargetProvider
{
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private int radius = 6;

    private void Awake()
    {
        if (groundTilemap == null)
        {
            groundTilemap = FindObjectOfType<Tilemap>();
        }
    }

    public Vector3 GetTarget()
    {
        if (groundTilemap == null)
        {
            Debug.LogError("Ground Tilemap not assigned and could not be found via FindObjectOfType!", this);
            return transform.position;
        }

        for (int i = 0; i < 15; i++) // Increased retries to 15
        {
            Vector3 offset = new Vector3(
                Random.Range(-radius, radius),
                Random.Range(-radius, radius),
                0
            );

            Vector3 worldPos = transform.position + offset;
            Vector3Int cell = groundTilemap.WorldToCell(worldPos);

            if (groundTilemap.HasTile(cell))
            {
                return groundTilemap.GetCellCenterWorld(cell);
            }
        }

        Debug.LogWarning("Could not find a valid ground tile within radius. Staying put.", this);
        return transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
