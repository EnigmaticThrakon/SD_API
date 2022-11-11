using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(2202211122004)]
    public class AddedEnvConfig : Migration
    {
        public override void Up()
        {
            Alter.Table("Units")
                .AddColumn("EnvironmentConfigurations").AsString(200).NotNullable().WithDefaultValue("{}");
        }

        public override void Down()
        {
            Delete.Column("EnvironmentConiguration").FromTable("Units");
        }
    }
}