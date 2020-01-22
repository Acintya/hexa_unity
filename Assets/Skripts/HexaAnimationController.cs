using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaAnimationController : MonoBehaviour
{
    public GameObject BaseOrigin;
    public GameObject Platform;
    public GameObject ArrowPrefab;
    public List<Transform> UpperArms;
    public List<Transform> LowerArms;
    public List<Transform> KDJoints;
    public List<Transform> PlatformSphJoints;

    const float R_B = 127.1f; //mm
    private float UnityUnitPerMm;

    // Start is called before the first frame update
    void Start()
    {
        ArrowPrefab.transform.localScale = new Vector3 (0.05f, 0.05f, 0.05f );
        GameObject BaseOriginXAxis = Instantiate(ArrowPrefab, BaseOrigin.transform.position, BaseOrigin.transform.localRotation);
        BaseOriginXAxis.transform.rotation = Quaternion.Euler(BaseOrigin.transform.forward);
        BaseOriginXAxis.transform.rotation = Quaternion.Euler(0, 60, 0);

        GameObject BaseOriginYAxis = Instantiate(ArrowPrefab, BaseOrigin.transform.position, BaseOrigin.transform.rotation);
        BaseOriginYAxis.transform.rotation = Quaternion.Euler(BaseOrigin.transform.forward);
        BaseOriginYAxis.transform.rotation = Quaternion.Euler(0, -30, 0);

        GameObject BaseOriginZAxis = Instantiate(ArrowPrefab, BaseOrigin.transform.position, Quaternion.Euler(BaseOrigin.transform.up));
        BaseOriginZAxis.transform.rotation = Quaternion.Euler(-90, 0, 0);

        //UpperArms[0].Rotate(new Vector3(0, 0, 1), 30);
        //UpperArms[5].Rotate(new Vector3(0, 0, 1), -30);

        //UpperArms[1].Rotate(new Vector3(1, 0, 0), 30);
        //UpperArms[2].Rotate(new Vector3(1, 0, 0), -30);

        //UpperArms[3].Rotate(new Vector3(1, 0, 0), 30);
        //UpperArms[4].Rotate(new Vector3(1, 0, 0), -30);

        float R_B_Dist_Unity = Vector3.Distance(BaseOrigin.transform.localPosition, new Vector3(UpperArms[0].localPosition.x, BaseOrigin.transform.localPosition.y, UpperArms[0].localPosition.z));
        UnityUnitPerMm = R_B_Dist_Unity / R_B;
    }

    public void Animate(List<float> platformPose, List<float> acuatorAngles)
    {
        if (platformPose.Count != 6 || acuatorAngles.Count != 6)
            return;
        Platform.transform.localPosition = (new Vector3(platformPose[1], - platformPose[2], platformPose[0]) + BaseOrigin.transform.localPosition) * UnityUnitPerMm;
        Debug.Log(Platform.transform.localPosition);
        Platform.transform.localEulerAngles = new Vector3(platformPose[3], 120 - platformPose[4], platformPose[5]);

        UpperArms[0].localEulerAngles = new Vector3(0, 30, acuatorAngles[0] - 45);
        UpperArms[5].localEulerAngles = new Vector3(0, 30, acuatorAngles[5] - 45 );

        UpperArms[1].localEulerAngles = new Vector3(acuatorAngles[1] - 45, 0, 0);
        UpperArms[2].localEulerAngles = new Vector3(acuatorAngles[2] - 45, 0, 0);

        UpperArms[3].localEulerAngles = new Vector3(- 35 + acuatorAngles[3], 60, 0);
        UpperArms[4].localEulerAngles = new Vector3(- 35 + acuatorAngles[4], 60, 0);

        for (int i = 0; i < PlatformSphJoints.Count; i++)
        {
            LowerArms[i].position = PlatformSphJoints[i].position;
            LowerArms[i].LookAt(KDJoints[i].position, LowerArms[i].up);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //BaseOrigin.transform.lo
    }
}
