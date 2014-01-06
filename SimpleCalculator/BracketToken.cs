using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SimpleCalculator
{
    public class BracketToken : Token
    {
        public char CloseBracket { get; private set; }

        public BracketToken(char bracketCharacter, char closeBracket)
        {
            TokenCharacters = new List<char>(bracketCharacter);
            CloseBracket = closeBracket;
            OperandNumber = 0;
        }

        public override bool IsLastTokenPoppedByCloseBracket(char closeBracket)
        {
            return closeBracket == CloseBracket;
        }

        public override bool IsPoppedByFucntionToken(FunctionToken operatorToken)
        {
            return false;
        }

        public override Expression<Func<IList<Expression>, Expression>> ComputationExpression()
        {
            return null;
        }
    }
}
