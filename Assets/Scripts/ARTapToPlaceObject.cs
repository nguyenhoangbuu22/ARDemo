using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using Unity.XR.CoreUtils;
using System;
using UnityEngine.XR.ARSubsystems;
using DG.Tweening;

public class ARTapToPlaceObject : MonoBehaviour
{

    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject placementIndicator;
    [SerializeField] private XROrigin sessionOrigin;
    [SerializeField] private ARRaycastManager raycastManager;

    private Pose placementPose;
    List<ARRaycastHit> hits = new();
    // Update is called once per frame

    void Update()
    {
        UpdatePlacementPose();
    }

    private void UpdatePlacementPose()
    {
        var camera = Camera.main;
        if (camera == null)
        {
            Debug.LogError("Main Camera is NULL");
            return;
        }
        var screenPosint = camera.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        //Debug.Log("B---------------- update");
        raycastManager.Raycast(screenPosint, hits, TrackableType.Planes);
        if (hits.Count > 0) { 
            //Debug.Log("B---------------- hits.Count: " + hits.Count);
            placementIndicator.SetActive(true);
            placementPose = hits[0].pose;
            Vector3 cammeraForwark = Camera.main.transform.forward;
            Vector3 cammeraBearing = new Vector3(cammeraForwark.x, 0, cammeraForwark.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cammeraBearing);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            //Debug.Log($"B---------------- poss  x:{placementPose.position.x} y:{placementPose.position.y} z:{placementPose.position.z}");
        }
        else
        {
            placementIndicator.SetActive(false);
            //Debug.Log("B---------------- placementIndicator: false");
        }
    }

    public void CreateCharacter()
    {
        GameObject characterObj = Instantiate(prefab, placementPose.position, placementPose.rotation);
        var sizeOrigin = characterObj.transform.localScale;
        characterObj.transform.localScale = Vector3.zero;
        characterObj.transform.DOScale(sizeOrigin, 0.5f).SetEase(Ease.OutBack);
        Character character = characterObj.GetComponent<Character>();
        GameControl.Ins.characterController.AddACharacter(character);
        GameControl.Ins.characterController.SelectCharacter(character);
    }
}
