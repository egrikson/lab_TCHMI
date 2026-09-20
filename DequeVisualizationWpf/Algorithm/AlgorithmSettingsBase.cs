namespace DequeVisualizationWpf
{
    public abstract class AlgorithmSettingsBase
    {
        public string AlgorithmName { get; set; } = "";
        public string Description { get; set; } = "";
        public ICollection<int> Collection { get; set; } = new List<int>();
    }
}
