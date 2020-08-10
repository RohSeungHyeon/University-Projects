using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingSphere : MonoBehaviour {


    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
