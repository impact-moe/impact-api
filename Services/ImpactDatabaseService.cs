using ImpactApi.Models;
using ImpactApi.Settings;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;
using System;

namespace ImpactApi.Services
{
    public class ImpactDatabaseService
    {
        string _connectionString;

        public ImpactDatabaseService(IImpactDatabaseSettings impactDatabaseSettings)
        {
            _connectionString = "Host=" + impactDatabaseSettings.Host + ";" +
                "Port=" + impactDatabaseSettings.Port + ";" +
                "Database=" + impactDatabaseSettings.Database + ";" +
                "Uid=" + impactDatabaseSettings.Uid + ";" +
                "Password=" + impactDatabaseSettings.Password + ";";
        }

        #region Artifact Operations
        private Artifact ReadArtifact(DbDataReader sqlReader)
        {
            Artifact artifact = new Artifact();

            artifact.Id = sqlReader.GetInt32("ArtifactTable.id");
            artifact.Rarity = sqlReader.GetInt32("ArtifactTable.rarity");
            artifact.Name = sqlReader["ArtifactTable.name"].ToString();
            artifact.Type = sqlReader["ArtifactTable.type"].ToString();
            artifact.Description = sqlReader["ArtifactTable.description"].ToString();
            artifact.Lore = sqlReader["ArtifactTable.lore"].ToString();
            artifact.Location = sqlReader["ArtifactTable.location"].ToString();
            artifact.Image = sqlReader["ArtifactTable.image"].ToString();

            artifact.ArtifactSet = ReadArtifactSet(sqlReader);

            return artifact;
        }
        public async Task<List<Artifact>> GetAllArtifacts()
        {
            List<Artifact> artifacts = new List<Artifact>();
            string command = "SELECT ArtifactTable.id AS 'ArtifactTable.id', " +
                "ArtifactTable.name AS 'ArtifactTable.name', " +
                "ArtifactTable.type AS 'ArtifactTable.type', " +
                "ArtifactTable.rarity AS 'ArtifactTable.rarity', " +
                "ArtifactTable.artifact_set_id AS 'ArtifactTable.artifact_set_id', " +
                "ArtifactTable.description AS 'ArtifactTable.description', " +
                "ArtifactTable.lore AS 'ArtifactTable.lore', " +
                "ArtifactTable.location AS 'ArtifactTable.location', " +
                "ArtifactTable.image AS 'ArtifactTable.image', " +
                "ArtifactSetTable.id AS 'ArtifactSetTable.id', " +
                "ArtifactSetTable.name AS 'ArtifactSetTable.name', " +
                "ArtifactSetTable.max_rarity AS 'ArtifactSetTable.max_rarity', " +
                "ArtifactSetTable.two_piece_bonus AS 'ArtifactSetTable.two_piece_bonus', " +
                "ArtifactSetTable.four_piece_bonus AS 'ArtifactSetTable.four_piece_bonus' " +
                "FROM ImpactDB.ArtifactTable " +
                "JOIN ImpactDB.ArtifactSetTable " +
                "ON ArtifactTable.artifact_set_id = ArtifactSetTable.id ";

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (MySqlCommand sqlCommand = new MySqlCommand(command, sqlConnection))
                using (DbDataReader sqlReader = await sqlCommand.ExecuteReaderAsync())
                    while (await sqlReader.ReadAsync())
                        artifacts.Add(ReadArtifact(sqlReader));
            }

            return artifacts;
        }

