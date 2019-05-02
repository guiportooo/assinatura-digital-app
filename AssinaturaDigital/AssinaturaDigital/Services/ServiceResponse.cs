using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AssinaturaDigital.Services
{
    public abstract class ServiceResponse
    {
        public int Status { get; set; }
        public IEnumerable<ResponseError> Errors { get; set; }
        public bool Succeeded => Status == (int)HttpStatusCode.OK;
        public IEnumerable<string> ErrorMessages => Errors.Select(x => x.Message);
    }
}
