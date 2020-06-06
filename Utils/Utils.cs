using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsmod.util
{
    class Utils
    {
        public static float GetLerpValue(float from, float to, float t, bool clamped = false)
        {
            if (clamped)
            {
                if ((double)from < (double)to)
                {
                    if ((double)t < (double)from)
                        return 0.0f;
                    if ((double)t > (double)to)
                        return 1f;
                }
                else
                {
                    if ((double)t < (double)to)
                        return 1f;
                    if ((double)t > (double)from)
                        return 0.0f;
                }
            }
            return (float)(((double)t - (double)from) / ((double)to - (double)from));
        }
    }
}
