Shader "CCC/Internal/GraphDrawer"
{
	Properties
	{
	}

	Category
	{
		Tags{ "Queue" = "Geometry" }
		Lighting Off
		BindChannels
		{
			Bind "Color", color
			Bind "Vertex", vertex
		}

		SubShader
		{
			Pass
			{
			}
		}
	}
}