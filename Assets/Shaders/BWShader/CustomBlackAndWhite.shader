Shader "Custom/BlackAndWhiteTransition"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TransitionProgress ("Transition Progress", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags
        { 
            "RenderType"="Opaque"
            "Queue"="Geometry"
        }

        LOD 100

        Pass
        {
            Tags
            {
                "IgnoreProjector"="[StayColorful]"
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionHCS : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _TransitionProgress;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(float4(v.positionOS.xyz, 1.0));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);
                float2 center = float2(0.5, 0.5);
                float2 uv = i.uv;
                float2 distanceVec = center - uv;
                float distance = length(distanceVec);
                float gray = dot(col.rgb, float3(0.299, 0.587, 0.114));
                half4 grayColor = half4(gray, gray, gray, col.a);

                // Siyah-beyaz dönüþümü
                float transitionSharpness = 1.2;
                float smoothstepValue = smoothstep(0, transitionSharpness, _TransitionProgress * 2 - distance * 2);
                return lerp(grayColor, col, smoothstepValue);
            }

            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
