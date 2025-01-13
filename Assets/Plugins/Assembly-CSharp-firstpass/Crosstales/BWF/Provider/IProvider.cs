namespace Crosstales.BWF.Provider
{
	public interface IProvider
	{
		bool isReady { get; set; }

		void Load();

		void Save();
	}
}
