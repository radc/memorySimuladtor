using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace memorySimulator
{
    public enum PowerStatus
    {
        SLEEP = 0,
        POWER_ON = 1,
        TOTAL_POWER_STATUS = 2
    }

    public class Memory
    {
        public PowerStatus[][] memoryStatus;        
        public ulong[][][] cyclesStaticPower;
        public ulong[][] toogleSleep2On;
        public ulong[][] toogleOn2Sleep;

        public ulong[][][] totalReadings;
        public ulong[][][] totalWritings;


        public Memory()
        {
            memoryStatus = new PowerStatus[Constants.memoryNumberOfSectors][];
            toogleSleep2On = new ulong[Constants.memoryNumberOfSectors][];
            toogleOn2Sleep = new ulong[Constants.memoryNumberOfSectors][];
            totalReadings = new ulong[Constants.memoryNumberOfSectors][][]; 
            totalWritings = new ulong[Constants.memoryNumberOfSectors][][];

            for (int i = 0; i < Constants.memoryNumberOfSectors; i++)
			{
                memoryStatus[i] = new PowerStatus[Constants.memoryNumberOfBanks];
                toogleSleep2On[i] = new ulong[Constants.memoryNumberOfBanks];
                toogleOn2Sleep[i] = new ulong[Constants.memoryNumberOfBanks];
                totalReadings[i] = new ulong[Constants.memoryNumberOfBanks][];
                totalWritings[i] = new ulong[Constants.memoryNumberOfBanks][];

                for (int j = 0; j < Constants.memoryNumberOfBanks; j++)
                {
                    memoryStatus[i][j] = PowerStatus.POWER_ON;
                    toogleSleep2On[i][j] = 0;
                    toogleOn2Sleep[i][j] = 0;
                    totalReadings[i][j] = new ulong[Constants.linesPerMemoryBlock];
                    totalWritings[i][j] = new ulong[Constants.linesPerMemoryBlock];
                    for (int k = 0; k < Constants.linesPerMemoryBlock; k++)
                    {
                        totalReadings[i][j][k] = 0;
                        totalWritings[i][j][k] = 0;
                    }
                }
			}

            cyclesStaticPower = new ulong[(int)PowerStatus.TOTAL_POWER_STATUS][][];
            for (int i = 0; i < (int)PowerStatus.TOTAL_POWER_STATUS; i++)
            {
                cyclesStaticPower[i] = new ulong[Constants.memoryNumberOfSectors][];
                for (int j = 0; j < Constants.memoryNumberOfSectors; j++)
                {
                    cyclesStaticPower[i][j] = new ulong[Constants.memoryNumberOfBanks];
                    for (int k = 0; k < Constants.memoryNumberOfBanks; k++)
                    {
                        cyclesStaticPower[i][j][k] = 0;
                    }
                }                
            }
        }

        public void incrementCycle()
        {
            for (int i = 0; i < Constants.memoryNumberOfSectors; i++)
            {
                for (int j = 0; j < Constants.memoryNumberOfBanks; j++)
                {
                    int memState = (int)memoryStatus[i][j];
                    cyclesStaticPower[memState][i][j]++;
                }
            }
        }

        public void incrementCycle(int numCycles)
        {
            if (numCycles == 0) return;

            for (int i = 0; i < Constants.memoryNumberOfSectors; i++)
            {
                for (int j = 0; j < Constants.memoryNumberOfBanks; j++)
                {
                    int memState = (int)memoryStatus[i][j];
                    cyclesStaticPower[memState][i][j] += (ulong)numCycles;
                }
            }
        }

        public void sleepMemoryBlock(int sector, int bank)
        {
            if (memoryStatus[sector][bank]==PowerStatus.SLEEP)
            {
                throw new Exception("Trying to sleep an already sleeping block!");
            }
            memoryStatus[sector][bank] = PowerStatus.SLEEP;
            toogleOn2Sleep[sector][bank]++;
        }

        public void trySleepMemoryBlock(int sector, int bank)
        {
            if (memoryStatus[sector][bank] == PowerStatus.SLEEP)
            {
                return;
            }
            else
            {
                memoryStatus[sector][bank] = PowerStatus.SLEEP;
                toogleOn2Sleep[sector][bank]++;
            }            
        }

        public void wakeMemoryBlock(int sector, int bank)
        {
            if (memoryStatus[sector][bank] == PowerStatus.POWER_ON)
            {
                throw new Exception("Trying to wake an already awake block!");
            }
            memoryStatus[sector][bank] = PowerStatus.POWER_ON;
            toogleSleep2On[sector][bank]++;
        }

        public void tryWakeAllMemory()
        {
            for (int i = 0; i < Constants.memoryNumberOfSectors; i++)
			{			 
                for (int j = 0; j < Constants.memoryNumberOfBanks; j++)			
                {
                    tryWakeMemoryBlock(i, j);
                }
			}
        }
        public void tryWakeMemoryBlock(int sector, int bank)
        {
            if (memoryStatus[sector][bank] == PowerStatus.POWER_ON)
            {
                return;
            }
            else
            {
                memoryStatus[sector][bank] = PowerStatus.POWER_ON;
                toogleSleep2On[sector][bank]++;
            }
            
        }

        public void definePowerOnRange(CodingBlock cb)
        {
            int lpmb = (int)Constants.linesPerMemoryBlock;
            int spl = (int)Constants.samplesPerLine;
            uint totalSectors = Constants.memoryNumberOfSectors;
            uint totalBanks = Constants.memoryNumberOfBanks;

            int posX = cb.posXinCtu;
            int posY = cb.posYinCtu;

            int firstSector = (int)Constants.posYCtuInMemory;
            firstSector += (posY / lpmb);
            firstSector += Math.Sign(cb.sr.LTVer) * (int)Math.Ceiling(Math.Abs((double)((double)cb.sr.LTVer)/(double)lpmb));
            
            int lastSector = (int)Constants.posYCtuInMemory;
            lastSector += (posY / lpmb);
            lastSector += Math.Sign(cb.sr.RBVer) * (int)Math.Ceiling(Math.Abs((double)((double)cb.sr.RBVer) / (double)lpmb));
            lastSector += (int)Math.Ceiling((double)cb.size / lpmb);
            lastSector--;

            int firstBank = (int)Constants.posXCtuInMemory;
            firstBank += (posX / spl);
            firstBank += Math.Sign(cb.sr.LTHor) * (int)Math.Ceiling(Math.Abs((double)((double)cb.sr.LTHor) / (double)spl));

            int lastBank = (int)Constants.posXCtuInMemory;
            lastBank += (posX / spl);
            lastBank += Math.Sign(cb.sr.RBHor) * (int)Math.Ceiling(Math.Abs((double)((double)cb.sr.RBHor) / (double)spl));
            lastBank += (int)Math.Ceiling((double)cb.size / spl);
            lastBank--;

            for (int i = 0; i < totalSectors; i++)
            {
                for (int j = 0; j < totalBanks; j++)
                {
                    if (i < firstSector || i > lastSector)
                    {
                        trySleepMemoryBlock(i, j);
                    }
                    else
                    {
                        if (j < firstBank || j > lastBank)
                        {
                            trySleepMemoryBlock(i, j);
                        }
                        else
                        {
                            tryWakeMemoryBlock(i, j);
                        }
                    }
                }
            }

           /*
            Console.Write("firstSector: ");
            Console.WriteLine(firstSector);
            Console.Write("lastSector: ");
            Console.WriteLine(lastSector);
            Console.Write("firstBank: ");
            Console.WriteLine(firstBank);
            Console.Write("lastBank: ");
            Console.WriteLine(lastBank);
            */
        }

        public void definePowerOnRangeCtuShift(CodingBlock cb)
        {
            int lpmb = (int)Constants.linesPerMemoryBlock;
            int spl = (int)Constants.samplesPerLine;
            uint totalSectors = Constants.memoryNumberOfSectors;
            uint totalBanks = Constants.memoryNumberOfBanks;

            int posX = cb.posXinCtu;
            int posY = cb.posYinCtu;

            int firstSector = (int)Constants.posYCtuInMemory;
            firstSector += (posY / lpmb);
            firstSector += Math.Sign(cb.sr.LTVer) * (int)Math.Ceiling(Math.Abs((double)((double)cb.sr.LTVer) / (double)lpmb));

            int lastSector = (int)Constants.posYCtuInMemory;
            lastSector += (posY / lpmb);
            lastSector += Math.Sign(cb.sr.RBVer) * (int)Math.Ceiling(Math.Abs((double)((double)cb.sr.RBVer) / (double)lpmb));
            lastSector += (int)Math.Ceiling((double)cb.size / lpmb);
            lastSector--;

            int firstBank = (int)Constants.posXCtuInMemory;
            firstBank += (posX / spl);
            firstBank += Math.Sign(cb.sr.LTHor) * (int)Math.Ceiling(Math.Abs((double)((double)cb.sr.LTHor) / (double)spl));

            int lastBank = (int)Constants.posXCtuInMemory;
            lastBank += (posX / spl);
            lastBank += Math.Sign(cb.sr.RBHor) * (int)Math.Ceiling(Math.Abs((double)((double)cb.sr.RBHor) / (double)spl));
            lastBank += (int)Math.Ceiling((double)cb.size / spl);
            lastBank--;

            for (int i = 0; i < totalSectors; i++)
            {
                for (int j = 0; j < totalBanks; j++)
                {
                    if (j > (int)Constants.memoryNumberOfBanks - 5)
                    {
                        tryWakeMemoryBlock(i, j);
                    }
                    else
                    {
                        if (i < firstSector || i > lastSector)
                        {
                            trySleepMemoryBlock(i, j);
                        }
                        else
                        {
                            if (j < firstBank || j > lastBank)
                            {
                                trySleepMemoryBlock(i, j);
                            }
                            else
                            {
                                tryWakeMemoryBlock(i, j);
                            }
                        }
                    }
                }
            }

            /*
             Console.Write("firstSector: ");
             Console.WriteLine(firstSector);
             Console.Write("lastSector: ");
             Console.WriteLine(lastSector);
             Console.Write("firstBank: ");
             Console.WriteLine(firstBank);
             Console.Write("lastBank: ");
             Console.WriteLine(lastBank);
             */
        }

        
        public void increaseLineReading(int sector, int bank, int line)
        {
            if (memoryStatus[sector][bank] != PowerStatus.POWER_ON)
            {
                throw new Exception("Trying to read a sleeping line!");
            }
            totalReadings[sector][bank][line]++;
        }

        public void increaseLineWriting(int sector, int bank, int line)
        {
            if (memoryStatus[sector][bank] != PowerStatus.POWER_ON)
            {
                throw new Exception("Trying to write in a sleeping line!");
            }
            totalWritings[sector][bank][line]++;
        }

        public void increaseBlockWriting(int sector, int bank)
        {
            if (memoryStatus[sector][bank] != PowerStatus.POWER_ON)
            {
                throw new Exception("Trying to write in a sleeping memory block!");
            }

            for (int i = 0; i < Constants.linesPerMemoryBlock; i++)
            {
                totalWritings[sector][bank][i]++;    
            }            
        }

        public void increaseWritingAllMemory()
        {
            for (int i = 0; i < Constants.memoryNumberOfSectors; i++)
            {
                for (int j = 0; j < Constants.memoryNumberOfBanks; j++)
                {
                    increaseBlockWriting(i, j);
                }
            }
        }

        public void increaseWritingBankRegion(int firstBank, int lastBank)
        {
            for (int i = firstBank; i <= lastBank; i++)
            {
                increaseWritingAllBank(i);
            }
        }

        public void increaseWritingAllBank(int bank)
        {
            for (int i = 0; i < Constants.memoryNumberOfSectors; i++)
            {
                for (int j = 0; j < Constants.linesPerMemoryBlock; j++)
                {
                    this.increaseLineWriting(i, bank, j);                    
                }
            }
        }

        public void increaseRegionReading(CodingBlock cb, int x, int y)
        {
            int spl = (int)Constants.samplesPerLine;
            int lpmb = (int)Constants.linesPerMemoryBlock;
            int size = cb.size;
            int a;


            int bankCU = (int)Constants.posXCtuInMemory + (cb.posXinCtu / spl);
            if (x < 0){
                a = (int)Math.Ceiling((double)Math.Abs(x)/spl);
            }else{
                a = (int)Math.Floor((double)Math.Abs(x)/spl);
            }
            int firstBank = bankCU + Math.Sign(x) * a;
            int lastBank = firstBank + size / spl + ((Math.Abs(x) % spl == 0) ? 0 : 1) - 1;


            int sectorCU = (int)Constants.posYCtuInMemory + (cb.posYinCtu / lpmb);
            if (y < 0){
                a = (int)Math.Ceiling((double)Math.Abs(y) / lpmb);
            }
            else{
                a = (int)Math.Floor((double)Math.Abs(y) / lpmb);
            }
            int firstSector = sectorCU + Math.Sign(y) * a;
            int lastSector = firstSector + size / lpmb + ((Math.Abs(y) % lpmb == 0) ? 0 : 1) - 1;

            int firstLine, lastLine;
            if (y >= 0)
            {                
                firstLine = y % lpmb;                
            }
            else
            {
                //firstLine = lpmb - (-y % lpmb);
                firstLine = (10*lpmb + y) % lpmb; //Asserts firstLine will never be negative
            }

            lastLine = (firstLine + size - 1) % lpmb;

            for (int i = firstSector; i <= lastSector; i++)
            {
                for (int j = firstBank; j <= lastBank; j++)
                {
                    if (i==firstSector)
                    {
                        for (int k = firstLine; k < lpmb; k++)
                        {
                            this.increaseLineReading(i, j, k);
                        }
                    }
                    else if (i == lastSector)
                    {
                        for (int k = 0; k <= lastLine; k++)
                        {
                            this.increaseLineReading(i, j, k);
                        }
                    }
                    else {
                        for (int k = 0; k < lpmb; k++)
                        {
                            this.increaseLineReading(i, j, k);
                        }
                    }
                }
            }

         /*   Console.Write("firstSector: ");
            Console.WriteLine(firstSector);
            Console.Write("lastSector: ");
            Console.WriteLine(lastSector);
            Console.Write("firstBank: ");
            Console.WriteLine(firstBank);
            Console.Write("lastBank: ");
            Console.WriteLine(lastBank);
            Console.Write("firstLine: ");
            Console.WriteLine(firstLine);
            Console.Write("lastLine: ");
            Console.WriteLine(lastLine); */
        }

        public void trySleepMemoryBanks(int firstBank, int lastBank)
        {
            for (int i = firstBank; i <= lastBank; i++)
            {
                trySleepAllMemoryBank(i);
            }
        }

        public void trySleepAllMemoryBank(int bank)
        {
            for (int i = 0; i < Constants.memoryNumberOfSectors; i++)
            {                
                this.trySleepMemoryBlock(i, bank);                
            }
        }



        public void tryWakeMemoryBanks(int firstBank, int lastBank)
        {
            for (int i = firstBank; i <= lastBank; i++)
            {
                tryWakeAllMemoryBank(i);
            }
        }

        public void tryWakeAllMemoryBank(int bank)
        {
            for (int i = 0; i < Constants.memoryNumberOfSectors; i++)
            {
                this.tryWakeMemoryBlock(i, bank);
            }
        }

        

        public void clearMemoryCounters()
        {
            for (int i = 0; i < Constants.memoryNumberOfSectors; i++)
            {
                for (int j = 0; j < Constants.memoryNumberOfBanks; j++)
                {
                    cyclesStaticPower[0][i][j] = 0;
                    cyclesStaticPower[1][i][j] = 0;
                    toogleSleep2On[i][j] = 0;
                    toogleOn2Sleep[i][j] = 0;
                    for (int k = 0; k < Constants.linesPerMemoryBlock; k++)
                    {
                        totalReadings[i][j][k] = 0;
                        totalWritings[i][j][k] = 0;
                    }
                }
            }
        }

        public void clearNonDeCounter()
        {
            for (int i = 0; i < Constants.memoryNumberOfSectors; i++)
            {
                for (int j = 0; j < Constants.memoryNumberOfBanks; j++)
                {
                    if (!(i > 3 && i < 8))
                    {
                        cyclesStaticPower[0][i][j] = 0;
                        cyclesStaticPower[1][i][j] = 0;
                        toogleSleep2On[i][j] = 0;
                        toogleOn2Sleep[i][j] = 0;
                        for (int k = 0; k < Constants.linesPerMemoryBlock; k++)
                        {
                            totalReadings[i][j][k] = 0;
                            totalWritings[i][j][k] = 0;
                        }
                    }
                    
                }
            }
        }

    }
}
