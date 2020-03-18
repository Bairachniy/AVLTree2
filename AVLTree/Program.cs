using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVLTree
{
    class Program
    {
        static void Main(string[] args)
        {
            AVLTree<int> avlTree = new AVLTree<int>();

            avlTree.Add(1);
            avlTree.Add(2);
            avlTree.Add(3);
            avlTree.Add(4);
            avlTree.Add(5);
            avlTree.Add(6);
            avlTree.Add(7);
            avlTree.Add(8);
            avlTree.Add(9);
            avlTree.Add(11);
            avlTree.Add(12);
            avlTree.Add(13);
            avlTree.Add(19);
            avlTree.Add(18);
            avlTree.Add(17);

            Console.WriteLine( avlTree.CLR() ); 
            //foreach (var i in avlTree)
            //    Console.WriteLine(i);
        }
    }
}
