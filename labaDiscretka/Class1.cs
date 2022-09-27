using System;

namespace Mnojestva
{
    public class Set
    {
        public string Name { get; set; }
        public int lowerBound { get=>lowerBound; set=>SetB1(value); }
        public int upperBound { get=>upperBound; set=>SetB2(value); }
        protected List<int> Bounds;
        protected bool changeBound1 = false;
        protected bool changeBound2 = false;
        public Set()
        {
            Bounds = new();
            lowerBound = 0;
            upperBound = 0;
            Name = "Empty Set";
        }
        public Set(string name, int lowerBound, int upperBound)
        {
            addBounds = new();
            Name = name;
            this.lowerBound=lowerBound;
            this.upperBound=upperBound;
        }
        public Set Crossing(Set A,Set B)
        {
            int lowerBound = A.lowerBound>B.lowerBound?A.lowerBound:B.lowerBound;
            int upperBound = A.upperBound<B.upperBound?A.upperBound:B.upperBound;
            List<int> Boundsn = new();
            
            string name = A.Name+" & "+B.Name;
            return new Set(name, lowerBound, upperBound);
        }
        public Set Unification(Set A, Set B)
        {
            int lowerBound = A.lowerBound<B.lowerBound ? A.lowerBound : B.lowerBound;
            int upperBound = A.upperBound>B.upperBound ? A.upperBound : B.upperBound;
            string name = A.Name+" | "+B.Name;
            return new Set(name, lowerBound, upperBound);
        }
        public Set Difference(Set A, Set B)
        {
            int lowerBound = A.lowerBound>B.lowerBound ? A.lowerBound : B.lowerBound-1;
            int upperBound = A.upperBound<B.upperBound ? A.upperBound : B.upperBound-1;
            string name = A.Name+" / "+B.Name;
            return new Set(name, lowerBound, upperBound);
        }
        public Set SymmDifference(Set A, Set B)
        {
            int lowerBound = A.lowerBound>B.lowerBound ? A.lowerBound : B.lowerBound-1;
            int upperBound = A.upperBound<B.upperBound ? A.upperBound : B.upperBound-1;
            string name = A.Name+"  △  "+B.Name;
            return new Set(name, lowerBound, upperBound);
        }
        public bool Contains(int item)
        {
            return lowerBound <= item && upperBound >= item;
        }
        public void addBound(int bound1,int bound2)
        {
            Bounds.Add(bound1);
            Bounds.Add(bound2);
            Bounds.Sort();
        }
        protected void SetB1(int value)
        {
            if(!changeBound2)
            {
                if (changeBound1)
                    Bounds.Remove(lowerBound);
                lowerBound = value;
                Bounds.Add(lowerBound);
            }
            else if(value<=upperBound)
            {
                if (changeBound1)
                    Bounds.Remove(lowerBound);
                lowerBound = value;
                Bounds.Add(lowerBound);
            }
            Bounds.Sort();
            Bounds.RemoveAll(x => x<lowerBound);
            changeBound1 = true;
        }
        protected void SetB2(int value)
        {
            if (!changeBound1)
            {
                if (changeBound1)
                    Bounds.Remove(upperBound);
                upperBound = value;
                Bounds.Add(upperBound);
            }
            else if (value>=lowerBound)
            {
                if (changeBound1)
                    Bounds.Remove(upperBound);
                upperBound = value;
                Bounds.Add(upperBound);
            }
            Bounds.Sort();
            Bounds.RemoveAll(x => x>upperBound);
            changeBound2 = true;
        }
    }
}
