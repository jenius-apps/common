using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace JeniusApps.Common.Models
{
    /// <summary>
    /// Object that represents the menu items.
    /// </summary>
    public class MenuItem : ObservableObject
    {
        private bool _isSelected;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="asyncRelayCommand">The command to trigger upon being selected.</param>
        /// <param name="text">The text to display on UI.</param>
        /// <param name="glyph">The glyph code to display on UI.</param>
        /// <param name="tag">A developer-facing tag to help programmaticaly identify this item.</param>
        /// <param name="tooltipText">Text to display in a tooltip.</param>
        /// <param name="tooltipSubtitle">Subtitle text to display in a tooltip.</param>
        public MenuItem(
            IRelayCommand asyncRelayCommand,
            string text,
            string glyph,
            string? tag = null,
            string? tooltipText = null,
            string? tooltipSubtitle = null)
        {
            ActionCommand = asyncRelayCommand;
            Text = text;
            Glyph = glyph;
            Tag = tag;
            ToolTipText = tooltipText ?? text;
            ToolTipSubtitle = tooltipSubtitle ?? string.Empty;
        }

        /// <summary>
        /// String used by developer to help identify this menu item
        /// programmatically. Not designed to be shown in UI.
        /// </summary>
        public string? Tag { get; }

        /// <summary>
        /// Determines if the item is selected.
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

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
        public string Glyph { get; } = string.Empty;

        /// <summary>
        /// Text to display in a tooltip.
        /// </summary>
        public string ToolTipText { get; } = string.Empty;

        /// <summary>
        /// Subtitle text to display in a tooltip.
        /// </summary>
        public string ToolTipSubtitle { get; } = string.Empty;
    }
}
