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

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for MBBFunctions.xaml
    /// </summary>
    public partial class MBBFunctions : UserControl
    {
        public delegate void EventSelectFunction(int mode_app);
        private EventSelectFunction eventSelectFunction;


        public MBBFunctions()
        {
            InitializeComponent();
        }
        /// <summary>
        /// In base alle funzioni che voglio vedere posiziona i bottoni circolarmente
        /// </summary>
        /// <param name="visF0"></param>
        /// <param name="visF1"></param>
        /// <param name="visF2"></param>
        /// <param name="visF3"></param>
        /// <param name="visF4"></param>
        public void SetVisibleFunctions(bool visF0, bool visF1, bool visF2, bool visF3, bool visF4)
        {
            List<Tuple<double, double>> lstpos = new List<Tuple<double, double>>();
            lstpos.Add(new Tuple<double, double>(149,-15));
            lstpos.Add(new Tuple<double, double>(200,0));
            lstpos.Add(new Tuple<double, double>(235,40));
            lstpos.Add(new Tuple<double, double>(245,92));
            lstpos.Add(new Tuple<double, double>(235,144));

            List<Tuple<int, Canvas>> lst = new List<Tuple<int, Canvas>>();
            int idx = 0;
            if (visF0) lst.Add(new Tuple<int, Canvas>(idx++, F0));
            if (visF1) lst.Add(new Tuple<int, Canvas>(idx++, F1));
            if (visF2) lst.Add(new Tuple<int, Canvas>(idx++, F2));
            if (visF3) lst.Add(new Tuple<int, Canvas>(idx++, F3));
            if (visF4) lst.Add(new Tuple<int, Canvas>(idx++, F4));

            foreach(Tuple<int,Canvas> t in lst)
            {
                Canvas.SetTop(t.Item2, lstpos.ElementAt(t.Item1).Item1);
                Canvas.SetLeft(t.Item2, lstpos.ElementAt(t.Item1).Item2);
            }
        }

        public void SetEventSelectFunction(EventSelectFunction evn)
        {
            this.eventSelectFunction = evn;
        }

        private void imgF0_Note_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.eventSelectFunction != null)
                this.eventSelectFunction(0);
        }

        private void imgF1_Docs_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.eventSelectFunction != null)
                this.eventSelectFunction(1);
        }

        private void imgF2_App_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.eventSelectFunction != null)
                this.eventSelectFunction(2);
        }

        private void imgF3_Activity_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.eventSelectFunction != null)
                this.eventSelectFunction(3);
        }

        private void imgF4_Sticky_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.eventSelectFunction != null)
                this.eventSelectFunction(4);
        }
    }
}
