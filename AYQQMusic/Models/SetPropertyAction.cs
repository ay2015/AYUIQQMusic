using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Interactivity;

namespace AYQQMusic
{
    public class SetPropertyAction : TriggerAction<FrameworkElement>
    {
        // PropertyName DependencyProperty.

        /// <summary>
        /// The property to be executed in response to the trigger.
        /// </summary>
        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        public static readonly DependencyProperty PropertyNameProperty
            = DependencyProperty.Register("PropertyName", typeof(string),
            typeof(SetPropertyAction));


        // PropertyValue DependencyProperty.

        /// <summary>
        /// The value to set the property to.
        /// </summary>
        public object PropertyValue
        {
            get { return GetValue(PropertyValueProperty); }
            set { SetValue(PropertyValueProperty, value); }
        }

        public static readonly DependencyProperty PropertyValueProperty
            = DependencyProperty.Register("PropertyValue", typeof(object),
            typeof(SetPropertyAction));


        // TargetObject DependencyProperty.

        /// <summary>
        /// Specifies the object upon which to set the property.
        /// </summary>
        public object TargetObject
        {
            get { return GetValue(TargetObjectProperty); }
            set { SetValue(TargetObjectProperty, value); }
        }

        public static readonly DependencyProperty TargetObjectProperty
            = DependencyProperty.Register("TargetObject", typeof(object),
            typeof(SetPropertyAction));


        // Private Implementation.

        protected override void Invoke(object parameter)
        {
            try
            {
                object target = TargetObject ?? AssociatedObject;
                PropertyInfo propertyInfo = target.GetType().GetProperty(
                    PropertyName,
                    BindingFlags.Instance | BindingFlags.Public
                    | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
                if (PropertyValue.ToString() == "true")
                {
                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(target, true, null);
                    }

                }
                else if (PropertyValue.ToString() == "false")
                {
                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(target, false, null);
                    }

                }
                else
                {
                    propertyInfo.SetValue(target, PropertyValue, null);
                }
            }
            catch 
            {

            }

        }
    }
}
