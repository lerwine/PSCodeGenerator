using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PSCodeGenerator
{
    public class ParsedTypeName
    {
        private int _arrayRank;

        public ReadOnlyCollection<string> NamespaceElements { get; private set; }

        public ReadOnlyCollection<ParsedTypeName> GenericArguments { get; private set; }

        public ParsedTypeName ElementType { get; private set; }

        public string Name { get; private set; }

        public int ArrayRank => (_arrayRank > 0) ? _arrayRank : 0;

        public bool IsPointer => _arrayRank == -1;

        private ParsedTypeName(ParsedTypeName baseType, List<IParsedToken> parts) { }

        public static ParsedTypeName ParseCSharp(string name)
        {
            throw new NotImplementedException();
        }

        public static string[] SplitNameNodes(string text, out string[] genericArgs)
        {
            string[] names = (string.IsNullOrEmpty(text)) ? new string[0] : _SplitNameNodes(text).Where(s => s.Length > 0).ToArray();
            if (names.Length > 0)
            {
                string t = names.Last();
                int startIndex = t.IndexOf('<') + 1;
                if (startIndex > 0)
                {
                    names[names.Length - 1] = t.Substring(0, startIndex);
                    List<string> ga = new List<string>();
                    int level = 0;
                    for (int endIndex = startIndex; endIndex < text.Length; endIndex++)
                    {
                        char c = text[endIndex];
                        if (level > 0)
                        {
                            if (c == '>')
                                level--;
                        }
                        else if (c == ',')
                        {
                            ga.Add(text.Substring(startIndex, endIndex - startIndex).Trim());
                            startIndex = endIndex + 1;
                        }
                        else if (c == '<')
                            level++;
                    }
                    if (startIndex < text.Length)
                        ga.Add(text.Substring(startIndex));
                    genericArgs = ga.Where(s => s.Length > 0).ToArray();
                    return names;
                }
            }
            genericArgs = new string[0];
            return names;
        }

        private static IEnumerable<string> _SplitNameNodes(string text)
        {
            int level = 0, startIndex = 0;
            for (int endIndex = 0; endIndex < text.Length; endIndex++)
            {
                char c = text[endIndex];
                if (level > 0)
                {
                    if (c == '>')
                        level--;
                }
                else if (c == '.')
                {
                    yield return text.Substring(startIndex, endIndex - startIndex).Trim();
                    startIndex = endIndex + 1;
                }
                else if (c == '<')
                    level++;
                else if (c == '[')
                {
                    yield return text.Substring(startIndex, endIndex - startIndex).Trim();
                    startIndex = endIndex;
                    endIndex++;
                    while (endIndex < text.Length && text[endIndex] != ']')
                        endIndex++;
                    if (endIndex < text.Length)
                        endIndex++;
                }
            }
            if (startIndex < text.Length)
                yield return text.Substring(startIndex);
        }


        interface IParsedToken
        {
            bool IsError { get; }
            string ErrorMessage { get; }
            int StartIndex { get; }
            int EndIndex { get; }
            IParsedToken PreviousToken { get; }
            IParsedToken AppendDot(int index);
            IParsedToken AppendName(string text, int index);
            IParsedToken AppendLeftAngleBracket(int index);
            IParsedToken AppendRightAngleBracket(int index);
            IParsedToken AppendLeftSquareBracket(int index);
            IParsedToken AppendRightSquareBracket(int index);
            IParsedToken AppendComma(int index);
            IParsedToken AppendPointerSymbol(int index);
            IParsedToken AppendWhitespace(string text, int index);
            ParsedTypeName ToParsedTypeName(string text);
        }

        class InvalidCharacter : IParsedToken
        {
            public IParsedToken PreviousToken { get; }

            public string ErrorMessage { get; }

            public int StartIndex { get; }

            public InvalidCharacter(int index, IParsedToken previousToken, string message)
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index");
                StartIndex = index;
                PreviousToken = previousToken;
                ErrorMessage = ((string.IsNullOrWhiteSpace(message)) ? "Invalid character" : message) + " at index " + index.ToString();
            }

            public InvalidCharacter(int index, IParsedToken previousToken) : this(index, previousToken, null) { }

            bool IParsedToken.IsError => true;

            int IParsedToken.EndIndex => StartIndex + 1;

            IParsedToken IParsedToken.AppendComma(int index) => throw new FormatException(ErrorMessage);

            IParsedToken IParsedToken.AppendDot(int index) => throw new FormatException(ErrorMessage);

            IParsedToken IParsedToken.AppendLeftAngleBracket(int index) => throw new FormatException(ErrorMessage);

            IParsedToken IParsedToken.AppendLeftSquareBracket(int index) => throw new FormatException(ErrorMessage);

            IParsedToken IParsedToken.AppendName(string text, int index) => throw new FormatException(ErrorMessage);

            IParsedToken IParsedToken.AppendPointerSymbol(int index) => throw new FormatException(ErrorMessage);

            IParsedToken IParsedToken.AppendRightAngleBracket(int index) => throw new FormatException(ErrorMessage);

            IParsedToken IParsedToken.AppendRightSquareBracket(int index) => throw new FormatException(ErrorMessage);

            IParsedToken IParsedToken.AppendWhitespace(string text, int index) => throw new FormatException(ErrorMessage);

            ParsedTypeName IParsedToken.ToParsedTypeName(string text) => throw new FormatException(ErrorMessage);
        }

        class EmptyText : IParsedToken
        {
            public const string ErrorMessage = "Unexpected end of string";
            bool IParsedToken.IsError => true;

            string IParsedToken.ErrorMessage => ErrorMessage;

            int IParsedToken.StartIndex => 0;

            public int EndIndex { get; private set; }

            IParsedToken IParsedToken.AppendComma(int index) => AppendInvalidCharacter(index);

            private InvalidCharacter AppendInvalidCharacter(int index)
            {
                if (index != EndIndex)
                    throw new ArgumentOutOfRangeException("index");
                return new InvalidCharacter(index, this);
            }

            IParsedToken IParsedToken.AppendDot(int index) => AppendInvalidCharacter(index);

            IParsedToken IParsedToken.AppendLeftAngleBracket(int index) => AppendInvalidCharacter(index);

            IParsedToken IParsedToken.AppendLeftSquareBracket(int index) => AppendInvalidCharacter(index);

            public ParsedName AppendName(string text, int index)
            {
                if (index != EndIndex)
                    throw new ArgumentOutOfRangeException("index");
                new ParsedName(text, index);
            }

            IParsedToken IParsedToken.AppendName(string text, int index) => AppendName(text, index);

            IParsedToken IParsedToken.AppendWhitespace(string text, int index) => AppendWhitespace(text, index);

            IParsedToken IParsedToken.AppendPointerSymbol(int index) => AppendInvalidCharacter(index);

            IParsedToken IParsedToken.AppendRightAngleBracket(int index) => AppendInvalidCharacter(index);

            IParsedToken IParsedToken.AppendRightSquareBracket(int index) => AppendInvalidCharacter(index);

            public EmptyText AppendWhitespace(string text, int index)
            {
                if (index != EndIndex || index >= text.Length)
                    throw new ArgumentOutOfRangeException("index");
                if (!Char.IsWhiteSpace(text[index]))
                    throw new InvalidOperationException("Character at index " + index + " is not a whitespace character");
                index++;
                while (index < text.Length && Char.IsWhiteSpace(text[index]))
                    index++;
                EndIndex = index;
                return this;
            }

            ParsedTypeName IParsedToken.ToParsedTypeName(string text) => throw new FormatException(ErrorMessage);
        }

        class ParsedName : IParsedToken
        {
            public int StartIndex { get; }

            public int EndIndex { get; }

            public ParsedName(string text, IParsedToken previousToken)
            {
                if (previousToken.EndIndex < 0 || previousToken.EndIndex >= text.Length)
                    throw new ArgumentOutOfRangeException("previousToken");
                if (!(Char.IsLetter(text[previousToken.EndIndex]) || text[previousToken.EndIndex] == '_'))
                    throw new InvalidOperationException("Character at index " + previousToken.EndIndex + " is not a whitespace character");

                StartIndex = previousToken.EndIndex;
                int endIndex = StartIndex + 1;
                while (endIndex < text.Length && (Char.IsLetter(text[endIndex]) || Char.IsDigit(text[endIndex]) || text[endIndex] == '_'))
                    endIndex++;
                EndIndex = endIndex;
            }

            bool IParsedToken.IsError => false;

            string IParsedToken.ErrorMessage => null;

            IParsedToken IParsedToken.AppendComma(int index)
            {
                throw new NotImplementedException();
            }

            IParsedToken IParsedToken.AppendDot(int index)
            {
                throw new NotImplementedException();
            }

            IParsedToken IParsedToken.AppendLeftAngleBracket(int index)
            {
                throw new NotImplementedException();
            }

            IParsedToken IParsedToken.AppendLeftSquareBracket(int index)
            {
                throw new NotImplementedException();
            }

            IParsedToken IParsedToken.AppendName(string text, int index)
            {
                throw new NotImplementedException();
            }

            IParsedToken IParsedToken.AppendPointerSymbol(int index)
            {
                throw new NotImplementedException();
            }

            IParsedToken IParsedToken.AppendRightAngleBracket(int index)
            {
                throw new NotImplementedException();
            }

            IParsedToken IParsedToken.AppendRightSquareBracket(int index)
            {
                throw new NotImplementedException();
            }

            IParsedToken IParsedToken.AppendWhitespace(string text, int index)
            {
                throw new NotImplementedException();
            }

            ParsedTypeName IParsedToken.ToParsedTypeName(string text)
            {
                throw new NotImplementedException();
            }
        }

        private static ParsedTypeName ParseCSharp(string name, int startIndex, out int endIndex)
        {
            char[] splitAt = new char[] { '.', '<', '[', '*', ',', '>' };

            List<IParsedToken> parts = new List<IParsedToken>();
            ParsedTypeName baseType = null;
            endIndex = startIndex;
            while (startIndex < name.Length && (endIndex = name.IndexOfAny(splitAt, startIndex)) >= 0)
            {
                switch (name[endIndex])
                {
                    case '.':
                        ParsedName n;
                         if (!ParsedName.TryCreate(name.Substring(startIndex, endIndex - startIndex), out n))
                            throw new FormatException("Invalid name at index " + startIndex.ToString());
                        parts.Add(n);
                        break;
                    case '>':
                    case ',':
                        return new ParsedTypeName(baseType, parts);
                    case '<':
                        List<ParsedTypeName> genericArgs = new List<ParsedTypeName>();
                        do
                        {
                            endIndex++;
                            while (endIndex < name.Length && Char.IsWhiteSpace(name[endIndex]))
                                endIndex++;
                            if (endIndex == name.Length)
                                throw new FormatException("Unexpected end of string");
                            genericArgs.Add(ParseCSharp(name, endIndex, out endIndex));
                            if (endIndex == name.Length)
                                throw new FormatException("Unexpected end of string");
                        } while (name[endIndex] == ',');
                        if (name[endIndex] != '>')
                            throw new FormatException("Invalid character at index " + endIndex.ToString());
                        break;
                    case '*':
                        if (parts.Count == 0)
                            throw new FormatException("Invalid character at index " + endIndex.ToString());
                        baseType = new ParsedTypeName(baseType, parts);
                        parts = new List<IParsedToken>();
                        parts.Add(new ParsedPointer());
                        break;
                    case '[':
                        int rank = 0;
                        do
                        {
                            rank++;
                            endIndex++;
                            while (endIndex < name.Length && Char.IsWhiteSpace(name[endIndex]))
                                endIndex++;
                            if (endIndex == name.Length)
                                throw new FormatException("Unexpected end of string");
                        } while (name[endIndex] == ',');
                        if (name[endIndex] != ']')
                            throw new FormatException("Invalid character at index " + endIndex.ToString());
                        baseType = new ParsedTypeName(baseType, parts);
                        parts = new List<IParsedToken>();
                        parts.Add(new ParsedIndexer(rank));
                        break;
                }
                startIndex = endIndex + 1;
            }

            endIndex = name.Length;
            return new ParsedTypeName(baseType, parts);
        }
    }
}
