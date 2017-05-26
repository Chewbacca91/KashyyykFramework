using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Threading.Tasks;

namespace KashyyykFramework.Mobile.ComponentModel
{
    /// <summary>
    /// Permet de lancer une tâche asynchrone dont les résultats sont à DataBinder sur une UI
    /// Scénario chargement d'une application, chargement de données, etc ...
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public sealed class NotifyTaskCompletion<TResult> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Tâche à lancer
        /// </summary>
        public Task<TResult> Task { get; private set; }

        /// <summary>
        /// Résultat de la tâche
        /// </summary>
        public TResult Result
        {
            get
            {
                return (this.Task.Status == TaskStatus.RanToCompletion ? this.Task.Result : default(TResult));
            }
        }

        /// <summary>
        /// Status de la tâche
        /// </summary>
        public TaskStatus Status { get { return this.Task.Status; } }

        /// <summary>
        /// indique si la tâche est terminée
        /// </summary>
        public bool IsCompleted { get { return Task.IsCompleted; } }

        /// <summary>
        /// Indique si la tache est en cours
        /// </summary>
        public bool IsNotCompleted { get { return !this.Task.IsCompleted; } }

        /// <summary>
        /// Indique si la tâche s'est correctement déroulée
        /// </summary>
        public bool IsSuccessfullyCompleted
        {
            get
            {
                return (this.Task.Status == TaskStatus.RanToCompletion);
            }
        }

        /// <summary>
        /// Indique si la tâche a été annulée
        /// </summary>
        public bool IsCanceled { get { return this.Task.IsCanceled; } }

        /// <summary>
        /// Indique si la tâche est en erreur
        /// </summary>
        public bool IsFaulted { get { return Task.IsFaulted; } }

        /// <summary>
        /// Exception(s) survenue(s) durant l'exécution de la tâche
        /// </summary>
        public AggregateException Exception { get { return this.Task.Exception; } }

        /// <summary>
        /// Inner Exception
        /// </summary>
        public Exception InnerException
        {
            get
            {
                return (this.Exception == null ? null : this.Exception.InnerException);
            }
        }

        /// <summary>
        /// Message d'erreur
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return (InnerException == null ? null : InnerException.Message);
            }
        }

        /// <summary>
        /// Constructeur démarrant la tâche en asynchrone avec gestion INotifyChanged
        /// </summary>
        /// <param name="task">
        /// Tâche à exécuter
        /// </param>
        public NotifyTaskCompletion(Task<TResult> task)
        {
            this.Task = task;

            if (!task.IsCompleted)
            {
                Task runWatcher = WatchTaskAsync(task);
            }
        }

        /// <summary>
        /// Lancement de la tâche en asynchrone avec une gestion d'erreur "interne"
        /// </summary>
        /// <param name="task">
        /// Tache à exécuter
        /// </param>
        /// <returns>
        /// Task
        /// </returns>
        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch { }

            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged == null) return;

            propertyChanged(this, new PropertyChangedEventArgs("Status"));
            propertyChanged(this, new PropertyChangedEventArgs("IsCompleted"));
            propertyChanged(this, new PropertyChangedEventArgs("IsNotCompleted"));

            if (task.IsCanceled)
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsCanceled"));
            }
            else if (task.IsFaulted)
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsFaulted"));
                propertyChanged(this, new PropertyChangedEventArgs("Exception"));
                propertyChanged(this, new PropertyChangedEventArgs("InnerException"));
                propertyChanged(this, new PropertyChangedEventArgs("ErrorMessage"));
            }
            else
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
                propertyChanged(this, new PropertyChangedEventArgs("Result"));
            }
        }
    }
}
