using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208062241)]
    public class AddedUserTable : Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Users");
        }
    }
}