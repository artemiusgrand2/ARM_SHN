﻿#pragma checksum "..\..\..\..\..\Strage\DrawElement\Strage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "52AE8BF6056053944BBCC7C4A43F96A3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace TrafficTrain.Strage.DrawElement {
    
    
    /// <summary>
    /// Strage
    /// </summary>
    public partial class Strage : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 4 "..\..\..\..\..\Strage\DrawElement\Strage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal TrafficTrain.Strage.DrawElement.Strage MainStrage;
        
        #line default
        #line hidden
        
        
        #line 9 "..\..\..\..\..\Strage\DrawElement\Strage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkBox_visible;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\..\..\Strage\DrawElement\Strage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas DrawCanvas;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TrafficTrain;component/strage/drawelement/strage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Strage\DrawElement\Strage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.MainStrage = ((TrafficTrain.Strage.DrawElement.Strage)(target));
            
            #line 5 "..\..\..\..\..\Strage\DrawElement\Strage.xaml"
            this.MainStrage.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.MainStrage_MouseDown);
            
            #line default
            #line hidden
            
            #line 5 "..\..\..\..\..\Strage\DrawElement\Strage.xaml"
            this.MainStrage.MouseMove += new System.Windows.Input.MouseEventHandler(this.MainStrage_MouseMove);
            
            #line default
            #line hidden
            
            #line 5 "..\..\..\..\..\Strage\DrawElement\Strage.xaml"
            this.MainStrage.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(this.MainStrage_MouseWheel);
            
            #line default
            #line hidden
            
            #line 5 "..\..\..\..\..\Strage\DrawElement\Strage.xaml"
            this.MainStrage.SizeChanged += new System.Windows.SizeChangedEventHandler(this.MainStrage_SizeChanged);
            
            #line default
            #line hidden
            
            #line 5 "..\..\..\..\..\Strage\DrawElement\Strage.xaml"
            this.MainStrage.LocationChanged += new System.EventHandler(this.MainStrage_LocationChanged);
            
            #line default
            #line hidden
            
            #line 6 "..\..\..\..\..\Strage\DrawElement\Strage.xaml"
            this.MainStrage.KeyDown += new System.Windows.Input.KeyEventHandler(this.MainStrage_KeyDown);
            
            #line default
            #line hidden
            
            #line 6 "..\..\..\..\..\Strage\DrawElement\Strage.xaml"
            this.MainStrage.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.MainStrage_MouseUp);
            
            #line default
            #line hidden
            
            #line 6 "..\..\..\..\..\Strage\DrawElement\Strage.xaml"
            this.MainStrage.Loaded += new System.Windows.RoutedEventHandler(this.MainStrage_Loaded);
            
            #line default
            #line hidden
            
            #line 6 "..\..\..\..\..\Strage\DrawElement\Strage.xaml"
            this.MainStrage.Closing += new System.ComponentModel.CancelEventHandler(this.MainStrage_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.checkBox_visible = ((System.Windows.Controls.CheckBox)(target));
            
            #line 9 "..\..\..\..\..\Strage\DrawElement\Strage.xaml"
            this.checkBox_visible.Click += new System.Windows.RoutedEventHandler(this.checkBox_visible_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.DrawCanvas = ((System.Windows.Controls.Canvas)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

