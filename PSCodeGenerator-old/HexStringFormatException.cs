using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace PSCodeGenerator
{
    /// <summary>
    /// The exception that is thrown when the format of a hexidecimal is invalid, or when the hexidecimal string is not well-formed.
    /// </summary>
    [Serializable]
    public class HexStringFormatException : FormatException
    {
        /// <summary>
        /// The relative zero-based index of the invalid character or -1 if the position was not provided or was out of the range of the string.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// String that contains the invalid hexidecimal string.
        /// </summary>
        public string HexChars { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HexStringFormatException"/> class with the source text, position of invalid character, a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="position">Zero-based index of first invalid hexidecimal character, which will be stored in <seealso cref="HexStringFormatException.Position"/>.</param>
        /// <param name="hexChars">Source hexidecimal characters that were being parsed, which will be stored as a string in <seealso cref="HexStringFormatException.HexChars"/></param>
        /// <param name="message">The error message that explains the reason for the exception.
        /// If the message paremeter is null or contains only whitespace, a default error message will be created based upon the <paramref name="position"/> (if it is greater than -1) or inner exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.
        /// If the innerException parameter is not a null reference (<c>Nothing</c> in Visual Basic), the current exception is raised in a catch block that handles the inner exception.</param>
        public HexStringFormatException(int position, IEnumerable<char> hexChars, string message, Exception innerException)
            : base(GetMessage(message, position, innerException), innerException)
        {
            Initialize(position, hexChars);
        }

        /// <summary>
        /// Initializes a new instance of the System.FormatException class with the source text, position of invalid character and a specified error message.
        /// </summary>
        /// <param name="position">Zero-based index of first invalid hexidecimal character, which will be stored in <seealso cref="HexStringFormatException.Position"/>.</param>
        /// <param name="hexChars">Source hexidecimal characters that were being parsed, which will be stored as a string in <seealso cref="HexStringFormatException.HexChars"/></param>
        /// <param name="message">The error message that explains the reason for the exception.
        /// If the message paremeter is null or contains only whitespace, a default error message will be created based upon the given <paramref name="position"/>.</param>
        public HexStringFormatException(int position, IEnumerable<char> hexChars, string message)
        : base(GetMessage(message, position, null))
        {
            Initialize(position, hexChars);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HexStringFormatException"/> class with the source text, position of invalid character, a reference to the inner exception that is the cause of this exception,
        /// and an error message that is based upon the given <paramref name="position"/> (if it is greater than -1) or inner exception.
        /// </summary>
        /// <param name="position">Zero-based index of first invalid hexidecimal character, which will be stored in <seealso cref="HexStringFormatException.Position"/>.</param>
        /// <param name="hexChars">Source hexidecimal characters that were being parsed, which will be stored as a string in <seealso cref="HexStringFormatException.HexChars"/></param>
        /// <param name="innerException">The exception that is the cause of the current exception.
        /// If the innerException parameter is not a null reference (<c>Nothing</c> in Visual Basic), the current exception is raised in a catch block that handles the inner exception.</param>
        public HexStringFormatException(int position, IEnumerable<char> hexChars, Exception innerException) : this(position, hexChars, null, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HexStringFormatException"/> class with the source text and position of invalid character with an error message that is based upon the given <paramref name="position"/>.
        /// </summary>
        /// <param name="position">Zero-based index of first invalid hexidecimal character, which will be stored in <seealso cref="HexStringFormatException.Position"/>.</param>
        /// <param name="hexChars">Source hexidecimal characters that were being parsed, which will be stored as a string in <seealso cref="HexStringFormatException.HexChars"/></param>
        public HexStringFormatException(int position, IEnumerable<char> hexChars) : this(position, hexChars, null as string) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HexStringFormatException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.
        /// If the message paremeter is null or contains only whitespace, a default error message will be created based upon the inner exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.
        /// If the innerException parameter is not a null reference (<c>Nothing</c> in Visual Basic), the current exception is raised in a catch block that handles the inner exception.</param>
        public HexStringFormatException(string message, Exception innerException) : this(-1, null, message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the System.FormatException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.
        /// If the message paremeter is null or contains only whitespace, a default error message will be used.</param>
        public HexStringFormatException(string message) : this(-1, null, message) { }

        /// <summary>
        /// Initializes a new instance of the System.FormatException class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected HexStringFormatException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HexStringFormatException"/> class with a default error message.
        /// </summary>
        public HexStringFormatException() : this(null as string) { }

        private void Initialize(int position, IEnumerable<char> hexChars)
        {
            HexChars = (hexChars == null || hexChars is string) ? (string)hexChars : new String((hexChars is char[]) ? (char[])hexChars : hexChars.ToArray());
            Position = (position < 0 || (HexChars != null && position >= HexChars.Length)) ? -1 : position;
        }

        private static string GetMessage(string message, int position, Exception innerException)
        {
            if (String.IsNullOrWhiteSpace(message))
            {
                if (position < 0)
                    return (innerException == null || String.IsNullOrWhiteSpace(innerException.Message)) ? "Invalid hexidecimal string" : innerException.Message;
                return "Invalid hexidecimal character at position " + position.ToString();
            }

            return message;
        }
    }
}