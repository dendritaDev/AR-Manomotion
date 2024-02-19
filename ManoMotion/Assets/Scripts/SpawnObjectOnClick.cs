using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectOnClick : MonoBehaviour
{
    
    [SerializeField] private GameObject arObject;

    // Update is called once per frame
    void Update()
    {
        ManomotionManager.Instance.ShouldCalculateGestures(true);

        GestureInfo gestureInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;

        ManoGestureTrigger currentGesture = gestureInfo.mano_gesture_trigger;

        if(currentGesture == ManoGestureTrigger.CLICK)
        {
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        ManomotionManager.Instance.ShouldCalculateSkeleton3D(true);

        TrackingInfo trackingInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;

        float depthEstimation = trackingInfo.depth_estimation;

        //[8] es la joint de la punta del dedo indice
        Vector3 jointPosition = ManoUtils.Instance.CalculateNewPositionSkeletonJointDepth(new Vector3(trackingInfo.skeleton.joints[8].x, trackingInfo.skeleton.joints[8].y, trackingInfo.skeleton.joints[8].z), depthEstimation);

        Instantiate(arObject, jointPosition, Quaternion.identity);

        Handheld.Vibrate();
    }
}
