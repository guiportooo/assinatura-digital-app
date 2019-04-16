using AssinaturaDigital.Utilities;
using System;
using System.Threading.Tasks;

namespace AssinaturaDigital.Extensions
{
    public static class TaskExtensions
    {
        public static async void FireAndForget(this Task task, IErrorHandler errorHandler)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                errorHandler.HandleError(ex);
            }
        }
    }
}
