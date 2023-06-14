using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GetVectorFromMouse
{
    public static Ray ray;


    public static Vector3 Take()
    {
        // Cast a ray from the mouse position into the scene
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        int groundLayerMask = LayerMask.GetMask("Ground"); // Set the layer name "Ground" here

        if (Physics.Raycast(ray, out hit, Mathf.Infinity,groundLayerMask))
        {
            float x = hit.point.x;
            float y = hit.point.y;
            float z = hit.point.z;
            var _pointToMove = new Vector3(x, y, z);
            return _pointToMove;
        }
        return Vector3.zero;
    }
}
