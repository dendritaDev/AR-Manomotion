using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryOnRing : MonoBehaviour
{

    [SerializeField] private GameObject ring;
    [SerializeField] private GameObject[] ringParts = new GameObject[2]; //las dos partes que forman el reloj

    private GestureInfo gestureInfo;
    private FingerInfo fingerInfo;
    // Start is called before the first frame update
    void Start()
    {
        ManomotionManager.Instance.ToggleFingerInfoFinger(4);

        //con esto hacemos que la parte front del anillo este situada exactamente donde la parte back, y segun la mano mire hacia arriba o abajo mostraremos una parte del anillo u otra.
        Vector3 invertScale = new Vector3(-ringParts[1].transform.localScale.x, -ringParts[1].transform.localScale.y, -ringParts[1].transform.localScale.z);
        ringParts[1].transform.localScale = invertScale;
    }

    // Update is called once per frame
    void Update()
    {
        ManomotionManager.Instance.ShouldRunFingerInfo(true);
        ManomotionManager.Instance.ShouldCalculateGestures(true);

        gestureInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;

        if(gestureInfo.mano_class == ManoClass.GRAB_GESTURE) //si hacemos grab gesture
        {
            ShowRing();
        }
        else
        {
            ring.transform.position = -Vector3.one;
        }
    }

    private void ShowRing()
    {
        ManomotionManager.Instance.ShouldCalculateSkeleton3D(true);

        fingerInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.fingerInfo;

        var deepEstimation = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.depth_estimation;

        var leftPoint = ManoUtils.Instance.CalculateNewPositionDepth(new Vector3(fingerInfo.left_point.x, fingerInfo.left_point.y, 0), deepEstimation);
        var rightPoint = ManoUtils.Instance.CalculateNewPositionDepth(new Vector3(fingerInfo.right_point.x, fingerInfo.right_point.y, 0), deepEstimation);

        Vector3 ringPos = Vector3.Lerp(leftPoint, rightPoint, 0.5f);

        ring.transform.position = ringPos; //situamos el anillo en el punto medio de los dos puntos (izquierda y derecho) del dedo 4
        ring.transform.LookAt(leftPoint);

        //adaptamos la escala del anillo a lo grande que sean los dedos
        float distanceFingerPoint = Vector3.Distance(fingerInfo.left_point, fingerInfo.right_point);
        ring.transform.localScale = new Vector3(distanceFingerPoint, distanceFingerPoint, distanceFingerPoint);

        if(gestureInfo.hand_side == HandSide.Palmside)
        {
            ShowRingPart(false);
        }
        else
        {
            ShowRingPart(true);
        }

    }

    private void ShowRingPart(bool isFront)
    {
        ringParts[0].SetActive(isFront);
        ringParts[1].SetActive(!isFront);
    }
}
