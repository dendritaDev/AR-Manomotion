using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TryOnWatch : MonoBehaviour
{
    
    [SerializeField] private GameObject watch;
    [SerializeField] private GameObject[]watchSides = new GameObject[2]; //las dos partes que forman el reloj

    private GestureInfo gestureInfo;
    // Start is called before the first frame update
    void Start()
    {
        //con esto hacemos que la parte front del reloj este situada exactamente donde la parte back, y segun la mano mire hacia arriba o abajo mostraremos una parte del reloj u otra.
        Vector3 invertScale = new Vector3(-watchSides[1].transform.localScale.x, -watchSides[1].transform.localScale.y, -watchSides[1].transform.localScale.z);
        watchSides[1].transform.localScale = invertScale;
    }

    // Update is called once per frame
    void Update()
    {
        
        ManomotionManager.Instance.ShouldRunWristInfo(true);
        ManomotionManager.Instance.ShouldCalculateGestures(true);

        gestureInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;

        if(gestureInfo.mano_class != ManoClass.NO_HAND) //si se detecta mano
        {
            ShowWatch();

        }
        else
        {
            watch.transform.position = -Vector3.one;
        }

    }

    private void ShowWatch()
    {
        var wristInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.wristInfo;
        var depthEstimation = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.depth_estimation;

        Vector3 leftPoint = ManoUtils.Instance.CalculateNewPositionDepth(new Vector3(wristInfo.left_point.x, wristInfo.left_point.y, 0), depthEstimation);
        Vector3 rightPoint = ManoUtils.Instance.CalculateNewPositionDepth(new Vector3(wristInfo.right_point.x, wristInfo.right_point.y, 0), depthEstimation);

        Vector3 watchPosition = Vector3.Lerp(leftPoint, rightPoint, 0.5f);
        
        watch.transform.position = watchPosition; //situamos el reloj en el punto medio de los dos puntos (izquierda y derecho) de la muñeca
        watch.transform.LookAt(leftPoint);

        if(gestureInfo.hand_side != HandSide.Palmside) //dorso
        {
            ShowWatchSide(true);
        }
        else //palma
        {
            ShowWatchSide(false); 
        }

    }

    private void ShowWatchSide(bool isFront)
    {
        watchSides[0].SetActive(isFront);
        watchSides[1].SetActive(!isFront);
    }
}
