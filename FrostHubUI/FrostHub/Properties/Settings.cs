// Decompiled with JetBrains decompiler
// Type: Syde.Properties.Settings
// Assembly: FrostHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B53F501E-4295-47D8-80B6-D5DA533B6790
// Assembly location: C:\Program Files\Synapse\FrostHub.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Syde.Properties
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default
    {
      get
      {
        Settings defaultInstance = Settings.defaultInstance;
        return defaultInstance;
      }
    }
  }
}
