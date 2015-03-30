﻿using HDLibrary.Wpf.Input;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Input;
using System.Windows.Interop;
using System.Xml;

namespace ReadResult
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private const int SW_SHOWMAXIMIZED = 3;

        [DllImport("user32.dll")]
        static extern bool ShowWindow(int hWnd, int nCmdShow);

    
        public MainWindow()
        {
            InitializeComponent();
            SetWorkingFolder selectFolderForm = new SetWorkingFolder();
            selectFolderForm.ShowDialog();
            this.Loaded += MainWindow_Loaded;

        }

        public XmlNode GetNode(XmlNodeList nodeList,string name)
        {
            foreach(XmlNode node in nodeList)
            {
                if (node.Name == name)
                    return node;
            }
            throw new Exception("Cannot find");
        }


        public static AutomationElement GetWindowByName(string name)
        {
            AutomationElement root = AutomationElement.RootElement;
            var collection = root.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window));
            

            foreach (AutomationElement window in collection)
            {
                Debug.WriteLine(window.Current.Name + window.Current.ClassName);
                if (window.Current.Name.Contains(name))
                {
                    return window;
                }
            }
            return null;
        }


       

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            HotKeyHost hotKeyHost = new HotKeyHost((HwndSource)HwndSource.FromVisual(App.Current.MainWindow));
            hotKeyHost.AddHotKey(new CustomHotKey(this,"CreateNew", Key.N, ModifierKeys.Control, true));
            hotKeyHost.AddHotKey(new CustomHotKey(this,"StartAcq", Key.F5, ModifierKeys.None, true));

    
         
//            AutomationElement startButton = automationElement.FindFirst(TreeScope.Children, new
//PropertyCondition(AutomationElement.NameProperty, "Start"));
//            InvokePattern ipStartButton = (InvokePattern)startButton.GetCurrentPattern(InvokePattern.Pattern);
//            ipStartButton.Invoke();
             
            //SystemWindow toolsBar = tecanWindow.AllChildWindows.Where(x => x.Title.Contains("toolStrip")).First();
            //SystemWindow statusBar = toolsBar.AllChildWindows[0].AllChildWindows[2];
            
            //Debug.WriteLine(statusBar.Title);


            // XmlDocument doc = new XmlDocument();      
            //try
            //{
            //    doc.Load(@"F:\MultiUserReader\result\result.xml");    //加载Xml文件  
            //    XmlElement rootElem = doc.DocumentElement;   //获取根节点  
            //    XmlNode sectionNode = GetNode(rootElem.ChildNodes, "Section");
            //    XmlNode dataNode = GetNode(sectionNode.ChildNodes, "Data");
            //}
            //catch(Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //}
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AutomationElement iControl = GetWindowByName("control");
                 // Sample usage
                ShowWindow(iControl.Current.NativeWindowHandle, SW_SHOWMAXIMIZED);
                //Thread.Sleep(500);
                AutomationElement startBtn = iControl.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Start"));
            
                var click = startBtn.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
                click.Invoke();
                Debug.WriteLine("Finished");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        internal void OnStartAcquisition()
        {
            MessageBox.Show("OnStartAcquisition");
        }

        public void OnNewPlate()
        {
            QueryLabel queryForm = new QueryLabel();
            queryForm.ShowDialog();
        }
    }



    [Serializable]
    public class CustomHotKey : HotKey
    {

        MainWindow hostWindow = null;
        public CustomHotKey(MainWindow window, string name, Key key, ModifierKeys modifiers, bool enabled)
            : base(key, modifiers, enabled)
        {
            hostWindow = window;
            Name = name;
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged(name);
                }
            }
        }

        protected override void OnHotKeyPress()
        {
            //MessageBox.Show(string.Format("'{0}' has been pressed ({1})", Name, this));
            if(Key == System.Windows.Input.Key.N && Modifiers == ModifierKeys.Control)
            {
                //Ctrl + N
                hostWindow.OnNewPlate();
            }
            else if( Key == System.Windows.Input.Key.F5)
            {
                hostWindow.OnStartAcquisition();
            }
            base.OnHotKeyPress();
        }


        protected CustomHotKey(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            Name = info.GetString("Name");
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Name", Name);
        }
    }
}
