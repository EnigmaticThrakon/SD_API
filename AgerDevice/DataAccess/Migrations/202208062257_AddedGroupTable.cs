using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208062257)]
    public class AddedGroupTable : Migration
    {
        public override void Up()
        {
            Create.Table("Groups")
                .WithColumn("UserId").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("GroupId").AsGuid().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Groups");
        }
    }
}