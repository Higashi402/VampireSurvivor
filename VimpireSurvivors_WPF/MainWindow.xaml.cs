using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32.SafeHandles;
using MvcModel.Frames;
using MvcController;
using System.Text;
using MvcModel;

namespace VimpireSurvivors_WPF
{
    public partial class MainWindow : Window
    {
        private readonly KeyListener _keyListener;
        private RenderManager renderManager;

        public MainWindow()
        {
            GameWindow.GetInstance().Resize(1600, 900);
            InitializeComponent();

            FrameInitializer fi = new FrameInitializer();


            MainMenuFrame mainMenu = new MainMenuFrame();
            DialogFrameController mainMenuController = new DialogFrameController(new MainMenuFrame());

            _keyListener = new KeyListener(mainMenuController);
            _keyListener.StartKeyListener();

            renderManager = new RenderManager();
            renderManager.Controller = mainMenuController;
            renderManager.StartRender();
            this.Content = renderManager;

            Loaded += (sender, args) => renderManager.StartRender();

            Unloaded += (sender, args) => renderManager.StopRender();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            _keyListener.OnKeyDown(e);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            _keyListener.OnKeyUp(e);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            //GameWindow.GetInstance().Resize((int)(e.NewSize.Width), (int)(e.NewSize.Height));
        }
    }
}