using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    /// <summary>
    /// This handles the spawn logic of the player. This is a scene specific script so that the respawn points can transfer cleanly from one scene to the next.
    /// While this script gets reused, it gets destroyed on load and each instance per scene has it's own spawn points assigned in inspector.
    /// </summary>
    [Header("Player Setup")]
    [SerializeField] private GameObject playerPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] playerSpawnPoints;        // Assigned in Inspector on each level. NOTE: Must be careful to avoid reordering and adding new points. Will look into how to better optimize.
    [SerializeField] private Transform activeSpawnPoint;           // The respawn point that the player has reached is stored here.

    private void Start()
    {
        // Reset to first spawn point when the scene loads (level start).
        if (playerSpawnPoints.Length > 0)
        {
            activeSpawnPoint = playerSpawnPoints[0];
            Debug.Log("Player RespawnManager defaulting to first spawn point.");
        }
        else
        {
            Debug.Log("No spawn points assigned.");
        }
    }
    // This is called by GameManager.
    internal void SpawnPlayer()
    {
        Instantiate(playerPrefab, activeSpawnPoint.position, activeSpawnPoint.rotation);
    }

    // This method is called when a player reaches a new check point.
    internal void SetActiveSpawnPoint(Transform newSpawn)
    {
        activeSpawnPoint = newSpawn;
    }
}
