using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject
{
    public class Counterss
    {
        private uint current;
        private uint mMax;
        private uint mMin;

        public Counterss(uint min)
        {
            this.mMin = min;
            this.mMax = uint.MaxValue;
            this.current = this.mMin;
        }

        public Counterss(uint min, uint max)
        {
            this.mMin = min;
            this.mMax = max;
            this.current = this.mMin;
        }

        public uint NextVal
        {
            get
            {
                this.current++;
                if (this.current >= this.mMax)
                {
                    this.current = this.mMin;
                }
                return this.current;
            }
        }
    }
}
