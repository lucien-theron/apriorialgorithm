using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AprioriAlgorithm
{
    class Program
    {
        private ArrayList items; //unique items in Data

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            Program main = new Program();
            double min_sup = 20; //20% min support
            double con_lvl = 60;
            ArrayList D = main.databaseOfTransactions();
            main.AprioriAlgorithm(D, min_sup, con_lvl);

            Console.ReadLine();
        }

        private void AprioriAlgorithm(ArrayList D, double min_sup, double con_lvl)
        {
            display("The transaction list", D, false, false, true);
            display("List of unique items in the transaction list", items, false, false, false);
            ArrayList storageL = new ArrayList(); //preserve L lists for generating association rules
            ArrayList L = find_frequent_1_itemsets(D, min_sup);
            storageL.Add(L);
            int k = 2;
            while (L.Count != 0)
            {
                ArrayList Ck = apriori_gen(L, (k - 1));
                foreach (Candidate t in D)// scan D for counts
                {
                    ArrayList powersets = generatePowersets(t);
                    foreach (Candidate c in Ck)
                    {
                        foreach (Candidate ps in powersets)
                        {
                            if (c.Equals(ps))
                            {
                                c.incrementCount();
                            }
                        }
                    }
                }
                k++;
                display("C" + (k - 1), Ck, true, false, false);
                L = checkSupport(Ck, min_sup);
                storageL.Add(L);
                display("L" + (k - 1), L, true, false, false);
            }
            ArrayList associationRules = generateAssociationRules(storageL, con_lvl);
            display("Association Rules", associationRules, false, true, false);
        }

        private ArrayList generateAssociationRules(ArrayList storage, double con_lvl)
        {
            ArrayList assosiationRules = new ArrayList();
            foreach (ArrayList al in storage)
            {
                foreach (Candidate c in al)
                {
                    if (c.getItemsSize() > 1)
                    {
                        ArrayList powersets = generatePowersets(c);
                        ArrayList nonEmptySubsets = getNonEmptySubsets(c.getItemsSize(), powersets);
                        foreach (Candidate subset in nonEmptySubsets)
                        {
                            Candidate L = findInStorage(storage, subset);
                            double confidenceLevel = Math.Round((c.getSupport() / L.getSupport()) * 100, 2);//confident now
                            if (confidenceLevel >= con_lvl)
                            {
                                Candidate tempNew = new Candidate();
                                tempNew.copyCandidate(subset);
                                tempNew.setSeperator(tempNew.getItemsSize() - 1);
                                foreach (Item item in c.getItems())
                                {
                                    if (!tempNew.checkForItem(item))
                                    {
                                        tempNew.addItem(item);
                                    }
                                }
                                tempNew.setConfidenceLvl(confidenceLevel);
                                assosiationRules.Add(tempNew);
                            }
                        }
                    }

                }
            }
            return assosiationRules;
        }

        private ArrayList getNonEmptySubsets(int maxSize, ArrayList powerset)
        {
            powerset.RemoveAt(0);
            for (int x = 0; x < powerset.Count; x++)
            {
                Candidate temp = (Candidate)powerset[x];
                if (temp.getItemsSize() == maxSize)
                {
                    powerset.RemoveAt(x);
                }
            }
            return powerset;
        }

        private Candidate findInStorage(ArrayList storage, Candidate toFind)
        {
            foreach (ArrayList l in storage)
            {
                foreach (Candidate c in l)
                {
                    if (c.Equals(toFind))
                    {
                        return c;
                    }
                }
            }
            return null;
        }

        private ArrayList checkSupport(ArrayList list, double min_sup)
        {
            ArrayList temp = new ArrayList();
            foreach (Candidate c in list)
            {
                if (c.getSupport() >= min_sup)
                {
                    temp.Add(c);
                }
                else
                {
                    Console.Write("Removed: ");
                    c.displayItems(true, false);
                }
            }
            return temp;
        }

        private ArrayList generatePowersets(Candidate items)
        {
            ArrayList result = new ArrayList();
            for (int i = 0; i < (1 << items.getItemsSize()); i++)
            {
                Candidate sublist = new Candidate();
                for (int j = 0; j < items.getItemsSize(); j++)
                {
                    if ((i & (1 << j)) != 0)
                    {
                        sublist.addItem(items.getItemAtPos(j));
                    }
                }
                result.Add(sublist);
            }
            return result;
        }

        private void display(string title, ArrayList list, Boolean displaySupport, Boolean displayConfidence, Boolean transactionList)
        {
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine(title + ":");
            Console.WriteLine("------------------------------------------------------------");
            if (list.Count == 0)
            {
                Console.WriteLine("Empty list");
            }
            else
            {
                int count = 0;
                foreach (Candidate candidate in list)
                {
                    if (transactionList == true)
                    {
                        Console.Write("T" + count + ": ");
                        count++;
                    }
                    candidate.displayItems(displaySupport, displayConfidence);
                }
            }
            Console.WriteLine("------------------------------------------------------------");
        }

        private ArrayList databaseOfTransactions() //could be replaced by a reader.
        {
            //build items list, a seperate method could determine unique items from a large list
            items = new ArrayList();

            Item I1 = new Item("I1");
            Item I2 = new Item("I2");
            Item I3 = new Item("I3");
            Item I4 = new Item("I4");
            Item I5 = new Item("I5");

            Candidate Item1 = new Candidate();
            Item1.addItem(I1);
            Candidate Item2 = new Candidate();
            Item2.addItem(I2);
            Candidate Item3 = new Candidate();
            Item3.addItem(I3);
            Candidate Item4 = new Candidate();
            Item4.addItem(I4);
            Candidate Item5 = new Candidate();
            Item5.addItem(I5);

            items.Add(Item1);
            items.Add(Item2);
            items.Add(Item3);
            items.Add(Item4);
            items.Add(Item5);

            //build transaction list
            ArrayList D = new ArrayList();

            Candidate T0 = new Candidate();//T0:    I2,I5,
            T0.addItem(I2);
            T0.addItem(I5);
            D.Add(T0);
            Candidate T1 = new Candidate();//T1:    I2,I4,I5,
            T1.addItem(I2);
            T1.addItem(I4);
            T1.addItem(I5);
            D.Add(T1);
            Candidate T2 = new Candidate();//T2:	I1,I2,I3,I4,I5,
            T2.addItem(I1);
            T2.addItem(I2);
            T2.addItem(I3);
            T2.addItem(I4);
            T2.addItem(I5);
            D.Add(T2);
            Candidate T3 = new Candidate();//T3:	I1,I2,I5,
            T3.addItem(I1);
            T3.addItem(I2);
            T3.addItem(I5);
            D.Add(T3);
            Candidate T4 = new Candidate();//T4:	I2,I3,I4,I5,
            T4.addItem(I2);
            T4.addItem(I3);
            T4.addItem(I4);
            T4.addItem(I5);
            D.Add(T4);
            Candidate T5 = new Candidate();//T5:	I1,I2,I5,
            T5.addItem(I1);
            T5.addItem(I2);
            T5.addItem(I5);
            D.Add(T5);
            Candidate T6 = new Candidate();//T6:	I2,I3,I5,
            T6.addItem(I2);
            T6.addItem(I3);
            T6.addItem(I5);
            D.Add(T6);
            Candidate T7 = new Candidate();//T7:	I3,I5,
            T7.addItem(I3);
            T7.addItem(I5);
            D.Add(T7);
            Candidate T8 = new Candidate();//T8:	I3,I5,
            T8.addItem(I3);
            T8.addItem(I5);
            D.Add(T8);
            Candidate T9 = new Candidate();//T9:	I2,I4,I5,
            T9.addItem(I2);
            T9.addItem(I4);
            T9.addItem(I5);
            D.Add(T9);
            Candidate T10 = new Candidate();//T10:	I1,I3,I5,
            T10.addItem(I1);
            T10.addItem(I3);
            T10.addItem(I5);
            D.Add(T10);
            Candidate T11 = new Candidate();//T11:	I2,
            T11.addItem(I2);
            D.Add(T11);
            Candidate T12 = new Candidate();//T12:	I1,I2,I3,I4,
            T12.addItem(I1);
            T12.addItem(I2);
            T12.addItem(I3);
            T12.addItem(I4);
            D.Add(T12);
            Candidate T13 = new Candidate();//T13:	I1,I3,
            T13.addItem(I1);
            T13.addItem(I3);
            D.Add(T13);
            Candidate T14 = new Candidate();//T14:	I1,I2,I4,I5,
            T14.addItem(I1);
            T14.addItem(I2);
            T14.addItem(I4);
            T14.addItem(I5);
            D.Add(T14);
            return D;
        }

        private ArrayList find_frequent_1_itemsets(ArrayList D, double min_sup)
        {
            ArrayList C1 = items;
            foreach (Candidate t in D)
            {
                ArrayList powersets = generatePowersets(t);
                foreach (Candidate c in C1)
                {
                    foreach (Candidate ps in powersets)
                    {
                        if (c.Equals(ps))
                        {
                            c.incrementCount();
                        }
                    }
                }
            }
            ArrayList L1 = checkSupport(C1, min_sup);

            display("C1", C1, true, false, false);
            display("L1", L1, true, false, false);
            return L1;
        }

        private ArrayList apriori_gen(ArrayList L, int k) //converted
        {
            ArrayList C = new ArrayList();
            foreach (Candidate l1 in L)
            {
                foreach (Candidate l2 in L)
                {
                    if (k == 1)//No candidates are removed from C2 with pruning and is handled seperately
                    {
                        if ((l1.getItemAtPos(k - 1).getSize() < l2.getItemAtPos(k - 1).getSize()))
                        {
                            Candidate c = new Candidate();
                            c.addItem(l1.getItemAtPos(k - 1));
                            c.addItem(l2.getItemAtPos(k - 1));
                            C.Add(c);
                        }
                    }
                    else
                    {
                        Candidate c = new Candidate();
                        if (l1.getItemAtPos(k - 2).getSize() == l2.getItemAtPos(k - 2).getSize() && (l1.getItemAtPos(k - 1).getSize() < l2.getItemAtPos(k - 1).getSize()))
                        {
                            for (int x = 0; x <= (k - 2); x++)
                            {
                                c.addItem(l1.getItemAtPos(x));
                            }
                            c.addItem(l1.getItemAtPos(k - 1));
                            c.addItem(l2.getItemAtPos(k - 1));
                            if (has_infrequent_subset(c, L))
                            {
                                Console.Write("Pruned: ");
                                c.displayItems(false, false);
                            }
                            else
                            {
                                C.Add(c);
                            }
                        }
                    }
                }
            }
            return C;
        }

        private Boolean has_infrequent_subset(Candidate c, ArrayList L) //converted
        {
            ArrayList s = generateSubsets(c);
            foreach (Candidate subset in s)
            {
                if (!containsSubset(subset, L))
                {
                    return true;
                }
            }
            return false;
        }

        private Boolean containsSubset(Candidate subset, ArrayList L) //converted
        {
            foreach (Candidate item in L)
            {
                if (item.Equals(subset))
                {
                    return true;
                }
            }
            return false;
        }

        private ArrayList generateSubsets(Candidate candidate) //converted
        {
            ArrayList subsets = new ArrayList();
            for (int x = 0; x < candidate.getItemsSize(); x++)
            {
                Candidate temp = new Candidate();
                for (int y = 0; y < candidate.getItemsSize(); y++)
                {
                    if (y != x)
                    {
                        temp.addItem(candidate.getItemAtPos(y));
                    }
                }
                subsets.Add(temp);
            }
            return subsets;
        }
    }
}
