# WinForms
- it is not a rewrite like WPF or MAUI in .net 8
- it is a modernization evalution of classic winforms with better DPI handling, improved design support, MVVM fridendly binding and accessibility upgrades 
- while still remaining windows only

# Support control and limitations
- button, textbox, label, combox etc

1. WPF style command biding (ICommand Support)

- MVVM style architecture
- improved testability
- cleaner sepration of UI and business logic

2. Modern windows system icon API
- modern windows 10/11 stock icons
- automatic scaling
- no embedded .ico file required

3. removed / legacy controls
- DataGrid -> DataGridView
- MainMenu -> MenuStrip
- ContextMenu -> ContextMenuStrip
- ToolBar -> ToolStrip

4. platform limitation
- widnows only

5. No XAML support
- winforms does not use XAML.UI 