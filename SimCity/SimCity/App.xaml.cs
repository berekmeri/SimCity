using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Threading;
using SimCity_Model.Model;
using SimCity.ViewModel;
using SimCity.View;

namespace SimCity
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private GameModel _model = null!;
        private SimCityViewModel _viewModel = null!;
        private MainWindow _view = null!;
        private DispatcherTimer _timer = null!;
        private bool _isGameOver;

        #endregion

        #region Constructors

        /// <summary>
        /// Alkalmazás példányosítása.
        /// </summary>

        App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }
        #endregion

        #region Application event handlers
        

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // modell létrehozása
            _model = new GameModel(true);
            _model.GameOver += new EventHandler<SimCityEventArgs>(Model_GameOver!);

            _model.NewGame();
            _isGameOver = false;

            // nézemodell létrehozása
            _viewModel = new SimCityViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.GameSpeedChanged += new EventHandler(ViewModel_GameSpeedChanged);
            _viewModel.GameStartedPaused += new EventHandler(ViewModel_PauseStartGame);

            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            //_view.Show();

            // időzítő létrehozása
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(333); // normál játéksebességről indulva
            _timer.Tick += new EventHandler(Timer_Tick);
            //_timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _model.AdvanceTime();
        }

        public void OpenGameWindow()
        {
            _view.Show();
            _timer.Start();
        }
        public void CloseGame()
        {
            _view.Close();
            _timer.Stop();
        }
        

        #endregion

        #region View event handlers

        /// <summary>
        /// Nézet bezárásának eseménykezelője.
        /// </summary>
        private void View_Closing(object? sender, CancelEventArgs e)
        {
            Boolean restartTimer = _timer.IsEnabled;

            _timer.Stop();

            if ( !_isGameOver && MessageBox.Show("Biztos, hogy ki akar lépni?", "SimCity", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true; // töröljük a bezárást

                if (restartTimer) // ha szükséges, elindítjuk az időzítőt
                    _timer.Start();
            }
        }

        #endregion

        #region ViewModel event handlers


        private void Model_GameOver(object sender, SimCityEventArgs e)
        {
            _isGameOver = true;
            _timer.Stop();
            MessageBox.Show("Game over!" + Environment.NewLine +
                            "You`ve managed to be the mayor of the city until: " +
                            e.GameTime.Item1 + "/" + e.GameTime.Item2 + "/" + e.GameTime.Item3 + ".",
                            "SimCity by Gentlemen",
                            MessageBoxButton.OK,
                            MessageBoxImage.Asterisk);
            _view.Close();

        }
        /// <summary>
        /// Új játék indításának eseménykezelője.
        /// </summary>
        private void ViewModel_NewGame(object? sender, EventArgs e)
        {
            _model.NewGame();
            _timer.Start();
        }

        /// <summary>
        /// Játékból való kilépés eseménykezelője.
        /// </summary>
        private void ViewModel_ExitGame(object? sender, System.EventArgs e)
        {
            _view.Close(); // ablak bezárása
        }

        private void ViewModel_GameSpeedChanged(object? sender, EventArgs e)
        {
            switch(_model.GameSpeed)
            {
                case GameSpeed.SLOW:
                    _timer.Interval = TimeSpan.FromMilliseconds(500); // egy hónap 15 másodperc, azaz 15/30 = 0.5 másodperc (500 millisecond) alatt telik el egy nap nap
                    break;
                case GameSpeed.NORMAL:
                    _timer.Interval = TimeSpan.FromMilliseconds(300); // egy hónap 10 másodperc, azaz 10/30 = 0.333 másodperc (333 millisecond) alatt telik el egy nap nap
                    break;
                case GameSpeed.FAST:
                    _timer.Interval = TimeSpan.FromMilliseconds(100); // egy hónap 5 másodperc, azaz 5/30 = 0.166 másodperc (166 millisecond) alatt telik el egy nap nap
                    break;
            }
        }
        private void ViewModel_PauseStartGame(object? sender, EventArgs e)
        {
            if (_timer.IsEnabled)
                _timer.Stop();
            else
            _timer.Start();
        }

        #endregion

        #region Model event handlers

        #endregion
    }
}
