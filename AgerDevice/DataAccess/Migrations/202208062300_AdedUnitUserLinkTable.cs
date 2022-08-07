using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208062300)]
    public class AddedUnitUserLinkTable : Migration
    {
        public override void Up()
        {
            Create.Table("UnitUserLink")
                .WithColumn("UnitId").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("UnitUserLink");
        }
    }
}