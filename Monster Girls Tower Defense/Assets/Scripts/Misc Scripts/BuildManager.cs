using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    public GameObject CurrentBuildableObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one BuildManager cannot exist in the scene.");
            return;
        }

        Instance = this;
    }
}
