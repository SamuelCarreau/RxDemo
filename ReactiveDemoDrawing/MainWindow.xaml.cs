using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReactiveDemoDrawing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ObserveDrawOnCanvas(canvas);
        }

        public IDisposable ObserveDrawOnCanvas(Canvas canvas)
        {
            var mouseDown =
                Observable.FromEventPattern<MouseButtonEventArgs>(canvas, nameof(canvas.MouseDown));
            var mouseUp =
                Observable.FromEventPattern<MouseButtonEventArgs>(canvas, nameof(canvas.MouseUp));
            var movements =
                Observable.FromEventPattern<MouseEventArgs>(canvas, nameof(canvas.MouseMove));

            Polyline line = null;
            return movements
                .SkipUntil(
                    mouseDown.Do(_ =>
                    {
                        line = new Polyline() { Stroke = Brushes.White, StrokeThickness = 3 };
                        canvas.Children.Add(line);
                    }))
                .TakeUntil(mouseUp)
                .Select(m => m.EventArgs.GetPosition(canvas))
                .Repeat()
                .Subscribe(pos => line.Points.Add(pos));
        }
    }
}
