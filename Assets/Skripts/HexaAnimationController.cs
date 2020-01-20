using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaAnimationController : MonoBehaviour
{
    public GameObject BaseOrigin;
    public GameObject ArrowPrefab;
    public List<Transform> UpperArms;

    const float R_B = 360f; //mm
    private float UnityUnitPerMm;

    // Start is called before the first frame update
    void Start()
    {
        ArrowPrefab.transform.localScale = new Vector3 (0.05f, 0.05f, 0.05f );
        GameObject BaseOriginXAxis = Instantiate(ArrowPrefab, BaseOrigin.transform.position, BaseOrigin.transform.rotation);
        BaseOriginXAxis.transform.rotation = Quaternion.Euler(BaseOrigin.transform.forward);
        BaseOriginXAxis.transform.rotation = Quaternion.Euler(0, 60, 0);

        GameObject BaseOriginYAxis = Instantiate(ArrowPrefab, BaseOrigin.transform.position, BaseOrigin.transform.rotation);
        BaseOriginYAxis.transform.rotation = Quaternion.Euler(BaseOrigin.transform.forward);
        BaseOriginYAxis.transform.rotation = Quaternion.Euler(0, -30, 0);

        GameObject BaseOriginZAxis = Instantiate(ArrowPrefab, BaseOrigin.transform.position, Quaternion.Euler(BaseOrigin.transform.up));
        BaseOriginZAxis.transform.rotation = Quaternion.Euler(-90, 0, 0);

        UpperArms[0].Rotate(new Vector3(0, 0, 1), 30);
        UpperArms[5].Rotate(new Vector3(0, 0, 1), -30);

        UpperArms[1].Rotate(new Vector3(1, 0, 0), 30);
        UpperArms[2].Rotate(new Vector3(1, 0, 0), -30);

        UpperArms[3].Rotate(new Vector3(1, 0, 0), 30);
        UpperArms[4].Rotate(new Vector3(1, 0, 0), -30);

        float R_B_Dist_Unity = Vector3.Distance(BaseOrigin.transform.position, new Vector3(UpperArms[0].position.x, BaseOrigin.transform.position.y, UpperArms[0].position.z));
        UnityUnitPerMm = R_B_Dist_Unity / R_B;
    }

    // Update is called once per frame
    void Update()
    {
        //BaseOrigin.transform.lo
    }
}
