﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PhysicsEngine : MonoBehaviour {

    // average velocity this FixedUpdate();
    public float mass = 1f;
    public Vector3 velocityVector;
    public Vector3 netForceVector;
    private List<Vector3> forceVectorList = new List<Vector3>();

    // Use this for initialization
    void Start() {
        SetupThrustTrails();
	}

    void FixedUpdate()
    {
        RenderTrails();
        UpdatePosition();
        /*
        if (netForceVector == Vector3.zero)
            transform.position += velocityVector * Time.deltaTime;
        else
            Debug.LogError("Unbalanced force detected, help!");
        */
    }

    public void AddForce(Vector3 forceVector)
    {
        forceVectorList.Add(forceVector);
    }

    private void UpdatePosition()
    {
        // Sum the forces and clear the list
        netForceVector = Vector3.zero;
        foreach (Vector3 forceVector in forceVectorList)
        {
            netForceVector = netForceVector + forceVector;
        }
        // clear the list
        forceVectorList = new List<Vector3>();

        // Calculate position change due to net force
        Vector3 accelerationVector = netForceVector / mass;
        velocityVector += accelerationVector * Time.deltaTime;
        transform.position += velocityVector * Time.deltaTime;
    }

    /// <summary>
    /// Code for drawing thrust trails
    /// </summary>
    public bool showTrails = true;

    private LineRenderer lineRenderer;
    private int numberOfForces;

    // Use this for initialization
    void SetupThrustTrails()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.SetColors(Color.yellow, Color.yellow);
        lineRenderer.SetWidth(0.2F, 0.2F);
        lineRenderer.useWorldSpace = false;
    }

    void RenderTrails()
    {
        if (showTrails)
        {
            lineRenderer.enabled = true;
            numberOfForces = forceVectorList.Count;
            lineRenderer.SetVertexCount(numberOfForces * 2);
            int i = 0;
            foreach (Vector3 forceVector in forceVectorList)
            {
                lineRenderer.SetPosition(i, Vector3.zero);
                lineRenderer.SetPosition(i + 1, -forceVector);
                i = i + 2;
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}
