using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using ICE.Common.Business.Models;
using ICE.Common.Business.Queue;
using ARCO.Entities.Enums;

namespace ARCO.Api.Common.Class
{
    public class General
    {
        public static void LogError(Exception error, Guid VisitLogCode, bool CatchLog = true, ErrorLogTypeEnum errorLogTypeVCode = ErrorLogTypeEnum.DEFAULT_TYPE, string Ip = null, string RequestUrl = null)
        {
            LogErrorIns(error: error,  VisitLogCode: VisitLogCode, CatchLog: CatchLog, errorLogTypeVCode: errorLogTypeVCode, Ip: Ip, RequestUrl: RequestUrl);
            if (error.InnerException != null)
            {
                var innerExeption = error.InnerException;
                for (int i = 0; i <= 10; i++)
                {
                    LogErrorIns(error: innerExeption, VisitLogCode: VisitLogCode, CatchLog: CatchLog, errorLogTypeVCode: errorLogTypeVCode, Ip: Ip, RequestUrl: RequestUrl);
                    innerExeption = innerExeption.InnerException;
                    if (innerExeption == null)
                        break;
                }
            }
        }

        private static void LogErrorIns(Exception error, Guid VisitLogCode, bool CatchLog = true, ErrorLogTypeEnum errorLogTypeVCode = ErrorLogTypeEnum.DEFAULT_TYPE, string Ip = null,string RequestUrl = null)
        {
            var code = 0;
            code = (error is HttpException) ? (error as HttpException).GetHttpCode() : 500;
            if (code == 404)
                return;
            string stackTrace = (error.StackTrace == null) ? "" : Regex.Replace(error.StackTrace.Replace("'", "\""), @"\t|\n|\r", "");
            var objError = new ErrorLogData();
            objError.Code = code;
            objError.IP = Ip;
            objError.Message = error.Message;
            objError.StackTrace = stackTrace;
            objError.RequestUrl = RequestUrl;
            objError.Source = error.Source;
            objError.WebBrowser = "";
            StackTrace st = new StackTrace(true);
            st = new StackTrace(error, true);
            var frames = st.GetFrames();
            var traceString = new StringBuilder();
            if (frames == null || frames.Count() == 0)
                traceString.Append("Biar: Frames object is nothing or count is zero");
            else
                foreach (StackFrame frame in frames)
                {
                    if ((frame.GetFileLineNumber() < 1))
                        continue;
                    traceString.Append("File: " + frame.GetFileName());
                    traceString.Append(", Method:" + frame.GetMethod().Name);
                    traceString.Append(", LineNumber: " + frame.GetFileLineNumber());
                    traceString.Append("  -->  ");
                }

            if (traceString.ToString().Length == 0)
                traceString.Append("Biar: traceString is empty");
            objError.ErrorLine = traceString.ToString();
            objError.MethodName = error.TargetSite != null ? error.TargetSite.Name : "NULL";
            objError.ModuleName = error.TargetSite != null ? error.TargetSite.ReflectedType == null || string.IsNullOrEmpty(error.TargetSite.ReflectedType.FullName) ? error.TargetSite.Module.Name : error.TargetSite.ReflectedType.FullName : "NULL";
            try
            {
                objError.VisitLogCode = VisitLogCode;
            }
            catch (Exception ex)
            {
            }
            ErrorLog.Instance.Insert(objError);
        }
        public static bool IsValidNationalCode(string input)
        {
            input = input.PadLeft(10, '0');
            if (!Regex.IsMatch(input, @"^\d{10}$"))
                return false;
            var check = Convert.ToInt32(input.Substring(9, 1));
            var sum = Enumerable.Range(0, 9).Select(x => Convert.ToInt32(input.Substring(x, 1)) * (10 - x)).Sum() % 11;
            return sum < 2 && check == sum || sum >= 2 && check + sum == 11;
        }

    }
}
