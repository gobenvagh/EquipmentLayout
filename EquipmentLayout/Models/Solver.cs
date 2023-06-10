using Google.OrTools.Sat;
using System.Collections.Generic;

namespace EquipmentLayout.Models
{
    internal class Solver
    {
        private class RectInfo
        {
            public IntVar X1 { get; set; }
            public IntVar Y1 { get; set; }
            public IntVar X2 { get; set; }
            public IntVar Y2 { get; set; }
            public IntervalVar XInterval { get; set; }
            public IntervalVar YInterval { get; set; }
            public bool IsExtra { get; set; }
        }

        public static List<int[]> PlaceEquipment(List<int[]> childRects, List<int[]> parentRects)
        {
            CpModel model = new CpModel();
            List<RectInfo> allVars = new List<RectInfo>();
            List<IntervalVar> xIntervals = new List<IntervalVar>();
            List<IntervalVar> yIntervals = new List<IntervalVar>();

            int parentWidth = parentRects[0][0]; 
            int parentHeight = parentRects[0][1]; 


            foreach (var rect in childRects)
            {
                int width = rect[0];
                int height = rect[1];
                int area = width * height;

                IntVar x1Var = model.NewIntVar(0, parentWidth, $"x1_{width}_{height}");
                IntVar x2Var = model.NewIntVar(0, parentWidth, $"x2_{width}_{height}");
                IntervalVar xIntervalVar = model.NewIntervalVar(x1Var, width, x2Var, $"x_interval_{width}_{height}");

                IntVar y1Var = model.NewIntVar(0, parentHeight, $"y1_{width}_{height}");
                IntVar y2Var = model.NewIntVar(0, parentHeight, $"y2_{width}_{height}");
                IntervalVar yIntervalVar = model.NewIntervalVar(y1Var, height, y2Var, $"y_interval_{width}_{height}");

                xIntervals.Add(xIntervalVar);
                yIntervals.Add(yIntervalVar);

                allVars.Add(new RectInfo
                {
                    X1 = x1Var,
                    Y1 = y1Var,
                    X2 = x2Var,
                    Y2 = y2Var,
                    XInterval = xIntervalVar,
                    YInterval = yIntervalVar,
                    IsExtra = false
                });
            }


            model.AddNoOverlap(xIntervals.ToArray());
            model.AddNoOverlap(yIntervals.ToArray());

            CpSolver solver = new CpSolver();
            CpSolverStatus status = solver.Solve(model);
            List<int[]> solutions = new List<int[]>();

            if (status == CpSolverStatus.Optimal)
            {
                foreach (var rect in allVars)
                {
                    int x1 = (int)solver.Value(rect.X1);
                    int x2 = (int)solver.Value(rect.X2);
                    int y1 = (int)solver.Value(rect.Y1);
                    int y2 = (int)solver.Value(rect.Y2);
                    solutions.Add(new int[] { x1, y1, x2, y2 });
                }
            }

            return solutions;
        }

    }
}
