using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SimpleCalculator
{
    public class FunctionToken : Token
    {
        public byte Precedence { get; private set; }
        public bool IsLeftAssociate { get; private set; }
        public MathOperationMethod MathOperationMethod { get; private set; }

        public FunctionToken(char operatorChar, byte precedence, byte operandNumber, bool isLeftAssociate, MathOperationMethod op)
        {
            // Todo FunctionToken can be more than one in future phases , e.g. >=
            TokenCharacters = new List<char> { operatorChar };
            Precedence = precedence;
            OperandNumber = operandNumber;
            IsLeftAssociate = isLeftAssociate;
            MathOperationMethod = op;
        }

        public override Expression<Func<IList<Expression>, Expression>> ComputationExpression()
        {
            switch (MathOperationMethod)
            {
                case MathOperationMethod.Addition:
                    Expression<Func<IList<Expression>, Expression>> addExpression =
                        expressions => Expression.Add(expressions[0], expressions[1]);
                    return addExpression;
                case MathOperationMethod.Subtraction:
                    Expression<Func<IList<Expression>, Expression>> subtractionExpression =
                        expressions => Expression.Subtract(expressions[0], expressions[1]);
                    return subtractionExpression;
                case MathOperationMethod.Multiplication:
                    Expression<Func<IList<Expression>, Expression>> mutliplicationExpression =
                        expressions => Expression.Multiply(expressions[0], expressions[1]);
                    return mutliplicationExpression;
                case MathOperationMethod.Division:
                    Expression<Func<IList<Expression>, Expression>> divisionExpression =
                        expressions => Expression.Divide(expressions[0], expressions[1]);
                    return divisionExpression;
                case MathOperationMethod.Negate:
                    Expression<Func<IList<Expression>, Expression>> negateExpression =
                        expressions => Expression.Negate(expressions[0]);
                    return negateExpression;
                default:
                    string ex = "Unhandled Operation: " + MathOperationMethod.ToString();
                    throw new InvalidOperationException(ex);
            }
        }

        public override bool IsLastTokenPoppedByCloseBracket(char closeBracket)
        {
            // this token is popped to queue but it is not the last token to be popped 
            return false;
        }

        public override bool IsPoppedByFucntionToken(FunctionToken functionToken)
        {
            if (functionToken.IsLeftAssociate && functionToken.Precedence == this.Precedence)
                return true;
            if (functionToken.Precedence < this.Precedence)
                return true;
            return false;
        }
    }

}
