// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelayCommand.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   A command whose sole purpose is to relay its functionality to other
//   objects by invoking delegates. The default return value for the CanExecute
//   method is 'true'.
//   This file originally from Josh Smith's MVVM Foundation open-source package, with changes
//   made for StyleCop compliance.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.MVVM
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;

    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other
    /// objects by invoking delegates. The default return value for the CanExecute
    /// method is 'true'.
    /// </summary>
    /// <typeparam name="T"> Type used for the executing action. </typeparam>
    public class RelayCommand<T> : ICommand
    {
        /// <summary> Action executed when the command is activated. </summary>
        private readonly Action<T> execute;

        /// <summary> Logic executed, determining whether the command can execute. </summary>
        private readonly Predicate<T> canExecute;

        /// <summary> Initializes a new instance of the <see cref="RelayCommand{T}"/> class. </summary>
        /// <param name="execute"> The action to execute. </param>
        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary> Initializes a new instance of the <see cref="RelayCommand{T}"/> class. Creates a new command. </summary>
        /// <param name="execute"> The execution logic. </param>
        /// <param name="canExecute"> The execution status logic. </param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary> Triggers when the CanExecute attached action has been changed. </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this.canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (this.canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary> Determines whether the attached command is able to function. If no CanExecute logic is specified, true is returned. </summary>
        /// <param name="parameter"> The parameter. </param>
        /// <returns> True if no canExecute logic is specified, otherwise the result of the canExecute action. </returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null ? true : this.canExecute((T)parameter);
        }

        /// <summary> Executes the specified command, using the optional parameter. </summary>
        /// <param name="parameter"> The parameter. </param>
        public void Execute(object parameter)
        {
            this.execute((T)parameter);
        }
    }

    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other
    /// objects by invoking delegates. The default return value for the CanExecute
    /// method is 'true'.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Reviewed. Suppression is OK here. Impossible to seperate implementation without keeping same file name.")]
    public class RelayCommand : ICommand
    {
        /// <summary> Action executed when the command is activated. </summary>
        private readonly Action execute;

        /// <summary> Logic executed, determining whether the command can execute. </summary>
        private readonly Func<bool> canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class. 
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute"> The execution logic. </param>
        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        /// <summary> Initializes a new instance of the <see cref="RelayCommand"/> class. Creates a new command. </summary>
        /// <param name="execute"> The execution logic. </param>
        /// <param name="canExecute"> The execution status logic. </param>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary> Triggers when the CanExecute attached action has been changed. </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this.canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (this.canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary> Returns whether the command can execute, if an action is specified. Otherwise, true is returned. </summary>
        /// <param name="parameter"> The parameter. </param>
        /// <returns> True if no canExecute logic is specified, otherwise returns the results of the canExecute action. </returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null ? true : this.canExecute();
        }

        /// <summary> Executes the specified action. </summary>
        /// <param name="parameter"> The parameter, which is not used. </param>
        public void Execute(object parameter)
        {
            this.execute();
        }
    }
}