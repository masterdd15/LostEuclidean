#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

struct CustomLightingData
{
	float3 worldPos;
	float3 worldNormal;
	float3 viewDirection;
	float4 shadowCoord;
	UnityTexture2D rampTex;
	UnitySamplerState samplerState;
};

float ToonRamp(CustomLightingData data, float intensity)
{
	float2 uv = float2(intensity, 1);
	float4 _SampleTexture2D_RGBA = SAMPLE_TEXTURE2D(data.rampTex, data.samplerState, uv);
	return _SampleTexture2D_RGBA.r;
}


#ifndef SHADERGRAPH_PREVIEW

float3 CustomLightHandling(CustomLightingData data, Light light)
{
	float3 radiance = light.color * (light.distanceAttenuation * light.shadowAttenuation);
	float3 diffuse = saturate(dot(data.worldNormal, light.direction));
	diffuse = ToonRamp(data, diffuse);
	return diffuse * radiance;
}

float3 CalculateLighting(CustomLightingData data)
{
	Light mainLight = GetMainLight(data.shadowCoord, data.worldPos, 1);

	float3 color = CustomLightHandling(data, mainLight);

	uint numAdditionalLights = GetAdditionalLightsCount();
	for (uint lightI = 0; lightI < numAdditionalLights; lightI++)
	{
        Light light = GetAdditionalLight(lightI, data.worldPos, 1);
        color += CustomLightHandling(data, light);
    }

	return color;
}
#endif

void CustomShading_float(float3 Position, float3 Normal, float3 ViewDirection, UnityTexture2D RampTexture, UnitySamplerState SS, out float3 Out)
{
	CustomLightingData data;
	data.worldPos = Position;
	data.worldNormal = Normal;
	data.viewDirection = ViewDirection;
	data.rampTex = RampTexture;
	data.samplerState = SS;

	#ifdef SHADERGRAPH_PREVIEW
		// In preview, there's no shadows or bakedGI
		data.shadowCoord = 0;
	#else
		// Calculate the main light shadow coord
		// There are two types depending on if cascades are enabled
		float4 clipPos = TransformWorldToHClip(Position);
		#if SHADOWS_SCREEN
			data.shadowCoord = ComputeScreenPos(clipPos);
		#else
			data.shadowCoord = TransformWorldToShadowCoord(Position);
		#endif
	#endif

	#ifdef SHADERGRAPH_PREVIEW
		// In preview, estimate diffuse + specular
		float3 lightDir = float3(0.5, 0.5, 0);
		float intensity = saturate(dot(data.worldNormal, lightDir));
		Out = intensity.rrr;

	#else
		float3 color = CalculateLighting(data);
		Out = color;

	#endif

	
}

#endif