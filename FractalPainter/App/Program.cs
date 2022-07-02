using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using FractalPainting.App.Actions;
using FractalPainting.App.Fractals;
using FractalPainting.Infrastructure.Common;
using FractalPainting.Infrastructure.Factories;
using FractalPainting.Infrastructure.UiActions;
using Ninject;
using Ninject.Extensions.Factory;
using Ninject.Extensions.Conventions;

namespace FractalPainting.App
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                var container = new StandardKernel();

                // start here
                container.Bind(x => x
                    .FromThisAssembly()
                    .SelectAllClasses()
                    .InheritedFrom<IUiAction>()
                    .BindAllInterfaces());
                
                container.Bind<IImageHolder, PictureBoxImageHolder>().To<PictureBoxImageHolder>().InSingletonScope();
                container.Bind<Palette>().ToSelf().InSingletonScope();
                container.Bind<KochPainter>().ToSelf();
                
                container.Bind<IDragonPainterFactory>().ToFactory();
                container.Bind<AppSettings, IImageDirectoryProvider>()
                    .ToMethod(context => context.Kernel.Get<SettingsManager>().Load())
                    .InSingletonScope();
                container.Bind<ImageSettings>()
                    .ToMethod(context => context.Kernel.Get<AppSettings>().ImageSettings)
                    .InSingletonScope();
                container.Bind<IObjectSerializer>().To<XmlObjectSerializer>()
                    .WhenInjectedInto<SettingsManager>();
                container.Bind<IBlobStorage>().To<FileBlobStorage>()
                    .WhenInjectedInto<SettingsManager>();

                var mainForm = container.Get<MainForm>();
                
                Application.Run(mainForm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}