
namespace Sets
{
    public static class Universum
    {
        static public string name { get; set; }
        static public List<int> elements = new();
    }
    public class Set
    {
        public string Name { get; set; }
        protected List<int> elements;
        public Set()
        {
            Name = "Empty Set";
            elements = new();
        }
        public Set(string name,params int[] x)
        {
            elements = new(x.Length);

            Name = name;

            foreach (int x1 in x)
                elements.Add(x1);
        }
        public void Add(int elem)
        {
            if (Universum.elements.Contains(elem) && !elements.Contains(elem))
            {
                elements.Add(elem);
            }
            else if (!Universum.elements.Contains(elem))
            {
                throw new Exception("Element dont exist in U");
            }
            else
                throw new Exception("Element already in the set");
        }
        public Set Crossing(Set A,Set B)
        {
            List<int> elements2 = new();
            foreach(int x in A.elements)
            {
                if(B.Contains(x))
                    elements2.Add(x);
            }
            return new Set(A.Name+" & "+B.Name,elements2.ToArray());
        }
        public Set Unification(Set A, Set B)
        {
            List<int> elements2 = new(A.elements.Count);
            foreach(int x in A.elements)
            {
                if (!elements2.Contains(x))
                    elements2.Add(x);
            }
            foreach (int x in B.elements)
            {
                if (!elements2.Contains(x))
                    elements2.Add(x);
            }
            return new Set(A.Name+" | "+B.Name,elements2.ToArray());
        }
        public Set Difference(Set A, Set B)
        {
            List<int> elements2 = new();
            foreach (int x in A.elements)
            {
                if (!elements2.Contains(x)  && !B.elements.Contains(x))
                    elements2.Add(x);
            }
            return new Set(A.Name+" | "+B.Name, elements2.ToArray());
        }
        public Set SymmDifference(Set A, Set B)
        {
            List<int> elements2 = new();
            foreach (int x in A.elements)
            {
                if (!elements2.Contains(x)  && !B.elements.Contains(x))
                    elements2.Add(x);
            }
            foreach (int x in B.elements)
            {
                if (!elements2.Contains(x)  && !A.elements.Contains(x))
                    elements2.Add(x);
            }
            return new Set(A.Name+" | "+B.Name, elements2.ToArray());
        }
        public Set Addition()
        {
            List<int> elements2 = new();
            foreach(int x in Universum.elements)
            {
                if (!elements.Contains(x))
                    elements2.Add(x);
            }
            return new Set("!"+Name, elements2.ToArray());
        }
        public bool Contains(int item)
        {
            return elements.Contains(item);
        }
    }
}
