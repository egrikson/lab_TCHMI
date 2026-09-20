namespace DequeVisualizationWpf
{
    public class DequeAlgorithmSettings : AlgorithmSettingsBase
    {
        public LinkedList<int> Deque => (LinkedList<int>)Collection;

        public DequeAlgorithmSettings()
        {
            AlgorithmName = "Визуализация дека";
            Description = "Дек, реализованный на основе двусвязного списка.";
            Collection = new LinkedList<int>();
        }
    }
}
