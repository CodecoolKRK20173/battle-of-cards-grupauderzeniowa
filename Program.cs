using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;

namespace battle_of_cards_grupauderzeniowa
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Run();
        }
    }
}
