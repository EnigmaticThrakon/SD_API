using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208062300)]
    public class AddedUnitGroupLinkTable : Migration
    {
        public override void Up()
        {
            Create.Table("UnitGroupLink")
                .WithColumn("UnitId").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("GroupId").AsGuid().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("UnitGroupLink");
        }
    }
}