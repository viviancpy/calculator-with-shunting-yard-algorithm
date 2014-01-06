using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SimpleCalculator
{
    public enum MathOperationMethod
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
        Negate
    }

    public abstract class Token
    {

        public IList<char> TokenCharacters { get; protected set; }
        public byte OperandNumber { get; protected set; }

        public abstract bool IsLastTokenPoppedByCloseBracket(char closeBracket);
        public abstract bool IsPoppedByFucntionToken(FunctionToken functionToken);

        public abstract Expression<Func<IList<Expression>, Expression>> ComputationExpression();
    }
}
