using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace memorySimulator
{
    public class SearchRange
    {
        public int LTHor { get; private set; }
        public int LTVer { get; private set; }
        public int RBHor { get; private set; }
        public int RBVer { get; private set; }
        public int poc { get; private set; }
        public int viewIdx { get; private set; }

        public SearchRange(int LTHor, int LTVer, int RBHor, int RBVer, int poc, int viewIdx)
        {
            this.LTHor = LTHor;
            this.LTVer = LTVer;
            this.RBHor = RBHor;
            this.RBVer = RBVer;
            this.poc = poc;
            this.viewIdx = viewIdx;
        }

        public SearchRange(int LTHor, int LTVer, int RBHor, int RBVer)
        {
            this.LTHor = LTHor;
            this.LTVer = LTVer;
            this.RBHor = RBHor;
            this.RBVer = RBVer;
            this.poc = -1;
            this.viewIdx = -1;
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            SearchRange sr = (SearchRange)obj;

            if (this.LTHor == sr.LTHor)
                if (this.LTVer == sr.LTVer)
                    if (this.RBHor == sr.RBHor)
                        if (this.RBVer == sr.RBVer)
                            if (this.poc == sr.poc)
                                if (this.viewIdx == sr.viewIdx)
                                    return true;

            return false;
        }
    }
}
