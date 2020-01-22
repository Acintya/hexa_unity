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
    private List<float> IKPResults;
    private List<float> FKPParams;
    private HexaAnimationController _hexaAnimationController;

    private void Start()
    {
        _hexaRequester = new HexaRequester();
        _hexaRequester.Start();
        IKPParams = new List<float>();
        IKPResults = new List<float>();

        _hexaAnimationController = HexaRobot.GetComponent<HexaAnimationController>();
    }

    private void OnDestroy()
    {
        _hexaRequester.Stop();
    }

    [ContextMenu("IKP")]
    public void IKP()
    {
        try
        {
            Debug.Log("IKP");
            UpdateIKPParameters();
            IKPResults = _hexaRequester.ReqIKP(IKPParams);
            UpdateIKPResults();

            _hexaAnimationController.Animate(IKPParams, IKPResults);
        }
        catch (System.Exception e)
        {
            Infobox.text += e.Message;
        }
        
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
        for (int i = 0; i < IKPResults.Count; i++)
        {
            IKPResOutput[i].text = IKPResults[i].ToString();
        }
    }

}