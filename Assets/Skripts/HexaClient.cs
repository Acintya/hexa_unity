using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class HexaClient : MonoBehaviour
{
    public List<InputField> IKPParamInput;
    public List<InputField> IKPResOutput;
    public GameObject HexaRobot;
    public InputField Infobox;

    private HexaRequester _hexaRequester;
    private List<float> IKPParams;
    private IKPResult _IKPResult;
    private List<float> FKPParams;
    private HexaAnimationController _hexaAnimationController;

    private void Start()
    {
        _hexaRequester = new HexaRequester();
        _hexaRequester.Start();
        IKPParams = new List<float>();

        _hexaAnimationController = HexaRobot.GetComponent<HexaAnimationController>();
    }

    private void OnDestroy()
    {
        _hexaRequester.Stop();
    }

    [ContextMenu("IKP")]
    public void IKP()
    {
        //try
        //{
            Debug.Log("IKP");
            UpdateIKPParameters();
            _IKPResult = _hexaRequester.ReqIKP(IKPParams);
            Debug.Log(_IKPResult.ResState);
            if (_IKPResult.ResState == "OK")
            {
                UpdateIKPResults();
                _hexaAnimationController.Animate(IKPParams, _IKPResult.AcuatorAngels);
                Infobox.text = "Time of IKP calculation: " + _IKPResult.CalcTime.ToString() + " ms.";
            }
            else if (_IKPResult.ResState == "Failed")
            {
                Infobox.text = "Cannot calculate the IKP with the input parameters.";
                Debug.Log("failed");
            }
            else
                Infobox.text += _IKPResult.Message;
        //}
        //catch (System.Exception e)
        //{
        //    Infobox.text += e.Message;
        //}
        
    }

    private void UpdateIKPParameters()
    {
        IKPParams.Clear();
        for (int i = 0; i < IKPParamInput.Count; i++)
        {
            IKPParams.Add(float.Parse(IKPParamInput[i].text));
        }
    }

    private void UpdateIKPResults()
    {
        for (int i = 0; i < _IKPResult.AcuatorAngels.Count; i++)
        {
            IKPResOutput[i].text = _IKPResult.AcuatorAngels[i].ToString();
        }
    }

}