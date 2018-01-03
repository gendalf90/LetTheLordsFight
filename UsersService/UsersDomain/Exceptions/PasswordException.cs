﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UsersDomain.Exceptions
{
    public class PasswordException : Exception
    {
        public PasswordException()
        {
        }

        public PasswordException(string message) : base(message)
        {
        }

        public PasswordException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
