using UnityEngine;
using System.Collections.Generic;


class MeshList
{
    public List<MeshItem> meshes { get; private set; }

    private GameObject _meshPrefab;
    private GameObject _meshParent;


    public MeshList(GameObject meshParent, GameObject meshPrefab)
    {
        meshes = new List<MeshItem>();
        _meshPrefab = meshPrefab;
        _meshParent = meshParent;
    }


    public void CreateMesh(List<Vector3> points)
    {
        MeshItem newMesh = new MeshItem();
        for (int i = 0; i < points.Count; i++)
            newMesh.AddPoint(points[i]);
        meshes.Add(newMesh);
        _GenerateObject(meshes[meshes.Count - 1]);
    }


    public void Reset()
    {
        meshes.Clear();

        // Destroy MeshObjects
        var children = new List<GameObject>();
        foreach (Transform child in _meshParent.transform)
            children.Add(child.gameObject);
        children.ForEach(child => Object.Destroy(child));
    }


    private void _GenerateObject(MeshItem mesh)
    {
        GameObject newObject = (GameObject)Object.Instantiate(_meshPrefab);
        newObject.transform.SetParent(_meshParent.transform);

        MeshFilter _meshFilter = newObject.GetComponent<MeshFilter>();
        PolygonCollider2D _collider = newObject.GetComponent<PolygonCollider2D>();

        List<Vector2> _uvs = new List<Vector2>();
        List<int> _triangles = new List<int>();
        Mesh _mesh = new Mesh();

        for (int i = 0; i < mesh.points.Count; i++)
        {
            _uvs.Add(new Vector2(mesh.points[i].x, mesh.points[i].y));
        }

        for (int i = 0; i < (mesh.points.Count - 2); i++)
        {
            _triangles.Add(0);
            _triangles.Add(i + 2);
            _triangles.Add(i + 1);
        }

        _mesh.vertices = mesh.points.ToArray();
        _mesh.uv = _uvs.ToArray();
        _mesh.triangles = _triangles.ToArray();
        _mesh.RecalculateNormals();

        _meshFilter.mesh = _mesh;

        _collider.points = _uvs.ToArray();
    }



}
