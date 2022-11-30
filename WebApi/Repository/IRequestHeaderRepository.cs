using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Model_hims;

namespace WebApi.Repository
{
    public interface IRequestHeaderRepository
    {
        public string AddRequestHeader(RequestHeader requestHeader);
    }
}
