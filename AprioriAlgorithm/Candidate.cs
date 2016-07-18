using System;
using System.Collections.Generic;
using System.Linq;

namespace AprioriAlgorithm
{
    class Candidate
    {
        private List<Item> items;
        private int count, seperator;
        private double confidenceLvl;
        public Candidate()
        {
            this.items = new List<Item>();
            this.count = 0;
            this.seperator = -1;
            this.confidenceLvl = 0;
        }

        public Candidate(List<Item> items)
        {
            this.items = items;
            this.count = 0;
            this.seperator = -1;
            this.confidenceLvl = 0;
        }

        public void setSeperator(int seperator)
        {
            this.seperator = seperator;
        }

        public void setConfidenceLvl(double confidenceLvl)
        {
            this.confidenceLvl = confidenceLvl;
        }

        public void incrementCount()
        {
            count++;
        }

        public double getSupport()
        {
            return Math.Round((count / 15.0) * 100, 2);
        }

        public List<Item> getItems()
        {
            return items;
        }

        public void addItem(Item item)
        {
            items.Add(item);
        }

        public Item getItemAtPos(int pos)
        {
            return items[pos];
        }

        public int getItemsSize()
        {
            return items.Count();
        }

        public void copyCandidate(Candidate toCopy)
        {
            foreach(Item item in toCopy.getItems())
            {
                this.items.Add(item);
            }
        }

        public void displayItems(Boolean displaySupport, Boolean displayConfidence)
        {
            if (displayConfidence)
            {
                for(int x = 0; x < items.Count(); x++)
                {
                    if(x==seperator)
                    {
                        Console.Write(items[x].getID() + " -> ");
                    }
                    else
                    {
                        Console.Write(items[x].getID() + " ");
                    }
                }
                Console.Write("Confidence Level: " + confidenceLvl + "%");
                Console.WriteLine();
            }
            else
            {
                foreach (Item item in items)
                {
                    Console.Write(item.getID() + " ");
                }
                if (displaySupport)
                    Console.Write("Support: " + Math.Round((count / 15.0) * 100, 2) + "%");
                Console.WriteLine();
            }

        }

        public int getCount()
        {
            return count;
        }

        public Boolean checkForItem(Item item1)
        {
            foreach(Item item2 in items)
            {
                if(item1.getID().Equals(item2.getID()))
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean checkContains(Candidate candidate)
        {
            foreach(Item item1 in items)
            {
                foreach(Item item2 in candidate.getItems())
                {
                    if(item1.Equals(item2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Boolean Equals(Candidate candidate)
        {
            string stringOne = "";
            foreach(Item item in items)
            {
                stringOne += item.getID();
            }
            string stringTwo = "";
            foreach (Item item in candidate.getItems())
            {
                stringTwo += item.getID();
            }
            if(stringOne.Equals(stringTwo))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
