using Template.CrossCutting.FluentValidator;
using System.Collections.Generic;

namespace Template.Tests.Util
{
    public class ResponseMvc
    {
        public bool sucess { get; set; }
        public List<Notification> errors { get; set; }
    }
}
