// Copyright (c) Henon 2019

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceAndDice
{
    public class SlicedShape : Shape
    {
        private int[] _reduced_dimensions;

        public override bool IsSliced => true;

        public SlicedShape(Shape shape, Slice[] slices)
        {
            OriginalShape = shape;
            // we need at least one slice per dimension in order to correctly handle multi-dimensional arrays, if not given, extend by Slice.All() which returns the whole dimension
            var tempslices = slices;
            if (slices == null)
                tempslices = new Slice[0];
            slices = new Slice[shape.NDim];
            for (int dim = 0; dim < shape.NDim; dim++)
            {
                if (tempslices.Length > dim)
                    slices[dim] = tempslices[dim] ?? Slice.All(); // <-- allow null for Slice.All()
                else
                    slices[dim] = Slice.All();
            }

            for (int dim = 0; dim < shape.NDim; dim++)
            {
                var slice = slices[dim] ?? Slice.All();                slices[dim] = slice; // make sure to overwrite potential nulls
                var size = shape.Dimensions[dim];
                if (slice.IsIndex)
                {
                    // special case: reduce this dimension
                    if (slice.Start < 0 || slice.Start >= size)
                        throw new IndexOutOfRangeException(
                            $"Index {slice.Start} is out of bounds for axis {dim} with size {size}");
                }
                slice.Start = Math.Max(0, slice.Start ?? 0);
                slice.Stop = Math.Min(size, slice.Stop ?? size);
            }
            Slices = slices;
            UnreducedShape = new Shape(shape.Dimensions.Select((dim, i) => slices[i].GetSize(dim)).ToArray());
            Dimensions = shape.Dimensions.Select((dim, i) => slices[i].GetSize(dim))
                .Where((dim, i) => !slices[i].IsIndex).ToArray();
        }

        public Slice[] Slices { get; private set; }
        public Shape OriginalShape { get; private set; }
        public Shape UnreducedShape { get; private set; }

        public override int GetIndex(params int[] coords)
        {
            var base_coords = ProjectCoords(coords, Slices);
            return OriginalShape.GetIndex(base_coords);
        }

        private int[] ProjectCoords(int[] coords, Slice[] slices)
        {
            var sliced_coords = new int[slices.Length];
            if (coords.Length < slices.Length)
            {
                // special case indexing into dimenionality reduced slice
                // the user of this view doesn't know the dims have been reduced so we have to augment the indices accordingly
                int coord_index = 0;
                for (int i = 0; i < slices.Length; i++)
                {
                    var slice = slices[i];
                    if (slice.IsIndex)
                    {
                        sliced_coords[i] = slice.Start.Value;
                        continue;
                    }
                    var idx = coord_index < coords.Length ? coords[coord_index] : 0;
                    coord_index++;
                    sliced_coords[i] = ProjectCoord(idx, slice);
                }
                return sliced_coords;
            }
            // normal case
            for (int i = 0; i < coords.Length; i++)
            {
                var idx = coords[i];
                var slice = slices[i];
                sliced_coords[i] = ProjectCoord(idx, slice);
            }
            return sliced_coords;
        }

        private int ProjectCoord(int idx, Slice slice)
        {
            var start = slice.Step > 0 ? slice.Start.Value : Math.Max(0, slice.Stop.Value - 1);
            //var stop = slice.Step > 0 ? slice.Stop.Value : Math.Max(0, slice.Start.Value - 1);
            return start + idx * slice.Step;
        }

        public override IEnumerator<T> GetEnumerator<T>(ArraySlice<T> data)
        {
            // since the sliced array is a subset of the original we have to copy here
            int size = UnreducedShape.Size;
            // the algorithm is split into 1-D and N-D because for 1-D we need not go through shape.GetDimIndexOutShape
            if (Slices.Length == 1)
            {
                for (var i = 0; i < size; i++)
                    yield return data.GetValue(i);
            }
            else
            {
                for (var i = 0; i < size; i++)
                    yield return data.GetValue(UnreducedShape.GetCoords(i));
            }
        }
    }
}
