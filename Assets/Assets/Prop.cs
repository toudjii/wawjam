using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    [SerializeField] internal Outline myOutline;
    [SerializeField] internal Transform myMeshT;
    [SerializeField] MeshFilter myMeshFilter;
    [SerializeField] MeshCollider myMeshCollider;

    internal Vector3 meshOriginalRelPos;
    internal bool isTarget = false;

    // Start is called before the first frame update
    void Start()
    {
        myMeshCollider.sharedMesh = myMeshFilter.sharedMesh;
        meshOriginalRelPos = myMeshT.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateOutline(Outline activeOutline)
    {
        if (activeOutline == myOutline)
        {
            myOutline.enabled = true;
        }
        else
        {
            myOutline.enabled = false;
        }
    }

    private void OnEnable()
    {
        SC_Interactor.UpdateOutlines += UpdateOutline;
    }

    private void OnDisable()
    {
        SC_Interactor.UpdateOutlines -= UpdateOutline;
    }
}
