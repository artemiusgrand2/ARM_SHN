using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace TrafficTrain.Themes
{

    class MesToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 2:
                   return ColorStateTableTrain.NotFixedReferenceInsideFon;
                case 1:
                   return ColorStateTableTrain.TrainWithoutReferenceFon; //new LinearGradientBrush(Color.FromArgb(255, 255, 255, 50), Color.FromArgb(127, 255, 255,220), 45);
                case 0:
                   return ColorStateTableTrain.TrainWithReferenceFon;
                default:
                   return ColorStateTableTrain.NotFixedReferenceOutsideFon;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }


    class MesToColorConverterMessage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ?
                 CommandButton._color_no_message
                : CommandButton._color_yes_message;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    class TrainToColorSelect : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ?
                 CommandButton._color_no_message
                : CommandButton._colorpasiv;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    class AutoPilotCommandUpdate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
        //{
        //    switch (((StatusCommand)value).Status)
        //    {
        //        case ViewStatusCommand.command_received:
        //            return ColorStateTableAutoPilot.ColorReceived;
        //        case ViewStatusCommand.command_check:
        //            return ColorStateTableAutoPilot.ColorCheck;
        //        case ViewStatusCommand.command_send:
        //            return ColorStateTableAutoPilot.ColorSend;
        //        case  ViewStatusCommand.command_performed:
        //            return ColorStateTableAutoPilot.ColorExecuted;
        //        case ViewStatusCommand.command_error:
        //            return ColorStateTableAutoPilot.ColorError;
        //        default:
        //            return ColorStateTableAutoPilot.ColorError;
        //    }

            return ColorStateTableAutoPilot.ColorError;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    class TrainTableText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (((int)value) != 0)
                return ColorStateTableTrain.TextPlan;
            else
                return ColorStateTableTrain.Text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

}
