﻿using CozyArce.Tree.Base;
using CozyArce.Tree.Base.Model;
using System;
using System.Windows;

namespace CozyArce.Tree.Generator.Sample
{
    public class LowGenerator : ITreeGenerator
    {
        const double PI = 3.1415926;
        const double Arg = PI / 5;
        const double GoldenSection = 0.618;
        Random rand = new Random();
        CozyTree tree;

        public CozyTree Generate()
        {
            tree = new CozyTree();
            DrawTree(new Point(500, 850), PI / 2, 200, 30);
            return tree;
        }
        public void DrawTree(Point begin, double angle, double length, short width)
        {
            if (length < 5)
            {
                if (length < 4.98)
                {
                    tree.Leaves.Add(new CozyLeaf()
                    {
                        begin = begin,
                        end = new Point(begin.X + 2 * Math.Cos(angle), begin.Y + 2 * Math.Cos(angle)),
                    });
                    return;
                }
                else
                {
                    tree.Flowers.Add(new CozyFlower()
                    {
                        pos = begin,
                        size = 3,
                    });
                }
                return;
            }

            int x = (int)(begin.X + length * Math.Cos(angle));
            int y = (int)(begin.Y - length * Math.Sin(angle));
            var end = new Point(x, y);

            tree.Branchs.Add(new CozyBranch()
            {
                begin = begin,
                end = end,
                width = width,
            });

            width -= (short)rand.Next(5, 8);
            if (width < 1) width = 1;
            var sub = rand.Next(2, 4);
            for (var i = 0; i < sub; ++i)
            {
                var len = rand.NextDouble();
                if (len < 0.4) len += 0.4;
                if (len > 0.8) len -= 0.2;
                DrawTree(end, angle + (rand.NextDouble() - 0.5) * PI / 2, len * length, width);
            }
        }
    }
}
