using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace JeniusApps.Common.Models;

/// <summary>
/// Object that represents the menu items.
/// </summary>
public partial class MenuItem : ObservableObject
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="asyncRelayCommand">The command to trigger upon being selected.</param>
    /// <param name="text">The text to display on UI.</param>
    /// <param name="glyph">The glyph code to display on UI.</param>
    /// <param name="tag">A developer-facing tag to help programmaticaly identify this item.</param>
    /// <param name="tooltipText">Text to display in a tooltip.</param>
    /// <param name="tooltipSubtitle">Subtitle text to display in a tooltip.</param>
    /// <param name="automationName">The text to be used for the menu item's AutomationProperties.Name.</param>
    /// <param name="keyboardShortcut">An integer representing the keyboard shortcut.</param>
    /// <param name="keyboardShortcutModifier">An unsigned integer representing the keyboard shortcut modifier.</param>
    public MenuItem(
        IRelayCommand asyncRelayCommand,
        string text,
        string glyph,
        string? tag = null,
        string? tooltipText = null,
        string? tooltipSubtitle = null,
        string? automationName = null,
        int keyboardShortcut = 0,
        uint keyboardShortcutModifier = 0u)
    {
        ActionCommand = asyncRelayCommand;
        Text = text;
        Glyph = glyph;
        Tag = tag;
        AutomationName = automationName ?? text;
        ToolTipText = tooltipText ?? text;
        ToolTipSubtitle = tooltipSubtitle ?? string.Empty;
        KeyboardShortcutIndex = keyboardShortcut;
        KeyboardShortcutModifierIndex = keyboardShortcutModifier;
    }

    /// <summary>
    /// String used by developer to help identify this menu item
    /// programmatically. Not designed to be shown in UI.
    /// </summary>
    public string? Tag { get; }

    /// <summary>
    /// Determines if the item is selected.
    /// </summary>
    [ObservableProperty]
    private bool _isSelected;

    /// <summary>
    /// A badge count that can be displayed on the UI.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BadgeCountVisible))]
    private int _badgeCount;

    /// <summary>
    /// The text to be used for the menu item's AutomationProperties.Name.
    /// </summary>
    [ObservableProperty]
    private string _automationName = string.Empty;

    /// <summary>
    /// Determines if the badge count should be visible.
    /// </summary>
    public bool BadgeCountVisible => BadgeCount > 0;

    /// <summary>
    /// The command to trigger upon being selected.
    /// </summary>
    public IRelayCommand ActionCommand { get; }

    /// <summary>
    /// The menu's text.
    /// </summary>
    public string Text { get; } = string.Empty;

    /// <summary>
    /// The menu's icon.
    /// </summary>
    [ObservableProperty]
    private string _glyph = string.Empty;

    /// <summary>
    /// Text to display in a tooltip.
    /// </summary>
    public string ToolTipText { get; } = string.Empty;

    /// <summary>
    /// Subtitle text to display in a tooltip.
    /// </summary>
    public string ToolTipSubtitle { get; } = string.Empty;

    /// <summary>
    /// List of sub menu items.
    /// </summary>
    public ObservableCollection<MenuItem> SubMenuItems { get; } = [];

    /// <summary>
    /// Integer representing a keybardo shortcut index.
    /// Inteded for use with Windows.System.VirtualKey.
    /// </summary>
    public int KeyboardShortcutIndex { get; }

    /// <summary>
    /// Unsigned integer representing modifiers.
    /// Intended for use with Windows.System.VirtualKeyModifiers.
    /// </summary>
    public uint KeyboardShortcutModifierIndex { get; }
}
