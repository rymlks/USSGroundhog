using System;
using System.Collections.Generic;

namespace StaticUtils
{
    public static class MathUtil
    {
        public static int ClampPositiveInt(int maximumExclusive, int value)
        {
            if (value < 0)
            {
                return maximumExclusive - 1;
            }
            else if (value >= maximumExclusive)
            {
                return 0;
            }
            else
            {
                return value;
            }
        }

        public static Object chooseRandom<T>(Random random, List<T> toChooseFrom)
        {
            return toChooseFrom[random.Next(toChooseFrom.Count)];
        }
    }
}