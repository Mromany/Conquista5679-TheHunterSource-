using System;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Data;
using MySql.Data.Types;
using System.Data.SqlClient;
namespace PhoenixProject.Database
{
    public class MySqlReader
    {
        //private MySqlDataReader Reader = null;
        //  const string Table = "table";
        private MySql.Data.MySqlClient.MySqlConnection _conn = DataHolder.MySqlConnection;
        private DataRow _datarow;
        private DataSet _dataset;
        private int _row;
        const string Table = "table";

        public MySqlReader(MySqlCommand command)
        {
            if (command.Type == MySqlCommandType.SELECT)
            {
                TryFill(command);
            }
        }
        private MySql.Data.MySqlClient.MySqlConnection SelectConnection()
        {
            return DataHolder.MySqlConnection;
        }
        private string _lasterror = null;
        public string LastError
        {
            get
            {
                return _lasterror;
            }
            set
            {
                _lasterror = value;
            }
        }
        private void TryFill(MySqlCommand command)
        {
            MySql.Data.MySqlClient.MySqlConnection connection = SelectConnection();
            MySqlDataAdapter DataAdapter = null;
            if (connection.State == ConnectionState.Open)
            {
                while (_dataset == null && (_lasterror == null || _lasterror.Contains("connection")))
                {
                    if (_lasterror != null && _lasterror.Contains("connection"))
                        connection = SelectConnection();
                    DataAdapter = new MySqlDataAdapter(command.Command, connection);
                    _dataset = new DataSet();
                    try
                    {
                        DataAdapter.Fill(_dataset, Table);
                    }
                    catch
                    {
                    }
                    _row = 0;
                }
            }
            DataAdapter = new MySqlDataAdapter(command.Command, connection);
            _dataset = new DataSet();
            try
            {
                DataAdapter.Fill(_dataset, Table);
            }
            catch
            {
            }
            _row = 0;

        }
        public bool Read()
        {
            if (_dataset.Tables[Table].Rows.Count > _row)
            {
                _datarow = _dataset.Tables[Table].Rows[_row];
                _row++;
                return true;
            }
            _row++;
            return false;
        }
        public void Dispose()
        {
        }
        public void Close()
        {
        }
        public sbyte ReadSByte(string columnName)
        {
            sbyte result = 0;
            sbyte.TryParse(_datarow[columnName].ToString(), out result);
            return result;
        }
        public byte ReadByte(string columnName)
        {
            byte result = 0;
            byte.TryParse(_datarow[columnName].ToString(), out result);
            return result;
        }
        public short ReadInt16(string columnName)
        {
            short result = 0;
            short.TryParse(_datarow[columnName].ToString(), out result);
            return result;
        }
        public ushort ReadUInt16(string columnName)
        {
            ushort result = 0;
            ushort.TryParse(_datarow[columnName].ToString(), out result);
            return result;
        }
        public int ReadInt32(string columnName)
        {
            int result = 0;
            int.TryParse(_datarow[columnName].ToString(), out result);
            return result;
        }
        public uint ReadUInt32(string columnName)
        {
            uint result = 0;
            uint.TryParse(_datarow[columnName].ToString(), out result);
            return result;
        }
        public long ReadInt64(string columnName)
        {
            long result = 0;
            long.TryParse(_datarow[columnName].ToString(), out result);
            return result;
        }
        public ulong ReadUInt64(string columnName)
        {
            ulong result = 0;
            ulong.TryParse(_datarow[columnName].ToString(), out result);
            return result;
        }
        public string ReadString(string columnName)
        {
            string result = "";
            result = _datarow[columnName].ToString();
            return result;
        }
        public bool ReadBoolean(string columnName)
        {
            bool result = false;
            string str = _datarow[columnName].ToString();
            if (str[0] == '1') return true;
            if (str[0] == '0') return false;

            bool.TryParse(_datarow[columnName].ToString(), out result);
            return result;
        }

    }
}