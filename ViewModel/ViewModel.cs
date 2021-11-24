﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using KGLab5.Helpers;

namespace KGLab5.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Canvas _canvasView;

        public Canvas CanvasView
        {
            get => _canvasView;
            set
            {
                _canvasView = value;
                OnPropertyChanged(nameof(CanvasView));
            }
        }

        public int CanvasWidth => 500;
        public int CanvasHeight => 500;

        private Point _pointDown;

        public Point PointDown
        {
            get => _pointDown;
            set
            {
                _pointDown = value;
                OnPropertyChanged(nameof(PointDown));
                RotationFigure(value.X);
            }
        }

        private async void RotationFigure(double x)
        {
            XPosition = (int)x;
            DrawFigure(InitMatrixTransform());
            var degrees = 0;
            DrawLine(new Point(x, 0), new Point(x, 500), false);
            while (!LeftButtonIsChecked)
            {
                await Task.Run(() => Thread.Sleep(1));
                degrees += Speed;
                if (FigureLines.Count != 0)
                {
                    foreach (var figureLine in FigureLines)
                    {
                        CanvasView.Children.Remove(figureLine);
                    }
                    FigureLines.Clear();
                }
                DrawFigure(InitMatrixTransformRotateY(degrees));
            }
        }

        #region Buttons

        private bool _leftButtonIsChecked;

        public bool LeftButtonIsChecked
        {
            get => _leftButtonIsChecked;
            set
            {
                _leftButtonIsChecked = value;
                OnPropertyChanged(nameof(LeftButtonIsChecked));
            }
        }

        private bool _rightButtonIsChecked;

        public bool RightButtonIsChecked
        {
            get => _rightButtonIsChecked;
            set
            {
                _rightButtonIsChecked = value;
                OnPropertyChanged(nameof(_rightButtonIsChecked));
            }
        }

        private bool _upButtonIsChecked;

        public bool UpButtonIsChecked
        {
            get => _upButtonIsChecked;
            set
            {
                _upButtonIsChecked = value;
                OnPropertyChanged(nameof(UpButtonIsChecked));
            }
        }

        private bool _downButtonIsChecked;

        public bool DownButtonIsChecked
        {
            get => _downButtonIsChecked;
            set
            {
                _downButtonIsChecked = value;
                OnPropertyChanged(nameof(DownButtonIsChecked));
            }
        }

        #endregion

        #region Position

        private int _xPosition;

        public int XPosition
        {
            get => _xPosition;
            set
            {
                if (value > 500)
                {
                    _xPosition = 0;
                }
                else if (value < 0)
                {
                    _xPosition = 500;
                }
                else
                {
                    _xPosition = value;
                }
                OnPropertyChanged(nameof(XPosition));
            }
        }
        private int _yPosition;

        public int YPosition
        {
            get => _yPosition;
            set
            {
                if (value > 500)
                {
                    _yPosition = 0;
                }
                else if (value < 0)
                {
                    _yPosition = 500;
                }
                else
                {
                    _yPosition = value;
                }
                OnPropertyChanged(nameof(YPosition));
            }
        }

        private int _zPosition;

        public int ZPosition
        {
            get => _zPosition;
            set
            {
                if (value > 500)
                {
                    _zPosition = 0;
                }
                else if (value < 0)
                {
                    _zPosition = 500;
                }
                else
                {
                    _zPosition = value;
                }
                OnPropertyChanged(nameof(ZPosition));
            }
        }

        #endregion

        #region Properties

        private int _speed;

        public int Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                OnPropertyChanged(nameof(Speed));
            }
        }

        private double _scale;

        public double Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                OnPropertyChanged(nameof(Scale));
                if (FigureLines.Count != 0)
                {
                    foreach (var figureLine in FigureLines)
                    {
                        CanvasView.Children.Remove(figureLine);
                    }
                    DrawFigure(InitMatrixTransformScale(value));
                }
            }
        }

        #region Rotates

        private double _rotateY;

        public double RotateY
        {
            get => _rotateY;
            set
            {
                _rotateY = value;
                OnPropertyChanged(nameof(RotateY));
                if (FigureLines.Count != 0)
                {
                    foreach (var figureLine in FigureLines)
                    {
                        CanvasView.Children.Remove(figureLine);
                    }

                    DrawFigure(InitMatrixTransformRotateY(value));
                }
            }
        }

        private double _rotateX;

        public double RotateX
        {
            get => _rotateX;
            set
            {
                _rotateX = value;
                OnPropertyChanged(nameof(RotateX));
                if (FigureLines.Count != 0)
                {
                    foreach (var figureLine in FigureLines)
                    {
                        CanvasView.Children.Remove(figureLine);
                    }

                    DrawFigure(InitMatrixTransformRotateX(value));
                }
            }
        }

        private double _rotateZ;

        public double RotateZ
        {
            get => _rotateZ;
            set
            {
                _rotateZ = value;
                OnPropertyChanged(nameof(RotateZ));
                if (FigureLines.Count != 0)
                {
                    foreach (var figureLine in FigureLines)
                    {
                        CanvasView.Children.Remove(figureLine);
                    }

                    DrawFigure(InitMatrixTransformRotateZ(value));
                }
            }
        }


        #endregion

        private Color _colorLine;

        public Color ColorLine
        {
            get => _colorLine;
            set
            {
                _colorLine = value;
                OnPropertyChanged(nameof(ColorLine));
            }
        }

        private int _boldValue;

        public int BoldValue
        {
            get => _boldValue;
            set
            {
                _boldValue = value;
                OnPropertyChanged(nameof(BoldValue));
            }
        }

        #endregion

        private List<Path> _figureLines;

        public List<Path> FigureLines
        {
            get => _figureLines;
            set
            {
                _figureLines = value;
                OnPropertyChanged(nameof(FigureLines));
            }
        }
        public double[,] Axes { get; set; } = new double[3, 4];

        private double[,] _figure;

        public double[,] Figure
        {
            get => _figure;
            set
            {
                _figure = value;
                OnPropertyChanged(nameof(Figure));
            }
        }


        public ViewModel()
        {
            CanvasView = new Canvas();
            BoldValue = 3;
            ColorLine = Colors.DeepSkyBlue;
            FigureLines = new List<Path>();
            Speed = 1;
            Scale = 1;
            RotateY = 0;
            XPosition = CanvasWidth / 2;
            YPosition = CanvasHeight / 2;
            ZPosition = CanvasHeight / 2;
            DrawAxes();
            InitFigure();
            DrawFigure(InitMatrixTransform());
        }

        private void InitFigure()
        {
            Figure = new double[6, 4];
            Figure[0, 0] = 0; Figure[0, 1] = 0; Figure[0, 2] = 50; Figure[0, 3] = 1; // однородные координаты.
            //Figure[1, 0] = 0; Figure[1, 1] = 0; Figure[1, 2] = -50; Figure[1, 3] = 1;
            Figure[2, 0] = 50; Figure[2, 1] = 0; Figure[2, 2] = 0; Figure[2, 3] = 1;
            Figure[3, 0] = -50; Figure[3, 1] = 0; Figure[3, 2] = 0; Figure[3, 3] = 1;
            Figure[4, 0] = 0; Figure[4, 1] = 50; Figure[4, 2] = 0; Figure[4, 3] = 1;
            Figure[5, 0] = 0; Figure[5, 1] = -50; Figure[5, 2] = 0; Figure[5, 3] = 1;
        }

        private double[,] InitMatrixTransform()
        {
            var matrixShift = new double[4, 4];
            matrixShift[0, 0] = 1; matrixShift[0, 1] = 0; matrixShift[0, 2] = 0; matrixShift[0, 3] = 0;
            matrixShift[1, 0] = 0; matrixShift[1, 1] = 1; matrixShift[1, 2] = 0; matrixShift[1, 3] = 0;
            matrixShift[2, 0] = 0; matrixShift[2, 1] = 0; matrixShift[2, 2] = 1; matrixShift[2, 3] = 0;
            matrixShift[3, 0] = XPosition; matrixShift[3, 1] = YPosition; matrixShift[3, 2] = ZPosition; matrixShift[3, 3] = 1;
            return matrixShift;
        }

        #region Transforms

        #region RotatesMatrixTransform

        private double[,] InitMatrixTransformRotateY(double degrees)
        {
            var cos = Math.Cos(Math.PI * degrees / 180.0);
            var sin = Math.Sin(Math.PI * degrees / 180.0);
            var matrixShift = new double[4, 4];
            matrixShift[0, 0] = cos; matrixShift[0, 1] = 0; matrixShift[0, 2] = -sin; matrixShift[0, 3] = 0;
            matrixShift[1, 0] = 0; matrixShift[1, 1] = 1; matrixShift[1, 2] = 0; matrixShift[1, 3] = 0;
            matrixShift[2, 0] = sin; matrixShift[2, 1] = 0; matrixShift[2, 2] = cos; matrixShift[2, 3] = 0;
            matrixShift[3, 0] = XPosition; matrixShift[3, 1] = YPosition; matrixShift[3, 2] = ZPosition; matrixShift[3, 3] = 1;
            return matrixShift;
        }

        private double[,] InitMatrixTransformRotateX(double degrees)
        {
            var cos = Math.Cos(Math.PI * degrees / 180.0);
            var sin = Math.Sin(Math.PI * degrees / 180.0);
            var matrixShift = new double[4, 4];
            matrixShift[0, 0] = 1; matrixShift[0, 1] = 0; matrixShift[0, 2] = 0; matrixShift[0, 3] = 0;
            matrixShift[1, 0] = 0; matrixShift[1, 1] = cos; matrixShift[1, 2] = sin; matrixShift[1, 3] = 0;
            matrixShift[2, 0] = 0; matrixShift[2, 1] = -sin; matrixShift[2, 2] = cos; matrixShift[2, 3] = 0;
            matrixShift[3, 0] = XPosition; matrixShift[3, 1] = YPosition; matrixShift[3, 2] = ZPosition; matrixShift[3, 3] = 1;
            return matrixShift;
        }

        private double[,] InitMatrixTransformRotateZ(double degrees)
        {
            var cos = Math.Cos(Math.PI * degrees / 180.0);
            var sin = Math.Sin(Math.PI * degrees / 180.0);
            var matrixShift = new double[4, 4];
            matrixShift[0, 0] = cos; matrixShift[0, 1] = sin; matrixShift[0, 2] = 0; matrixShift[0, 3] = 0;
            matrixShift[1, 0] = -sin; matrixShift[1, 1] = cos; matrixShift[1, 2] = 0; matrixShift[1, 3] = 0;
            matrixShift[2, 0] = 0; matrixShift[2, 1] = 0; matrixShift[2, 2] = 1; matrixShift[2, 3] = 0;
            matrixShift[3, 0] = XPosition; matrixShift[3, 1] = YPosition; matrixShift[3, 2] = ZPosition; matrixShift[3, 3] = 1;
            return matrixShift;
        }

        #endregion

        private double[,] InitMatrixTransformScale(double scale)
        {
            var matrixShift = new double[4, 4];
            matrixShift[0, 0] = scale; matrixShift[0, 1] = 0; matrixShift[0, 2] = 0; matrixShift[0, 3] = 0;
            matrixShift[1, 0] = 0; matrixShift[1, 1] = scale; matrixShift[1, 2] = 0; matrixShift[1, 3] = 0;
            matrixShift[2, 0] = 0; matrixShift[2, 1] = 0; matrixShift[2, 2] = scale; matrixShift[2, 3] = 0;
            matrixShift[3, 0] = XPosition; matrixShift[3, 1] = YPosition; matrixShift[3, 2] = ZPosition; matrixShift[3, 3] = scale;
            return matrixShift;
        }

        #endregion

        private void DrawFigure(double[,] matrixTransform)
        {
            var figure = MultiplyMatrix(Figure, matrixTransform);

            DrawLine(new Point(figure[5, 0], figure[5, 1]), new Point(figure[0, 0], figure[0, 1]));
            DrawLine(new Point(figure[4, 0], figure[4, 1]), new Point(figure[0, 0], figure[0, 1]));
            DrawLine(new Point(figure[3, 0], figure[3, 1]), new Point(figure[0, 0], figure[0, 1]));
            DrawLine(new Point(figure[2, 0], figure[2, 1]), new Point(figure[0, 0], figure[0, 1]));

            //DrawLine(new Point(figure[5, 0], figure[5, 1]), new Point(figure[1, 0], figure[1, 1]));
            //DrawLine(new Point(figure[4, 0], figure[4, 1]), new Point(figure[1, 0], figure[1, 1]));
            //DrawLine(new Point(figure[3, 0], figure[3, 1]), new Point(figure[1, 0], figure[1, 1]));
            //DrawLine(new Point(figure[2, 0], figure[2, 1]), new Point(figure[1, 0], figure[1, 1]));

            DrawLine(new Point(figure[5, 0], figure[5, 1]), new Point(figure[2, 0], figure[2, 1]));
            DrawLine(new Point(figure[4, 0], figure[4, 1]), new Point(figure[2, 0], figure[2, 1]));
            //DrawLine(new Point(figure[1, 0], figure[1, 1]), new Point(figure[2, 0], figure[2, 1]));
            DrawLine(new Point(figure[0, 0], figure[0, 1]), new Point(figure[2, 0], figure[2, 1]));

            DrawLine(new Point(figure[5, 0], figure[5, 1]), new Point(figure[3, 0], figure[3, 1]));
            DrawLine(new Point(figure[4, 0], figure[4, 1]), new Point(figure[3, 0], figure[3, 1]));
            //DrawLine(new Point(figure[1, 0], figure[1, 1]), new Point(figure[3, 0], figure[3, 1]));
            DrawLine(new Point(figure[0, 0], figure[0, 1]), new Point(figure[3, 0], figure[3, 1]));

            DrawLine(new Point(figure[3, 0], figure[3, 1]), new Point(figure[4, 0], figure[4, 1]));
            DrawLine(new Point(figure[2, 0], figure[2, 1]), new Point(figure[4, 0], figure[4, 1]));
            //DrawLine(new Point(figure[1, 0], figure[1, 1]), new Point(figure[4, 0], figure[4, 1]));
            DrawLine(new Point(figure[0, 0], figure[0, 1]), new Point(figure[4, 0], figure[4, 1]));

            DrawLine(new Point(figure[3, 0], figure[3, 1]), new Point(figure[5, 0], figure[5, 1]));
            DrawLine(new Point(figure[2, 0], figure[2, 1]), new Point(figure[5, 0], figure[5, 1]));
            //DrawLine(new Point(figure[1, 0], figure[1, 1]), new Point(figure[5, 0], figure[5, 1]));
            DrawLine(new Point(figure[0, 0], figure[0, 1]), new Point(figure[5, 0], figure[5, 1]));
        }

        private void DrawAxes()
        {
            var axes = new double[6, 4];
            axes[0, 0] = -CanvasHeight / 2; axes[0, 1] = 0; axes[0, 2] = 0; axes[0, 3] = 1;
            axes[1, 0] = CanvasHeight / 2; axes[1, 1] = 0; axes[1, 2] = 0; axes[1, 3] = 1;
            axes[2, 0] = 0; axes[2, 1] = CanvasWidth / 2; axes[2, 2] = 0; axes[2, 3] = 1;
            axes[3, 0] = 0; axes[3, 1] = -CanvasWidth / 2; axes[3, 2] = 0; axes[3, 3] = 1;
            axes[4, 0] = 0; axes[4, 1] = 0; axes[4, 2] = CanvasWidth / 2; axes[4, 3] = 1;
            axes[5, 0] = 0; axes[5, 1] = 0; axes[5, 2] = -CanvasWidth / 2; axes[5, 3] = 1;
            var tempBold = BoldValue;
            var tempColor = ColorLine;
            ColorLine = Colors.Blue;
            BoldValue = 1;
            axes = MultiplyMatrix(axes, InitMatrixTransform());
            DrawLine(new Point(axes[0, 0], axes[0, 1]), new Point(axes[1, 0], axes[1, 1]), false);
            DrawLine(new Point(axes[2, 0], axes[2, 1]), new Point(axes[3, 0], axes[3, 1]), false);
            DrawLine(new Point(axes[4, 0], axes[4, 1]), new Point(axes[5, 0], axes[5, 1]), false);
            BoldValue = tempBold;
            ColorLine = tempColor;
        }

        #region Commands

        public RelayCommand XOYView => new(obj =>
        {
            var matrixShift = new double[4, 4];
            matrixShift[0, 0] = 1; matrixShift[0, 1] = 0; matrixShift[0, 2] = 0; matrixShift[0, 3] = 0;
            matrixShift[1, 0] = 0; matrixShift[1, 1] = 1; matrixShift[1, 2] = 0; matrixShift[1, 3] = 0;
            matrixShift[2, 0] = 0; matrixShift[2, 1] = 0; matrixShift[2, 2] = -1; matrixShift[2, 3] = 0;
            matrixShift[3, 0] = 250; matrixShift[3, 1] = 250; matrixShift[3, 2] = 0; matrixShift[3, 3] = 1;
            DrawFigure(matrixShift);
        });
        public RelayCommand YOZView => new(obj =>
        {
            var matrixShift = new double[4, 4];
            matrixShift[0, 0] = -1; matrixShift[0, 1] = 0; matrixShift[0, 2] = 0; matrixShift[0, 3] = 0;
            matrixShift[1, 0] = 0; matrixShift[1, 1] = 1; matrixShift[1, 2] = 0; matrixShift[1, 3] = 0;
            matrixShift[2, 0] = 0; matrixShift[2, 1] = 0; matrixShift[2, 2] = 1; matrixShift[2, 3] = 0;
            matrixShift[3, 0] = 250; matrixShift[3, 1] = 250; matrixShift[3, 2] = 0; matrixShift[3, 3] = 1;
            DrawFigure(matrixShift);
        });

        public RelayCommand XOZView => new(obj =>
        {
            var matrixShift = new double[4, 4];
            matrixShift[0, 0] = 1; matrixShift[0, 1] = 0; matrixShift[0, 2] = 0; matrixShift[0, 3] = 0;
            matrixShift[1, 0] = 0; matrixShift[1, 1] = -1; matrixShift[1, 2] = 0; matrixShift[1, 3] = 0;
            matrixShift[2, 0] = 0; matrixShift[2, 1] = 0; matrixShift[2, 2] = 1; matrixShift[2, 3] = 0;
            matrixShift[3, 0] = 250; matrixShift[3, 1] = 250; matrixShift[3, 2] = 0; matrixShift[3, 3] = 1;
            DrawFigure(matrixShift);
        });
        public RelayCommand ClearCommand => new(obj =>
        {
            CanvasView.Children.Clear();
        });

        public RelayCommand DrawFigureCommand => new RelayCommand(obj =>
        {
            DrawFigure(InitMatrixTransform());
        });

        public RelayCommand LeftTransformCommand => new RelayCommand(async obj =>
        {
            while (LeftButtonIsChecked)
            {
                await Task.Run(() => Thread.Sleep(1));
                XPosition -= Speed;
                foreach (var figureLine in FigureLines)
                {
                    CanvasView.Children.Remove(figureLine);
                }
                FigureLines.Clear();
                DrawFigure(InitMatrixTransform());
            }
        }, obj => FigureLines.Count != 0);

        public RelayCommand RightTransformCommand => new RelayCommand(async obj =>
        {
            while (RightButtonIsChecked)
            {
                await Task.Run(() => Thread.Sleep(1));
                XPosition += Speed;
                foreach (var figureLine in FigureLines)
                {
                    CanvasView.Children.Remove(figureLine);
                }
                FigureLines.Clear();
                DrawFigure(InitMatrixTransform());
            }
        }, obj => FigureLines.Count != 0);

        public RelayCommand UpTransformCommand => new RelayCommand(async obj =>
        {
            while (UpButtonIsChecked)
            {
                await Task.Run(() => Thread.Sleep(1));
                YPosition -= Speed;
                foreach (var figureLine in FigureLines)
                {
                    CanvasView.Children.Remove(figureLine);
                }
                FigureLines.Clear();
                DrawFigure(InitMatrixTransform());
            }
        }, obj => FigureLines.Count != 0);

        public RelayCommand DownTransformCommand => new RelayCommand(async obj =>
        {
            while (DownButtonIsChecked)
            {
                await Task.Run(() => Thread.Sleep(1));
                YPosition += Speed;
                foreach (var figureLine in FigureLines)
                {
                    CanvasView.Children.Remove(figureLine);
                }
                FigureLines.Clear();
                DrawFigure(InitMatrixTransform());
            }
        }, obj => FigureLines.Count != 0);
        #endregion

        #region Helpers

        private void DrawLine(Point startPoint, Point endPoint, bool IsFigure = true)
        {
            var line = new LineGeometry(startPoint, endPoint);
            var myPath1 = new Path
            {
                Stroke = new SolidColorBrush(ColorLine),
                StrokeThickness = BoldValue
            };
            var gp = new GeometryGroup();
            gp.Children.Add(line);
            myPath1.Data = gp;
            CanvasView.Children.Add(myPath1);
            if (IsFigure)
            {
                FigureLines.Add(myPath1);
            }
        }
        private double[,] MultiplyMatrix(double[,] a, double[,] b)
        {
            var n = a.GetLength(0);
            var m = a.GetLength(1);

            var r = new double[n, m];
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < m; j++)
                {
                    r[i, j] = 0;
                    for (var ii = 0; ii < m; ii++)
                    {
                        r[i, j] += a[i, ii] * b[ii, j];
                    }
                }
            }
            return r;
        }

        #endregion

        #region Notify

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}