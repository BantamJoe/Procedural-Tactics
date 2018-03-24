Shader "Bubble Shield"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _RimPower("Rim Power", Float) = 1
        _Thickness("Thickness", Float) = 0.01
        _DistortionStrength("DistortionStrength", Float) = 0.1
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent-1"
        }
 
        GrabPass
        {
            "_BackgroundTexture"
        }
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
           
            #include "UnityCG.cginc"
 

            struct appdata
            {
                float4 vertex : POSITION;
            };
 
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 localVertex : TEXCOORD1;
                float4 grabPos : TEXCOORD0;
            };
           
            v2f vert (appdata v)
            {
                v2f o;
                o.localVertex = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.vertex);
                return o;
            }
           
            float4 _Color; 
            float _Thickness;
 
            float4 _Ripples[10];
            float _MaxRippleSize;
 
            float _DistortionStrength;
            float _RimPower;
 
            sampler2D _BackgroundTexture;
 
            fixed4 frag (v2f input) : SV_Target
            {
                float pi = 3.1415;
 
                float3 sphereNormal = normalize(input.localVertex.xyz);
                float3 tangent = cross(sphereNormal, float3(0,1,0));
                float3 bitangent = cross(sphereNormal, tangent);
 
                float3 normal = float3(0, 0, 1);
 
                for (int i = 0; i < 10; i++)
                {
                    float angle = acos(dot(sphereNormal, normalize(_Ripples[i].xyz)));
 
                    float radius = _Ripples[i].w;
 
                    float t = saturate((angle - (radius - _Thickness)) / (_Thickness * 2));
 
                    float offset = sin(t * pi * 2);
 
                    float rippleStrength = 1 - (radius / _MaxRippleSize);
 
                    normal.y += offset * rippleStrength;
                }
 
                normal = normalize(normal);
                float3x3 m = transpose(float3x3(tangent, bitangent, sphereNormal));
                normal = mul(m, normal);
 
                float3 viewNormal = mul(UNITY_MATRIX_MV, normal);
 
                float4 uv = input.grabPos;
                uv.xy += viewNormal.xy * _DistortionStrength;
 
                float4 bgcolor = tex2Dproj(_BackgroundTexture, uv);
 
                float3 viewDir = ObjSpaceViewDir(input.localVertex);
                float NdotV = 1 - saturate(dot(normal, viewDir));
                float rimIntensity = pow(NdotV, _RimPower);
                float4 rim = _Color * rimIntensity;
 
                return bgcolor + rim;
 
                // return float4((normal * 0.5) + 0.5, 1);
            }
            ENDCG
        }
    }
}