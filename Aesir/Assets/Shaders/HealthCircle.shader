// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-1905-OUT,alpha-2015-OUT,clip-2015-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:31808,y:31679,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.04411763,c3:0.1825556,c4:1;n:type:ShaderForge.SFN_Lerp,id:6109,x:32206,y:32068,varname:node_6109,prsc:2|A-7241-RGB,B-2175-RGB,T-701-OUT;n:type:ShaderForge.SFN_Color,id:2175,x:31406,y:31807,ptovrint:False,ptlb:Color_copy,ptin:_Color_copy,varname:_Color_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.1999999,c2:1,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:1905,x:32468,y:32547,varname:node_1905,prsc:2|A-6109-OUT,B-8450-OUT;n:type:ShaderForge.SFN_Slider,id:701,x:30920,y:31927,ptovrint:False,ptlb:Health,ptin:_Health,varname:node_701,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.7179487,max:1;n:type:ShaderForge.SFN_RemapRange,id:2109,x:30502,y:32481,varname:node_2109,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-5749-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:5749,x:30112,y:32392,varname:node_5749,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Length,id:8758,x:30734,y:32586,varname:node_8758,prsc:2|IN-2109-OUT;n:type:ShaderForge.SFN_Floor,id:9303,x:30959,y:32753,varname:node_9303,prsc:2|IN-8758-OUT;n:type:ShaderForge.SFN_OneMinus,id:8296,x:31244,y:32767,varname:node_8296,prsc:2|IN-9303-OUT;n:type:ShaderForge.SFN_Add,id:3120,x:31412,y:32554,varname:node_3120,prsc:2|A-6300-OUT,B-8758-OUT;n:type:ShaderForge.SFN_Vector1,id:6300,x:31272,y:32487,varname:node_6300,prsc:2,v1:0.2;n:type:ShaderForge.SFN_Floor,id:3783,x:31648,y:32543,varname:node_3783,prsc:2|IN-3120-OUT;n:type:ShaderForge.SFN_Multiply,id:8450,x:32185,y:32578,varname:node_8450,prsc:2|A-9709-OUT,B-3783-OUT,C-8296-OUT;n:type:ShaderForge.SFN_ArcTan2,id:640,x:31028,y:32269,varname:node_640,prsc:2,attp:2|A-8264-G,B-8264-R;n:type:ShaderForge.SFN_ComponentMask,id:8264,x:30779,y:32269,varname:node_8264,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-2109-OUT;n:type:ShaderForge.SFN_OneMinus,id:9709,x:31885,y:32242,varname:node_9709,prsc:2|IN-6075-OUT;n:type:ShaderForge.SFN_Ceil,id:6075,x:31652,y:32242,varname:node_6075,prsc:2|IN-966-OUT;n:type:ShaderForge.SFN_Subtract,id:966,x:31433,y:32242,varname:node_966,prsc:2|A-7532-OUT,B-701-OUT;n:type:ShaderForge.SFN_OneMinus,id:7532,x:31174,y:32269,varname:node_7532,prsc:2|IN-640-OUT;n:type:ShaderForge.SFN_Multiply,id:2015,x:31975,y:32845,varname:node_2015,prsc:2|A-3783-OUT,B-8296-OUT;proporder:7241-2175-701;pass:END;sub:END;*/

Shader "Shader Forge/HealthCircle" {
    Properties {
        _Color ("Color", Color) = (1,0.04411763,0.1825556,1)
        _Color_copy ("Color_copy", Color) = (0.1999999,1,0,1)
        _Health ("Health", Range(0, 1)) = 0.7179487
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _Color;
            uniform float4 _Color_copy;
            uniform float _Health;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float2 node_2109 = (i.uv0*2.0+-1.0);
                float node_8758 = length(node_2109);
                float node_3783 = floor((0.2+node_8758));
                float node_8296 = (1.0 - floor(node_8758));
                float node_2015 = (node_3783*node_8296);
                clip(node_2015 - 0.5);
////// Lighting:
////// Emissive:
                float2 node_8264 = node_2109.rg;
                float3 emissive = (lerp(_Color.rgb,_Color_copy.rgb,_Health)*((1.0 - ceil(((1.0 - ((atan2(node_8264.g,node_8264.r)/6.28318530718)+0.5))-_Health)))*node_3783*node_8296));
                float3 finalColor = emissive;
                return fixed4(finalColor,node_2015);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float2 node_2109 = (i.uv0*2.0+-1.0);
                float node_8758 = length(node_2109);
                float node_3783 = floor((0.2+node_8758));
                float node_8296 = (1.0 - floor(node_8758));
                float node_2015 = (node_3783*node_8296);
                clip(node_2015 - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
