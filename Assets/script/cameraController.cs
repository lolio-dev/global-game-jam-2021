using UnityEngine;

public class cameraController : MonoBehaviour
{
    public Camera cam;

    public GameObject player1;
    public GameObject player2;
    public GameObject gbPlayerPositionStart;
    public float playerPositionYStart;

    public float camSize;
    public float camPositionX;
    public float camPositionY;
    public float zoomMax;
    
    void Update()
    {
        playerPositionYStart = gbPlayerPositionStart.transform.position.y;

        if (player1.transform.position.y > player2.transform.position.y || player1.transform.position.y == player2.transform.position.y)
        {
            camSize = Mathf.Abs(player1.transform.position.x - player2.transform.position.x) * 2 / 4 + Mathf.Abs(player1.transform.position.y - playerPositionYStart);
        }
        else
        {
            camSize = Mathf.Abs(player1.transform.position.x - player2.transform.position.x) * 2 / 4 + Mathf.Abs(player2.transform.position.y - playerPositionYStart);
        }
        
        
        camPositionX = (player1.transform.position.x + player2.transform.position.x) / 2;
        camPositionY = camSize - 5;
        transform.position = new Vector3(camPositionX, camPositionY, -10);

        cam.orthographicSize = camSize;

        if (cam.orthographicSize <= zoomMax)
        {
            cam.orthographicSize = zoomMax;
            transform.position = new Vector3(camPositionX, cam.orthographicSize - 5, -10);
        }
    }
}