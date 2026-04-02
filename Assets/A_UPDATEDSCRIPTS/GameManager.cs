using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// So far, what this script does is manage player respawn/instantiating.
    /// </summary>
    [SerializeField] private GameObject playerPrefab;

    // Respawn points are scattered throughout the level. Once the player collides with them, it sets the active respawn point.
    // The player may backtrack through the level, but this will change where they respawn if they die. May be adjusted as flow develops.
    [SerializeField] private Transform[] playerSpawnPoints;
    [SerializeField] private Transform activeSpawnPoint;

    internal void StartGame()
    {
        if (activeSpawnPoint == null)
        {
            activeSpawnPoint = playerSpawnPoints[0];
            Debug.Log("SpawnPoint not found, defaulting to level start.");
        }
        InstantiatePlayer(activeSpawnPoint);
    }
    private void InstantiatePlayer(Transform spawnPoint)
    {
        // The rotation of the spawn points are locked and set at 0, this serves as a failsafe to prevent rotation errors.
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
