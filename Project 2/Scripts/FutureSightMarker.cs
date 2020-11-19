using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutureSightMarker : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collider)
    {
        Debug.Log("Hit the future sight marker!");
        Destroy(gameObject);
        Level.GetInstance().DestroyFutureSightMarker();
    }
}
