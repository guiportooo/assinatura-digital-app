using AssinaturaDigital.Utilities;
using System;

namespace AssinaturaDigital.UnitTests.Mocks
{
    public class ErrorHandlerMock : IErrorHandler
    {
        public Exception Exception { get; set; }

        public void HandleError(Exception ex) => Exception = ex;
    }
}
