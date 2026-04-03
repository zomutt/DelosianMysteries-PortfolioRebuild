using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private RespawnManager RespawnManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Respawn();     // This is not meant to permanently live here. This lives here while I define the overarching flow and design scenes, menus, etc.
    }
    internal void Respawn()
    {
        // Needs to find the RespawnManager.cs each scene because it is a non-persistent script. This avoids having to do scene checks.
        RespawnManager = FindAnyObjectByType<RespawnManager>();
        RespawnManager.SpawnPlayer();
        Debug.Log("GameManager respawning player.");
    }
}
