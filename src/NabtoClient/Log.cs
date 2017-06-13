using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace Nabto
{
    /// <summary>
    /// Provides wrapper methods for writing to the debug console.
    /// </summary>
    class Log
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        static public void Write(string message)
        {
            Debug.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString("HH.mm.ss:fff"), message));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        static public void Write(object obj)
        {
            Write(obj.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        static public void Write(string format, params object[] arguments)
        {
            Write(string.Format(format, arguments));
        }

#if NET45

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        static public void Action([CallerFilePath]string callerFilePath = "", [CallerMemberName]string callerMemberName = "")
        {
            Write("{0}.{1}()", ConvertCallerFilePathToClassName(callerFilePath), callerMemberName);
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        static public void Action(object arg0, [CallerFilePath]string callerFilePath = "", [CallerMemberName]string callerMemberName = "")
        {
            Write("{0}.{1}({2})", ConvertCallerFilePathToClassName(callerFilePath), callerMemberName, arg0);
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        static public void Action(object arg0, object arg1, [CallerFilePath]string callerFilePath = "", [CallerMemberName]string callerMemberName = "")
        {
            Write("{0}.{1}({2}, {3})", ConvertCallerFilePathToClassName(callerFilePath), callerMemberName, arg0, arg1);
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        static public void Action(object arg0, object arg1, object arg2, [CallerFilePath]string callerFilePath = "", [CallerMemberName]string callerMemberName = "")
        {
            Write("{0}.{1}({2}, {3}, {4})", ConvertCallerFilePathToClassName(callerFilePath), callerMemberName, arg0, arg1, arg2);
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        static public void Action(object arg0, object arg1, object arg2, object arg3, [CallerFilePath]string callerFilePath = "", [CallerMemberName]string callerMemberName = "")
        {
            Write("{0}.{1}({2}, {3}, {4}, {5})", ConvertCallerFilePathToClassName(callerFilePath), callerMemberName, arg0, arg1, arg2, arg3);
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        static public void Action(object arg0, object arg1, object arg2, object arg3, object arg4, [CallerFilePath]string callerFilePath = "", [CallerMemberName]string callerMemberName = "")
        {
            Write("{0}.{1}({2}, {3}, {4}, {5}, {6})", ConvertCallerFilePathToClassName(callerFilePath), callerMemberName, arg0, arg1, arg2, arg3, arg4);
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        static public void Func(object returnValue, [CallerFilePath]string callerFilePath = "", [CallerMemberName]string callerMemberName = "")
        {
            Write("{0}.{1}() => {2}", ConvertCallerFilePathToClassName(callerFilePath), callerMemberName, returnValue);
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        static public void Func(object arg0, object returnValue, [CallerFilePath]string callerFilePath = "", [CallerMemberName]string callerMemberName = "")
        {
            Write("{0}.{1}({2}) => {3}", ConvertCallerFilePathToClassName(callerFilePath), callerMemberName, arg0, returnValue);
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        static public void Func(object arg0, object arg1, object returnValue, [CallerFilePath]string callerFilePath = "", [CallerMemberName]string callerMemberName = "")
        {
            Write("{0}.{1}({2}, {3}) => {4}", ConvertCallerFilePathToClassName(callerFilePath), callerMemberName, arg0, arg1, returnValue);
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        static public void Func(object arg0, object arg1, object arg2, object returnValue, [CallerFilePath]string callerFilePath = "", [CallerMemberName]string callerMemberName = "")
        {
            Write("{0}.{1}({2}, {3}, {4}) => {5}", ConvertCallerFilePathToClassName(callerFilePath), callerMemberName, arg0, arg1, arg2, returnValue);
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        static public void Func(object arg0, object arg1, object arg2, object arg3, object returnValue, [CallerFilePath]string callerFilePath = "", [CallerMemberName]string callerMemberName = "")
        {
            Write("{0}.{1}({2}, {3}, {4}, {5}) => {6}", ConvertCallerFilePathToClassName(callerFilePath), callerMemberName, arg0, arg1, arg2, arg3, returnValue);
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        static public void Func(object arg0, object arg1, object arg2, object arg3, object arg4, object returnValue, [CallerFilePath]string callerFilePath = "", [CallerMemberName]string callerMemberName = "")
        {
            Write("{0}.{1}({2}, {3}, {4}, {5}, {6}) => {7}", ConvertCallerFilePathToClassName(callerFilePath), callerMemberName, arg0, arg1, arg2, arg3, arg4, returnValue);
        }

        static string ConvertCallerFilePathToClassName(string callerFilePath)
        {
            return Path.GetFileNameWithoutExtension(callerFilePath);
        }
#endif
    }
}
