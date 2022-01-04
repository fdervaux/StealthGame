using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
    public GameObject player;

    public float _angleVision = 30;
    public float _distanceVision = 15;
    public int _subdivisions = 30;

    public LayerMask obstacleLayerMask;

    private Mesh _mesh;

    private MeshRenderer _meshRenderer;


    public void setColor(Color color)
    {
        _meshRenderer.material.SetColor("_BaseColor", color);
    }

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        _mesh = new Mesh();
        UpdateMesh(_subdivisions, Mathf.Tan(_angleVision * 0.5f * Mathf.Deg2Rad) * _distanceVision, _distanceVision);
        GetComponent<MeshFilter>().sharedMesh = _mesh;

    }

    void Update()
    {
        UpdateMesh(_subdivisions, Mathf.Tan(_angleVision * 0.5f * Mathf.Deg2Rad) * _distanceVision, _distanceVision);
    }


    public bool playerIsVisble()
    {
        Vector3 originToPlayerVector = player.transform.position + transform.up * 1.2f - transform.position;

        float angle = Vector3.Angle(originToPlayerVector, Vector3.ProjectOnPlane(transform.right, Vector3.up));

        if (originToPlayerVector.magnitude > _distanceVision)
            return false;

        if (angle > _angleVision * 0.5f)
            return false;


        RaycastHit hit;
        if (Physics.Raycast(transform.position, originToPlayerVector.normalized, out hit, _distanceVision, obstacleLayerMask))
        {
            if (hit.transform.gameObject != player)
                return false;
        }

        return true;
    }

    private void UpdateMesh(int subdivisions, float radius, float height)
    {

        Vector3[] vertices = new Vector3[subdivisions + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[(subdivisions * 2) * 3];

        vertices[0] = new Vector3(0f, 0f, 0f);
        uv[0] = new Vector2(0f, 1f);
        for (int i = 0, n = subdivisions - 1; i < subdivisions; i++)
        {
            float ratio = (float)i / n;
            float r = ratio * (Mathf.PI * 2f);
            float x = Mathf.Cos(r) * radius / 4;
            float z = Mathf.Sin(r) * radius;
            vertices[i + 1] = new Vector3(height, x, z);

            RaycastHit hit;

            float rayMaxDistance = height / Mathf.Cos(_angleVision * 0.5f * Mathf.Deg2Rad);

            //Debug.DrawLine(transform.position, transform.TransformPoint( vertices[i + 1]),Color.green);
            float distance = rayMaxDistance;

            if (Physics.Raycast(transform.position, transform.TransformDirection(vertices[i + 1]), out hit, rayMaxDistance, obstacleLayerMask))
            {
                vertices[i + 1] = transform.InverseTransformPoint(hit.point);
                distance = hit.distance;
                //Debug.DrawLine(transform.position, hit.point, Color.red );
            }
            uv[i + 1] = new Vector2(0, 1 - distance / rayMaxDistance);
        }

        // construct sides
        int bottomOffset = subdivisions * 3;
        for (int i = 0, n = subdivisions - 1; i < n; i++)
        {
            int offset = i * 3 + bottomOffset;
            triangles[offset] = i + 1;
            triangles[offset + 1] = 0;
            triangles[offset + 2] = i + 2;
        }

        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.triangles = triangles;
        _mesh.RecalculateBounds();
        _mesh.RecalculateNormals();
    }
}
