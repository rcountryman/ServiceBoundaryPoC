using System;

namespace Common.Database
{
	public enum MongoConnectionType
	{
		Command = 1,
		Query = 2
	}

	public class UsernamePassword
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}

	public class MongoSettings
	{
		public UsernamePassword Command { get; set; }
		public UsernamePassword Query { get; set; }
		public string Server { get; set; }
		public string Database { get; set; }
		public string Options { get; set; }
		public bool UseSrvRecord { get; set; } = true;

		public string ToUrl(MongoConnectionType connectionType) =>
			$"mongodb{(UseSrvRecord ? "+srv" : string.Empty)}://{(connectionType == MongoConnectionType.Command ? $"{Command.Username}:{Command.Password}" : $"{Query.Username}:{Query.Password}")}@{Server ?? throw new ArgumentNullException(nameof(Server))}/{Database ?? throw new ArgumentNullException(nameof(Database))}{(Options == null ? string.Empty : $"?{Options}")}";
	}
}
