// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyObserver.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Monitors the PropertyChanged event of an object that implements INotifyPropertyChanged,
//   and executes callback methods (i.e. handlers) registered for properties of that object.
//   This file originally from Josh Smith's MVVM Foundation open-source package, with changes
//   made for StyleCop compliance.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.MVVM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Windows;

    /// <summary>
    /// Monitors the PropertyChanged event of an object that implements INotifyPropertyChanged,
    /// and executes callback methods (i.e. handlers) registered for properties of that object.
    /// </summary>
    /// <typeparam name="TPropertySource">The type of object to monitor for property changes.</typeparam>
    public class PropertyObserver<TPropertySource> : IWeakEventListener
        where TPropertySource : class, INotifyPropertyChanged
    {
        /// <summary> Keeps references between a property name and the registered handlers. </summary>
        private readonly Dictionary<string, Action<TPropertySource>> propertyNameToHandlerMap;

        /// <summary> Weak reference to the property source. </summary>
        private readonly WeakReference propertySourceRef;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyObserver{TPropertySource}"/> class. 
        /// Observes the 'propertySource' object for property changes.
        /// </summary>
        /// <param name="propertySource">
        /// The object to monitor for property changes.
        /// </param>
        public PropertyObserver(TPropertySource propertySource)
        {
            if (propertySource == null)
            {
                throw new ArgumentNullException("propertySource");
            }

            this.propertySourceRef = new WeakReference(propertySource);
            this.propertyNameToHandlerMap = new Dictionary<string, Action<TPropertySource>>();
        }

        /// <summary>
        /// Registers a callback to be invoked when the PropertyChanged event has been raised for the specified property.
        /// </summary>
        /// <param name="expression">A lambda expression like 'n => n.PropertyName'.</param>
        /// <param name="handler">The callback to invoke when the property has changed.</param>
        /// <returns>The object on which this method was invoked, to allow for multiple invocations chained together.</returns>
        public PropertyObserver<TPropertySource> RegisterHandler(
            Expression<Func<TPropertySource, object>> expression,
            Action<TPropertySource> handler)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            string propertyName = GetPropertyName(expression);
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("'expression' did not provide a property name.");
            }

            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            TPropertySource propertySource = this.GetPropertySource();
            if (propertySource != null)
            {
                Debug.Assert(!this.propertyNameToHandlerMap.ContainsKey(propertyName), "Why is the '" + propertyName + "' property being registered again?");

                this.propertyNameToHandlerMap[propertyName] = handler;
                PropertyChangedEventManager.AddListener(propertySource, this, propertyName);
            }

            return this;
        }

        /// <summary> Removes the callback associated with the specified property. </summary>
        /// <param name="expression"> The expression. </param>
        /// <returns> The object on which this method was invoked, to allow for multiple invocations chained together. </returns>
        public PropertyObserver<TPropertySource> UnregisterHandler(Expression<Func<TPropertySource, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            string propertyName = GetPropertyName(expression);
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("'expression' did not provide a property name.");
            }

            var propertySource = this.GetPropertySource();
            if (propertySource != null)
            {
                if (this.propertyNameToHandlerMap.ContainsKey(propertyName))
                {
                    this.propertyNameToHandlerMap.Remove(propertyName);
                    PropertyChangedEventManager.RemoveListener(propertySource, this, propertyName);
                }
            }

            return this;
        }

        /// <summary> Receives a weak event and triggers all subscribed event handlers </summary>
        /// <param name="managerType"> The manager type. </param>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        /// <returns> Whether the event has been handled. </returns>
        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            bool handled = false;

            if (managerType == typeof(PropertyChangedEventManager))
            {
                var args = e as PropertyChangedEventArgs;
                if (args != null && sender is TPropertySource)
                {
                    string propertyName = args.PropertyName;
                    var propertySource = (TPropertySource)sender;

                    if (string.IsNullOrEmpty(propertyName))
                    {
                        // When the property name is empty, all properties are considered to be invalidated.
                        // Iterate over a copy of the list of handlers, in case a handler is registered by a callback.
                        foreach (Action<TPropertySource> handler in this.propertyNameToHandlerMap.Values.ToArray())
                        {
                            handler(propertySource);
                        }

                        handled = true;
                    }
                    else
                    {
                        Action<TPropertySource> handler;
                        if (this.propertyNameToHandlerMap.TryGetValue(propertyName, out handler))
                        {
                            handler(propertySource);

                            handled = true;
                        }
                    }
                }
            }

            return handled;
        }

        /// <summary> Returns the property name provided through a lambda. </summary>
        /// <param name="expression"> The expression. </param> 
        /// <returns> The property name, if found. Otherwise, null. </returns>
        private static string GetPropertyName(Expression<Func<TPropertySource, object>> expression)
        {
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression = null;

            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                
                if (unaryExpression != null)
                {
                    memberExpression = unaryExpression.Operand as MemberExpression;
                }
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            if (memberExpression != null)
            {
                var propertyInfo = memberExpression.Member as PropertyInfo;

                if (propertyInfo != null)
                {
                    return propertyInfo.Name;
                }
            }

            Debug.Assert(memberExpression != null, "Please provide a lambda expression like 'n => n.PropertyName'");

            return null;
        }

        /// <summary> Gets the source of a property. </summary>
        /// <returns> The property source. </returns>
        private TPropertySource GetPropertySource()
        {
            try
            {
                return (TPropertySource)this.propertySourceRef.Target;
            }
            catch 
            {
                return default(TPropertySource);
            }
        }
    }
}