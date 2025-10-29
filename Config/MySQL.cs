using MySqlConnector;
using Game_Manager_GoldKingZ.Config;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Game_Manager_GoldKingZ;

public class MySqlDataManager
{
    private static readonly object _loadLock = new object();
    private static bool _isSending = false;
    
    private static readonly Dictionary<string, string> CurrentTableSchema = new Dictionary<string, string>
    {
        {"PlayerSteamID", "BIGINT UNSIGNED PRIMARY KEY"},
        {"Toggle_AimPunch", "INT NOT NULL DEFAULT 0"},
        {"Toggle_Custom_MuteSounds1", "INT NOT NULL DEFAULT 0"},
        {"Toggle_Custom_MuteSounds2", "INT NOT NULL DEFAULT 0"},
        {"Toggle_Custom_MuteSounds3", "INT NOT NULL DEFAULT 0"},
        {"DateAndTime", "DATETIME NOT NULL"}
    };

    private static List<MySqlConnectionStringBuilder> GetConnectionStrings()
    {
        var connectionStrings = new List<MySqlConnectionStringBuilder>();
        var config = Configs.Instance.MySql_Config;

        if (config.MySql_Servers == null || config.MySql_Servers.Count == 0) return connectionStrings;

        foreach (var serverConfig in config.MySql_Servers)
        {
            try
            {
                var builder = new MySqlConnectionStringBuilder
                {
                    Server = serverConfig.Server,
                    Port = (uint)serverConfig.Port,
                    Database = serverConfig.Database,
                    UserID = serverConfig.Username,
                    Password = serverConfig.Password,
                    ConnectionTimeout = (uint)Configs.Instance.MySql_ConnectionTimeout,
                    Pooling = true,
                    MinimumPoolSize = 0,
                    MaximumPoolSize = 100,
                };

                connectionStrings.Add(builder);
                Helper.DebugMessage($"Configured MySQL server: {serverConfig.Server}:{serverConfig.Port} with database: {serverConfig.Database}", Configs.Instance.EnableDebug.ToDebugConfig(1));
            }
            catch (Exception ex)
            {
                Helper.DebugMessage($"Error configuring MySQL server {serverConfig.Server}:{serverConfig.Port}: {ex.Message}", 0);
            }
        }

        return connectionStrings;
    }

    public static async Task<MySqlConnection?> GetConnectionAsync()
    {
        var connectionStrings = GetConnectionStrings();
        if (connectionStrings.Count == 0)
        {
            Helper.DebugMessage("No MySQL servers configured", Configs.Instance.EnableDebug.ToDebugConfig(1));
            return null;
        }

        Exception? lastException = null;

        for (int attempt = 0; attempt < Configs.Instance.MySql_RetryAttempts; attempt++)
        {
            if (attempt > 0)
            {
                Helper.DebugMessage($"Retry attempt {attempt + 1} of {Configs.Instance.MySql_RetryAttempts}", Configs.Instance.EnableDebug.ToDebugConfig(1));
            }

            foreach (var connectionStringBuilder in connectionStrings)
            {
                try
                {
                    var connection = new MySqlConnection(connectionStringBuilder.ConnectionString);
                    
                    using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(Configs.Instance.MySql_ConnectionTimeout));
                    await connection.OpenAsync(cancellationTokenSource.Token);
                    
                    await using var testCmd = new MySqlCommand("SELECT 1", connection);
                    await testCmd.ExecuteScalarAsync();
                    
                    Helper.DebugMessage($"Successfully connected to MySQL server: {connectionStringBuilder.Server}:{connectionStringBuilder.Port}", Configs.Instance.EnableDebug.ToDebugConfig(1));
                    return connection;
                }
                catch (OperationCanceledException ex)
                {
                    lastException = ex;
                    Helper.DebugMessage($"Connection timeout to {connectionStringBuilder.Server}:{connectionStringBuilder.Port}", 0);
                }
                catch (MySqlException ex)
                {
                    lastException = ex;
                    Helper.DebugMessage($"MySQL error connecting to {connectionStringBuilder.Server}:{connectionStringBuilder.Port} - {ex.Message}", 0);
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    Helper.DebugMessage($"Failed to connect to {connectionStringBuilder.Server}:{connectionStringBuilder.Port} - {ex.Message}", 0);
                }
            }

            if (attempt < Configs.Instance.MySql_RetryAttempts - 1)
            {
                Helper.DebugMessage($"Waiting {Configs.Instance.MySql_RetryDelay}s before next retry...", Configs.Instance.EnableDebug.ToDebugConfig(1));
                await Task.Delay(TimeSpan.FromSeconds(Configs.Instance.MySql_RetryDelay));
            }
        }

