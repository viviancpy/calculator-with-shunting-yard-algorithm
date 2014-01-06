using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SimpleCalculator
{
    /// <summary>
    /// Interaction logic for Calculator.xaml
    /// </summary>
    public partial class Calculator : Window
    {
        private string _inputFormula = "";

        public Calculator()
        {
            InitializeComponent();
        }

        private void OnCalculate(object sender, RoutedEventArgs e)
        {
            var syntaxTree = new RpnSyntaxTree(_inputFormula);
            try
            {
                double result = syntaxTree.GetExpressionResult();
                ResultTextBlock.Text = string.Format("{0} = {1}", _inputFormula, result);
            }
            catch{
                ResultTextBlock.Text = string.Format("Cannot compute '{0}'", _inputFormula);

            }finally
            {
                _inputFormula = "";
                e.Handled = true;
            }

            
            
        }

        private void OnKeyPressed(object sender, RoutedEventArgs e)
        {
            string content = ((Button) e.Source).Content.ToString();
            _inputFormula = string.Concat(_inputFormula, content);
            ResultTextBlock.Text = _inputFormula;
        }
    }
}
