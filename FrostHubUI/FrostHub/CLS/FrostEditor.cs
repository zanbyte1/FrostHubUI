// Decompiled with JetBrains decompiler
// Type: Syde.CLS.FrostEditor
// Assembly: FrostHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B53F501E-4295-47D8-80B6-D5DA533B6790
// Assembly location: C:\Program Files\Synapse\FrostHub.exe

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Syde.CLS
{
  public class FrostEditor : TextEditor
  {
    public FrostEditor()
      : this("")
    {
    }

    public FrostEditor(string text = "")
    {
      this.SnapsToDevicePixels = true;
      this.ShowLineNumbers = true;
      this.BorderThickness = new Thickness(0.0);
      this.SyntaxHighlighting = MainWindow.HighlightingDefinition;
      this.Text = text;
      this.Options = new TextEditorOptions()
      {
        ConvertTabsToSpaces = false,
        IndentationSize = 2,
        EnableEmailHyperlinks = false,
        EnableHyperlinks = false
      };
      this.SetResourceReference(Control.BackgroundProperty, (object) "Std.Background2");
      this.SetResourceReference(Control.ForegroundProperty, (object) "Std.Foreground");
      this.SetResourceReference(Control.BorderBrushProperty, (object) "Std.Border2");
      this.SetResourceReference(TextEditor.LineNumbersForegroundProperty, (object) "Std.Foreground");
      this.NotifyOnResourcesChanged((EventHandler) ((s, e) => this.TextArea.SelectionBorder = new Pen(this.FindResource((object) "Item.Border") as Brush, 1.0)));
      this.TextArea.SelectionBorder = new Pen(this.FindResource((object) "Item.Border") as Brush, 1.0);
      this.TextArea.SetResourceReference(TextArea.SelectionBrushProperty, (object) "Item.Background");
      this.TextArea.Margin = new Thickness(1.0);
      this.TextArea.ClipToBounds = false;
      this.TextArea.TextView.ClipToBounds = false;
      Line line1 = new Line();
      line1.Width = 8.0;
      line1.X1 = 3.0;
      line1.X2 = 3.0;
      line1.Y1 = 0.0;
      Line line2 = line1;
      line2.SetBinding(Line.Y2Property, (BindingBase) new Binding("ActualHeight")
      {
        Source = (object) this.TextArea
      });
      line2.SetResourceReference(Shape.StrokeProperty, (object) "Std.Border2");
      MyLineNumberMargin lineNumberMargin = new MyLineNumberMargin();
      lineNumberMargin.SetResourceReference(MyLineNumberMargin.BackgroundProperty, (object) "Std.Background2");
      this.TextArea.LeftMargins[1] = (UIElement) line2;
      this.TextArea.LeftMargins[0] = (UIElement) lineNumberMargin;
      this.TextArea.TextEntered += (TextCompositionEventHandler) ((s, e) =>
      {
        string text1 = e.Text;
        if (!(text1 == "{"))
        {
          if (!(text1 == "("))
          {
            if (text1 == "[")
              this.Insert("]");
          }
          else
            this.Insert(")");
        }
        else
          this.Insert("}");
        if (this.Text.Length <= 1 || this.CaretOffset <= 1 || this.CaretOffset == this.Text.Length)
          return;
        char ch = this.CharAt(this.CaretOffset);
        string text2 = e.Text;
        if (!(text2 == "'"))
        {
          if (!(text2 == "\""))
          {
            if (!(text2 == "}"))
            {
              if (!(text2 == ")"))
              {
                if (!(text2 == "]"))
                {
                  if (text2 == null)
                    ;
                  return;
                }
                if (ch != ']')
                  return;
              }
              else if (ch != ')')
                return;
            }
            else if (ch != '}')
              return;
          }
          else if (ch != '"')
          {
            if (ch == '"')
              return;
            goto label_29;
          }
        }
        else if (ch != '\'')
        {
          if (ch == '\'')
            return;
          goto label_29;
        }
        this.DeleteNext();
        return;
label_29:
        this.Insert(string.Format("{0}", (object) e.Text[0]));
      });
      this.TextArea.PreviewKeyDown += (KeyEventHandler) ((s, e) =>
      {
        if (e.Key != Key.Back || this.TextArea.Selection.Length >= 1 || this.Document.TextLength <= 1 || this.CaretOffset == this.Document.TextLength)
          return;
        int num = (int) this.CharAt(this.CaretOffset - 1);
        char ch = this.CharAt(this.CaretOffset);
        switch ((char) num)
        {
          case '"':
            if (ch != '"')
              break;
            this.DeleteNext();
            break;
          case '\'':
            if (ch != '\'')
              break;
            this.DeleteNext();
            break;
          case '(':
            if (ch != ')')
              break;
            this.DeleteNext();
            break;
          case '[':
            if (ch != ']')
              break;
            this.DeleteNext();
            break;
          case '{':
            if (ch != '}')
              break;
            this.DeleteNext();
            break;
        }
      });
    }
  }
}
