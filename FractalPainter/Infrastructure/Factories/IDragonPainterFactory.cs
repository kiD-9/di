using FractalPainting.App.Fractals;

namespace FractalPainting.Infrastructure.Factories
{
    public interface IDragonPainterFactory
    {
        public DragonPainter CreatePainter(DragonSettings settings);
    }
}