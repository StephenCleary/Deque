﻿using System;
using System.Collections;
using System.Collections.Generic;
using Nito.Collections;
using Xunit;

#pragma warning disable xUnit2013  // "Do not use Assert.Equal() to check for collection size"

namespace UnitTests
{
    public class DequeUnitTests
    {
        [Fact]
        public void Capacity_SetTo0_ActsLikeList()
        {
            var list = new List<int>();
            list.Capacity = 0;
            Assert.Equal(0, list.Capacity);

            var deque = new Deque<int>();
            deque.Capacity = 0;
            Assert.Equal(0, deque.Capacity);
        }

        [Fact]
        public void Capacity_SetNegative_ActsLikeList()
        {
            var list = new List<int>();
            Assert.Throws<ArgumentOutOfRangeException>("value", () => { list.Capacity = -1; });

            var deque = new Deque<int>();
            Assert.Throws<ArgumentOutOfRangeException>("value", () => { deque.Capacity = -1; });
        }

        [Fact]
        public void Capacity_SetLarger_UsesSpecifiedCapacity()
        {
            var deque = new Deque<int>(1);
            Assert.Equal(1, deque.Capacity);
            deque.Capacity = 17;
            Assert.Equal(17, deque.Capacity);
        }

        [Fact]
        public void Capacity_SetSmaller_UsesSpecifiedCapacity()
        {
            var deque = new Deque<int>(13);
            Assert.Equal(13, deque.Capacity);
            deque.Capacity = 7;
            Assert.Equal(7, deque.Capacity);
        }

        [Fact]
        public void Capacity_Set_PreservesData()
        {
            var deque = new Deque<int>(new int[] { 1, 2, 3 });
            Assert.Equal(3, deque.Capacity);
            deque.Capacity = 7;
            Assert.Equal(7, deque.Capacity);
            Assert.Equal(new[] { 1, 2, 3 }, deque);
        }

        [Fact]
        public void Capacity_Set_WhenSplit_PreservesData()
        {
            var deque = new Deque<int>(new int[] { 1, 2, 3 });
            deque.RemoveFromFront();
            deque.AddToBack(4);
            Assert.Equal(3, deque.Capacity);
            deque.Capacity = 7;
            Assert.Equal(7, deque.Capacity);
            Assert.Equal(new[] { 2, 3, 4 }, deque);
        }

        [Fact]
        public void Capacity_Set_SmallerThanCount_ActsLikeList()
        {
            var list = new List<int>(new int[] { 1, 2, 3 });
            Assert.Equal(3, list.Capacity);
            Assert.Throws<ArgumentOutOfRangeException>("value", () => { list.Capacity = 2; });

            var deque = new Deque<int>(new int[] { 1, 2, 3 });
            Assert.Equal(3, deque.Capacity);
            Assert.Throws<ArgumentOutOfRangeException>("value", () => { deque.Capacity = 2; });
        }

        [Fact]
        public void Capacity_Set_ToItself_DoesNothing()
        {
            var deque = new Deque<int>(13);
            Assert.Equal(13, deque.Capacity);
            deque.Capacity = 13;
            Assert.Equal(13, deque.Capacity);
        }

        // Implementation detail: the default capacity.
        const int DefaultCapacity = 8;

        [Fact]
        public void Constructor_WithoutExplicitCapacity_UsesDefaultCapacity()
        {
            var deque = new Deque<int>();
            Assert.Equal(DefaultCapacity, deque.Capacity);
        }

        [Fact]
        public void Constructor_CapacityOf0_ActsLikeList()
        {
            var list = new List<int>(0);
            Assert.Equal(0, list.Capacity);

            var deque = new Deque<int>(0);
            Assert.Equal(0, deque.Capacity);
        }

        [Fact]
        public void Constructor_CapacityOf0_PermitsAdd()
        {
            var deque = new Deque<int>(0);
            deque.AddToBack(13);
            Assert.Equal(new[] { 13 }, deque);
        }

        [Fact]
        public void Constructor_NegativeCapacity_ActsLikeList()
        {
            Assert.Throws<ArgumentOutOfRangeException>("capacity", () => new List<int>(-1));

            Assert.Throws<ArgumentOutOfRangeException>("capacity", () => new Deque<int>(-1));
        }

        [Fact]
        public void Constructor_CapacityOf1_UsesSpecifiedCapacity()
        {
            var deque = new Deque<int>(1);
            Assert.Equal(1, deque.Capacity);
        }

