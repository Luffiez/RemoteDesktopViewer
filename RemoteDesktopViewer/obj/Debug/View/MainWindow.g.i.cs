﻿#pragma checksum "..\..\..\View\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "304994A706F8EEBED8A8ECFBDBB4AAC7CC24702B5BFC08EDB96FEB8674E0DA03"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using RemoteDesktopViewer;
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


namespace RemoteDesktopViewer {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 52 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox GroupList;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ConnectButton;
        
        #line default
        #line hidden
        
        
        #line 126 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ConnectionList;
        
        #line default
        #line hidden
        
        
        #line 143 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem JoinSession_MenuItem;
        
        #line default
        #line hidden
        
        
        #line 144 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem TakeOver_MenuItem;
        
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
            System.Uri resourceLocater = new System.Uri("/RemoteDesktopViewer;component/view/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\MainWindow.xaml"
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
            
            #line 10 "..\..\..\View\MainWindow.xaml"
            ((RemoteDesktopViewer.MainWindow)(target)).Activated += new System.EventHandler(this.WindowActivated);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 34 "..\..\..\View\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Click_CreateGroup);
            
            #line default
            #line hidden
            return;
            case 3:
            this.GroupList = ((System.Windows.Controls.ListBox)(target));
            
            #line 55 "..\..\..\View\MainWindow.xaml"
            this.GroupList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Group_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 86 "..\..\..\View\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Click_CreateGroup);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 87 "..\..\..\View\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Click_EditGroup);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 88 "..\..\..\View\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Click_DeleteGroup);
            
            #line default
            #line hidden
            return;
            case 7:
            this.ConnectButton = ((System.Windows.Controls.Button)(target));
            
            #line 108 "..\..\..\View\MainWindow.xaml"
            this.ConnectButton.Click += new System.Windows.RoutedEventHandler(this.Click_Connect);
            
            #line default
            #line hidden
            return;
            case 8:
            this.ConnectionList = ((System.Windows.Controls.ListView)(target));
            
            #line 128 "..\..\..\View\MainWindow.xaml"
            this.ConnectionList.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.Connection_OnClick);
            
            #line default
            #line hidden
            
            #line 129 "..\..\..\View\MainWindow.xaml"
            this.ConnectionList.MouseRightButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.StationList_PreviewMouseRightButtonDown);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 142 "..\..\..\View\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Click_Connect);
            
            #line default
            #line hidden
            return;
            case 10:
            this.JoinSession_MenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 143 "..\..\..\View\MainWindow.xaml"
            this.JoinSession_MenuItem.Click += new System.Windows.RoutedEventHandler(this.Click_Join);
            
            #line default
            #line hidden
            return;
            case 11:
            this.TakeOver_MenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 144 "..\..\..\View\MainWindow.xaml"
            this.TakeOver_MenuItem.Click += new System.Windows.RoutedEventHandler(this.Click_TakeOver);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 145 "..\..\..\View\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.RightClick_EditStation);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

