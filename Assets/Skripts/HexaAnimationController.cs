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

    const float R_B = 119.435f; //mm
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

        for (int i = 0; i < PlatformSphJoints.Count; i++)
        {
            LowerArms[i].LookAt(KDJoints[i]);
        }

    }

    public void Animate(List<float> platformPose, List<float> acuatorAngles)
    {
        if (platformPose.Count != 6 || acuatorAngles.Count != 6)
        {
            Debug.LogError("Incorrect input of acuator angles.");
            return;
        }
        LeanTween.moveLocal(Platform, new Vector3(platformPose[1], -platformPose[2], platformPose[0]) * UnityUnitPerMm + BaseOrigin.transform.localPosition, 2f);
        LeanTween.rotateLocal(Platform, new Vector3(platformPose[4], 120 - platformPose[5], platformPose[3]), 2f);

        LeanTween.rotateLocal(UpperArms[0].gameObject, new Vector3(0, 30, acuatorAngles[0] - 20), 2f);
        LeanTween.rotateLocal(UpperArms[5].gameObject, new Vector3(0, 30, acuatorAngles[5] - 20), 2f);

        LeanTween.rotateLocal(UpperArms[1].gameObject, new Vector3(20 - acuatorAngles[1], 0, 0), 2f);
        LeanTween.rotateLocal(UpperArms[2].gameObject, new Vector3(20 - acuatorAngles[2], 0, 0), 2f);

        LeanTween.rotateLocal(UpperArms[3].gameObject, new Vector3(-20 + acuatorAngles[3], 60, 0), 2f);
        LeanTween.rotateLocal(UpperArms[4].gameObject, new Vector3(-20 + acuatorAngles[4], 60, 0), 2f);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < PlatformSphJoints.Count; i++)
        {
            LeanTween.move(LowerArms[i].gameObject, PlatformSphJoints[i], 0f);
            LowerArms[i].LookAt(KDJoints[i]);
        }

    }
}
