using Nabto.Client.Interop;
using System;

namespace Nabto.Client
{
    /// <summary>
    /// Represents general Nabto client API exceptions.
    /// </summary>
    public class NabtoClientException : Exception
    {
        /// <summary>
        /// Initializes a new instance of a NabtoClientException with the specified message and internal Nabto status code.
        /// </summary>
        /// <param name="status">The internal Nabto status message related to the exception.</param>
        /// <param name="message">A message describing the exception.</param>
        internal NabtoClientException(NabtoStatus status, string message)
            : base(message)
        {
            HResult = CreateHResult(status);
        }

        /// <summary>
        /// Initializes a new instance of a NabtoClientException with the specified message.
        /// </summary>
        /// <param name="status">The internal Nabto status message related to the exception.</param>
        /// <param name="format">The format string for the message.</param>
        /// <param name="arguments">The arguments for the message.</param>
        internal NabtoClientException(NabtoStatus status, string format, params object[] arguments)
            : base(string.Format(format, arguments))
        {
            HResult = CreateHResult(status);
        }

        /// <summary>
        /// Creates a valid COM HResult that does not conflict with Microsofts predefined COM HResults (range 0x00000000-0x000001ff).
        /// </summary>
        /// <param name="errorClass"></param>
        /// <param name="code"></param>
        /// <returns>The HResult.</returns>
        internal int CreateHResult(ErrorClass errorClass, int code)
        {
            const int hresultSeverity = 1 << 31;
            const int hresultCustomer = 1 << 29;
            const int hresultFacilityItf = 4 << 16;

            switch (errorClass)
            {
                case ErrorClass.ClientApi:
                    code = ScaleCode(code, 0, (int)NabtoStatus.ErrorCodeCount - 1);
                    break;

                case ErrorClass.Misc:
                    code = ScaleCode(code, 7000, 7003);
                    break;

                case ErrorClass.Internal:
                    code = ScaleCode(code, 1000000, 1000104);
                    break;

                case ErrorClass.General:
                    code = ScaleCode(code, 2000000, 2000064);
                    break;

                case ErrorClass.Configuration:
                    code = ScaleCode(code, 3000000, 3000000);
                    break;
            }

            code |= (int)errorClass << 8;

            return hresultSeverity | hresultCustomer | hresultFacilityItf | code;
        }

        /// <summary>
        /// Creates an HResult from a NabtoStatus code.
        /// </summary>
        /// <param name="status">The NabtoStatus code to create an HResult from.</param>
        /// <returns>The HResult.</returns>
        int CreateHResult(NabtoStatus status)
        {
            return CreateHResult(ErrorClass.ClientApi, (int)status);
        }

        int ScaleCode(int code, int first, int last)
        {
            if (code < first || code > last)
            {
                throw new ArgumentOutOfRangeException("code");
            }

            code -= first;

            return code;
        }
    }

    internal enum ErrorClass
    {
        ClientApi = 2,
        Misc = 3,
        Internal = 4,
        General = 5,
        Configuration = 6
    }
}