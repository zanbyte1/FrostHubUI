// Decompiled with JetBrains decompiler
// Type: Syde._1TimeWindow
// Assembly: FrostHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B53F501E-4295-47D8-80B6-D5DA533B6790
// Assembly location: C:\Program Files\Synapse\FrostHub.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace Syde
{
  public class _1TimeWindow : Window, IComponentConnector
  {
    internal _1TimeWindow LoaderWin;
    internal DropShadowEffect ShadowEff;
    internal Border LoaderBord;
    internal Grid LoaderGrid;
    internal Ellipse Circle;
    internal Image image;
    internal TextBlock Status;
    internal TextBlock Status_Copy;
    private bool _contentLoaded;

    public _1TimeWindow()
    {
      this.InitializeComponent();
      this.LoaderBord.Opacity = 0.0;
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      DoubleAnimation animation = new DoubleAnimation(1.0, (Duration) TimeSpan.FromSeconds(1.0));
      this.LoaderBord.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) animation);
      await Task.Delay(1000);
      Directory.CreateDirectory("FrostBin");
      WebClient client = new WebClient();
      client.DownloadFile(client.DownloadString("https://pastebin.com/raw/fJVwur08"), "FrostLua.xshd");
      System.IO.File.Move(MainWindow.direct + "/FrostLua.xshd", MainWindow.direct + "/FrostBin/FrostLua.xshd");
      System.IO.File.Create(MainWindow.direct + "/FrostBin/ShadowColor.txt").Close();
      this.Status.Text = "Download Finish!";
      await Task.Delay(250);
      DoubleAnimation animation2 = new DoubleAnimation(0.0, (Duration) TimeSpan.FromSeconds(0.5));
      this.LoaderBord.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) animation2);
      await Task.Delay(500);
      new MainWindow().Show();
      this.Hide();
      animation = (DoubleAnimation) null;
      client = (WebClient) null;
      animation2 = (DoubleAnimation) null;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/FrostHub;component/1timewindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.LoaderWin = (_1TimeWindow) target;
          this.LoaderWin.Loaded += new RoutedEventHandler(this.Window_Loaded);
          break;
        case 2:
          this.ShadowEff = (DropShadowEffect) target;
          break;
        case 3:
          this.LoaderBord = (Border) target;
          break;
        case 4:
          this.LoaderGrid = (Grid) target;
          break;
        case 5:
          this.Circle = (Ellipse) target;
          break;
        case 6:
          this.image = (Image) target;
          break;
        case 7:
          this.Status = (TextBlock) target;
          break;
        case 8:
          this.Status_Copy = (TextBlock) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
