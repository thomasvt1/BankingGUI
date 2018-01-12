using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace BankingClientV1
{
    /// <summary>
    /// Interaction logic for NoArduino.xaml
    /// </summary>
    public partial class NoArduino : Window
    {
        public NoArduino()
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

            if (System.Diagnostics.Debugger.IsAttached)
            {
                button.Visibility = Visibility.Visible;
            }



        }

        float a = 1;
        Boolean asc = false;

        private void MagicColorChanger()
        {
            Thread.Sleep(10);

            

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                buitenwerking.Foreground = new SolidColorBrush(Rainbow(a));
                button.Foreground = new SolidColorBrush(Rainbow(a - (float) -1.3));
            }));

            a = a + (float)0.001;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
            Application.Current.Shutdown();
        }
    }
}
