using DataBase.Commands;
using Mono.Data.Sqlite;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace DataBaseManagement
{
	public class DataBaseCommandExecutor : IDisposable
	{
		private readonly object _lock = new object();

		private SqliteConnection _connection;
		private Stream _databaseStream;

		public bool IsOpen
		{
			get;
			private set;
		}

		public void Dispose()
		{
			lock(_lock)
			{
				_connection.Close();
				_connection.Dispose();
				_databaseStream.Close();
				IsOpen = false;
			}
		}

		public void OpenConnection(string pathToDatabase)
		{
			lock(_lock)
			{
				_connection = new SqliteConnection($"URI=file:{pathToDatabase};");
				if(!File.Exists(pathToDatabase))
				{
					File.Create(pathToDatabase).Dispose();
				}

				_databaseStream = File.Open(pathToDatabase, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				_connection.Open();
				IsOpen = true;
			}
		}

		public UniTask OpenConnectionAsync(string pathToDatabase)
		{
			return UniTask.RunOnThreadPool(() => OpenConnection(pathToDatabase));
		}

		public int ExecuteNonQuery(ICommand command)
		{
			lock(_lock)
			{
				if(!IsOpen)
				{
					throw new InvalidOperationException("Database is not open");
				}

				using var sqliteCommand = CreateCommand(command);
				return sqliteCommand.ExecuteNonQuery();
			}
		}

		public UniTask<int> ExecuteNonQueryAsync(ICommand command)
		{
			return UniTask.RunOnThreadPool(() => ExecuteNonQuery(command));
		}

		public object ExecuteScalar(ICommand command)
		{
			lock(_lock)
			{
				if(!IsOpen)
				{
					throw new InvalidOperationException("Database is not open");
				}

				using var sqliteCommand = CreateCommand(command);
				return sqliteCommand.ExecuteScalar();
			}
		}

		public UniTask<object> ExecuteScalarAsync(ICommand command)
		{
			return UniTask.RunOnThreadPool(() => ExecuteScalar(command));
		}

		public void ExecuteReader(ICommand command, Action<SqliteDataReader> readAction)
		{
			lock(_lock)
			{
				if(!IsOpen)
				{
					throw new InvalidOperationException("Database is not open");
				}

				using var sqliteCommand = CreateCommand(command);
				using var reader = sqliteCommand.ExecuteReader();
				readAction?.Invoke(reader);
			}
		}

		public UniTask ExecuteReaderAsync(ICommand command, Action<SqliteDataReader> readAction)
		{
			return UniTask.RunOnThreadPool(() => ExecuteReader(command, readAction));
		}

		private SqliteCommand CreateCommand(ICommand command)
		{
			var sqliteCommand = _connection.CreateCommand();
			sqliteCommand.CommandText = command.Command();
			return sqliteCommand;
		}
	}
}