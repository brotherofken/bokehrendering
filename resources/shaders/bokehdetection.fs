//-----------------------------------------------------------------------------
#version 420
//#extension GL_NV_gpu_shader5 : enable
//#extension GL_EXT_shader_image_load_store : enable

//-----------------------------------------------------------------------------
layout(size1x32) coherent  uniform uimage1D 	BokehCountTex;
layout(size4x32) writeonly uniform  image2D 	BokehPositionTex;
layout(size4x32) writeonly uniform  image2D 	BokehColorTex;
//-----------------------------------------------------------------------------
uniform sampler2D		BlurDepthTex;
uniform sampler2D		ColorTex;
uniform float			MaxCoCRadius;
uniform float			LumThreshold;
uniform float			CoCThreshold;
out vec4 				FragColor;
//------------------------------------------------------------------------------


void main()
{
	vec2 rcpSize  =  1.f / vec2(textureSize(ColorTex,0));
	vec2  coord   = gl_FragCoord.xy * rcpSize;
	vec2  bd	  = textureLod(BlurDepthTex,coord,0).xy;
	float blur    = bd.x;
	float depth   = bd.y;
	vec3  color   = textureLod(ColorTex,coord,0).xyz;
	float cocSize = blur * MaxCoCRadius;

	// Compute 5x5 neighborhood color with 9 samples
	vec3 avgColor   = vec3(0);
	avgColor += textureLod(ColorTex,(gl_FragCoord.xy+vec2(-1.5f,-1.5f))*rcpSize,0).xyz;
	avgColor += textureLod(ColorTex,(gl_FragCoord.xy+vec2( 0.5f,-1.5f))*rcpSize,0).xyz;
	avgColor += textureLod(ColorTex,(gl_FragCoord.xy+vec2( 1.5f,-1.5f))*rcpSize,0).xyz;
	avgColor += textureLod(ColorTex,(gl_FragCoord.xy+vec2(-1.5f, 0.5f))*rcpSize,0).xyz;
	avgColor += textureLod(ColorTex,(gl_FragCoord.xy+vec2( 0.5f, 0.5f))*rcpSize,0).xyz;
	avgColor += textureLod(ColorTex,(gl_FragCoord.xy+vec2( 1.5f, 0.5f))*rcpSize,0).xyz;
	avgColor += textureLod(ColorTex,(gl_FragCoord.xy+vec2(-1.5f, 1.5f))*rcpSize,0).xyz;
	avgColor += textureLod(ColorTex,(gl_FragCoord.xy+vec2( 0.5f, 1.5f))*rcpSize,0).xyz;
	avgColor += textureLod(ColorTex,(gl_FragCoord.xy+vec2( 1.5f, 1.5f))*rcpSize,0).xyz;
	avgColor /= 9;

	// Compute luminosity (with equal weights)
	float colorLum   = dot(vec3(1), color);
	float avgLum     = dot(vec3(1), avgColor);
	float difLum     = max(colorLum-avgLum,0);

	// Count point where intensity of neighbors is less than the current pixel
	if(difLum>LumThreshold && cocSize>CoCThreshold)
	{
//if(coord.x > 10000)
//{
		ivec2 bufSize, coord;
		int current = int(imageAtomicAdd(BokehCountTex, 1, 1));
		bufSize 	= textureSize(ColorTex,0).xy;
		coord.y 	= int(floor(current/bufSize.y));
		coord.x 	= current - coord.y*bufSize.y;
//}

//ivec2 coord = ivec2(int(floor(gl_FragCoord.x)),int(floor(gl_FragCoord.y)));
//if(coord.x > 100)
//{
		// Compute energy of the bokeh according to CoC size
		vec3 lcolor = color.xyz / (3.141592654f*cocSize*cocSize);
		imageStore(BokehPositionTex,coord,vec4(gl_FragCoord.x,gl_FragCoord.y,depth,blur));
		imageStore(BokehColorTex,coord,vec4(lcolor,1));
		color 		= vec3(0);
//}
	}

	FragColor = vec4(color,1);
}

