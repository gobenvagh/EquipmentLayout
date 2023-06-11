using EquipmentLayout.Models;
using EquipmentLayout.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EquipmentLayout.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
            //canvas.Loaded += Canvas_Loaded; 
        }

/*        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            DrawGrid();
        }*/

        /*private void DrawGrid()
        {
            int gridSize = 10; // Размер сетки (20 пикселей между линиями)
            int width = (int)canvas.ActualWidth;
            int height = (int)canvas.ActualHeight;
            var thickness = 0.5;
            var color = Brushes.Gray;
            // Очистка холста
            //canvas.Children.Clear();

            // Рисование горизонтальных линий
            for (int y = 0; y < height; y += gridSize)
            {
                Line line = new Line
                {
                    X1 = 0,
                    Y1 = y,
                    X2 = width,
                    Y2 = y,
                    Stroke = color,
                    StrokeThickness = thickness
                };
                canvas.Children.Add(line);
            }

            // Рисование вертикальных линий
            for (int x = 0; x < width; x += gridSize)
            {
                Line line = new Line
                {
                    X1 = x,
                    Y1 = 0,
                    X2 = x,
                    Y2 = height,
                    Stroke = color,
                    StrokeThickness = thickness
                };
                canvas.Children.Add(line);
            }
        }*/

        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //DrawGrid();
        }

        private void ListBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void ListBoxObstacle_GotFocus(object sender, RoutedEventArgs e)
        {
            ListBoxDeviceTemplate.SelectedItem = null;
        }
    }
}
