using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bar : MonoBehaviour {

    [SerializeField]
    private float fillAmount;

    [SerializeField]
    private Image content;

    public float maxValue;

    public float Value
    {
        set
        {
            fillAmount = Map(value, 0, maxValue, 0, 1);
        }
    }

    
    void Start()
    {
	
	}
	
	
	void Update() 
    {
        HandleBar();
    }

 
    private void HandleBar()
    {
        if (fillAmount != content.fillAmount)
        {
            content.fillAmount = fillAmount;
        }
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
