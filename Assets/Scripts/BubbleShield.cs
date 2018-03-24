using UnityEngine;
using System.Collections.Generic;

public class BubbleShield : MonoBehaviour {

    #region Variables
    #endregion

    [SerializeField]
    int maximumRippleCount = 10;

    [SerializeField]
    float rippleGrowSpeed = 1;

    [SerializeField]
    float maxRippleSize = 2;

    private Material material;

    private List<Vector4> ripples;
    private int currentRippleIndex = 0;

    void Awake()
    {
        ripples = new List<Vector4>(maximumRippleCount);

        for (int i = 0; i < maximumRippleCount; i++)
            ripples.Add(Vector4.zero);

        material = GetComponent<Renderer>().material;
        material.SetVectorArray("_Ripples", ripples);
        material.SetFloat("_MaxRippleSize", maxRippleSize);
    }

    public void AddImpact(Vector3 position)
    {
        ripples[currentRippleIndex] = new Vector4(position.x, position.y, position.z, 0);

        currentRippleIndex = (currentRippleIndex + 1) % maximumRippleCount;
    }

    void Update()
    {
        for (int i = 0; i < ripples.Count; i++)
        {
            Vector4 ripple = ripples[i];

            if (ripple == Vector4.zero)
                continue;

            ripple.w += rippleGrowSpeed * Time.deltaTime;

            if (ripple.w > maxRippleSize)
                ripple = Vector4.zero;

            ripples[i] = ripple;
        }

        material.SetVectorArray("_Ripples", ripples);
    }
}
