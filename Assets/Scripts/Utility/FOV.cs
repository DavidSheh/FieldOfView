using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

class FOV
{
    private Transform _player;
    private RectTransform _toolbar;
    private Vector3[] _Edges;
    private Vector2 _screenBounds;

    private List<Vector3> colPoints = new List<Vector3>();
    private List<float> colAngles = new List<float>();

    private MeshFilter _meshFilter;
    private List<Vector2> _uvs;
    private List<int> _triangles;
    private Mesh _mesh;

    public FOV(Transform player)
    {
        _player = player;

        _screenBounds.x = (Camera.main.orthographicSize * 2) * ((float)Screen.width / Screen.height);
        _screenBounds.y = (Camera.main.orthographicSize * 2);

        // Inicijaliziranje varijabli za mesh
        _meshFilter = GameObject.Find("FOV").GetComponent<MeshFilter>();
        _uvs = new List<Vector2>();
        _triangles = new List<int>();
        _mesh = new Mesh();


        // Generiranje rubova
        _Edges = new Vector3[4];
        _Edges[0] = new Vector3((float)-_screenBounds.x / 2, (float)-_screenBounds.y / 2);
        _Edges[1] = new Vector3((float)_screenBounds.x / 2, (float)-_screenBounds.y / 2);
        _Edges[2] = new Vector3((float)_screenBounds.x / 2, (float)_screenBounds.y / 2);
        _Edges[3] = new Vector3((float)-_screenBounds.x / 2, (float)_screenBounds.y / 2);
    }


    public void Raycast(MeshList meshList)
    {
        colPoints.Clear();
        colAngles.Clear();

        Vector3 hitPoint;
        bool collided;
        Vector3 newDirection;
        Vector3 point;

        // Prvo idemo po rubovima
        for (int i = 0; i < _Edges.Length; i++)
        {
            hitPoint = _GetCollisionPoint(_Edges[i], out collided);
            colPoints.Add(hitPoint);
            colAngles.Add(_CalculateAngle(hitPoint));
        }


        // Onda idemo po poligonima
        for (int i = 0; i < meshList.meshes.Count; i++)
        {
            for (int j = 0; j < meshList.meshes[i].points.Count; j++)
            {
                point = meshList.meshes[i].points[j];

                hitPoint = _GetCollisionPoint(point, out collided);
                colPoints.Add(hitPoint);
                colAngles.Add(_CalculateAngle(hitPoint));

                // Za svaku točku u poligonu tražimo i lijevo/desno (TODO: samo za rubne!)
                newDirection = Quaternion.AngleAxis(.1f, Vector3.forward) * point;
                hitPoint = _GetCollisionPoint(newDirection, out collided);
                colPoints.Add(hitPoint);
                colAngles.Add(_CalculateAngle(hitPoint));

                newDirection = Quaternion.AngleAxis(-.1f, Vector3.forward) * point;
                hitPoint = _GetCollisionPoint(newDirection, out collided);
                colPoints.Add(hitPoint);
                colAngles.Add(_CalculateAngle(hitPoint));
      
            }
        }


        // Sortiramo liste po kutevima
        for (int i = 0; i < colAngles.Count - 1; i++)
        {
            for (int j = i + 1; j < colAngles.Count; j++)
            {
                if (colAngles[j] < colAngles[i])
                {
                    Vector3 temp = colPoints[i];
                    colPoints[i] = colPoints[j];
                    colPoints[j] = temp;

                    float tempF = colAngles[i];
                    colAngles[i] = colAngles[j];
                    colAngles[j] = tempF;
                }
            }
        }

        // Dodajemo na prvo mjesto i poziciju igrača
        colPoints.Insert(0, _player.position);
        colAngles.Insert(0, 0f);
    }


    public void DrawMesh()
    {
        _meshFilter.mesh = null;

        _uvs.Clear();
        _triangles.Clear();

        for (int i = 0; i < colPoints.Count; i++)
        {
            _uvs.Add(new Vector2(colPoints[i].x, colPoints[i].y));
        }

        for (int i = 0; i < (colPoints.Count - 2); i++)
        {
            _triangles.Add(i + 2);
            _triangles.Add(i + 1);
            _triangles.Add(0);
        }

        // Na kraju dodajemo i završni trokut
        _triangles.Add(1);
        _triangles.Add(colPoints.Count - 1);
        _triangles.Add(0);


        _mesh.vertices = colPoints.ToArray();
        _mesh.uv = _uvs.ToArray();
        _mesh.triangles = _triangles.ToArray();
        _mesh.RecalculateNormals();
        
        _meshFilter.mesh = _mesh;
    }


    public void DrawLines()
    {
        Color _color;

        for (int i = 1; i < colPoints.Count; i++)
        {
            if (i == 1)
                _color = Color.green;
            else if (i == colPoints.Count - 1)
                _color = Color.red;
            else
                _color = Color.blue;

            Debug.DrawLine(_player.position, colPoints[i], _color);
        }
    }



    private float _CalculateAngle(Vector3 point)
    {
        //float angle = Vector3.Angle(_player.position, point);
        //Vector3 cross = Vector3.Cross(_player.position, point);

        float angle = Vector3.Angle(point - _player.position, Vector3.up);
        Vector3 cross = Vector3.Cross(point - _player.position, Vector3.up);

        if (cross.z > 0)
            angle = 360 - angle;
        
        return angle;
    }


    private Vector3 _GetCollisionPoint(Vector3 endPoint, out bool collided)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(_player.position, endPoint - _player.position, 50f);
        if (hitInfo)
        {
            collided = true;
            return hitInfo.point;
        }
        else
        {
            collided = false;
            return endPoint;
        }
    }
}
