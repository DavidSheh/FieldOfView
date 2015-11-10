using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    private EventSystem _eventSystem;
    private MeshList _meshList;
    private FOV _fov;
    private List<Vector3> _editPoints;

    private GameObject _meshParent;
    private GameObject _editPointsParent;

    private Transform _transform;


    public GameObject editPointPrefab;
    public GameObject meshPrefab;
    public LayerMask meshLayer;


    void Awake()
    {
        _transform = transform;

        _meshParent = GameObject.Find("Meshes");
        _editPointsParent = GameObject.Find("EditPoints");

        _editPoints = new List<Vector3>();
        _meshList = new MeshList(_meshParent, meshPrefab);
        _fov = new FOV(_transform);
        _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }


	void Start ()
    {
        StartMesh();
        AddMeshPoint(new Vector3(-5f, 1f, 0f));
        AddMeshPoint(new Vector3(-3f, 2f, 0f));
        AddMeshPoint(new Vector3(-1.5f, 3f, 0f));
        AddMeshPoint(new Vector3(-4f, 4f, 0f));
        EndMesh();
        StartMesh();
        AddMeshPoint(new Vector3(4f, 4f, 0f));
        AddMeshPoint(new Vector3(1.5f, 3f, 0f));
        AddMeshPoint(new Vector3(3f, 2f, 0f));
        AddMeshPoint(new Vector3(5f, 1f, 0f));
        EndMesh();
	}
	

	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Application.Quit(); 
        }

        if (UIManager.instance.inEditMode)
        {
            // Edit mode
            StartCoroutine("EditMode");
        }
        else
        {
            // FOV mode
            StartCoroutine("FOVMode");
        }        
	}


    IEnumerator EditMode()
    {
        if ((Application.platform == RuntimePlatform.WindowsPlayer) || (Application.platform == RuntimePlatform.WindowsEditor))
        {
            if (Input.GetMouseButtonDown(0) && (!_eventSystem.IsPointerOverGameObject()))
            {
                AddMeshPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began) && (!_eventSystem.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
            {
                AddMeshPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }


        yield return null;
    }


    IEnumerator FOVMode()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f);

        _fov.Raycast(_meshList);
        _fov.DrawMesh();
        _fov.DrawLines();

        yield return null;
    }


    public void StartMesh()
    {
        _editPoints.Clear();
    }


    public void AddMeshPoint(Vector3 point)
    {
        _editPoints.Add(point);
        GameObject newPoint = (GameObject)Instantiate(editPointPrefab, point, Quaternion.identity);
        newPoint.transform.position = new Vector3(point.x, point.y, -1f);
        newPoint.transform.SetParent(GameObject.Find("EditPoints").transform);
    }


    public void EndMesh()
    {
        if (_editPoints.Count > 2)
        {
            _meshList.CreateMesh(_editPoints);
        }

        _editPoints.Clear();

        // Destroy EditPoints
        var children = new List<GameObject>();
        foreach (Transform child in _editPointsParent.transform)
            children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }


    public void ResetAll()
    {
        _meshList.Reset();
    }
}
