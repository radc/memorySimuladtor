using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace memorySimulator
{
    public class CodingBlock
    {
        public int poc { get; set; }
        public int viewIdx { get; set; }
        public bool isDepth { get; set; }
        public int posX { get; set; }
        public int posY { get; set; }
        //public int posXinCtu { get; set; }
        //public int posYinCtu { get; set; }
        public int size { get; set; }
        public SearchRange sr { get; set; }


        public CodingBlock(int size, int posX, int posY, SearchRange sr, int poc, int viewIdx, bool isDepth)
        {
            this.size = size;
            this.posX = posX;
            this.posY = posY;

            
            this.sr = sr;

            this.poc = poc;
            this.viewIdx = viewIdx;
            this.isDepth = isDepth;
        }

        public CodingBlock(int size, int posX, int posY, SearchRange sr)
        {
            this.size = size;
            this.posX = posX;
            this.posY = posY;


            this.sr = sr;

            this.poc = -1;
            this.viewIdx = -1;
            this.isDepth = false;
        }

        public CodingBlock()
        {
            this.size = -1;
            this.posX = -1;
            this.posY = -1;


            this.sr = null;

            this.poc = -1;
            this.viewIdx = -1;
            this.isDepth = false;
        }


        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            CodingBlock cb = (CodingBlock)obj;

            if (this.poc == cb.poc)
                if (this.viewIdx == cb.viewIdx)
                    if (this.isDepth == cb.isDepth)
                        if (this.posX == cb.posX)
                            if (this.posY == cb.posY)
                                if (this.size == cb.size)
                                    if (this.sr.Equals(cb.sr))
                                        return true;
            return false;
        }

        /*
         * Does not considers SearchRange in the Equals evaluation
         */
        public bool EqualsExceptSearchRange(CodingBlock obj)
        {
            if (this.poc == obj.poc)
                if (this.viewIdx == obj.viewIdx)
                    if (this.isDepth == obj.isDepth)
                        if (this.posX == obj.posX)
                            if (this.posY == obj.posY)
                                if (this.size == obj.size)                                    
                                        return true;
            return false;
        }

        public void copyMyAttributesTo(CodingBlock dest)
        {
            dest.posX = this.posX;
            dest.posY = this.posY;
            dest.size = this.size;
            dest.viewIdx = this.viewIdx;
            dest.poc = this.poc;
            dest.isDepth = this.isDepth;
        }

        public int posXinCtu
        {
            get{
                return posX % 64;
            }

        }

        public int posYinCtu
        {
            get{
                return posY % 64;
            }

        }

        public int ctuPosX
        {
            get
            {
                return (int)Math.Floor((double)this.posX / 64);                
            }
        }

        public int ctuPosY
        {
            get
            {
                return (int)Math.Floor((double)this.posY / 64);
            }
        }


        
        
    }
}
