﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {
    private LineRenderer line;
    private Vector3 mousepos;
    private Vector3 startpos;
    private Vector3 endPos;
    public List<Vector2> newVerticies = new List<Vector2>();
    private Vector2[] colvec = new Vector2[0];
    private EdgeCollider2D col;
    private bool ison = true;
    int n = 1;
    bool lline = false;
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetMouseButtonDown(0)&& ison)
        {
            if (line == null)
                createLine();
            mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos.z = 0;
            newVerticies.Add(new Vector2(mousepos.x, mousepos.y));
            line.SetPosition(0, mousepos);
            line.SetPosition(1, mousepos);
            
            startpos = mousepos;
        }
        else if(Input.GetMouseButtonUp(0)&& line && ison)
        {
            if (line)
            {
                mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousepos.z = 0;
                endPos = mousepos;
                col.points = newVerticies.ToArray();
                line = null;
                ison = false;
                n = 1;
            }
        }
        else if(Input.GetMouseButton(0)&& ison)
        {
            line.positionCount = n+1;
         
            mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos.z = 0;
            line.SetPosition(n, mousepos);
            newVerticies.Add(new Vector2(mousepos.x, mousepos.y));
            n++;
        }

        if (lline)
        {
           for(int i = 0; i<=n;++i)
            {
                line.SetPosition(n, col.points[n]);
            }
        }
    }
    private void createLine()
    {
        line = new GameObject("Line").AddComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Diffuse"));
        col = GameObject.Find("Line").AddComponent<EdgeCollider2D>();
        col.edgeRadius = 0.1f;
        line.startWidth = 0.1f;
        line.startColor = Color.black;
        line.useWorldSpace = true;
    }
    private void addColliderToLine()
    {
      
  
  


    }

}