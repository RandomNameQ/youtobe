using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TreeStatController : MonoBehaviour
{
    public List<GameObject> NodesObjects = new();

    public void AddNode(GameObject newNode)
    {
        if (!NodesObjects.Contains(newNode))
        {
            NodesObjects.Add(newNode);
        }
    }

    public GameObject GetSecondLastNode()
    {

        return NodesObjects[NodesObjects.Count - 2];
    }

    private void Start()
    {
        NodesObjects.RemoveAll(node => node.name == "START");
        transform.position += new Vector3(0, 0, -0.1f);

    }

}
