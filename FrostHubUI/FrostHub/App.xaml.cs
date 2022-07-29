// Decompiled with JetBrains decompiler
// Type: Syde.App
// Assembly: FrostHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B53F501E-4295-47D8-80B6-D5DA533B6790
// Assembly location: C:\Program Files\Synapse\FrostHub.exe

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;

namespace Syde
{
  public partial class App : Application
  {
    private bool _contentLoaded;

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
      Application.LoadComponent((object) this, new Uri("/FrostHub;component/app.xaml", UriKind.Relative));
    }

    [STAThread]
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public static void Main()
    {
      App app = new App();
      app.InitializeComponent();
      app.Run();
    }
  }
}
