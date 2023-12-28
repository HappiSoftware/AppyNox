using AppyNox.Services.Base.Domain.ExceptionExtensions.Enums;
using AppyNox.Services.Base.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Domain.ExceptionExtensions.Base
{
    public abstract class NoxException : Exception
    {
        #region [ Properties ]

        private readonly string _title = ExceptionThrownLayer.DomainBase.ToString();

        private readonly int _statusCode;

        public int StatusCode
        {
            get => _statusCode;
        }

        public string Title
        {
            get => _title;
        }

        #endregion

        #region [ Public Constructors ]

        protected NoxException()
            : base()
        {
            _statusCode = 500;
        }

        protected NoxException(string message)
            : base(message)
        {
            _statusCode = 500;
        }

        protected NoxException(int statusCode)
            : base()
        {
            _statusCode = statusCode;
        }

        protected NoxException(string message, int statusCode)
            : base(message)
        {
            _statusCode = statusCode;
        }

        protected NoxException(Enum title, string message, int statusCode)
            : base(message)
        {
            _title = title.GetDisplayName();
            _statusCode = statusCode;
        }

        protected NoxException(Enum title, string message, Exception innerException)
            : base(message, innerException)
        {
            _title = title.GetDisplayName();
            _statusCode = 500;
        }

        protected NoxException(string message, int statusCode, Exception innerException)
            : base(message, innerException)
        {
            _statusCode = statusCode;
        }

        #endregion
    }
}