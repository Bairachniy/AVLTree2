using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVLTree
{
    class AVLTree<T>: IEnumerable<T> where T:IComparable
    {
        public AVLTreeNode<T> Head { get; internal set; }
        public IEnumerator<T> InOrderTraversal()
        {
            if (Head != null)
            {
                Stack<AVLTreeNode<T>> stack = new Stack<AVLTreeNode<T>>();
                AVLTreeNode<T> current = Head;
                bool goLeftNext = true;
                stack.Push(current);
                while (stack.Count > 0)
                {
                    if (goLeftNext)
                    {
                        while (current.Left != null)
                        {
                            stack.Push(current);
                            current = current.Left;
                        }
                    }
                    yield return current.Value;
                    if (current.Right != null)
                    {
                        current = current.Right;
                        goLeftNext = true;
                    }
                    else
                    {
                        current = stack.Pop();
                        goLeftNext = false;
                    }
                }
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        // Свойство для корня дерева

        #region Количество узлов дерева
        private int count;
        public int Count
        {
            get { return count; }
            private set { count = value; }
        }
        #endregion

        #region Метод Add

        // Метод добавлет новый узел

        public void Add(T value)
        {
            // Вариант 1:  Дерево пустое - создание корня дерева      
            if (Head == null)
            {
                Head = new AVLTreeNode<T>(value, null, this);
            }

            // Вариант 2: Дерево не пустое - найти место для добавление нового узла.

            else
            {
                AddTo(Head, value);
            }

            Count++;
        }

        // Алгоритм рекурсивного добавления нового узла в дерево.

        private void AddTo(AVLTreeNode<T> node, T value)
        {
            // Вариант 1: Добавление нового значения в дерево. Значение добавлемого узла меньше чем значение текущего узла.      

            if (value.CompareTo(node.Value) < 0)
            {
                //Создание левого узла, если его нет.

                if (node.Left == null)
                {
                    node.Left = new AVLTreeNode<T>(value, node, this);
                }

                else
                {
                    // Переходим к следующему левому узлу
                    AddTo(node.Left, value);
                }
            }
            // Вариант 2: Добавлемое значение больше или равно текущему значению.

            else
            {
                //Создание правого узла, если его нет.         
                if (node.Right == null)
                {
                    node.Right = new AVLTreeNode<T>(value, node, this);
                }
                else
                {
                    // Переход к следующему правому узлу.             
                    AddTo(node.Right, value);
                }
            }
            node.Balance();
        }

        #endregion
        public AVLTreeNode<T> FindNode(T data, AVLTreeNode<T> startWithNode = null)
        {
            startWithNode = startWithNode ?? Head;
            int result;
            return (result = data.CompareTo(startWithNode.Value)) == 0
                ? startWithNode
                : result < 0
                    ? startWithNode.Left == null
                        ? null
                        : FindNode(data, startWithNode.Left)
                    : startWithNode.Right == null
                        ? null
                        : FindNode(data, startWithNode.Right);
        }

        //find
        public bool Contains(T value)
        {
            AVLTreeNode<T> parent;
            return FindWithParent(value, out parent) != null;
        }
        private AVLTreeNode<T> FindWithParent(T value, out AVLTreeNode<T> parent)
        {
            AVLTreeNode<T> current = Head;
            parent = null;
            while (current != null)
            {
                int res = current.CompareTo(value);
                if (res > 0)
                {
                    parent = current;
                    current = current.Left;
                }
                else if (res < 0)
                {
                    parent = current;
                    current = current.Right;
                }
                else
                    break;
            }
            return current;
        }
        //remove
        private AVLTreeNode<T> Find(T value)
        {

            AVLTreeNode<T> current = Head; // помещаем текущий элемент в корень дерева

            // Пока текщий узел на пустой 
            while (current != null)
            {
                int result = current.CompareTo(value); // сравнение значения текущего элемента с искомым значением

                if (result > 0)
                {
                    // Если значение меньшне текущего - переход влево 
                    current = current.Left;
                }
                else if (result < 0)
                {
                    // Если значение больше текщего - переход вправо             
                    current = current.Right;
                }
                else
                {
                    // Элемент найден      
                    break;
                }
            }
            return current;
        }
        public bool Remove(T value)
        {
            AVLTreeNode<T> current;
            current = Find(value); // находим узел с удаляемым значением

            if (current == null) // узел не найден
            {
                return false;
            }

            AVLTreeNode<T> treeToBalance = current.Parent; // баланс дерева относительно узла родителя
            Count--;                                       // уменьшение колиества узлов

            // Вариант 1: Если удаляемый узел не имеет правого потомка      

            if (current.Right == null) // если нет правого потомка
            {
                if (current.Parent == null) // удаляемый узел является корнем
                {
                    Head = current.Left;    // на место корня перемещаем левого потомка

                    if (Head != null)
                    {
                        Head.Parent = null; // убераем ссылку на родителя  
                    }
                }
                else // удаляемый узел не является корнем
                {
                    int result = current.Parent.CompareTo(current.Value);

                    if (result > 0)
                    {
                        // Если значение родительского узла больше значения удаляемого,
                        // сделать левого потомка удаляемого узла, левым потомком родителя.  

                        current.Parent.Left = current.Left;
                    }
                    else if (result < 0)
                    {

                        // Если значение родительского узла меньше чем удаляемого,                 
                        // сделать левого потомка удаляемого узла - правым потомком родительского узла.                 

                        current.Parent.Right = current.Left;
                    }
                }
            }

            // Вариант 2: Если правый потомок удаляемого узла не имеет левого потомка, тогда правый потомок удаляемого узла
            // становится потомком родительского узла.      

            else if (current.Right.Left == null) // если у правого потомка нет левого потомка
            {
                current.Right.Left = current.Left;

                if (current.Parent == null) // текущий элемент является корнем
                {
                    Head = current.Right;

                    if (Head != null)
                    {
                        Head.Parent = null;
                    }
                }
                else
                {
                    int result = current.Parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        // Если значение узла родителя больше чем значение удаляемого узла,                 
                        // сделать правого потомка удаляемого узла, левым потомком его родителя.                 

                        current.Parent.Left = current.Right;
                    }

                    else if (result < 0)
                    {
                        // Если значение родительского узла меньше значения удаляемого,                 
                        // сделать правого потомка удаляемого узла - правым потомком родителя.                 

                        current.Parent.Right = current.Right;
                    }
                }
            }

            // Вариант 3: Если правый потомок удаляемого узла имеет левого потомка,      
            // заместить удаляемый узел, крайним левым потомком правого потомка.     
            else
            {
                // Нахожление крайнего левого узла для правого потомка удаляемого узла.       

                AVLTreeNode<T> leftmost = current.Right.Left;

                while (leftmost.Left != null)
                {
                    leftmost = leftmost.Left;
                }

                // Родительское правое поддерево становится родительским левым поддеревом.         

                leftmost.Parent.Left = leftmost.Right;

                // Присвоить крайнему левому узлу, ссылки на правого и левого потомка удаляемого узла.         
                leftmost.Left = current.Left;
                leftmost.Right = current.Right;

                if (current.Parent == null)
                {
                    Head = leftmost;

                    if (Head != null)
                    {
                        Head.Parent = null;
                    }
                }
                else
                {
                    int result = current.Parent.CompareTo(current.Value);

                    if (result > 0)
                    {
                        // Если значение родительского узла больше значения удаляемого,                 
                        // сделать крайнего левого потомка левым потомком родителя удаляемого узла.                 

                        current.Parent.Left = leftmost;
                    }
                    else if (result < 0)
                    {
                        // Если значение родительского узла, меньше чем значение удаляемого,                 
                        // сделать крайнего левого потомка, правым потомком родителя удаляемого узла.                 

                        current.Parent.Right = leftmost;
                    }
                }
            }

            if (treeToBalance != null)
            {
                treeToBalance.Balance();
            }

            else
            {
                if (Head != null)
                {
                    Head.Balance();
                }
            }

            return true;

        }

        public string CLR()
        {
            string str = "";
            CLR(Head, ref str, true);
            return str;
        }
        private void CLR(AVLTreeNode<T> node, ref string s, bool detailed)
        {
            /*
             Аргументы метода:
             1. TreeNode node - текущий "элемент дерева" (ref  передача по ссылке)       
             2. ref string s - строка, в которой накапливается результат (ref - передача по ссылке)
            */
            if (node != null)
            {
                if (detailed)
                    s += "    получили значение " + node.Value.ToString() + Environment.NewLine;
                else
                    s += node.Value.ToString() + " "; // запомнить текущее значение
                if (detailed) s += "    обходим левое поддерево" + Environment.NewLine;
                CLR(node.Left, ref s, detailed); // обойти левое поддерево
                if (detailed) s += "    обходим правое поддерево" + Environment.NewLine;
                CLR(node.Right, ref s, detailed); // обойти правое поддерево
            }
            else if (detailed) s += "    значение отсутствует - null" + Environment.NewLine;
        }

        //#region Print
        //class NodeInfo
        //{
        //    public AVLTree<T> Node;
        //    public string Text;
        //    public int StartPos;
        //    public int Size { get { return Text.Length; } }
        //    public int EndPos { get { return StartPos + Size; } set { StartPos = value - Size; } }
        //    public NodeInfo Parent, Left, Right;
        //}

        //public static void Print(this AVLTree<T> root, int topMargin = 2, int leftMargin = 2)
        //{
        //    if (root == null) return;
        //    int rootTop = Console.CursorTop + topMargin;
        //    var last = new List<NodeInfo>();
        //    var next = root;
        //    for (int level = 0; next != null; level++)
        //    {
        //        var item = new NodeInfo { Node = next, Text = next.Value.ToString() };
        //        if (level < last.Count)
        //        {
        //            item.StartPos = last[level].EndPos + 1;
        //            last[level] = item;
        //        }
        //        else
        //        {
        //            item.StartPos = leftMargin;
        //            last.Add(item);
        //        }
        //        if (level > 0)
        //        {
        //            item.Parent = last[level - 1];
        //            if (next == item.Parent.Node.Left)
        //            {
        //                item.Parent.Left = item;
        //                item.EndPos = Math.Max(item.EndPos, item.Parent.StartPos);
        //            }
        //            else
        //            {
        //                item.Parent.Right = item;
        //                item.StartPos = Math.Max(item.StartPos, item.Parent.EndPos);
        //            }
        //        }
        //        next = next.Left ?? next.Right;
        //        for (; next == null; item = item.Parent)
        //        {
        //            Print(item, rootTop + 2 * level);
        //            if (--level < 0) break;
        //            if (item == item.Parent.Left)
        //            {
        //                item.Parent.StartPos = item.EndPos;
        //                next = item.Parent.Node.Right;
        //            }
        //            else
        //            {
        //                if (item.Parent.Left == null)
        //                    item.Parent.EndPos = item.StartPos;
        //                else
        //                    item.Parent.StartPos += (item.StartPos - item.Parent.EndPos) / 2;
        //            }
        //        }
        //    }
        //    Console.SetCursorPosition(0, rootTop + 2 * last.Count - 1);
        //}

        //private static void Print(NodeInfo item, int top)
        //{
        //    SwapColors();
        //    Print(item.Text, top, item.StartPos);
        //    SwapColors();
        //    if (item.Left != null)
        //        PrintLink(top + 1, "┌", "┘", item.Left.StartPos + item.Left.Size / 2, item.StartPos);
        //    if (item.Right != null)
        //        PrintLink(top + 1, "└", "┐", item.EndPos - 1, item.Right.StartPos + item.Right.Size / 2);
        //}

        //private static void PrintLink(int top, string start, string end, int startPos, int endPos)
        //{
        //    Print(start, top, startPos);
        //    Print("─", top, startPos + 1, endPos);
        //    Print(end, top, endPos);
        //}

        //private static void Print(string s, int top, int left, int right = -1)
        //{
        //    Console.SetCursorPosition(left, top);
        //    if (right < 0) right = left + s.Length;
        //    while (Console.CursorLeft < right) Console.Write(s);
        //}

        //private static void SwapColors()
        //{
        //    var color = Console.ForegroundColor;
        //    Console.ForegroundColor = Console.BackgroundColor;
        //    Console.BackgroundColor = color;
        //}
        //#endregion
    }
}
