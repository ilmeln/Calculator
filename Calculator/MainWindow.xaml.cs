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

namespace Calculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public enum Type
    {
        XpowY,
        sin,
        cos,
        tan,
        sqrt,
        log,
        Exp,
        Mod,
        e,
        ln,
        Divide,
        pi,
        Multiply,
        Minus,
        Plus,
        fact,
        LeftS,
        RightS,
        Number,
        End
    }


    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            tStream = new TokenStream();
            calculator = new Caclulator();
            exp = nmb = "";

        }

        private void OperatorClick(object sender,RoutedEventArgs args)
        {
            Button button = (Button)sender;
            string s = button.Content.ToString();
            switch(s)
            {
                case "cos": case "sin": case "tan": case "√": case "log": case "ln":
                    if(nmb!="")
                    {
                        MessageBox.Show("ERROR");
                        return;
                    }
                    tStream.Add(s);
                    tStream.Add("(");
                    exp += s;
                    exp += "(";
                    break;
                case "C":
                    tStream.Clear();
                    nmb = "";
                    exp = "";
                    break;
                case "1/x":
                    if(nmb!="")
                    {
                        MessageBox.Show("ERROR");
                        return;
                    }
                    tStream.Add(1);
                    tStream.Add("/");
                    exp += "1/";
                    break;
                case "Del":
                    exp = exp.Remove(exp.Length - 1);
                    if (nmb != "")
                        nmb = nmb.Remove(nmb.Length - 1);
                    else
                        tStream.DelLast();
                    break;
                case "x^2":
                    if(nmb!="")
                    {
                        tStream.Add(Convert.ToDouble(nmb));
                        nmb = "";
                    }
                    tStream.Add("^");
                    tStream.Add(2);
                    exp += "^2";
                    break;
                case "10^x":
                    if(nmb!="")
                    {
                        MessageBox.Show("ERROR");
                        return;
                    }
                    tStream.Add(10);
                    tStream.Add("^");
                    exp += "10^";
                    break;
                case "Exp":
                    if(nmb=="")
                    {
                        MessageBox.Show("ERROR");
                        return;
                    }
                    tStream.Add(Convert.ToDouble(nmb));
                    nmb = "";
                    tStream.Add(s);
                    exp += "E";
                    break;
                case "e": case "pi":
                    if (nmb == "")
                        tStream.Add(s);
                    else
                    {
                        tStream.Add("*");
                        tStream.Add(s);
                    }
                    exp += s;
                    break;
                case "x^y":
                    if(nmb!="")
                    {
                        tStream.Add(Convert.ToDouble(nmb));
                        nmb = "";
                    }
                    exp += "^";
                    tStream.Add("^");
                    break;
                case "=":
                    if (nmb != "")
                        tStream.Add(Convert.ToDouble(nmb));
                    tStream.Add(s);
                    Result r = calculator.Calc(tStream);
                    exp += "=";
                    if(r.res=="OK")
                    {
                        this.UpTB.Text = exp;
                        this.DownTB.Text = r.value.ToString();
                    }
                    else
                    {
                        this.UpTB.Text = this.DownTB.Text = "";
                        MessageBox.Show(r.res);
                    }
                    exp = "";
                    nmb = "";
                    tStream.Clear();
                    return;
                default:
                    if(nmb!="")
                    {
                        tStream.Add(Convert.ToDouble(nmb));
                        nmb = "";
                    }
                    tStream.Add(s);
                    exp += s;
                    break;
            }
            this.DownTB.Text = exp;
        }


        private void NumberClick(object sender,RoutedEventArgs args)
        {
            Button button = (Button)sender;
            nmb += button.Content.ToString();
            exp += button.Content.ToString();
            this.DownTB.Text = exp;
        }



        private string exp;
        private string nmb;
        private TokenStream tStream;
        Caclulator calculator;
    }


    public partial class Token
    {
        public Type type;
        public double value;
        public Token(Type t)
        {
            type = t;
            value = 0;
        }
        public Token(double v)
        {
            type = Type.Number;
            value = v;
        }
    }


    public partial class TokenStream
    {
        private List<Token> tokens;
        private int index;
        public TokenStream()
        {
            tokens = new List<Token>();
            index = 0;
        }
        public void DelLast()
        {
            tokens.RemoveAt(tokens.Count - 1);
        }
        public void Add(string s)
        {
            switch (s)
            {
                case "^":
                    tokens.Add(new Token(Type.XpowY));
                    break;
                case "sin":
                    tokens.Add(new Token(Type.sin));
                    break;
                case "cos":
                    tokens.Add(new Token(Type.cos));
                    break;
                case "tan":
                    tokens.Add(new Token(Type.tan));
                    break;
                case "√":
                    tokens.Add(new Token(Type.sqrt));
                    break;
                case "log":
                    tokens.Add(new Token(Type.log));
                    break;
                case "Exp":
                    tokens.Add(new Token(Type.Exp));
                    break;
                case "Mod":
                    tokens.Add(new Token(Type.Mod));
                    break;
                case "e":
                    tokens.Add(new Token(Math.E));
                    break;
                case "ln":
                    tokens.Add(new Token(Type.ln));
                    break;
                case "/":
                    tokens.Add(new Token(Type.Divide));
                    break;
                case "*":
                    tokens.Add(new Token(Type.Multiply));
                    break;
                case "+":
                    tokens.Add(new Token(Type.Plus));
                    break;
                case "-":
                    tokens.Add(new Token(Type.Minus));
                    break;
                case "pi":
                    tokens.Add(new Token(Math.PI));
                    break;
                case "!":
                    tokens.Add(new Token(Type.fact));
                    break;
                case "(":
                    tokens.Add(new Token(Type.LeftS));
                    break;
                case ")":
                    tokens.Add(new Token(Type.RightS));
                    break;
                case "=":
                    tokens.Add(new Token(Type.End));
                    break;
                default:
                    tokens.Add(new Token(Convert.ToDouble(s)));
                    break;
            }
        }
        public void Add(double d)
        {
            tokens.Add(new Token(d));
        }
        public Token GetToken()
        {
            ++index;
            return tokens[index - 1];
        }
        public void PutBack()
        {
            --index;
        }
        public void Clear()
        {
            tokens.Clear();
            index = 0;
        }
    }

    public partial class Result
    {
        public string res;
        public double value;
        public Result(double v)
        {
            res = "OK";
            value = v;
        }
        public Result(string s)
        {
            res = s;
            value = 0;
        }
        public Result()
        {
            res = "";
            value = 0;
        }
    }


    public partial class Caclulator
    {
        private TokenStream tokenStream;
        private Result rslt;
        public Caclulator()
        {
            tokenStream = new TokenStream();
            rslt = new Result();
        }
        public Result Calc(TokenStream ts)
        {
            tokenStream = ts;
            rslt.res = "OK";
            rslt.value = Expression();
            ts.Clear();
            return rslt;
        }
        private double Primary()
        {
            Token t = tokenStream.GetToken();
            if(IsFunction(t.type))
            {
                Type f = t.type;
                t = tokenStream.GetToken();
                if(t.type!=Type.LeftS)
                {
                    rslt.res = "ERROR WITH LEFTS";
                    return 0;
                }
                double v = Expression();
                t = tokenStream.GetToken();
                if(t.type!=Type.RightS)
                {
                    rslt.res = "ERROR WITH RIGHTS";
                    return 0;
                }
                return CaclFunction(f, v);
            }
            switch(t.type)
            {
                case Type.Number:
                    return t.value;
                case Type.LeftS:
                    double v = Expression();
                    t = tokenStream.GetToken();
                    if (t.type == Type.RightS)
                        return v;
                    rslt.res = "ERROR WITH RIGHTS IN PRIMARY";
                    return 0;
                default:
                    rslt.res = "ERROR WITH PRIMARY";
                    return 0;
            }
        }
        private double Special()
        {
            double res = Primary();
            while(true)
            {
                Token t = tokenStream.GetToken();
                switch(t.type)
                {
                    case Type.fact:
                        if(IsDouble(res))
                        {
                            rslt = new Result("DOUBLE FACTORIAL");
                            return 0;
                        }
                        res = Factorial(res);
                        break;
                    case Type.XpowY:
                        double y = Primary();
                        res = Math.Pow(res, y);
                        break;
                    case Type.Mod:
                        double v = Primary();
                        if(v==0||IsDouble(v)||IsDouble(res))
                        {
                            rslt.res = "ERROR WITH MOD";
                            return 0;
                        }
                        res = res % v;
                        break;
                    default:
                        tokenStream.PutBack();
                        return res;

                }
            }
        }
        private double Term()
        {
            double res = Special();
            while(true)
            {
                Token t = tokenStream.GetToken();
                switch(t.type)
                {
                    case Type.Multiply:
                        res *= Special();
                        break;
                    case Type.Divide:
                        double rgh = Special();
                        if(rgh==0)
                        {
                            rslt = new Result("DIVIDE BY ZERO");
                            return 0;
                        }
                        res /= rgh;
                        break;
                    case Type.Exp:
                        t = tokenStream.GetToken();
                        if(t.type!=Type.Number)
                        {
                            rslt.res = "ERROR WITH EXP";
                            return 0;
                        }
                        if(IsDouble(t.value))
                        {
                            rslt.res = "ERROR WITH dEXP";
                            return 0;
                        }
                        res *= Math.Pow(10, t.value);
                        break;
                    default:
                        tokenStream.PutBack();
                        return res;
                }
            }
        }
        private double Expression()
        {
            double res = Term();
            while(true)
            {
                Token t = tokenStream.GetToken();
                switch(t.type)
                {
                    case Type.Plus:
                        res += Term();
                        break;
                    case Type.Minus:
                        res -= Term();
                        break;
                    default:
                        tokenStream.PutBack();
                        return res;
                }
            }
        }
        private bool IsDouble(double v)
        {
            string s = v.ToString();
            foreach(char ch in s)
            {
                if (ch == ',')
                    return true;
            }
            return false;
        }
        private double Factorial(double v)
        {
            if (v == 0 || v == 1)
                return 1;
            double res = 1;
            for(;v>=1;--v)
            {
                res *= v;
            }
            return res;
        }
        private bool IsFunction(Type type)
        {
            switch(type)
            {
                case Type.sin: case Type.cos: case Type.ln: case Type.log: case Type.tan: case Type.sqrt:
                    return true;
            }
            return false;
        }
        private double CaclFunction(Type t,double v)
        {
            switch(t)
            {
                case Type.sin:
                    return Math.Sin(v);
                case Type.cos:
                    return Math.Cos(v);
                case Type.ln:
                    if (v < 0)
                    {
                        rslt.res = "LN<0";
                        return 0;
                    }
                    return Math.Log(v);
                case Type.log:
                    if (v < 0)
                    {
                        rslt.res = "LOG<0";
                        return 0;
                    }
                    return Math.Log10(v);
                case Type.tan:
                    return Math.Tan(v);
                case Type.sqrt:
                    if(v<0)
                    {
                        rslt.res = "SQRT<0";
                        return 0;
                    }
                    return Math.Sqrt(v);
                default:
                    rslt.res = "ERROR WITH CALC FUNCTION";
                    return 0;
            }
        }
    }
}
