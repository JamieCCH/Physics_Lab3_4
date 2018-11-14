using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTrajectory : MonoBehaviour {

    [SerializeField]int iterations = 20;

    private Color initialColor;
    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.material.color = Color.red;
    }

    public void UpdateArc(float speed, float distance, float gravity, float angle, Vector3 direction)
    {
        Vector2[] arcPoints = ProjectileArcPoints(iterations, speed, distance, gravity, angle);
        Vector3[] points3d = new Vector3[arcPoints.Length];

        for (int i = 0; i < arcPoints.Length; i++)
        {
            points3d[i] = new Vector3(0, arcPoints[i].y, arcPoints[i].x);
        }

        lineRenderer.positionCount = arcPoints.Length;
        lineRenderer.SetPositions(points3d);

        transform.rotation = Quaternion.LookRotation(direction);

    }

    protected Vector2[] ProjectileArcPoints(int iterations, float speed, float distance, float gravity, float angle)
    {
        float iterationSize = distance / iterations;

        float radians = angle;

        Vector2[] points = new Vector2[iterations + 1];

        for (int i = 0; i <= iterations; i++)
        {
            float x = iterationSize * i;
            float t = x / (speed * Mathf.Cos(radians));
            float y = -0.5f * gravity * (t * t) + speed * Mathf.Sin(radians) * t;

            Vector2 p = new Vector2(x, y);

            points[i] = p;
        }

        return points;
    }
}
