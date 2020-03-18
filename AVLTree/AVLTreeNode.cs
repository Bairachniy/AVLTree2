using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVLTree
{
    class AVLTreeNode<T> : IComparable<T> where T : IComparable
    {
        AVLTree<T> _tree;
        AVLTreeNode<T> _left;
        AVLTreeNode<T> _right;
        public T Value
        {
            get;
            private set;
        }
        public AVLTreeNode<T> Parent
        {
            get;
            internal set;
        }
        public AVLTreeNode(T value, AVLTreeNode<T> parent, AVLTree<T> tree)
        {
            Value = value;
            Parent = parent;
            _tree = tree;
        }
        public AVLTreeNode<T> Left
        {
            get
            {
                return _left;
            }
            internal set
            {
                _left = value;
                if (_left != null)
                    _left.Parent = this;
            }
        }
        public AVLTreeNode<T> Right
        {
            get
            {
                return _right;
            }
            internal set
            {
                _right = value;
                if (_right != null)
                    _right.Parent = this;
            }
        }
        public int CompareTo(T other)
        {
            return Value.CompareTo(other);
        }
        #region Balance

        internal void Balance()
        {
            if (State == TreeState.RightHeavy)
            {
                if (Right != null && Right.BalanceFactor < 0)
                {
                    LeftRightRotation();
                }

                else
                {
                    LeftRotation();
                }
            }
            else if (State == TreeState.LeftHeavy)
            {
                if (Left != null && Left.BalanceFactor > 0)
                {
                    RightLeftRotation();
                }
                else
                {
                    RightRotation();
                }
            }
        }
        private int MaxChildHeight(AVLTreeNode<T> node)
        {
            if (node != null)
            {
                return 1 + Math.Max(MaxChildHeight(node.Left), MaxChildHeight(node.Right));
            }

            return 0;
        }

        private int LeftHeight
        {
            get
            {
                return MaxChildHeight(Left);
            }
        }

        private int RightHeight
        {
            get
            {
                return MaxChildHeight(Right);
            }
        }

        private TreeState State
        {
            get
            {
                if (LeftHeight - RightHeight > 1)
                {
                    return TreeState.LeftHeavy;
                }

                if (RightHeight - LeftHeight > 1)
                {
                    return TreeState.RightHeavy;
                }

                return TreeState.Balanced;
            }
        }


        private int BalanceFactor
        {
            get
            {
                return RightHeight - LeftHeight;
            }
        }

        enum TreeState
        {
            Balanced,
            LeftHeavy,
            RightHeavy,
        }

        #endregion

        #region LeftRotation

        private void LeftRotation()
        {

            AVLTreeNode<T> newRoot = Right;
            ReplaceRoot(newRoot);
 
            Right = newRoot.Left;
            
            newRoot.Left = this;
        }

        #endregion

        #region RightRotation

        private void RightRotation()
        {
           
            AVLTreeNode<T> newRoot = Left;
            ReplaceRoot(newRoot);

            Left = newRoot.Right;
    
            newRoot.Right = this;
        }

        #endregion

        #region LeftRightRotation

        private void LeftRightRotation()
        {
            Right.RightRotation();
            LeftRotation();
        }
        #endregion

        #region RightLeftRotation

        private void RightLeftRotation()
        {
            Left.LeftRotation();
            RightRotation();
        }
        #endregion

        #region Перемещение корня

        private void ReplaceRoot(AVLTreeNode<T> newRoot)
        {
            if (this.Parent != null)
            {
                if (this.Parent.Left == this)
                {
                    this.Parent.Left = newRoot;
                }
                else if (this.Parent.Right == this)
                {
                    this.Parent.Right = newRoot;
                }
            }
            else
            {
                _tree.Head = newRoot;
            }

            newRoot.Parent = this.Parent;
            this.Parent = newRoot;
        }

        #endregion
    }
}

