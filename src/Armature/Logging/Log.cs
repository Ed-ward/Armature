﻿using System;
using JetBrains.Annotations;

namespace Armature.Logging
{
  public static class Log
  {
    private static int _indent;
    private static LogLevel _logLevel;

    public static IDisposable Enabled(LogLevel logLevel = LogLevel.Info)
    {
      _logLevel = logLevel;
      return new Bracket(() => _logLevel = logLevel, () => _logLevel = LogLevel.None);
    }

    public static IDisposable Block(string name, LogLevel logLevel = LogLevel.Info)
    {
      var currentLogLevel = _logLevel;
      if (logLevel > _logLevel) // disable logging for all block content
        return new Bracket(() => _logLevel = LogLevel.None, () => _logLevel = currentLogLevel);
      
      WriteLine(logLevel, name);
      return logLevel <= _logLevel ? AddIndent(true) : new DumbDisposable();
    }

    [StringFormatMethod("format")]
    public static void Info(string format, params object[] parameters)
    {
      WriteLine(LogLevel.Info, format, parameters);
    }
    
    [StringFormatMethod("format")]
    public static void Trace(string format, params object[] parameters)
    {
      WriteLine(LogLevel.Trace, format, parameters);
    }
    
    [StringFormatMethod("format")]
    public static void Verbose(string format, params object[] parameters)
    {
      WriteLine(LogLevel.Verbose, format, parameters);
    }

    [StringFormatMethod("format")]
    public static void WriteLine(LogLevel logLevel, string format, params object[] parameters)
    {
      if(logLevel > _logLevel) return;
      System.Diagnostics.Trace.WriteLine(GetIndent() + string.Format(format, parameters));
    }

    private static string GetIndent()
    {
      var indent = "";
      for (var i = 0; i < _indent; i++)
        indent += "  ";
      return indent;
    }

    public static IDisposable AddIndent(bool newBlock = false, int count = 1)
    {
      return new Indenter(newBlock, count);
    }

    private class Indenter : IDisposable
    {
      private readonly bool _newBlock;
      private readonly int _count;

      public Indenter(bool newBlock, int count)
      {
        if(newBlock)
          Info("{{");

        _newBlock = newBlock;
        _count = count;
        _indent += count;
      }

      public void Dispose()
      {
        _indent -= _count;
        if(_newBlock)
          Info("}}");
      }
    }

    private class Bracket : IDisposable
    {
      private readonly Action _endAction;

      public Bracket(Action startAction, Action endAction)
      {
        startAction();
        _endAction = endAction;
      }

      public void Dispose()
      {
        _endAction();
      }
    }

    private class DumbDisposable : IDisposable
    {
      public void Dispose()
      {
        // dumb
      }
    }
  }

  public enum LogLevel
  {
    None = 0,
    Info,
    Verbose,
    Trace,
  }
}