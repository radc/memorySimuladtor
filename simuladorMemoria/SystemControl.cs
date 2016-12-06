using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace memorySimulator
{
    public class SystemControl
    {
        Tracer tracer;
        Memory mem;
        public ulong sumActiveCyclesCb, sumCb, sumCtu, sumFrame, sumCtuSkip, sumIdleCycles;
        public ulong sumReadings = 0;
        public ulong sumWritings = 0;
        public ulong sumToogleOn2Sleep = 0;
        public ulong sumToogleSleep2On = 0;
        public ulong sumSleep = 0;
        public ulong sumPowerOn = 0;
        public bool isDe;
        public bool isMe;
        //private string tracePath;

        public SystemControl(string tracePath, bool isDe)
        {
            // TODO: Complete member initialization
            tracer = new Tracer(tracePath);
            mem = new Memory();

            this.isDe = isDe;
            this.isMe = !isDe;
            mem.clearMemoryCounters();
        }

        public void scrollEntireFile(){
            while (!tracer.isEof()){
                tracer.processLine();
            }
        }

        public Memory Mem{
            get{
                return this.mem;
            }
        }

        public void startMemorySimulation()
        {
            int clocks;
            bool firstCbOfCtu;
            bool firstCtuOfRow;
            
            sumActiveCyclesCb = 0;
            sumCb = 0;
            sumCtu = 0;
            sumFrame = 0;
            sumCtuSkip = 0;
            sumIdleCycles = 0;
            firstCbOfCtu = true;
            firstCtuOfRow = true;
            tracer.processLine();
            //Console.WriteLine("First CTU Pos X: " +tracer.currCb.ctuPosX.ToString());
            //increaseNewLineDyanamicPower();
            definePowerOnRange(firstCbOfCtu, firstCtuOfRow);            
            mem.increaseRegionReading(tracer.currCb, tracer.tzX, tracer.tzY);

            while (!tracer.isEof())
            {
                tracer.processLine();                

                if (tracer.isCbChanged())
                {
                    //Increment Analysis Counter                    
                    sumCb++;

                    //Increment Static Power of Last Cb
                    clocks = traceLines2clockCycles(tracer.lastCyclesCounter, tracer.lastCb.size);
                    incrStaticAndDynamicPower(firstCbOfCtu, firstCtuOfRow, clocks);

                    if (tracer.isCtuChanged())
                    {
                        sumCtu++; //Increment Analysis Ctu Counter

                        firstCbOfCtu = true;
                        firstCtuOfRow = (tracer.currCb.ctuPosX == 0);
                        
                        if (tracer.isFrameChanged()) //Check if occurred a frame change
                        {                            
                            sumFrame++; //Increment Analysis Frame Counter\

                            if (!tracer.isLastCTUofFrame())
                            {
                                shiftMemoryUntilFrameEnds();
                                tracer.lastCb.posX = -64;
                                tracer.lastCb.posY = 0;
                            }
                            
                        }
                        shiftMemoryUntilReachCurrentCtu();
                    }
                    else
                    {
                        firstCbOfCtu = false;
                    }

                    definePowerOnRange(firstCbOfCtu, firstCtuOfRow);                    
                }

                mem.increaseRegionReading(tracer.currCb, tracer.tzX, tracer.tzY);
                                             
            }
            //Static Power of last Cb
            sumCb++;
            sumCtu++;
            sumFrame++;

            clocks = traceLines2clockCycles(tracer.lastCyclesCounter, tracer.lastCb.size);
            mem.incrementCycle(clocks);
            sumActiveCyclesCb += (ulong)clocks;

            shiftMemoryUntilFrameEnds();

            testMemory();
                        
            //Console.WriteLine("Last Pos X" + tracer.currCb.ctuPosX.ToString());
            tracer.closeFile();

        }

        private void incShiftDynamicPowerIdle()
        {
            mem.trySleepMemoryBanks(0, (int)Constants.memoryNumberOfBanks - 5);
            mem.tryWakeMemoryBanks((int)Constants.memoryNumberOfBanks - 4, (int)Constants.memoryNumberOfBanks - 1);            
            mem.increaseWritingBankRegion((int)Constants.memoryNumberOfBanks - 4, (int)Constants.memoryNumberOfBanks - 1);         
        }

        private void incShiftDynamicPower()
        {   
            mem.increaseWritingBankRegion((int)Constants.memoryNumberOfBanks - 4, (int)Constants.memoryNumberOfBanks - 1);
        }

        private void increaseNewLineDynamicPowerIdle()
        {
            mem.tryWakeAllMemory();
            mem.increaseWritingAllMemory();            
        }

        private void incNewLineDynamicPower()
        {   
            mem.increaseWritingAllMemory();            
        }

        private void incrStaticAndDynamicPower(bool firstCbOfCtu, bool firstCtuOfRow, int clocks){
            if (firstCbOfCtu)
            {
                if (firstCtuOfRow)
                {
                    incNewLineDynamicPower();
                    increaseStaticCountersFirstCtuRow(clocks);
                }
                else
                {
                    incShiftDynamicPower();
                    increaseStaticCountersFirstCbOfCtu(clocks);
                }
            }
            else
            {
                mem.incrementCycle(clocks);
                sumActiveCyclesCb += (ulong)clocks;
            }
        }

        private void definePowerOnRange(bool firstCbOfCtu, bool firstCtuOfRow)
        {
            if (firstCbOfCtu)
            {
                if (firstCtuOfRow)
                {
                    mem.tryWakeAllMemory();
                }
                else
                {
                    mem.definePowerOnRangeCtuShift(tracer.currCb);
                }
            }
            else
            {
                mem.definePowerOnRange(tracer.currCb);
            }
        }

        private void increaseIdleStaticPowerCtuShift()
        {
            mem.incrementCycle((int)Constants.cyclesPerCTUShift);
            sumIdleCycles += (ulong)Constants.cyclesPerCTUShift;
        }

        private void increaseStaticCountersFirstCbOfCtu(int clocks)
        {
            if (clocks < Constants.cyclesPerCTUShift)
            {
                 increaseIdleStaticPowerCtuShift();
                 sumActiveCyclesCb += (ulong)clocks;
                 //sumIdleCycles += ((ulong)Constants.cyclesPerCTUShift - (ulong)clocks);
                sumIdleCycles -= ((ulong)clocks);
            }
            else
            {
                mem.incrementCycle(clocks);
                sumActiveCyclesCb += (ulong)clocks;
            }
        }

        private void increaseStaticCountersFirstCtuRow(int clocks)
        {
            //Sets memory-writing number of cycles
            mem.incrementCycle((int)Constants.cyclesPerCTUNewLine); //10 x 10 x 16 x N
            sumIdleCycles += ((ulong)Constants.cyclesPerCTUNewLine);

            //Sets processing number of cycles
            mem.definePowerOnRange(tracer.currCb);
            mem.incrementCycle(clocks);
            sumActiveCyclesCb += (ulong)clocks;
        }

        private void increaseIdleStaticPower(int memoryBlocksWritting)
        {
            mem.incrementCycle((int)Constants.cyclesPerMemoryBlock * memoryBlocksWritting);
            sumIdleCycles += (ulong)Constants.cyclesPerMemoryBlock * (ulong)memoryBlocksWritting;
        }

        private void shiftMemoryUntilFrameEnds(){
            CodingBlock auxCb = new CodingBlock();
            tracer.lastCb.copyMyAttributesTo(auxCb);

            int auxX = auxCb.ctuPosX + 1;
            int auxY = auxCb.ctuPosY;

            while (auxY < Constants.videoCtuHeight)
            {
                while (auxX < Constants.videoCtuWidth)
                {
                    if (auxX == 0)
                    {
                        increaseNewLineDynamicPowerIdle();
                        sumCtuSkip++;
                        increaseIdleStaticPowerCtuShift();
                        increaseIdleStaticPowerCtuShift();
                        increaseIdleStaticPowerCtuShift();
                        //increaseIdleStaticPower();
                    }
                    else
                    {
                        incShiftDynamicPowerIdle();
                        sumCtuSkip++;
                        increaseIdleStaticPowerCtuShift();
                    }
                    auxX++;
                }
                auxX = 0; //
                auxY++;
            }
        }

        private void shiftMemoryUntilReachCurrentCtu()
        {            
            int lastX = tracer.lastCb.ctuPosX + 1;
            int lastY = tracer.lastCb.ctuPosY;

            int currX = tracer.currCb.ctuPosX;
            int currY = tracer.currCb.ctuPosY;

            while (lastY < currY)
            {
                while (lastX < Constants.videoCtuWidth)
                {
                    if (lastX == 0)
                    {
                        increaseNewLineDynamicPowerIdle();
                        sumCtuSkip++;
                        increaseIdleStaticPowerCtuShift();
                        increaseIdleStaticPowerCtuShift();
                        increaseIdleStaticPowerCtuShift();
                        //increaseIdleStaticPower();
                    }
                    else
                    { //CTU Shift 
                        incShiftDynamicPowerIdle();
                        sumCtuSkip++;
                        increaseIdleStaticPowerCtuShift();
                    }
                    lastX ++;
                }
                lastY ++;
                lastX = 0;
            }
            while (lastX < currX)
            {
                if (lastX == 0)
                {
                    increaseNewLineDynamicPowerIdle();
                    sumCtuSkip++;
                    increaseIdleStaticPowerCtuShift();
                    increaseIdleStaticPowerCtuShift();
                    increaseIdleStaticPowerCtuShift();
                    //increaseIdleStaticPower();
                }
                else
                {
                    incShiftDynamicPowerIdle();
                    sumCtuSkip++;
                    increaseIdleStaticPowerCtuShift();
                }
                lastX ++;
            }

            /*
            if (lastX == 0)
                increaseNewLineDynamicPowerIdle();
            else
                incShiftDynamicPowerIdle();
             * */

            if (lastX != currX || lastY != currY) throw new Exception("Error counting shifting dynamic and static power");
        }


        public void testMemory()
        {
            sumReadings = 0;
            sumWritings = 0;
            sumToogleOn2Sleep = 0;
            sumToogleSleep2On = 0;
            sumSleep = 0;
            sumPowerOn = 0;
            for (int i = 0; i < Constants.memoryNumberOfSectors; i++)
            {
                for (int j = 0; j < Constants.memoryNumberOfBanks; j++)
                {
                    for (int k = 0; k < Constants.linesPerMemoryBlock; k++)
                    {
                        sumReadings += mem.totalReadings[i][j][k];
                        sumWritings += mem.totalWritings[i][j][k];                        
                    }
                    sumToogleOn2Sleep += mem.toogleOn2Sleep[i][j];
                    sumToogleSleep2On += mem.toogleSleep2On[i][j];
                    sumSleep += mem.cyclesStaticPower[(int)PowerStatus.SLEEP][i][j];
                    sumPowerOn += mem.cyclesStaticPower[(int)PowerStatus.POWER_ON][i][j];
                }    
            }

            //Console.WriteLine("Total of Readings: " +sumReadings.ToString());
            //Console.WriteLine("Total of Writings: " + sumWritings.ToString());
            //Console.WriteLine("Total of Toogles (On -> Sleep): " + sumToogleOn2Sleep.ToString());
            //Console.WriteLine("Total of Toogles (Sleep -> On): " + sumToogleSleep2On.ToString());
        }

        private int traceLines2clockCycles(int n, int size){
            int c;
            int acc;
            if (n == 0) return 0;

            if (size == 16)
            {
                if (this.isDe) acc = 48; //DE 16
                else acc = (int)Math.Ceiling((double)n / (double)Constants.archParallelism); //ME 16

                c = (int)Constants.archLatency16 + acc * (int)Constants.archCyclesPerSetOfCand16 + (int)Constants.archDrag16;
            }
            else
            {

                if (this.isDe) acc = 96; //DE 32
                else acc = (int)Math.Ceiling((double)n / (double)Constants.archParallelism); //ME 32

                if (size != 32) throw new Exception("Block size is neither 16 or 32");
                c = (int)Constants.archLatency32 + acc * (int)Constants.archCyclesPerSetOfCand32 + (int)Constants.archDrag32;
            }
            return c;
        }

    }
}
