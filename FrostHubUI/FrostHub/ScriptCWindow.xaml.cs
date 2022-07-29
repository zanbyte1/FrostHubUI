// Decompiled with JetBrains decompiler
// Type: Syde.ScriptCWindow
// Assembly: FrostHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B53F501E-4295-47D8-80B6-D5DA533B6790
// Assembly location: C:\Program Files\Synapse\FrostHub.exe

using ICSharpCode.AvalonEdit;
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
  public partial class ScriptCWindow : Window, IComponentConnector
  {
    private byte colorR = 0;
    private byte colorG = 0;
    private byte colorB = 0;
    public string colour = "";
    private MainWindow mainWindow;
    public string filePath;
    public static ScriptCWindow Instance;
    internal DropShadowEffect scriptShadowEff;
    internal Border Mask;
    private bool _contentLoaded;

    public ScriptCWindow(MainWindow mainWindow)
    {
      this.InitializeComponent();
      this.mainWindow = mainWindow;
      this.Topmost = true;
      this.Deactivated += (EventHandler) ((s, f) => this.Hide());
    }

    public void Show(string filePath)
    {
      string pattern = "(^#[0-9A-F]{6}$)|(^#[0-9A-F]{3}$)";
      if (Regex.Match(File.ReadAllText(MainWindow.direct + "/FrostBin/Shadowcolor.txt"), pattern, RegexOptions.IgnoreCase).Success)
      {
        this.colour = File.ReadAllText(MainWindow.direct + "/FrostBin/ShadowColor.txt");
        this.colorR = Convert.ToByte(this.colour.Substring(1, 2), 16);
        this.colorG = Convert.ToByte(this.colour.Substring(3, 2), 16);
        this.colorB = Convert.ToByte(this.colour.Substring(5, 2), 16);
        this.scriptShadowEff.Color = Color.FromRgb(this.colorR, this.colorG, this.colorB);
      }
      if (File.ReadAllText(MainWindow.direct + "/FrostBin/Shadowcolor.txt") == "rainbow")
        ((Storyboard) this.TryFindResource((object) "RainbowDropShadowStoryboard")).Begin();
      this.filePath = filePath;
      this.Show();
    }

    private void Load(object sender, RoutedEventArgs e)
    {
      TextEditor textEditor = MainWindow.Instance.GetTextEditor();
      if (textEditor != null)
      {
        TextBox header = (MainWindow.Instance.GetTextEditor().Parent as TabItem).Header as TextBox;
        if (header.Text == "Tab " && string.IsNullOrEmpty(textEditor.Text))
        {
          header.Text = Path.GetFileNameWithoutExtension(this.filePath);
          textEditor.Text = File.ReadAllText(this.filePath);
        }
        else
          MainWindow.Instance.AddTab(Path.GetFileNameWithoutExtension(this.filePath), File.ReadAllText(this.filePath));
      }
      this.Hide();
    }

    private void Execute(object sender, RoutedEventArgs e) => this.Hide();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/FrostHub;component/scriptcwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.scriptShadowEff = (DropShadowEffect) target;
          break;
        case 2:
          this.Mask = (Border) target;
          break;
        case 3:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.Execute);
          break;
        case 4:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.Load);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
