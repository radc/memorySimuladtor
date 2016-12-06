using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace memorySimulator
{
    public class Tracer
    {
        public StreamReader file { private set; get; }
        private bool fileOpened;
        public CodingBlock lastCb { private set; get; }
        public CodingBlock currCb { private set; get; }
        public bool noLineRead { private set; get; }
        public int tzX { private set; get; }
        public int tzY { private set; get; }
        public string currentLine;
        public string lastLine;
        public int currCyclesCounter { private set; get; }
        public int lastCyclesCounter { private set; get; }

        public Tracer (string tracePath)
        {
            try
            {
                fileOpened = true;
                file = new StreamReader(tracePath);                
            }
            catch
            {    
                fileOpened = false;
                throw new Exception("Failed trying to open trace file");
            }
            
            this.lastCb = new CodingBlock();
            this.currCb = new CodingBlock();
            this.noLineRead = true;
            this.currentLine = "";
            this.lastLine = "";

            this.currCyclesCounter = 0;
            this.lastCyclesCounter = 0;
        }

        public void processLine (ref int x, ref int y)
        {
            processLine();
            x = this.tzX;
            y = this.tzY;
        }
        public void processLine ()
        {
            if (fileOpened == false)
            {
                throw new Exception("File not opened!");
            }
            
            string line;
            string[] param;
            

            line = file.ReadLine();
            param = line.Split(',');

            noLineRead = false;

            int size = 64 >> int.Parse(param[(int)TracePosition.cuDepth]);
            int cuPosX = int.Parse(param[(int)TracePosition.cuPelX]);
            int cuPosY = int.Parse(param[(int)TracePosition.cuPelY]);
            int currViewIdx = int.Parse(param[(int)TracePosition.currPicViewIdx]);
            int currPoc = int.Parse(param[(int)TracePosition.currPicPoc]);
            int refViewIdx = int.Parse(param[(int)TracePosition.refPicViewIdx]);
            int refPoc = int.Parse(param[(int)TracePosition.refPicPoc]);
            int srLTHor = int.Parse(param[(int)TracePosition.srLTHor]);
            int srLTVer = int.Parse(param[(int)TracePosition.srLTVer]);
            int srRBHor = int.Parse(param[(int)TracePosition.srRBHor]);
            int srRBVer = int.Parse(param[(int)TracePosition.srRBVer]);
            tzX = int.Parse(param[(int)TracePosition.tzX]);
            tzY = int.Parse(param[(int)TracePosition.tzY]);
            bool isDepth = (int.Parse(param[(int)TracePosition.isDepth]) == 1) ? true : false;
//            sr = new SearchRange()            
            lastCb = currCb;             
            SearchRange sr = new SearchRange(srLTHor, srLTVer, srRBHor, srRBVer, refPoc, refViewIdx);
            currCb = new CodingBlock(size, cuPosX, cuPosY, sr, currPoc, currViewIdx, isDepth);

            this.lastLine = this.currentLine;
            this.currentLine = line;
            
            checkMultiCandidatesError();

            
        }

        private void checkMultiCandidatesError()
        {
            if (currCb == null || lastCb == null) return;

            if (currCb.poc == lastCb.poc)
                if (currCb.viewIdx == lastCb.viewIdx)
                    if (currCb.sr.poc != lastCb.sr.poc || currCb.sr.viewIdx != lastCb.sr.viewIdx)
                        throw new Exception("More than one ME/DE candidate per block!");
        }

        public bool isEof(){
            if (!fileOpened) return true;

            return this.file.EndOfStream;
        }

        public bool isFirstProcessedCbInCTURow()
        {
            if (lastCb == null) return true;

            if (this.lastCb.poc != this.currCb.poc || this.lastCb.viewIdx != this.currCb.viewIdx) return true;
            else{
                if (this.lastCb.sr.poc != this.currCb.sr.poc || this.lastCb.sr.viewIdx != this.currCb.sr.viewIdx) throw new Exception("More than one ME/DE candidate!");
            }
            
            //double currX = Math.Floor((double)currCb.posX / 64);
            //double lastX = Math.Floor((double)lastCb.posX / 64);
            double currY = Math.Floor((double)currCb.posY / 64);
            double lastY = Math.Floor((double)lastCb.posY / 64);

            if (lastY < currY) return true;

            return false;
        }

        public bool isFirstCUofCTU()
        {
            if (lastCb == null) return true;

            double currX, lastX;
            currX = Math.Floor((double)currCb.posX / 64);
            lastX = Math.Floor((double)lastCb.posX / 64);

            if (currX < lastX) throw new Exception("Current CU.posX/64 is fewer than Last CU.posX/64");

            if (currX > lastX) return true;
            else return false;
        }

        public bool isCbChanged()
        {
            bool changed;

            if (lastCb == null) changed = true;
            else changed = !currCb.Equals(lastCb);
            
            if (changed)
            {
                this.lastCyclesCounter = this.currCyclesCounter;
                this.currCyclesCounter = 1;                
            }
            else
            {
                this.currCyclesCounter++;
            }

            return changed;
        }

        public bool isFrameChanged()
        {
            bool changed;

            if (lastCb == null) changed = true;
            else changed = !(currCb.poc == lastCb.poc && currCb.viewIdx == lastCb.viewIdx);

            return changed;
        }

        public bool isLastCTUofFrame()
        {
            return this.currCb.ctuPosX == Constants.videoCtuWidth - 1 && this.currCb.ctuPosY == Constants.videoCtuHeight - 1;
        }

        public bool isCtuChanged()
        {
            if (this.currCb.ctuPosX == this.lastCb.ctuPosX)
                if (this.currCb.ctuPosY == this.lastCb.ctuPosY)
                    if (this.currCb.poc == this.lastCb.poc)
                        if (this.currCb.viewIdx == this.lastCb.viewIdx)
                            return false;
            return true;
        }

        public void closeFile()
        {
            this.file.Close();
        }


    }
}

