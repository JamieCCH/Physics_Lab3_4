using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class ProjectileComponent : MonoBehaviour {

    public Vector3 m_initialVelocity = Vector3.zero;
    //public float m_travelTime = 1.0f;
    public Transform m_desiredDestination = null;
    public GameObject projectilePrefab;

    private Rigidbody m_rb = null;
    private GameObject m_landingDisplay = null;
    private bool m_isGrounded = true;
    public bool IsGrounded
    {
        get { return m_isGrounded; }
    }

    public float verticalAngle = 30.0f;
    public float horizontalAngle = 0.0f;

    public float initialVelocity = 50.0f;

    const float minHorizontalAngle = -35.0f;
    const float maxHorizontalAngle = 35.0f;
    const float maxVerticalAngle = 89.0f;
    const float minVerticalAngle = 1.0f;

    const float maxVelocity = 50.0f;
    const float minVelocity = 1.0f;

    public float inputForce = 50.0f;

    void Start () 
    {
        m_rb = GetComponent<Rigidbody>();
        m_landingDisplay = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        m_landingDisplay.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        m_landingDisplay.transform.localScale = new Vector3(1.0f, 0.3f, 1.0f);
        m_landingDisplay.GetComponent<Renderer>().material.color = Color.blue;
        m_landingDisplay.GetComponent<Collider>().enabled = false;
        m_landingDisplay.GetComponent<Renderer>().enabled = false;

        //Debug.Log("CalculateMaxVi: " + CalculateMaxVi());

    }

    public void Launch()
    {
        if(!IsGrounded)
        {
            return;
        }

        m_landingDisplay.transform.position = DisplayLandingSpot();
        m_landingDisplay.GetComponent<Renderer>().enabled = true;
        m_isGrounded = false;
        GetComponent<Renderer>().enabled = false;

        GameObject ball = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector3 v = GetTrajectoryEnd();
        v.y = GetLandingPosition().y;
        ball.GetComponent<Rigidbody>().velocity = GetLandingPosition() * inputForce;
        Debug.Log("max velocity: " + ball.GetComponent<Rigidbody>().velocity);
    }


    public float SetVerticalAngle(float rotX)
    {
        verticalAngle += rotX;
        if (verticalAngle < minVerticalAngle || verticalAngle > maxVerticalAngle){
            verticalAngle += rotX * -1;
        }
        
        return verticalAngle;
    }

    public float SetHorizontalAngle(float rotY)
     {
        horizontalAngle += rotY;

        if (horizontalAngle > maxHorizontalAngle || horizontalAngle < minHorizontalAngle)
        {
            horizontalAngle += rotY * -1;
        }

        return horizontalAngle;
    }



    Vector3 GetLandingPosition()
    {
        float vertAngleRad = Mathf.Deg2Rad * verticalAngle;
        float hozAngleRad = Mathf.Deg2Rad * horizontalAngle;
        float dh = ((-2 * initialVelocity * initialVelocity * Mathf.Sin(vertAngleRad) * Mathf.Cos(vertAngleRad)) / Physics.gravity.y);

        Vector3 landingPosition = new Vector3(Mathf.Sin(hozAngleRad), Mathf.Sin(vertAngleRad), Mathf.Cos(hozAngleRad));

        //print("trajectory: " + GetTrajectoryEnd()); 
        print("landingPosition: " + landingPosition);
        return landingPosition;
    }

    public Vector3 GetTrajectoryEnd()
    {
        float vertAngleRad = Mathf.Deg2Rad * verticalAngle;
        float dh = ((-2 * initialVelocity * initialVelocity * Mathf.Sin(vertAngleRad) * Mathf.Cos(vertAngleRad)) / Physics.gravity.y);
        float hozAngleRad = Mathf.Deg2Rad * horizontalAngle;
        Vector3 destination = new Vector3(dh * Mathf.Sin(hozAngleRad), 0, dh * Mathf.Cos(hozAngleRad))+transform.position;

        return destination;
    }

    public float GetMaxSpeed()
    {
        float vertAngleRad = Mathf.Deg2Rad * verticalAngle;

        Vector3 direction = GetTrajectoryEnd()- transform.position;
        float yOffset = -direction.y;
        float d = direction.magnitude;
        float speed = (d * Mathf.Sqrt(Physics.gravity.magnitude) * Mathf.Sqrt(1 / Mathf.Cos(vertAngleRad))) / Mathf.Sqrt(2 * d * Mathf.Sin(vertAngleRad) + 2 * yOffset * Mathf.Cos(vertAngleRad));

        Debug.Log("speed: "+speed); //50
        return speed;
    }

    public Vector3 DisplayLandingSpot(){

        float yOffset = GetLandingPosition().y * GetMaxSpeed();
        float time = (0.0f - yOffset) / Physics.gravity.y;
            time *= 2.0f;
        Vector3 flatVelocity = GetLandingPosition()* GetMaxSpeed();
            flatVelocity.y = 0.0f;
            flatVelocity *= time;

            return transform.position + flatVelocity; 
    }



}
