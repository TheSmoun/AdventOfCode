﻿using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace AoC2019.Days
{
    public sealed class Day8 : DayBase<List<int[,]>, string>
    {
        private const int ImageWidth = 25;
        private const int ImageHeight = 6;

        public override string Name => "Day 8: Space Image Format";

        public override List<int[,]> ParseInput(IEnumerable<string> lines)
        {
            var pixels = lines.Single().ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();

            var image = new List<int[,]>();
            foreach (var layerPixels in pixels.Batch(ImageWidth * ImageHeight))
            {
                var layer = new int[ImageHeight, ImageWidth];
                var y = 0;
                foreach (var row in layerPixels.Batch(ImageWidth))
                {
                    var x = 0;
                    foreach (var i in row)
                    {
                        layer[y, x++] = i;
                    }

                    y++;
                }

                image.Add(layer);
            }

            return image;
        }

        public override string RunPart1(List<int[,]> input)
        {
            var minLayer = input.Select(l => l.Cast<int>()).MinBy(l => l.Count(p => p == 0)).First().ToList();
            return (minLayer.Count(p => p == 1) * minLayer.Count(p => p == 2)).ToString();
        }

        public override string RunPart2(List<int[,]> input)
        {
            for (var y = 0; y < ImageHeight; y++)
            {
                for (var x = 0; x < ImageWidth; x++)
                {
                    var pixel = input.Select(l => l[y, x]).First(p => p != 2);
                    Console.Write(pixel == 0 ? " " : "#");
                }

                Console.WriteLine();
            }

            return "LBRCE";
        }
    }
}
