using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UUIDTests
{
    public class ArrayExtensionTests
    {
        [Fact]
        public void Fill_ShouldGenerateUniqueValues()
        {
            UUID[] array = new UUID[1000];
            array.Fill();

            HashSet<UUID> set = new(array);
            Assert.Equal(array.Length, set.Count);
        }

        [Fact]
        public void Fill_ShouldThrowOnNullArray()
        {
            UUID[] array = null;
            Assert.Throws<ArgumentNullException>(array.Fill);
        }

        [Fact]
        public void TryFill_ShouldHandleNullAndEmptyArrays()
        {
            // Null array
            UUID[] nullArray = null;
            Assert.False(nullArray.TryFill());

            // Empty array
            UUID[] emptyArray = Array.Empty<UUID>();
            Assert.True(emptyArray.TryFill());
        }

        [Fact]
        public void TryFill_ShouldGenerateUniqueValues()
        {
            UUID[] array = new UUID[1000];
            Assert.True(array.TryFill());

            HashSet<UUID> set = new(array);
            Assert.Equal(array.Length, set.Count);
        }

        [Fact]
        public void Generate_ShouldCreateArrayOfSpecifiedSize()
        {
            int size = 100;
            UUID[] array = ArrayExtension.Generate(size);

            Assert.Equal(size, array.Length);
            Assert.DoesNotContain(array, uuid => uuid.Equals(default));
        }

        [Fact]
        public void Generate_ShouldThrowOnNegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ArrayExtension.Generate(-1));
        }

        [Fact]
        public void TryGenerate_ShouldHandleValidAndInvalidCounts()
        {
            // Negative count
            Assert.False(ArrayExtension.TryGenerate(-1, out UUID[] result));
            Assert.Null(result);

            // Zero count
            Assert.True(ArrayExtension.TryGenerate(0, out result));
            Assert.Empty(result);

            // Valid count
            Assert.True(ArrayExtension.TryGenerate(100, out result));
            Assert.Equal(100, result.Length);
            Assert.DoesNotContain(result, uuid => uuid.Equals(default));
        }

        [Fact]
        public void Generate_And_TryGenerate_ShouldCreateUniqueValues()
        {
            // Test Generate
            UUID[] array1 = ArrayExtension.Generate(500);
            HashSet<UUID> set1 = new(array1);
            Assert.Equal(array1.Length, set1.Count);

            // Test TryGenerate
            Assert.True(ArrayExtension.TryGenerate(500, out UUID[] array2));
            HashSet<UUID> set2 = new(array2);
            Assert.Equal(array2.Length, set2.Count);

            // Test that arrays are different
            Assert.Empty(array1.Intersect(array2));
        }

        [Fact]
        public void FillParallel_ShouldGenerateUniqueValues()
        {
            UUID[] array = new UUID[2000];
            array.FillParallel();

            HashSet<UUID> set = new(array);
            Assert.Equal(array.Length, set.Count);
        }

        [Fact]
        public void FillParallel_ShouldThrowOnNullArray()
        {
            UUID[] array = null;
            Assert.Throws<ArgumentNullException>(() => array.FillParallel());
        }

        [Theory]
        [InlineData(500)]
        [InlineData(1000)]
        [InlineData(2000)]
        public void FillParallel_ShouldHandleVariousThresholds(int threshold)
        {
            UUID[] array = new UUID[2000];
            array.FillParallel(threshold);

            HashSet<UUID> set = new(array);
            Assert.Equal(array.Length, set.Count);
        }

        [Fact]
        public void TryFillParallel_ShouldHandleNullAndEmptyArrays()
        {
            // Null array
            UUID[] nullArray = null;
            Assert.False(nullArray.TryFillParallel());

            // Empty array
            UUID[] emptyArray = Array.Empty<UUID>();
            Assert.True(emptyArray.TryFillParallel());
        }

        [Fact]
        public void TryFillParallel_ShouldGenerateUniqueValues()
        {
            UUID[] array = new UUID[2000];
            Assert.True(array.TryFillParallel());

            HashSet<UUID> set = new(array);
            Assert.Equal(array.Length, set.Count);
        }

        [Theory]
        [InlineData(500)]
        [InlineData(1000)]
        [InlineData(2000)]
        public void TryFillParallel_ShouldHandleVariousThresholds(int threshold)
        {
            UUID[] array = new UUID[2000];
            Assert.True(array.TryFillParallel(threshold));

            HashSet<UUID> set = new(array);
            Assert.Equal(array.Length, set.Count);
        }

        [Fact]
        public void GenerateOrdered_And_TryGenerateOrdered_ShouldCreateUniqueValues()
        {
            // Test GenerateOrdered
            UUID[] array1 = ArrayExtension.GenerateOrdered(500);
            HashSet<UUID> set1 = new(array1);
            Assert.Equal(array1.Length, set1.Count);

            // Test TryGenerateOrdered
            Assert.True(ArrayExtension.TryGenerateOrdered(500, out UUID[] array2));
            HashSet<UUID> set2 = new(array2);
            Assert.Equal(array2.Length, set2.Count);

            // Test that arrays are different
            Assert.Empty(array1.Intersect(array2));
        }

        [Fact]
        public void TryGenerateOrdered_ShouldHandleInvalidCounts()
        {
            // Negative count
            Assert.False(ArrayExtension.TryGenerateOrdered(-1, out UUID[] result));
            Assert.Null(result);

            // Zero count
            Assert.True(ArrayExtension.TryGenerateOrdered(0, out result));
            Assert.Empty(result);
        }

        [Fact]
        public void TryGenerateOrdered_ShouldRespectStartTime()
        {
            var startTime = DateTimeOffset.UtcNow.AddDays(-1);
            Assert.True(ArrayExtension.TryGenerateOrdered(10, out UUID[] result, startTime));

            // Check if all UUIDs have timestamps after startTime
            foreach (var uuid in result)
            {
                Assert.True(uuid.Time >= startTime);
            }

            // Check if timestamps are sequential
            for (int i = 1; i < result.Length; i++)
            {
                Assert.True(result[i].Time > result[i - 1].Time);
            }
        }

        [Fact]
        public void GenerateOrdered_ShouldMaintainSequentialOrder()
        {
            int size = 100;
            UUID[] array = ArrayExtension.GenerateOrdered(size);

            // Check if array is ordered
            for (int i = 1; i < array.Length; i++)
            {
                Assert.True(array[i] > array[i - 1]);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void GenerateOrdered_ShouldHandleVariousSizes(int size)
        {
            UUID[] array = ArrayExtension.GenerateOrdered(size);
            Assert.Equal(size, array.Length);

            if (size > 1)
            {
                // Verify ordering
                for (int i = 1; i < array.Length; i++)
                {
                    Assert.True(array[i] > array[i - 1]);
                }
            }
        }
    }
}