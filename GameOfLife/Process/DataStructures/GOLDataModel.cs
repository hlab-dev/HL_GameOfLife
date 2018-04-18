using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfLife.Process.DataStructures
{
    //Process Flow Sates
    public enum GOLState
    {
        Halt,
        Run,
        Exit,
        Pause,
        Start
    }

    //Data structure to keep the x,y and live state of the current cell in the matrix.
    public class CellEntry
    {
        public int XPos { get; set; } //Grid X position.
        public int YPos { get; set; } //Grid Y position.
        public bool CellSate { get; set; } //Dead or Alive Indicator
    }

    //Process data to use in the Game of Life Logic Flow.
    public class MatrixMetaData
    {
        public int GridXMaxLen { get; set; } = 10; //Grid X Total size.
        public int GridYMaxLen { get; set; } = 10; //Grid Y Total size.
        public int Evolution { get; set; } = 0; //Iteration Cycles.
        public int EvolutionRefreshDelay { get; set; } = 100; //Not being used.
    }

    //Matrix Data storage structures that will be use for data manipulation and display rendering.
    public class MatrixProcessData
    {
        public List<CellEntry> MatrixList = new List<CellEntry>(); //Master List Matrix 
        public List<CellEntry> BufferMatrixList = new List<CellEntry>(); //List Matrix that will be processed and displyed.
    }

}
