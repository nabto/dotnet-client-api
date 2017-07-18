using System;
using System.ComponentModel;

namespace NabtoTunnelManager
{
	[Serializable]
	public class TunnelConfiguration
	{
		[Description("A user friendly name for the tunnel.")]
		public string Name { get; set; }
		[Description("Put comments, a description or whatever you feel like here.")]
		public string Comments { get; set; }
		[Description("This is the id of the server to use for this tunnel.")]
		public string Server { get; set; }
		[Description("The username to use when connecting to the server.")]
		public string Email { get; set; }
		[Description("The password to use when connecting to the server.")]
		public string Password { get; set; }
		[Description("The local endpoint to associate with this tunnel.")]
		public string LocalEndpoint { get; set; }
		[Description("The remote endpoint to associate with this tunnel.")]
		public string RemoteEndpoint { get; set; }

		public TunnelConfiguration()
		{
			Name = "Nabto stream demo";
			Comments = "Tunnel to the stream demo server.";
			Server = "streamdemo.nabto.net";
			Email = "guest";
			Password = "";
			LocalEndpoint = "127.0.0.1:8080";
			RemoteEndpoint = "127.0.0.1:80";
		}

		public TunnelConfiguration(TunnelConfiguration tunnelConfiguration)
		{
			Name = tunnelConfiguration.Name;
			Comments = tunnelConfiguration.Comments;
			Server = tunnelConfiguration.Server;
			Email = tunnelConfiguration.Email;
			Password = tunnelConfiguration.Password;
			LocalEndpoint = tunnelConfiguration.LocalEndpoint;
			RemoteEndpoint = tunnelConfiguration.RemoteEndpoint;
		}
	}
}
