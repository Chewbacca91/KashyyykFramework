using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KashyyykFramework.Mobile.ComponentModel
{
    /// <summary>
    /// Classe de commande générique implémentant ICommand
    /// </summary>
    public class CommandHandler : ICommand
    {
        /// <summary>
        /// Action à exécuter par la commande
        /// </summary>
        private Action<object> execute;
        /// <summary>
        /// Prédicat pour Can Execute
        /// </summary>
        private Predicate<object> canExecute;
        /// <summary>
        /// Evenement indiquant un changement de valeur de CanExecute
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Constructeur spécifiant l'action
        /// CanExecute par défaut
        /// </summary>
        /// <param name="execute">
        /// Action à executer par la commande
        /// </param>
        public CommandHandler(Action<object> execute)
            : this(execute, CommandHandler.DefaultCanExecute)
        {
            //
        }

        /// <summary>
        /// Constructeur spécifiant l'action et le prédicat pour CanExecute
        /// </summary>
        /// <param name="execute">Action à executer par la commande</param>
        /// <param name="canExecute"></param>
        public CommandHandler(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null) { throw new ArgumentNullException("execute"); }
            if (canExecute == null) { throw new ArgumentNullException("canExecute"); }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Donne une valeur par défaut de CanExecute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }

        #region Implémentation ICommand

        public bool CanExecute(object parameter)
        {
            return this.canExecute != null && this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null) this.CanExecuteChanged(this, EventArgs.Empty);
        }

        #endregion
    }
}
