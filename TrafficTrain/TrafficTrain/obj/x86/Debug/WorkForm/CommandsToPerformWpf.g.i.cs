﻿#pragma checksum "..\..\..\..\WorkForm\CommandsToPerformWpf.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "23BA0E5ACF747584310D66FA1FE3D2F1"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
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


namespace TrafficTrain {
    
    
    /// <summary>
    /// CommandsToPerformWpf
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class CommandsToPerformWpf : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\..\WorkForm\CommandsToPerformWpf.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid TableTU;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\WorkForm\CommandsToPerformWpf.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button_close;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\WorkForm\CommandsToPerformWpf.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button_ok;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TrafficTrain;component/workform/commandstoperformwpf.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\WorkForm\CommandsToPerformWpf.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 4 "..\..\..\..\WorkForm\CommandsToPerformWpf.xaml"
            ((TrafficTrain.CommandsToPerformWpf)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TableTU = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 3:
            this.button_close = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\..\WorkForm\CommandsToPerformWpf.xaml"
            this.button_close.Click += new System.Windows.RoutedEventHandler(this.button_close_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.button_ok = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\..\WorkForm\CommandsToPerformWpf.xaml"
            this.button_ok.Click += new System.Windows.RoutedEventHandler(this.button_ok_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

