using System.Collections.Generic;
using ARCO.Api.Common.Enums;

namespace ARCO.Api.Common.Models
{
    public class ActionResponse<T> : IActionResponse
    {
        public ActionResponse()
        {
            Errors = new List<string>();
        }

        public T Data { get; set; }
        public ResponseStateEnum State { get; set; }
        public List<string> Errors { get; set; }
    }

    public class ActionResponse : IActionResponse
    {
        public ActionResponse()
        {
            Errors = new List<string>();
        }

        public ResponseStateEnum State { get; set; }
        public List<string> Errors { get; set; }
    }

    public interface IActionResponse
    {
        ResponseStateEnum State { get; set; }
        List<string> Errors { get; set; }
    }
}