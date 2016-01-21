﻿using CozyLauncher.Core.Plugin;
using CozyLauncher.PluginBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using CozyLauncher.Utils;
using System.Windows.Interop;

namespace CozyLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            InitHoeKey();
        }

        private enum HotKeyId : int
        {
            OPEN_HOTKEY_ID = 1000,
        }

        private void InitHoeKey()
        {
            IntPtr handle = new WindowInteropHelper(this).Handle;
            var hWndSource = HwndSource.FromHwnd(handle);
            hWndSource.AddHook(MainWindowProc);

            HotkeyUtil.RegisterHotKey(handle, (uint)HotKeyId.OPEN_HOTKEY_ID, (uint)KeyModifiers.Ctrl, (uint)VirtualKey.K_Q);
        }

        public const int WM_HOTKEY = 0x312;
        private IntPtr MainWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_HOTKEY:
                    int sid = wParam.ToInt32();
                    if (sid == (int)HotKeyId.OPEN_HOTKEY_ID)
                    {
                        ShowApp();
                    }
                    handled = true;
                    break;
            }

            return IntPtr.Zero;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            IntPtr handle = new WindowInteropHelper(this).Handle;
            HotkeyUtil.UnregisterHotKey(handle, (int)HotKeyId.OPEN_HOTKEY_ID);
        }

        private void OnWindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }

        private void OnQueryKeyUp(object sender, KeyEventArgs e)
        {
            var oldHandle = e.Handled;
            e.Handled = true;
            Key key = (e.Key == Key.System ? e.SystemKey : e.Key);

            switch (key)
            {
                case Key.Up:
                    this.ViewModel.UpCommand.Execute(null);
                    break;
                case Key.Down:
                    this.ViewModel.DownCommand.Execute(null);
                    break;
                case Key.Enter:
                    this.ViewModel.DoCommand.Execute(null);
                    break;
                case Key.Escape:
                    this.ViewModel.CloseApp();
                    break;
                default:
                    e.Handled = oldHandle;
                    break;
            }
        }

        private void OnQueryTextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox != null)
            {
                var text = textbox.Text.Trim();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    this.ViewModel.QueryCommand.Execute(text);
                }
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "HideApp")
            {
                HideApp();
            }
            else if (e.PropertyName == "CloseApp")
            {
                CloseApp();
            }
            else if (e.PropertyName == "ShowApp")
            {
                ShowApp();
            }
        }

        private void HideWox()
        {
            Hide();
        }

        private void ShowWox()
        {
            Show();
        }

        public void CloseApp()
        {
            Application.Current.Shutdown();
        }

        public void HideApp()
        {
            Dispatcher.Invoke(HideWox);
        }

        public void ShowApp()
        {
            Dispatcher.Invoke(ShowWox);
        }
    }
}
