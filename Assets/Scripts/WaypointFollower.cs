using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypoint = 0;
   
    [SerializeField] private float speed = 1.0f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, waypoints[currentWaypoint].transform.position);

        if (distance < 0.1f)
        {
            currentWaypoint = (currentWaypoint+1) % waypoints.Length; 
        }
        transform.position =  Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position,speed*Time.deltaTime);
    }
}
