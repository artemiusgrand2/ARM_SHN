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

        public override bool IsMouseOver
        {
            get
            {
                return base.IsMouseOver || Text.IsMouseOver;
            }
        }

        public BaseTextGraficElement(PathGeometry geometry, double rotate = 0) : base(geometry)
        {
            ViewModel = new GraficElementTextModel(); 
            SetBindins(ViewModel);
            RotateText = rotate;
        }

        public BaseTextGraficElement(string name,  ViewElement view, PathGeometry geometry, double rotate = 0) : base(name, view,geometry)
        {
            ViewModel = new GraficElementTextModel(name, view);
            SetBindins(ViewModel);
            RotateText = rotate;
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


        /// <summary>
        /// Центрируем текст по центру
        /// </summary>
        public void LocationText()
        {
            ViewModel.FontSize = AlingmentText.FontSizeText(ConstantValue.Kwtext, ConstantValue.Khtext, _width, _height,
            ViewModel.Text, RotateText);
            switch (_current_alignment)
            {
                default:
                    ViewModel.Margin = AlingmentText.AlingmentCenter(_startPoint.X, _startPoint.Y, _width, _height, ViewModel, RotateText);
                    break;
            }

        }
    }
}
