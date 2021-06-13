using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ImpactApi.Entities
{
    public partial class ImpactDbContext : IdentityDbContext<User>
    {
        public ImpactDbContext()
        {
        }

        public ImpactDbContext(DbContextOptions<ImpactDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Alchemy> Alchemys { get; set; }
        public virtual DbSet<ArtifactMainStat> ArtifactMainStats { get; set; }
        public virtual DbSet<ArtifactPriority> ArtifactPriorities { get; set; }
        public virtual DbSet<ArtifactSet> ArtifactSets { get; set; }
        public virtual DbSet<Artifact> Artifacts { get; set; }
        public virtual DbSet<CharacterOverview> CharacterOverviews { get; set; }
        public virtual DbSet<Character> Characters { get; set; }
        public virtual DbSet<Constellation> Constellations { get; set; }
        public virtual DbSet<Faction> Factions { get; set; }
        public virtual DbSet<Food> Foods { get; set; }
        public virtual DbSet<MainStatPriority> MainStatPriorities { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SubStatPriority> SubStatPriorities { get; set; }
        public virtual DbSet<Talent> Talents { get; set; }
        public virtual DbSet<WeaponPriority> WeaponPriorities { get; set; }
        public virtual DbSet<WeaponStat> WeaponStats { get; set; }
        public virtual DbSet<Weapon> Weapons { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("name=ConnectionStrings:ImpactDatabase", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.20-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");

            modelBuilder.Entity<Alchemy>(entity =>
            {
                entity.ToTable("AlchemyTable");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(700)
                    .HasColumnName("description");

                entity.Property(e => e.Effect)
                    .HasMaxLength(500)
                    .HasColumnName("effect");

                entity.Property(e => e.Image)
                    .HasMaxLength(500)
                    .HasColumnName("image");

                entity.Property(e => e.IngredientFour)
                    .HasMaxLength(45)
                    .HasColumnName("ingredient_four");

                entity.Property(e => e.IngredientOne)
                    .HasMaxLength(45)
                    .HasColumnName("ingredient_one");

                entity.Property(e => e.IngredientThree)
                    .HasMaxLength(45)
                    .HasColumnName("ingredient_three");

                entity.Property(e => e.IngredientTwo)
                    .HasMaxLength(45)
                    .HasColumnName("ingredient_two");

                entity.Property(e => e.Name)
                    .HasMaxLength(128)
                    .HasColumnName("name");

                entity.Property(e => e.Type)
                    .HasMaxLength(45)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<ArtifactMainStat>(entity =>
            {
                entity.ToTable("ArtifactMainStatTable");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Level)
                    .HasMaxLength(45)
                    .HasColumnName("level");

                entity.Property(e => e.Stat)
                    .HasMaxLength(45)
                    .HasColumnName("stat");

                entity.Property(e => e.Tier).HasColumnName("tier");

                entity.Property(e => e.Type)
                    .HasMaxLength(45)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<ArtifactPriority>(entity =>
            {
                entity.ToTable("ArtifactPriorityTable");

                entity.HasIndex(e => e.ArtifactSetId, "ArtifactSet_idx");

                entity.HasIndex(e => e.CharacterId, "Character-ArtifactPriority_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArtifactSetId).HasColumnName("artifact_set_id");

                entity.Property(e => e.CharacterId)
                    .IsRequired()
                    .HasColumnName("character_id");

                entity.Property(e => e.CharacterRole)
                    .HasMaxLength(255)
                    .HasColumnName("character_role");

                entity.Property(e => e.Rank).HasColumnName("rank");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.ArtifactSet)
                    .WithMany(p => p.ArtifactPriorities)
                    .HasForeignKey(d => d.ArtifactSetId)
                    .HasConstraintName("ArtifactSet-ArtifactPriority");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.ArtifactPriorities)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Character-ArtifactPriority");

            });

            modelBuilder.Entity<ArtifactSet>(entity =>
            {
                entity.ToTable("ArtifactSetTable");

                entity.Property(e => e.Id)
                    .HasMaxLength(45)
                    .HasColumnName("id");

                entity.Property(e => e.FourPieceBonus)
                    .HasMaxLength(500)
                    .HasColumnName("four_piece_bonus");

                entity.Property(e => e.MaxRarity).HasColumnName("max_rarity");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.TwoPieceBonus)
                    .HasMaxLength(500)
                    .HasColumnName("two_piece_bonus");
            });

            modelBuilder.Entity<Artifact>(entity =>
            {
                entity.ToTable("ArtifactTable");

                entity.HasIndex(e => e.ArtifactSetId, "Artifact-ArtifactSet_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArtifactSetId)
                    .HasMaxLength(45)
                    .HasColumnName("artifact_set_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .HasColumnName("description");

                entity.Property(e => e.Image)
                    .HasMaxLength(100)
                    .HasColumnName("image");

                entity.Property(e => e.Location)
                    .HasMaxLength(100)
                    .HasColumnName("location");

                entity.Property(e => e.Lore)
                    .HasMaxLength(2500)
                    .HasColumnName("lore");

                entity.Property(e => e.Name)
                    .HasMaxLength(128)
                    .HasColumnName("name");

                entity.Property(e => e.Rarity)
                    .HasColumnName("rarity")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Type)
                    .HasMaxLength(32)
                    .HasColumnName("type");

                entity.HasOne(d => d.ArtifactSet)
                    .WithMany(p => p.Artifacts)
                    .HasForeignKey(d => d.ArtifactSetId)
                    .HasConstraintName("Artifact-ArtifactSet");
            });

            modelBuilder.Entity<CharacterOverview>(entity =>
            {
                entity.HasKey(e => e.CharacterId)
                    .HasName("PRIMARY");

                entity.ToTable("CharacterOverviewTable");

                entity.Property(e => e.CharacterId)
                    .HasMaxLength(45)
                    .HasColumnName("character_id");

                entity.Property(e => e.AbilityTips)
                    .HasMaxLength(2500)
                    .HasColumnName("ability_tips");

                entity.Property(e => e.RecommendedRole)
                    .HasMaxLength(45)
                    .HasColumnName("recommended_role");           
            });

            modelBuilder.Entity<Character>(entity =>
            {
                entity.ToTable("CharacterTable");

                entity.HasIndex(e => e.Faction, "FactionFK_idx");

                entity.HasIndex(e => e.Region, "RegionFK_idx");

                entity.Property(e => e.Id)
                    .HasMaxLength(45)
                    .HasColumnName("id");

                entity.Property(e => e.Birthday)
                    .HasMaxLength(45)
                    .HasColumnName("birthday");

                entity.Property(e => e.ChineseVa)
                    .HasMaxLength(100)
                    .HasColumnName("chinese_va");

                entity.Property(e => e.Constellation)
                    .HasMaxLength(100)
                    .HasColumnName("constellation");

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .HasColumnName("description");

                entity.Property(e => e.Element)
                    .HasMaxLength(45)
                    .HasColumnName("element");

                entity.Property(e => e.EnglishVa)
                    .HasMaxLength(100)
                    .HasColumnName("english_va");

                entity.Property(e => e.Faction)
                    .HasMaxLength(45)
                    .HasColumnName("faction");

                entity.Property(e => e.Icon)
                    .HasMaxLength(100)
                    .HasColumnName("icon");

                entity.Property(e => e.Image)
                    .HasMaxLength(100)
                    .HasColumnName("image");

                entity.Property(e => e.JapaneseVa)
                    .HasMaxLength(100)
                    .HasColumnName("japanese_va");

                entity.Property(e => e.KoreanVa)
                    .HasMaxLength(100)
                    .HasColumnName("korean_va");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.Quote)
                    .HasMaxLength(100)
                    .HasColumnName("quote");

                entity.Property(e => e.Rarity)
                    .HasColumnName("rarity")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Region)
                    .HasMaxLength(45)
                    .HasColumnName("region");

                entity.Property(e => e.SquareCard)
                    .HasMaxLength(100)
                    .HasColumnName("square_card");

                entity.Property(e => e.Tier)
                    .HasMaxLength(8)
                    .HasColumnName("tier");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.Property(e => e.Weapon)
                    .HasMaxLength(45)
                    .HasColumnName("weapon");

                entity.HasOne<CharacterOverview>(d => d.CharacterOverview)
                    .WithOne(p => p.Character)
                    .HasForeignKey<CharacterOverview>(d => d.CharacterId);
            });

            modelBuilder.Entity<Constellation>(entity =>
            {
                entity.ToTable("ConstellationTable");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CharacterId)
                    .HasMaxLength(45)
                    .HasColumnName("character_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(1024)
                    .HasColumnName("description");

                entity.Property(e => e.Image)
                    .HasMaxLength(256)
                    .HasColumnName("image");

                entity.Property(e => e.Name)
                    .HasMaxLength(48)
                    .HasColumnName("name");

                entity.Property(e => e.Order).HasColumnName("order");

                entity.HasOne<Character>(d => d.Character)
                    .WithMany(p => p.Constellations)
                    .HasForeignKey(d => d.CharacterId)
                    .HasConstraintName("Character-Constellation");
            });

            modelBuilder.Entity<Faction>(entity =>
            {
                entity.ToTable("FactionTable");

                entity.Property(e => e.Id)
                    .HasMaxLength(45)
                    .HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Food>(entity =>
            {
                entity.ToTable("FoodTable");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CharacterName)
                    .HasMaxLength(45)
                    .HasColumnName("character_name");

                entity.Property(e => e.Effect)
                    .HasMaxLength(150)
                    .HasColumnName("effect");

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .HasColumnName("image");

                entity.Property(e => e.IngredientFour)
                    .HasMaxLength(45)
                    .HasColumnName("ingredient_four");

                entity.Property(e => e.IngredientOne)
                    .HasMaxLength(45)
                    .HasColumnName("ingredient_one");

                entity.Property(e => e.IngredientThree)
                    .HasMaxLength(45)
                    .HasColumnName("ingredient_three");

                entity.Property(e => e.IngredientTwo)
                    .HasMaxLength(45)
                    .HasColumnName("ingredient_two");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.Type)
                    .HasMaxLength(45)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<MainStatPriority>(entity =>
            {
                entity.ToTable("MainStatPriorityTable");

                entity.HasIndex(e => e.CharacterId, "Character-MainStatPriority_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArtifactType)
                    .HasMaxLength(32)
                    .HasColumnName("artifact_type");

                entity.Property(e => e.CharacterId).HasColumnName("character_id");

                entity.Property(e => e.CharacterRole)
                    .HasMaxLength(255)
                    .HasColumnName("character_role");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .HasColumnName("type");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.MainStatPriorities)
                    .HasForeignKey(d => d.CharacterId)
                    .HasConstraintName("Character-MainStatPriority");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("RegionTable");

                entity.Property(e => e.Id)
                    .HasMaxLength(45)
                    .HasColumnName("id");

                entity.Property(e => e.Archon)
                    .HasMaxLength(45)
                    .HasColumnName("archon");

                entity.Property(e => e.Element)
                    .HasMaxLength(45)
                    .HasColumnName("element");

                entity.Property(e => e.FactionId)
                    .HasMaxLength(45)
                    .HasColumnName("faction_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("RoleNotesTable");

                entity.HasIndex(e => e.CharacterId, "character-rolenotes_idx");

                entity.Property(e => e.Id)
                    .HasMaxLength(45)
                    .HasColumnName("id");

                entity.Property(e => e.CharacterId)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("character_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(3500)
                    .HasColumnName("notes");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("character-rolenotes");

            });

            modelBuilder.Entity<SubStatPriority>(entity =>
            {
                entity.ToTable("SubStatPriorityTable");

                entity.HasIndex(e => e.CharacterId, "character-substat_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CharacterId).HasColumnName("character_id");

                entity.Property(e => e.CharacterRole)
                    .HasMaxLength(255)
                    .HasColumnName("character_role");

                entity.Property(e => e.Rank).HasColumnName("rank");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .HasColumnName("type");
                
                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.SubStatPriorities)
                    .HasForeignKey(d => d.CharacterId)
                    .HasConstraintName("character-substat");
            });

            modelBuilder.Entity<Talent>(entity =>
            {
                entity.ToTable("TalentTable");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CharacterId)
                    .HasMaxLength(24)
                    .HasColumnName("character_id")
                    .HasComment("Corresponds to id from Characters");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .HasColumnName("description");

                entity.Property(e => e.Image)
                    .HasMaxLength(500)
                    .HasColumnName("image");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .HasColumnName("name");

                entity.Property(e => e.Type)
                    .HasMaxLength(24)
                    .HasColumnName("type");

                entity.HasOne<Character>(d => d.Character)
                    .WithMany(p => p.Talents)
                    .HasForeignKey(d => d.CharacterId)
                    .HasConstraintName("Character-Talent");
            });

            modelBuilder.Entity<WeaponPriority>(entity =>
            {
                entity.ToTable("WeaponPriorityTable");

                entity.HasIndex(e => e.CharacterId, "Character-WeaponPriority_idx");

                entity.HasIndex(e => e.WeaponId, "Weapon-WeaponPriority_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CharacterId).HasColumnName("character_id");

                entity.Property(e => e.CharacterRole)
                    .HasMaxLength(255)
                    .HasColumnName("character_role");

                entity.Property(e => e.Rank).HasColumnName("rank");

                entity.Property(e => e.WeaponId).HasColumnName("weapon_id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.WeaponPriorities)
                    .HasForeignKey(d => d.CharacterId)
                    .HasConstraintName("Character-WeaponPriority");

                entity.HasOne(d => d.Weapon)
                    .WithMany(p => p.WeaponPriorities)
                    .HasForeignKey(d => d.WeaponId)
                    .HasConstraintName("Weapon-WeaponPriority");
            });

            modelBuilder.Entity<WeaponStat>(entity =>
            {
                entity.ToTable("WeaponStatTable");

                entity.HasIndex(e => e.WeaponId, "Weapon-WeaponStat_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BaseAtk).HasColumnName("base_atk");

                entity.Property(e => e.Level)
                    .HasMaxLength(45)
                    .HasColumnName("level");

                entity.Property(e => e.SubStat)
                    .HasPrecision(4, 1)
                    .HasColumnName("sub_stat");

                entity.Property(e => e.WeaponId)
                    .HasMaxLength(45)
                    .HasColumnName("weapon_id");

                entity.HasOne(d => d.Weapon)
                    .WithMany(p => p.WeaponStats)
                    .HasForeignKey(d => d.WeaponId)
                    .HasConstraintName("Weapon-WeaponStat");
            });

            modelBuilder.Entity<Weapon>(entity =>
            {
                entity.ToTable("WeaponTable");

                entity.Property(e => e.Id)
                    .HasMaxLength(33)
                    .HasColumnName("id");

                entity.Property(e => e.AbilityDescription)
                    .HasMaxLength(837)
                    .HasColumnName("ability_description");

                entity.Property(e => e.AbilityName)
                    .HasMaxLength(31)
                    .HasColumnName("ability_name");

                entity.Property(e => e.BaseAtk).HasColumnName("base_atk");

                entity.Property(e => e.Description)
                    .HasMaxLength(2450)
                    .HasColumnName("description");

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .HasColumnName("image");

                entity.Property(e => e.Location)
                    .HasMaxLength(200)
                    .HasColumnName("location");

                entity.Property(e => e.Lore)
                    .HasMaxLength(3000)
                    .HasColumnName("lore");

                entity.Property(e => e.Name)
                    .HasMaxLength(33)
                    .HasColumnName("name");

                entity.Property(e => e.Rarity).HasColumnName("rarity");

                entity.Property(e => e.SubStat)
                    .HasPrecision(4, 1)
                    .HasColumnName("sub_stat");

                entity.Property(e => e.SubStatType)
                    .HasMaxLength(17)
                    .HasColumnName("sub_stat_type");

                entity.Property(e => e.Type)
                    .HasMaxLength(8)
                    .HasColumnName("type");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