        public async Task<Artifact> GetArtifact(string id)
        {
            string command = "SELECT ArtifactTable.id AS 'ArtifactTable.id', " +
                "ArtifactTable.name AS 'ArtifactTable.name', " +
                "ArtifactTable.type AS 'ArtifactTable.type', " +
                "ArtifactTable.rarity AS 'ArtifactTable.rarity', " +
                "ArtifactTable.artifact_set_id AS 'ArtifactTable.artifact_set_id', " +
                "ArtifactTable.description AS 'ArtifactTable.description', " +
                "ArtifactTable.lore AS 'ArtifactTable.lore', " +
                "ArtifactTable.location AS 'ArtifactTable.location', " +
                "ArtifactTable.image AS 'ArtifactTable.image', " +
                "ArtifactSetTable.id AS 'ArtifactSetTable.id', " +
                "ArtifactSetTable.name AS 'ArtifactSetTable.name', " +
                "ArtifactSetTable.max_rarity AS 'ArtifactSetTable.max_rarity', " +
                "ArtifactSetTable.two_piece_bonus AS 'ArtifactSetTable.two_piece_bonus', " +
                "ArtifactSetTable.four_piece_bonus AS 'ArtifactSetTable.four_piece_bonus' " +
                "FROM ImpactDB.ArtifactTable " +
                "JOIN ImpactDB.ArtifactSetTable " +
                "ON ArtifactTable.artifact_set_id = ArtifactSetTable.id " +
                "WHERE (ArtifactTable.id = @id)";

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (MySqlCommand sqlCommand = new MySqlCommand(command, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", id);

                    using (DbDataReader sqlReader = await sqlCommand.ExecuteReaderAsync())
                        if (await sqlReader.ReadAsync())
                            return ReadArtifact(sqlReader);
                }
            }

            return null;
        }
        #endregion

        #region Role Operations
        public async Task<List<Role>> GetCharacterRoles(string characterId, string expand)
        {
            List<Role> roles = new List<Role>();
            List<WeaponPriority> weaponPriorities = await GetWeaponPriorities(characterId, string.Empty);
            List<ArtifactPriority> artifactPriorities = await GetArtifactPriorities(characterId, string.Empty);
            List<MainStatPriority> mainStatPriorities = await GetMainStatPriorities(characterId);
            List<SubStatPriority> subStatPriorities = await GetSubStatPriorities(characterId);
            string[] parameters;

            if (expand != string.Empty)
            {
                parameters = expand.Split(",");
                foreach (string parameter in parameters)
                {
                    parameter.ToLower();
                    switch (parameter)
                    {
                        case "weapon":
                            weaponPriorities = await GetWeaponPriorities(characterId, "weapon");
                            break;
                        case "artifactset":
                            artifactPriorities = await GetArtifactPriorities(characterId, "artifactset");
                            break;
                    }
                }
            }

            foreach (WeaponPriority weaponPriority in weaponPriorities)
            {
                bool found = false;

                foreach (Role role in roles)
                {
                    if (role.Name.Equals(weaponPriority.CharacterRole))
                    {
                        role.Weapons.Add(weaponPriority);

                        found = true;
                    }
                }

                if (!found)
                {
                    Role newRole = new Role();
                    newRole.Name = weaponPriority.CharacterRole;
                    newRole.Weapons.Add(weaponPriority);

                    roles.Add(newRole);
                }
            }

            foreach (ArtifactPriority artifactPriority in artifactPriorities)
            {
                bool found = false;

                foreach (Role role in roles)
                {
                    if (role.Name.Equals(artifactPriority.CharacterRole))
                    {
                        role.Artifacts.Add(artifactPriority);

                        found = true;
                    }
                }

                if (!found)
                {
                    Role newRole = new Role();
                    newRole.Name = artifactPriority.CharacterRole;
                    newRole.Artifacts.Add(artifactPriority);

                    roles.Add(newRole);
                }
            }

            foreach (MainStatPriority mainStatPriority in mainStatPriorities)
            {
                bool found = false;

                foreach (Role role in roles)
                {
                    if (role.Name.Equals(mainStatPriority.CharacterRole))
                    {
                        role.MainStats.Add(mainStatPriority);

                        found = true;
                    }
                }

                if (!found)
                {
                    Role newRole = new Role();
                    newRole.Name = mainStatPriority.CharacterRole;
                    newRole.MainStats.Add(mainStatPriority);

                    roles.Add(newRole);
                }
            }

            foreach (SubStatPriority subStatPriority in subStatPriorities)
            {
                bool found = false;

                foreach (Role role in roles)
                {
                    if (role.Name.Equals(subStatPriority.CharacterRole))
                    {
                        role.SubStats.Add(subStatPriority);

                        found = true;
                    }
                }

                if (!found)
                {
                    Role newRole = new Role();
                    newRole.Name = subStatPriority.CharacterRole;
                    newRole.SubStats.Add(subStatPriority);

                    roles.Add(newRole);
                }
            }

            foreach (Role role in roles)
                role.Notes = await GetRoleNotes(characterId, role.Name);

            return roles;
        }

