using System;
using System.Collections.Generic;

namespace SimpleCalculator
{
    public static class TokenManager
    {
        private static readonly IList<char> SupportedFunctions = new List<char>();
        private static readonly IDictionary<char, char> Brackets = new Dictionary<char, char>();

        static TokenManager()
        {
            SupportedFunctions.Add('+');
            SupportedFunctions.Add('-');
            SupportedFunctions.Add('*');
            SupportedFunctions.Add('/');

            Brackets.Add('(', ')');
            Brackets.Add('[', ']');
            Brackets.Add('{', '}');
        }

        public static ConstantToken CreateConstantToken(ConstantToken lastConstantToken, char nextCharacter)
        {
            // need currentComputationToken because
            // if current computation token is a constant token (number) and if nextCharacter is still a number, 
            // attach to current constant token (number) 

            // nextCharacter is numeric, only ConstantToken will be returned

            if (lastConstantToken != null)
            {
                var characters = new List<char>(lastConstantToken.TokenCharacters);
                characters.Add(nextCharacter);
                return new ConstantToken(characters);
            }
            return new ConstantToken(new List<char> { nextCharacter });
        }

        public static FunctionToken CreateFunctionToken(Token currentComputationToken, char nextCharacter)
        {
            // need currentComputationToken because
            // if current computation token is a functional token (operator not including brackets) and if nextCharacter -
            // define the token as negate instead of subtraction
            
            switch (nextCharacter)
            {
                case '+':
                    return new FunctionToken('+', 2, 2, true, MathOperationMethod.Addition);
                case '*':
                    return new FunctionToken('*', 3, 2, true, MathOperationMethod.Multiplication);
                case '/':
                    return new FunctionToken('*', 3, 2, true, MathOperationMethod.Division);
                case '-':
                    var t = currentComputationToken as FunctionToken;
                    return currentComputationToken != null && t == null ? new FunctionToken('-', 2, 2, true, MathOperationMethod.Subtraction)
                               : new FunctionToken('-', 4, 1, false, MathOperationMethod.Negate);
                default:
                    throw new ArgumentException("Unknown operation: " + nextCharacter);
            }
        }

        public static BracketToken CreateBracketToken(char nextCharacter)
        {
            switch (nextCharacter)
            {
                case '(':
                    return new BracketToken('(', ')');
                case '{':
                    return new BracketToken('{', '}');
                case '[':
                    return new BracketToken('[', ']');
                default:
                    throw new ArgumentException("Unknown bracket: " + nextCharacter);
            }
        }

        public static bool IsOpenBracket(char chr)
        {
            return Brackets.Keys.Contains(chr);
        }

        public static bool IsCloseBracket(char chr)
        {
            return Brackets.Values.Contains(chr);
        }

        public static bool IsSupportedFunction(char chr)
        {
            return SupportedFunctions.Contains(chr);
        }
    }
}
