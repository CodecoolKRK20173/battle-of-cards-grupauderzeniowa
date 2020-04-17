using System;
using System.Collections.Generic;
using System.Linq;

namespace battle_of_cards_grupauderzeniowa
{
    public static class Analisis
    {
        public static HandCombination HandAnalizer(List<Card> hand)
        {
            // find combination with the same color: flush, poker, POKER
            hand.Sort(new CardComparer_Value());
            Card first = hand[0];
            int counter = 0;
            for (int i = 1; i < 5; i++)
            {
                if (first.Suit != hand[i].Suit) break;
                counter +=1;
            }
            if (counter == 4)  //ten sam kolor  
            {
                // sprawdzamy sekwencję
                if (hand[0].Rank == Ranks.King && hand[1].Rank == Ranks.Queen && hand[2].Rank == Ranks.Jack 
                && hand[3].Rank == Ranks.Ten && hand[4].Rank == Ranks.Ace ) return HandCombination.POKER;
                counter = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (hand[i].Rank != hand[i+1].Rank -1) break;
                    counter += 1;
                }
                if (counter == 4)
                {
                    if (hand[0].Rank == Ranks.King) return HandCombination.poker_K;
                    if (hand[0].Rank == Ranks.Queen) return HandCombination.poker_Q;
                    if (hand[0].Rank == Ranks.Jack) return HandCombination.poker_J;
                    if (hand[0].Rank == Ranks.Ten) return HandCombination.poker_10;
                    if (hand[0].Rank == Ranks.Nine) return HandCombination.poker_9;
                    if (hand[0].Rank == Ranks.Eight) return HandCombination.poker_8;
                    if (hand[0].Rank == Ranks.Seven) return HandCombination.poker_7;
                    if (hand[0].Rank == Ranks.Six) return HandCombination.poker_6;
                    else return HandCombination.poker_5;
                }
                else
                {
                    if (hand[4].Rank == Ranks.Ace) return HandCombination.flush_A;
                    if (hand[0].Rank == Ranks.King) return HandCombination.flush_K;
                    if (hand[0].Rank == Ranks.Queen) return HandCombination.flush_Q;
                    if (hand[0].Rank == Ranks.Jack) return HandCombination.flush_J;
                    if (hand[0].Rank == Ranks.Ten) return HandCombination.flush_10;
                    if (hand[0].Rank == Ranks.Nine) return HandCombination.flush_9;
                    if (hand[0].Rank == Ranks.Eight) return HandCombination.flush_8;
                    else return HandCombination.flush_7;
                } 
            }
            else  //różne kolory
            {
                // sprawdzamy sekwencję 
                if (hand[0].Rank == Ranks.King && hand[1].Rank == Ranks.Queen && hand[2].Rank == Ranks.Jack 
                && hand[3].Rank == Ranks.Ten && hand[4].Rank == Ranks.Ace ) return HandCombination.straight_A;
                counter = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (hand[i].Rank != hand[i+1].Rank -1) break;
                }
                if (counter == 4)
                {
                    if (hand[0].Rank == Ranks.King) return HandCombination.straight_K;
                    if (hand[0].Rank == Ranks.Queen) return HandCombination.straight_Q;
                    if (hand[0].Rank == Ranks.Jack) return HandCombination.straight_J;
                    if (hand[0].Rank == Ranks.Ten) return HandCombination.straight_10;
                    if (hand[0].Rank == Ranks.Nine) return HandCombination.straight_9;
                    if (hand[0].Rank == Ranks.Eight) return HandCombination.straight_8;
                    if (hand[0].Rank == Ranks.Seven) return HandCombination.straight_7;
                    if (hand[0].Rank == Ranks.Six) return HandCombination.straight_6;
                    else return HandCombination.straight_5;
                } 
                else
                {
                    // porównujemy te same wartości
                    Dictionary<Ranks,int> mydict = new Dictionary<Ranks,int>();
                    for (int i = 0; i < 5; i++)
                    {
                        if (!mydict.ContainsKey(hand[i].Rank))
                        {
                            mydict[hand[i].Rank] = 1;
                        }
                        else
                        {
                            mydict[hand[i].Rank] += 1;
                        }
                    }
                    var values = mydict.Values;
                    List<int> lvalues = new List<int>();
                    foreach (int item in values)
                    {
                        lvalues.Add(item);
                    }
                    
                    if (lvalues.Contains(4))
                    {
                        var myKey = mydict.FirstOrDefault(x=>x.Value == 4).Key;
                        if (myKey == Ranks.Ace) return HandCombination.quads_A;
                        if (myKey == Ranks.King) return HandCombination.quads_K;
                        if (myKey == Ranks.Queen) return HandCombination.quads_Q;
                        if (myKey == Ranks.Jack) return HandCombination.quads_J;
                        if (myKey == Ranks.Ten) return HandCombination.quads_10;
                        if (myKey == Ranks.Nine) return HandCombination.quads_9;
                        if (myKey == Ranks.Eight) return HandCombination.quads_8;
                        if (myKey == Ranks.Seven) return HandCombination.quads_7;
                        if (myKey == Ranks.Six) return HandCombination.quads_6;
                        if (myKey == Ranks.Five) return HandCombination.quads_5;
                        if (myKey == Ranks.Four) return HandCombination.quads_4;
                        if (myKey == Ranks.Three) return HandCombination.quads_3;
                        if (myKey == Ranks.Two) return HandCombination.quads_2;
                        
                    }
                    if (lvalues.Contains(3) && lvalues.Contains(2)) 
                    {
                        var myKey = mydict.FirstOrDefault(x=>x.Value == 3).Key;
                        if (myKey == Ranks.Ace) return HandCombination.full_A;
                        if (myKey == Ranks.King) return HandCombination.full_K;
                        if (myKey == Ranks.Queen) return HandCombination.full_Q;
                        if (myKey == Ranks.Jack) return HandCombination.full_J;
                        if (myKey == Ranks.Ten) return HandCombination.full_10;
                        if (myKey == Ranks.Nine) return HandCombination.full_9;
                        if (myKey == Ranks.Eight) return HandCombination.full_8;
                        if (myKey == Ranks.Seven) return HandCombination.full_7;
                        if (myKey == Ranks.Six) return HandCombination.full_6;
                        if (myKey == Ranks.Five) return HandCombination.full_5;
                        if (myKey == Ranks.Four) return HandCombination.full_4;
                        if (myKey == Ranks.Three) return HandCombination.full_3;
                        if (myKey == Ranks.Two) return HandCombination.full_2;
                    }
                    if (lvalues.Contains(3))
                    {
                        var myKey = mydict.FirstOrDefault(x=>x.Value == 3).Key;
                        if (myKey == Ranks.Ace) return HandCombination.triplets_A;
                        if (myKey == Ranks.King) return HandCombination.triplets_K;
                        if (myKey == Ranks.Queen) return HandCombination.triplets_Q;
                        if (myKey == Ranks.Jack) return HandCombination.triplets_J;
                        if (myKey == Ranks.Ten) return HandCombination.triplets_10;
                        if (myKey == Ranks.Nine) return HandCombination.triplets_9;
                        if (myKey == Ranks.Eight) return HandCombination.triplets_8;
                        if (myKey == Ranks.Seven) return HandCombination.triplets_7;
                        if (myKey == Ranks.Six) return HandCombination.triplets_6;
                        if (myKey == Ranks.Five) return HandCombination.triplets_5;
                        if (myKey == Ranks.Four) return HandCombination.triplets_4;
                        if (myKey == Ranks.Three) return HandCombination.triplets_3;
                        if (myKey == Ranks.Two) return HandCombination.triplets_2; 
                    } 
                    if (lvalues.Contains(2))
                    {
                        lvalues.Sort();
                        lvalues.Reverse();
                        if (lvalues[0]== lvalues[1])  //pairs
                        {
                            var myBigger = mydict.FirstOrDefault(x=>x.Value == 2).Key;
                            for (int i = 0; i<3; i++)
                            {
                                if (mydict.Values.ElementAt(i)==2)
                                {
                                    if (mydict.Keys.ElementAt(i)> myBigger)
                                    {
                                        myBigger = mydict.Keys.ElementAt(i);
                                    }
                                }
                            }
                            if (myBigger == Ranks.Ace) return HandCombination.dpair_A;
                            if (myBigger == Ranks.King) return HandCombination.dpair_K;
                            if (myBigger == Ranks.Queen) return HandCombination.dpair_Q;
                            if (myBigger == Ranks.Jack) return HandCombination.dpair_J;
                            if (myBigger == Ranks.Ten) return HandCombination.dpair_10;
                            if (myBigger == Ranks.Nine) return HandCombination.dpair_9;
                            if (myBigger == Ranks.Eight) return HandCombination.dpair_8;
                            if (myBigger == Ranks.Seven) return HandCombination.dpair_7;
                            if (myBigger == Ranks.Six) return HandCombination.dpair_6;
                            if (myBigger == Ranks.Five) return HandCombination.dpair_5;
                            if (myBigger == Ranks.Four) return HandCombination.dpair_4;
                            else return HandCombination.dpair_3;
                        } 
                        else 
                        {
                            var myKey = mydict.FirstOrDefault(x=>x.Value == 2).Key;
                            if (myKey == Ranks.Ace) return HandCombination.pair_A;
                            if (myKey == Ranks.King) return HandCombination.pair_K;
                            if (myKey == Ranks.Queen) return HandCombination.pair_Q;
                            if (myKey == Ranks.Jack) return HandCombination.pair_J;
                            if (myKey == Ranks.Ten) return HandCombination.pair_10;
                            if (myKey == Ranks.Nine) return HandCombination.pair_9;
                            if (myKey == Ranks.Eight) return HandCombination.pair_8;
                            if (myKey == Ranks.Seven) return HandCombination.pair_7;
                            if (myKey == Ranks.Six) return HandCombination.pair_6;
                            if (myKey == Ranks.Five) return HandCombination.pair_5;
                            if (myKey == Ranks.Four) return HandCombination.pair_4;
                            if (myKey == Ranks.Three) return HandCombination.pair_3;
                            if (myKey == Ranks.Two) return HandCombination.pair_2;
                        }
                    }
                } 
            }
            return HandCombination.nothing;
        }
    }
}