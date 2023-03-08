#include "ClassicNoise2D.hlsl"

void PiecewiseFractalNoise_float (float2 input, float frequency, out float Out)
{
	float noise = 0;


	noise += cnoise(input * 1) / (2^1) ;
	noise += cnoise(input * 2) / (2^2) ;
	noise += cnoise(input * 3) / (2^3) ;
	noise += cnoise(input * 4) / (2^4) ;


	noise = (cnoise(input) + 1) / 2;

	noise *= frequency;
	noise = floor(noise);
	noise /= frequency;

	Out = noise - 0.5f;
}