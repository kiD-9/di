using System;
using FractalPainting.App.Fractals;
using FractalPainting.Infrastructure.Common;
using FractalPainting.Infrastructure.Factories;
using FractalPainting.Infrastructure.UiActions;

namespace FractalPainting.App.Actions
{
    public class DragonFractalAction : IUiAction
    {
        private IImageHolder imageHolder;
        private readonly IDragonPainterFactory dragonPainterFactory;
        private readonly Func<Random, DragonSettingsGenerator> dragonSettingsFactory;

        public DragonFractalAction(IImageHolder imageHolder, IDragonPainterFactory dragonPainterFactory, Func<Random, DragonSettingsGenerator> dragonSettingsFactory)
        {
            this.imageHolder = imageHolder;
            this.dragonPainterFactory = dragonPainterFactory;
            this.dragonSettingsFactory = dragonSettingsFactory;
        }

        public string Category => "Фракталы";
        public string Name => "Дракон";
        public string Description => "Дракон Хартера-Хейтуэя";

        public void Perform()
        {
            var dragonSettings = CreateRandomSettings();
            var dragonPainter = dragonPainterFactory.CreatePainter(dragonSettings);
            
            SettingsForm.For(dragonSettings).ShowDialog();
            dragonPainter.Paint();

        }

        private DragonSettings CreateRandomSettings()
        {
            return dragonSettingsFactory(new Random()).Generate();
        }
    }
}