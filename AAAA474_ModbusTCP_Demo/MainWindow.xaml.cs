using System.Windows;

namespace AAAA474;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ModbusTCPMasterDevice _modbusTCPMasterDevice;

    public MainWindow()
    {
        _modbusTCPMasterDevice = new ModbusTCPMasterDevice(Log);
        InitializeComponent();
    }

    private void Log(string msg)
    {
        listBoxDebug.Items.Insert(0, msg);
    }

    private void buttonConnect_Click(object sender, RoutedEventArgs e)
    {
        _modbusTCPMasterDevice.Connect();
    }

    private void buttonDisconnect_Click(object sender, RoutedEventArgs e)
    {
        _modbusTCPMasterDevice.Disconnect();
    }

    private void buttonLesenMastHoehe_Click(object sender, RoutedEventArgs e)
    {
        if (_modbusTCPMasterDevice.ReadInputRegister(0, 300, out ushort value)) {
            textBoxMastHoeheIst.Text = $"{value}";
        }
    }

    private void buttonLesenMastHoeheSoll_Click(object sender, RoutedEventArgs e)
    {
        if (_modbusTCPMasterDevice.ReadHoldingRegister(0, 210, out ushort value)) {
            textBoxMastHoeheSoll.Text = $"{value}";
        }
    }

    private void buttonSchreibenMastHoeheSoll_Click(object sender, RoutedEventArgs e)
    {
        if (ushort.TryParse(textBoxMastHoeheSoll.Text, out ushort value)) {
            _modbusTCPMasterDevice.WriteHoldingRegister(0, 210, value);
        }
    }

    private void buttonBefehlPositionierenSet_Click(object sender, RoutedEventArgs e)
    {
        _modbusTCPMasterDevice.WriteCoil(0, 1, true);
    }

    private void buttonBefehlPositionierenClr_Click(object sender, RoutedEventArgs e)
    {
        _modbusTCPMasterDevice.WriteCoil(0, 1, false);
    }

    private void buttonBefehlPositionierenLesen_Click(object sender, RoutedEventArgs e)
    {
        if (_modbusTCPMasterDevice.ReadCoil(0, 1, out bool value)) {
            textBoxBefehlPositionieren.Text = $"{value}";
        }
    }

    private void buttonESUnterePosLesen_Click(object sender, RoutedEventArgs e)
    {
        if (_modbusTCPMasterDevice.ReadDiscreteInput(0, 119, out bool value)) {
            textBoxESUnterPos.Text = $"{value}";
        }
    }

}
