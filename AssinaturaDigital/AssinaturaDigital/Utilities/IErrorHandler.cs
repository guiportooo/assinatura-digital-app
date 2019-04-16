using System;
namespace AssinaturaDigital.Utilities
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
