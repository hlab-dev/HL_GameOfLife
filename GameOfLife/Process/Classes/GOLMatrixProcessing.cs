using System;
using System.Collections.Generic;
using System.Text;
using GameOfLife.Process.DataStructures;
using GameOfLife.Process.Classes;
using System.Linq;

namespace GameOfLife.Process.Classes
{
    class GOLMatrixProcessing
    {

        private void AddNeigbourStateToList(ref List<bool> NeighboursList, ref MatrixProcessData MatrixData, int XPos, int YPos)
        {
            int cellX = XPos;
            int cellY = YPos;
            
            var NeghbourCell = new CellEntry();

            //Bug Fix: We need to lookup neighbours from the origional matrix list and not the Buffer List to accuratly determine if the cell is dead or alive.
            NeghbourCell = MatrixData.MatrixList.Find(x => x.XPos == cellX && x.YPos == cellY);

            if (NeghbourCell != null)
            {
                NeighboursList.Add(NeghbourCell.CellSate);
            }

        }

        private List<bool> FetchNeighboursList(ref MatrixProcessData MatrixData, CellEntry CellItem)
        {

            List<bool> neighboursList = new List<bool>();

            int cellX = CellItem.XPos;
            int cellY = CellItem.YPos;
            bool cellstate = CellItem.CellSate;

            //Fetch the eight neghbours cells to add to the list for calculation and rule processing.
            AddNeigbourStateToList(ref neighboursList, ref MatrixData, cellX-1,cellY-1);
            AddNeigbourStateToList(ref neighboursList, ref MatrixData, cellX, cellY-1);
            AddNeigbourStateToList(ref neighboursList, ref MatrixData, cellX+1, cellY-1);
            AddNeigbourStateToList(ref neighboursList, ref MatrixData, cellX-1, cellY);
            AddNeigbourStateToList(ref neighboursList, ref MatrixData, cellX+1, cellY);
            AddNeigbourStateToList(ref neighboursList, ref MatrixData, cellX-1, cellY+1);
            AddNeigbourStateToList(ref neighboursList, ref MatrixData, cellX, cellY+1);
            AddNeigbourStateToList(ref neighboursList, ref MatrixData, cellX+1, cellY+1);

            return neighboursList;
        }

        int CountNeighbourValues(List<bool> NeighboursList, bool val)
        {
            return NeighboursList.Count(c => c == val);
        }

        private void ApplyRulesAndUpdateBufferMatrix(ref MatrixProcessData MatrixData)
        {

            //Bug Fix: When processing rules we need to process it from the Master Matrix List into the Buffer Matrix List.
            foreach(var CellItem in MatrixData.MatrixList)
            {
                int cellX = CellItem.XPos;
                int cellY = CellItem.YPos;
                bool cellstate = CellItem.CellSate;

                //Get Neighbours list to determine the state of the cell
                var neighboursList = FetchNeighboursList(ref MatrixData, CellItem);

                //Calculate the amount of active and dead neighbours from Master Matrix List.
                int AliveCells = CountNeighbourValues(neighboursList,true);
                int DeadCells = CountNeighbourValues(neighboursList, false);

                //Bug Fix:
                //Apply the rules to set the state of the current cell
                //Write the result of the rules from the Master Matrix List into Buffer Matrix List.
                if (cellstate)
                {//Live Cell

                    //Rule. Live Cell: 1
                    if (AliveCells == 0 || AliveCells == 1)
                    {
                        //Make Dead
                        MatrixData.BufferMatrixList.Find(x => x.XPos == cellX && x.YPos == cellY).CellSate = false;
                        continue;
                    }

                    //Rule. Live Cell: 2
                    if (AliveCells == 2 || AliveCells == 3)
                    {
                        //Make Alive
                        MatrixData.BufferMatrixList.Find(x => x.XPos == cellX && x.YPos == cellY).CellSate = true;
                        continue;
                    }

                    //Rule. Live Cell: 3
                    if (AliveCells >= 4)
                    {
                        //Make Dead
                        MatrixData.BufferMatrixList.Find(x => x.XPos == cellX && x.YPos == cellY).CellSate = false;
                        continue;
                    }

                }
                else
                {//Dead Cell

                    //Rule. Dead Cell: 1
                    if (AliveCells == 3)
                    {
                        //Make Alive
                        MatrixData.BufferMatrixList.Find(x => x.XPos == cellX && x.YPos == cellY).CellSate = true;
                        continue;
                    } else
                    {//Rule. Dead Cell: 2
                        //Make Dead
                        MatrixData.BufferMatrixList.Find(x => x.XPos == cellX && x.YPos == cellY).CellSate = false;
                        continue;
                    }
                }
            }

        }


        public void ProcessBufferMatrix(ref MatrixProcessData MatrixData)
        {
            MatrixData.BufferMatrixList.Clear();

            //Bug Fix: 
            //Initialise Buffer Matrix and set it equal to the master matrix list except with default cell state as false.
            foreach (var CellItem in MatrixData.MatrixList)
            {
                //Override the current cell state and set to being a dead cell
                CellEntry newCellItem = new CellEntry();
                newCellItem.XPos = CellItem.XPos;
                newCellItem.YPos = CellItem.YPos;
                newCellItem.CellSate = false;

                MatrixData.BufferMatrixList.Add(newCellItem);
            }

            //Do GOL Rules processing here on the Buffer Matrix..
            ApplyRulesAndUpdateBufferMatrix(ref MatrixData);


            //Swap Buffer Matrix back to GOL Matrix
            MatrixData.MatrixList.Clear();

            foreach (var CellItem in MatrixData.BufferMatrixList)
            {
                MatrixData.MatrixList.Add(CellItem);
            }

        }

    }
}
