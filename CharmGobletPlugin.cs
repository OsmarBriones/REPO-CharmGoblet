using BepInEx;
using CharmCore.Api;
using HarmonyLib;
using UnityEngine;

namespace CharmGoblet
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("com.osmar.Pockets", BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency("com.osmar.CharmCore", BepInDependency.DependencyFlags.HardDependency)]
	public class CharmGobletPlugin : BaseUnityPlugin
	{
		public const string PluginGuid = "com.osmar.CharmGoblet";
		public const string PluginName = "CharmGoblet";
		public const string PluginVersion = "1.0.0";

		internal static new BepInEx.Logging.ManualLogSource Logger { get; private set; } = null!;

		/// <summary>The registered charm, for the Harmony patch.</summary>
		internal static GobletCharm Charm { get; private set; } = null!;

		private void Awake()
		{
			Logger = base.Logger;

			// Prevent the plugin from being deleted
			this.gameObject.transform.parent = null;
			this.gameObject.hideFlags = HideFlags.HideAndDontSave;

			// Registering binds Enabled, WearPercent, the charm's own settings and the Debug section (DebugMode) in this mod's config file.
			Charm = new GobletCharm();
			CharmsApi.Register(Charm, Config);

			new Harmony(PluginGuid).PatchAll();

			Logger.LogInfo($"{PluginName} {PluginVersion} loaded!");
		}
	}
}
