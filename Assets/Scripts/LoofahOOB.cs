using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoofahOOB : MonoBehaviour
{
    private float maxYDistance = -6f;
    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < maxYDistance)
            Destroy(this.gameObject);
            
    }
}
