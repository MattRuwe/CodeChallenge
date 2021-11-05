using System;
using System.Collections.Generic;
using System.Text;
namespace OmahaMTG.Challenge.FrequenciesChallenge
{
    /// <summary>
    /// Represents a pseudo-random number generator that is faster and provides more randomness than .NET framework.
    /// </summary>
    public sealed class Rand
    {
        private static Object newLock = new Object();
        private static int newSeed = (int)(System.DateTime.Now.Ticks + System.DateTime.Now.Millisecond);
        private int value, mix, twist;

        /// <summary>
        /// Intialize a new instance of the Rand class. Each new instance will be different than the previous.
        /// </summary>
        public Rand()
        {
            lock (newLock)
            {
                int oldSeed = newSeed;
                value = newSeed;
                mix = 51651237;
                twist = 895213268;
                CalculateNext();
                newSeed = value;
                SetSeed(oldSeed);
            }
        }
        /// <summary>
        /// Initialize a new instance of the Rand class with the specified seed.
        /// </summary>
        /// <param name="seed"></param>
        public Rand(int seed)
        {
            SetSeed(seed);
        }
        /// <summary>
        /// Reset this Rand object based on the specified seed.
        /// </summary>
        /// <param name="seed"></param>
        public void SetSeed(int seed)
        {
            mix = 51651237;
            twist = 895213268;
            value = seed;
            for (int i = 0; i < 50; i++)
            {
                CalculateNext();
            }
            seed ^= (seed >> 15);
            value = (int)((uint)0x9417B3AF ^ seed);
            for (int i = 0; i < 950; i++)
            {
                CalculateNext();
            }
        }
        private void CalculateNext()
        {
            int y = value ^ twist - mix ^ value;
            y ^= twist ^ value ^ mix;
            mix ^= twist ^ value;
            value ^= twist - mix;
            twist ^= value ^ y;
            value ^= (twist << 7) ^ (mix >> 16) ^ (y << 8);
        }
        /// <summary>
        /// Returns the next random integer in [0,2^31-1].
        /// </summary>
        public int Next
        {
            get
            {
                CalculateNext();
                return value & 0x7fffffff;
            }
        }
        /// <summary>
        /// Returns the next random integer in [-2^31,2^31-1].
        /// </summary>
        public int NextInt
        {
            get
            {
                CalculateNext();
                return value;
            }
        }
    }
}