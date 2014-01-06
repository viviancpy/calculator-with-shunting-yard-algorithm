using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator
{
    public class ConstantToken : Token
    {
        public ConstantToken(IEnumerable<char> constantCharacter)
        {
            TokenCharacters = new List<char>(constantCharacter);
            OperandNumber = 0;
        }

        public override bool IsLastTokenPoppedByCloseBracket(char closeBracket)
        {
            return false;
        }

        public override bool IsPoppedByFucntionToken(FunctionToken functionToken)
        {
            return false;
        }

        public override Expression<Func<IList<Expression>, Expression>> ComputationExpression()
        {
            return expressions => ConstantExpression();
        }

        public Expression ConstantExpression()
        {
            double constantValue = GetValue();
            return Expression.Constant(constantValue, typeof(double));
        }

        private double GetValue()
        {
            try
            {
                return Convert.ToDouble(new string(TokenCharacters.ToArray()));
            }
            catch
            {
                return 0.0;
            }
        }
    }

}
