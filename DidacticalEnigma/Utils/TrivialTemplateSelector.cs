﻿using System.Windows;
using System.Windows.Controls;

namespace DidacticalEnigma.Utils
{
    public class TrivialTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Default { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return Default;
        }
    }
}
