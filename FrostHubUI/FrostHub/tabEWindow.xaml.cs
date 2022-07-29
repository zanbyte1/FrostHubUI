// Decompiled with JetBrains decompiler
// Type: Syde.tabEWindow
// Assembly: FrostHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B53F501E-4295-47D8-80B6-D5DA533B6790
// Assembly location: C:\Program Files\Synapse\FrostHub.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace Syde
{
  public partial class tabEWindow : Window, IComponentConnector
  {
    public static tabEWindow Instance;
    private byte colorR = 0;
    private byte colorG = 0;
    private byte colorB = 0;
    public string colour = "";
    private TabControl tabs;
    private TabItem tab;
    internal DropShadowEffect tabDropShadowEff;
    internal Border Mask;
    private bool _contentLoaded;

    public tabEWindow(TabControl tabs)
    {
      this.InitializeComponent();
      this.tabs = tabs;
      this.Topmost = true;
      this.Deactivated += (EventHandler) ((s, e) => this.Hide());
    }

    private void RenameTab(object sender, RoutedEventArgs e)
    {
      this.Hide();
      TextBox header = this.tab.Header as TextBox;
      header.IsEnabled = true;
      header.Focus();
      header.SelectAll();
    }

    public void Show(TabItem tab)
    {
      string pattern = "(^#[0-9A-F]{6}$)|(^#[0-9A-F]{3}$)";
      if (Regex.Match(File.ReadAllText(MainWindow.direct + "/FrostBin/Shadowcolor.txt"), pattern, RegexOptions.IgnoreCase).Success)
      {
        this.colour = File.ReadAllText(MainWindow.direct + "/FrostBin/ShadowColor.txt");
        this.colorR = Convert.ToByte(this.colour.Substring(1, 2), 16);
        this.colorG = Convert.ToByte(this.colour.Substring(3, 2), 16);
        this.colorB = Convert.ToByte(this.colour.Substring(5, 2), 16);
        this.tabDropShadowEff.Color = Color.FromRgb(this.colorR, this.colorG, this.colorB);
      }
      if (File.ReadAllText(MainWindow.direct + "/FrostBin/Shadowcolor.txt") == "rainbow")
        ((Storyboard) this.TryFindResource((object) "RainbowDropShadowStoryboard")).Begin();
      this.tab = tab;
      this.Show();
    }

    private void CloseTab(object sender, RoutedEventArgs e)
    {
      this.Hide();
      this.tabs.Items.Remove((object) this.tab);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/FrostHub;component/tabewindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.tabDropShadowEff = (DropShadowEffect) target;
          break;
        case 2:
          this.Mask = (Border) target;
          break;
        case 3:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.RenameTab);
          break;
        case 4:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.CloseTab);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
