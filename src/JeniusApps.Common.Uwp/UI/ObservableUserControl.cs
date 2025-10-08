using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

#nullable enable

namespace JeniusApps.Common.UI;

/// <summary>
/// A user control that also implements <see cref="INotifyPropertyChanged"/>.
/// </summary>
public abstract class ObservableUserControl : UserControl, INotifyPropertyChanged
{
    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Sets the given value for the dependency property and raises a PropertyChanged event.
    /// </summary>
    /// <param name="property">The dependency property.</param>
    /// <param name="value">The new value.</param>
    /// <param name="p">The caller name.</param>
    protected void SetValueDp(DependencyProperty property, object value,
        [System.Runtime.CompilerServices.CallerMemberName] string? p = null)
    {
        SetValue(property, value);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
    }

    protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? p = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
    }
}