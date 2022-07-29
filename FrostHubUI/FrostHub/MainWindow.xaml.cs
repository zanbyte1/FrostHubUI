// Decompiled with JetBrains decompiler
// Type: Syde.MainWindow
// Assembly: FrostHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B53F501E-4295-47D8-80B6-D5DA533B6790
// Assembly location: C:\Program Files\Synapse\FrostHub.exe

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Microsoft.Win32;
using sxlib;
using sxlib.Specialized;
using Syde.CLS;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;

namespace Syde
{
  public partial class MainWindow : Window, IComponentConnector
  {
    public static SxLibWPF syn;
    public static IHighlightingDefinition HighlightingDefinition;
    public static MainWindow Instance;
    public static bool ScriptHubOpened = false;
    public static bool SettingsOpened = false;
    public static bool synReady = false;
    public static IHighlightingDefinition Highlighting;
    public static string direct = Directory.GetCurrentDirectory();
    private readonly tabEWindow tabEditWindow;
    private readonly ScriptCWindow scriptsContextMenu;
    private ScrollViewer tabScroller;
    private byte colorR = 0;
    private byte colorG = 0;
    private byte colorB = 0;
    private string colour = "";
    internal MainWindow sWindow;
    internal DropShadowEffect dropShadowEff;
    internal Border sBorder;
    internal Grid mainGrid;
    internal TextBlock Status;
    internal Button ExitBtn;
    internal Button MiniBtn;
    internal TabControl Editors;
    internal Border ScriptViewerOpacityMask;
    internal Border SVContainer;
    internal Button ExecuteBtn;
    internal Button ClearBtn;
    internal Button AttachBtn;
    internal Image InjectImage;
    internal Button loadBtn;
    internal Button saveBtn;
    internal Button settingsBtn;
    private bool _contentLoaded;

    public MainWindow()
    {
      this.InitializeComponent();
      this.sBorder.Opacity = 0.0;
      this.tabEditWindow = new tabEWindow(this.Editors);
      this.scriptsContextMenu = new ScriptCWindow(this);
      MainWindow.syn = SxLib.InitializeWPF((Window) this, AppDomain.CurrentDomain.BaseDirectory);
      this.Topmost = true;
      this.Title = Functions.RandomString(10);
      Application.Current.MainWindow = (Window) this;
    }

