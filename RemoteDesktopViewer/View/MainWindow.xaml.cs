using System.Windows;
using System.Diagnostics;
using RemoteDesktopViewer.ViewModel;
using RemoteDesktopViewer.Model;
using System.Windows.Controls;
using System.Windows.Input;
using System;
using RemoteDesktopViewer.Functionality;

namespace RemoteDesktopViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel mainWindowViewModel;
        GroupManagerViewModel groupManagerViewModel;

        public MainWindow(MainWindowViewModel _mainWindowViewModel, GroupManagerViewModel _groupManagerViewModel)
        {
            mainWindowViewModel = _mainWindowViewModel;
            groupManagerViewModel = _groupManagerViewModel;
            InitializeComponent();
        }

        private void Click_Connect(object sender, RoutedEventArgs e)
        {
            var selectedItem = ConnectionList.SelectedItem;
            if (selectedItem is ConnectionModel item)
            {
                mainWindowViewModel.ConnectToStation(item);
                Debug.WriteLine("Clicked on connect: " + e.Source.ToString());
            }
        }

        private void Click_TakeOver(object sender, RoutedEventArgs e)
        {
            var selectedItem = ConnectionList.SelectedItem;
            if (selectedItem is ConnectionModel item)
            {
                mainWindowViewModel.TakeOverStation(item);
                Debug.WriteLine("Clicked on take over: " + e.Source.ToString());
            }
        }

        private void StationList_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView view)
            {
                if (view.SelectedIndex == -1)
                    return;

                if(view.Items[view.SelectedIndex] is ConnectionModel connection)
                {
                    if (connection.ConnectionStatus == "Occupied")
                    {
                        JoinSession_MenuItem.IsEnabled = true;
                        TakeOver_MenuItem.IsEnabled = true;
                    }
                    else
                    {
                        JoinSession_MenuItem.IsEnabled = false;
                        TakeOver_MenuItem.IsEnabled = false;
                    }
                }
            }
        }

        //private void WindowActivated(object sender, EventArgs e)
        //{
        //    if(mainWindowViewModel != null)
        //        mainWindowViewModel.UpdateGroupStatus();
        //}

        private void RightClick_EditStation(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Right-clicked on station: " + e.Source.ToString());
        }

        private void Group_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count > 0)
            {
                Debug.WriteLine("Group selection changed: " + e.AddedItems[0]);
                mainWindowViewModel.ChangeSelectedGroup(e.AddedItems[0] as ConnectionGroupModel);
            }
        }

        private void Connection_OnClick(object sender,MouseButtonEventArgs e)
        {
            if(ConnectionList.Items.Count > 0)
                mainWindowViewModel.UpdateConnectionStatus(ConnectionList.SelectedIndex);
        }

        private void Click_CreateGroup(object sender, RoutedEventArgs e)
        {
            groupManagerViewModel.OpenWindow_Create();
        }

        private void Click_EditGroup(object sender, RoutedEventArgs e)
        {
            var selectedItem = GroupList.SelectedItem;
            if(selectedItem is ConnectionGroupModel item)
            {
                mainWindowViewModel.GroupBeingEdited = item;
                groupManagerViewModel.OpenWindow_Edit(item);
            }
        }

        private void Click_DeleteGroup(object sender, RoutedEventArgs e)
        {
            var selectedItem = GroupList.SelectedItem;
            if (selectedItem is ConnectionGroupModel group)
            {
                mainWindowViewModel.DeleteGroup(group);
            }
        }

        private void Click_Join(object sender, RoutedEventArgs e)
        {
            var selectedItem = ConnectionList.SelectedItem;
            if (selectedItem is ConnectionModel item && item.ConnectionID != null)
            {
                mainWindowViewModel.JoinStation(item);
                Debug.WriteLine("Clicked on connect: " + e.Source.ToString());
            }
        }

        private void Click_ImportGroup(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel.ImportGroup();
        }

        private void Click_ExportGroup(object sender, RoutedEventArgs e)
        {
            // opens the folder in explorer
            Process.Start("explorer.exe", GroupManager.GROUP_PATH);
        }

        private void Click_Refresh(object sender, RoutedEventArgs e)
        {
            if (mainWindowViewModel != null)
                mainWindowViewModel.RefreshGroupList();
        }

        private void HyperLink_Luffiez(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
    }
}
