using UnityEngine;

namespace Tim
{
    public static class UtilityMethods
    {
        public static float RoundTo(float f, int precision)
        {   
            return Mathf.Round(f * 10 * precision) / (10 * precision); 
        }
        
        public static Vector3 V2toV3(Vector2 f)
        {
            return new Vector3(f.x, f.y);
        }
    }
}