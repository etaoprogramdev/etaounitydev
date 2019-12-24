using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeResetter : MonoBehaviour
{
    // Update is called once per frame
    public void Update()
    {
        Time.timeScale = 1f;
        Destroy(gameObject);
    }
}
