# Introduction #

I would like to explore more about using Expression and `Expression<TDelegate>` and therefore I started this project. The product is a simple calculator which can perform `+-*/ and ()`.

I apply the Shunting yard algorithm when I build the Expression tree.
The details of the algorithm can be found [here](http://en.wikipedia.org/wiki/Shunting-yard_algorithm).


# Details #
## Token ##

In a simple mathematical arithmetic expression, I categorize the elements into three types:


**1. Constant**

Constant is just the number in the inputted expression. For example, 2, 0, 35.6. It should be noted that -3 is not considered as a single constant, instead it is interpreted as Negate of 3 where negate will be explained later.
ConstantToken class represents this type of element.


**2. Functional operator**

Functional operator is the mathematical operator which will requires some operands and perform some arithmetic computation. In my simple calculator, only addition, multiplication, subtraction, division and negate are supported. Each FunctionToken will define its only ComputationExpression() method according to the input list of Expression.

FunctionToken class represents this type of element.


**3. Bracket**

Bracket in an arithmetic expression represents to perform some operation with higher priority which means the interpretation of the expression is not a simply from left to right expression. ( and ) is the pair of brackets supported in this calculator.
But in the extension of this project, I would like to implement calculator with memory and it may need [and ](.md) to represent something related.
BracketToken class represents this type of

## TokenManager ##
It is the manager class to create different type of tokens. It should be noted that to create different token for negate and subtraction, the manager needs to know the currentComputationToken, which represents the last FunctionToken or Constant parsed. If it was a FunctionToken, and we have a - again, then this - will be treated as a Negate. Otherwise, it will be treated as a Subtraction.

## RpnSyntaxTree ##
To implement the algorithm, there are several components which are neccessary.


**1. Expression Tree**

It is the expression tree which translate the inputted arithmetic expression to a tree like logic. The tree is stored in `Stack<Expression> _expressionStack`.


How is this updated:

Whenever there is a valid ConstantToken created, the constant value is transformed to an expression using Expression.Constant and this expression is push to this stack. Here valid means a complete ConstantToken. It is necessary to stress it because the ParseFormular method of this class interpret each character in the inputted arithmetic expression one by one. If the user inputs as 12 + 3 , after reading 1, the ConstantToken should not be concluded yet as it is obvious that there should be a single ConstantToken should represent 12.

Whenever there is a FunctionToken popped from the operatorStack (discussed later), the Expression Stack will pop enough expression out and the popped expressions become the operands of the Expression related to the FunctionToken. The Expression related to the FunctionToken will use the operands to perform the lambda wrapped by the Expression related to the FunctionToken and form a new Expression object, which is the result of the lambda. This new Expression object will be pushed to the Expression Stack. When all the characters in the inputted arithmetic expression are handled, this stack should only contain only one Expression object and it represents the Expression Tree.


**2. Operator Token Stack**

It is the operator stack described in the [ttp://en.wikipedia.org/wiki/Shunting-yard\_algorithm Shunting-yard algorithm]. The stack is represented as `Stack<Token> _opeartorTokenStack`.


It will store the open bracket and the function operator. It will be updated if a close bracket comes or a function operator comes.
In short, the operator token stack will be popped if the newly come function operator has lower priority (considering left associate and precedence) than the first function token on the stack. Another possibility the operator stack could be updated is when a close bracket comes and the operators will be popped until a matched open bracket is found in the stack.

Popping out operator token from the stack means that enough operands are available and it is time the Exprssion associated with the popped function operator should be added to the expression tree.


**3. Computation Token Queue**

This is a modified version of the output queue described in the [ttp://en.wikipedia.org/wiki/Shunting-yard\_algorithm Shunting-yard algorithm]. Instead of storing the operator in the queue, the queue in this project will only contain the constantToken. The reason for this is because whenever there is a function operator token popped out from the operator token stack, it could already transform to a proper Expression object and add to the Expression Stack and there is no need to wait for the whole RPN expression , e.g. `3 4 2 * 1 5 − / + for  3 + 4 * 2 / ( 1 - 5 )`. The queue is represented by `Queue<ConstantToken> _computationTokenQueue`.