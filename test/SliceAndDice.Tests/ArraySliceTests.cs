// Copyright (c) 2019, Henon <meinrad.recheis@gmal.com>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SliceAndDice.Tests
{
    [TestFixture]
    public class ArraySliceTests
    {
        [Test]
        public void BasicTests()
        {
            var a = new ArraySlice<int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            // enumerable?
            Assert.AreEqual(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, a);
            Assert.AreNotEqual(new[] { 0, 1, 2, 3, 4444, 5, 6, 7, 8, 9 }, a);
            Assert.AreEqual(new ArraySlice<int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9), a);
            // auto shape
            Assert.AreEqual(new Shape(10), a.Shape);
            // indexing
            Assert.AreEqual(0, a[0]);
            Assert.AreEqual(5, a[5]);
            Assert.AreEqual(9, a[9]);
            Assert.AreEqual(0, a.GetValue(0));
            Assert.AreEqual(5, a.GetValue(5));
            Assert.AreEqual(9, a.GetValue(9));
            // length (for 1-D arrayslices, first dimension for N-D)
            Assert.AreEqual(10, a.Length);
        }

        [Test]
        public void EnumeratorTest()
        {
            var a = new ArraySlice<int>(0, 1, 2);
            var iter = a.GetEnumerator();
            Assert.IsTrue(iter.MoveNext());
            Assert.AreEqual(0, iter.Current);
            Assert.IsTrue(iter.MoveNext());
            Assert.AreEqual(1, iter.Current);
            Assert.IsTrue(iter.MoveNext());
            Assert.AreEqual(2, iter.Current);
            Assert.IsFalse(iter.MoveNext());
            a = new ArraySlice<int>(new List<int>() { 0, 1, 2 });
            iter = a.GetEnumerator();
            Assert.IsTrue(iter.MoveNext());
            Assert.AreEqual(0, iter.Current);
            Assert.IsTrue(iter.MoveNext());
            Assert.AreEqual(1, iter.Current);
            Assert.IsTrue(iter.MoveNext());
            Assert.AreEqual(2, iter.Current);
            Assert.IsFalse(iter.MoveNext());
            a = new ArraySlice<int>(new ArraySlice<int>(0, 1, 2));
            iter = a.GetEnumerator();
            Assert.IsTrue(iter.MoveNext());
            Assert.AreEqual(0, iter.Current);
            Assert.IsTrue(iter.MoveNext());
            Assert.AreEqual(1, iter.Current);
            Assert.IsTrue(iter.MoveNext());
            Assert.AreEqual(2, iter.Current);
            Assert.IsFalse(iter.MoveNext());
        }

        [Test]
        public void SlicingTest_1D()
        {
            var data = new ArraySlice<int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.AreEqual(new Shape(10), data.Shape);
            // return identical view
            var view = new ArraySlice<int>(data, ":");
            Assert.AreEqual(new Shape(10), view.Shape);
            Assert.AreEqual(data, view);
            view = new ArraySlice<int>(data, "-77:77");
            Assert.AreEqual(new Shape(10), view.Shape);
            Assert.AreEqual(data, view);
            // return reduced view
            view = new ArraySlice<int>(data, "7:");
            Assert.AreEqual(new Shape(3), view.Shape);
            Assert.AreEqual(new int[] { 7, 8, 9 }, view);
            view = new ArraySlice<int>(data, ":5");
            Assert.AreEqual(new Shape(5), view.Shape);
            Assert.AreEqual(new int[] { 0, 1, 2, 3, 4 }, view);
            view = new ArraySlice<int>(data, "2:3");
            Assert.AreEqual(new Shape(1), view.Shape);
            Assert.AreEqual(new int[] { 2 }, view);
            // return stepped view
            view = new ArraySlice<int>(data, "::2");
            Assert.AreEqual(new Shape(5), view.Shape);
            Assert.AreEqual(new int[] { 0, 2, 4, 6, 8 }, view);
            view = new ArraySlice<int>(data, "::3");
            Assert.AreEqual(new Shape(4), view.Shape);
            Assert.AreEqual(new int[] { 0, 3, 6, 9 }, view);
            view = new ArraySlice<int>(data, "-77:77:77");
            Assert.AreEqual(new Shape(1), view.Shape);
            Assert.AreEqual(new[] { 0 }, view);
            // negative step!
            view = new ArraySlice<int>(data, "::-1");
            Assert.AreEqual(new Shape(10), view.Shape);
            Assert.AreEqual(data.Reverse().ToArray(), view);
            view = new ArraySlice<int>(data, "::-2");
            Assert.AreEqual(new Shape(5), view.Shape);
            Assert.AreEqual(new int[] { 9, 7, 5, 3, 1 }, view);
            view = new ArraySlice<int>(data, "::-3");
            Assert.AreEqual(new Shape(4), view.Shape);
            Assert.AreEqual(new int[] { 9, 6, 3, 0 }, view);
            view = new ArraySlice<int>(data, "-77:77:-77");
            Assert.AreEqual(new Shape(1), view.Shape);
            Assert.AreEqual(new[] { 9 }, view);
        }

        [Test]
        public void Indexing_1D()
        {
            var data = new ArraySlice<int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            // return identical view
            var view = new ArraySlice<int>(data, ":");
            Assert.AreEqual(0, view.GetValue(0));
            Assert.AreEqual(5, view.GetValue(5));
            Assert.AreEqual(9, view.GetValue(9));
            view = new ArraySlice<int>(data, "-77:77");
            Assert.AreEqual(0, view.GetValue(0));
            Assert.AreEqual(5, view.GetValue(5));
            Assert.AreEqual(9, view.GetValue(9));
            // return reduced view
            view = new ArraySlice<int>(data, "7:");
            Assert.AreEqual(7, view.GetValue(0));
            Assert.AreEqual(8, view.GetValue(1));
            Assert.AreEqual(9, view.GetValue(2));
            view = new ArraySlice<int>(data, ":5");
            Assert.AreEqual(0, view.GetValue(0));
            Assert.AreEqual(1, view.GetValue(1));
            Assert.AreEqual(2, view.GetValue(2));
            Assert.AreEqual(3, view.GetValue(3));
            Assert.AreEqual(4, view.GetValue(4));
            view = new ArraySlice<int>(data, "2:3");
            Assert.AreEqual(2, view.GetValue(0));
            // return stepped view
            view = new ArraySlice<int>(data, "::2");
            Assert.AreEqual(0, view.GetValue(0));
            Assert.AreEqual(2, view.GetValue(1));
            Assert.AreEqual(4, view.GetValue(2));
            Assert.AreEqual(6, view.GetValue(3));
            Assert.AreEqual(8, view.GetValue(4));
            view = new ArraySlice<int>(data, "::3");
            Assert.AreEqual(0, view.GetValue(0));
            Assert.AreEqual(3, view.GetValue(1));
            Assert.AreEqual(6, view.GetValue(2));
            Assert.AreEqual(9, view.GetValue(3));
            view = new ArraySlice<int>(data, "-77:77:77");
            Assert.AreEqual(0, view.GetValue(0));
            // negative step!
            view = new ArraySlice<int>(data, "::-1");
            Assert.AreEqual(9, view.GetValue(0));
            Assert.AreEqual(4, view.GetValue(5));
            Assert.AreEqual(0, view.GetValue(9));
            view = new ArraySlice<int>(data, "::-2");
            Assert.AreEqual(9, view.GetValue(0));
            Assert.AreEqual(7, view.GetValue(1));
            Assert.AreEqual(5, view.GetValue(2));
            Assert.AreEqual(3, view.GetValue(3));
            Assert.AreEqual(1, view.GetValue(4));
            view = new ArraySlice<int>(data, "::-3");
            Assert.AreEqual(9, view.GetValue(0));
            Assert.AreEqual(6, view.GetValue(1));
            Assert.AreEqual(3, view.GetValue(2));
            Assert.AreEqual(0, view.GetValue(3));
            view = new ArraySlice<int>(data, "-77:77:-77");
            Assert.AreEqual(9, view.GetValue(0));
        }

        [Test]
        public void NestedView_1D()
        {
            var data = new ArraySlice<int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            // return identical view
            var identical = new ArraySlice<int>(data, ":");
            Assert.AreEqual(new Shape(10), identical.Shape);
            var view1 = new ArraySlice<int>(identical, "1:9");
            Assert.AreEqual(new Shape(8), view1.Shape);
            Assert.AreEqual(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, }, view1);
            var view2 = new ArraySlice<int>(view1, "::-2");
            Assert.AreEqual(new Shape(4), view2.Shape);
            Assert.AreEqual(new int[] { 8, 6, 4, 2, }, view2);
            var view3 = new ArraySlice<int>(view2, "::-3");
            Assert.AreEqual(new Shape(2), view3.Shape);
            Assert.AreEqual(new int[] { 2, 8 }, view3);
            // all must see the same modifications, no matter if original or any view is modified
            // modify original
            data.SetValues(
0, new int[] { 0, -1, -2, -3, -4, -5, -6, -7, -8, -9 });
            Assert.AreEqual(new int[] { -1, -2, -3, -4, -5, -6, -7, -8, }, view1);
            Assert.AreEqual(new int[] { -8, -6, -4, -2, }, view2);
            Assert.AreEqual(new int[] { -2, -8 }, view3);
            // modify views
            view1[7] = 88;
            Assert.AreEqual(new int[] { 0, -1, -2, -3, -4, -5, -6, -7, 88, -9 }, data);
            Assert.AreEqual(new int[] { -1, -2, -3, -4, -5, -6, -7, 88, }, view1);
            Assert.AreEqual(new int[] { 88, -6, -4, -2, }, view2);
            Assert.AreEqual(new int[] { -2, 88 }, view3);
            view3[0] = 22;
            Assert.AreEqual(new int[] { 0, -1, 22, -3, -4, -5, -6, -7, 88, -9 }, data);
            Assert.AreEqual(new int[] { -1, 22, -3, -4, -5, -6, -7, 88, }, view1);
            Assert.AreEqual(new int[] { 88, -6, -4, 22, }, view2);
            Assert.AreEqual(new int[] { 22, 88 }, view3);
        }

        [Test]
        public void GetData_2D()
        {
            //>>> x = np.arange(9).reshape(3, 3)
            //>>> x
            //array([[0, 1, 2],
            //       [3, 4, 5],
            //       [6, 7, 8]])
            //>>> x[:]
            //array([[0, 1, 2],
            //       [3, 4, 5],
            //       [6, 7, 8]])
            //>>> x[1:]
            //array([[3, 4, 5],
            //       [6, 7, 8]])
            //>>> x[1:,:]
            //array([[3, 4, 5],
            //       [6, 7, 8]])
            //>>> x[:, 1:]
            //array([[1, 2],
            //       [4, 5],
            //       [7, 8]])
            //>>> x[1:2, 0:1]
            //array([[3]])
            var data = new ArraySlice<int>(0, 1, 2, 3, 4, 5, 6, 7, 8).Reshape(3, 3);
            Assert.AreEqual(new Shape(3, 3), data.Shape);
            // return identical view
            var view = new ArraySlice<int>(data, ":");
            Assert.AreEqual(new Shape(3, 3), view.Shape);
            Assert.AreEqual(data, view);
            // return reduced view
            view = new ArraySlice<int>(data, "1:");
            Assert.AreEqual(new Shape(2, 3), view.Shape);
            Assert.AreEqual(new int[] { 3, 4, 5, 6, 7, 8 }, view);
            view = new ArraySlice<int>(data, "1:,:");
            Assert.AreEqual(new Shape(2, 3), view.Shape);
            Assert.AreEqual(new int[] { 3, 4, 5, 6, 7, 8 }, view);
            view = new ArraySlice<int>(data, ":,1:");
            Assert.AreEqual(new Shape(3, 2), view.Shape);
            Assert.AreEqual(new int[] { 1, 2, 4, 5, 7, 8 }, view);
            view = new ArraySlice<int>(data, "1:2, 0:1");
            Assert.AreEqual(new Shape(1, 1), view.Shape);
            Assert.AreEqual(new int[] { 3 }, view);
            // return stepped view
            //>>> x
            //array([[0, 1, 2],
            //       [3, 4, 5],
            //       [6, 7, 8]])
            //>>> x[::2]
            //array([[0, 1, 2],
            //       [6, 7, 8]])
            //>>> x[::3]
            //array([[0, 1, 2]])
            //>>> x[::- 1]
            //array([[6, 7, 8],
            //       [3, 4, 5],
            //       [0, 1, 2]])
            //>>> x[::- 2]
            //array([[6, 7, 8],
            //       [0, 1, 2]])
            //>>> x[::- 3]
            //array([[6, 7, 8]])
            view = new ArraySlice<int>(data, "::2");
            Assert.AreEqual(new Shape(2, 3), view.Shape);
            Assert.AreEqual(new int[] { 0, 1, 2, 6, 7, 8 }, view);
            view = new ArraySlice<int>(data, "::3");
            Assert.AreEqual(new Shape(1, 3), view.Shape);
            Assert.AreEqual(new int[] { 0, 1, 2 }, view);
            view = new ArraySlice<int>(data, "::-1");
            Assert.AreEqual(new Shape(3, 3), view.Shape);
            Assert.AreEqual(new int[] { 6, 7, 8, 3, 4, 5, 0, 1, 2, }, view);
            view = new ArraySlice<int>(data, "::-2");
            Assert.AreEqual(new Shape(2, 3), view.Shape);
            Assert.AreEqual(new int[] { 6, 7, 8, 0, 1, 2, }, view);
            view = new ArraySlice<int>(data, "::-3");
            Assert.AreEqual(new Shape(1, 3), view.Shape);
            Assert.AreEqual(new int[] { 6, 7, 8, }, view);
            // N-Dim Stepping
            //>>> x[::2,::2]
            //array([[0, 2],
            //       [6, 8]])
            //>>> x[::- 1,::- 2]
            //array([[8, 6],
            //       [5, 3],
            //       [2, 0]])
            view = new ArraySlice<int>(data, "::2, ::2");
            Assert.AreEqual(new Shape(2, 2), view.Shape);
            Assert.AreEqual(new int[] { 0, 2, 6, 8 }, view);
            view = new ArraySlice<int>(data, "::-1, ::-2");
            Assert.AreEqual(new Shape(3, 2), view.Shape);
            Assert.AreEqual(new int[] { 8, 6, 5, 3, 2, 0 }, view);
        }

        [Test]
        public void NestedView_2D()
        {
            var data = new ArraySlice<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }).Reshape(2, 10);
            //>>> x = np.array([0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9])
            //>>> x = x.reshape(2, 10)
            //>>> x
            //array([[0, 1, 2, 3, 4, 5, 6, 7, 8, 9],
            //       [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]])
            // return identical view
            var identical = new ArraySlice<int>(data, ":");
            Assert.AreEqual(new Shape(2, 10), identical.Shape);
            //>>> x[:, 1:9]
            //array([[1, 2, 3, 4, 5, 6, 7, 8],
            //       [1, 2, 3, 4, 5, 6, 7, 8]])
            var view1 = new ArraySlice<int>(identical, ":,1:9");
            Assert.AreEqual(new Shape(2, 8), view1.Shape);
            Assert.AreEqual(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 }, view1);
            //>>> x[:, 1:9][:,::- 2]
            //array([[8, 6, 4, 2],
            //       [8, 6, 4, 2]])
            var view2 = new ArraySlice<int>(view1, ":,::-2");
            Assert.AreEqual(new Shape(2, 4), view2.Shape);
            Assert.AreEqual(new int[] { 8, 6, 4, 2, 8, 6, 4, 2 }, view2);
            //>>> x[:, 1:9][:,::- 2][:,::- 3]
            //array([[2, 8],
            //       [2, 8]])
            var view3 = new ArraySlice<int>(view2, ":,::-3");
            Assert.AreEqual(new Shape(2, 2), view3.Shape);
            Assert.AreEqual(new int[] { 2, 8, 2, 8 }, view3);
            // all must see the same modifications, no matter if original or any view is modified
            // modify original
            data.SetValues(0, new int[] { 0, -1, -2, -3, -4, -5, -6, -7, -8, -9, 0, -1, -2, -3, -4, -5, -6, -7, -8, -9 });
            Assert.AreEqual(new int[] { -1, -2, -3, -4, -5, -6, -7, -8, -1, -2, -3, -4, -5, -6, -7, -8 }, view1);
            Assert.AreEqual(new int[] { -8, -6, -4, -2, -8, -6, -4, -2 }, view2);
            Assert.AreEqual(new int[] { -2, -8, -2, -8 }, view3);
            // modify views
            view1[0, 7] = 88;
            view1[1, 7] = 888;
            Assert.AreEqual(new int[] { 0, -1, -2, -3, -4, -5, -6, -7, 88, -9, 0, -1, -2, -3, -4, -5, -6, -7, 888, -9 },
                data);
            Assert.AreEqual(new int[] { -1, -2, -3, -4, -5, -6, -7, 88, -1, -2, -3, -4, -5, -6, -7, 888 },
                view1);
            Assert.AreEqual(new int[] { 88, -6, -4, -2, 888, -6, -4, -2 }, view2);
            Assert.AreEqual(new int[] { -2, 88, -2, 888 }, view3);
            view3[0, 0] = 22;
            view3[1, 0] = 222;
            Assert.AreEqual(new int[] { 0, -1, 22, -3, -4, -5, -6, -7, 88, -9, 0, -1, 222, -3, -4, -5, -6, -7, 888, -9 },
                data);
            Assert.AreEqual(new int[] { -1, 22, -3, -4, -5, -6, -7, 88, -1, 222, -3, -4, -5, -6, -7, 888 },
                view1);
            Assert.AreEqual(new int[] { 88, -6, -4, 22, 888, -6, -4, 222 }, view2);
            Assert.AreEqual(new int[] { 22, 88, 222, 888 }, view3);
        }

        [Test]
        public void Reduce_1D_to_Scalar()
        {
            var data = new ArraySlice<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            Assert.AreEqual(new Shape(10), data.Shape);
            // return scalar
            var view = new ArraySlice<int>(data, "7");
            Assert.AreEqual(new Shape(), view.Shape);
            Assert.AreEqual(new int[] { 7 }, view);
        }

        [Test]
        public void Reduce_2D_to_1D_and_0D()
        {
            //>>> x = np.arange(9).reshape(3, 3)
            //>>> x
            //array([[0, 1, 2],
            //       [3, 4, 5],
            //       [6, 7, 8]])
            //>>> x[1]
            //array([3, 4, 5])
            //>>> x[:,1]
            //array([1, 4, 7])
            //>>> x[2, 2]
            //8
            var data = new ArraySlice<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }).Reshape(3, 3);
            Assert.AreEqual(new Shape(3, 3), data.Shape);
            // return identical view
            var view = new ArraySlice<int>(data, "1");
            Assert.AreEqual(new Shape(3), view.Shape);
            Assert.AreEqual(new int[] { 3, 4, 5 }, view);
            // return reduced view
            view = new ArraySlice<int>(data, ":,1");
            Assert.AreEqual(new Shape(3), view.Shape);
            Assert.AreEqual(new int[] { 1, 4, 7 }, view);
            view = new ArraySlice<int>(data, "2,2");
            Assert.AreEqual(new Shape(), view.Shape);
            Assert.AreEqual(new int[] { 8 }, view);
            // recursive dimensionality reduction
            view = new ArraySlice<int>(new ArraySlice<int>(data, "2"), "2");
            Assert.AreEqual(new Shape(), view.Shape);
            Assert.AreEqual(new int[] { 8 }, view);
        }

        [Test]
        public void NestedDimensionalityReduction()
        {
            var data = new ArraySlice<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }).Reshape(3, 3);
            Assert.AreEqual(new Shape(3, 3), data.Shape);
            var view = new ArraySlice<int>(data, "2");
            Assert.AreEqual(new Shape(3), view.Shape);
            //Assert.AreEqual(new int[] { 6, 7, 8 }, view);
            var view1 = new ArraySlice<int>(view, "2");
            Assert.AreEqual(new Shape(), view1.Shape);
            Assert.AreEqual(new int[] { 8 }, view1);
            var view2 = new ArraySlice<int>(view, ":2:-1");
            Assert.AreEqual(new Shape(2), view2.Shape);
            Assert.AreEqual(new int[] { 7, 6 }, view2);
        }

        [Test]
        public void ImageProcessingExample()        {            var width = 4; // px
            var height = 4; // px
            var bytes_per_pixel = 3; // r, g, b

            // note that Slice and Dice uses the row major coordinate system like numpy
            // so the coordinate order is y, x, color in this example
            var shape = new Shape(height, width, bytes_per_pixel);

            // create the raw pixel data using Shape.Size to calculate how long the array 
            // must be to hold a 3D volume of h x w x byte_per_pixel
            var raw_bytes = new byte[shape.Size];
            var image = new ArraySlice<byte>(raw_bytes, shape);

            // we can now access single pixel values by coordinates

            // get the three bytes for the pixel at the upper right corner 
            // as ArraySlice<T> without copying any data!
            var pixel = image.GetSlice("0, 3");
            Console.WriteLine("pixel: " + pixel);  // prints: pixel: [0, 0, 0]

            // set the pixel to white
            image.SetValues(new int[] { 0, 3 }, new byte[] { 255, 255, 255 });

            // get the 2 by 2 pixel patch at the center and color it green without copying any data:
            var patch = image.GetSlice("1:3, 1:3, 1"); // this gets only the green channel 
            // note how we can now use relative coordinates in the patch            // since this is a slice of the green channel we can directly set the green byte at location (y,x)
            for (int y = 0; y < 2; y++)
                for (int x = 0; x < 2; x++)
                    patch[y, x] = 255; 

            // print the bitmap out row by row like this (black = '#', white = ' ', green= 'o')            // obviously by using slicing this is a no-brainer            for (int y = 0; y < height; y++)            {
                Console.Write("|");
                // Slice.Index gives us a slice-definition for a row at index y. 
                // You could also use image.GetSlice($"{y}")
                var row =image.GetSlice(Slice.Index(y));                for (int x = 0; x < width; x++)                {                    var red = row[x, 0];                    var green = row[x, 1];                    if (red == 0 && green==0)                    {                        // must be black                        Console.Write("#");
                        continue;                    }                    if (red==255 && green==255)
                    {
                        // must be white
                        Console.Write(" ");
                        continue;
                    }
                    if (green==255)                        Console.Write("o");                }                // at the end of the row, break the line                Console.WriteLine("|");            }
        }
    }
}
