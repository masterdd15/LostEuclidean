#pragma once
struct LightingData
{
	float3 worldPos;
	float3 worldNormal;
	float3 viewDirection;
};

float3 CalculateLighting(LightingData data)
{
	Light mainLight = GetMainLight();

	float3 diffuse = saturate(dot(data.normal, mainLight.direction));

	float3 color = diffuse;

	return color;
}

void CustomShading_float(float3 position, float3 normal, float3 viewDirection, out float out)
{
	LightingData data;
	data.worldPos = position;
	data.worldNormal = normal;
	data.viewDirection = viewDirection;

	float3 color = CalculateLighting(data);

	out = color.r;
}