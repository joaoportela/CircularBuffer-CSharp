using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameInputDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CircularBuffer.CircularBuffer<UserActions> _circularBuffer;
        System.Timers.Timer _popTimer;
        bool _programStarted;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void _popTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (_circularBuffer.Front() != null)
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    OutputTextBox.Text += _circularBuffer.Front() + "\n";
                    _circularBuffer.PopFront();
                    UpdateBufferBox();
                });
            }
        }

        private void Lightning_Click(object sender, RoutedEventArgs e)
        {
            pushBackUpdateBuffer(UserActions.Lightning);

        }

        private void FireButton_Click(object sender, RoutedEventArgs e)
        {
            pushBackUpdateBuffer(UserActions.Fire);

        }

        private void EarthButton_Click(object sender, RoutedEventArgs e)
        {
            pushBackUpdateBuffer(UserActions.Earth);
        }

        private void pushBackUpdateBuffer(UserActions userActions)
        {
            if (_programStarted)
            {
                if (_circularBuffer.IsEmpty)
                {
                    _circularBuffer.PushBack(userActions);
                    UpdateBufferBox();
                }
                else if (_circularBuffer.Back() != userActions)
                {
                    _circularBuffer.PushBack(userActions);
                    UpdateBufferBox();
                }
            }
        }

        private void UpdateBufferBox()
        {
            BufferTextbox.Clear();
            foreach (var item in _circularBuffer.ToArray())
            {
                BufferTextbox.Text += item.ToString() + "\n";
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D1)
                pushBackUpdateBuffer(UserActions.Fire);
            else if (e.Key == Key.D2)
                pushBackUpdateBuffer(UserActions.Lightning);
            else if (e.Key == Key.D3)
                pushBackUpdateBuffer(UserActions.Earth);
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(BufferSizeTxt.Text, out int bufferSize))
            {
                _circularBuffer = new CircularBuffer.CircularBuffer<UserActions>(bufferSize);
            }
            else
            {
                _circularBuffer = new CircularBuffer.CircularBuffer<UserActions>(4);
                BufferSizeTxt.Text = 4.ToString();
            }

            if (int.TryParse(UpdateTimeTxt.Text, out int updateTime))
            {
                _popTimer = new System.Timers.Timer(updateTime);
            }
            else
            {
                _popTimer = new System.Timers.Timer(500);
                UpdateTimeTxt.Text = 500.ToString();
            }
            _popTimer.AutoReset = true;
            _popTimer.Elapsed += _popTimer_Elapsed;
            _popTimer.Start();

            _programStarted = true;
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_programStarted)
            {
                _popTimer.Stop();
                _programStarted = false;
                OutputTextBox.Clear();
            }
        }
    }
}