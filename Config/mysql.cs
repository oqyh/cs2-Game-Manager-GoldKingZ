using MySqlConnector;
using System.Data;
using Game_Manager_GoldKingZ.Config;

namespace Game_Manager_GoldKingZ;

public class MySqlDataManager
{
    public class MySqlConnectionSettings
    {
        public string? MySqlHost { get; set; }
        public string? MySqlDatabase { get; set; }
        public string? MySqlUsername { get; set; }
        public string? MySqlPassword { get; set; }
        public int MySqlPort { get; set; }
    }

    public class PersonData
    {
        public ulong PlayerSteamID { get; set; }
        public int Disable_Chat { get; set; }
        public int Disable_Legs { get; set; }
        public int Disable_Weapons { get; set; }
        public DateTime DateAndTime { get; set; }
    }

    public static async Task CreatePersonDataTableIfNotExistsAsync(MySqlConnection connection)
    {
        string query = @"CREATE TABLE IF NOT EXISTS Game_Manager_PersonData (
                    PlayerSteamID BIGINT UNSIGNED PRIMARY KEY,
                    Disable_Chat INT,
                    Disable_Legs INT,
                    Disable_Weapons INT,
                    DateAndTime DATETIME
                );";

        try
        {
            using (var command = new MySqlCommand(query, connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"======================== ERROR =============================");
            Console.WriteLine($"Error creating Game_Manager_PersonData table: {ex.Message}");
            Console.WriteLine($"======================== ERROR =============================");
            throw;
        }
    }

    public static async Task SaveToMySqlAsync(ulong PlayerSteamID, int Disable_Chat, int Disable_Legs, int Disable_Weapons, DateTime DateAndTime, MySqlConnection connection, MySqlConnectionSettings connectionSettings)
    {
        int days = Configs.GetConfigData().Toggle_AutoRemovePlayerMySqlOlderThanXDays;
        string deleteOldRecordsQuery = $"DELETE FROM Game_Manager_PersonData WHERE DateAndTime < NOW() - INTERVAL {days} DAY";

        string insertOrUpdateQuery = @"
        INSERT INTO Game_Manager_PersonData (PlayerSteamID, Disable_Chat, Disable_Legs, Disable_Weapons, DateAndTime)
        VALUES (@PlayerSteamID, @Disable_Chat, @Disable_Legs, @Disable_Weapons, @DateAndTime)
        ON DUPLICATE KEY UPDATE 
            Disable_Chat = VALUES(Disable_Chat), 
            Disable_Legs = VALUES(Disable_Legs),
            Disable_Weapons = VALUES(Disable_Weapons), 
            DateAndTime = VALUES(DateAndTime)";

        try
        {
            using (var deleteCommand = new MySqlCommand(deleteOldRecordsQuery, connection))
            {
                await deleteCommand.ExecuteNonQueryAsync();
            }

            using (var command = new MySqlCommand(insertOrUpdateQuery, connection))
            {
                command.Parameters.AddWithValue("@PlayerSteamID", PlayerSteamID);
                command.Parameters.AddWithValue("@Disable_Chat", Disable_Chat);
                command.Parameters.AddWithValue("@Disable_Legs", Disable_Legs);
                command.Parameters.AddWithValue("@Disable_Weapons", Disable_Weapons);
                command.Parameters.AddWithValue("@DateAndTime", DateAndTime);

                await command.ExecuteNonQueryAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"======================== ERROR =============================");
            Console.WriteLine($"Error saving data to MySQL: {ex.Message}");
            Console.WriteLine($"======================== ERROR =============================");
            throw;
        }
    }
    public static async Task RemoveFromMySqlAsync(ulong PlayerSteamID, MySqlConnection connection, MySqlConnectionSettings connectionSettings)
    {
        try
        {
            await connection.OpenAsync();

            string query = @"DELETE FROM Game_Manager_PersonData WHERE PlayerSteamID = @PlayerSteamID";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PlayerSteamID", PlayerSteamID);

                await command.ExecuteNonQueryAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing data from MySQL: {ex.Message}");
            throw;
        }
        finally
        {
            if (connection.State != ConnectionState.Closed)
                connection.Close();
        }
    }

    public static async Task<PersonData> RetrievePersonDataByIdAsync(ulong targetId, MySqlConnection connection)
    {
        int days = Configs.GetConfigData().Toggle_AutoRemovePlayerMySqlOlderThanXDays;
        string deleteOldRecordsQuery = $"DELETE FROM Game_Manager_PersonData WHERE DateAndTime < NOW() - INTERVAL {days} DAY";

        string retrieveQuery = "SELECT * FROM Game_Manager_PersonData WHERE PlayerSteamID = @PlayerSteamID";
        var personData = new PersonData();

        try
        {
            using (var deleteCommand = new MySqlCommand(deleteOldRecordsQuery, connection))
            {
                await deleteCommand.ExecuteNonQueryAsync();
            }

            using (var command = new MySqlCommand(retrieveQuery, connection))
            {
                command.Parameters.AddWithValue("@PlayerSteamID", targetId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        personData = new PersonData
                        {
                            PlayerSteamID = Convert.ToUInt64(reader["PlayerSteamID"]),
                            Disable_Chat = Convert.ToInt32(reader["Disable_Chat"]),
                            Disable_Legs = Convert.ToInt32(reader["Disable_Legs"]),
                            Disable_Weapons = Convert.ToInt32(reader["Disable_Weapons"]),
                            DateAndTime = Convert.ToDateTime(reader["DateAndTime"])
                        };
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"======================== ERROR =============================");
            Console.WriteLine($"Error retrieving data from MySQL: {ex.Message}");
            Console.WriteLine($"======================== ERROR =============================");
            throw;
        }
        return personData;
    }
}