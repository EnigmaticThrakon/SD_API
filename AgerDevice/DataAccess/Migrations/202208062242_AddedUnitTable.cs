using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208062242)]
    public class AddedUnitTable : Migration
    {
        public override void Up()
        {
            Create.Table("Units")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Units");
        }
    }
}