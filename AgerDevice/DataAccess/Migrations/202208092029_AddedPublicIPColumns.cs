using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208092029)]
    public class AddedPublicIPColumns : Migration
    {
        public override void Up()
        {
            Create.Column("PublicIP").OnTable("Units").AsString(50).Nullable();
            Create.Column("PublicIP").OnTable("Users").AsString(50).Nullable();
        }

        public override void Down()
        {
            Delete.Column("PublicIP").FromTable("Units");
            Delete.Column("PublicIP").FromTable("Users");
        }
    }
}