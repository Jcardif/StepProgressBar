using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace StepProgressBench.Controls
{
    public sealed partial class StepProgressBar : UserControl, INotifyPropertyChanged
    {
        #region Colors
        public static readonly DependencyProperty ActiveStepStrokeProperty = DependencyProperty.Register(
            "ActiveStepStroke", typeof(Color), typeof(StepProgressBar), new PropertyMetadata(default(Color)));

        public Color ActiveStepStroke
        {
            get { return (Color)GetValue(ActiveStepStrokeProperty); }
            set { SetValue(ActiveStepStrokeProperty, value); }
        }

        public static readonly DependencyProperty CompletedStepFillProperty = DependencyProperty.Register(
            "CompletedStepFill", typeof(Color), typeof(StepProgressBar), new PropertyMetadata(default(Color)));

        public Color CompletedStepFill
        {
            get { return (Color)GetValue(CompletedStepFillProperty); }
            set { SetValue(CompletedStepFillProperty, value); }
        }

        
        public static readonly DependencyProperty NewStepStrokeProperty = DependencyProperty.Register(
            "NewStepStroke", typeof(Color), typeof(StepProgressBar), new PropertyMetadata(default(Color)));

        public Color NewStepStroke
        {
            get { return (Color) GetValue(NewStepStrokeProperty); }
            set { SetValue(NewStepStrokeProperty, value); }
        }


        #endregion

        #region Dimensions

        public static readonly DependencyProperty NodeWidthProperty = DependencyProperty.Register(
            "NodeWidth", typeof(int), typeof(StepProgressBar), new PropertyMetadata(default(int)));

        public int NodeWidth
        {
            get { return (int)GetValue(NodeWidthProperty); }
            set { SetValue(NodeWidthProperty, value); }
        }

        public static readonly DependencyProperty StepSpacingProperty = DependencyProperty.Register(
            "StepSpacing", typeof(int), typeof(StepProgressBar), new PropertyMetadata(default(int)));

        public int StepSpacing
        {
            get { return (int)GetValue(StepSpacingProperty); }
            set { SetValue(StepSpacingProperty, value); }
        }

        public static readonly DependencyProperty LineHeightProperty = DependencyProperty.Register(
            "LineHeight", typeof(int), typeof(StepProgressBar), new PropertyMetadata(default(int)));

        public int LineHeight
        {
            get { return (int)GetValue(LineHeightProperty); }
            set { SetValue(LineHeightProperty, value); }
        }

        public static readonly DependencyProperty TextSizeProperty = DependencyProperty.Register(
            "TextSize", typeof(double), typeof(StepProgressBar), new PropertyMetadata(default(double)));

        public double TextSize
        {
            get { return (double) GetValue(TextSizeProperty); }
            set { SetValue(TextSizeProperty, value); }
        }

        #endregion

        public static readonly DependencyProperty TextFontFamilyProperty = DependencyProperty.Register(
            "TextFontFamily", typeof(FontFamily), typeof(StepProgressBar), new PropertyMetadata(default(FontFamily)));

        public FontFamily TextFontFamily
        {
            get { return (FontFamily) GetValue(TextFontFamilyProperty); }
            set { SetValue(TextFontFamilyProperty, value); }
        }



        public static readonly DependencyProperty StepsProperty = DependencyProperty.Register(
            "Steps", typeof(List<StepProgress>), typeof(StepProgressBar), new PropertyMetadata(default(List<StepProgress>)));

        public List<StepProgress> Steps
        {
            get { return (List<StepProgress>) GetValue(StepsProperty); }
            set
            {
                SetValue(StepsProperty, value);
                CreateStepProgressBar();
            }
        }

      public StepProgressBar()
        {
            this.InitializeComponent();
        }

        private void CreateStepProgressBar()
        {
            var i = 0;
            foreach (var step in Steps)
            {
                StepProgressBarGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });

                var grid = new Grid()
                {
                    ColumnDefinitions = 
                    {
                        new ColumnDefinition {Width = new GridLength(0, GridUnitType.Auto)},
                        new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                    },
                    RowDefinitions =
                    {
                        new RowDefinition {Height = new GridLength(0, GridUnitType.Auto)},
                        new RowDefinition {Height = new GridLength(0, GridUnitType.Auto)}
                    }
                };

                grid.SetValue(Grid.ColumnProperty, i);

                var ellipse = new Ellipse()
                {
                    Width = (NodeWidth == 0) ? 50 : NodeWidth,
                    Height = (NodeWidth == 0) ? 50 : NodeWidth,
                };
                PaintEllipse(ellipse, step);


                ellipse.SetValue(Grid.ColumnProperty, 0);
                ellipse.SetValue(Grid.RowProperty, 0);
                grid.Children.Add(ellipse);


                if (i != Steps.Count-1)
                {
                    var border = new Border
                    {
                        BorderThickness = new Thickness((LineHeight == 0) ? 2 : LineHeight),
                        Height = (LineHeight == 0) ? 2 : LineHeight,
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };
                    if (StepSpacing != 0)
                        border.Width = StepSpacing;
                    border.SetValue(Grid.ColumnProperty, 1);
                    border.SetValue(Grid.RowProperty, 0);
                    grid.Children.Add(border);
                    PaintBorder(border, step);
                }

                var textBlock = new TextBlock
                {
                    Text = step.Text,
                    Width =100,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    HorizontalTextAlignment = TextAlignment.Left,
                    TextWrapping = TextWrapping.WrapWholeWords,
                    Margin = new Thickness(0, 8, 0, 0)
                };

                if (TextSize != 0)
                    textBlock.FontSize = TextSize;
                if (TextFontFamily != null)
                    textBlock.FontFamily = TextFontFamily;

                PaintTextBlock(textBlock, step);

                textBlock.SetValue(Grid.ColumnProperty, 0);
                textBlock.SetValue(Grid.RowProperty, 1);
                Grid.SetColumnSpan(textBlock, 2);
                grid.Children.Add(textBlock);

                StepProgressBarGrid.Children.Add(grid);

                i++;
            }
            
        }

        private void PaintTextBlock(TextBlock textBlock, StepProgress step)
        {
            switch (step.StepState)
            {
                case StepState.New:
                    textBlock.Foreground = NewStepStroke.ToSolidColorBrush();
                    break;
                case StepState.Active:
                    textBlock.Foreground = ActiveStepStroke.ToSolidColorBrush();
                    break;
                case StepState.Done:
                    textBlock.Foreground = ActiveStepStroke.ToSolidColorBrush();
                    textBlock.FontWeight = FontWeights.Bold;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PaintBorder(Border border, StepProgress step)
        {
            switch (step.StepState)
            {
                case StepState.New:
                    border.BorderBrush = NewStepStroke.ToSolidColorBrush();
                    break;
                case StepState.Active:
                    border.BorderBrush = NewStepStroke.ToSolidColorBrush();
                    break;
                case StepState.Done:
                    border.BorderBrush = CompletedStepFill.ToSolidColorBrush();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PaintEllipse(Ellipse ellipse, StepProgress step)
        {
            var transparent = System.Drawing.Color.Transparent;
            switch (step.StepState)
            {
                case StepState.New:
                    ellipse.Stroke = NewStepStroke.ToSolidColorBrush();
                    ellipse.Fill = Color.FromArgb(transparent.A, transparent.R, transparent.G, transparent.B).ToSolidColorBrush();
                    break;

                case StepState.Active:
                    ellipse.Stroke = ActiveStepStroke.ToSolidColorBrush();
                    ellipse.Fill = Color.FromArgb(transparent.A, transparent.R, transparent.G, transparent.B).ToSolidColorBrush();
                    break;
                case StepState.Done:
                    ellipse.Stroke = CompletedStepFill.ToSolidColorBrush();
                    ellipse.Fill = CompletedStepFill.ToSolidColorBrush();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return;

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public static class Extension
    {
        public static SolidColorBrush ToSolidColorBrush(this Color color) =>
            new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
    }

    public class StepProgress
    {
        public string Text { get; set; }
        public StepState StepState { get; set; }
    }

    public enum StepState
    {
        New, Active, Done
    }
}
