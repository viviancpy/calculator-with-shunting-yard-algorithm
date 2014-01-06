using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SimpleCalculator
{
    public class RpnSyntaxTree
    {
        private readonly Stack<Token> _opeartorTokenStack = new Stack<Token>();
        private readonly Queue<ConstantToken> _computationTokenQueue = new Queue<ConstantToken>();
        private readonly Stack<Expression> _expressionStack = new Stack<Expression>();
        private string _inputFormulaString;

        public RpnSyntaxTree(string inputFormulaString)
        {
            _inputFormulaString = inputFormulaString;
        }

        public double GetExpressionResult()
        {
            ParseFormula();

            if (_expressionStack.Count > 1)
                throw new ApplicationException("Expression cannot be computed");
            Expression expression = _expressionStack.Peek();
            double result = Expression.Lambda<Func<double>>(expression).Compile()();
            return result;
        }

        private void ParseFormula()
        {
            Token currentComputationToken = null;
            ConstantToken currentConstantToken = null;

            // ignore any space
            _inputFormulaString = _inputFormulaString.Replace(" ", string.Empty);

            // loop through all characters in the formula
            foreach (char c in _inputFormulaString)
            {
                // Case 1: character is a number or decimal place
                if ((c >= '0' && c <= '9') || (c == '.'))
                {
                    currentConstantToken = TokenManager.CreateConstantToken(currentConstantToken, c);
                    currentComputationToken = currentConstantToken;
                }
                else
                {
                    // Character is not numeric 
                    // check if it means end of the constant by checking if has a current constant token
                    // if there is , enqueue it to the computation queue
                    if (currentConstantToken != null)
                    {
                        _computationTokenQueue.Enqueue(currentConstantToken);
                        var expression = currentConstantToken.ConstantExpression();
                        PushExpressionStack(expression);
                        currentConstantToken = null;
                    }

                    // before pushing to the operatorStack check if the current token 
                    // would make any possible pop to the computation queue

                    // Case 2: character is a close bracket character
                    if (TokenManager.IsCloseBracket(c))
                    {
                        if (_opeartorTokenStack.Count > 0)
                        {
                            bool foundMatchedOpenBracket = false;
                            do
                            {
                                var topOperatorToken = _opeartorTokenStack.Peek();
                                foundMatchedOpenBracket = topOperatorToken.IsLastTokenPoppedByCloseBracket(c);

                                // if it is a open bracket , no expression is needed to push to expression stack 
                                // because bracket does not perform any real operation
                                if (!foundMatchedOpenBracket)
                                {
                                    PopOperatorToComputation();
                                }
                            } while (_opeartorTokenStack.Count > 0 && !foundMatchedOpenBracket);

                            // if foundMatchedOpenBracket was never true, the open bracket was never found --> problem
                            if (!foundMatchedOpenBracket)
                                throw new InvalidOperationException("Parenthesis in the expression is not consistent.");
                        }
                        else
                        {
                            throw new InvalidOperationException("The operator token stack is empty. We encountered a closebracket while there is nothing in the operator stack");
                        }
                    }
                    else
                    {
                        // Case 3: character is a functional character
                        if (TokenManager.IsSupportedFunction(c))
                        {
                            FunctionToken functionToken = TokenManager.CreateFunctionToken(currentComputationToken, c);
                            if (_opeartorTokenStack.Count > 0)
                            {
                                bool popOneOperatorFromStack = false;
                                do
                                {
                                    var topOperatorToken = _opeartorTokenStack.Peek();
                                    popOneOperatorFromStack = topOperatorToken.IsPoppedByFucntionToken(functionToken);
                                    if (popOneOperatorFromStack)
                                    {
                                        PopOperatorToComputation();
                                    }
                                } while (_opeartorTokenStack.Count > 0 && popOneOperatorFromStack);
                            }
                            _opeartorTokenStack.Push(functionToken);
                            currentComputationToken = functionToken;
                        }

                        else
                        {
                            // Case 4: charcter is a open bracket character
                            if (TokenManager.IsOpenBracket(c))
                            {
                                BracketToken openBracketToken = TokenManager.CreateBracketToken(c);
                                _opeartorTokenStack.Push(openBracketToken);
                            }
                            else
                            {
                                // Case 5: don't know why the character is in the formular
                                throw new InvalidOperationException("Cannot identify character " + c + " in the input formula");
                            }
                        }
                    }
                }
            }

            // all characters have been parsed
            // put the last numeric to the queue, if any
            if (currentConstantToken != null)
            {
                _computationTokenQueue.Enqueue(currentConstantToken);
                Expression constantExpression = currentConstantToken.ConstantExpression();
                PushExpressionStack(constantExpression);
            }

            // pop all the operators and push expression to expression stack
            while (_opeartorTokenStack.Count > 0)
            {
                PopOperatorToComputation();
            }
        }

        private void PopOperatorToComputation()
        {
            var topOperatorToken = _opeartorTokenStack.Pop();
            Expression<Func<IList<Expression>, Expression>> expr =
                topOperatorToken.ComputationExpression();
            if (expr != null)
            {
                Stack<Expression> operands = new Stack<Expression>();
                if (_expressionStack.Count < topOperatorToken.OperandNumber)
                    throw new InvalidOperationException(
                        "Not enough operands on the expression stack for the operator.");
                for (int i = 0; i < topOperatorToken.OperandNumber; i++)
                {
                    var operand = _expressionStack.Pop();
                    operands.Push(operand);
                }
                Func<IList<Expression>, Expression> func = expr.Compile();
                Expression compiledExpression = func(operands.ToList());
                PushExpressionStack(compiledExpression);
            }
        }

        private void PushExpressionStack(Expression e)
        {
            _expressionStack.Push(e);
        }
    }

}
