Shader "Custom/Simple Color"
{
    Properties
    {
        _Color("Tint", Color) = (0, 0, 0, 0)
        _Alpha("Transparency", Range(0, 1)) = 1
    }
    
    SubShader
    {
        Blend srcAlpha OneMinusSrcAlpha
    
        Tags { "Queue"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vertex
            #pragma fragment fragment
            
            //Variables
            float4 _Color;
            float _Alpha;
            
            //structs
             struct vertexInput
            {
                float4 vertex : POSITION;
            };
            
            struct vertexOutput
			{
				float4 position : SV_POSITION;
			};
	
	        struct fragmentOutput
	        {
	            float4 color : COLOR;
	        };
            
            vertexOutput vertex(vertexInput vInput)
	        {
	            vertexOutput vOutput;
	         
	            vOutput.position = UnityObjectToClipPos(vInput.vertex);
	         
	            return vOutput;  
	        }
	        
	        fragmentOutput fragment(vertexOutput fInput)
	        {
	            fragmentOutput fOutput;
	            
	            fOutput.color = _Color;
	            fOutput.color.a = _Alpha;
	            
	            return fOutput;
	        }
            
            ENDCG
        }
    }
}
