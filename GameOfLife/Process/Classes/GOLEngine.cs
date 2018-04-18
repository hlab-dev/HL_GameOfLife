using System;
using System.Collections.Generic;
using System.Text;
using GameOfLife.Process.DataStructures;
using GameOfLife.Process.Classes;

namespace GameOfLife.Process.Classes
{

    public static class Utils
    {
        public static readonly Random random = new Random();
    }

    public class GOLEngine
    {
        private MatrixMetaData _matrixMetaData = new MatrixMetaData();
        private MatrixProcessData _matrixProcessData = new MatrixProcessData();
        public GOLState RunningState;

        public GOLEngine(int MatrixGridXSize, int MatrixGridYSize, int DrawRefreshRate)
        {
            _matrixMetaData.GridXMaxLen = MatrixGridXSize;
            _matrixMetaData.GridYMaxLen = MatrixGridYSize;
            _matrixMetaData.EvolutionRefreshDelay = DrawRefreshRate;
            _matrixMetaData.Evolution = 0;
        }

        public void InitialiseGOL()
        {
            RunningState = GOLState.Run;

            //Initialise the Matrix List with default Cell States.
            for (int y = 1; y <= _matrixMetaData.GridYMaxLen; y++)
            {
                for (int x = 1; x <= _matrixMetaData.GridXMaxLen;x++)
                {
                    CellEntry cell = new CellEntry();
                    cell.XPos = x;
                    cell.YPos = y;
                    cell.CellSate = false;

                    _matrixProcessData.MatrixList.Add(cell);
                }
            }

            //Added some random crazy funnies
            //Insert a sample random patern into the matrix to see what the generic outcomes would be. 
             for (int i = 1; i <= 500; i++)
             {
                 _matrixProcessData.MatrixList[Utils.random.Next(1, (_matrixMetaData.GridXMaxLen*_matrixMetaData.GridYMaxLen)-1)].CellSate = true;
             }
             

            //Added Glider Test
            /*_matrixProcessData.MatrixList.Find(x => x.XPos == 4 + 10 && x.YPos == 1 + 10).CellSate = true;
            _matrixProcessData.MatrixList.Find(x => x.XPos == 5 + 10 && x.YPos == 2 + 10).CellSate = true;
            _matrixProcessData.MatrixList.Find(x => x.XPos == 5 + 10 && x.YPos == 3 + 10).CellSate = true;
            _matrixProcessData.MatrixList.Find(x => x.XPos == 4 + 10 && x.YPos == 3 + 10).CellSate = true;
            _matrixProcessData.MatrixList.Find(x => x.XPos == 3 + 10 && x.YPos == 3 + 10).CellSate = true;*/

            /*
            //Added Blinking Test
            _matrixProcessData.MatrixList.Find(x => x.XPos == 3 + 10 && x.YPos == 1 + 10).CellSate = true;
            _matrixProcessData.MatrixList.Find(x => x.XPos == 3 + 10 && x.YPos == 2 + 10).CellSate = true;
            _matrixProcessData.MatrixList.Find(x => x.XPos == 3 + 10 && x.YPos == 3 + 10).CellSate = true;*/


            DrawGOLMatrix();
        }

        public void DrawGOLMatrix()
        {

            Console.Clear();
            Console.WriteLine("Press Enter key to continue or Press Esc key to exit!!");

            //Draw the Grid info on the console screen.
            foreach (CellEntry CellItem in _matrixProcessData.MatrixList)
            {
                string cellChar = "";

                if (CellItem.CellSate)
                    cellChar = "#";
                else
                    cellChar = "";

                Console.SetCursorPosition(CellItem.XPos,CellItem.YPos);
                Console.Write(cellChar);
            }

            Console.SetCursorPosition(0, _matrixMetaData.GridYMaxLen + 2);
            Console.WriteLine("Evolution Itteration: "+ _matrixMetaData.Evolution.ToString());
        }

        public void DrawGOLBufferMatrix()
        {

            Console.Clear();
            Console.WriteLine("Press Enter key to continue or Press Esc key to exit!!");

            //Draw the Grid info on the console screen.
            foreach (CellEntry CellItem in _matrixProcessData.BufferMatrixList)
            {
                string cellChar = "";

                if (CellItem.CellSate)
                    cellChar = "#";
                else
                    cellChar = "";

                Console.SetCursorPosition(CellItem.XPos, CellItem.YPos);
                Console.Write(cellChar);
            }

            _matrixMetaData.Evolution++;

            Console.SetCursorPosition(0, _matrixMetaData.GridYMaxLen+2);
            Console.WriteLine("Evolution Itteration: " + _matrixMetaData.Evolution.ToString());
        }

        public void RunGOL(ConsoleKey KeyPressed)
        {

            while (KeyPressed != ConsoleKey.Escape)
            {

                ConsoleKeyInfo ReadKeyPress = Console.ReadKey();

                if (ReadKeyPress.Key == ConsoleKey.Escape)
                {
                    RunningState = GOLState.Exit;

                    Console.Clear();
                    Console.WriteLine("Press Escape Key to Exit!");

                    return;
                }

                //Before an evolution srtarts we need to cater for certain flow control states.
                switch (RunningState)
                {
                    case GOLState.Pause: continue;
                    case GOLState.Halt: continue; 
                }
                
                if (RunningState == GOLState.Exit) break;

                //Process Matrix States Into Buffer Matrix to draw the new evolution.

                GOLMatrixProcessing MatrixProcess = new GOLMatrixProcessing();

                MatrixProcess.ProcessBufferMatrix(ref _matrixProcessData);

                //-------------------------------------------------------------------
                
                //Update the Screen from the processed Buffer Matrix.
                DrawGOLBufferMatrix();
            }
        }

        public void PauseGOL()
        {
            RunningState = GOLState.Pause;
        }

        public void StartGOL()
        {
            InitialiseGOL();

            while (true)
            {
                ConsoleKeyInfo ReadKeyPress = Console.ReadKey();

                if (ReadKeyPress.Key == ConsoleKey.Escape)
                {
                    RunningState = GOLState.Exit;
                    break;
                }

                //Make sure we dont run the GOL Evolution Iteration if an Exit instruction was issued.
                if (RunningState != GOLState.Exit)
                    RunGOL(ReadKeyPress.Key);
            }

            ExitGOL();

            Console.Clear();
            Console.WriteLine("Press Escape Key to Exit or Enter to start over!!");

            ConsoleKeyInfo ReadKeyPressFinal = Console.ReadKey();

            if (ReadKeyPressFinal.Key == ConsoleKey.Escape)
            {
                return;
            }else
            {
                //Recursive call to start the same game flow process over from scratch.
                StartGOL();
            }

        }

        public void ExitGOL()
        {
            _matrixProcessData.BufferMatrixList.Clear();
            _matrixProcessData.MatrixList.Clear();
            _matrixMetaData.Evolution = 0;
        }

    }
}
