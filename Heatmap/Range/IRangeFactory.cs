namespace Heatmap.Range
{
    public interface IRangeFactory
    {
        public IRange Create(float min, float max);
    }
}
