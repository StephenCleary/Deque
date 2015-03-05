using System;
using Nito;
using Xunit;
using System.Collections.Generic;
using System.Collections;

namespace UnitTests
{
    public class Deque
    {
        [Fact]
        public void Capacity_SetTo0_ThrowsException()
        {
            var deque = new Deque<int>();
            Assert.ThrowsAny<ArgumentException>(() => { deque.Capacity = 0; });
        }

        [Fact]
        public void Capacity_SetNegative_ThrowsException()
        {
            var deque = new Deque<int>();
            Assert.ThrowsAny<ArgumentException>(() => { deque.Capacity = -1; });
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
        public void Capacity_Set_SmallerThanCount_ThrowsException()
        {
            var deque = new Deque<int>(new int[] { 1, 2, 3 });
            Assert.Equal(3, deque.Capacity);
            Assert.ThrowsAny<InvalidOperationException>(() => { deque.Capacity = 2; });
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
        public void Constructor_CapacityOf0_ThrowsException()
        {
            Assert.ThrowsAny<ArgumentException>(() => new Deque<int>(0));
        }

        [Fact]
        public void Constructor_NegativeCapacity_ThrowsException()
        {
            Assert.ThrowsAny<ArgumentException>(() => new Deque<int>(-1));
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
        public void CopyTo_NullArray_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.ThrowsAny<ArgumentNullException>(() => ((ICollection<int>)deque).CopyTo(null, 0));
        }

        [Fact]
        public void CopyTo_NegativeOffset_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            var results = new int[3];
            Assert.ThrowsAny<ArgumentOutOfRangeException>(() => ((ICollection<int>)deque).CopyTo(results, -1));
        }

        [Fact]
        public void CopyTo_InsufficientSpace_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            var results = new int[3];
            Assert.ThrowsAny<ArgumentException>(() => ((ICollection<int>)deque).CopyTo(results, 1));
        }

        [Fact]
        public void Clear_EmptiesAllItems()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            deque.Clear();
            Assert.Equal(0, deque.Count);
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
        public void RemoveFromFront_Empty_ThrowsException()
        {
            var deque = new Deque<int>();
            Assert.ThrowsAny<InvalidOperationException>(() => deque.RemoveFromFront());
        }

        [Fact]
        public void RemoveFromBack_Empty_ThrowsException()
        {
            var deque = new Deque<int>();
            Assert.ThrowsAny<InvalidOperationException>(() => deque.RemoveFromBack());
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
        public void Insert_NegativeIndex_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.ThrowsAny<ArgumentException>(() => deque.Insert(-1, 0));
        }

        [Fact]
        public void Insert_IndexTooLarge_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.ThrowsAny<ArgumentException>(() => deque.Insert(deque.Count + 1, 0));
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
        public void RemoveAt_NegativeIndex_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.ThrowsAny<ArgumentException>(() => deque.RemoveAt(-1));
        }

        [Fact]
        public void RemoveAt_IndexTooLarge_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.ThrowsAny<ArgumentException>(() => deque.RemoveAt(deque.Count));
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
        public void Remove_NegativeCount_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.ThrowsAny<ArgumentException>(() => deque.RemoveRange(1, -1));
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
        public void GetItem_IndexTooLarge_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.ThrowsAny<ArgumentException>(() => deque[3]);
        }

        [Fact]
        public void GetItem_NegativeIndex_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.ThrowsAny<ArgumentException>(() => deque[-1]);
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
        public void SetItem_IndexTooLarge_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.ThrowsAny<ArgumentException>(() => { deque[3] = 13; });
        }

        [Fact]
        public void SetItem_NegativeIndex_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 });
            Assert.ThrowsAny<ArgumentException>(() => { deque[-1] = 13; });
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
        public void NongenericIndexOf_WrongItemType_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 1, 2 }) as IList;
            Assert.ThrowsAny<Exception>(() => deque.IndexOf(this));
        }

        [Fact]
        public void NongenericContains_WrongItemType_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 1, 2 }) as IList;
            Assert.ThrowsAny<Exception>(() => deque.Contains(this));
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
        public void NongenericCopyTo_NullArray_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 }) as IList;
            Assert.ThrowsAny<ArgumentNullException>(() => deque.CopyTo(null, 0));
        }

        [Fact]
        public void NongenericCopyTo_NegativeOffset_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 }) as IList;
            var results = new int[3];
            Assert.ThrowsAny<ArgumentOutOfRangeException>(() => deque.CopyTo(results, -1));
        }

        [Fact]
        public void NongenericCopyTo_InsufficientSpace_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 }) as IList;
            var results = new int[3];
            Assert.ThrowsAny<ArgumentException>(() => deque.CopyTo(results, 1));
        }

        [Fact]
        public void NongenericCopyTo_WrongType_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 }) as IList;
            var results = new IList[3];
            Assert.ThrowsAny<Exception>(() => deque.CopyTo(results, 0));
        }

        [Fact]
        public void NongenericCopyTo_MultidimensionalArray_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 1, 2, 3 }) as IList;
            var results = new int[3, 3];
            Assert.ThrowsAny<Exception>(() => deque.CopyTo(results, 0));
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
        public void NongenericStruct_InsertNull_ThrowsException()
        {
            var deque = new Deque<int>() as IList;
            Assert.ThrowsAny<Exception>(() => deque.Add(null));
        }

        [Fact]
        public void NongenericGenericStruct_InsertNull_ThrowsException()
        {
            var deque = new Deque<KeyValuePair<int, int>>() as IList;
            Assert.ThrowsAny<Exception>(() => deque.Add(null));
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
        public void NongenericInsert_WrongType_ThrowsException()
        {
            var deque = new Deque<int>() as IList;
            Assert.ThrowsAny<Exception>(() => deque.Insert(0, this));
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
        public void NongenericRemove_WrongType_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 13 }) as IList;
            Assert.ThrowsAny<Exception>(() => deque.Remove(this));
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
        public void NongenericSet_WrongType_ThrowsException()
        {
            var deque = new Deque<int>(new[] { 13 }) as IList;
            Assert.ThrowsAny<Exception>(() => { deque[0] = this; });
        }
    }
}