using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Common
{
    public abstract class BaseResponse
    {
        public bool IsSucceeded { get; set; }
        public string? Message { get; set; }
    }
}
