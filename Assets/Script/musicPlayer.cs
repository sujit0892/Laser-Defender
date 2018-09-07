using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicPlayer : MonoBehaviour {

    // Use this for initialization

    void Awake()
    {
        SetupSingleton();
    }

    private void SetupSingleton()
    {
        if(FindObjectsOfType(GetType()).Length>1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
