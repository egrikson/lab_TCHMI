namespace DequeVisualizationWpf
{
    public interface IAlgorithm
    {
        void Run(AlgorithmSettingsBase settings);
        void SaveToFile(string path);
        void LoadFromFile(string path);
    }
}
