using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoofahScript : MonoBehaviour
{
    
    [SerializeField]private Vector2 touchStart;
    [SerializeField]private Vector2 touchEnd;
    [SerializeField]private int flickTime = 5;
    [SerializeField]private int flickLength = 0;
    [SerializeField]private float ballVelocity = 0.0f;
    [SerializeField]private float ballSpeed = 0;
    [SerializeField]private Vector3 worldAngle;
    [SerializeField]private bool GetVelocity = false;
    public GameObject loofahPrefab;
    //public GameObject[] woosh; //no
//    public AudioClip ballAudio;  //yes
    private float comfortZone = 0.0f;
    private bool couldBeSwipe;




    void Start ()
    {
        Time.timeScale = 1;
        if ( Application.isEditor )
        {
            Time.fixedDeltaTime = 0.01f;
        }
    }

    void Update()
    {
        /*
        if (Input.touchCount > 0)
        {
            Debug.Log("test");

            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;

            

            var touch = Input.touches[0];
            switch (touch.phase)
            {
            case TouchPhase.Began:
                // only consider flicking when user clicks on the thing
                if (Physics.Raycast(ray, out hit, 100))
                {
                    Debug.Log("LoofahScript hit");

                    if (hit.transform.tag == "loofah")
                    {
                        Debug.Log("LoofahScript: " + hit.transform.name + " : " + hit.transform.tag);
                        flickTime = 5;
                        timeIncrease();
                        couldBeSwipe = true;
                        GetVelocity = true;
                        touchStart= touch.position;
                    }
                }
                break;
            case TouchPhase.Moved:
                if (Mathf.Abs(touch.position.y - touchStart.y) < comfortZone)
                {
                    couldBeSwipe = false;
                }
                else
                {
                    couldBeSwipe = true;
                }
                break;
            case TouchPhase.Stationary:
                if (Mathf.Abs(touch.position.y - touchStart.y) < comfortZone)
                {
                    couldBeSwipe = false;
                }
                break;
            case TouchPhase.Ended:
                float swipeDist =(touch.position - touchStart).magnitude;
                //couldBeSwipe
                if (couldBeSwipe ||  swipeDist > comfortZone)
                {
                    GetVelocity = false;
                    touchEnd = touch.position;
                    GetSpeed();
                    GetAngle();
                    GameObject ball = Instantiate(loofahPrefab, new Vector3(0.0f,0,-3.0f), Quaternion.identity) as GameObject;
                    Debug.Log("Instantiated");
                    Destroy(ball.GetComponent<LoofahScript>());
                    ball.GetComponent<Rigidbody>().useGravity = true;
                    ball.GetComponent<Rigidbody>().AddForce(new Vector3((worldAngle.x * ballSpeed), (worldAngle.y * ballSpeed), (worldAngle.z * ballSpeed)));
                    

                }
                break;
            default :
                break;

            }//end switch case
            if (GetVelocity)
            {
                flickTime++;
            }
        }
        */

        // -------------------- MOUSE FLICK --------------------
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Input.mousePosition;
            couldBeSwipe = true;
            GetVelocity = true;
            flickTime = 0;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 current = Input.mousePosition;

            if (Mathf.Abs(current.y - touchStart.y) < comfortZone)
                couldBeSwipe = false;

            if (GetVelocity)
                flickTime++;
        }

        if (Input.GetMouseButtonUp(0))
        {
            touchEnd = Input.mousePosition;

            float swipeDist = (touchEnd - touchStart).magnitude;

            if (couldBeSwipe || swipeDist > comfortZone)
            {
                // Calculate velocity + angle
                GetVelocity = false;
                GetSpeed();
                GetAngle();

                // -------------------- FIXED SPAWN --------------------

                GameObject ball = Instantiate(loofahPrefab, this.transform.position, Quaternion.identity);
                ball.transform.parent = this.transform.parent;
                
                AddLoofahGravity(ball);

                Rigidbody rb = ball.GetComponent<Rigidbody>();
                rb.useGravity = true;

                Vector3 force = new Vector3(
                    worldAngle.x * ballSpeed,
                    worldAngle.y * ballSpeed,
                    worldAngle.z * ballSpeed
                );

                float maxForce = 15f; // your chosen limit

                if (force.magnitude > maxForce)
                {
                    force = force.normalized * maxForce; // keep direction, limit strength
                }

                Debug.Log(force);
                rb.AddForce(force, ForceMode.Impulse);
            }
        }
        
    }

    void timeIncrease()
    {
        if (GetVelocity)
        {
            flickTime++;
        }
    }

    void GetSpeed()
    {
        flickLength = 90;
        if (flickTime > 0)
        {
            ballVelocity = flickLength / (flickLength - flickTime);
        }
        ballSpeed = ballVelocity * 30;
        ballSpeed = ballSpeed - (ballSpeed * 1.65f);
        if (ballSpeed <= -33)
        {
            ballSpeed = -33;
        }
        Debug.Log("flick was " + flickTime);
        flickTime = 5;
    }

    void GetAngle ()
    {
        worldAngle = Camera.main.ScreenToWorldPoint(new Vector3 (touchEnd.x, touchEnd.y + 50f, ((Camera.main.nearClipPlane - 50.0f))));
    }

    void AddLoofahGravity(GameObject ball)
    {
        ball.AddComponent<Rigidbody>();
        ball.GetComponent<Rigidbody>().useGravity = true;

        ball.AddComponent<LoofahOOB>();

        Destroy(ball.GetComponent<LoofahScript>());
    }
}
