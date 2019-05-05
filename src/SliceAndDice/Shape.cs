// Note: the Shape class has been taken from SciSharp/NumSharp and enhanced/changed
// Copyright (c) SciSharp 2018
// Copyright (c) 2019, Henon <meinrad.recheis@gmal.com>

using System;
using System.Collections.Generic;
using System.Linq;

namespace SliceAndDice
{
    public class Shape
    {
        /// <summary>
        /// 0: Row major
        /// 1: Column major
        /// </summary>
        private int _layout=0;
        //public string Order => layout == 1 ? "F" : "C";

        public int NDim => Dimensions.Length;

        public int[] Dimensions { get; protected set; }

        public int[] Strides { get; protected set; }

        private int size;
        public int Size => size;

        public Shape(params int[] dims)
        {
            Reshape(dims);
        }

        public virtual bool IsSliced => false;

        public int this[int dim]
        {
            get
            {
                return Dimensions[dim];
            }
            set
            {
                Dimensions[dim] = value;
            }
        }

        protected void _SetDimOffset()
        {
            if (Dimensions.Length == 0)
                return;

            if (_layout == 0)
            {
                Strides[Strides.Length - 1] = 1;
                for (int idx = Strides.Length - 1; idx >= 1; idx--)
                    Strides[idx - 1] = Strides[idx] * Dimensions[idx];
            }
            else
            {
                Strides[0] = 1;
                for (int idx = 1; idx < Strides.Length; idx++)
                    Strides[idx] = Strides[idx - 1] * Dimensions[idx - 1];
            }
        }

        /// <summary>
        /// get index by coordinates
        /// for example: 2 x 2 row major
        /// [[1, 2, 3], [4, 5, 6]]
        /// GetIndexInShape(0, 1) = 1
        /// GetIndexInShape(1, 1) = 5
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public virtual int GetIndex(params int[] coords)
        {
            if (NDim == 0 && coords.Length == 1)
                return coords[0];

            int idx = 0;

            for (int i = 0; i < coords.Length; i++)
                idx += Strides[i] * coords[i];

            return idx;
        }

        /// <summary>
        /// get coordinates in shape by index
        /// [[1, 2, 3], [4, 5, 6]]
        /// GetCoords(1) = (0, 1)
        /// GetCoords(4) = (1, 1)
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        public int[] GetCoords(int select)
        {
            int[] dimIndexes = null;
            if (Strides.Length == 1)
                dimIndexes = new int[] { select };

            else if (_layout == 0)
            {
                int counter = select;
                dimIndexes = new int[Strides.Length];

                for (int idx = 0; idx < Strides.Length; idx++)
                {
                    dimIndexes[idx] = counter / Strides[idx];
                    counter -= dimIndexes[idx] * Strides[idx];
                }
            }
            else
            {
                int counter = select;
                dimIndexes = new int[Strides.Length];

                for (int idx = Strides.Length - 1; idx > -1; idx--)
                {
                    dimIndexes[idx] = counter / Strides[idx];
                    counter -= dimIndexes[idx] * Strides[idx];
                }
            }

            return dimIndexes;
        }

        //public void ChangeTensorLayout(string order = "C")
        //{
        //    layout = order == "C" ? 0 : 1;
        //    Strides = new int[Dimensions.Length];
        //    _SetDimOffset();
        //}

        private void Reshape(params int[] dims)
        {
            Dimensions = dims;
            Strides = new int[Dimensions.Length];

            size = 1;

            for (int idx = 0; idx < dims.Length; idx++)
                size *= dims[idx];
            _SetDimOffset();
        }

        public static int GetSize(int[] dims)
        {
            int size = 1;

            for (int idx = 0; idx < dims.Length; idx++)
                size *= dims[idx];

            return size;
        }
        public static int[] GetShape(int[] dims, int axis = -1)
        {
            switch (axis)
            {
                case -1:
                    return dims;
                case 0:
                    return dims.Skip(1).Take(dims.Length - 1).ToArray();
                case 1:
                    return new int[] { dims[0] }.Concat(dims.Skip(2).Take(dims.Length - 2)).ToArray();
                case 2:
                    return dims.Take(2).ToArray();
                default:
                    throw new NotImplementedException($"GetCoordinates shape: {string.Join(", ", dims)} axis: {axis}");
            }
        }

        public virtual IEnumerator<T> GetEnumerator<T>(ArraySlice<T> array)
        {
            return array.GetEnumerator();
        }

        //public static implicit operator int[] (Shape shape) => shape.Dimensions;
        //public static implicit operator Shape(int[] dims) => new Shape(dims);

        //public static implicit operator int(Shape shape) => shape.Size;
        //public static implicit operator Shape(int dim) => new Shape(dim);

        //public static implicit operator (int, int) (Shape shape) => shape.Dimensions.Length == 2 ? (shape.Dimensions[0], shape.Dimensions[1]) : (0, 0);
        //public static implicit operator Shape((int, int) dims) => new Shape(dims.Item1, dims.Item2);

        //public static implicit operator (int, int, int) (Shape shape) => shape.Dimensions.Length == 3 ? (shape.Dimensions[0], shape.Dimensions[1], shape.Dimensions[2]) : (0, 0, 0);
        //public static implicit operator Shape((int, int, int) dims) => new Shape(dims.Item1, dims.Item2, dims.Item3);

        //public static implicit operator (int, int, int, int) (Shape shape) => shape.Dimensions.Length == 4 ? (shape.Dimensions[0], shape.Dimensions[1], shape.Dimensions[2], shape.Dimensions[3]) : (0, 0, 0, 0);
        //public static implicit operator Shape((int, int, int, int) dims) => new Shape(dims.Item1, dims.Item2, dims.Item3, dims.Item4);

        #region Equality

        public static bool operator ==(Shape a, Shape b)
        {
            if (b is null) return false;
            return Enumerable.SequenceEqual(a.Dimensions, b?.Dimensions);
        }

        public static bool operator !=(Shape a, Shape b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Shape))
                return false;
            return Enumerable.SequenceEqual(Dimensions, ((Shape)obj).Dimensions);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        #endregion

        public override string ToString()
        {
            return "(" + string.Join(", ", Dimensions) + ")";
        }

    }
}
