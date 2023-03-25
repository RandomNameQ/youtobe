using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LaserWayToPoint : MonoBehaviour
{


    [SerializeField]
    private NavMeshAgent _agent;
    private NavMeshPath _path;
    private Vector3 _startPos;
    private GameObject _cubePrefab;

    private LineRenderer _lineRenderer;
    private List<Vector3> _linePoints;

    public Material _material;

    [SerializeField]
    private List<CubeChain> _cubeChains = new List<CubeChain>();
    [SerializeField]
    private List<GameObject> _ways = new List<GameObject>();
    private Vector3 _rightClickPoint;

    private void Start()
    {

        _agent = _agent.GetComponent<NavMeshAgent>();
        _path = new NavMeshPath();

        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 0;
        _linePoints = new List<Vector3>();
    }

    private void UpdateLineRenderer()
    {
        _lineRenderer.positionCount = _linePoints.Count;
        _lineRenderer.SetPositions(_linePoints.ToArray());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                _rightClickPoint = hit.point;
                MakeWay(_rightClickPoint);
            }
        }
    }

    public void MakeWay(Vector3 endPos)
    {
        _startPos = transform.position;
        _agent.CalculatePath(endPos, _path);

        _linePoints.Clear();
        for (int i = 0; i < _path.corners.Length; i++)
        {
         
            _linePoints.Add(_path.corners[i]);
        }

        UpdateLineRenderer();

        // Create a new chain for this path
        CubeChain chain = new CubeChain();
        chain.name = "Chain " + _cubeChains.Count;

        for (int i = 0; i < _lineRenderer.positionCount - 1; i++)
        {
            // Get the start and end positions of this line segment
            Vector3 startPosition = _lineRenderer.GetPosition(i);
            Vector3 endPosition = _lineRenderer.GetPosition(i + 1);

            // Calculate the direction and distance of the line segment
            Vector3 direction = (endPosition - startPosition).normalized;
            float distance = Vector3.Distance(startPosition, endPosition);

            // Create a cube and add it to the chain
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = startPosition + direction * (distance / 2);
            cube.transform.localScale = new Vector3(0.1f, 0.1f, distance);
            if (direction == Vector3.zero)
            {
                cube.transform.rotation = Quaternion.identity;
            }
            else
            {
                cube.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
            chain.cubes.Add(cube);
            // TODO change create destroy cubes > pool
           /* Destroy(cube);*/
        }

        GameObject combinedObject = chain.CombineCubes();
        combinedObject.GetComponent<MeshRenderer>().material = _material;
        _cubeChains.Add(chain);
        _ways.Add(combinedObject);
    }

    [System.Serializable]
    public class CubeChain
    {
        public string name;
        public List<GameObject> cubes = new List<GameObject>();

        public GameObject CombineCubes()
        {
            GameObject combinedObject = new GameObject(name + " Combined");
            combinedObject.transform.position = Vector3.zero;
            combinedObject.transform.rotation = Quaternion.identity;

            MeshFilter[] meshFilters = new MeshFilter[cubes.Count];
            for (int i = 0; i < cubes.Count; i++)
            {
                meshFilters[i] = cubes[i].GetComponent<MeshFilter>();
            }

            CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length];
            for (int i = 0; i < meshFilters.Length; i++)
            {
                combineInstances[i].mesh = meshFilters[i].sharedMesh;
                combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);
            }

            MeshFilter combinedMeshFilter = combinedObject.AddComponent<MeshFilter>();
            combinedMeshFilter.mesh = new Mesh();
            combinedMeshFilter.mesh.CombineMeshes(combineInstances);
            combinedObject.SetActive(true);

            MeshRenderer combinedMeshRenderer = combinedObject.AddComponent<MeshRenderer>();
            if (cubes.Count > 0)
            {
                combinedMeshRenderer.sharedMaterial = cubes[0].GetComponent<MeshRenderer>().sharedMaterial;
            }

            return combinedObject;
        }
    }
}