
Shader "Wunderwunsch/HexTile"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Line ("Line Thickness", Range(0.0,1.0)) = 0.1
		_Color ("Line Color", Color) = (0,0,0,0)
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog
			
			#include "UnityCG.cginc"


			float _Line;
			float4 _Color;

//hexagonal cell projection, based on https://www.shadertoy.com/view/Xd2GR3
//this uses distance maths to calculate the borders
			float4 hexagon(in float2 p) 
			{
				float2 q = float2( p.x*1.1547006, p.y + p.x*0.5773503 );
				float2 pi = floor(q), pf = frac(q);
				float v = fmod(pi.x + pi.y, 3.0);
				float ca = step(1.0,v), cb = step(2.0,v);
				float2  ma = step(pf.xy,pf.yx);
				float e = dot( ma, 1.0-pf.yx + ca*(pf.x+pf.y-1.0) + cb*(pf.yx-2.0*pf.xy) );
				p = float2( q.x + floor(0.5+p.y/1.5), 0.666666666*p.y ) + 0.5;
				float f = length( (frac(p) - 0.5)*float2(1.0,0.85) );			
				return float4( pi + ca - cb*ma, e, f );
			}
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				half4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD1;
				float4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos=mul(unity_ObjectToWorld,v.vertex);
				o.color = v.color;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 vals = i.color;
				//if (vals.r > 0.05f && vals.r < 0.99f)
				//{
				//	vals.r = _FoW;
				//	vals.g = _FoW;
				//	vals.b = _FoW;
				//	vals.a = 1;
				//}
				//
				fixed4 col = tex2D(_MainTex, i.uv);

				//project hexagon grid in X & Z axes
				float4 hex = hexagon(abs(i.worldPos.xz));

				
				float hexline= hex.b;

				//create outline from the hexagon function data
				hexline=step(_Line,hexline);

				//visualize gride lines
				if (hexline==0)
				col=col*(1-_Color.a)+_Color*_Color.a;

				return col;
			}
			ENDCG
		}
	}
}