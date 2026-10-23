using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 0, -10f);

    private void Start()
    {
        GameObject p = GameObject.FindWithTag("Player");      // this really only works because this is a single player game,, otherwise,, dont do this
        if (p != null) 
            player = p.transform;
        else
            Debug.Log("Camera can't find the player.");
    }
    void LateUpdate()
    {
        if (player == null) return;
        Vector3 desiredPosition = player.position + offset;               // this is where we want cam to be
        Vector3 smoothed = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);         // this reduces stutter
        transform.position = smoothed;        // boom
    }
}
