/**
 * Created by Mike Marek (April 2018)
 * PointTracker.cs
 * Tracks vertices on a mesh relative to its parent transformations.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class PointTracker : MonoBehaviour
{
    private MeshFilter  meshFilter;
    private Transform[] markers;


    /**
     * Obtain references to the attached MeshFilter component and point tracker
     * prefabs used to mark the mesh vertices.
     *
     * @param   null
     * @return  null
    **/
    void Start()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        markers = new Transform[meshFilter.mesh.vertexCount];

        Debug.Log(string.Format(
            "Tracking {0} vertices in \"{1}\"",
            meshFilter.mesh.vertexCount,
            transform.name));

        for (int i = 0; i < meshFilter.mesh.vertexCount; i++)
        {
            markers[i] = ObjectPool.Instance.GetObject().transform;
            markers[i].name = string.Format("{0} marker{1}", transform.name, i);
        }
    }


    /**
     * Update the positions of the pointer trackers marking the mesh vertices.
     *
     * @param   null
     * @return  null
    **/
    void Update()
    {
        // make a locally scoped copy of mesh vertices -- if we directly access
        // vertices using meshFilter.mesh.vertices[i] inside of the for() loop
        // we create a tremendous amount of GC cleanup (~113MB...)
        Vector3[] vertices = meshFilter.mesh.vertices;

        for (int i = 0; i < meshFilter.mesh.vertexCount; i++)
            markers[i].position = TransformVertex(vertices[i], transform);
    }


    /**
     * Modify a mesh vertex position based on the mesh's parent transform
     * (position, scale, and rotation).
     *
     * @param   Vector3     vertex position on unmodified mesh
     * @param   Transform   parent transform of the mesh
     * @return  Vector3     modified vertex position relative to transform
    **/
    private Vector3 TransformVertex(Vector3 vertex, Transform parent)
    {
        return parent.localToWorldMatrix.MultiplyPoint3x4(vertex);
    }
}
