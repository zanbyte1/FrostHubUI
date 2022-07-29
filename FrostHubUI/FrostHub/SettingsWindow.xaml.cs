// Decompiled with JetBrains decompiler
// Type: Syde.SettingsWindow
// Assembly: FrostHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B53F501E-4295-47D8-80B6-D5DA533B6790
// Assembly location: C:\Program Files\Synapse\FrostHub.exe

using sxlib.Specialized;
using sxlib.Static;
using Syde.CLS;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Syde
{
  public partial class SettingsWindow : Window, IComponentConnector
  {
    public static SettingsWindow Instance;
    public static string data;
    private readonly Window window;
    private byte colorR = 0;
    private byte colorG = 0;
    private byte colorB = 0;
    public string colour = "";
    internal SettingsWindow SettingsWin;
    internal DropShadowEffect shadowEffSet;
    internal Border mainBord;
    internal Grid mainGrid;
    internal Image image;
    internal Button btnExit;
    internal Border scripthubBord;
    internal ListBox ScriptList;
    internal ListBoxItem Dark_Dex;
    internal ListBoxItem Remote_Spy;
    internal ListBoxItem Script_Dumper;
    internal Image ScriptImage;
    internal TextBlock ScriptDescription;
    internal Button ExecuteButton;
    internal Button settingsBtn;
    internal Border settingsBord;
    internal TextBlock rValue;
    internal TextBlock gValue;
    internal TextBlock bValue;
    internal Slider sliderR;
    internal Slider sliderG;
    internal Slider sliderB;
    internal TextBox colorTextBox;
    internal Button resetcolorBtn;
    internal CheckBox AutoAttachCheck;
    internal CheckBox AutoLaunchCheck;
    internal CheckBox InternalUICheck;
    internal CheckBox UnlockFPSCheck;
    internal Button SaveButton;
    internal CheckBox TopMostCheck;
    internal Button scripthubBtn;
    internal CheckBox SilentLaunchCheck;
    private bool _contentLoaded;

    public SettingsWindow(Window cW)
    {
      this.InitializeComponent();
      this.Title = Functions.RandomString(10);
      this.SettingsWin.Opacity = 0.0;
      this.window = cW;
      this.Topmost = true;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
      MainWindow.syn.SetOptions(new Data.Options()
      {
        AutoAttach = this.AutoAttachCheck.IsChecked.GetValueOrDefault(),
        AutoLaunch = this.AutoLaunchCheck.IsChecked.GetValueOrDefault(),
        UnlockFPS = this.UnlockFPSCheck.IsChecked.GetValueOrDefault(),
        InternalUI = this.InternalUICheck.IsChecked.GetValueOrDefault(),
        TopMost = this.TopMostCheck.IsChecked.GetValueOrDefault(),
        SilentLaunch = this.SilentLaunchCheck.IsChecked.GetValueOrDefault()
      });
      MainWindow.Instance.Topmost = this.TopMostCheck.IsChecked.GetValueOrDefault();
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      SettingsWindow.Instance = this;
      DoubleAnimation win = new DoubleAnimation(1.0, (Duration) TimeSpan.FromSeconds(1.0));
      this.SettingsWin.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) win);
      DispatcherTimer checkColor = new DispatcherTimer();
      checkColor.Tick += (EventHandler) ((injected, orno) =>
      {
        MainWindow.Instance.dropShadowEff.Color = Color.FromRgb(this.colorR, this.colorG, this.colorB);
        this.shadowEffSet.Color = Color.FromRgb(this.colorR, this.colorG, this.colorB);
        this.rValue.Text = this.colorR.ToString();
        this.gValue.Text = this.colorG.ToString();
        this.bValue.Text = this.colorB.ToString();
      });
      checkColor.Interval = TimeSpan.FromSeconds(0.01);
      checkColor.Start();
      if (File.Exists(MainWindow.direct + "/FrostBin/ShadowColor.txt"))
      {
        if (File.ReadAllText(MainWindow.direct + "/FrostBin/Shadowcolor.txt").Length != 0)
          ;
        if (File.ReadAllText(MainWindow.direct + "/FrostBin/Shadowcolor.txt") == "rainbow")
        {
          ((Storyboard) this.TryFindResource((object) "RainbowDropShadowStoryboard")).Begin();
          ((Storyboard) MainWindow.Instance.TryFindResource((object) "RainbowDropShadowStoryboard")).Begin();
          this.colorTextBox.Text = "rainbow";
        }
        if (File.ReadAllText(MainWindow.direct + "/FrostBin/Shadowcolor.txt").Contains("#"))
        {
          string regex = "(^#[0-9A-F]{6}$)|(^#[0-9A-F]{3}$)";
          Match match = Regex.Match(File.ReadAllText(MainWindow.direct + "/FrostBin/Shadowcolor.txt"), regex, RegexOptions.IgnoreCase);
          if (match.Success)
          {
            this.colour = File.ReadAllText(MainWindow.direct + "/FrostBin/ShadowColor.txt");
            this.colorR = Convert.ToByte(this.colour.Substring(1, 2), 16);
            this.colorG = Convert.ToByte(this.colour.Substring(3, 2), 16);
            this.colorB = Convert.ToByte(this.colour.Substring(5, 2), 16);
            this.sliderR.Value = (double) Convert.ToByte(this.colour.Substring(1, 2), 16);
            this.sliderG.Value = (double) Convert.ToByte(this.colour.Substring(2, 2), 16);
            this.sliderB.Value = (double) Convert.ToByte(this.colour.Substring(4, 2), 16);
            this.shadowEffSet.Color = Color.FromRgb(this.colorR, this.colorG, this.colorB);
          }
          regex = (string) null;
          match = (Match) null;
        }
      }
      SxLibWPF sxLib = MainWindow.syn;
      this.SilentLaunchCheck.IsChecked = new bool?(sxLib.GetOptions().SilentLaunch);
      this.TopMostCheck.IsChecked = new bool?(sxLib.GetOptions().TopMost);
      this.AutoAttachCheck.IsChecked = new bool?(sxLib.GetOptions().AutoAttach);
      this.AutoLaunchCheck.IsChecked = new bool?(sxLib.GetOptions().AutoLaunch);
      this.InternalUICheck.IsChecked = new bool?(sxLib.GetOptions().InternalUI);
      this.UnlockFPSCheck.IsChecked = new bool?(sxLib.GetOptions().UnlockFPS);
      MainWindow.Instance.Topmost = sxLib.GetOptions().TopMost;
      this.window.LocationChanged += new EventHandler(this.Window_LocChanged);
      this.IsFocused.Equals((object) this.window.IsFocused.ToString());
      bool ready = false;
      while (!ready)
      {
        await Task.Delay(1);
        if (this.Opacity != 1.0)
          this.Opacity += 0.05;
        else
          ready = true;
      }
      win = (DoubleAnimation) null;
      sxLib = (SxLibWPF) null;
      MainWindow.SettingsOpened = true;
      win = (DoubleAnimation) null;
      checkColor = (DispatcherTimer) null;
      sxLib = (SxLibWPF) null;
    }

    private void Window_LocChanged(object sender, EventArgs e)
    {
      this.Left = this.window.Left + this.window.Width;
      this.Top = this.window.Top;
    }

    private async void btnExit_Click(object sender, RoutedEventArgs e)
    {
      DoubleAnimation animation = new DoubleAnimation(0.0, (Duration) TimeSpan.FromSeconds(0.5));
      this.SettingsWin.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) animation);
      await Task.Delay(500);
      MainWindow.SettingsOpened = false;
      if (File.Exists(MainWindow.direct + "/FrostBin/ShadowColor.txt"))
      {
        string regex = "(^#[0-9A-F]{6}$)|(^#[0-9A-F]{3}$)";
        Match match = Regex.Match(this.colorTextBox.Text, regex, RegexOptions.IgnoreCase);
        if (this.colorTextBox.Text == "rainbow")
          File.WriteAllText(MainWindow.direct + "/FrostBin/ShadowColor.txt", "rainbow");
        if (match.Success)
          File.WriteAllText(MainWindow.direct + "/FrostBin/ShadowColor.txt", "#" + this.colour);
        regex = (string) null;
        match = (Match) null;
      }
      this.Close();
      this.Topmost = false;
      animation = (DoubleAnimation) null;
    }

    private void resetcolorBtn_Click(object sender, RoutedEventArgs e)
    {
      ((Storyboard) this.TryFindResource((object) "RainbowDropShadowStoryboard")).Stop();
      ((Storyboard) MainWindow.Instance.TryFindResource((object) "RainbowDropShadowStoryboard")).Stop();
      this.colorR = (byte) 185;
      this.colorG = (byte) 173;
      this.colorB = byte.MaxValue;
      this.sliderR.Value = 185.0;
      this.sliderG.Value = 173.0;
      this.sliderB.Value = (double) byte.MaxValue;
      this.colour = this.colorR.ToString("X2") + this.colorG.ToString("X2") + this.colorB.ToString("X2");
      this.colorTextBox.Text = "#" + this.colour;
      MainWindow.Instance.dropShadowEff.Color = Color.FromRgb(this.colorR, this.colorG, this.colorB);
      this.shadowEffSet.Color = Color.FromRgb(this.colorR, this.colorG, this.colorB);
      if (!File.Exists(MainWindow.direct + "/FrostBin/ShadowColor.txt"))
        return;
      Match match = Regex.Match(this.colorTextBox.Text, "(^#[0-9A-F]{6}$)|(^#[0-9A-F]{3}$)", RegexOptions.IgnoreCase);
      if (this.colorTextBox.Text == "rainbow")
        File.WriteAllText(MainWindow.direct + "/FrostBin/ShadowColor.txt", "rainbow");
      if (match.Success)
        File.WriteAllText(MainWindow.direct + "/FrostBin/ShadowColor.txt", "#" + this.colour);
    }

    private void sliderR_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      ((Storyboard) this.TryFindResource((object) "RainbowDropShadowStoryboard")).Stop();
      ((Storyboard) MainWindow.Instance.TryFindResource((object) "RainbowDropShadowStoryboard")).Stop();
      this.rValue.Text = this.colorR.ToString();
      this.colorR = (byte) this.sliderR.Value;
      this.colour = this.colorR.ToString("X2") + this.colorG.ToString("X2") + this.colorB.ToString("X2");
      if (this.colorTextBox == null)
        return;
      this.colorTextBox.Text = "#" + this.colour;
      if (File.Exists(MainWindow.direct + "/FrostBin/ShadowColor.txt"))
      {
        Match match = Regex.Match(this.colorTextBox.Text, "(^#[0-9A-F]{6}$)|(^#[0-9A-F]{3}$)", RegexOptions.IgnoreCase);
        if (this.colorTextBox.Text == "rainbow")
          File.WriteAllText(MainWindow.direct + "/FrostBin/ShadowColor.txt", "rainbow");
        if (match.Success)
          File.WriteAllText(MainWindow.direct + "/FrostBin/ShadowColor.txt", "#" + this.colour);
      }
      MainWindow.Instance.dropShadowEff.Color = Color.FromRgb(this.colorR, this.colorG, this.colorB);
      this.shadowEffSet.Color = Color.FromRgb(this.colorR, this.colorG, this.colorB);
    }

    private void sliderG_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      ((Storyboard) this.TryFindResource((object) "RainbowDropShadowStoryboard")).Stop();
      ((Storyboard) MainWindow.Instance.TryFindResource((object) "RainbowDropShadowStoryboard")).Stop();
      this.gValue.Text = this.colorG.ToString();
      this.colorG = (byte) this.sliderG.Value;
      this.colour = this.colorR.ToString("X2") + this.colorG.ToString("X2") + this.colorB.ToString("X2");
      if (this.colorTextBox == null)
        return;
      this.colorTextBox.Text = "#" + this.colour;
      if (File.Exists(MainWindow.direct + "/FrostBin/ShadowColor.txt"))
      {
        Match match = Regex.Match(this.colorTextBox.Text, "(^#[0-9A-F]{6}$)|(^#[0-9A-F]{3}$)", RegexOptions.IgnoreCase);
        if (this.colorTextBox.Text == "rainbow")
          File.WriteAllText(MainWindow.direct + "/FrostBin/ShadowColor.txt", "rainbow");
        if (match.Success)
          File.WriteAllText(MainWindow.direct + "/FrostBin/ShadowColor.txt", "#" + this.colour);
      }
      MainWindow.Instance.dropShadowEff.Color = Color.FromRgb(this.colorR, this.colorG, this.colorB);
      this.shadowEffSet.Color = Color.FromRgb(this.colorR, this.colorG, this.colorB);
    }

    private void sliderB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      ((Storyboard) this.TryFindResource((object) "RainbowDropShadowStoryboard")).Stop();
      ((Storyboard) MainWindow.Instance.TryFindResource((object) "RainbowDropShadowStoryboard")).Stop();
      this.bValue.Text = this.colorB.ToString();
      this.colorB = (byte) this.sliderB.Value;
      this.colour = this.colorR.ToString("X2") + this.colorG.ToString("X2") + this.colorB.ToString("X2");
      if (this.colorTextBox == null)
        return;
      this.colorTextBox.Text = "#" + this.colour;
      if (File.Exists(MainWindow.direct + "/FrostBin/ShadowColor.txt"))
      {
        Match match = Regex.Match(this.colorTextBox.Text, "(^#[0-9A-F]{6}$)|(^#[0-9A-F]{3}$)", RegexOptions.IgnoreCase);
        if (this.colorTextBox.Text == "rainbow")
          File.WriteAllText(MainWindow.direct + "/FrostBin/ShadowColor.txt", "rainbow");
        if (match.Success)
          File.WriteAllText(MainWindow.direct + "/FrostBin/ShadowColor.txt", "#" + this.colour);
      }
      MainWindow.Instance.dropShadowEff.Color = Color.FromRgb(this.colorR, this.colorG, this.colorB);
      this.shadowEffSet.Color = Color.FromRgb(this.colorR, this.colorG, this.colorB);
    }

    private void colorTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      Match match = Regex.Match(this.colorTextBox.Text, "(^#[0-9A-F]{6}$)|(^#[0-9A-F]{3}$)", RegexOptions.IgnoreCase);
      if (e.Key != Key.Return)
        return;
      if (this.colorTextBox.Text.Contains("rainbow"))
      {
        ((Storyboard) this.TryFindResource((object) "RainbowDropShadowStoryboard")).Begin();
        ((Storyboard) MainWindow.Instance.TryFindResource((object) "RainbowDropShadowStoryboard")).Begin();
        if (File.Exists(MainWindow.direct + "/FrostBin/ShadowColor.txt"))
          File.WriteAllText(MainWindow.direct + "/FrostBin/ShadowColor.txt", "rainbow");
      }
      if (match.Success)
      {
        ((Storyboard) this.TryFindResource((object) "RainbowDropShadowStoryboard")).Stop();
        ((Storyboard) MainWindow.Instance.TryFindResource((object) "RainbowDropShadowStoryboard")).Stop();
        this.colour = this.colorTextBox.Text;
        this.colorR = Convert.ToByte(this.colour.Substring(1, 2), 16);
        this.colorG = Convert.ToByte(this.colour.Substring(3, 2), 16);
        this.colorB = Convert.ToByte(this.colour.Substring(5, 2), 16);
        this.sliderR.Value = (double) Convert.ToByte(this.colour.Substring(1, 2), 16);
        this.sliderG.Value = (double) Convert.ToByte(this.colour.Substring(2, 2), 16);
        this.sliderB.Value = (double) Convert.ToByte(this.colour.Substring(4, 2), 16);
        if (File.Exists(MainWindow.direct + "/FrostBin/ShadowColor.txt"))
          File.WriteAllText(MainWindow.direct + "/FrostBin/ShadowColor.txt", "#" + this.colour);
        MainWindow.Instance.dropShadowEff.Color = Color.FromRgb(this.colorR, this.colorG, this.colorB);
        this.shadowEffSet.Color = Color.FromRgb(this.colorR, this.colorG, this.colorB);
      }
    }

    private void ScriptList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.ScriptList.SelectedItem == this.Dark_Dex)
      {
        SettingsWindow.data = "Dark Dex";
        this.ScriptDescription.Text = "A version of the popular Dex explorer with patches specifically for Synapse. Bypasses most anti - exploits aswell.";
        this.ScriptImage.Source = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/FrostHub;component/IMG/DarkDex.png"));
      }
      if (this.ScriptList.SelectedItem == this.Remote_Spy)
      {
        SettingsWindow.data = "Remote Spy";
        this.ScriptDescription.Text = "Allows you to view RemoteEvents and RemoteFunctions called by the game.";
        this.ScriptImage.Source = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/FrostHub;component/IMG/RemoteSpy.png"));
      }
      if (this.ScriptList.SelectedItem != this.Script_Dumper)
        return;
      SettingsWindow.data = "Script Dumper";
      this.ScriptDescription.Text = "Dumps all LocalScripts and ModuleScripts in the game. Works even if they attempt to hide their scripts.";
      this.ScriptImage.Source = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/FrostHub;component/IMG/ScriptDumper.png"));
    }

    private async void scripthubBtn_Click(object sender, RoutedEventArgs e)
    {
      DoubleAnimation doubleAnimation1 = new DoubleAnimation();
      doubleAnimation1.To = new double?(0.0);
      doubleAnimation1.Duration = (Duration) TimeSpan.FromMilliseconds(500.0);
      doubleAnimation1.FillBehavior = FillBehavior.HoldEnd;
      DoubleAnimation animation = doubleAnimation1;
      this.settingsBord.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) animation);
      await Task.Delay(500);
      this.settingsBord.Visibility = Visibility.Collapsed;
      await Task.Delay(500);
      this.scripthubBord.Visibility = Visibility.Visible;
      DoubleAnimation doubleAnimation2 = new DoubleAnimation();
      doubleAnimation2.To = new double?(1.0);
      doubleAnimation2.Duration = (Duration) TimeSpan.FromMilliseconds(500.0);
      doubleAnimation2.FillBehavior = FillBehavior.HoldEnd;
      DoubleAnimation animation2 = doubleAnimation2;
      this.scripthubBord.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) animation2);
      animation = (DoubleAnimation) null;
      animation2 = (DoubleAnimation) null;
    }

    private async void settingsBtn_Click(object sender, RoutedEventArgs e)
    {
      DoubleAnimation doubleAnimation1 = new DoubleAnimation();
      doubleAnimation1.To = new double?(0.0);
      doubleAnimation1.Duration = (Duration) TimeSpan.FromMilliseconds(500.0);
      doubleAnimation1.FillBehavior = FillBehavior.HoldEnd;
      DoubleAnimation animation2 = doubleAnimation1;
      this.scripthubBord.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) animation2);
      await Task.Delay(500);
      this.scripthubBord.Visibility = Visibility.Collapsed;
      await Task.Delay(500);
      this.settingsBord.Visibility = Visibility.Visible;
      DoubleAnimation doubleAnimation2 = new DoubleAnimation();
      doubleAnimation2.To = new double?(1.0);
      doubleAnimation2.Duration = (Duration) TimeSpan.FromMilliseconds(500.0);
      doubleAnimation2.FillBehavior = FillBehavior.HoldEnd;
      DoubleAnimation animation = doubleAnimation2;
      this.settingsBord.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) animation);
      animation2 = (DoubleAnimation) null;
      animation = (DoubleAnimation) null;
    }

    private void ExecuteButton_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        MainWindow.syn.ScriptHub();
        MainWindow.syn.ScriptHubMarkAsClosed();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error : " + ex.Message, "FrostHub", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/FrostHub;component/settingswindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.SettingsWin = (SettingsWindow) target;
          this.SettingsWin.Loaded += new RoutedEventHandler(this.Window_Loaded);
          break;
        case 2:
          this.shadowEffSet = (DropShadowEffect) target;
          break;
        case 3:
          this.mainBord = (Border) target;
          break;
        case 4:
          this.mainGrid = (Grid) target;
          break;
        case 5:
          this.image = (Image) target;
          break;
        case 6:
          this.btnExit = (Button) target;
          this.btnExit.Click += new RoutedEventHandler(this.btnExit_Click);
          break;
        case 7:
          this.scripthubBord = (Border) target;
          break;
        case 8:
          this.ScriptList = (ListBox) target;
          this.ScriptList.SelectionChanged += new SelectionChangedEventHandler(this.ScriptList_SelectionChanged);
          break;
        case 9:
          this.Dark_Dex = (ListBoxItem) target;
          break;
        case 10:
          this.Remote_Spy = (ListBoxItem) target;
          break;
        case 11:
          this.Script_Dumper = (ListBoxItem) target;
          break;
        case 12:
          this.ScriptImage = (Image) target;
          break;
        case 13:
          this.ScriptDescription = (TextBlock) target;
          break;
        case 14:
          this.ExecuteButton = (Button) target;
          this.ExecuteButton.Click += new RoutedEventHandler(this.ExecuteButton_Click);
          break;
        case 15:
          this.settingsBtn = (Button) target;
          this.settingsBtn.Click += new RoutedEventHandler(this.settingsBtn_Click);
          break;
        case 16:
          this.settingsBord = (Border) target;
          break;
        case 17:
          this.rValue = (TextBlock) target;
          break;
        case 18:
          this.gValue = (TextBlock) target;
          break;
        case 19:
          this.bValue = (TextBlock) target;
          break;
        case 20:
          this.sliderR = (Slider) target;
          this.sliderR.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.sliderR_ValueChanged);
          break;
        case 21:
          this.sliderG = (Slider) target;
          this.sliderG.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.sliderG_ValueChanged);
          break;
        case 22:
          this.sliderB = (Slider) target;
          this.sliderB.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.sliderB_ValueChanged);
          break;
        case 23:
          this.colorTextBox = (TextBox) target;
          this.colorTextBox.KeyDown += new KeyEventHandler(this.colorTextBox_KeyDown);
          break;
        case 24:
          this.resetcolorBtn = (Button) target;
          this.resetcolorBtn.Click += new RoutedEventHandler(this.resetcolorBtn_Click);
          break;
        case 25:
          this.AutoAttachCheck = (CheckBox) target;
          break;
        case 26:
          this.AutoLaunchCheck = (CheckBox) target;
          break;
        case 27:
          this.InternalUICheck = (CheckBox) target;
          break;
        case 28:
          this.UnlockFPSCheck = (CheckBox) target;
          break;
        case 29:
          this.SaveButton = (Button) target;
          this.SaveButton.Click += new RoutedEventHandler(this.SaveButton_Click);
          break;
        case 30:
          this.TopMostCheck = (CheckBox) target;
          break;
        case 31:
          this.scripthubBtn = (Button) target;
          this.scripthubBtn.Click += new RoutedEventHandler(this.scripthubBtn_Click);
          break;
        case 32:
          this.SilentLaunchCheck = (CheckBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
