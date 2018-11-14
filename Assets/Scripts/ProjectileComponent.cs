using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class ProjectileComponent : MonoBehaviour {

    public Vector3 m_initialVelocity = Vector3.zero;
    public float m_travelTime = 1.0f;
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
    public float horizontalAngle = 5.0f;
    public float initialVelocity = 50.0f;

    const float minHorizontalAngle = -35.0f;
    const float maxHorizontalAngle = 35.0f;
    const float maxVerticalAngle = 90.0f;
    const float minVerticalAngle = 0.0f;

    private float maxForce;
    private float minForce = 1.0f;

    private float inputForce;

    void Start () {
        m_rb = GetComponent<Rigidbody>();
        m_landingDisplay = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        m_landingDisplay.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        m_landingDisplay.transform.localScale = new Vector3(1.0f, 0.1f, 1.0f);
        m_landingDisplay.GetComponent<Renderer>().material.color = Color.blue;
        m_landingDisplay.GetComponent<Collider>().enabled = false;
        m_landingDisplay.GetComponent<Renderer>().enabled = false;

        maxForce = CalculateMaxVi().magnitude;

        print(maxForce);
    }

    public void Launch()
    {
        if(!IsGrounded)
        {
            return;
        }

       
        m_landingDisplay.transform.position = transform.position + CalculateMaxVi();
        m_landingDisplay.GetComponent<Renderer>().enabled = true;
        m_isGrounded = false;
        GetComponent<Renderer>().enabled = false;

        inputForce = maxForce;

        GameObject ball = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        ball.GetComponent<Rigidbody>().velocity = GetLandingPosition() * inputForce;
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


   public Vector3 GetTrajectoryEnd(){

        float vertAngleRad = Mathf.Deg2Rad * verticalAngle;
        float dh = ((-2 * initialVelocity * initialVelocity * Mathf.Sin(vertAngleRad) * Mathf.Cos(vertAngleRad)) / Physics.gravity.y);
        float hozAngleRad = Mathf.Deg2Rad * horizontalAngle;
        Vector3 destination = new Vector3(dh * Mathf.Sin(hozAngleRad), 0, dh * Mathf.Cos(hozAngleRad))+transform.position;

        return destination;
    }


    Vector3 GetLandingPosition()
    {

        float vertAngleRad = Mathf.Deg2Rad * verticalAngle;
        float hozAngleRad = Mathf.Deg2Rad * horizontalAngle;
 
        Vector3 landingPosition = new Vector3(Mathf.Sin(hozAngleRad), Mathf.Sin(vertAngleRad), Mathf.Cos(hozAngleRad));

        Debug.Log(landingPosition.magnitude*inputForce);

        return landingPosition;
    }


    Vector3 CalculateMaxVi(){
        //d = Vit + 0.5g(t^2)
        //vi = (d - 0.5g(t^2))/t
        Vector3 d = m_desiredDestination.position - transform.position;
        float t = 2.5f;
        Vector3 vi = (d - (0.5f * Physics.gravity * Mathf.Sqrt(t))) / t;
        vi.y = 0;

        return vi;
    }

    //Vector3 GetLandingPosition()
    //{
    //    //d = Vit + 0.5at^2

    //    //vf = vi + at
    //    //vf - vi = at
    //    //(vf - vi)/a = t

    //    float time = (0.0f - m_initialVelocity.y) / Physics.gravity.y;
    //    time *= 2.0f;
    //    //zero out y component and multiply vector by time
    //    Vector3 flatVelocity = m_initialVelocity;
    //    flatVelocity.y = 0.0f;
    //    flatVelocity *= time;

    //    return transform.position + flatVelocity; 
    //}


}