        Helper.DebugMessage($"Failed to connect to any MySQL server after {Configs.Instance.MySql_RetryAttempts} attempts", 0);
        return null;
    }

    public static async Task<bool> CreateTableIfNotExistsAsync()
    {
        lock (_loadLock)
        {
            if (_isSending)
            {
                return false;
            }
            _isSending = true;
        }

        try
        {
            await using var connection = await GetConnectionAsync();
            if (connection == null)
            {
                Helper.DebugMessage("Cannot create table - no MySQL connection available", 0);
                return false;
            }

            bool tableExists;
            await using (var checkCmd = new MySqlCommand("SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = 'Game_Manager_PersonData'", connection))
            {
                tableExists = Convert.ToInt32(await checkCmd.ExecuteScalarAsync()) > 0;
            }

            if (!tableExists)
            {
                await CreateCompleteTableAsync(connection);
                Helper.DebugMessage("Database table created successfully with all columns", Configs.Instance.EnableDebug.ToDebugConfig(1));
                return true;
            }
            else
            {
                await CheckAndUpdateTableSchemaAsync(connection);
                Helper.DebugMessage("Database table schema verified and updated if needed", Configs.Instance.EnableDebug.ToDebugConfig(1));
                return true;
            }
        }
        catch (Exception ex)
        {
            Helper.DebugMessage($"DB Init Error: {ex.Message}", 0);
            return false;
        }
        finally
        {
            lock (_loadLock)
            {
                _isSending = false;
            }
        }
    }

    private static async Task CreateCompleteTableAsync(MySqlConnection connection)
    {
        var columns = new List<string>();
        foreach (var column in CurrentTableSchema)
        {
            columns.Add($"{column.Key} {column.Value}");
        }

        string createTableQuery = $@"
            CREATE TABLE Game_Manager_PersonData (
                {string.Join(",\n                ", columns)}
            )";

        await using var command = new MySqlCommand(createTableQuery, connection);
        await command.ExecuteNonQueryAsync();
    }

    private static async Task CheckAndUpdateTableSchemaAsync(MySqlConnection connection)
    {
        try
        {
            var existingColumns = new HashSet<string>();
            string getColumnsQuery = @"
                SELECT COLUMN_NAME 
                FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_SCHEMA = DATABASE() 
                AND TABLE_NAME = 'Game_Manager_PersonData'";

            await using (var cmd = new MySqlCommand(getColumnsQuery, connection))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    existingColumns.Add(reader.GetString("COLUMN_NAME"));
                }
            }

            foreach (var requiredColumn in CurrentTableSchema)
            {
                if (!existingColumns.Contains(requiredColumn.Key))
                {
                    Helper.DebugMessage($"Adding missing column: {requiredColumn.Key}", Configs.Instance.EnableDebug.ToDebugConfig(1));
                    await AddColumnToTableAsync(connection, requiredColumn.Key, requiredColumn.Value);
                }
            }
        }
        catch (Exception ex)
        {
            Helper.DebugMessage($"Error checking/updating table schema: {ex.Message}", 0);
        }
    }

    private static async Task AddColumnToTableAsync(MySqlConnection connection, string columnName, string columnDefinition)
    {
        try
        {
            string afterColumn = GetColumnPosition(columnName);
            
            string alterTableQuery = $"ALTER TABLE Game_Manager_PersonData ADD COLUMN {columnName} {columnDefinition} {afterColumn}";
            
            await using var command = new MySqlCommand(alterTableQuery, connection);
            await command.ExecuteNonQueryAsync();
            
            Helper.DebugMessage($"Successfully added column: {columnName}", Configs.Instance.EnableDebug.ToDebugConfig(1));
        }
        catch (Exception ex)
        {
            Helper.DebugMessage($"Error adding column {columnName}: {ex.Message}", 0);
        }
    }

    private static string GetColumnPosition(string columnName)
    {
        var columnOrder = new List<string>
        {
            "PlayerSteamID",
            "Toggle_AimPunch",
            "Toggle_Custom_MuteSounds1",
            "Toggle_Custom_MuteSounds2",
            "Toggle_Custom_MuteSounds3",
            "DateAndTime"
        };

        int index = columnOrder.IndexOf(columnName);
        if (index > 0)
        {
            return $"AFTER {columnOrder[index - 1]}";
        }
        
        return "";
    }

    public static async Task<bool> SaveToMySqlAsync(Globals_Static.PersonData data)
    {
        const string insertOrUpdateQuery = @"
            INSERT INTO Game_Manager_PersonData 
                (PlayerSteamID, Toggle_AimPunch, Toggle_Custom_MuteSounds1, Toggle_Custom_MuteSounds2, Toggle_Custom_MuteSounds3, DateAndTime)
            VALUES 
                (@PlayerSteamID, @Toggle_AimPunch, @Toggle_Custom_MuteSounds1, @Toggle_Custom_MuteSounds2, @Toggle_Custom_MuteSounds3, @DateAndTime)
            ON DUPLICATE KEY UPDATE 
                Toggle_AimPunch = VALUES(Toggle_AimPunch),
                Toggle_Custom_MuteSounds1 = VALUES(Toggle_Custom_MuteSounds1),
                Toggle_Custom_MuteSounds2 = VALUES(Toggle_Custom_MuteSounds2),
                Toggle_Custom_MuteSounds3 = VALUES(Toggle_Custom_MuteSounds3),
                DateAndTime = VALUES(DateAndTime)";
        
        try
        {
            await using var connection = await GetConnectionAsync();
            if (connection == null)
            {
                Helper.DebugMessage("Cannot save data - no MySQL connection available", 0);
                return false;
            }
            
            await using var command = new MySqlCommand(insertOrUpdateQuery, connection);
            AddPersonDataParameters(command, data);
            await command.ExecuteNonQueryAsync();
            return true;
        }
        catch (Exception ex)
        {
            Helper.DebugMessage($"Saving Values In MySql Error: {ex.Message}", 0);
            return false;
        }
    }

    public static async Task<Globals_Static.PersonData> RetrievePersonDataByIdAsync(ulong steamId)
    {
        const string retrieveQuery = "SELECT * FROM Game_Manager_PersonData WHERE PlayerSteamID = @PlayerSteamID";
        try
        {
            await using var connection = await GetConnectionAsync();
            if (connection == null)
            {
                Helper.DebugMessage("Cannot retrieve data - no MySQL connection available", 0);
                return new Globals_Static.PersonData();
            }
            
            await using var command = new MySqlCommand(retrieveQuery, connection);
            command.Parameters.Add("@PlayerSteamID", MySqlDbType.UInt64).Value = steamId;

            await using var reader = await command.ExecuteReaderAsync();
            
            if (await reader.ReadAsync())
            {
                return new Globals_Static.PersonData
                {
                    PlayerSteamID = reader.GetUInt64("PlayerSteamID"),
                    Toggle_AimPunch = reader.GetInt32("Toggle_AimPunch"),
                    Toggle_Custom_MuteSounds1 = reader.GetInt32("Toggle_Custom_MuteSounds1"),
                    Toggle_Custom_MuteSounds2 = reader.GetInt32("Toggle_Custom_MuteSounds2"),
                    Toggle_Custom_MuteSounds3 = reader.GetInt32("Toggle_Custom_MuteSounds3"),
                    DateAndTime = reader.GetDateTime("DateAndTime")
                };
            }
        }
        catch (Exception ex)
        {
            Helper.DebugMessage($"Retrieve Values In MySql Error: {ex.Message}", 0);
        }
        return new Globals_Static.PersonData();
    }

    public static async Task<bool> DeleteOldPlayersAsync()
    {
        if (Configs.Instance.MySql_AutoRemovePlayerOlderThanXDays < 1)
        {
            return false;
        }

        try
        {
            int days = Configs.Instance.MySql_AutoRemovePlayerOlderThanXDays;
            const string cleanupQuery = "DELETE FROM Game_Manager_PersonData WHERE DateAndTime < NOW() - INTERVAL @Days DAY";

            await using var connection = await GetConnectionAsync();
            if (connection == null)
            {
                return false;
            }
            
            await using var cleanupCommand = new MySqlCommand(cleanupQuery, connection);
            cleanupCommand.Parameters.Add("@Days", MySqlDbType.Int32).Value = days;
            var affectedRows = await cleanupCommand.ExecuteNonQueryAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            Helper.DebugMessage($"Delete Old Players In MySql Error: {ex.Message}", 0);
            return false;
        }
    }

    private static void AddPersonDataParameters(MySqlCommand command, Globals_Static.PersonData data)
    {
        command.Parameters.Add("@PlayerSteamID", MySqlDbType.UInt64).Value = data.PlayerSteamID;
        command.Parameters.Add("@Toggle_AimPunch", MySqlDbType.Int32).Value = data.Toggle_AimPunch;
        command.Parameters.Add("@Toggle_Custom_MuteSounds1", MySqlDbType.Int32).Value = data.Toggle_Custom_MuteSounds1;
        command.Parameters.Add("@Toggle_Custom_MuteSounds2", MySqlDbType.Int32).Value = data.Toggle_Custom_MuteSounds2;
        command.Parameters.Add("@Toggle_Custom_MuteSounds3", MySqlDbType.Int32).Value = data.Toggle_Custom_MuteSounds3;
        command.Parameters.Add("@DateAndTime", MySqlDbType.DateTime).Value = data.DateAndTime;
    }
}