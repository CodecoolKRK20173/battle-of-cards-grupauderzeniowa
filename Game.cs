using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;

namespace battle_of_cards_grupauderzeniowa
{
    class Game
    {
        private Deck deck = new Deck();
        private Gambler gambler = new Gambler(500);
        public void Run()
        {
            int ante = 20;
            bool anothergame = true;
            while(anothergame)
            {
                // utworzenie potasowanej talii 52 kart
                deck.Shuffle();
                // utworzenie dwóch "rąk"
                List <Card> dealerHand = new List<Card>();
                List<Card> playerHand = new List<Card>();
                List<Card> twoHAND = new List<Card>();
                twoHAND = deck.handDeck();
                for (int i = 0; i < 10; i++)
                {
                    if (i<5) dealerHand.Add(twoHAND[i]);
                    else playerHand.Add(twoHAND[i]);
                }
                List<Card> dealerHandCover = Turn(dealerHand);
                // karta playera uporzadkowana
                playerHand.Sort(new CardComparer_Value());
                //plansza przed Call ale juz z Ante
                bool beforeCall = true;
                      
                gambler.SetMoney(gambler.GetMoney()-ante);
                DisplayTable.DisplayBoard(dealerHandCover, playerHand, ante, 0, gambler.GetMoney(), beforeCall);
                bool setcal = gambler.SetCall();
                if (setcal) 
                {
                    beforeCall = false;
                    gambler.SetMoney(gambler.GetMoney()- 2* ante);
                    DisplayTable.DisplayBoard(dealerHand, playerHand, ante, 2*ante, gambler.GetMoney(), beforeCall);
                    HandCombination dealer = Analisis.HandAnalizer(dealerHand);
                    HandCombination player = Analisis.HandAnalizer(playerHand);
                    // sprawdzenie czy ręka delera kwlifikuje sie do porównania
                    if (dealer != HandCombination.nothing)
                    {
                        Hand handpower = new Hand(dealer);
                        int outcome = handpower.CompareTo(player);
                        if (outcome == 1)
                        {
                            Console.WriteLine("You lost your Call/Ante");
                            Console.WriteLine("Press any key");
                            Console.ReadKey();
                        } 
                        if(outcome == -1) 
                        {
                            Console.WriteLine("You won aggregate dealer bet plus get back Call/Ante");
                            Console.WriteLine("Press any key");
                            Console.ReadKey();
                            int agregate = 1;
                            string generalhand = player.ToString().Substring(0,2);
                            if (generalhand == "PO") {agregate = 100;}
                            if (generalhand == "po") {agregate = 50;}
                            if (generalhand == "qu") {agregate = 20;}
                            if (generalhand == "fu") {agregate = 7;}
                            if (generalhand == "fl") {agregate = 5;}
                            if (generalhand == "st") {agregate = 4;}
                            if (generalhand == "tr") {agregate = 3;}
                            if (generalhand == "dp") {agregate = 2;}
                            gambler.SetMoney(gambler.GetMoney() + agregate*2*ante + ante);
                        }
                        if(outcome == 0) 
                        {
                            Console.WriteLine("draw, you get back Call/Ante");
                            Console.WriteLine("Press any key");
                            Console.ReadKey();
                            gambler.SetMoney(gambler.GetMoney()+ 3*ante);
                        }
                    }
                    else   //dealer: nothing
                    {
                        bool qualify = false;
                        if (dealerHand[4].Rank == Ranks.Ace || dealerHand[0].Rank == Ranks.King)
                        {
                            qualify = true;
                        }
                        if (qualify)
                        {
                            Hand handpower = new Hand(dealer);
                            int outcome = handpower.CompareTo(player);
                            if (outcome == 1)
                            {
                                Console.WriteLine("Dealer won, you lost your Call/Ante");
                                Console.WriteLine("Press any key");
                                Console.ReadKey();
                            }
                            if(outcome == -1) 
                            {
                                Console.WriteLine ("You won aggregate dealer bet plus get back Call/Ante");
                                Console.WriteLine("Press any key");
                                Console.ReadKey();
                                int agregate = 1;
                                string generalhand = player.ToString().Substring(0,2);
                                if (generalhand == "PO") {agregate = 100;}
                                if (generalhand == "po") {agregate = 50;}
                                if (generalhand == "qu") {agregate = 20;}
                                if (generalhand == "fu") {agregate = 7;}
                                if (generalhand == "fl") {agregate = 5;}
                                if (generalhand == "st") {agregate = 4;}
                                if (generalhand == "tr") {agregate = 3;}
                                if (generalhand == "dp") {agregate = 2;}
                                gambler.SetMoney(gambler.GetMoney() + agregate*2*ante + ante);
                            }
                            if(outcome == 0) 
                            {
                                Console.WriteLine("draw, get back Call/Ante");
                                Console.WriteLine("Press any key");
                                Console.ReadKey();
                                gambler.SetMoney(gambler.GetMoney()+3*ante);
                            }
                        }    
                        else
                        {
                            Console.WriteLine("You get back Call/Ante plus extra Ante");
                            Console.WriteLine("Press any key");
                            Console.ReadKey();
                            gambler.SetMoney(gambler.GetMoney() + 4 *ante);
                        }
                    }
                }
                else
                {
                    Console.Write("Would you like to play? - press Y: ");
                    string choice2 = Console.ReadLine().ToUpper();
                    if (choice2 != "Y")
                    {
                        anothergame = false;
                    } 
                }
                //beforeCall = true;
                if (gambler.GetMoney()<= 0)
                {
                    anothergame = false; 
                } 
            } 
            int finalscore = gambler.GetMoney();
            if (finalscore > 0)
            {
                if (finalscore>500) Console.WriteLine("You gain ${0}. See you next time",finalscore-500);
                else Console.WriteLine("You lost ${0}. Next time will be better", 500 - finalscore);
            }
            else Console.WriteLine("End of the Game. You lost all your money");
            Console.Write("Enter your NickName: ");
            string my_name = Console.ReadLine();
            int my_score = finalscore - 500;
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "./baza.db";
            using(var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();
                /*var delTabCmd = connection.CreateCommand();
                delTabCmd.CommandText = "DROP TABLE karaiby";
                delTabCmd.ExecuteNonQuery();
                var createTabCmd = connection.CreateCommand();
                createTabCmd.CommandText = "CREATE TABLE karaiby(name VARCHAR(10), score INT)";
                createTabCmd.ExecuteNonQuery();*/
                using(var transaction = connection.BeginTransaction())
                {
                    var insertCmd = connection.CreateCommand();
                    insertCmd.CommandText = $"INSERT INTO karaiby VALUES ('{my_name}', {my_score})";
                    insertCmd.ExecuteNonQuery();
                    /*insertCmd.CommandText = "INSERT INTO karaiby VALUES ('Dorota', 120)";
                    insertCmd.ExecuteNonQuery();*/
                    transaction.Commit();
                }
                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = "SELECT name, score FROM karaiby ORDER BY score DESC LIMIT 5";
                using(var reader = selectCmd.ExecuteReader())
                {
                    Console.WriteLine("Up to date scores:");
                    while(reader.Read())
                    {
                        var message = reader.GetString(0) + " " + reader.GetString(1);
                        Console.WriteLine(message);
                    }
                }
            }
        }

        public List<Card> Turn(List<Card> lc)
        {
            Card backside = new Card(Suits.Clubs, Ranks.King, "\U0001F0A0  ");
            List<Card> outcome = new List<Card>();
            outcome.Add(lc[0]);
            for (int i = 0; i<4; i++)
            {
                outcome.Add(backside);
            }
            return outcome;
        }
    }
}