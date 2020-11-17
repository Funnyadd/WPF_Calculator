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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        private StringBuilder sb1 = new StringBuilder();
        private StringBuilder sb2 = new StringBuilder();
        private double total, lastValue;
        private static SelectedOperator? operators;
        private bool operatorAvailable = true;

        private enum SelectedOperator
        {
            Addition,
            Subtraction,
            Multiplication,
            Division
        }
        public MainWindow()
        {
            InitializeComponent();

        }

        private void btn_number_Click(object sender, RoutedEventArgs e)
        {
            Button num = (Button)sender;

            string btn_name = num.Name.Split('_')[1];

            if (sb1.Length <= 9)
            {
                sb1.Append(btn_name);
                input_show.Content = sb1.ToString();

                lastValue = double.Parse(sb1.ToString());
            }
        }

        private void btn_operator_Click(object sender, RoutedEventArgs e)
        {
            Button op = (Button)sender;

            string btn_name = op.Name;
            string operatorStr = "";

            if (operatorAvailable)
            {
                if (lastValue.ToString() == "0")
                {
                    sb1.Append("0");
                    input_show.Content = sb1.ToString();
                }

                if (sb1.ToString().EndsWith("."))
                {
                    sb1.Remove(sb1.Length - 1, 1);
                }

                total = lastValue;
                switch (btn_name)
                {
                    case "btn_div":
                        operatorStr = "÷";
                        operators = SelectedOperator.Division;
                        break;
                    case "btn_mult":
                        operatorStr = "x";
                        operators = SelectedOperator.Multiplication;
                        break;
                    case "btn_plus":
                        operatorStr = "+";
                        operators = SelectedOperator.Addition;
                        break;
                    case "btn_minus":
                        operatorStr = "-";
                        operators = SelectedOperator.Subtraction;
                        break;
                }
                sb2.Append(sb1.ToString() + " " + operatorStr + " ");
                contentBox.Content = sb2.ToString();
                sb1 = new StringBuilder();
                operatorAvailable = false;
                btn_plus.Foreground = new SolidColorBrush(Color.FromArgb(255, 25, 25, 25));
                btn_div.Foreground = new SolidColorBrush(Color.FromArgb(255, 25, 25, 25));
                btn_mult.Foreground = new SolidColorBrush(Color.FromArgb(255, 25, 25, 25));
                btn_minus.Foreground = new SolidColorBrush(Color.FromArgb(255, 25, 25, 25));
            }
        }

        private double? Operations(SelectedOperator? a, double x, double y)
        {
            switch (a)
            {
                case SelectedOperator.Addition: 
                    return OperationsMaker.Add(x, y);
                case SelectedOperator.Subtraction: 
                    return OperationsMaker.Subtract(x, y);
                case SelectedOperator.Multiplication: 
                    return OperationsMaker.Multiply(x, y);
                case SelectedOperator.Division: 
                    return OperationsMaker.Divide(x, y);
                default: 
                    return null;
            }
        }
        private void btn_dot_Click(object sender, RoutedEventArgs e)
        {
            if (!sb1.ToString().Contains("."))
            {
                if(sb1.Length != 0)
                {
                    sb1.Append(".");
                }
                else
                {
                    sb1.Append("0.");
                }
                input_show.Content = sb1.ToString();
            }
        }
        private void btn_plus_minus_Click(object sender, RoutedEventArgs e)
        {
            if (!sb1.ToString().Contains("-"))
            {
                sb1.Insert(0, "-");
            }
            else
            {
                sb1.Remove(0, 1);
            }
            input_show.Content = sb1.ToString();
        }
        private void btn_percent_Click(object sender, RoutedEventArgs e)
        {
            double temp;
            string minusTemp;
            if(!operatorAvailable)
            {
                temp = double.Parse(sb2.ToString().Split('-', '+', '÷', 'x')[0]);
                lastValue = (temp * lastValue) / 100;

            } else
            {
                lastValue = lastValue / 100;
            }

            if (sb1.ToString().Contains("-"))
            {
                minusTemp = "-" + lastValue.ToString();
                lastValue = double.Parse(minusTemp);
            }

            sb1 = new StringBuilder();
            sb1.Append(lastValue.ToString());
            input_show.Content = sb1.ToString();
           
        }
        private void btn_equals_Click(object sender, RoutedEventArgs e)
        {
            if (operators != null)
            {

                if (total.ToString() == "0")
                {
                    sb1.Append("0");
                    input_show.Content = sb1.ToString();
                }

                if (sb1.ToString().EndsWith("."))
                {
                    sb1.Remove(sb1.Length - 1, 1);
                }

                sb2.Append(lastValue.ToString());
                sb2.Append(" = ");
                contentBox.Content = sb2.ToString();
                sb1 = new StringBuilder();

                try
                {
                    total = (double)Operations(operators, total, lastValue);
                    sb1.Append(total.ToString());
                    
                    if(sb1.ToString().Contains("."))
                    {
                        input_show.Content = Math.Round(double.Parse(sb1.ToString()), 4);
                    }
                    else
                    {
                        input_show.Content = sb1.ToString();
                    }
                }
                catch (Exception ex) when (ex is ArithmeticException || ex is DivideByZeroException)
                {
                    input_show.FontSize = 30;
                    input_show.Content = "Cannot divide by zero";
                }

                Reset();
            }
        }
        private void btn_AC_Click(object sender, RoutedEventArgs e)
        {
            input_show.Content = "0";
            contentBox.Content = "";
            input_show.FontSize = 48;

            Reset();
        }
    
        private void Reset()
        {
            sb1 = new StringBuilder();
            sb2 = new StringBuilder();

            btn_plus.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            btn_div.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            btn_mult.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            btn_minus.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

            operatorAvailable = true;
            total = 0;
            lastValue = 0;
        }
    }
}
