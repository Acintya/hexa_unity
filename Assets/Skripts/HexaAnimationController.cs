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

        float R_B_Dist_Unity = Vector3.Distance(BaseOrigin.transform.localPosition, new Vector3(UpperArms[0].localPosition.x, BaseOrigin.transform.localPosition.y, UpperArms[0].localPosition.z));
        UnityUnitPerMm = R_B_Dist_Unity / R_B;
    }

    public void Animate(List<float> platformPose, List<float> acuatorAngles)
    {
        if (platformPose.Count != 6 || acuatorAngles.Count != 6)
            return;
        //Platform.transform.localPosition = (new Vector3(platformPose[1], - platformPose[2], platformPose[0]) + BaseOrigin.transform.localPosition) * UnityUnitPerMm;
        LeanTween.moveLocal(Platform, (new Vector3(platformPose[1], -platformPose[2], platformPose[0]) + BaseOrigin.transform.localPosition) * UnityUnitPerMm, 2f);
        //Platform.transform.localEulerAngles = new Vector3(platformPose[3], 120 - platformPose[4], platformPose[5]);
        LeanTween.rotateLocal(Platform, new Vector3(platformPose[3], 120 - platformPose[4], platformPose[5]), 2f);

        //UpperArms[0].localEulerAngles = new Vector3(0, 30, acuatorAngles[0] - 45);
        LeanTween.rotateLocal(UpperArms[0].gameObject, new Vector3(0, 30, acuatorAngles[0] - 45), 2f);
        //UpperArms[5].localEulerAngles = new Vector3(0, 30, acuatorAngles[5] - 45 );
        LeanTween.rotateLocal(UpperArms[5].gameObject, new Vector3(0, 30, acuatorAngles[5] - 45), 2f);

        //UpperArms[1].localEulerAngles = new Vector3(45 - acuatorAngles[1], 0, 0);
        LeanTween.rotateLocal(UpperArms[1].gameObject, new Vector3(45 - acuatorAngles[1], 0, 0), 2f);
        //UpperArms[2].localEulerAngles = new Vector3(45 - acuatorAngles[2], 0, 0);
        LeanTween.rotateLocal(UpperArms[2].gameObject, new Vector3(45 - acuatorAngles[2], 0, 0), 2f);

        //UpperArms[3].localEulerAngles = new Vector3(- 35 + acuatorAngles[3], 60, 0);
        LeanTween.rotateLocal(UpperArms[3].gameObject, new Vector3(-35 + acuatorAngles[3], 60, 0), 2f);
        //UpperArms[4].localEulerAngles = new Vector3(- 35 + acuatorAngles[4], 60, 0);
        LeanTween.rotateLocal(UpperArms[4].gameObject, new Vector3(-35 + acuatorAngles[4], 60, 0), 2f);

        for (int i = 0; i < PlatformSphJoints.Count; i++)
        {
            //LowerArms[i].position = PlatformSphJoints[i].position;
            LeanTween.move(LowerArms[i].gameObject, PlatformSphJoints[i].position, 2f);
            LowerArms[i].LookAt(KDJoints[i].position, LowerArms[i].forward);
        }
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
