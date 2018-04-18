using System;
using GameOfLife.Process.Classes;

namespace GameOfLife
{
    class Program
    {
        static void Main(string[] args)
        {
            GOLEngine GOL = new GOLEngine(50,25,100);

            //Lets start the game.
            GOL.StartGOL();

            Console.Clear();
            Console.WriteLine("Game Of Life Ended!! Press Enter.");
            Console.ReadLine();
        }
    }
}
