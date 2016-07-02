# viviancpy-simple-calculator
I got a requirement from a trader to create a calculator for him to defined his customized formular like Pivot = (High+Low+Open+Close)/4 and plot it on the trading app he is using.

While I thought calculating +-*/ is something simple, how to program the computer the do it right is any other piece of art.

In this example, it demonstrates how to :

1. define the algebaric steps in C# Expression Tree
2. put the variables (like High, Low, Open, Close and any other self defined variables) in Stack and Queue to queue up in right calculation order. Remember 3+3*3= 12, not 18.
3. use WPF and XAML to show the num pad and result

Reference of Algorithm for scientific calculator (Shunting -yard):
https://brilliant.org/wiki/shunting-yard-algorithm/

