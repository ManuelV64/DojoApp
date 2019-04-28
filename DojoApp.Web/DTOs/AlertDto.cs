using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DojoApp.Web.DTOs
{
    public class AlertDto
    {
        public AlertDto(int status, string header, string message) {
            Status = status;
            Header = header;
            Message = message;
        }

        public int Status { get; set; }
        public string Header { get; set; }
        public string Message { get; set; }
    }
}

