using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Contracts;

namespace EjesUI.Services
{
    public class SnackBarNotifierService
    {
        private readonly ISnackbarService _snackbarService;
        private int _snackbarTimeout = 2000;

        public SnackBarNotifierService(ISnackbarService snackbarService)
        {
            _snackbarService = snackbarService;
        }

        public void Show(string title, string message, int snackbarAppearance = 1)
        {
            ControlAppearance _snackbarAppearance = UpdateSnackbarAppearance(snackbarAppearance);
            _snackbarService.Timeout = _snackbarTimeout;
            _snackbarService.Show(title, message, SymbolRegular.Fluent24, _snackbarAppearance);
        }

        private ControlAppearance UpdateSnackbarAppearance(int appearanceIndex)
        {
            return appearanceIndex switch
            {
                1 => ControlAppearance.Secondary,
                2 => ControlAppearance.Info,
                3 => ControlAppearance.Success,
                4 => ControlAppearance.Caution,
                5 => ControlAppearance.Danger,
                6 => ControlAppearance.Light,
                7 => ControlAppearance.Dark,
                8 => ControlAppearance.Transparent,
                _ => ControlAppearance.Primary
            };
        }
    }
}
