namespace Migrator.Migrations;

[TimestampedMigration(2023, 1, 1, 0, 0)]
public sealed class InitialMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Items")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString().NotNullable();
    }
}