        [Fact]
        public void Constructor_FromEmptySequence_UsesDefaultCapacity()
        {
            var deque = new Deque<int>(new int[] { });
            Assert.Equal(DefaultCapacity, deque.Capacity);
        }

        [Fact]
        public void Constructor_FromSequence_InitializesFromSequence()
        {
            var deque = new Deque<int>(new int[] { 1, 2, 3 });
            Assert.Equal(3, deque.Capacity);
            Assert.Equal(3, deque.Count);
            Assert.Equal(new int[] { 1, 2, 3 }, deque);
        }

        [Fact]
        public void Constructor_FromSequence_InitializesFromSpan()
        {
            var deque = Deque.FromSpan<int>(new int[] { 1, 2, 3 });
            Assert.Equal(3, deque.Capacity);
            Assert.Equal(3, deque.Count);
            Assert.Equal(new int[] { 1, 2, 3 }, deque);
        }

        [Fact]
        public void IndexOf_ItemPresent_ReturnsItemIndex()
        {
            var deque = new Deque<int>(new[] { 1, 2 });
            var result = deque.IndexOf(2);
            Assert.Equal(1, result);
        }

        [Fact]
        public void IndexOf_ItemNotPresent_ReturnsNegativeOne()
        {
            var deque = new Deque<int>(new[] { 1, 2 });
            var result = deque.IndexOf(3);
            Assert.Equal(-1, result);
        }

        [Fact]
        public void IndexOf_ItemPresentAndSplit_ReturnsItemIndex()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            deque.RemoveFromBack();
            deque.AddToFront(0);
            Assert.Equal(0, deque.IndexOf(0));
            Assert.Equal(1, deque.IndexOf(1));
            Assert.Equal(2, deque.IndexOf(2));
        }

        [Fact]
        public void Contains_ItemPresent_ReturnsTrue()
        {
            var deque = new Deque<int>(new[] { 1, 2 }) as ICollection<int>;
            Assert.True(deque.Contains(2));
        }

        [Fact]
        public void Contains_ItemNotPresent_ReturnsFalse()
        {
            var deque = new Deque<int>(new[] { 1, 2 }) as ICollection<int>;
            Assert.False(deque.Contains(3));
        }

