#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

struct CustomLightingData
{
	float3 worldPos;
	float3 worldNormal;
	float3 viewDirection;
	float4 shadowCoord;
	float roughness;
	float metallic;
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

float SpecularStrength (CustomLightingData data, Light light)
{
	float3 h = normalize(light.direction + data.viewDirection);
	float nh2 = pow((saturate(dot(data.worldNormal, h))), 2);
	float lh2 = pow((saturate(dot(light.direction, h))), 2);
	float r2 = pow(data.roughness, 2);
	float d2 = pow(nh2 * (r2 - 1.0) + 1.00001, 2);
	float normalization = data.roughness * 4.0 + 2.0;
	return r2 / (d2 * max(0.1, lh2) * normalization);
}

float3 CustomLightHandling(CustomLightingData data, Light light)
{
	float3 radiance = light.color * (light.distanceAttenuation * light.shadowAttenuation);

	float3 baseColor = saturate(dot(data.worldNormal, light.direction));

	float3 diffuse = baseColor * (1 - data.metallic);
	float3 specular = lerp(0, baseColor, data.metallic);

	float specularStrength = SpecularStrength(data, light);

	float3 color = specularStrength * specular + diffuse;

	float3 toonColor = ToonRamp(data, color);
	return toonColor * radiance;
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

void CustomShading_float(float3 Position, float3 Normal, float3 ViewDirection, UnityTexture2D RampTexture,
						 UnitySamplerState SS, float Roughness, float Metallic, float3 UnpackedNormal, out float3 Out)
{
	CustomLightingData data;
	data.worldPos = Position;
	data.worldNormal = normalize(Normal + UnpackedNormal);
	data.viewDirection = ViewDirection;
	data.roughness = Roughness;
	data.metallic = Metallic;
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