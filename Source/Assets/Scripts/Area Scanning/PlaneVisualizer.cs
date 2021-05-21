using UnityEngine;
using GoogleARCore;
using System;
using System.Collections.Generic;

/// <summary>
/// Visualizes the planes detected by ARCore by generating its own mesh. Self destructs if the plane is submersed by another one.
/// </summary>
public class PlaneVisualizer : MonoBehaviour
{
    // References
    Mesh mesh;
    MeshRenderer meshRenderer;

    // Variables
    List<Vector3> boundaryVertices = new List<Vector3>();
    List<Vector3> previousBoundaryVertices = new List<Vector3>();
    DetectedPlane plane;

    void Start()
    {
        SetupReferences();
    }

    void SetupReferences()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        mesh = GetComponent<MeshFilter>().mesh = new Mesh();
    }

    void Update()
    {
        if (plane == null)
        {
            return;
        }
        else if (plane.SubsumedBy != null)
        {
            Destroy(this);
            return;
        }
        else if (plane.TrackingState != TrackingState.Tracking)
        {
            meshRenderer.enabled = false;
            return;
        }
        else if (plane.PlaneType != DetectedPlaneType.HorizontalUpwardFacing)
        {
            return;
        }

        meshRenderer.enabled = true;

        UpdateMesh();
    }

    /// <summary>
    /// Initialize his plane by providing which mesh it should be tracked by.
    /// </summary>
    /// <param name="plane"></param>
    public void Initialize(DetectedPlane plane)
    {
        this.plane = plane;
    }


    /// <summary>
    /// Update the mesh based on the boundary from the plane. Doesn't update if the boundary is the same. 
    /// </summary>
    void UpdateMesh()
    {
        plane.GetBoundaryPolygon(boundaryVertices);

        if (AreBoundariesEqual())
            return;

        // Plane generation code (below) is taken from GoogleARCore examples.

        // var _meshColors = new List<Color>();
        var meshIndices = new List<int>();

        var planeCenter = plane.CenterPose.position;

        Vector3 planeNormal = plane.CenterPose.rotation * Vector3.up;

        meshRenderer.material.SetVector("_PlaneNormal", planeNormal);

        int planePolygonCount = boundaryVertices.Count;

        // The following code converts a polygon to a mesh with two polygons, inner polygon
        // renders with 100% opacity and fade out to outter polygon with opacity 0%, as shown
        // below.  The indices shown in the diagram are used in comments below.
        // _______________     0_______________1
        // |             |      |4___________5|
        // |             |      | |         | |
        // |             | =>   | |         | |
        // |             |      | |         | |
        // |             |      |7-----------6|
        // ---------------     3---------------2

        // _meshColors.Clear();

        // // Fill transparent color to vertices 0 to 3.
        // for (int i = 0; i < planePolygonCount; ++i)
        // {
        //     _meshColors.Add(Color.clear);
        // }

        // Feather distance 0.2 meters.
        const float featherLength = 0.2f;

        // Feather scale over the distance between plane center and vertices.
        const float featherScale = 0.2f;

        // Add vertex 4 to 7.
        for (int i = 0; i < planePolygonCount; ++i)
        {
            Vector3 v = boundaryVertices[i];

            // Vector from plane center to current point
            Vector3 d = v - planeCenter;

            float scale = 1.0f - Mathf.Min(featherLength / d.magnitude, featherScale);
            boundaryVertices.Add((scale * d) + planeCenter);

            // _meshColors.Add(Color.white);
        }

        meshIndices.Clear();
        int firstOuterVertex = 0;
        int firstInnerVertex = planePolygonCount;

        // Generate triangle (4, 5, 6) and (4, 6, 7).
        for (int i = 0; i < planePolygonCount - 2; ++i)
        {
            meshIndices.Add(firstInnerVertex);
            meshIndices.Add(firstInnerVertex + i + 1);
            meshIndices.Add(firstInnerVertex + i + 2);
        }

        // Generate triangle (0, 1, 4), (4, 1, 5), (5, 1, 2), (5, 2, 6), (6, 2, 3), (6, 3, 7)
        // (7, 3, 0), (7, 0, 4)
        for (int i = 0; i < planePolygonCount; ++i)
        {
            int outerVertex1 = firstOuterVertex + i;
            int outerVertex2 = firstOuterVertex + ((i + 1) % planePolygonCount);
            int innerVertex1 = firstInnerVertex + i;
            int innerVertex2 = firstInnerVertex + ((i + 1) % planePolygonCount);

            meshIndices.Add(outerVertex1);
            meshIndices.Add(outerVertex2);
            meshIndices.Add(innerVertex1);

            meshIndices.Add(innerVertex1);
            meshIndices.Add(outerVertex2);
            meshIndices.Add(innerVertex2);
        }

        mesh.Clear();
        mesh.SetVertices(boundaryVertices);
        mesh.SetTriangles(meshIndices, 0);

        previousBoundaryVertices = this.boundaryVertices;
    }

    /// <summary>
    /// Check if the boundary is the same as the previous frame.
    /// </summary>
    /// <returns>True if the boundary is; false if it is not.</returns>
    bool AreBoundariesEqual()
    {
        if (boundaryVertices.Count != previousBoundaryVertices.Count)
        {
            return false;
        }

        for (var i = 0; i < boundaryVertices.Count; ++i)
        {
            if (boundaryVertices[i] != previousBoundaryVertices[i])
                return false;
        }

        return true;
    }
}