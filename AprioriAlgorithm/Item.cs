using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprioriAlgorithm
{
    class Item
    {
        private String ID;
        public Item(String ID)
        {
            this.ID = ID;
        }

        public int getSize()
        {
            String some = ID.Remove(0, 1);
            return Int32.Parse(some);
        }

        public String getID()
        {
            return ID;
        }

    }
}
