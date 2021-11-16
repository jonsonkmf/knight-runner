 struct ClosestPoint
 {
    float3 Point; // точка на отрезке
    float onLine; // 0 - точка за пределами, 1 - точка на отрезке
};

// получение ближайшей точки на прямой, заданной отрезком 
inline ClosestPoint GetClosestPoint(float3 coord, float3 start, float3 end)
{
    float3 Line = end - start;
    float len = length(Line);
    Line = normalize(Line);
    
    float3 v = coord - start;
    float d = dot(v, Line);
    
    ClosestPoint res;
    res.Point = start + Line * d;
    res.onLine = step(0, d) * step(d, len); 
    
    return res;
}

// получение ближайшей точки на отрезке
inline float3 GetClosestPointClamped(float3 coord, float3 start, float3 end)
{
    float3 Line = end - start;
    float len = length(Line);
    Line = normalize(Line);
    
    float3 v = coord - start;
    float d = dot(v, Line);
    d = clamp(d, 0.0, len); // ограничение на отрезке
    
    return start + Line * d;
}
// получение проекции вектора на плокость
inline float3 ProjectOnPlane(float3 v, float3 planeNormal)
{
    float num1 = dot(planeNormal, planeNormal);
    float num2 = dot(v, planeNormal);
    return float3(v.x - planeNormal.x * num2 / num1, v.y - planeNormal.y * num2 / num1, v.z - planeNormal.z * num2 / num1);
}

// аддитивный блендинг с учётом альфы
inline float4 AdditiveMix(float4 bottom, float4 top)
{
    float4 result = float4(bottom.rgb * (1.0 - top.a) + top.rgb * top.a, top.a + bottom.a * (1.0 - top.a));
    return result;
}
float3 rgb2hsv(float3 rgb)
{
    float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    float4 p = lerp(float4(rgb.bg, K.wz), float4(rgb.gb, K.xy), step(rgb.b, rgb.g));
    float4 q = lerp(float4(p.xyw, rgb.r), float4(rgb.r, p.yzx), step(p.x, rgb.r));

    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

float3 hsv2rgb(float3 hsv)
{
    float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    float3 p = abs(frac(hsv.xxx + K.xyz) * 6.0 - K.www);
    return hsv.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), hsv.y);
}