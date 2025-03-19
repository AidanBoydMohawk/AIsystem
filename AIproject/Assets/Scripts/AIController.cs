using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private StateMachine stateMachine;
    public Transform player;

    //patrolling
    public Transform[] patrolWaypoints;
    public int waypointIndex;
    public float patrolSpeed = 5;
    public float detectionRange = 3;
    //vision
    private bool isPLayerInVisionCone = false;
    public float visionAngle = 120f;

    //hearing
    public float hearingThreshold = 10f;
    public float hearingRange = 15f;
    public float playerVolume = 12f; //temp move to player controller when we have it 

    //how aware of the plaeyr the ai is (sound wise)
    private float awarenessLevel = 0f;
    private float awarenessThreshold = 100f;
    public float awarenessDecay;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new StateMachine();
        //set default state here
        ChangeState(new StateIdle(this));
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.update();

    }

    public void ChangeState(State newState)
    {
        stateMachine.ChangeState(newState);
    }

    public bool CanSeePlayer()
    {

        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        float angleToPLayer = Vector3.Angle(transform.forward, directionToPlayer);
        if(angleToPLayer < visionAngle / 2)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRange))
            {
                if (hit.transform == player)
                {
                    return true;
                }

            }

        }
        return false; 

       /* if(!isPLayerInVisionCone)
        {
            return false; //player must be inside vision cone
        }

        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRange))
        {
            if(hit.transform == player)
            {
                return true;
            }
            
        }

        return false;*/
        

    }

    public bool CanHearPlayer()
    {
        if (player == null) return false; 

        if (Vector3.Distance(transform.position, player.position) < hearingRange && playerVolume > hearingThreshold)
        {
            return true;
        }
        return false;
    }

    public void UpdateAwareness()
    {
        if (CanSeePlayer())
        {
            awarenessLevel += 50 * Time.deltaTime; 
        }
        else if(CanHearPlayer())
        {
            awarenessLevel += 20 * Time.deltaTime;
        }
        else
        {
            awarenessLevel -= awarenessDecay * Time.deltaTime;
        }

        //awarenessLevel = Mathf.Clamp(awarenessLevel, 0, awarenessThreshold);
    }

    public void SetPlayerInVisionCone(bool isVisible)
    {
       isPLayerInVisionCone=isVisible;

    }    

    public void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * patrolSpeed);

    }

    public void RotateToObject(Vector3 objectPosition)
    {
        Vector3 direction = (objectPosition - transform.position).normalized;
        float rotationSpeed = 3f;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotationSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public void Patrol()
    {
        if (patrolWaypoints.Length == 0) return;

        Transform targetWaypoint = patrolWaypoints[waypointIndex];

        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, Time.deltaTime * patrolSpeed);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            waypointIndex = (waypointIndex + 1) % patrolWaypoints.Length;
        }

        float rotationSpeed = 3f;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotationSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
