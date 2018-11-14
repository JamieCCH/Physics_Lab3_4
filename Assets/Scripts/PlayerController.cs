using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProjectileComponent))]
[RequireComponent(typeof(DrawTrajectory))]

public class PlayerController : MonoBehaviour {

    ProjectileComponent m_projectile = null;
    DrawTrajectory m_projectileArc = null;
    [SerializeField] float speed = 1.5f;

    void Start () {
        m_projectile = GetComponent<ProjectileComponent>();
        m_projectileArc = GetComponentInChildren<DrawTrajectory>();
    }
	

	void Update () {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_projectile.Launch();
        }

        //m_projectile.SetVerticalAngle(Input.GetAxis("Vertical")* speed);
        //m_projectile.SetHorizontalAngle(Input.GetAxis("Horizontal") * speed);

        if (Input.GetKey(KeyCode.RightArrow))
        {
            m_projectile.SetHorizontalAngle(speed);
        }
        if(Input.GetKey(KeyCode.LeftArrow)){
            m_projectile.SetHorizontalAngle(-speed);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            m_projectile.SetVerticalAngle(speed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            m_projectile.SetVerticalAngle(-speed);
        }


        Vector3 direction = m_projectile.GetTrajectoryEnd()- transform.position;
        m_projectileArc.UpdateArc(m_projectile.initialVelocity, direction.magnitude, Physics.gravity.magnitude, m_projectile.verticalAngle * Mathf.Deg2Rad, direction);
    }

}
