﻿#pragma checksum "..\..\..\..\..\..\WPF\Views\Guest1Views\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "F186985F69DF01F541934BB3B017CB8F88D1947F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using InitialProject.Localization;
using InitialProject.WPF.Views.Guest1Views;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace InitialProject.WPF.Views.Guest1Views {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\..\..\..\..\WPF\Views\Guest1Views\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton DarkTheme;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\..\..\WPF\Views\Guest1Views\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame MainPreview;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.3.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/InitialProject;component/wpf/views/guest1views/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\WPF\Views\Guest1Views\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.3.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 14 "..\..\..\..\..\..\WPF\Views\Guest1Views\MainWindow.xaml"
            ((System.Windows.Controls.ComboBox)(target)).SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ChangeLanguage);
            
            #line default
            #line hidden
            return;
            case 2:
            this.DarkTheme = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 19 "..\..\..\..\..\..\WPF\Views\Guest1Views\MainWindow.xaml"
            this.DarkTheme.Checked += new System.Windows.RoutedEventHandler(this.DarkTheme_Checked);
            
            #line default
            #line hidden
            
            #line 19 "..\..\..\..\..\..\WPF\Views\Guest1Views\MainWindow.xaml"
            this.DarkTheme.Unchecked += new System.Windows.RoutedEventHandler(this.DarkTheme_Unchecked);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 20 "..\..\..\..\..\..\WPF\Views\Guest1Views\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.NotificationsButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 27 "..\..\..\..\..\..\WPF\Views\Guest1Views\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.YourProfileButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 34 "..\..\..\..\..\..\WPF\Views\Guest1Views\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SignOutButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 37 "..\..\..\..\..\..\WPF\Views\Guest1Views\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AccommodationsButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 38 "..\..\..\..\..\..\WPF\Views\Guest1Views\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ReservationsButton_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 39 "..\..\..\..\..\..\WPF\Views\Guest1Views\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ReviewsButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 40 "..\..\..\..\..\..\WPF\Views\Guest1Views\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ForumButton_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.MainPreview = ((System.Windows.Controls.Frame)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

