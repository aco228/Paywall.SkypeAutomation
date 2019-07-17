using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Database
{
  public abstract class MysqlDatabaseBase
  {
    private MySqlConnection _connection = null;
    private MySqlCommand _command = null;
    private MySqlDataReader _reader = null;
    private MySqlTransaction _transaction = null;

    private string _databaseName = string.Empty;
    private int _timeout = -1;

    private bool _error = false;
    private bool _connected = false;
    private string _connectionString = string.Empty;
    private string _lastErrorMessage = string.Empty;

    public bool IsConnected { get { return this._connected; } }

    public MysqlDatabaseBase(string databaseName)
    {
      this._databaseName = databaseName;
    }

    // SUMMARY: Setup connection string from child class
    protected void SetConnectionString(string connectionString)
    {
      this._connectionString = connectionString;
    }

    // SUMMARY: Open Database connection
    protected bool Connect()
    {
      this._lastErrorMessage = string.Empty;
      this._error = false;

      if (string.IsNullOrEmpty(this._connectionString))
      {
        this._error = true;
        this._connectionString = "Connection string is empty";
        return this._error;
      }

      if (this._connected || this._connection != null)
      {
        this._error = true;
        this._connectionString = "Connection exists";
        return this._error;
      }

      try
      {
        this._connection = new MySqlConnection(this._connectionString);
        this._connection.Open();
        this._error = false;
        this._connected = true;
        return this._error;
      }
      catch (Exception e)
      {
        this._error = false;
        this._lastErrorMessage = e.Message;
      }
      return this._error;
    }

    // SUMMARY: Close Database connection
    protected bool Disconnect()
    {
      this._lastErrorMessage = string.Empty;
      this._error = false;

      if (!this._connected)
      {
        this._error = true;
        this._lastErrorMessage = "Connection is not open";
        return this._error;
      }

      if (string.IsNullOrEmpty(this._connectionString))
      {
        this._error = true;
        this._lastErrorMessage = "Connection string is empty";
        return this._error;
      }

      try
      {
        this._connection.Close();
        this._connection = null;
        this._connected = false;
        return this._error;
      }
      catch (Exception e)
      {
        this._error = false;
        this._lastErrorMessage = e.Message;
      }
      return this._error;
    }

    // SUMMARY: Shared compose command
    protected string ComposeCommand(string command)
    {
      // return "START TRANSACTION; " + command + " COMMIT;";
      return command;
    }

    // SUMMARY: Manualy set timeout
    protected void SetTimeout(int timeout)
    {
      this._timeout = timeout;
    }

    // SUMMARY: Load table from database
    public DataTable Load(string command)
    {
      if (this.Connect()) return null;
      command = this.ComposeCommand(command);

      try
      {
        this._command = new MySqlCommand(command, this._connection);
        this._transaction = this._connection.BeginTransaction(IsolationLevel.ReadCommitted);
        this._command.Transaction = this._transaction;
        if (this._timeout != -1) this._command.CommandTimeout = this._timeout;
        MySqlDataAdapter adapter = new MySqlDataAdapter(this._command);
        DataTable table = new DataTable();
        adapter.Fill(table);

        this._transaction.Commit();
        adapter.Dispose();
        this.Disconnect();
        return table;
      }
      catch (Exception e)
      {
        this._lastErrorMessage = e.Message;
        return null;
      }
    }

    // SUMMARY: Execute command in dataabse
    public void Execute(string command)
    {
      if (this.Connect()) return;
      command = this.ComposeCommand(command);

      try
      {
        this._command = new MySqlCommand(command, this._connection);
        this._transaction = this._connection.BeginTransaction(IsolationLevel.ReadCommitted);
        this._command.Transaction = this._transaction;
        if (this._timeout != -1) this._command.CommandTimeout = this._timeout;

        this._command.ExecuteNonQuery();

        this._transaction.Commit();
        this.Disconnect();
        return;
      }
      catch (Exception e)
      {
        this._lastErrorMessage = e.Message;
        return;
      }
    }

    public string Date(DateTime date)
    {
      return date.ToString("yyyy-MM-dd HH:mm:ss");
    }

  }
}
