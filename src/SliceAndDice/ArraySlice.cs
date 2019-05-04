// Copyright (c) Henon 2019

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceAndDice
{
    public class ArraySlice<T> : IEnumerable<T>
    {
        public ArraySlice(IEnumerable<T> data_source, Shape shape = null) : this(data_source.ToArray(), shape) { }

        public ArraySlice(params T[] data) : this(data, null) { }

        public ArraySlice(T[] data, Shape shape = null)
        {
            _data = data;
            Shape = shape ?? new Shape(data.Length);
            GetValueAt = i => data[i];
            SetValueAt = (i, value) => data[i] = value;
            IEnumerator = () => data.AsEnumerable().GetEnumerator();
        }

        public ArraySlice(List<T> data, Shape shape = null)
        {
            _data = data;
            Shape = shape ?? new Shape(data.Count);
            GetValueAt = i => data[i];
            SetValueAt = (i, value) => data[i] = value;
            IEnumerator = () => data.AsEnumerable().GetEnumerator();
        }

        public ArraySlice(ArraySlice<T> data, Shape shape = null)
        {
            _data = data;
            Shape = data.Shape;
            GetValueAt = i => data[i];
            SetValueAt = (i, value) => data[i] = value;
            IEnumerator = data.GetEnumerator;
        }

        public ArraySlice(ArraySlice<T> data, string slices) : this(data, Slice.ParseSlices(slices)) { }

        public ArraySlice(ArraySlice<T> data, Slice[] slices)
        {
            _data = data;
            Shape = new SlicedShape(data.Shape, slices);
            GetValueAt = data.GetValueAt;
            SetValueAt = data.SetValueAt;
            IEnumerator = () => Shape.GetEnumerator(this);
        }

        public static ArraySlice<T> Create(object data, Shape shape = null)
        {
            switch (data)
            {
                case T[] array: return new ArraySlice<T>(array, shape);
                case List<T> array: return new ArraySlice<T>(array, shape);
                case ArraySlice<T> array: return new ArraySlice<T>(array, shape);
                case IEnumerable<T> array: return new ArraySlice<T>(array, shape);
                default:
                    throw new ArgumentException($"Type {data.GetType()} not supported yet?");
            }
        }

        public Shape Shape { get; private set; }
        private object _data; // internal data

        private Func<int, T> GetValueAt;
        private Action<int, T> SetValueAt;

        public T this[params int[] coordinates]
        {
            get => GetValue(coordinates);
            set => SetValue(coordinates, value);
        }

        public ArraySlice<T> this[string slices]
        {
            get => GetSlice(Slice.ParseSlices(slices));
        }

        public ArraySlice<T> GetSlice(string slices) => GetSlice(Slice.ParseSlices(slices));

        public ArraySlice<T> GetSlice(params Slice[] slices)
        {
            return new ArraySlice<T>(this, slices);
        }

        public T GetValue(params int[] coords)
        {
            var index = Shape.GetIndex(coords);
            return GetValueAt(index);
        }

        public void SetValue(int[] coords, T value)
        {
            var index = Shape.GetIndex(coords);
            SetValueAt(index, value);
        }

        public void SetValues(int coord_1d, IEnumerable<T> values) => SetValues(new[] { coord_1d }, values);

        public void SetValues(int[] coords, IEnumerable<T> values)
        {
            var index = Shape.GetIndex(coords);
            foreach (var value in values)
                SetValueAt(index++, value);
        }

        public int Length => Shape[0];
        private Func<IEnumerator<T>> IEnumerator;

        public IEnumerator<T> GetEnumerator()
        {
            return IEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return IEnumerator();
        }

        public ArraySlice<T> Reshape(params int[] dimensions)
        {
            if (Shape.IsSliced)
                return new ArraySlice<T>(this, new Shape(dimensions)); // todo: merge slicing info
            return Create(_data, new Shape(dimensions));
        }

        public static ArraySlice<int> Range(int stop, int start = 0, int step = 1)
        {
            if (step < 1)
                throw new ArgumentException("Step must be > 0. Given: " + step);
            if (stop < start)
                (start, stop) = (stop, start);
            var size = Math.Abs((int)Math.Ceiling((stop - start) / (double)step));
            var data = new int[size];
            int index = 0;
            for (int i = start; i < stop; i += step)
                data[index++] = i;
            return new ArraySlice<int>(data);
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            PrettyPrint(s);
            return s.ToString();
        }

        public string ToString(bool flat=false)
        {
            var s = new StringBuilder();
            PrettyPrint(s, flat);
            return s.ToString();
        }

        private void PrettyPrint(StringBuilder s, bool flat = false)
        {
            if (Shape.Dimensions.Length == 0)
            {
                s.Append($"{GetValue(0)}");
                return;
            }
            if (Shape.Dimensions.Length == 1)
            {
                s.Append("[");
                s.Append(string.Join(", ", this.Select(x => x == null ? "null" : x.ToString())));
                s.Append("]");
                return;
            }
            var size = Shape.Dimensions.First();
            s.Append("[");
            for (int i = 0; i < size; i++)
            {
                var n_minus_one_dim_slice = this.GetSlice(Slice.Index(i));
                n_minus_one_dim_slice.PrettyPrint(s,flat);
                if (i < size - 1)
                {
                    s.Append(", ");
                    if (!flat)
                        s.AppendLine();
                }
            }
            s.Append("]");
        }
    }
}