        public async Task<string> GetRoleNotes(string characterId, string name)
        {
            string command = "SELECT notes FROM ImpactDB.RoleNotesTable WHERE name=@name AND character_id=@character_id";

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (MySqlCommand sqlCommand = new MySqlCommand(command, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@name", name);
                    sqlCommand.Parameters.AddWithValue("@character_id", characterId);

                    using (DbDataReader sqlReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (sqlReader.Read())
                            return sqlReader.GetString("notes");
                    }
                }
            }

            return string.Empty;
        }
        #endregion

        #region Character Operations
        public async Task<List<Character>> GetAllCharacters(string expand)
        {
            List<Character> characters = new List<Character>();
            string[] parameters;
            string command = "SELECT * FROM ImpactDB.CharacterTable";

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(command, sqlConnection))
                {
                    DataTable table = new DataTable();
                    sqlAdapter.Fill(table);

                    foreach (DataRow row in table.Rows)
                    {
                        Character character = ReadCharacter(row);

                        if (expand != string.Empty)
                        {
                            parameters = expand.Split(",");

                            foreach (string parameter in parameters) 
                            {
                                parameter.ToLower();
                                switch (parameter) 
                                {
                                    case "talents":
                                        character.Talents = await GetTalents(character.Id);
                                        break;
                                    case "constellations":
                                        character.Constellations = await GetConstellations(character.Id);
                                        break;
                                    case "overview":
                                        character.CharacterOverview = await GetCharacterOverview(character.Id);
                                        break;
                                    case "characteroverview":
                                        character.CharacterOverview = await GetCharacterOverview(character.Id);
                                        break;
                                    case "roles":
                                        character.Roles = await GetCharacterRoles(character.Id, string.Empty);
                                        break;
                                }
                            }
                        } 
                
                        characters.Add(character);
                    }
                }
            }

            return characters;
        }

        public async Task<Character> GetCharacter(string characterId, string expand)
        {
            Character character;
            string[] parameters;
            string command = "SELECT * FROM ImpactDB.CharacterTable WHERE (id = @id)";
            
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString)) 
            {
                await sqlConnection.OpenAsync();

                using (MySqlCommand sqlCommand = new MySqlCommand(command, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", characterId);

                    using (DbDataReader sqlReader = await sqlCommand.ExecuteReaderAsync())
                        if (sqlReader.Read())
                            character = ReadCharacter(sqlReader);
                        else
                            return null;
                }
            }

            if (expand != string.Empty)
            {
                parameters = expand.Split(",");

                foreach (string parameter in parameters) 
                {
                    parameter.ToLower();
                    switch (parameter) 
                    {
                        case "talents":
                            character.Talents = await GetTalents(characterId);
                            break;
                        case "constellations":
                            character.Constellations = await GetConstellations(characterId);
                            break;
                        case "overview":
                            character.CharacterOverview = await GetCharacterOverview(characterId);
                            break;
                        case "roles":
                            character.Roles = await GetCharacterRoles(characterId, string.Empty);
                            break;
                    }
                }
            }

            return character;
        }

        private Character ReadCharacter(DataRow row)
        {
            Character character = new Character();
            
            character.Id = row["id"].ToString();
            character.Name = row["name"].ToString();
            character.Rarity = Convert.ToInt32(row["rarity"]);
            character.Weapon = row["weapon"].ToString();
            character.Element = row["element"].ToString();
            character.Region = row["region"].ToString();
            character.Description = row["description"].ToString();
            character.Image = row["image"].ToString();
            character.Quote = row["quote"].ToString();
            character.Title = row["title"].ToString();
            character.Faction = row["faction"].ToString();
            character.Birthday = row["birthday"].ToString();
            character.Constellation = row["constellation"].ToString();
            character.ChineseVA = row["chinese_va"].ToString();
            character.JapaneseVA = row["japanese_va"].ToString();
            character.EnglishVA = row["english_va"].ToString();
            character.KoreanVA = row["korean_va"].ToString();
            character.Tier = row["tier"].ToString();
            character.SquareCard = row["square_card"].ToString();
            character.Icon = row["icon"].ToString();

            return character;
        }

        private Character ReadCharacter(DbDataReader sqlReader)
        {
            Character character = new Character();

            character.Id = sqlReader["id"].ToString();
            character.Name = sqlReader["name"].ToString();
            character.Rarity = sqlReader.GetInt32("rarity");
            character.Weapon = sqlReader["weapon"].ToString();
            character.Element = sqlReader["element"].ToString();
            character.Region = sqlReader["region"].ToString();
            character.Description = sqlReader["description"].ToString();
            character.Image = sqlReader["image"].ToString();
            character.Quote = sqlReader["quote"].ToString();
            character.Title = sqlReader["title"].ToString();
            character.Faction = sqlReader["faction"].ToString();
            character.Birthday = sqlReader["birthday"].ToString();
            character.Constellation = sqlReader["constellation"].ToString();
            character.ChineseVA = sqlReader["chinese_va"].ToString();
            character.JapaneseVA = sqlReader["japanese_va"].ToString();
            character.EnglishVA = sqlReader["english_va"].ToString();
            character.KoreanVA = sqlReader["korean_va"].ToString();
            character.Tier = sqlReader["tier"].ToString();
            character.SquareCard = sqlReader["square_card"].ToString();
            character.Icon = sqlReader["icon"].ToString();

            return character;
        }
        #endregion

        #region Weapon Operations
        private Weapon ReadWeapon(DbDataReader sqlReader)
        {
            Weapon weapon = new Weapon();

            weapon.Id = sqlReader["id"].ToString();
            weapon.Name = sqlReader["name"].ToString();
            weapon.Type = sqlReader["type"].ToString();
            weapon.SubStatType = sqlReader["sub_stat_type"].ToString();
            weapon.AbilityName = sqlReader["ability_name"].ToString();
            weapon.AbilityDescription = sqlReader["ability_description"].ToString();
            weapon.Description = sqlReader["description"].ToString();
            weapon.Lore = sqlReader["lore"].ToString();
            weapon.Image = sqlReader["image"].ToString();
            weapon.Location = sqlReader["location"].ToString();
            weapon.Rarity = sqlReader.GetInt32("rarity");
            weapon.SubStat = sqlReader.GetFloat("sub_stat");
            weapon.BaseAtk = sqlReader.GetInt32("base_atk");

            return weapon;
        }

        public async Task<List<Weapon>> GetAllWeapons(string expand)
        {
            List<Weapon> weapons = new List<Weapon>();
            string[] parameters;
            string command = "SELECT * FROM ImpactDB.WeaponTable";

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (MySqlCommand sqlCommand = new MySqlCommand(command, sqlConnection))
                    using (DbDataReader sqlReader = await sqlCommand.ExecuteReaderAsync())
                        while (sqlReader.Read())
                            weapons.Add(ReadWeapon(sqlReader));
            }

            if (expand != string.Empty)        
            {    
                parameters = expand.Split(",");

                foreach (string parameter in parameters) 
                {
                    parameter.ToLower();
                    switch (parameter)
                    {
                        case "stats":
                            foreach (Weapon weapon in weapons)
                                weapon.Stats = await GetWeaponStats(weapon.Id);
                            break;
                    }
                }
            }

            return weapons;
        }

        public async Task<Weapon> GetWeapon(string id, string expand)
        {
            Weapon weapon;
            string[] parameters;
            string command = "SELECT * FROM ImpactDB.WeaponTable WHERE (id = @id)";

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (MySqlCommand sqlCommand = new MySqlCommand(command, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", id);

                    using (DbDataReader sqlReader = await sqlCommand.ExecuteReaderAsync())
                        if (sqlReader.Read())
                            weapon = ReadWeapon(sqlReader);
                        else    
                            return null;
                }
            }


            if (expand != string.Empty)        
            {    
                parameters = expand.Split(",");

                foreach (string parameter in parameters) 
                {
                    parameter.ToLower();
                    switch (parameter)
                    {
                        case "stats":
                            weapon.Stats = await GetWeaponStats(weapon.Id);
                            break;
                    }
                }
            }

            return weapon;
        }
        #endregion

        #region ArtifactSet Operations
        private ArtifactSet ReadArtifactSet(DbDataReader sqlReader)
        {
            ArtifactSet artifactSet = new ArtifactSet();

            artifactSet.Id = sqlReader["ArtifactSetTable.id"].ToString();
            artifactSet.Name = sqlReader["ArtifactSetTable.name"].ToString();
            artifactSet.MaxRarity = sqlReader.GetInt32("ArtifactSetTable.max_rarity");
            artifactSet.TwoPieceBonus = sqlReader["ArtifactSetTable.two_piece_bonus"].ToString();
            artifactSet.FourPieceBonus = sqlReader["ArtifactSetTable.four_piece_bonus"].ToString();

            return artifactSet;
        }
        public async Task<List<ArtifactSet>> GetAllArtifactSets()
        {
            List<ArtifactSet> artifacts = new List<ArtifactSet>();
            string command = "SELECT ArtifactSetTable.id AS 'ArtifactSetTable.id', " +
                "ArtifactSetTable.name AS 'ArtifactSetTable.name', " +
                "ArtifactSetTable.max_rarity AS 'ArtifactSetTable.max_rarity', " +
                "ArtifactSetTable.two_piece_bonus AS 'ArtifactSetTable.two_piece_bonus', " +
                "ArtifactSetTable.four_piece_bonus AS 'ArtifactSetTable.four_piece_bonus' " + 
                "FROM ImpactDB.ArtifactSetTable";

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (MySqlCommand sqlCommand = new MySqlCommand(command, sqlConnection))
                using (DbDataReader sqlReader = await sqlCommand.ExecuteReaderAsync())
                    while (sqlReader.Read())
                        artifacts.Add(ReadArtifactSet(sqlReader));
            }

            return artifacts;
        }

        public async Task<ArtifactSet> GetArtifactSet(string id)
        {
            string command = "SELECT ArtifactSetTable.id AS 'ArtifactSetTable.id', " +
                "ArtifactSetTable.name AS 'ArtifactSetTable.name', " +
                "ArtifactSetTable.max_rarity AS 'ArtifactSetTable.max_rarity', " +
                "ArtifactSetTable.two_piece_bonus AS 'ArtifactSetTable.two_piece_bonus', " +
                "ArtifactSetTable.four_piece_bonus AS 'ArtifactSetTable.four_piece_bonus' " + 
                "FROM ImpactDB.ArtifactSetTable WHERE id = @id";

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (MySqlCommand sqlCommand = new MySqlCommand(command, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", id);

                    using (DbDataReader sqlReader = await sqlCommand.ExecuteReaderAsync())
                        if (await sqlReader.ReadAsync())
                            return ReadArtifactSet(sqlReader);
                }
            }

            return null;
        }
        #endregion

        #region Talent Operations
        public async Task<List<Talent>> GetTalents(string characterId)
        {
            List<Talent> talents = new List<Talent>();
            string command = "SELECT * FROM ImpactDB.TalentTable WHERE (character_id = @character_id)";

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (MySqlCommand sqlCommand = new MySqlCommand(command, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@character_id", characterId);

                    using (DbDataReader sqlReader = await sqlCommand.ExecuteReaderAsync())
                        while (await sqlReader.ReadAsync())
                            talents.Add(ReadTalent(sqlReader));
                }
            }

            return talents;
        }

        private Talent ReadTalent(DbDataReader sqlReader)
        {
            Talent talent = new Talent();

            talent.Id = sqlReader["id"].ToString();
            talent.Name = sqlReader["name"].ToString();
            talent.Description = sqlReader["description"].ToString();
            talent.Image = sqlReader["image"].ToString();
            talent.Type = sqlReader["type"].ToString();

            return talent;
        }
        #endregion

        #region Constellation Operations
        public async Task<List<Constellation>> GetConstellations(string characterId)
        {
            List<Constellation> constellations = new List<Constellation>();
            string command = "SELECT * FROM ImpactDB.ConstellationTable WHERE (character_id = @character_id) ORDER BY ConstellationTable.order ASC";

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (MySqlCommand sqlCommand = new MySqlCommand(command, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@character_id", characterId);

                    using (DbDataReader sqlReader = await sqlCommand.ExecuteReaderAsync())
                        while (await sqlReader.ReadAsync())
                            constellations.Add(ReadConstellation(sqlReader));
                }
            }

            return constellations;
        }

        private Constellation ReadConstellation(DbDataReader sqlReader)
        {
            Constellation constellation = new Constellation();

            constellation.Id = sqlReader["id"].ToString();
            constellation.Name = sqlReader["name"].ToString();
            constellation.Description = sqlReader["description"].ToString();
            constellation.Image = sqlReader["image"].ToString();
            constellation.Order = sqlReader.GetInt32("order");

            return constellation;
        }
        #endregion

        #region Region Operations
        public async Task<Region> GetRegion(string id)
        {
            string command = "SELECT * FROM ImpactDB.RegionTable WHERE id = @id";
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                
                using (MySqlCommand sqlCommand = new MySqlCommand(command, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", id);

                    using (DbDataReader sqlReader = await sqlCommand.ExecuteReaderAsync())
                        if (await sqlReader.ReadAsync())
                            return ReadRegion(sqlReader);
                }
            }

            return null;
        }

        private Region ReadRegion(DbDataReader sqlReader)
        {
            Region region = new Region();

            region.Id = sqlReader["id"].ToString();
            region.Name = sqlReader["name"].ToString();
            region.Element = sqlReader["element"].ToString();
            region.Archon = sqlReader["archon"].ToString();
            region.Faction = sqlReader["faction"].ToString();

            return region;
        }
        #endregion

        #region Faction Operations
        public async Task<Faction> GetFaction(string id)
        {
            string command = "SELECT * FROM ImpactDB.FactionTable WHERE id = @id";

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (MySqlCommand sqlCommand = new MySqlCommand(command, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", id);

                    using (DbDataReader sqlReader = await sqlCommand.ExecuteReaderAsync())
                        if (await sqlReader.ReadAsync())
                            return ReadFaction(sqlReader);
                }
            }

            return null;
        }

        private Faction ReadFaction(DbDataReader sqlReader)
        {
            Faction faction = new Faction();

            faction.Id = sqlReader["id"].ToString();
            faction.Name = sqlReader["name"].ToString();
            faction.Description = sqlReader["description"].ToString();

            return faction;
        }
        #endregion

        #region CharacterOverview Operations
        public async Task<CharacterOverview> GetCharacterOverview(string characterId)
        {
            string command = "SELECT * FROM ImpactDB.CharacterOverviewTable WHERE character_id = @character_id";

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (MySqlCommand sqlCommand = new MySqlCommand(command, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@character_id", characterId);

                    using (DbDataReader sqlReader = await sqlCommand.ExecuteReaderAsync())
                        if (await sqlReader.ReadAsync())
                            return ReadCharacterOverview(sqlReader);
                }
            }

            return null;
        }

        private CharacterOverview ReadCharacterOverview(DbDataReader sqlReader)
        {
            CharacterOverview characterOverview = new CharacterOverview();

            characterOverview.AbilityTips = sqlReader["ability_tips"].ToString();
            characterOverview.RecommendedRole = sqlReader["recommended_role"].ToString();

            return characterOverview;
        }
        #endregion

        #region ArtifactPriority Operations
        public async Task<List<ArtifactPriority>> GetArtifactPriorities(string characterId, string expand)
        {
            List<ArtifactPriority> artifactPriorities = new List<ArtifactPriority>();
            string[] parameters;
            string command = "SELECT * FROM ImpactDB.ArtifactPriorityTable WHERE character_id = @character_id ORDER BY ArtifactPriorityTable.rank ASC";

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (MySqlCommand sqlCommand = new MySqlCommand(command, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@character_id", characterId);

                    using (DbDataReader sqlReader = await sqlCommand.ExecuteReaderAsync())
                        while (await sqlReader.ReadAsync())
                            artifactPriorities.Add(ReadArtifactPriority(sqlReader));

                    if (expand != string.Empty)        
                    {    
                        parameters = expand.Split(",");

                        foreach (string parameter in parameters) 
                        {
                            parameter.ToLower();
                            switch (parameter)
                            {                                  
                                case "artifactset":
                                    foreach (ArtifactPriority artifactPriority in artifactPriorities)
                                        artifactPriority.ArtifactSet = await GetArtifactSet(artifactPriority.ArtifactSetId);
                                    break;
                                case "artifact":
                                    foreach (ArtifactPriority artifactPriority in artifactPriorities)
                                        artifactPriority.ArtifactSet = await GetArtifactSet(artifactPriority.ArtifactSetId);
                                    break;
                                case "details":
                                    foreach (ArtifactPriority artifactPriority in artifactPriorities)
                                        artifactPriority.ArtifactSet = await GetArtifactSet(artifactPriority.ArtifactSetId);
                                    break;
                                case "set":
                                    foreach (ArtifactPriority artifactPriority in artifactPriorities)
                                        artifactPriority.ArtifactSet = await GetArtifactSet(artifactPriority.ArtifactSetId);
                                    break;
                            }
                        }
                    }
                }
            }

            return artifactPriorities;
        }

        private ArtifactPriority ReadArtifactPriority(DbDataReader sqlReader)
        {
            ArtifactPriority artifactPriority = new ArtifactPriority();

            artifactPriority.Id = sqlReader.GetInt32("id");
            artifactPriority.Rank = sqlReader.GetInt32("rank");
            artifactPriority.CharacterId = sqlReader["character_id"].ToString();
            artifactPriority.CharacterRole = sqlReader["character_role"].ToString();
            artifactPriority.ArtifactSetId = sqlReader["artifact_set_id"].ToString();

            return artifactPriority;
        }
        #endregion

        #region WeaponPriority Operations
        public async Task<List<WeaponPriority>> GetWeaponPriorities(string characterId, string expand)
        {
            List<WeaponPriority> weaponPriorities = new List<WeaponPriority>();
            string[] parameters;
            string command = "SELECT * FROM ImpactDB.WeaponPriorityTable WHERE character_id = @character_id ORDER BY WeaponPriorityTable.rank ASC";

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (MySqlCommand sqlCommand = new MySqlCommand(command, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@character_id", characterId);

                    using (DbDataReader sqlReader = await sqlCommand.ExecuteReaderAsync())
                        while (await sqlReader.ReadAsync())
                            weaponPriorities.Add(ReadWeaponPriority(sqlReader));

                    if (expand != string.Empty)        
                    {    
                        parameters = expand.Split(",");

                        foreach (string parameter in parameters) 
                        {
                            parameter.ToLower();
                            switch (parameter)
                            {
                                case "weapon":
                                    foreach (WeaponPriority weaponPriority in weaponPriorities)
                                        weaponPriority.Weapon = await GetWeapon(weaponPriority.WeaponId, string.Empty);
                                    break;
                            }
                        }
                    }
                }
            }

            return weaponPriorities;
        }

        private WeaponPriority ReadWeaponPriority(DbDataReader sqlReader)
        {
            WeaponPriority weaponPriority = new WeaponPriority();

            weaponPriority.Id = sqlReader.GetInt32("id");
            weaponPriority.Rank = sqlReader.GetInt32("rank");
            weaponPriority.CharacterId = sqlReader["character_id"].ToString();
            weaponPriority.CharacterRole = sqlReader["character_role"].ToString();
            weaponPriority.WeaponId = sqlReader["weapon_id"].ToString();

            return weaponPriority;
        }
        #endregion

        #region MainStatPriority Operations
        public async Task<List<MainStatPriority>> GetMainStatPriorities(string characterId)
        {
            List<MainStatPriority> mainStatPriorities = new List<MainStatPriority>();
            string command = "SELECT * FROM ImpactDB.MainStatPriorityTable WHERE character_id = @character_id";

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (MySqlCommand sqlCommand = new MySqlCommand(command, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@character_id", characterId);

                    using (DbDataReader sqlReader = await sqlCommand.ExecuteReaderAsync())
                        while (await sqlReader.ReadAsync())
                            mainStatPriorities.Add(ReadMainStatPriority(sqlReader));
                }
            }

            return mainStatPriorities;
        }

        private MainStatPriority ReadMainStatPriority(DbDataReader sqlReader)
        {
            MainStatPriority mainStatPriority = new MainStatPriority();

            mainStatPriority.Id = sqlReader.GetInt32("id");
            mainStatPriority.ArtifactType = sqlReader["artifact_type"].ToString();
            mainStatPriority.CharacterId = sqlReader["character_id"].ToString();
            mainStatPriority.CharacterRole = sqlReader["character_role"].ToString();
            mainStatPriority.Type = sqlReader["type"].ToString();

            return mainStatPriority;
        }
        #endregion

        #region SubStatPriority Operations
        public async Task<List<SubStatPriority>> GetSubStatPriorities(string characterId)
        {
            List<SubStatPriority> subStatPriorities = new List<SubStatPriority>();
            string command = "SELECT * FROM ImpactDB.SubStatPriorityTable WHERE character_id = @character_id ORDER BY SubStatPriorityTable.rank ASC";

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (MySqlCommand sqlCommand = new MySqlCommand(command, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@character_id", characterId);

                    using (DbDataReader sqlReader = await sqlCommand.ExecuteReaderAsync())
                        while (await sqlReader.ReadAsync())
                            subStatPriorities.Add(ReadSubStatPriority(sqlReader));
                }
            }

            return subStatPriorities;
        }

        private SubStatPriority ReadSubStatPriority(DbDataReader sqlReader)
        {
            SubStatPriority subStatPriority = new SubStatPriority();

            subStatPriority.Id = sqlReader.GetInt32("id");
            subStatPriority.Rank = sqlReader.GetInt32("rank");
            subStatPriority.Type = sqlReader["type"].ToString();
            subStatPriority.CharacterId = sqlReader["character_id"].ToString();
            subStatPriority.CharacterRole = sqlReader["character_role"].ToString();

            return subStatPriority;
        }
        #endregion

        #region WeaponStat Operations
        public async Task<List<WeaponStat>> GetWeaponStats(string characterId)
        {
            List<WeaponStat> weaponStats = new List<WeaponStat>();
            string command = "SELECT * FROM ImpactDB.WeaponStatTable WHERE weapon_id = @weapon_id";

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (MySqlCommand sqlCommand = new MySqlCommand(command, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@weapon_id", characterId);

                    using (DbDataReader sqlReader = await sqlCommand.ExecuteReaderAsync())
                        while (await sqlReader.ReadAsync())
                            weaponStats.Add(ReadWeaponStat(sqlReader));
                }
            }

            return weaponStats;
        }

        private WeaponStat ReadWeaponStat(DbDataReader sqlReader)
        {
            WeaponStat weaponStat = new WeaponStat();

            weaponStat.Id = sqlReader.GetInt32("id");
            weaponStat.WeaponId = sqlReader["weapon_id"].ToString();
            weaponStat.Level = sqlReader["level"].ToString();
            weaponStat.BaseAtk = sqlReader.GetInt32("base_atk");
            weaponStat.SubStat = sqlReader.GetFloat("sub_stat");

            return weaponStat;
        }
        #endregion
    }
}
