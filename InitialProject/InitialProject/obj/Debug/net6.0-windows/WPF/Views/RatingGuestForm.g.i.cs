﻿#pragma checksum "..\..\..\..\..\WPF\Views\RatingGuestForm.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6B66937C16DE134F7901D6C3BA36B5AF8FBC50EF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using InitialProject.WPF.Views;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace InitialProject.WPF.Views {
    
    
    /// <summary>
    /// RatingGuestForm
    /// </summary>
    public partial class RatingGuestForm : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\..\..\..\WPF\Views\RatingGuestForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ComboBoxCleanliness;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\..\WPF\Views\RatingGuestForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ComboBoxFollowingTheRules;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\..\WPF\Views\RatingGuestForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TextBlockComment;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\..\WPF\Views\RatingGuestForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextBoxComment;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\..\WPF\Views\RatingGuestForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonRate;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\..\WPF\Views\RatingGuestForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonCancel;
        
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
            System.Uri resourceLocater = new System.Uri("/InitialProject;V1.0.0.0;component/wpf/views/ratingguestform.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\WPF\Views\RatingGuestForm.xaml"
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
            this.ComboBoxCleanliness = ((System.Windows.Controls.ComboBox)(target));
            
            #line 20 "..\..\..\..\..\WPF\Views\RatingGuestForm.xaml"
            this.ComboBoxCleanliness.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ComboBoxFollowingTheRules = ((System.Windows.Controls.ComboBox)(target));
            
            #line 31 "..\..\..\..\..\WPF\Views\RatingGuestForm.xaml"
            this.ComboBoxFollowingTheRules.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.TextBlockComment = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.TextBoxComment = ((System.Windows.Controls.TextBox)(target));
            
            #line 41 "..\..\..\..\..\WPF\Views\RatingGuestForm.xaml"
            this.TextBoxComment.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextBoxComment_TextChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ButtonRate = ((System.Windows.Controls.Button)(target));
            
            #line 44 "..\..\..\..\..\WPF\Views\RatingGuestForm.xaml"
            this.ButtonRate.Click += new System.Windows.RoutedEventHandler(this.ButtonRate_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.ButtonCancel = ((System.Windows.Controls.Button)(target));
            
            #line 45 "..\..\..\..\..\WPF\Views\RatingGuestForm.xaml"
            this.ButtonCancel.Click += new System.Windows.RoutedEventHandler(this.ButtonCancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
