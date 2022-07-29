// Decompiled with JetBrains decompiler
// Type: Syde.CLS.Functions
// Assembly: FrostHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B53F501E-4295-47D8-80B6-D5DA533B6790
// Assembly location: C:\Program Files\Synapse\FrostHub.exe

using ICSharpCode.AvalonEdit;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Syde.CLS
{
  internal static class Functions
  {
    public static readonly Random Rnd = new Random();
    private static readonly EventInfo ResourcesChangedEvent = typeof (FrameworkElement).GetEvent("ResourcesChanged", BindingFlags.Instance | BindingFlags.NonPublic);

    public static string RandomString(int length) => new string(Enumerable.Repeat<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*=+-;:\\|,<.>/?", length).Select<string, char>((Func<string, char>) (s => s[Functions.Rnd.Next(s.Length)])).ToArray<char>());

    public static void NotifyOnResourcesChanged(
      this FrameworkElement element,
      EventHandler onResourcesChanged)
    {
      Functions.ResourcesChangedEvent.AddMethod.Invoke((object) element, new object[1]
      {
        (object) onResourcesChanged
      });
    }

    public static Window GetRootWindow(this Visual visual) => Window.GetWindow((DependencyObject) visual);

    public static T[] Slice<T>(this T[] arr, int start, int length)
    {
      T[] objArray = new T[length];
      for (int index = start; index < length; ++index)
        objArray[index - start] = arr[index];
      return objArray;
    }

    public static void DeleteNext(this TextEditor editor) => editor.Document.Remove(editor.CaretOffset, 1);

    public static void Insert(this TextEditor editor, string text)
    {
      editor.Document.Insert(editor.CaretOffset, text);
      --editor.CaretOffset;
    }

    public static char CharAt(this TextEditor editor, int offset) => editor.Document.GetCharAt(offset);

    public static void Invoke(this UIElement element, Action a) => element.Dispatcher.Invoke(a);

    public static T GetTemplateItem<T>(this Control elem, string name) => elem.Template.FindName(name, (FrameworkElement) elem) is T name1 ? name1 : default (T);

    public static RoutedEventHandlerInfo[] GetRoutedEventHandlers(
      this UIElement element,
      RoutedEvent routedEvent)
    {
      PropertyInfo property = typeof (UIElement).GetProperty("EventHandlersStore", BindingFlags.Instance | BindingFlags.NonPublic);
      object obj = property != (PropertyInfo) null ? property.GetValue((object) element, (object[]) null) : (object) null;
      MethodInfo method = obj?.GetType().GetMethod(nameof (GetRoutedEventHandlers), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      object routedEventHandlers;
      if (!(method != (MethodInfo) null))
        routedEventHandlers = (object) null;
      else
        routedEventHandlers = method.Invoke(obj, new object[1]
        {
          (object) routedEvent
        });
      return (RoutedEventHandlerInfo[]) routedEventHandlers;
    }

    public static void ClearClickHandlers(this Button element)
    {
      foreach (RoutedEventHandlerInfo routedEventHandler in element.GetRoutedEventHandlers(ButtonBase.ClickEvent))
        element.Click -= (RoutedEventHandler) routedEventHandler.Handler;
    }

    public static T GetTemplateChild<T>(this Control e, string name) where T : FrameworkElement => e.Template.FindName(name, (FrameworkElement) e) as T;
  }
}