    private void sWindow_Loaded(object sender, RoutedEventArgs e)
    {
      if (!Directory.Exists(MainWindow.direct + "/FrostBin"))
      {
        _1TimeWindow obj = new _1TimeWindow();
        this.Hide();
        obj.Show();
      }
      else
      {
        DoubleAnimation doubleAnimation = new DoubleAnimation();
        doubleAnimation.To = new double?(1.0);
        doubleAnimation.Duration = (Duration) TimeSpan.FromMilliseconds(1500.0);
        doubleAnimation.FillBehavior = FillBehavior.HoldEnd;
        DoubleAnimation animation = doubleAnimation;
        this.sBorder.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) animation);
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        dispatcherTimer.Tick += (EventHandler) ((injected, orno) =>
        {
          if (MainWindow.syn.Ready())
          {
            this.InjectImage.Source = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/FrostHub;component/IMG/injected.png"));
            this.AttachBtn.ToolTip = (object) "Injected";
          }
          else
          {
            if (MainWindow.syn.Ready())
              return;
            this.InjectImage.Source = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/FrostHub;component/IMG/inject.png"));
            this.AttachBtn.ToolTip = (object) "Inject";
          }
          if (MainWindow.SettingsOpened)
            this.Topmost = true;
          if (MainWindow.SettingsOpened || !MainWindow.synReady)
            return;
          this.Topmost = MainWindow.syn.GetOptions().TopMost;
        });
        dispatcherTimer.Interval = TimeSpan.FromSeconds(0.5);
        dispatcherTimer.Start();
        this.Editors.GetTemplateItem<Button>("AddTab").Click += (RoutedEventHandler) ((a, b) => this.AddTab());
        this.Topmost = false;
        if (File.ReadAllText(MainWindow.direct + "/FrostBin/Shadowcolor.txt").Length != 0)
          ;
        if (File.ReadAllText(MainWindow.direct + "/FrostBin/Shadowcolor.txt") == "rainbow")
          ((Storyboard) this.TryFindResource((object) "RainbowDropShadowStoryboard")).Begin();
        if (File.ReadAllText(MainWindow.direct + "/FrostBin/Shadowcolor.txt").Contains("#"))
        {
          string pattern = "(^#[0-9A-F]{6}$)|(^#[0-9A-F]{3}$)";
          if (Regex.Match(File.ReadAllText(MainWindow.direct + "/FrostBin/Shadowcolor.txt"), pattern, RegexOptions.IgnoreCase).Success)
          {
            this.colour = File.ReadAllText(MainWindow.direct + "/FrostBin/ShadowColor.txt");
            this.colorR = Convert.ToByte(this.colour.Substring(1, 2), 16);
            this.colorG = Convert.ToByte(this.colour.Substring(3, 2), 16);
            this.colorB = Convert.ToByte(this.colour.Substring(5, 2), 16);
            this.dropShadowEff.Color = System.Windows.Media.Color.FromRgb(this.colorR, this.colorG, this.colorB);
          }
        }
        this.tabScroller = this.Editors.GetTemplateChild<ScrollViewer>("TabScrollViewer");
        MainWindow.syn.LoadEvent += new SxLibWPF.SynLoadDelegate(this.Syn_LoadEvent);
        MainWindow.syn.AttachEvent += new SxLibWPF.SynAttachDelegate(this.SynapseAttachEvent);
        MainWindow.syn.ScriptHubEvent += new SxLibWPF.SynScriptHubDelegate(this.SxLibScriptHubEvent);
        MainWindow.syn.Load();
        MainWindow.syn.SetWindow((Window) this);
        MainWindow.Instance = this;
        this.ListRefresh();
        this.AddTab(text: "-- You can customize the lua highlighting \n-- at /FrostBin/FrostLua if you don't like it\n-- Type rainbow on the textbox above reset button on settings for rainbow");
      }
    }

    private void SxLibScriptHubEvent(List<SxLibBase.SynHubEntry> Entries)
    {
      foreach (SxLibBase.SynHubEntry entry in Entries)
      {
        if (SettingsWindow.data == entry.Name)
          entry.Execute();
      }
    }

    private async void Syn_LoadEvent(SxLibBase.SynLoadEvents e, object Param)
    {
      SxLibBase.SynLoadEvents synLoadEvents = e;
      switch (synLoadEvents)
      {
        case SxLibBase.SynLoadEvents.UNKNOWN:
          this.Status.Foreground = (Brush) Brushes.Red;
          this.Status.Text = "Unknown Error!";
          await Task.Delay(1500);
          this.Status.Foreground = (Brush) Brushes.White;
          this.Status.Text = "";
          break;
        case SxLibBase.SynLoadEvents.NOT_LOGGED_IN:
          this.Status.Foreground = (Brush) Brushes.Red;
          this.Status.Text = "Not logged in!";
          await Task.Delay(1500);
          this.Status.Foreground = (Brush) Brushes.White;
          this.Status.Text = "";
          break;
        case SxLibBase.SynLoadEvents.NOT_UPDATED:
          this.Status.Foreground = (Brush) Brushes.Red;
          this.Status.Text = "Not Updated!";
          await Task.Delay(1500);
          this.Status.Foreground = (Brush) Brushes.White;
          this.Status.Text = "";
          break;
        case SxLibBase.SynLoadEvents.FAILED_TO_VERIFY:
          this.Status.Foreground = (Brush) Brushes.Red;
          this.Status.Text = "Failed to verify!";
          await Task.Delay(1500);
          this.Status.Foreground = (Brush) Brushes.White;
          this.Status.Text = "";
          break;
        case SxLibBase.SynLoadEvents.FAILED_TO_DOWNLOAD:
          this.Status.Foreground = (Brush) Brushes.Red;
          this.Status.Text = "Failed to Download!";
          await Task.Delay(1500);
          this.Status.Foreground = (Brush) Brushes.White;
          this.Status.Text = "";
          break;
        case SxLibBase.SynLoadEvents.UNAUTHORIZED_HWID:
          this.Status.Foreground = (Brush) Brushes.Red;
          this.Status.Text = "Unauthorized HWID!";
          await Task.Delay(1500);
          this.Status.Foreground = (Brush) Brushes.White;
          this.Status.Text = "";
          break;
        case SxLibBase.SynLoadEvents.ALREADY_EXISTING_WL:
          this.Status.Foreground = (Brush) Brushes.Red;
          this.Status.Text = "Already exising WL!";
          await Task.Delay(1500);
          this.Status.Foreground = (Brush) Brushes.White;
          this.Status.Text = "";
          break;
        case SxLibBase.SynLoadEvents.NOT_ENOUGH_TIME:
          this.Status.Foreground = (Brush) Brushes.Red;
          this.Status.Text = "Not enough time!!";
          await Task.Delay(1500);
          this.Status.Foreground = (Brush) Brushes.White;
          this.Status.Text = "";
          break;
        case SxLibBase.SynLoadEvents.CHECKING_WL:
          this.Status.Text = "Checking Whitelist";
          break;
        case SxLibBase.SynLoadEvents.DOWNLOADING_DATA:
          this.Status.Text = "Downloading Data";
          break;
        case SxLibBase.SynLoadEvents.CHECKING_DATA:
          this.Status.Text = "Checking Data";
          break;
        case SxLibBase.SynLoadEvents.DOWNLOADING_DLLS:
          this.Status.Text = "Downloading DLLs";
          await Task.Delay(1500);
          this.Status.Foreground = (Brush) Brushes.White;
          this.Status.Text = "";
          break;
        case SxLibBase.SynLoadEvents.READY:
          this.Status.Foreground = (Brush) Brushes.Green;
          this.Status.Text = "Ready!";
          MainWindow.synReady = true;
          this.Topmost = MainWindow.syn.GetOptions().TopMost;
          await Task.Delay(500);
          this.Status.Foreground = (Brush) Brushes.White;
          this.Status.Text = "Made By SydeFx";
          break;
      }
    }

    private async void SynapseAttachEvent(SxLibBase.SynAttachEvents evnt, object _)
    {
      SxLibBase.SynAttachEvents synAttachEvents = evnt;
      switch (synAttachEvents)
      {
        case SxLibBase.SynAttachEvents.CHECKING:
          this.Status.Text = "Checking";
          break;
        case SxLibBase.SynAttachEvents.INJECTING:
          this.Status.Text = "Attaching...";
          break;
        case SxLibBase.SynAttachEvents.CHECKING_WHITELIST:
          this.Status.Text = "Checking Whitelist";
          break;
        case SxLibBase.SynAttachEvents.SCANNING:
          this.Status.Text = "Scannig";
          break;
        case SxLibBase.SynAttachEvents.READY:
          this.Status.Foreground = (Brush) Brushes.Green;
          this.Status.Text = "Attached.";
          await Task.Delay(1500);
          this.Status.Foreground = (Brush) Brushes.White;
          this.Status.Text = "";
          break;
        case SxLibBase.SynAttachEvents.FAILED_TO_ATTACH:
          this.Status.Foreground = (Brush) Brushes.Red;
          this.Status.Text = "Failed to attach";
          await Task.Delay(1500);
          this.Status.Foreground = (Brush) Brushes.White;
          this.Status.Text = "";
          break;
        case SxLibBase.SynAttachEvents.FAILED_TO_FIND:
          this.Status.Foreground = (Brush) Brushes.Red;
          this.Status.Text = "Roblox not found";
          await Task.Delay(1500);
          this.Status.Foreground = (Brush) Brushes.White;
          this.Status.Text = "";
          break;
        case SxLibBase.SynAttachEvents.NOT_RUNNING_LATEST_VER_UPDATING:
          this.Status.Text = "Updating";
          break;
        case SxLibBase.SynAttachEvents.UPDATING_DLLS:
          this.Status.Text = "Updating Dlls";
          break;
        case SxLibBase.SynAttachEvents.NOT_UPDATED:
          this.Status.Foreground = (Brush) Brushes.Red;
          this.Status.Text = "Not updated.";
          await Task.Delay(1500);
          this.Status.Foreground = (Brush) Brushes.White;
          this.Status.Text = "";
          break;
        case SxLibBase.SynAttachEvents.FAILED_TO_UPDATE:
          this.Status.Foreground = (Brush) Brushes.Red;
          this.Status.Text = "Failed To Update";
          await Task.Delay(1500);
          this.Status.Foreground = (Brush) Brushes.White;
          this.Status.Text = "";
          break;
        case SxLibBase.SynAttachEvents.REINJECTING:
          this.Status.Text = "Re-attaching";
          break;
        case SxLibBase.SynAttachEvents.NOT_INJECTED:
          this.Status.Foreground = (Brush) Brushes.Red;
          this.Status.Text = "Not attached.";
          await Task.Delay(1500);
          this.Status.Foreground = (Brush) Brushes.White;
          this.Status.Text = "";
          break;
        case SxLibBase.SynAttachEvents.ALREADY_INJECTED:
          this.Status.Foreground = (Brush) Brushes.Red;
          this.Status.Text = "Already Attached!";
          await Task.Delay(1500);
          this.Status.Foreground = (Brush) Brushes.White;
          this.Status.Text = "";
          break;
        case SxLibBase.SynAttachEvents.PROC_CREATION:
          this.Status.Foreground = (Brush) Brushes.Red;
          this.Status.Text = "Roblox detected.";
          await Task.Delay(1500);
          this.Status.Foreground = (Brush) Brushes.White;
          this.Status.Text = "";
          break;
        case SxLibBase.SynAttachEvents.PROC_DELETION:
          this.Status.Foreground = (Brush) Brushes.Red;
          this.Status.Text = "Not attached.";
          await Task.Delay(1500);
          this.Status.Foreground = (Brush) Brushes.White;
          this.Status.Text = "";
          break;
        default:
          this.Status.Text = "Unknown";
          break;
      }
    }

    public static TextEditor MakeEditor()
    {
      FrostEditor frostEditor = new FrostEditor();
      SolidColorBrush solidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb((byte) 100, (byte) 30, (byte) 30, (byte) 30));
      frostEditor.Background = (Brush) solidColorBrush;
      frostEditor.Options.EnableEmailHyperlinks = false;
      frostEditor.Options.EnableHyperlinks = false;
      frostEditor.Options.AllowScrollBelowDocument = true;
      frostEditor.Options.ShowTabs = false;
      frostEditor.Options.ShowSpaces = false;
      frostEditor.Options.EnableHyperlinks = false;
      frostEditor.Options.HideCursorWhileTyping = true;
      frostEditor.Options.HighlightCurrentLine = false;
      frostEditor.ShowLineNumbers = true;
      frostEditor.WordWrap = false;
      frostEditor.Cursor = Cursors.IBeam;
      frostEditor.Foreground = (Brush) Brushes.White;
      frostEditor.LineNumbersForeground = (Brush) Brushes.Gray;
      Line line1 = new Line();
      line1.X1 = 0.0;
      line1.Y1 = 0.0;
      line1.X2 = 0.0;
      line1.Y2 = 1.0;
      line1.Stretch = Stretch.Fill;
      line1.StrokeThickness = 1.0;
      line1.Margin = new Thickness(2.0, 0.0, 6.0, 0.0);
      line1.Tag = new object();
      Line line2 = line1;
      line2.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding("LineNumbersForeground")
      {
        Source = (object) frostEditor
      });
      frostEditor.TextArea.LeftMargins.RemoveAt(1);
      frostEditor.TextArea.LeftMargins.Insert(1, (UIElement) line2);
      frostEditor.SyntaxHighlighting = HighlightingLoader.Load((XmlReader) new XmlTextReader((Stream) File.OpenRead("./FrostBin/FrostLua.xshd")), (IHighlightingDefinitionReferenceResolver) HighlightingManager.Instance);
      return (TextEditor) frostEditor;
    }

    public TabItem AddTab(string title = "Tab ", string text = "")
    {
      bool loaded = false;
      TextEditor textEditor = MainWindow.MakeEditor();
      textEditor.Text = text;
      TextBox textBox = new TextBox();
      textBox.Text = title;
      textBox.IsEnabled = false;
      textBox.TextWrapping = TextWrapping.NoWrap;
      textBox.IsHitTestVisible = false;
      textBox.Style = this.TryFindResource((object) "InvisibleTextBox") as Style;
      TextBox tbox = textBox;
      TabItem tabItem = new TabItem();
      tabItem.Content = (object) textEditor;
      tabItem.Style = this.TryFindResource((object) "Tab") as Style;
      tabItem.Header = (object) tbox;
      tabItem.AllowDrop = true;
      TabItem tab = tabItem;
      tab.MouseWheel += new MouseWheelEventHandler(this.ScrollTabs);
      tab.Loaded += (RoutedEventHandler) ((s, f) =>
      {
        if (loaded)
          return;
        tab.GetTemplateChild<Button>("CloseButton").Click += (RoutedEventHandler) ((d, x) => this.Editors.Items.Remove((object) tab));
        this.tabScroller.ScrollToRightEnd();
        loaded = true;
      });
      tab.MouseDown += (MouseButtonEventHandler) ((sender, e) =>
      {
        if (!(e.OriginalSource is Border))
          return;
        if (e.MiddleButton == MouseButtonState.Pressed)
          this.Editors.Items.Remove((object) tab);
        else if (e.RightButton == MouseButtonState.Pressed)
        {
          tabEWindow tabEditWindow1 = this.tabEditWindow;
          Point position = e.GetPosition((IInputElement) null);
          double num1 = position.X - 12.0 + this.Left;
          tabEditWindow1.Left = num1;
          tabEWindow tabEditWindow2 = this.tabEditWindow;
          position = e.GetPosition((IInputElement) null);
          double num2 = position.Y - 12.0 + this.Top;
          tabEditWindow2.Top = num2;
          this.tabEditWindow.Show(tab);
        }
      });
      tab.MouseMove += new MouseEventHandler(this.MoveTab);
      tab.Drop += new DragEventHandler(this.DropTab);
      string oldHeader = title;
      tbox.GotFocus += (RoutedEventHandler) ((s, f) =>
      {
        oldHeader = tbox.Text;
        tbox.CaretIndex = tbox.Text.Length - 1;
      });
      tbox.KeyDown += (KeyEventHandler) ((s, e) =>
      {
        switch (e.Key)
        {
          case Key.Return:
            tbox.IsEnabled = false;
            break;
          case Key.Escape:
            tbox.Text = oldHeader;
            goto case Key.Return;
        }
      });
      tbox.LostFocus += (RoutedEventHandler) ((s, f) => tbox.IsEnabled = false);
      this.Editors.SelectedIndex = this.Editors.Items.Add((object) tab);
      return tab;
    }

    public TextEditor GetTextEditor()
    {
      TextEditor textEditor = (TextEditor) null;
      if (this.Editors != null)
        textEditor = (TextEditor) this.Editors.SelectedContent;
      return textEditor;
    }

    public void ListRefresh()
    {
      StackPanel stackPanel = new StackPanel();
      foreach (string enumerateFile in Directory.EnumerateFiles("scripts\\"))
      {
        string f = enumerateFile;
        Button button = new Button();
        button.Content = (object) System.IO.Path.GetFileNameWithoutExtension(f);
        Button element = button;
        element.Click += (RoutedEventHandler) ((sender, e) =>
        {
          if (this.GetTextEditor() == null)
            return;
          this.GetTextEditor().Text = File.ReadAllText(f);
        });
        element.MouseRightButtonUp += (MouseButtonEventHandler) ((sender, e) =>
        {
          this.scriptsContextMenu.Left = e.GetPosition((IInputElement) null).X - 12.0 + this.Left;
          this.scriptsContextMenu.Top = e.GetPosition((IInputElement) null).Y - 12.0 + this.Top;
          this.scriptsContextMenu.Show(f);
        });
        stackPanel.Children.Add((UIElement) element);
      }
      this.SVContainer.Child = (UIElement) stackPanel;
    }

    private void ScrollTabs(object sender, MouseWheelEventArgs e) => this.tabScroller.ScrollToHorizontalOffset(this.tabScroller.HorizontalOffset + (double) (e.Delta / 10));

    private void MoveTab(object sender, MouseEventArgs e)
    {
      if (!(e.Source is TabItem source) || Mouse.PrimaryDevice.LeftButton != MouseButtonState.Pressed || VisualTreeHelper.HitTest((Visual) source, Mouse.GetPosition((IInputElement) source)).VisualHit is Button)
        return;
      int num = (int) DragDrop.DoDragDrop((DependencyObject) source, (object) source, DragDropEffects.Move);
    }

    private void DropTab(object sender, DragEventArgs e)
    {
      if (!(e.Source is TabItem source) || !(e.Data.GetData(typeof (TabItem)) is TabItem data) || source.Equals((object) data))
        return;
      TabControl parent = source.Parent as TabControl;
      int insertIndex1 = parent.Items.IndexOf((object) data);
      int insertIndex2 = parent.Items.IndexOf((object) source);
      parent.Items.Remove((object) data);
      parent.Items.Insert(insertIndex2, (object) data);
      parent.Items.Remove((object) source);
      parent.Items.Insert(insertIndex1, (object) source);
      parent.SelectedIndex = insertIndex2;
    }

    private async void ExitBtn_Click(object sender, RoutedEventArgs e)
    {
      DoubleAnimation animation2 = new DoubleAnimation(0.0, (Duration) TimeSpan.FromSeconds(0.3));
      this.sWindow.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) animation2);
      if (MainWindow.SettingsOpened)
        SettingsWindow.Instance.SettingsWin.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) animation2);
      await Task.Delay(1000);
      if (File.Exists(MainWindow.direct + "/FrostBin/ShadowColor.txt"))
      {
        if (SettingsWindow.Instance.colorTextBox.Text == "rainbow")
          File.WriteAllText(MainWindow.direct + "/FrostBin/ShadowColor.txt", "rainbow");
        else
          File.WriteAllText(MainWindow.direct + "/FrostBin/ShadowColor.txt", SettingsWindow.Instance.colour);
      }
      MainWindow.syn.Close();
      Process.GetCurrentProcess().Kill();
      animation2 = (DoubleAnimation) null;
    }

    private void MiniBtn_Click(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;

    private void ExecuteBtn_Click(object sender, RoutedEventArgs e)
    {
      if (this.GetTextEditor() == null)
        return;
      MainWindow.syn.Execute(this.GetTextEditor().Text);
    }

    private void ClearBtn_Click(object sender, RoutedEventArgs e)
    {
      if (this.GetTextEditor() == null)
        return;
      this.GetTextEditor().Clear();
    }

    private void AttachBtn_Click(object sender, RoutedEventArgs e)
    {
      if (!MainWindow.synReady.Equals(true))
        return;
      MainWindow.syn.Attach();
    }

    private void loadBtn_Click(object sender, RoutedEventArgs e)
    {
      TextEditor textEditor = this.GetTextEditor();
      if (textEditor == null)
        return;
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.CheckFileExists = true;
      openFileDialog1.Multiselect = false;
      openFileDialog1.DefaultExt = ".lua";
      openFileDialog1.Filter = "Scripts (*.lua; *.txt)|*.lua;*.txt|All Files (*.*)|*.*";
      OpenFileDialog openFileDialog2 = openFileDialog1;
      bool? nullable = openFileDialog2.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      textEditor.Text = File.ReadAllText(openFileDialog2.FileName);
      ((textEditor.Parent as TabItem).Header as TextBox).Text = System.IO.Path.GetFileNameWithoutExtension(openFileDialog2.FileName);
    }

    private void saveBtn_Click(object sender, RoutedEventArgs e)
    {
      TextEditor textEditor = this.GetTextEditor();
      if (textEditor == null)
        return;
      SaveFileDialog saveFileDialog1 = new SaveFileDialog();
      saveFileDialog1.DefaultExt = ".lua";
      saveFileDialog1.Filter = "Scripts (*.lua; *.txt)|*.lua;*.txt|All Files (*.*)|*.*";
      SaveFileDialog saveFileDialog2 = saveFileDialog1;
      bool? nullable = saveFileDialog2.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      File.WriteAllText(saveFileDialog2.FileName, textEditor.Text);
      TextBox header = (textEditor.Parent as TabItem).Header as TextBox;
      if (header.Text == "New Tab")
        header.Text = System.IO.Path.GetFileNameWithoutExtension(saveFileDialog2.FileName);
      this.ListRefresh();
    }

    private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton != MouseButton.Left)
        return;
      this.DragMove();
    }

    private void settingsBtn_Click(object sender, RoutedEventArgs e)
    {
      if (MainWindow.SettingsOpened || !MainWindow.synReady)
        return;
      ((Storyboard) this.TryFindResource((object) "RainbowDropShadowStoryboard")).Stop();
      SettingsWindow settingsWindow = new SettingsWindow((Window) this);
      settingsWindow.Show();
      settingsWindow.Left = this.Left + this.Width;
      settingsWindow.Top = this.Top;
      MainWindow.SettingsOpened = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/FrostHub;component/mainwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.sWindow = (MainWindow) target;
          this.sWindow.Loaded += new RoutedEventHandler(this.sWindow_Loaded);
          break;
        case 2:
          this.dropShadowEff = (DropShadowEffect) target;
          break;
        case 3:
          this.sBorder = (Border) target;
          break;
        case 4:
          this.mainGrid = (Grid) target;
          break;
        case 5:
          this.Status = (TextBlock) target;
          break;
        case 6:
          ((UIElement) target).MouseDown += new MouseButtonEventHandler(this.Border_MouseDown);
          break;
        case 7:
          this.ExitBtn = (Button) target;
          this.ExitBtn.Click += new RoutedEventHandler(this.ExitBtn_Click);
          break;
        case 8:
          this.MiniBtn = (Button) target;
          this.MiniBtn.Click += new RoutedEventHandler(this.MiniBtn_Click);
          break;
        case 9:
          this.Editors = (TabControl) target;
          break;
        case 10:
          this.ScriptViewerOpacityMask = (Border) target;
          break;
        case 11:
          this.SVContainer = (Border) target;
          break;
        case 12:
          this.ExecuteBtn = (Button) target;
          this.ExecuteBtn.Click += new RoutedEventHandler(this.ExecuteBtn_Click);
          break;
        case 13:
          this.ClearBtn = (Button) target;
          this.ClearBtn.Click += new RoutedEventHandler(this.ClearBtn_Click);
          break;
        case 14:
          this.AttachBtn = (Button) target;
          this.AttachBtn.Click += new RoutedEventHandler(this.AttachBtn_Click);
          break;
        case 15:
          this.InjectImage = (Image) target;
          break;
        case 16:
          this.loadBtn = (Button) target;
          this.loadBtn.Click += new RoutedEventHandler(this.loadBtn_Click);
          break;
        case 17:
          this.saveBtn = (Button) target;
          this.saveBtn.Click += new RoutedEventHandler(this.saveBtn_Click);
          break;
        case 18:
          this.settingsBtn = (Button) target;
          this.settingsBtn.Click += new RoutedEventHandler(this.settingsBtn_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
