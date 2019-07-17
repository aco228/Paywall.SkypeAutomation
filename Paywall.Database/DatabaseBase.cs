using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Database
{
 public abstract class DatabaseBase
  {
    protected SqlConnection _connection = null;
    protected SqlCommand _command = null;
    protected SqlDataReader _reader = null;
    protected SqlTransaction _transaction = null;

    private string _databaseName = string.Empty;
    private string _databaseSchema = string.Empty;
    private int _timeout = -1;

    private bool _error = false;
    private bool _connected = false;
    private string _connectionString = string.Empty;
    private string _lastErrorMessage = string.Empty;

    public bool IsConnected { get { return this._connected; } }

    public DatabaseBase(string databaseName, string schemaName)
    {
      this._databaseName = databaseName;
      this._databaseSchema = schemaName;
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

      try
      {
        this._connection = new SqlConnection(this._connectionString);
        this._connection.Open();
        this._error = false;
        this._connected = true;
        return this._error;
      }
      catch (Exception e)
      {
        this._error = true;
        this._lastErrorMessage = e.Message;
        return this._error;
      }
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
        this._lastErrorMessage = "Connection string is not set up";
        return this._error;
      }

      try
      {
        this._connection.Close();
        this._connected = false;
        return this._error;
      }
      catch (Exception e)
      {
        this._error = true;
        this._lastErrorMessage = e.Message;
        return this._error;
      }
    }

    // SUMMARY: Shared compose command
    protected string ComposeCommand(string command)
    {
      //return "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " + command;
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
        this._command = new SqlCommand(command, this._connection);
        this._transaction = this._connection.BeginTransaction(IsolationLevel.ReadUncommitted);
        this._command.Transaction = this._transaction;
        if (this._timeout != -1)
        {
          if (this._timeout > 30) this._timeout = 30;
          this._command.CommandTimeout = this._timeout;
        }
        SqlDataAdapter adapter = new SqlDataAdapter(this._command);
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

    protected bool Execute(List<string> commands)
    {
      if (this.Connect()) return false;

      try
      {
        this._command = new SqlCommand();
        this._command.Connection = this._connection;
        this._command.CommandType = CommandType.Text;

        this._transaction = this._connection.BeginTransaction(IsolationLevel.ReadUncommitted);
        this._command.Transaction = this._transaction;
        if (this._timeout != -1)
        {
          if (this._timeout > 30) this._timeout = 30;
          this._command.CommandTimeout = this._timeout;
        }

        foreach (string command in commands)
        {
          this._command.CommandText = command;
          this._command.ExecuteNonQuery();
        }

        this._transaction.Commit();
        this.Disconnect();
        return true;
      }
      catch (Exception e)
      {
        this._lastErrorMessage = e.Message;
        return false;
      }
    }

    public string Date(DateTime date)
    {
      return date.ToString("yyyy-MM-dd HH:mm:ss");
    }
  }
}
