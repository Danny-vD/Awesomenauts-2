Shader "Custom/Waterfall"
{
    Properties
    {
        _Color("tint", Color) = (1, 1, 1, 1)
        _Alpha("Alpha", Range(0, 1)) = 0
    
        [Header(Texture)]
        _MainTex("Albedo (RGB)", 2D) = "white"
        
        [Header(Flowing points)]
        _FlowDirectionNORMAL("Default", Vector) = (0, 0, 0, 0)
        _FlowDirectionX("> X limit", Vector) = (0, 0, 0, 0)
        _FlowDirectionZ("> Z limit ", Vector) = (0, 0, 0, 0)
        _FlowDirectionXZ("> XZ limit", Vector) = (0, 0, 0, 0)
        
        [Header(Flowing parameters)]
        _XLimit("X limit", float) = 0
        _ZLimit("Z limit", float) = 0
        
        _FlowSpeed("Flow Speed", float) = 0.2
        
        [MaterialToggle] _XFlow("Flow X?", float) = 1
        [MaterialToggle] _ZFlow("Flow Z?", float) = 1
        
    }
    
    SubShader
    {
        Blend srcAlpha OneMinusSrcAlpha
        ZTest Less
    
        Tags
        {
            "Queue" = "AlphaTest"
        }    
    
        Pass
        {
            CGPROGRAM
            
            #pragma vertex vertex
			#pragma fragment fragment
            
            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0
        
	        ///Variables
	        //Texture
	        uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST; //tiling(xy) and offset(zw)
	        uniform float _Alpha;
	        uniform float4 _Color;
	        
	        //Flow points
	        uniform float4 _FlowDirectionNORMAL;
	        uniform float4 _FlowDirectionX;
	        uniform float4 _FlowDirectionZ;
	        uniform float4 _FlowDirectionXZ;
	        
	        //Flow parameters  
	        uniform float _XLimit;
	        uniform float _ZLimit;
	        uniform float _FlowSpeed;  
	        
	        uniform bool _XFlow;
	        uniform bool _ZFlow;
        
            ///Functions
            float2 GetCorrectSamplerPos(float3 objectPos);
            float3 GetFlowDirection(float2 objectPos);
            
            float rand(float3 coords);

        
            ///Structs
            struct VertexInput
            {
                float4 vertex : POSITION;
				float4 texCoord : TEXCOORD0;
            };
            
            struct vertexOutput
			{
				float4 position : SV_POSITION;
				float4 objectPos : TEXCOORD1;
				
				float4 texCoord : TEXCOORD0;
			};
	
	        struct fragmentOutput
	        {
	            float4 color : COLOR;
	        };
	        
	        vertexOutput vertex(VertexInput vInput)
	        {
	            vInput.texCoord.xy = vInput.texCoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
	        
	            vertexOutput vOutput;
	         
	            vOutput.objectPos = vInput.vertex;
	            vOutput.position = UnityObjectToClipPos(vInput.vertex);
	            
	            vOutput.texCoord = vInput.texCoord;
	         
	            return vOutput;  
	        }
	        
	        fragmentOutput fragment(vertexOutput fInput)
	        {
	            fragmentOutput fOutput;
	            
	            float4 textureColor = tex2D(_MainTex, GetCorrectSamplerPos(fInput.objectPos.xyz));
	            
	            fOutput.color = textureColor * _Color;
	            fOutput.color.a = _Alpha;
	            
	            return fOutput;
	        }
	        
	        float2 GetCorrectSamplerPos(float3 objectPos)
	        {
	            float3 flowDirectionPoint = GetFlowDirection(objectPos.xz);
	            float2 relativeObjPos = flowDirectionPoint.xz - objectPos.xz;
	            
	            float2 flowDirection = relativeObjPos;
	            flowDirection.x = normalize(flowDirection.x) * _XFlow;
	            flowDirection.y = normalize(flowDirection.y) * _ZFlow;
	            
	            int factor = (flowDirectionPoint.y < objectPos.y)? 1 : -1;
	            float2 yCorrection = flowDirection * objectPos.y * factor;
	            relativeObjPos += yCorrection;
	            
	            relativeObjPos += flowDirection * _Time.y * _FlowSpeed;
	            
	            return relativeObjPos * 0.1 * _MainTex_ST.xy + _MainTex_ST.zw;
	        }
	        
	        float3 GetFlowDirection(float2 objectPos)
	        {
	            float3 flowDirectionPoint;
	        
	            if(objectPos.x > _XLimit)
	            {
	                if(objectPos.y > _ZLimit)
	                {
	                    flowDirectionPoint = _FlowDirectionXZ;
	                }
	                else
	                {
	                    flowDirectionPoint = _FlowDirectionX;
	                }
	            }
	            else if(objectPos.y > _ZLimit)
	            {
	                flowDirectionPoint = _FlowDirectionZ;   
	            }
	            else
	            {
	                flowDirectionPoint = _FlowDirectionNORMAL;
	            }
	            
	            return flowDirectionPoint;
	        }
            ENDCG
        }
    }
}
