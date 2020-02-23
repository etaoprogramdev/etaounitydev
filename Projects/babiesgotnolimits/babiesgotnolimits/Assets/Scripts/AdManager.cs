using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public GameObject adsErrorLog;
    static public bool noAds = false;
    public void Update()
    {
        if (noAds)
        {
            adsErrorLog.SetActive(true);
        } else
        {
            closeLog();
        }
    }

    public void closeLog()
    {
        adsErrorLog.SetActive(false);
    }
}
