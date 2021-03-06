﻿using System.Collections.Generic;
using AoC2019.Days;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2019.Tests
{
    [TestClass]
    public class Day1Tests : DayTestBase<Day1, IEnumerable<int>, int>
    {
        protected override int ExpectedPart1Result => 3317100;
        protected override int ExpectedPart2Result => 4972784;
    }

    [TestClass]
    public sealed class Day2Tests : DayTestBase<Day2, int[], int>
    {
        protected override int ExpectedPart1Result => 9581917;
        protected override int ExpectedPart2Result => 2505;
    }

    [TestClass]
    public sealed class Day3Tests : DayTestBase<Day3, Dictionary<(int, int), int>[], int>
    {
        protected override int ExpectedPart1Result => 855;
        protected override int ExpectedPart2Result => 11238;
    }

    [TestClass]
    public sealed class Day4Tests : DayTestBase<Day4, (int, int), int>
    {
        protected override int ExpectedPart1Result => 1955;
        protected override int ExpectedPart2Result => 1319;
    }

    [TestClass]
    public sealed class Day5Tests : DayTestBase<Day5, long[], long>
    {
        protected override long ExpectedPart1Result => 9775037;
        protected override long ExpectedPart2Result => 15586959;
    }

    [TestClass]
    public sealed class Day6Tests : DayTestBase<Day6, Dictionary<string, Day6.Orbit>, int>
    {
        protected override int ExpectedPart1Result => 295834;
        protected override int ExpectedPart2Result => 361;
    }

    [TestClass]
    public sealed class Day7Tests : DayTestBase<Day7, long[], long>
    {
        protected override long ExpectedPart1Result => 30940;
        protected override long ExpectedPart2Result => 76211147;
    }

    [TestClass]
    public sealed class Day8Tests : DayTestBase<Day8, List<int[,]>, string>
    {
        protected override string ExpectedPart1Result => "1596";
        protected override string ExpectedPart2Result => "LBRCE";
    }

    [TestClass]
    public sealed class Day9Tests : DayTestBase<Day9, long[], long>
    {
        protected override long ExpectedPart1Result => 3454977209;
        protected override long ExpectedPart2Result => 50120;
    }

    [TestClass]
    public sealed class Day10Tests : DayTestBase<Day10, List<(int X, int Y)>, int>
    {
        protected override int ExpectedPart1Result => 280;
        protected override int ExpectedPart2Result => 706;
    }

    [TestClass]
    public sealed class Day11Tests : DayTestBase<Day11, long[], string>
    {
        protected override string ExpectedPart1Result => "1876";
        protected override string ExpectedPart2Result => "CGPJCGCL";
    }

    [TestClass]
    public sealed class Day12Tests : DayTestBase<Day12, Day12.Moon[], long>
    {
        protected override long ExpectedPart1Result => 8538;
        protected override long ExpectedPart2Result => 506359021038056;
    }
}
