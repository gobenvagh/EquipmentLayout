﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace EquipmentLayout.Models
{
    internal class Solver
    {
        private class RectInfo
        {
            public int X1 { get; set; }
            public int Y1 { get; set; }
            public int X2 { get; set; }
            public int Y2 { get; set; }
            public bool IsExtra { get; set; }
        }

        public static List<int[]> PlaceEquipment(List<int[]> childRects, List<int[]> parentRects)
        {
            List<int[]> solutions = new List<int[]>();

            foreach (var rect in childRects)
            {
                int width = rect[0];
                int height = rect[1];
                int parentWidth = parentRects[0][0];
                int parentHeight = parentRects[0][1];

                if (width > parentWidth || height > parentHeight)
                {
                    throw new InvalidOperationException("Размеры оборудования превышают размеры зоны.");
                }

                bool intersects;
                int x = 0;
                int y = 0;

                while (true)
                {
                    // Проверяем на пересечение с ранее размещенными прямоугольниками
                    intersects = solutions.Any(solution =>
                    {
                        int solutionX1 = solution[0];
                        int solutionY1 = solution[1];
                        int solutionX2 = solution[2];
                        int solutionY2 = solution[3];

                        return !(x + width <= solutionX1 || x >= solutionX2 || y + height <= solutionY1 || y >= solutionY2);
                    });

                    if (!intersects)
                    {
                        solutions.Add(new int[] { x, y, x + width, y + height });
                        break;
                    }

                    x += width;
                    if (x + width > parentWidth)
                    {
                        x = 0;
                        y += height;
                        if (y + height > parentHeight)
                        {
                            throw new InvalidOperationException("Не удалось разместить оборудование без пересечений в зоне.");
                        }
                    }
                }
            }

            return solutions;
        }


    }
}
