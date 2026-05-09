# WPF UI Consolidation Plan

## Goal
Reduce duplicated XAML across views by moving shared visual primitives to centralized resources and introducing a small set of reusable controls/templates.

## Current Duplication Snapshot
The following patterns are repeated and should be consolidated first:

- `PrimaryButtonStyle` appears in:
  - `Authorization/AuthorizationUserControl.xaml`
  - `ArtistSearch/ArtistSearchView.xaml`
  - `SongList/SongListView.xaml`
  - `Playlist/Playlist.xaml`
  - `Complete/CompleteView.xaml`
- `SetlistButtonStyle` appears in:
  - `ArtistSearch/ArtistSearchView.xaml`
  - `Playlist/Playlist.xaml`
- Repeated "page shell" layout in most views:
  - Outer `Grid` with `Background="#F3F3F3"`
  - Centered `Border` card (`CornerRadius`, `Background="White"`, `Margin`, `Padding`)
  - Repeated `DropShadowEffect`
- Repeated list item card visuals:
  - `Background="White"`, border brush `#DDD`, corner radius, padding/margin
- Header text pattern repeated:
  - `Text="Spotify Setlist Creator"`, large/bold text styling

## Recommended Consolidation Approach
Use a layered approach:

1. Shared ResourceDictionaries for design tokens, common styles, and templates.
2. Reusable UserControls for larger repeated layout chunks (page shell, section header).
3. Keep view-specific behavior and bindings in individual views.

This keeps MVVM boundaries clean while reducing duplicated XAML.

## Target Structure
Create a `Styles` folder under `SetlistPlaylistCreator.Wpf`:

- `Styles/Colors.xaml`
- `Styles/Spacing.xaml`
- `Styles/Typography.xaml`
- `Styles/Buttons.xaml`
- `Styles/Cards.xaml`
- `Styles/ListView.xaml`
- `Styles/Templates.xaml` (optional, for DataTemplates/ControlTemplates)

Create a `Controls` folder for reusable shell components:

- `Controls/PageShell.xaml` (host card + outer background)
- `Controls/SectionHeader.xaml` (standard title/subtitle block, optional)

## Consolidation Phases

### Phase 1: Baseline and Safety
- Capture current UI screenshots for each view:
  - Authorization
  - ArtistSearch
  - SongList
  - Playlist
  - Complete
- Add/update UI smoke tests if available (or manual checklist).
- Define acceptance criteria:
  - No visual regressions beyond minor spacing adjustments.
  - No binding or command regressions.

### Phase 2: Introduce Global Resource Dictionaries
- Add dictionary files in `Styles`.
- Register dictionaries in `App.xaml` using `ResourceDictionary.MergedDictionaries`.
- Add explicit style keys first (do not switch to implicit styles yet) to avoid unintended global side effects.

Example direction in `App.xaml`:

```xml
<Application.Resources>
  <ResourceDictionary>
    <ResourceDictionary.MergedDictionaries>
      <ResourceDictionary Source="Styles/Colors.xaml" />
      <ResourceDictionary Source="Styles/Typography.xaml" />
      <ResourceDictionary Source="Styles/Buttons.xaml" />
      <ResourceDictionary Source="Styles/Cards.xaml" />
      <ResourceDictionary Source="Styles/ListView.xaml" />
    </ResourceDictionary.MergedDictionaries>
  </ResourceDictionary>
</Application.Resources>
```

### Phase 3: Consolidate Button and Card Styles
- Move `PrimaryButtonStyle` and `SetlistButtonStyle` into `Styles/Buttons.xaml`.
- Normalize style differences with variants:
  - `PrimaryButtonStyle`
  - `PrimaryButtonCompactStyle`
  - `SecondaryActionButtonStyle`
- Move recurring card visuals into `Styles/Cards.xaml`:
  - `PageCardStyle`
  - `ListItemCardStyle`
  - `InsetPanelCardStyle`

### Phase 4: Consolidate List and Item Container Styles
- Extract repeated `ListView` settings into styles in `Styles/ListView.xaml`:
  - Scrollbar behavior
  - `HorizontalContentAlignment`
  - `BorderThickness`
- Extract `ListViewItem` container style where repeated.
- For repeated item row patterns, move to shared `DataTemplate` resources when view models match.

### Phase 5: Introduce Reusable Shell Controls
- Create `Controls/PageShell.xaml` for repeated outer layout:
  - Gray page background
  - Centered white card
  - Corner radius/shadow
  - Configurable `Padding` and `Margin`
- Replace duplicated shell markup in each view with `PageShell` content slot.
- Keep per-view content inside the shell to preserve clear ownership.

### Phase 6: Clean Up and Standardize
- Remove all duplicated local styles from each view file.
- Keep only view-local resources that are truly unique.
- Ensure naming convention consistency:
  - Shared style keys use clear names and variants.
  - Avoid one-off style keys unless justified.

## Proposed Shared Keys (Initial)
- Colors/Brushes:
  - `Brush.PageBackground`
  - `Brush.CardBackground`
  - `Brush.CardBorder`
  - `Brush.TextPrimary`
  - `Brush.TextSecondary`
- Typography:
  - `TextBlock.PageTitleStyle`
  - `TextBlock.SectionTitleStyle`
  - `TextBlock.BodyStyle`
- Buttons:
  - `Button.PrimaryStyle`
  - `Button.CompactActionStyle`
- Cards:
  - `Border.PageCardStyle`
  - `Border.ListItemCardStyle`

## Best Practices and Guardrails
- Prefer `DynamicResource` for themeable colors and `StaticResource` for stable styles/templates.
- Avoid over-abstracting too early; only extract patterns used in at least 2 views.
- Keep converter resources (`InverseBooleanConverter`, `NullToVisibilityConverter`) where globally useful; otherwise scope locally.
- Preserve MVVM: no behavior logic in code-behind while consolidating styles.
- Keep design-time support (`d:DataContext`) intact in each view.

## Risk Areas
- Implicit global styles can unintentionally alter controls in all views.
- Shared templates can become too generic and harder to maintain.
- Minor spacing/alignment shifts are likely when standardizing.

Mitigation:
- Start with explicit keys.
- Migrate one view at a time and compare against baseline screenshots.
- Keep commits small and scoped by phase.

## Suggested Execution Order by View
1. `AuthorizationUserControl` (smallest, lowest risk)
2. `CompleteView`
3. `SongListView`
4. `ArtistSearchView`
5. `Playlist` (most complex)

## Definition of Done
- No duplicated button/card/list base styles remain in view-local resources.
- All common styles are sourced from shared dictionaries.
- Repeated shell layout is consolidated through a shared control or template.
- Existing navigation and command bindings behave the same.
- XAML remains readable and easier to maintain.

## Optional Future Enhancements
- Add light/dark theme dictionaries with runtime switching.
- Introduce typography and spacing scale tokens more formally.
- Add visual regression automation for core views.
