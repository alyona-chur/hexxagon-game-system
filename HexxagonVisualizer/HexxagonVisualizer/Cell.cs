using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ConstantsLibrary;

namespace HexxagonVisualizer
{
    public class Cell
    {
        #region Var

        private int _row;
        private int _col;
        private int _type;
        //шесть точек по две координаты
        private Point[] _points;
        private PointCollection _pointsCollect;

        #endregion

        #region Properties

        public int Row
        {
            get
            {
                return _row;
            }
            set
            {
                _row = value;
            }
        }
        public int Col
        {
            get
            {
                return _col;
            }
            set
            {
                _col = value;
            }
        }
        public int Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
        public Point[] Points
        {
            get
            {
                return _points;
            }
            set
            {
                _points = value;
            }
        }
        public PointCollection PointsCollect
        {
            get
            {
                return _pointsCollect;
            }
            set
            {
                _pointsCollect = value;
            }
        }

        #endregion

        #region Constroctors

        public Cell()
        {
            Row = 0;
            Col = 0;
            Type = Constants.CELL_NOT_EXIST;
            Points = new Point[6];
            PointsCollect = new PointCollection();
        }
        public Cell(int row, int col, int type)
        {
            Row = row;
            Col = col;
            Type = type;
            Points = new Point[6];
            PointsCollect = new PointCollection();
        }

        #endregion

        public void InitCellPoint(int i, double x, double y)
        {
            Points[i] = new Point(x, y);
        }
        public void InitCellPointsColl()
        {
            for(int i = 0; i < 6; ++i)
                PointsCollect.Add(Points[i]);
        }
    }
}
