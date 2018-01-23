using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace SBIReportUtility.Common.General
{
    /// <summary>
    /// Class MethodHelper.
    /// </summary>
    public class MethodHelper
    {
        /// <summary>
        /// Gets the name of the current method.
        /// </summary>
        /// <returns>System.String.</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethodName()
        {
            string methodName = "";
            try
            {
                var st = new StackTrace(new StackFrame(1));
                methodName = st.GetFrame(0).GetMethod().Name;
            }
            catch
            {
                methodName = "Error";
            }
            return methodName;
        }

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        public static void CreateDirectory(string baseUrl)
        {
            if (!Directory.Exists(baseUrl)) Directory.CreateDirectory(baseUrl);
        }

        /// <summary>
        /// Masks the code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>System.String.</returns>
        public static string MaskCode(string code)
        {
            if (!string.IsNullOrEmpty(code))
                return new string('X', code.Length);
            else return code;
        }
    }
}
