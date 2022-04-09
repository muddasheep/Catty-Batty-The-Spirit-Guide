Shader "Custom/DrPostProcessor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [IntRange] _CameraMode ("CameraMode", Range (1,6)) = 0

        /* GameBoy Colors */
        _Darkest ("Darkest", color) = (0.0588235, 0.21961, 0.0588235)
        _Dark ("Dark", color) = (0.188235, 0.38431, 0.188235)
        _Ligt ("Light", color) = (0.545098, 0.6745098, 0.0588235)
        _Ligtest ("Lightest", color) = (0.607843, 0.7372549, 0.0588235)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            fixed _value;
            int _CameraMode;
            float4 _Darkest, _Dark, _Ligt, _Ligtest;

            fixed4 frag (v2f i) : SV_Target
            {
                if (_CameraMode == 2) {
                    // less contrast mode
                    fixed4 col2 = tex2D(_MainTex, i.uv);
                    col2.rgb = col2.rgb / 1.5;
                    return col2;
                }

                if (_CameraMode == 3) {
                    // sepia mode
                    fixed4 original = tex2D(_MainTex, i.uv);
 
                    // get intensity value (Y part of YIQ color space)
                    fixed Y = dot (fixed3(0.299, 0.587, 0.114), original.rgb);
 
                    // Convert to Sepia Tone by adding constant
                    fixed4 sepiaConvert = float4 (0.191, -0.054, -0.221, 0.0);
                    fixed4 output = sepiaConvert + Y;
                    output.a = original.a;
                    output = lerp(output, original, _value);
                    return output;
                }

                if (_CameraMode == 4) {
                    // dark mode - inverted colors, lower contras
                    fixed4 col = tex2D(_MainTex, i.uv);
                    col.rgb = 1 - col.rgb / 1.2;
                    return col;
                }

                if (_CameraMode == 5) {
                    // dark mode - inverted colors
                    fixed4 col = tex2D(_MainTex, i.uv);
                    col.rgb = 1 - col.rgb;
                    return col;
                }

                if (_CameraMode == 6) {
                    // GameBoy shader - github KumoKairo
                    float4 originalColor = tex2D(_MainTex, i.uv);

                    float luma = dot(originalColor.rgb, float3(0.2126, 0.7152, 0.0722));
                    float posterized = floor(luma * 4) / (4 - 1);
                    float lumaTimesThree = posterized * 3.0;

                    float darkest = saturate(lumaTimesThree);
                    float4 color = lerp(_Darkest, _Dark, darkest);

                    float light = saturate(lumaTimesThree - 1.0);
                    color = lerp(color, _Ligt, light);

                    float lightest = saturate(lumaTimesThree - 2.0);
                    color = lerp(color, _Ligtest, lightest);

                    return color;
                }

                fixed4 col1 = tex2D(_MainTex, i.uv);
                return col1;
            }
            ENDCG
        }
    }
}
