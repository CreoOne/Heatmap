using Heatmap.Primitives;

namespace Heatmap.Receivers
{
    public interface IReceiver
    {
        void Receive(Fragment fragment);
    }
}
