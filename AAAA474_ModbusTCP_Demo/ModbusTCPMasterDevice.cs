using NModbus;
using System.Net.Sockets;

namespace AAAA474;

internal class ModbusTCPMasterDevice
{
    private TcpClient? _tcpClient = null;
    private IModbusMaster? _master = null;
    private ModbusFactory _modbusFactory = new ModbusFactory();
    private Log _log;

    public delegate void Log(string msg);

    public ModbusTCPMasterDevice(Log log)
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }

    public bool Connect()
    {
        try {
            _tcpClient = new TcpClient("192.168.222.247", 502);
            _master = _modbusFactory.CreateMaster(_tcpClient);
            _master.Transport.ReadTimeout = 100;
            _master.Transport.WriteTimeout = 100;
            _master.Transport.Retries = 3;
            _master.Transport.WaitToRetryMilliseconds = 100;
            _log($"Verbunden");
            return true;
        } catch { 
            return false; 
        }
    }

    public bool Disconnect()
    {
        if (_master != null) {
            _master.Dispose();
            _master = null;
        }
        if (_tcpClient != null) {
            _tcpClient.Close();
            _tcpClient.Dispose();
            _tcpClient = null;
        }
        _log($"Getrennt");
        return true;
    }

    public bool ReadHoldingRegister(byte slaveAddress, ushort startAddress, out ushort value)
    {
        value = 0;
        if (CheckConnection()) {
            try {
                value = _master.ReadHoldingRegisters(slaveAddress, startAddress, 1)[0];
                _log($"Holding Register {startAddress} gelesen, Wert: {value}");
                return true;
            }
            catch (Exception ex) {
                _log(ex.Message);
            }
        }
        return false;
    }

    public bool ReadInputRegister(byte slaveAddress, ushort startAddress, out ushort value)
    {
        value = 0;
        if (CheckConnection()) {
            try {
                value = _master.ReadInputRegisters(slaveAddress, startAddress, 1)[0];
                _log($"Input Register {startAddress} gelesen, Wert: {value}");
                return true;
            }
            catch (Exception ex) {
                _log(ex.Message);
            }
        }
        return false;
    }

    public bool WriteHoldingRegister(byte slaveAddress, ushort startAddress, ushort value)
    {
        if (CheckConnection()) {
            try {
                _master.WriteSingleRegister(slaveAddress, startAddress, value);
                _log($"Holding Register {startAddress} geschrieben, Wert: {value}");
                return true;
            }
            catch (Exception ex) {
                _log(ex.Message);
            }
        }
        return false;
    }

    public bool ReadCoil(byte slaveAddress, ushort startAddress, out bool value)
    {
        value = false;
        if (CheckConnection()) {
            try {
                value = _master.ReadCoils(slaveAddress, startAddress, 1)[0];
                _log($"Coil {startAddress} gelesen, Wert: {value}");
                return true;
            }
            catch (Exception ex) {
                _log(ex.Message);
            }
        }
        return false;
    }

    public bool ReadDiscreteInput(byte slaveAddress, ushort startAddress, out bool value)
    {
        value = false;
        if (CheckConnection()) {
            try {
                value = _master.ReadInputs(slaveAddress, startAddress, 1)[0];
                _log($"Discrete Input {startAddress} gelesen, Wert: {value}");
                return true;
            }
            catch (Exception ex) {
                _log(ex.Message);
            }
        }
        return false;
    }

    public bool WriteCoil(byte slaveAddress, ushort startAddress, bool value)
    {
        if (CheckConnection()) {
            try {
                _master.WriteSingleCoil(slaveAddress, startAddress, value);
                _log($"Coil Register {startAddress} geschrieben, Wert: {value}");
                return true;
            }
            catch (Exception ex) {
                _log(ex.Message);
            }
        }
        return false;
    }

    private bool CheckConnection()
    {
        if (_tcpClient != null && _tcpClient.Connected && _master != null) {
            return true;
        } else {
            _log($"Nicht verbunden");
        }
        return false;
    }

}
