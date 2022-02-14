# Tags Layer Generator
Is a custom `Editor` class-file generator based-on Unity Tags, Layer and SortingLayers built-it engine enums. Generated classes provide at runtime fast access to actual Tag/Layers/SortingLayers variables.

# How to use:
- Go to `Editor Preferences` : Tags Layers Generator tab.
- Setup path for classes.
- Namespace if need.
- Toggle checkbox for necessary file/s.
- When you change something in `Editor (Tags/Layers)` don't forget to update classes.(run generator again).
- Enjoy.

# Details of constants:
#### Tags:
class contains: `string` tag names.
#### Layers:
class contains: `string` layer name, `int` mask, `int` number.
#### SortingLayers:
class contains: `string` layer names and `int` layer id.