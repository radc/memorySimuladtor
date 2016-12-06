using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace memorySimulator
{
    public abstract class Constants
    {
        //BIT CONSTANTS
            //static power
        public static uint memoryNumberOfSectors = 12;
        public static uint memoryNumberOfBanks = 12;
        public static uint linesPerMemoryBlock = 16;
        public static uint bitsPerLine = 128;

        public static uint bitsPerSample = 8;

        public static uint samplesPerLine = bitsPerLine / bitsPerSample;

        public static uint posYCtuInMemory = 4;
        public static uint posXCtuInMemory = 4;

        //MEMORY CYCLES
        public static double cyclesPerLine = 0.25;
        public static double cyclesPerMemoryBlock = cyclesPerLine * linesPerMemoryBlock;
        public static double cyclesPerBank = cyclesPerMemoryBlock * memoryNumberOfSectors;
        public static double cyclesPerCTUShift = cyclesPerBank * posXCtuInMemory;

        public static double cyclesPerCTUNewLine = 10 * 10 * 16 * cyclesPerLine;

        //VIDEO CONSTANTS
        public static uint videoWidth = 1024;
        public static uint videoHeight = 768;

        public static uint videoCtuWidth = videoWidth / 64;
        public static uint videoCtuHeight = videoHeight / 64;

        //ARCHITECTURE CONSTANTS
        public static uint archLatency16 = 4;
        public static uint archLatency32 = 5;

        public static uint archDrag16 = 3; //"Arrasto"
        public static uint archDrag32 = 3; //"Arrasto"

        public static uint archCyclesPerSetOfCand16 = 16; 
        public static uint archCyclesPerSetOfCand32 = 32;

        public static uint archParallelism = 16;

        private static double original_cyclesPerCTUNewLine = cyclesPerCTUNewLine;
        private static double original_cyclesPerCTUShift = cyclesPerBank * posXCtuInMemory;


        public static void setMeClocks()
        {
            cyclesPerCTUNewLine = original_cyclesPerCTUNewLine;
            cyclesPerCTUShift = original_cyclesPerCTUShift;

            archDrag16 = 3; //"Arrasto"
            archDrag32 = 3; //"Arrasto"

            cyclesPerLine = 0.25;
            cyclesPerMemoryBlock = cyclesPerLine * linesPerMemoryBlock;
            cyclesPerBank = cyclesPerMemoryBlock * memoryNumberOfSectors;

            cyclesPerCTUShift = cyclesPerBank * posXCtuInMemory;

            Console.WriteLine(cyclesPerCTUNewLine);
            Console.WriteLine(cyclesPerCTUShift);


        }
        public static void setDeClocks()
        {
            setMeClocks();
            


            cyclesPerLine = 1;
            cyclesPerMemoryBlock = cyclesPerLine * linesPerMemoryBlock;
            cyclesPerBank = cyclesPerMemoryBlock * memoryNumberOfSectors;
            cyclesPerCTUShift = cyclesPerBank * posXCtuInMemory;

            cyclesPerCTUShift = Math.Ceiling(cyclesPerCTUShift / 3);
            cyclesPerCTUNewLine = ((2 * 10 * 16) + (2 * 10 * 17)) * cyclesPerLine;

            archDrag16 = 4; //"Arrasto"
            archDrag32 = 5; //"Arrasto"

            cyclesPerLine = 1;

            Console.WriteLine(cyclesPerCTUNewLine);
            Console.WriteLine(cyclesPerCTUShift);
        }

    }

    enum TracePosition
    {
        cuDepth = 0,
        cuPelX,
        cuPelY,
        currPicViewIdx,
        currPicPoc,
        isDepth,
        refPicViewIdx,
        refPicPoc,
        srLTHor,
        srRBHor,
        srLTVer,
        srRBVer,
        partTz,
        tzX,
        tzY
    }

    
}
