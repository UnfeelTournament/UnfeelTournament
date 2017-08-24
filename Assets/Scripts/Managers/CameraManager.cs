using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public float ReactionTime = 0.2f;               // reaction time, so camera not instantly moving/zooming
    public float Padding = 2f;                      // padding
    public float MinSize = 0.5f;                    // Min Size
    public float MaxSize = 15f;                    // Min Size

    [HideInInspector]
    public float originalOrth;
    [HideInInspector]
    public Vector3 originalP;
    
    public Transform[] Players;
    [HideInInspector]
    public int NumOfPlayer;

    private Camera main_Camera;                     // Camera pointer
    private float m_ZoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
    private Vector3 m_MoveVelocity;                 // Reference velocity for the smooth damping of the position.
    private Vector3 m_DesiredPosition;              // The position the camera is moving towards.

    private void Awake()
    {
        //Destroy Main Camera in the scene, if exist.
        GameObject mc = GameObject.FindGameObjectWithTag("MainCamera");
        //Debug.Log(mc);
        Destroy(mc);

        main_Camera = GetComponentInChildren<Camera>();
        if (Players.Length != 0) { NumOfPlayer = Players.Length; } 
        else
        {
            NumOfPlayer = GlobalManager._instance._numOfPlayers;
            Players = new Transform[NumOfPlayer];
        }

        originalOrth = main_Camera.orthographicSize;
        originalP = main_Camera.WorldToViewportPoint(main_Camera.gameObject.transform.position);
    }

    private void Update()
    {
       //every frame per second, move and zoom.
       Move();
       Zoom();
    }

    private void Move()
    {
        FindAveragePosition();
        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, ReactionTime);
    }

    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3(0,0,0);
        int numTargets = 0;

        //for (int i = 0; i < Players.Length; i++)
        for (int i = 0; i < NumOfPlayer; i++)
        {
            // If the target isn't active/ dead, dount count it calculation.
            //if (!Players[i].gameObject.activeSelf) continue;
            if (!GlobalManager._instance._isActive[i] || GlobalManager._instance._players[i].GetComponent<Character>()._isDead) continue;

            // Add to the average and increment the number of targets in the average.
            //averagePos += Players[i].position;
            averagePos += GlobalManager._instance._players[i].GetComponent<Transform>().position;
            numTargets++;
        }
        
        // If there are targets divide the sum of the positions by the number of them to find the average.
        if (numTargets > 0)
            averagePos /= numTargets;

        // Keep the same y value.
        //averagePos.y = transform.position.y;
        averagePos.z = -20f;
        // The desired position is the average position;
        m_DesiredPosition = averagePos;

        //Debug.Log("camera Position" + averagePos);

        //adjusting backGroundImage, so it feels alive
        GameObject BackGroundImage = GameObject.Find("BackGroundImage");
        BackGroundImage.transform.position = new Vector3(averagePos.x*0.3f, (averagePos.y*0.2f)+2f,10);
    }

    private void Zoom()
    {
        // Find the required size based on the desired position and smoothly transition to that size.
        float requiredSize = FindRequiredSize();

        /*Scale HUD so it always has the same size to viewer*/
        //Transform HUD = transform.FindChild("HUD(Clone)");
        Transform HUD = GlobalManager._instance._hud.transform;
        if (!HUD) Debug.Log("HUD not found");

        Vector3 sc = HUD.localScale;
        float ns = (float)0.008f * main_Camera.orthographicSize / originalOrth;
        sc.x = sc.y = ns;
        HUD.localScale = sc;

        Vector3 nv = main_Camera.ViewportToWorldPoint(originalP);
        nv.z = -10f;
        HUD.transform.position = nv;

        main_Camera.orthographicSize = Mathf.SmoothDamp(main_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, ReactionTime);
    }

    private float FindRequiredSize()
    {
        // Find the position the camera holder is moving towards in its local space.
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

        // Start the camera's size calculation at zero.
        float size = 0f;

        // Go through all the players...
        for (int i = 0; i < NumOfPlayer; i++)
        {
            // if a target isn't active continue on to the next target.
            //if (!Players[i].gameObject.activeSelf) continue;
            if (!GlobalManager._instance._isActive[i] || GlobalManager._instance._players[i].GetComponent<Character>()._isDead) continue;

            // Otherwise, find the position of the target in the camera's local space.
            //Vector3 targetLocalPos = transform.InverseTransformPoint(Players[i].position); 
            Vector3 targetLocalPos = transform.InverseTransformPoint(GlobalManager._instance._players[i].GetComponent<Transform>().position);

            // Find the position of the target from the desired position of the camera's local space.
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            // Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

            // Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / main_Camera.aspect);
        }
        
        size += Padding; //add padding
        size = Mathf.Max(size, MinSize);    //check if its below minimum size

        return size > MaxSize ? MaxSize : size;
    }

    public void SetStartPositionAndSize()
    {
        FindAveragePosition();
        transform.position = m_DesiredPosition;
        main_Camera.orthographicSize = FindRequiredSize();
    }
}
