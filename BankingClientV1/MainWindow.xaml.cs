using Newtonsoft.Json.Linq;
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
using System.Threading;

// Wij zijn groepje 21

namespace BankingClientV1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Console.WriteLine("Starting a");

            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    MagicColorChanger();
                }

            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        float a = 1;
        Boolean asc = false;

        private void MagicColorChanger()
        {
            Thread.Sleep(500);



            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Brush SpeedColor = new SolidColorBrush(Rainbow(a));
            }));

            if (a < 0 || a > 1)
                asc = !asc;

            if (asc)
                a = a + (float)0.01;
            else
                a = a - (float)0.01;
        }


        private Color Rainbow(float progress)
        {
            float div = (Math.Abs(progress % 1) * 6);
            int ascending = (int)((div % 1) * 255);
            int descending = 255 - ascending;

            switch ((int)div)
            {
                case 0:
                    return Color.FromArgb(255, 255, Convert.ToByte(ascending), 0);
                case 1:
                    return Color.FromArgb(255, Convert.ToByte(descending), 255, 0);
                case 2:
                    return Color.FromArgb(255, 0, 255, Convert.ToByte(ascending));
                case 3:
                    return Color.FromArgb(255, 0, Convert.ToByte(descending), 255);
                case 4:
                    return Color.FromArgb(255, Convert.ToByte(ascending), 0, 255);
                default: // case 5:
                    return Color.FromArgb(255, 255, 0, Convert.ToByte(descending));
            }
        }
    }
}