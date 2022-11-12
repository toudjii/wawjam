using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Interactor : MonoBehaviour
{
    [SerializeField] SC_FPSController scFpsController;
    [SerializeField] Camera cam;
    [SerializeField] LayerMask raycastLayer;
    [SerializeField] Transform propHolder;

    internal static System.Action<Outline> UpdateOutlines;
    internal Prop activeProp;
    bool isHoldingProp = false;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (!isHoldingProp)
        {
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, raycastLayer))
            {
                activeProp = hit.collider.gameObject.transform.parent.gameObject.GetComponent<Prop>();
            }
            else
            {
                activeProp = null;
            }
        }
        
        if (activeProp != null)
        {
            if (!isHoldingProp)
            {
                UpdateOutlines?.Invoke(activeProp.myOutline);
            }
            else
            {
                UpdateOutlines?.Invoke(null);
            }
            
            if (Input.GetButtonDown("Interact"))
            {
                if (!isHoldingProp)
                {
                    EnterHoldingProp();
                }
                else
                {
                    ExitHoldingProp();
                }
            }

            if (Input.GetButtonDown("PickHeldProp") && isHoldingProp)
            {
                // determine if target props
            }
        }
        else
        {
            UpdateOutlines?.Invoke(null);
        }

        if (isHoldingProp)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            Vector3 rotVector;
            rotVector.y = -mouseX;
            rotVector.x = mouseY;
            rotVector.z = 0;

            //Vector3 currentRot = activeProp.myMeshT.rotation.eulerAngles;
            //activeProp.myMeshT.rotation = Quaternion.Euler(currentRot + rotVector);
            activeProp.myMeshT.RotateAround(activeProp.myMeshT.position, propHolder.up, -mouseX);
            activeProp.myMeshT.RotateAround(activeProp.myMeshT.position, propHolder.right, mouseY);
        }
    }

    private void EnterHoldingProp()
    {
        isHoldingProp = true;
        activeProp.myMeshT.position = propHolder.position;
        activeProp.myMeshT.parent = propHolder;
        activeProp.myMeshT.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        scFpsController.DisableControls();
        GameMgr.instance.ToggleHoldPropUI(true);
    }

    private void ExitHoldingProp()
    {
        isHoldingProp = false;
        scFpsController.EnableControls();
        GameMgr.instance.ToggleHoldPropUI(false);

        if (activeProp == null)
        {
            return;
        }
        
        activeProp.myMeshT.position = activeProp.transform.position + activeProp.meshOriginalRelPos;
        activeProp.myMeshT.rotation = activeProp.transform.rotation;
        activeProp.myMeshT.parent = activeProp.transform;
        activeProp.myMeshT.localScale = new Vector3(1f, 1f, 1f);
    }

    private void OnEnable()
    {
        GameMgr.PropsReshuffled += ExitHoldingProp;
    }

    private void OnDisable()
    {
        GameMgr.PropsReshuffled -= ExitHoldingProp;
    }
}
