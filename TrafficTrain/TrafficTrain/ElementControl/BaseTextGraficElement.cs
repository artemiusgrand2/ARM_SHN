using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using ARM_SHN.Interface;
using ARM_SHN.EditText;
using ARM_SHN.Constant;

using SCADA.Common.Models;
using SCADA.Common.Enums;

namespace ARM_SHN.ElementControl
{
    public class BaseTextGraficElement : BaseGraficElement
    {
        /// <summary>
        /// тескт названия объекта
        /// </summary>
        public TextBlock Text { get; private set; } = new TextBlock();

        /// <summary>
        /// поворот текста
        /// </summary>
        public double RotateText { get; }

        /// <summary>
        /// текущее выравнивание текста
        /// </summary>
        TextAlignment _current_alignment = TextAlignment.Center;

        public new GraficElementTextModel ViewModel { get; private set; }

        public BaseTextGraficElement(PathGeometry geometry, double rotate = 0) : base(geometry)
        {
            ViewModel = new GraficElementTextModel();
            SetBindins(ViewModel);
            RotateText = rotate;
        }

        public BaseTextGraficElement(string name, int stationNumber, ViewElement view, TypeView typeView, PathGeometry geometry, double rotate = 0) : base(name, stationNumber, view, typeView, geometry)
        {
            ViewModel = new GraficElementTextModel(name, stationNumber, view, typeView);
            SetBindins(ViewModel);
            RotateText = rotate;
        }

        protected override bool CheckMouseOver()
        {
            return base.IsMouseOver || this.Text.IsMouseOver;
        }

        protected override void SetBindins(GraficElementModel model)
        {
            var bindingText = new Binding("Text");
            bindingText.Source = model;
            this.Text.SetBinding(TextBlock.TextProperty, bindingText);
            //
            var bindingForeground = new Binding("Foreground");
            bindingForeground.Source = model;
            this.Text.SetBinding(TextBlock.ForegroundProperty, bindingForeground);
            //
            var bindingFontSize = new Binding("FontSize");
            bindingFontSize.Source = model;
            this.Text.SetBinding(TextBlock.FontSizeProperty, bindingFontSize);
            //
            var bindingMargin = new Binding("Margin");
            bindingMargin.Source = model;
            this.Text.SetBinding(TextBlock.MarginProperty, bindingMargin);
            //
            var bindingRenderTransform = new Binding("RenderTransform");
            bindingRenderTransform.Source = model;
            this.Text.SetBinding(TextBlock.RenderTransformProperty, bindingRenderTransform);
            //
            var bindingFontWeight = new Binding("FontWeight");
            bindingFontWeight.Source = model;
            this.Text.SetBinding(TextBlock.FontWeightProperty, bindingFontWeight);
            //
            var bindingVisibility = new Binding("TextVisibility");
            bindingVisibility.Source = model;
            this.Text.SetBinding(TextBlock.VisibilityProperty, bindingVisibility);
            //
            var bindingTextWrapping = new Binding("TextWrapping");
            bindingTextWrapping.Source = model;
            this.Text.SetBinding(TextBlock.TextWrappingProperty, bindingTextWrapping);
            //
            var bindingTextAlignment = new Binding("TextAlignment");
            bindingTextAlignment.Source = model;
            this.Text.SetBinding(TextBlock.TextAlignmentProperty, bindingTextAlignment);
            //
            var bindingFontFamily = new Binding("FontFamily");
            bindingFontFamily.Source = model;
            this.Text.SetBinding(TextBlock.FontFamilyProperty, bindingFontFamily);
            //
            var bindingFontStyle = new Binding("FontStyle");
            bindingFontStyle.Source = model;
            this.Text.SetBinding(TextBlock.FontStyleProperty, bindingFontStyle);
            //
            var bindingFontStretch = new Binding("FontStretch");
            bindingFontStretch.Source = model;
            this.Text.SetBinding(TextBlock.FontStretchProperty, bindingFontStretch);
            base.SetBindins(model);
            //
            var bindingZIndex = new Binding("TextZIndex");
            bindingZIndex.Source = model;
            this.Text.SetBinding(Canvas.ZIndexProperty, bindingZIndex);
        }


        //private void LocationText()
        //{
        //    //центрируем надпись
        //    double width = AlingmentText.LenghtStorona(((ArcSegment)(DefiningGeometry as PathGeometry).Figures[0].Segments[_figure.Figures[0].Segments.Count - 2]).Point, _figure.Figures[0].StartPoint);
        //    double height = AlingmentText.LenghtStorona(((ArcSegment)_figure.Figures[0].Segments[2]).Point, _figure.Figures[0].StartPoint);
        //    m_text.FontSize = AlingmentText.FontSizeText(_ktextweight, _ktextheight,
        //        new Rectangle()
        //        {
        //            Width = width,
        //            Height = height
        //        },
        //    m_text.Text, RotateText);
        //    m_text.Margin = AlingmentText.AlingmentCenter(_figure.Figures[0].StartPoint.X, _figure.Figures[0].StartPoint.Y, width, height, m_text, RotateText);
        //}

        /// <summary>
        /// Центрируем текст по центру
        /// </summary>
        public void LocationText()
        {
            ViewModel.FontSize = AlingmentText.FontSizeText(ConstantValue.Kwtext, ConstantValue.Khtext, Width, Height,
            ViewModel.Text, RotateText);
            switch (_current_alignment)
            {
                default:
                    ViewModel.Margin = AlingmentText.AlingmentCenter((DefiningGeometry as PathGeometry).Figures[0].StartPoint.X, (DefiningGeometry as PathGeometry).Figures[0].StartPoint.Y, Width, Height, ViewModel, RotateText);
                    break;
            }
        }
    }
}
