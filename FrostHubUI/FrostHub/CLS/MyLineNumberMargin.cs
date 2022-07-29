// Decompiled with JetBrains decompiler
// Type: Syde.CLS.MyLineNumberMargin
// Assembly: FrostHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B53F501E-4295-47D8-80B6-D5DA533B6790
// Assembly location: C:\Program Files\Synapse\FrostHub.exe

using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Rendering;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Syde.CLS
{
  internal class MyLineNumberMargin : LineNumberMargin
  {
    public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(nameof (Background), typeof (SolidColorBrush), typeof (MyLineNumberMargin));

    public MyLineNumberMargin()
    {
      this.SetResourceReference(MyLineNumberMargin.BackgroundProperty, (object) "Std.Background2");
      this.NotifyOnResourcesChanged((EventHandler) ((s, e) => this.InvalidateVisual()));
    }

    [Obsolete]
    protected new virtual void OnRender(DrawingContext dc)
    {
      TextView textView = this.TextView;
      Size renderSize = this.RenderSize;
      if (textView == null || !textView.VisualLinesValid)
        return;
      dc.DrawRectangle((Brush) this.Background, new Pen((Brush) Brushes.Black, 0.0), new Rect(new Point(-1.0, -1.0), new Size(this.RenderSize.Width + 4.0, this.RenderSize.Height + 2.0)));
      Brush foreground = (Brush) this.GetValue(Control.ForegroundProperty);
      foreach (VisualLine visualLine in textView.VisualLines)
      {
        FormattedText formattedText = new FormattedText(visualLine.FirstDocumentLine.LineNumber.ToString((IFormatProvider) CultureInfo.CurrentCulture), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, this.typeface, this.emSize, foreground, (NumberSubstitution) null, TextOptions.GetTextFormattingMode((DependencyObject) this));
        double lineVisualYposition = visualLine.GetTextLineVisualYPosition(visualLine.TextLines[0], VisualYPosition.TextTop);
        dc.DrawText(formattedText, new Point(renderSize.Width - formattedText.Width, lineVisualYposition - textView.VerticalOffset));
      }
    }

    public SolidColorBrush Background
    {
      get => (SolidColorBrush) this.GetValue(MyLineNumberMargin.BackgroundProperty);
      set => this.SetValue(MyLineNumberMargin.BackgroundProperty, (object) value);
    }
  }
}
