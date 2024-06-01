using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodPickupable : Pickupables
{
    [SerializeField]
    List<Mesh> meshes;

    MeshFilter meshFilter;

    private void Awake() {
        meshFilter = GetComponentInChildren<MeshFilter>();
    }

    private void OnEnable() {
        meshFilter.mesh = meshes[Random.Range(0, meshes.Count)];
    }
}
