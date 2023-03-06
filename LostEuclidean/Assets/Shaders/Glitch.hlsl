#include "ClassicNoise2D.hlsl"

void PiecewiseFractalNoise_float (float2 input, float frequency, out float Out)
{
	float noise = 0;
	
	[Unroll]
	for (int i = 1; i < 5; i++)
	{
		noise += 1 * cnoise(input * i) / (2^i) ;
	}

	noise *= frequency;
	noise = floor(noise);
	noise /= frequency;

	Out = noise;
}