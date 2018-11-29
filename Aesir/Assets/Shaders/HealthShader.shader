// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-6072-RGB,clip-4717-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:5118,x:32192,y:32623,ptovrint:False,ptlb:node_5118,ptin:_node_5118,varname:node_5118,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:eec20df1cf4fd384cbe803f61fad3424,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:6072,x:32461,y:32522,varname:node_2497,prsc:2,tex:eec20df1cf4fd384cbe803f61fad3424,ntxv:0,isnm:False|UVIN-7216-OUT,TEX-5118-TEX;n:type:ShaderForge.SFN_Append,id:7216,x:32244,y:32449,varname:node_7216,prsc:2|A-6400-OUT,B-1375-OUT;n:type:ShaderForge.SFN_Vector1,id:1375,x:32072,y:32514,varname:node_1375,prsc:2,v1:0;n:type:ShaderForge.SFN_Tex2d,id:7969,x:31386,y:32959,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_7969,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:359,x:30904,y:32788,ptovrint:False,ptlb:Dissolve,ptin:_Dissolve,varname:node_359,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.4667512,max:1;n:type:ShaderForge.SFN_Add,id:4717,x:31605,y:33047,varname:node_4717,prsc:2|A-95-OUT,B-7969-R;n:type:ShaderForge.SFN_RemapRange,id:95,x:31439,y:32742,varname:node_95,prsc:2,frmn:0,frmx:1,tomn:-0.5,tomx:0.5|IN-1256-OUT;n:type:ShaderForge.SFN_OneMinus,id:1256,x:31266,y:32742,varname:node_1256,prsc:2|IN-359-OUT;n:type:ShaderForge.SFN_RemapRange,id:6123,x:31658,y:32605,varname:node_6123,prsc:2,frmn:0,frmx:1,tomn:-4,tomx:4|IN-4717-OUT;n:type:ShaderForge.SFN_Clamp01,id:8997,x:31814,y:32444,varname:node_8997,prsc:2|IN-6123-OUT;n:type:ShaderForge.SFN_OneMinus,id:6400,x:31993,y:32353,varname:node_6400,prsc:2|IN-8997-OUT;proporder:7969-359-5118;pass:END;sub:END;*/

Shader "Shader Forge/HealthShader" {
    Properties {
        _Noise ("Noise", 2D) = "white" {}
        _Dissolve ("Dissolve", Range(0, 1)) = 0.4667512
        _node_5118 ("node_5118", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _node_5118; uniform float4 _node_5118_ST;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _Dissolve;
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
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(i.uv0, _Noise));
                float node_4717 = (((1.0 - _Dissolve)*1.0+-0.5)+_Noise_var.r);
                clip(node_4717 - 0.5);
////// Lighting:
////// Emissive:
                float2 node_7216 = float2((1.0 - saturate((node_4717*8.0+-4.0))),0.0);
                float4 node_2497 = tex2D(_node_5118,TRANSFORM_TEX(node_7216, _node_5118));
                float3 emissive = node_2497.rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
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
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _Dissolve;
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
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(i.uv0, _Noise));
                float node_4717 = (((1.0 - _Dissolve)*1.0+-0.5)+_Noise_var.r);
                clip(node_4717 - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