        [Fact]
        public void Contains_ItemPresentAndSplit_ReturnsTrue()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            deque.RemoveFromBack();
            deque.AddToFront(0);
            var deq = deque as ICollection<int>;
            Assert.True(deq.Contains(0));
            Assert.True(deq.Contains(1));
            Assert.True(deq.Contains(2));
            Assert.False(deq.Contains(3));
        }

        [Fact]
        public void Add_IsAddToBack()
        {
            var deque1 = new Deque<int>(new[] { 1, 2 });
            var deque2 = new Deque<int>(new[] { 1, 2 });
            ((ICollection<int>)deque1).Add(3);
            deque2.AddToBack(3);
            Assert.Equal(deque1, deque2);
        }

        [Fact]
        public void NonGenericEnumerator_EnumeratesItems()
        {
            var deque = new Deque<int>(new[] { 1, 2 });
            var results = new List<int>();
            var objEnum = ((System.Collections.IEnumerable)deque).GetEnumerator();
            while (objEnum.MoveNext())
            {
                results.Add((int)objEnum.Current);
            }
            Assert.Equal(results, deque);
        }

        [Fact]
        public void IsReadOnly_ReturnsFalse()
        {
            var deque = new Deque<int>();
            Assert.False(((ICollection<int>)deque).IsReadOnly);
        }

        [Fact]
        public void CopyTo_CopiesItems()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            var results = new int[3];
            ((ICollection<int>)deque).CopyTo(results, 0);
        }

        [Fact]
        public void CopyTo_NullArray_ActsLikeList()
        {
            var list = new List<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentNullException>(() => ((ICollection<int>)list).CopyTo(null, 0));

            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentNullException>(() => ((ICollection<int>)deque).CopyTo(null, 0));
        }

        [Fact]
        public void CopyTo_NegativeOffset_ActsLikeList()
        {
            var destination = new int[3];
            var list = new List<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>(() => ((ICollection<int>)list).CopyTo(destination, -1));

            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>(() => ((ICollection<int>)deque).CopyTo(destination, -1));
        }

        [Fact]
        public void CopyTo_InsufficientSpace_ActsLikeList()
        {
            var destination = new int[3];
            var list = new List<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentException>(() => ((ICollection<int>)list).CopyTo(destination, 1));

            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentException>(() => ((ICollection<int>)deque).CopyTo(destination, 1));
        }

        [Fact]
        public void Clear_EmptiesAllItems()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            deque.Clear();
            Assert.Empty(deque);
            Assert.Equal(new int[] { }, deque);
        }

        [Fact]
        public void Clear_DoesNotChangeCapacity()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.Equal(3, deque.Capacity);
            deque.Clear();
            Assert.Equal(3, deque.Capacity);
        }

        [Fact]
        public void RemoveFromFront_Empty_ActsLikeStack()
        {
            var stack = new Stack<int>();
            Assert.Throws<InvalidOperationException>(() => stack.Pop());

            var deque = new Deque<int>();
            Assert.Throws<InvalidOperationException>(() => deque.RemoveFromFront());
        }

        [Fact]
        public void RemoveFromBack_Empty_ActsLikeQueue()
        {
            var queue = new Queue<int>();
            Assert.Throws<InvalidOperationException>(() => queue.Dequeue());

            var deque = new Deque<int>();
            Assert.Throws<InvalidOperationException>(() => deque.RemoveFromBack());
        }

        [Fact]
        public void Remove_ItemPresent_RemovesItemAndReturnsTrue()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3, 4 });
            var result = deque.Remove(3);
            Assert.True(result);
            Assert.Equal(new[] { 1, 2, 4 }, deque);
        }

        [Fact]
        public void Remove_ItemNotPresent_KeepsItemsReturnsFalse()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3, 4 });
            var result = deque.Remove(5);
            Assert.False(result);
            Assert.Equal(new[] { 1, 2, 3, 4 }, deque);
        }

        [Fact]
        public void Insert_InsertsElementAtIndex()
        {
            var deque = new Deque<int>(new[] { 1, 2 });
            deque.Insert(1, 13);
            Assert.Equal(new[] { 1, 13, 2 }, deque);
        }

        [Fact]
        public void Insert_AtIndex0_IsSameAsAddToFront()
        {
            var deque1 = new Deque<int>(new[] { 1, 2 });
            var deque2 = new Deque<int>(new[] { 1, 2 });
            deque1.Insert(0, 0);
            deque2.AddToFront(0);
            Assert.Equal(deque1, deque2);
        }

        [Fact]
        public void Insert_AtCount_IsSameAsAddToBack()
        {
            var deque1 = new Deque<int>(new[] { 1, 2 });
            var deque2 = new Deque<int>(new[] { 1, 2 });
            deque1.Insert(deque1.Count, 0);
            deque2.AddToBack(0);
            Assert.Equal(deque1, deque2);
        }

        [Fact]
        public void Insert_NegativeIndex_ActsLikeList()
        {
            var list = new List<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>("index", () => list.Insert(-1, 0));

            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>("index", () => deque.Insert(-1, 0));
        }

        [Fact]
        public void Insert_IndexTooLarge_ActsLikeList()
        {
            var list = new List<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>("index", () => list.Insert(list.Count + 1, 0));

            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>("index", () => deque.Insert(deque.Count + 1, 0));
        }

        [Fact]
        public void RemoveAt_RemovesElementAtIndex()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            deque.RemoveFromBack();
            deque.AddToFront(0);
            deque.RemoveAt(1);
            Assert.Equal(new[] { 0, 2 }, deque);
        }

        [Fact]
        public void RemoveAt_Index0_IsSameAsRemoveFromFront()
        {
            var deque1 = new Deque<int>(new[] { 1, 2 });
            var deque2 = new Deque<int>(new[] { 1, 2 });
            deque1.RemoveAt(0);
            deque2.RemoveFromFront();
            Assert.Equal(deque1, deque2);
        }

        [Fact]
        public void RemoveAt_LastIndex_IsSameAsRemoveFromBack()
        {
            var deque1 = new Deque<int>(new[] { 1, 2 });
            var deque2 = new Deque<int>(new[] { 1, 2 });
            deque1.RemoveAt(1);
            deque2.RemoveFromBack();
            Assert.Equal(deque1, deque2);
        }

        [Fact]
        public void RemoveAt_NegativeIndex_ActsLikeList()
        {
            var list = new List<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>("index", () => list.RemoveAt(-1));

            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>("index", () => deque.RemoveAt(-1));
        }

        [Fact]
        public void RemoveAt_IndexTooLarge_ActsLikeList()
        {
            var list = new List<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>("index", () => list.RemoveAt(list.Count));

            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>("index", () => deque.RemoveAt(deque.Count));
        }

        [Fact]
        public void InsertMultiple()
        {
            InsertTest(new[] { 1, 2, 3 }, new[] { 7, 13 });
            InsertTest(new[] { 1, 2, 3, 4 }, new[] { 7, 13 });
        }

        private void InsertTest(IReadOnlyCollection<int> initial, IReadOnlyCollection<int> items)
        {
            var totalCapacity = initial.Count + items.Count;
            for (int rotated = 0; rotated <= totalCapacity; ++rotated)
            {
                for (int index = 0; index <= initial.Count; ++index)
                {
                    // Calculate the expected result using the slower List<int>.
                    var result = new List<int>(initial);
                    for (int i = 0; i != rotated; ++i)
                    {
                        var item = result[0];
                        result.RemoveAt(0);
                        result.Add(item);
                    }
                    result.InsertRange(index, items);

                    // First, start off the deque with the initial items.
                    var deque = new Deque<int>(initial);

                    // Ensure there's enough room for the inserted items.
                    deque.Capacity += items.Count;

                    // Rotate the existing items.
                    for (int i = 0; i != rotated; ++i)
                    {
                        var item = deque[0];
                        deque.RemoveFromFront();
                        deque.AddToBack(item);
                    }

                    // Do the insert.
                    deque.InsertRange(index, items);

                    // Ensure the results are as expected.
                    Assert.Equal(result, deque);
                }
            }
        }

        [Fact]
        public void Insert_RangeOfZeroElements_HasNoEffect()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            deque.InsertRange(1, new int[] { });
            Assert.Equal(new[] { 1, 2, 3 }, deque);
        }

        [Fact]
        public void InsertMultiple_MakesRoomForNewElements()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            deque.InsertRange(1, new[] { 7, 13 });
            Assert.Equal(new[] { 1, 7, 13, 2, 3 }, deque);
            Assert.Equal(5, deque.Capacity);
        }

        [Fact]
        public void AddToBackFromSpan()
        {
            var deque = new Deque<int>(1);
            deque.AddToBackFromSpan(new[] { 1, 2, 3 });
            Assert.Equal(4, deque.Capacity);  // Should have doubled twice
            Assert.Equal(1, deque.RemoveFromFront());
            Assert.Equal(2, deque.RemoveFromFront());
            Assert.Equal(1, deque.Count);
            deque.AddToBackFromSpan(new[] { 4, 5, 6 });  // Inserts to both ends of the buffer
            Assert.Equal(4, deque.Count);
            Assert.Equal(4, deque.Capacity);
            Assert.Equal(3, deque.RemoveFromFront());
            Assert.Equal(4, deque.RemoveFromFront());
            Assert.Equal(5, deque.RemoveFromFront());
            Assert.Equal(6, deque.RemoveFromFront());
        }

        [Fact]
        public void RepeatedAddToBackFromSpanThenRemoveFromFront()
        {
            var deque = new Deque<int>(1);
            for (int i = 0; i < 20; ++i)
            {
                List<int> list = new List<int>();
                for (int j = 0; j < i; ++j)
                {
                    list.Add(j);
                }
                deque.AddToBackFromSpan(list.ToArray());
                for (int j = 0; j < i; ++j)
                {
                    Assert.Equal(j, deque.RemoveFromFront());
                }
            }
        }

        [Fact]
        public void RepeatedAddToFrontFromSpanThenRemoveFromBack()
        {
            var deque = new Deque<int>(1);
            for (int i = 0; i < 20; ++i)
            {
                List<int> list = new List<int>();
                for (int j = 0; j < i; ++j)
                {
                    list.Add(j);
                }
                list.Reverse();
                deque.AddToFrontFromSpan(list.ToArray());
                for (int j = 0; j < i; ++j)
                {
                    Assert.Equal(j, deque.RemoveFromBack());
                }
            }
        }

        [Fact]
        public void AddToFrontFromSpan()
        {
            var deque = new Deque<int>(1);
            deque.AddToFrontFromSpan(new[] { 4, 5, 6 });
            Assert.Equal(4, deque.Capacity);  // Should have doubled twice
            Assert.Equal(6, deque.RemoveFromBack());
            Assert.Equal(5, deque.RemoveFromBack());
            Assert.Equal(1, deque.Count);
            deque.AddToFrontFromSpan(new[] { 1, 2, 3 });  // Inserts to both ends of the buffer
            Assert.Equal(4, deque.Count);
            Assert.Equal(4, deque.Capacity);
            Assert.Equal(4, deque.RemoveFromBack());
            Assert.Equal(3, deque.RemoveFromBack());
            Assert.Equal(2, deque.RemoveFromBack());
            Assert.Equal(1, deque.RemoveFromBack());
        }

        [Fact]
        public void Randomized_BulkAddAndRemoveTest()
        {
            var rng = new Random(123456789);
            const int maxOperationSize = 16;
            int[] GenerateSomeRandomInts()
            {
                var n = rng.Next(maxOperationSize);
                var ints = new int[n];
                for (int i = 0; i < n; ++i)
                {
                    ints[i] = rng.Next(10000);
                }
                return ints;
            }

            for (int repetition = 0; repetition < 50; ++repetition)
            {
                var individualDeque = new Deque<int>(0);
                var bulkDeque = new Deque<int>(0);
                for (int actionCounter = 0; actionCounter < 100; ++actionCounter)
                {
                    int action = rng.Next(4);
                    switch (action)
                    {
                        case 0:
                            {
                                var values = GenerateSomeRandomInts();
                                bulkDeque.AddToBackFromSpan(values);
                                foreach (var v in values)
                                {
                                    individualDeque.AddToBack(v);
                                }
                                break;
                            }
                        case 1:
                            {
                                var values = GenerateSomeRandomInts();
                                bulkDeque.AddToFrontFromSpan(values);
                                Array.Reverse(values);
                                foreach (var v in values)
                                {
                                    individualDeque.AddToFront(v);
                                }
                                break;
                            }
                        case 2:
                            {
                                var n = rng.Next(Math.Min(individualDeque.Count, maxOperationSize));
                                bulkDeque.RemoveFromBack(n);
                                for (int i = 0; i < n; ++i)
                                {
                                    individualDeque.RemoveFromBack();
                                }
                                break;
                            }
                        case 3:
                            {
                                var n = rng.Next(Math.Min(individualDeque.Count, maxOperationSize));
                                bulkDeque.RemoveFromFront(n);
                                for (int i = 0; i < n; ++i)
                                {
                                    individualDeque.RemoveFromFront();
                                }
                                break;
                            }
                    }

                    Assert.Equal(individualDeque.Count, bulkDeque.Count);
                    Assert.Equal(individualDeque, bulkDeque);

                    Span<int> firstPart;
                    Span<int> secondPart;
                    bulkDeque.AsSpans(out firstPart, out secondPart);
                    Assert.Equal(bulkDeque.Count, firstPart.Length + secondPart.Length);
                    for (int i = 0; i < firstPart.Length; ++i)
                    {
                        Assert.Equal(individualDeque[i], firstPart[i]);
                    }
                    for (int i = 0; i < secondPart.Length; ++i)
                    {
                        Assert.Equal(individualDeque[firstPart.Length + i], secondPart[i]);
                    }

                    {
                        var copyOfFront = new int[rng.Next(2) == 0 ? bulkDeque.Count : rng.Next(bulkDeque.Count * 5 / 4)];
                        var n = bulkDeque.CopyFromFrontToSpan(copyOfFront);
                        for (int i = 0; i < n; ++i)
                        {
                            Assert.Equal(bulkDeque[i], copyOfFront[i]);
                        }
                    }
                }
            }
        }

        [Fact]
        public void RemoveMultiple()
        {
            RemoveTest(new[] { 1, 2, 3 });
            RemoveTest(new[] { 1, 2, 3, 4 });
        }

        private void RemoveTest(IReadOnlyCollection<int> initial)
        {
            for (int count = 0; count <= initial.Count; ++count)
            {
                for (int rotated = 0; rotated <= initial.Count; ++rotated)
                {
                    for (int index = 0; index <= initial.Count - count; ++index)
                    {
                        // Calculated the expected result using the slower List<int>.
                        var result = new List<int>(initial);
                        for (int i = 0; i != rotated; ++i)
                        {
                            var item = result[0];
                            result.RemoveAt(0);
                            result.Add(item);
                        }
                        result.RemoveRange(index, count);

                        // First, start off the deque with the initial items.
                        var deque = new Deque<int>(initial);

                        // Rotate the existing items.
                        for (int i = 0; i != rotated; ++i)
                        {
                            var item = deque[0];
                            deque.RemoveFromFront();
                            deque.AddToBack(item);
                        }

                        // Do the remove.
                        deque.RemoveRange(index, count);

                        // Ensure the results are as expected.
                        Assert.Equal(result, deque);
                    }
                }
            }
        }

        [Fact]
        public void Remove_RangeOfZeroElements_HasNoEffect()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            deque.RemoveRange(1, 0);
            Assert.Equal(new[] { 1, 2, 3 }, deque);
        }

        [Fact]
        public void Remove_NegativeCount_ActsLikeList()
        {
            var list = new List<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>("count", () => list.RemoveRange(1, -1));

            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>("count", () => deque.RemoveRange(1, -1));
        }

        [Fact]
        public void GetItem_ReadsElements()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.Equal(1, deque[0]);
            Assert.Equal(2, deque[1]);
            Assert.Equal(3, deque[2]);
        }

        [Fact]
        public void GetItem_OptionallyReturnsByReference()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            ref var r = ref deque[1];
            r = 20;
            Assert.Equal(20, deque[1]);
            Assert.Equal(1, deque.RemoveFromFront());
            Assert.Equal(20, deque[0]);
            r = 22;
            Assert.Equal(22, deque[0]);
            Assert.Equal(22, deque.RemoveFromFront());
        }

        [Fact]
        public void GetItem_Split_ReadsElements()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            deque.RemoveFromBack();
            deque.AddToFront(0);
            Assert.Equal(0, deque[0]);
            Assert.Equal(1, deque[1]);
            Assert.Equal(2, deque[2]);
        }

        [Fact]
        public void GetItem_IndexTooLarge_ActsLikeList()
        {
            var list = new List<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>("index", () => list[3]);

            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>("index", () => deque[3]);
        }

        [Fact]
        public void GetItem_NegativeIndex_ActsLikeList()
        {
            var list = new List<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>("index", () => list[-1]);

            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>("index", () => deque[-1]);
        }

        [Fact]
        public void SetItem_WritesElements()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            deque[0] = 7;
            deque[1] = 11;
            deque[2] = 13;
            Assert.Equal(new[] { 7, 11, 13 }, deque);
        }

        [Fact]
        public void SetItem_Split_WritesElements()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            deque.RemoveFromBack();
            deque.AddToFront(0);
            deque[0] = 7;
            deque[1] = 11;
            deque[2] = 13;
            Assert.Equal(new[] { 7, 11, 13 }, deque);
        }

        [Fact]
        public void SetItem_IndexTooLarge_ActsLikeList()
        {
            var list = new List<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>("index", () => { list[3] = 13; });

            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>("index", () => { deque[3] = 13; });
        }

        [Fact]
        public void SetItem_NegativeIndex_ActsLikeList()
        {
            var list = new List<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>("index", () => { list[-1] = 13; });

            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.Throws<ArgumentOutOfRangeException>("index", () => { deque[-1] = 13; });
        }

        [Fact]
        public void NongenericIndexOf_ItemPresent_ReturnsItemIndex()
        {
            var deque = new Deque<int>(new[] { 1, 2 }) as IList;
            var result = deque.IndexOf(2);
            Assert.Equal(1, result);
        }

        [Fact]
        public void NongenericIndexOf_ItemNotPresent_ReturnsNegativeOne()
        {
            var deque = new Deque<int>(new[] { 1, 2 }) as IList;
            var result = deque.IndexOf(3);
            Assert.Equal(-1, result);
        }

        [Fact]
        public void NongenericIndexOf_ItemPresentAndSplit_ReturnsItemIndex()
        {
            var deque_ = new Deque<int>(new[] { 1, 2, 3 });
            deque_.RemoveFromBack();
            deque_.AddToFront(0);
            var deque = deque_ as IList;
            Assert.Equal(0, deque.IndexOf(0));
            Assert.Equal(1, deque.IndexOf(1));
            Assert.Equal(2, deque.IndexOf(2));
        }

        [Fact]
        public void NongenericIndexOf_WrongItemType_ReturnsNegativeOne()
        {
            var list = new List<int>(new[] { 1, 2 }) as IList;
            Assert.Equal(-1, list.IndexOf(this));

            var deque = new Deque<int>(new[] { 1, 2 }) as IList;
            Assert.Equal(-1, deque.IndexOf(this));
        }

        [Fact]
        public void NongenericContains_WrongItemType_ReturnsFalse()
        {
            var list = new List<int>(new[] { 1, 2 }) as IList;
            Assert.False(list.Contains(this));

            var deque = new Deque<int>(new[] { 1, 2 }) as IList;
            Assert.False(deque.Contains(this));
        }

        [Fact]
        public void NongenericContains_ItemPresent_ReturnsTrue()
        {
            var deque = new Deque<int>(new[] { 1, 2 }) as IList;
            Assert.True(deque.Contains(2));
        }

        [Fact]
        public void NongenericContains_ItemNotPresent_ReturnsFalse()
        {
            var deque = new Deque<int>(new[] { 1, 2 }) as IList;
            Assert.False(deque.Contains(3));
        }

        [Fact]
        public void NongenericContains_ItemPresentAndSplit_ReturnsTrue()
        {
            var deque_ = new Deque<int>(new[] { 1, 2, 3 });
            deque_.RemoveFromBack();
            deque_.AddToFront(0);
            var deque = deque_ as IList;
            Assert.True(deque.Contains(0));
            Assert.True(deque.Contains(1));
            Assert.True(deque.Contains(2));
            Assert.False(deque.Contains(3));
        }

        [Fact]
        public void NongenericIsReadOnly_ReturnsFalse()
        {
            var deque = new Deque<int>() as IList;
            Assert.False(deque.IsReadOnly);
        }

        [Fact]
        public void NongenericCopyTo_CopiesItems()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 }) as IList;
            var results = new int[3];
            deque.CopyTo(results, 0);
        }

        [Fact]
        public void NongenericCopyTo_NullArray_ActsLikeList()
        {
            var list = new List<int>(new[] { 1, 2, 3 }) as IList;
            Assert.Throws<ArgumentNullException>(() => list.CopyTo(null, 0));

            var deque = new Deque<int>(new[] { 1, 2, 3 }) as IList;
            Assert.Throws<ArgumentNullException>(() => deque.CopyTo(null, 0));
        }

        [Fact]
        public void NongenericCopyTo_NegativeOffset_ActsLikeList()
        {
            var destination = new int[3];
            var list = new List<int>(new[] { 1, 2, 3 }) as IList;
            Assert.Throws<ArgumentOutOfRangeException>(() => list.CopyTo(destination, -1));

            var deque = new Deque<int>(new[] { 1, 2, 3 }) as IList;
            Assert.Throws<ArgumentOutOfRangeException>(() => deque.CopyTo(destination, -1));
        }

        [Fact]
        public void NongenericCopyTo_InsufficientSpace_ActsLikeList()
        {
            var destination = new int[3];
            var list = new List<int>(new[] { 1, 2, 3 }) as IList;
            Assert.Throws<ArgumentException>(() => list.CopyTo(destination, 1));

            var deque = new Deque<int>(new[] { 1, 2, 3 }) as IList;
            Assert.Throws<ArgumentException>(() => deque.CopyTo(destination, 1));
        }

        [Fact]
        public void NongenericCopyTo_WrongType_ActsLikeList()
        {
            var destination = new IList[3];
            var list = new List<int>(new[] { 1, 2, 3 }) as IList;
            Assert.Throws<ArgumentException>(() => list.CopyTo(destination, 0));

            var deque = new Deque<int>(new[] { 1, 2, 3 }) as IList;
            Assert.Throws<ArgumentException>(() => deque.CopyTo(destination, 0));
        }

        [Fact]
        public void NongenericCopyTo_MultidimensionalArray_ActsLikeList()
        {
            var destination = new int[3, 3];
            var list = new List<int>(new[] { 1, 2, 3 }) as IList;
            Assert.Throws<ArgumentException>(() => list.CopyTo(destination, 0));

            var deque = new Deque<int>(new[] { 1, 2, 3 }) as IList;
            Assert.Throws<ArgumentException>(() => deque.CopyTo(destination, 0));
        }

        [Fact]
        public void NongenericAdd_WrongType_ActsLikeList()
        {
            var list = new List<int>() as IList;
            Assert.Throws<ArgumentException>("value", () => list.Add(this));

            var deque = new Deque<int>() as IList;
            Assert.Throws<ArgumentException>("value", () => deque.Add(this));
        }

        [Fact]
        public void NongenericNullableType_AllowsInsertingNull()
        {
            var deque = new Deque<int?>();
            var list = deque as IList;
            var result = list.Add(null);
            Assert.Equal(0, result);
            Assert.Equal(new int?[] { null }, deque);
        }

        [Fact]
        public void NongenericClassType_AllowsInsertingNull()
        {
            var deque = new Deque<object>();
            var list = deque as IList;
            var result = list.Add(null);
            Assert.Equal(0, result);
            Assert.Equal(new object[] { null }, deque);
        }

        [Fact]
        public void NongenericInterfaceType_AllowsInsertingNull()
        {
            var deque = new Deque<IList>();
            var list = deque as IList;
            var result = list.Add(null);
            Assert.Equal(0, result);
            Assert.Equal(new IList[] { null }, deque);
        }

        [Fact]
        public void NongenericStruct_AddNull_ActsLikeList()
        {
            var list = new List<int>() as IList;
            Assert.Throws<ArgumentNullException>(() => list.Add(null));

            var deque = new Deque<int>() as IList;
            Assert.Throws<ArgumentNullException>(() => deque.Add(null));
        }

        [Fact]
        public void NongenericGenericStruct_AddNull_ActsLikeList()
        {
            var list = new List<KeyValuePair<int, int>>() as IList;
            Assert.Throws<ArgumentNullException>(() => list.Add(null));

            var deque = new Deque<KeyValuePair<int, int>>() as IList;
            Assert.Throws<ArgumentNullException>(() => deque.Add(null));
        }

        [Fact]
        public void NongenericIsFixedSize_IsFalse()
        {
            var deque = new Deque<int>() as IList;
            Assert.False(deque.IsFixedSize);
        }

        [Fact]
        public void NongenericIsSynchronized_IsFalse()
        {
            var deque = new Deque<int>() as IList;
            Assert.False(deque.IsSynchronized);
        }

        [Fact]
        public void NongenericSyncRoot_IsSelf()
        {
            var deque = new Deque<int>() as IList;
            Assert.Same(deque, deque.SyncRoot);
        }

        [Fact]
        public void NongenericInsert_InsertsItem()
        {
            var deque = new Deque<int>();
            var list = deque as IList;
            list.Insert(0, 7);
            Assert.Equal(new[] { 7 }, deque);
        }

        [Fact]
        public void NongenericInsert_WrongType_ActsLikeList()
        {
            var list = new List<int>() as IList;
            Assert.Throws<ArgumentException>("value", () => list.Insert(0, this));

            var deque = new Deque<int>() as IList;
            Assert.Throws<ArgumentException>("value", () => deque.Insert(0, this));
        }

        [Fact]
        public void NongenericStruct_InsertNull_ActsMostlyLikeList()
        {
            var list = new List<int>() as IList;
            Assert.Throws<ArgumentNullException>("item", () => list.Insert(0, null)); // Should probably be "value".

            var deque = new Deque<int>() as IList;
            Assert.Throws<ArgumentNullException>("value", () => deque.Insert(0, null));
        }

        [Fact]
        public void NongenericRemove_RemovesItem()
        {
            var deque = new Deque<int>(new[] { 13 });
            var list = deque as IList;
            list.Remove(13);
            Assert.Equal(new int[] { }, deque);
        }

        [Fact]
        public void NongenericRemove_WrongType_DoesNothing()
        {
            var list = new List<int>(new[] { 13 }) as IList;
            list.Remove(this);
            list.Remove(null);
            Assert.Equal(1, list.Count);

            var deque = new Deque<int>(new[] { 13 }) as IList;
            deque.Remove(this);
            deque.Remove(null);
            Assert.Equal(1, deque.Count);
        }

        [Fact]
        public void NongenericGet_GetsItem()
        {
            var deque = new Deque<int>(new[] { 13 }) as IList;
            var value = (int)deque[0];
            Assert.Equal(13, value);
        }

        [Fact]
        public void NongenericSet_SetsItem()
        {
            var deque = new Deque<int>(new[] { 13 });
            var list = deque as IList;
            list[0] = 7;
            Assert.Equal(new[] { 7 }, deque);
        }

        [Fact]
        public void NongenericSet_WrongType_ActsLikeList()
        {
            var list = new List<int>(new[] { 13 }) as IList;
            Assert.Throws<ArgumentException>("value", () => { list[0] = this; });

            var deque = new Deque<int>(new[] { 13 }) as IList;
            Assert.Throws<ArgumentException>("value", () => { deque[0] = this; });
        }

        [Fact]
        public void NongenericStruct_SetNull_ActsLikeList()
        {
            var list = new List<int>(new[] { 13 }) as IList;
            Assert.Throws<ArgumentNullException>("value", () => { list[0] = null; });

            var deque = new Deque<int>(new[] { 13 }) as IList;
            Assert.Throws<ArgumentNullException>("value", () => { deque[0] = null; });
        }

        [Fact]
        public void ToArray_CopiesToNewArray()
        {
            var deque = new Deque<int>(new[] { 0, 1 });
            deque.AddToBack(13);
            var result = deque.ToArray();
            Assert.Equal(new[] { 0, 1, 13 }, result);
        }
    }
}